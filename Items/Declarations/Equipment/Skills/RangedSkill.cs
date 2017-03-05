using System;
using System.Linq;
using System.Text;
using AoM;
using Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Newtonsoft.Json;
using Screens;
using Skills;
using Units;
using Units.Skills;

namespace Items.Declarations.Equipment.Skills
{

	/// <summary>
	/// Skill de ataque de rango de esta clase.
	/// </summary>
	public abstract class RangedSkill : ISkill
	{
		const string defaultInfoboxText = "Info box";

		float ISkill.Value //TODO
		{ get { return 100; } }

		/// <summary>
		/// Gets the unique name of the skill
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; }

		/// <summary>
		/// Devuelve la última instancia generada.
		/// </summary>
		/// <value>The last generated instance.</value>
		[JsonIgnore]
		public SkillInstance LastGeneratedInstance { get; protected set; }

		/// <summary>
		/// Builds the instance
		/// </summary>
		public abstract SkillInstance BuildSkillInstance (IUnidad user, IUnidad target);

		/// <summary>
		/// Build a skill instance
		/// </summary>
		/// <param name="user">Caster</param>
		public void GetInstance (IUnidad user)
		{
			var dialSer = new Moggle.Screens.Dials.ScreenDialSerial ();
			var grid = user.Grid;
			var selScr = new SelectTargetScreen (Program.MyGame, grid);
			selScr.GridSelector.CameraUnidad = user as Unidad;

			var visEnemies = grid.GetVisibleAliveUnidad (user).Where (z => z.Team != user.Team);

			// Put the cursor in the closest visible enemy (if any)
			var ppos = visEnemies.Any () ? 
				grid.GetClosestVisibleEnemy (user) : 
				user.Location;
			selScr.GridSelector.SetCursor (ppos, user);

			var infoBox = new EtiquetaMultiLínea (selScr)
			{
				MaxWidth = 200,
				TextColor = Color.White,
				TopLeft = new Point (500, 200),
				UseFont = "Fonts//small",
				Texto = defaultInfoboxText,
				BackgroundColor = Color.Black
			};
			selScr.AddComponent (infoBox);

			dialSer.AddRequest (selScr);
			selScr.GridSelector.CursorMoved += 
			 	(s, e) => updateInfoBox (selScr.GridSelector, user, infoBox);

			updateInfoBox (selScr.GridSelector, user, infoBox);
			dialSer.HayRespuesta += delegate(object sender, object [] e)
			{
				var pt = (Point)e [0];
				var tg = user.Grid [pt].GetAliveUnidadHere ();
				if (tg == null)
					return;
				LastGeneratedInstance = BuildSkillInstance (user, tg);
				LastGeneratedInstance.Effects.Executed += delegate(object sender2,
				                                                   EffectResultEnum efRes)
				{
					switch (efRes)
					{
						case EffectResultEnum.Hit:
							OnHit (efRes, user, tg);
							break;
						case EffectResultEnum.Miss:
							OnMiss (efRes, user, tg);
							break;
						default:
							throw new Exception ();
					}
				};

				LastGeneratedInstance.Execute ();
			};

			selScr.HayRespuesta += (sender, e) => Executed?.Invoke (
				this,
				EventArgs.Empty);
			dialSer.Executar (Program.MyGame.ScreenManager.ActiveThread);	
		}

		protected abstract void OnHit (EffectResultEnum result, IUnidad user, IUnidad target);

		protected abstract void OnMiss (EffectResultEnum result, IUnidad user, IUnidad target);

		void updateInfoBox (SelectableGridControl selGrid,
		                    IUnidad user,
		                    EtiquetaMultiLínea infoBox)
		{
			// Se cambió la selección; volver a calcular skill
			var pt = selGrid.CursorPosition;
			var tg = user.Grid [pt].GetAliveUnidadHere ();
			if (tg == null)
			{
				infoBox.Texto = defaultInfoboxText;
				return;
			}
			var skInst = BuildSkillInstance (user, tg);
			var infoStrBuilding = new StringBuilder ();
			foreach (var eff in skInst.Effects)
				infoStrBuilding.AppendLine (" * " + eff.DetailedInfo ());

			infoBox.Texto = infoStrBuilding.ToString ();
		}

		/// <summary>
		/// Determines whether this skill is castable by the specified user.
		/// </summary>
		/// <param name="user">User</param>
		public virtual bool IsCastable (IUnidad user)
		{
			return true;
		}

		/// <summary>
		/// Determines whether this instance is visible the specified user.
		/// </summary>
		/// <param name="user">User.</param>
		public virtual bool IsVisible (IUnidad user)
		{
			return true;
		}

		/// <summary>
		/// Determines whether this skill can be learned
		/// </summary>
		protected bool IsLearnable;

		/// <summary>
		/// If <see cref="IsLearnable"/>, this contains the required skills before learning this one.
		/// </summary>
		protected string [] RequiredSkills;

		bool ISkill.IsLearnable { get { return IsLearnable; } }

		string[] ISkill.RequiredSkills { get { return RequiredSkills; } }

		/// <summary>
		/// Occurs when the eexecution finishes completly
		/// </summary>
		public event EventHandler Executed;

		#region Contenido

		/// <summary>
		/// Gets or sets the name of the texture.
		/// </summary>
		/// <value>The name of the texture.</value>
		public string TextureName { get; set; }

		/// <summary>
		/// Gets the icon.
		/// </summary>
		/// <value>The icon.</value>
		[JsonIgnore]
		protected Texture2D Icon { get; private set; }

		/// <summary>
		/// Gets the name of the icon texture
		/// </summary>
		protected string IconName { get; }

		void IComponent.LoadContent (ContentManager manager)
		{
			Icon = manager.Load<Texture2D> (IconName);
		}

		#endregion

		void IGameComponent.Initialize ()
		{
		}

		void IDibujable.Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (Icon, destinationRectangle: rect, layerDepth: Depths.SkillIcon);
		}

		protected RangedSkill (string Name, string TextureName, string Icon)
		{
			this.Name = Name;
			this.TextureName = TextureName;
			IconName = Icon;
		}
	}
}
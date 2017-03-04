using System;
using System.Linq;
using System.Text;
using AoM;
using Cells;
using Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Newtonsoft.Json;
using Screens;
using Skills;
using Units;
using Units.Recursos;
using Units.Skills;

namespace Items.Declarations.Equipment.Skills
{
	/// <summary>
	/// Skill de ataque de rango de esta clase.
	/// </summary>
	public class RangedSkill : ISkill
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
		/// Gets the cooldown time for this skill
		/// </summary>
		public readonly float BaseCooldown;

		/// <summary>
		/// Builds the instance
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="target">Target.</param>
		public SkillInstance BuildSkillInstance (IUnidad user, IUnidad target)
		{
			var quiver = user.Equipment.EquipmentInSlot (EquipSlot.Quiver).OfType<Arrow> ();
			if (!quiver.Any ())
				throw new Exception ("Cannot invoke ranged skill without ammo.");
			var arrow = quiver.First ();

			if (target == null)
				throw new ArgumentNullException ("target");
			if (user == null)
				throw new ArgumentNullException ("user");
			
			var baseHit = arrow.BaseHit;
			var chance = HitDamageCalculator.GetPctHit (
				             user,
				             target,
				             ConstantesRecursos.CertezaRango,
				             ConstantesRecursos.EvasiónRango,
				             baseHit);
			var dmg = HitDamageCalculator.Damage (
				          user,
				          target,
				          ConstantesRecursos.Fuerza,
				          ConstantesRecursos.Fuerza, arrow.Attribute);
			dmg *= arrow.DamageMultiplier;

			var ef = new ChangeRecurso (
				         user,
				         target,
				         ConstantesRecursos.HP,
				         -dmg, 
				         chance);

			var ret = new SkillInstance (this, user);
			ret.Effects.Chance = chance;
			ret.Effects.AddEffect (ef);
			ret.Effects.AddEffect (new GenerateCooldownEffect (user, user, BaseCooldown), true);
			ret.Effects.AddEffect (new RemoveItemEffect (user, user, arrow, 1));

			return ret;
		}

		/// <summary>
		/// Devuelve la última instancia generada.
		/// </summary>
		/// <value>The last generated instance.</value>
		[JsonIgnore]
		public SkillInstance LastGeneratedInstance { get; protected set; }

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
							user.Exp.AddAssignation (ConstantesRecursos.CertezaRango, "base", 0.4f);
							tg.Exp.AddAssignation (ConstantesRecursos.EvasiónRango, "base", 0.2f);
							tg.Recursos.GetRecurso (ConstantesRecursos.Equilibrio).Valor -= 0.1f;
							break;
						case EffectResultEnum.Miss:
							user.Exp.AddAssignation (ConstantesRecursos.CertezaRango, "base", 0.2f);
							tg.Exp.AddAssignation (ConstantesRecursos.EvasiónRango, "base", 0.4f);
							tg.Recursos.GetRecurso (ConstantesRecursos.Equilibrio).Valor -= 0.2f;
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
		public bool IsCastable (IUnidad user)
		{
			// TODO: Debe consumir(requerir) ¿ammo?
			return true;
		}

		/// <summary>
		/// Determines whether this instance is visible the specified user.
		/// </summary>
		/// <param name="user">User.</param>
		public bool IsVisible (IUnidad user)
		{
			return true;
		}

		bool ISkill.IsLearnable { get { return false; } }

		string[] ISkill.RequieredSkills { get { return new string[]{ }; } }

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

		[JsonConstructor]
		RangedSkill (string Name, string TextureName, string Icon, float BaseCooldown, string Attribute)
		{
			this.Name = Name;
			this.TextureName = TextureName;
			IconName = Icon;
			this.BaseCooldown = BaseCooldown;
		}
	}
}
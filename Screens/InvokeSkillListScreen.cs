using System;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Moggle.Screens;
using Units;
using Units.Skills;
using AoM;
using Skills;

namespace Screens
{
	/// <summary>
	/// This <see cref="Screen"/> lets the user pick a skill to use
	/// </summary>
	public class InvokeSkillListScreen : Screen
	{
		/// <summary>
		/// Gets a value indicating whether the user has selected a <see cref="ISkill"/> or not.
		/// </summary>
		/// <value><c>true</c> if hay selección; otherwise, <c>false</c>.</value>
		public bool HaySelección { get { return _selección != null; } }

		ISkill _selección;
		SkillInstance _skillInstance;

		/// <summary>
		/// Gets the selected <see cref="ISkill"/>
		/// </summary>
		/// <value>The selected <see cref="ISkill"/>; <c>null</c> if it does not exist</value>
		public ISkill Selección
		{
			get
			{
				if (!HaySelección)
					throw new InvalidOperationException ();
				return _selección;
			}
		}

		/// <summary>
		/// Gets the <see cref="IUnidad"/> user
		/// </summary>
		public IUnidad Unidad { get; }

		/// <summary>
		/// Gets the main selection control
		/// </summary>
		/// <value>The contenedor.</value>
		public ContenedorSelección<ISkill> Contenedor { get; }

		/// <summary>
		/// Gets the background color.
		/// <see cref="Color.DarkGray"/>
		/// </summary>
		public override Color? BgColor { get { return Color.DarkGray; } }

		/// <summary>
		/// Initializes the controls and builds the skill list
		/// </summary>
		protected override void DoInitialization ()
		{
			buildSkillList ();
			base.DoInitialization ();
		}

		/// <summary>
		/// Builds the skill list.
		/// </summary>
		/// <remarks>It does not clear the state of the list</remarks>
		void buildSkillList ()
		{
			// generar skills propios

			foreach (var sk in Unidad.EnumerateAllSkills ())
			{
				sk.AddContent ();
				Contenedor.Add (sk);
			}
		}

		/// <summary>
		/// Rebice señal del teclado.
		/// <c>Esc</c> to leave
		/// </summary>
		/// <param name="data">Señal tecla</param>
		public override bool RecibirSeñal (Tuple<MonoGame.Extended.InputListeners.KeyboardEventArgs, ScreenThread> data)
		{
			var key = data.Item1;
			if (key.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
			{
				Juego.ScreenManager.ActiveThread.TerminateLast ();
				return true;
			}
			return base.RecibirSeñal (data);
		}

		void AlSeleccionarSkill (object sender, EventArgs e)
		{
			_selección = Contenedor.FocusedItem;
			Contenedor.Activado -= AlSeleccionarSkill;
		}

		public override void Update (GameTime gameTime, ScreenThread currentThread)
		{
			base.Update (gameTime, currentThread);
			if (_skillInstance == null)
			{
				if (_selección != null)
				{
					_selección.GetInstance (Unidad);
					_skillInstance = _selección.LastGeneratedInstance;
				}
			}
			else
			{
				_skillInstance.Execute ();
				currentThread.TerminateLast ();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.InvokeSkillListScreen"/> class.
		/// </summary>
		/// <param name="gm">Game instance</param>
		/// <param name="unid">Unidad</param>
		public InvokeSkillListScreen (Moggle.Game gm, IUnidad unid)
			: base (gm)
		{
			Unidad = unid;
			Contenedor = new ContenedorSelección<ISkill> (this)
			{
				TextureFondoName = "Interface//win_bg",
				TipoOrden = Contenedor<ISkill>.TipoOrdenEnum.FilaPrimero,
				TamañoBotón = new MonoGame.Extended.Size (32, 32),
				Posición = new Point (30, 30),
				MargenExterno = new MargenType
				{
					Top = 5,
					Left = 5,
					Bot = 5,
					Right = 5
				},
				MargenInterno = new MargenType
				{
					Top = 1,
					Left = 1,
					Bot = 1,
					Right = 1
				},
				GridSize = new MonoGame.Extended.Size (15, 15),
				BgColor = Color.LightBlue * 0.5f,
				SelectionEnabled = false,

				DownKey = GlobalKeys.SelectDownKey [0],
				UpKey = GlobalKeys.SelectUp [0],
				LeftKey = GlobalKeys.SelectLeft [0],
				RightKey = GlobalKeys.SelectRight [0],
				EnterKey = GlobalKeys.Accept [0],
			};

			AddComponent (Contenedor);
			Contenedor.Activado += AlSeleccionarSkill;
		}
	}
}
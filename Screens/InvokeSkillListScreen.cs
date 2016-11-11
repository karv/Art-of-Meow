using System;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Moggle.Screens;
using Units;
using Units.Skills;

namespace Screens
{
	/// <summary>
	/// This <see cref="Screen"/> lets the user pick a skill to use
	/// </summary>
	public class InvokeSkillListScreen : DialScreen
	{
		/// <summary>
		/// Gets a value indicating whether the user has selected a <see cref="ISkill"/> or not.
		/// </summary>
		/// <value><c>true</c> if hay selección; otherwise, <c>false</c>.</value>
		public bool HaySelección { get { return _selección != null; } }

		ISkill _selección;

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
		public override Color BgColor { get { return Color.DarkGray; } }

		/// <summary>
		/// Initializes the controls and builds the skill list
		/// </summary>
		public override void Initialize ()
		{
			buildSkillList ();
			base.Initialize ();

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
		/// Always draw base screen
		/// </summary>
		public override bool DibujarBase { get { return true; } }

		/// <summary>
		/// Rebice señal del teclado.
		/// <c>Esc</c> to leave
		/// </summary>
		/// <param name="key">Señal tecla</param>
		public override bool RecibirSeñal (MonoGame.Extended.InputListeners.KeyboardEventArgs key)
		{
			if (key.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
				Salir ();
			return base.RecibirSeñal (key);
		}

		void AlSeleccionarSkill (object sender, EventArgs e)
		{
			_selección = Contenedor.FocusedItem;
			_selección.Execute (Unidad);
			Salir ();
		}

		/// <summary>
		/// Se sale de este diálogo.
		/// Libera todo los recursos usados.
		/// </summary>
		public override void Salir ()
		{
			base.Salir ();
			// This line should be auto-handled by the garbage collector
			Contenedor.Activado -= AlSeleccionarSkill;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.InvokeSkillListScreen"/> class.
		/// </summary>
		/// <param name="baseScreen">Base screen</param>
		/// <param name="unid">Unidad</param>
		public InvokeSkillListScreen (IScreen baseScreen, IUnidad unid)
			: base (baseScreen.Juego,
			        baseScreen)
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

			};

			AddComponent (Contenedor);
			Contenedor.Activado += AlSeleccionarSkill;
		}
	}
}
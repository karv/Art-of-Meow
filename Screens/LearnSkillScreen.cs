using System;
using AoM;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Moggle.Screens;
using Skills;
using Units;

namespace Screens
{
	/// <summary>
	/// The window that asks for next skill to learn
	/// </summary>
	public class LearnSkillScreen : Screen
	{
		/// <summary>
		/// Unidad
		/// </summary>
		public readonly IUnidad Unidad;
		/// <summary>
		/// The icon container
		/// </summary>
		public readonly ContenedorSelección<ISkill> Contenedor;

		/// <summary>
		/// Realiza la inicialización
		/// </summary>
		protected override void DoInitialization ()
		{
			base.DoInitialization ();
			buildList ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.LearnSkillScreen"/> class.
		/// </summary>
		/// <param name="game">Game.</param>
		/// <param name="unid">Unid.</param>
		public LearnSkillScreen (Moggle.Game game, IUnidad unid)
			: base (game)
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
				SelectionEnabled = true,

				DownKey = GlobalKeys.SelectDownKey [0],
				UpKey = GlobalKeys.SelectUp [0],
				LeftKey = GlobalKeys.SelectLeft [0],
				RightKey = GlobalKeys.SelectRight [0],
				EnterKey = GlobalKeys.Accept [0]
			};

			Contenedor.Selection.AllowEmpty = true;
			Contenedor.Selection.AllowMultiple = false;

			Contenedor.Activado += select;

			AddComponent (Contenedor);
		}

		void buildList ()
		{
			Contenedor.Clear ();
			var openSkills = ((Juego)Juego).SkillList.GetOpenSkillsFor (Unidad);
			foreach (var sk in openSkills)
			{
				Contenedor.Add (sk);
			}
			if (Contenedor.Count == 0)
				throw new Exception ();

		}

		void select (object sender, EventArgs e)
		{
			var selSkill = Contenedor.FocusedItem;
			Unidad.Skills.Learning.CurrentlyLearning = selSkill;
			Juego.ScreenManager.ActiveThread.TerminateLast ();
		}
	}
}
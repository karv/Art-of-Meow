using AoM;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Moggle.Screens;
using Units;
using Skills;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Screens
{


	public class LearnSkillScreen : Screen
	{
		public readonly IUnidad Unidad;
		public readonly ContenedorSelección<ISkill> Contenedor;

		protected override void DoInitialization ()
		{
			base.DoInitialization ();
			buildList ();
		}

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

			Contenedor.Selection.AllowMultiple = false;
			Contenedor.Selection.AllowEmpty = false;

			Contenedor.Activado += select;

			AddComponent (Contenedor);
		}

		void buildList ()
		{
			Contenedor.Clear ();
			var openSkills = ((Juego)Juego).SkillList.GetOpenSkillsFor (Unidad);
			foreach (var sk in openSkills)
			{
				var skl = ((Juego)Juego).SkillList.GetSkill (sk);
				Contenedor.Add (skl);
			}
		}

		void select (object sender, System.EventArgs e)
		{
			var sel = Contenedor.Selection.GetSelection ();
			if (sel.Count == 0)
				return;

			var selSkill = sel [0];
			Unidad.Skills.Learning.CurrentlyLearning = selSkill;
			Juego.ScreenManager.ActiveThread.TerminateLast ();
		}
	}
}
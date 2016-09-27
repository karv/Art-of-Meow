using System;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Moggle.Screens;
using Units;

namespace Screens
{
	public class InvokeSkillListScreen : DialScreen
	{
		public bool HaySelección { get { return _selección != null; } }

		ISkill _selección;

		public ISkill Selección
		{
			get
			{
				if (!HaySelección)
					throw new InvalidOperationException ();
				return _selección;
			}
		}

		public IUnidad Unidad { get; }

		public ContenedorSelección<ISkill> Contenedor { get; }

		public override Color BgColor { get { return Color.DarkGray; } }

		public override void Initialize ()
		{
			base.Initialize ();
			buildSkillList ();
		}

		void buildSkillList ()
		{
			throw new NotImplementedException ();
		}

		public override bool DibujarBase { get { return true; } }

		public override bool RecibirSeñal (MonoGame.Extended.InputListeners.KeyboardEventArgs key)
		{
			if (key.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
				Salir ();
			return base.RecibirSeñal (key);
		}

		void AlSeleccionarSkill (object sender, EventArgs e)
		{
			_selección = Contenedor.FocusedItem;
			Salir ();
		}

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
				SelectionEnabled = false
			};

			AddComponent (Contenedor);
			Contenedor.Activado += AlSeleccionarSkill;
		}
	}
}
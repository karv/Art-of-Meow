using System;
using Units;
using Moggle.Controles;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Shapes;
using MonoGame.Extended;

namespace Componentes
{
	/// <summary>
	/// Muestra información útil del jugador
	/// </summary>
	public class PlayerInfoControl : DSBC
	{
		public IUnidad Player { get; }

		Rectangle drawingArea;

		public Rectangle DrawingArea
		{
			get
			{
				return drawingArea;
			}
			set
			{
				drawingArea = value;
				_playerHooks.Posición = DrawingArea.Location + new Point (30, 30);
			}
		}

		/// <summary>
		/// Gets the visual control that displays the buffs
		/// </summary>
		public BuffDisplay _playerHooks { get; private set; }

		#region DSBC

		protected override IShapeF GetBounds ()
		{
			return (RectangleF)DrawingArea;
		}

		public override void Update (GameTime gameTime)
		{
		}

		protected override void Draw ()
		{
			(_playerHooks as IDrawable).Draw (null);
		}

		#endregion

		public PlayerInfoControl (IComponentContainerComponent<IControl> cont,
		                          IUnidad player)
			: base (cont)
		{
			if (player == null)
				throw new ArgumentNullException ("player");
			Player = player;
			_playerHooks = new BuffDisplay (cont, Player)
			{
				MargenInterno = new MargenType
				{
					Top = 1,
					Bot = 1,
					Left = 1,
					Right = 1
				},
				TamañoBotón = new Size (16, 16),
				TipoOrden = Contenedor<IDibujable>.TipoOrdenEnum.ColumnaPrimero,
				Posición = DrawingArea.Location + new Point (30, 30)
			};
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using Units;

namespace Componentes
{
	/// <summary>
	/// Muestra información útil del jugador
	/// </summary>
	public class PlayerInfoControl : DSBC
	{
		/// <summary>
		/// Jugador del cual se muestra la información
		/// </summary>
		public IUnidad Player { get; }

		Rectangle drawingArea;

		/// <summary>
		/// Devuelve o establece el área de dibujo para este control
		/// </summary>
		/// <value>The drawing area.</value>
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

		/// <summary>
		/// Devuelve el límite gráfico del control.
		/// </summary>
		/// <returns>The bounds.</returns>
		protected override IShapeF GetBounds ()
		{
			return (RectangleF)DrawingArea;
		}

		/// <summary>
		/// Update lógico
		/// </summary>
		public override void Update (GameTime gameTime)
		{
		}

		/// <summary>
		/// Dibuja el control.
		/// </summary>
		protected override void Draw ()
		{
			(_playerHooks as IDrawable).Draw (null);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Componentes.PlayerInfoControl"/> class.
		/// </summary>
		/// <param name="cont">Contenedor</param>
		/// <param name="player">Jugador de información</param>
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
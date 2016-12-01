using System;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using Units;
using Moggle.Screens;

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
				PlayerHooks.Posición = DrawingArea.Location + new Point (30, 30);
			}
		}

		/// <summary>
		/// Gets the visual control that displays the buffs
		/// </summary>
		public BuffDisplay PlayerHooks { get; private set; }

		/// <summary>
		/// Resource view manager
		/// </summary>
		public RecursoView RecursoView { get; private set; }

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
			(PlayerHooks as IDrawable).Draw (null);
			(RecursoView as IDrawable).Draw (null);
		}

		/// <summary>
		/// Se ejecuta antes del ciclo, pero después de saber un poco sobre los controladores.
		///  No invoca LoadContent por lo que es seguro agregar componentes
		/// </summary>
		public override void Initialize ()
		{
			PlayerHooks.Initialize ();
			RecursoView.TopLeft = PlayerHooks.Posición + new Point (0, 60);
			(RecursoView as IComponent).Initialize ();
		}

		/// <summary>
		/// Loads the content.
		/// </summary>
		protected override void AddContent ()
		{
			(PlayerHooks as IComponent).AddContent ();
			(RecursoView as IComponent).AddContent ();
		}

		/// <summary>
		/// Vincula el contenido a campos de clase
		/// </summary>
		protected override void InitializeContent ()
		{
			(PlayerHooks as IComponent).InitializeContent ();
			(RecursoView as IComponent).InitializeContent ();
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Componentes.PlayerInfoControl"/> class.
		/// </summary>
		/// <param name="cont">Contenedor</param>
		/// <param name="player">Jugador de información</param>
		public PlayerInfoControl (IScreen cont,
		                          IUnidad player)
			: base (cont)
		{
			if (player == null)
				throw new ArgumentNullException ("player");
			Player = player;
			PlayerHooks = new BuffDisplay (cont, Player)
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

			RecursoView = new RecursoView (cont, Player.Recursos);
		}
	}
}
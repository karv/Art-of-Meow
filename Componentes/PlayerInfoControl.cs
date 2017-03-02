using System;
using AoM;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Shapes;
using Screens;
using Units;
using Units.Recursos;
using Units.Inteligencia;

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
		public Unidad Player { get; }

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
				Stats.Pos = DrawingArea.Location + new Point (30, 140);
			}
		}

		BitmapFont smallFont;

		/// <summary>
		/// Gets the visual control that displays the buffs
		/// </summary>
		public BuffDisplay PlayerHooks { get; private set; }

		/// <summary>
		/// Resource view manager
		/// </summary>
		public RecursoView RecursoView { get; private set; }

		public MinimapControl Minimap { get; private set; }

		/// <summary>
		/// Devuelve el control que muestra los stats
		/// </summary>
		/// <value>The stats.</value>
		public MultiEtiqueta Stats { get; private set; }

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
			RecursoView.Update (gameTime);
			PlayerHooks.Update (gameTime);
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
		/// Reconstruye la lista de stats
		/// </summary>
		public void ReloadStats ()
		{
			Stats.Clear ();
			foreach (var stat in Player.Recursos.Enumerate ())
				foreach (var param in stat.EnumerateParameters ())
				{
					Stats.Add (
						smallFont,
						string.Format (
							"{0}.{1}:{2}[{3} + {4}]",
							stat.NombreCorto ?? stat.NombreÚnico,
							param.NombreÚnico,
							param.ModifiedValue (),
							param.Valor,
							param.DeltaValue ()),
						Color.White);
				}
		}

		/// <summary>
		/// Loads the content using a given manager
		/// </summary>
		protected override void LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			(PlayerHooks as IComponent).LoadContent (manager);
			(RecursoView as IComponent).LoadContent (manager);
			smallFont = manager.Load<BitmapFont> ("Fonts//small");
			ReloadStats ();
		}

		#endregion

		GameTimeManager Time { get; }

		/// <summary>
		/// Shuts down the component.
		///  De de-suscribe a los eventos del ratón
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Componentes.PlayerInfoControl"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Componentes.PlayerInfoControl"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="Componentes.PlayerInfoControl"/>
		/// so the garbage collector can reclaim the memory that the <see cref="Componentes.PlayerInfoControl"/> was occupying.</remarks>
		protected override void Dispose ()
		{
			base.Dispose ();
			Time.AfterTimePassed -= updateEvent;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Componentes.PlayerInfoControl"/> class.
		/// </summary>
		/// <param name="cont">Contenedor</param>
		/// <param name="player">Jugador de información</param>
		public PlayerInfoControl (MapMainScreen cont,
		                          Unidad player)
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

			Minimap = new MinimapControl (cont)
			{
				Location = new Rectangle (700, 500, 200, 200),
				DisplayingGrid = (Player.Inteligencia as HumanIntelligence).Memory
			};

			Stats = new MultiEtiqueta (cont)
			{
				NumEntradasMostrar = 15,
				TiempoEntreCambios = TimeSpan.FromMilliseconds (4000)
			};

			Time = cont.Grid.TimeManager;
			Time.AfterTimePassed += updateEvent;
		}

		void updateEvent (object sender, float e)
		{
			ReloadStats ();
		}
	}
}
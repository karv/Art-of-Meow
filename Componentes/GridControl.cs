using System;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System.Runtime.InteropServices;
using Units;
using System.Threading;
using Units.Inteligencia;
using Units.Recursos;
using System.Diagnostics;
using Helper;

namespace Componentes
{
	/// <summary>
	/// The Griod system
	/// </summary>
	public class GridControl : DSBC, IComponentContainerComponent<IGridObject>
	{
		/// <summary>
		/// Devuelve el tablero lógico
		/// </summary>
		public LogicGrid Grid { get; private set; }

		/// <summary>
		/// Cambia el tablero actual, liberando completamente al anterior e inicializando el nuevo
		/// </summary>
		/// <param name="newGrid">Nuevo tablero lógico</param>
		public void ChangeGrid (LogicGrid newGrid)
		{
			Grid.Dispose ();
			Grid = newGrid;
			reInitialize ();
		}

		void reInitialize ()
		{
			Initialize ();
			var cont = Game.Contenido;
			AddContent ();
			cont.Load ();
			InitializeContent ();
		}

		void suscribtions ()
		{
			// Suscribirse a las unidades
			foreach (var x in AIPlayers)
			{
				var hpRec = x.Recursos.GetRecurso (ConstantesRecursos.HP) as RecursoHP;
				hpRec.ValorChanged += hp_damage_done_event;
			}
		}

		const string damageFont = "Fonts//damage";

		void hp_damage_done_event (object sender, float e)
		{
			const int milisecsDuration = 3000;
			var rec = sender as IRecurso;
			var unid = rec.Unidad;
			var delta = rec.Valor - e;
			var str = new VanishingLabel (
				          Screen,
				          delta.ToString (),
				          TimeSpan.FromMilliseconds (milisecsDuration))
			{
				ColorInicial = Color.Green,
				ColorFinal = Color.Transparent,
				FontName = damageFont,
				Velocidad = new Vector2 (0, -1),
			};
			((IComponent)str).InitializeContent ();
			((IComponent)str).Initialize ();

			// La posición ya que cargue el contenido
			str.Centro = CellSpotLocation (unid.Location).ToVector2 ();
			Screen.AddComponent (str);
		}


		IEnumerable<Unidad> AIPlayers
		{ 
			get { return _objects.OfType<Unidad> ().Where (z => !(z.Inteligencia is HumanIntelligence)); }
		}

		ICollection<IGridObject> _objects { get { return Grid.Objects; } }

		/// <summary>
		/// The size of a cell (Draw)
		/// </summary>
		public Size CellSize = new Size (24, 24);

		/// <summary>
		/// Celda de _data que se muestra en celda visible (0,0)
		/// </summary>
		public Point CurrentVisibleTopLeft = Point.Zero;

		/// <summary>
		/// Posición top left del control.
		/// </summary>
		public Point ControlTopLeft = Point.Zero;
		/// <summary>
		/// Gets the number of visible cells
		/// </summary>
		public Size VisibleCells = new Size (50, 20);

		/// <summary>
		/// Gets the size of this grid, as a <see cref="IControl"/>
		/// </summary>
		/// <value>The size of the control.</value>
		public Size ControlSize
		{
			get
			{
				return new Size (VisibleCells.Width * CellSize.Width,
					VisibleCells.Height * CellSize.Height);
			}
		}

		/// <summary>
		/// Devuelve la posición de un spot de celda (por lo tanto coordenadas absolutas)
		/// </summary>
		/// <param name="p">coordenadas del spot</param>
		public Point CellSpotLocation (Point p)
		{
			var _x = ControlTopLeft.X + CellSize.Width * (p.X - CurrentVisibleTopLeft.X);
			var _y = ControlTopLeft.Y + CellSize.Height * (p.Y - CurrentVisibleTopLeft.Y);
			return new Point (_x, _y);
		}

		/// <summary>
		/// Gets the bounds
		/// </summary>
		/// <value>The bounds.</value>
		public RectangleF Bounds
		{
			get
			{
				return new RectangleF (ControlTopLeft.ToVector2 (), ControlSize);
			}
		}

		/// <summary>
		/// Devuelve la posición de un spot de celda (por lo tanto coordenadas absolutas)
		/// </summary>
		/// <param name="x">Coordenada X</param>
		/// <param name="y">Coordenada Y</param>
		public Point CellSpotLocation (int x, int y)
		{
			return CellSpotLocation (new Point (x, y));
		}

		public IUnidad CameraUnidad { get; set; }

		public Point VisibilityPoint
		{ 
			get{ return CameraUnidad.Location; }
		}

		/// <summary>
		/// Dibuja el control.
		/// </summary>
		protected override void Draw ()
		{
			//var bat = Screen.
			//bat.Begin (SpriteSortMode.BackToFront);
			var bat = Screen.Batch;
			foreach (var x in _objects)
			{
				// TODO: ¿Dibujar los Cells y no los Objects?
				if (IsVisible (x.Location) &&
				    IsVisibleFrom (VisibilityPoint, x.Location)) // Si está dentro del área
				{
					var rectOutput = new Rectangle (CellSpotLocation (x.Location), CellSize);
					x.Draw (bat, rectOutput);
				}
			}
		}

		public bool IsVisibleFrom (Point source, Point target)
		{
			var line = Geometry.EnumerateLine (source, target);
			foreach (var x in line)
			{
				var pCell = Grid.GetCell (x);
				if (pCell.BlocksVisibility ())
					return false;
			}
			return true;
		}


		/// <summary>
		/// Devuelve el límite gráfico del control.
		/// </summary>
		/// <returns>The bounds.</returns>
		protected override IShapeF GetBounds ()
		{
			return Bounds;
		}

		/// <summary>
		/// Agrega el contenido a la biblitoeca
		/// </summary>
		protected override void AddContent ()
		{
			base.AddContent ();
			foreach (var x in _objects)
				x.AddContent ();

			Screen.Content.AddContent (damageFont);
		}

		/// <summary>
		/// Vincula el contenido a campos de clase
		/// </summary>
		protected override void InitializeContent ()
		{
			base.InitializeContent ();
			foreach (var x in _objects)
				x.InitializeContent ();
		}

		/// <summary>
		/// Update lógico
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public override void Update (GameTime gameTime)
		{
		}

		/// <summary>
		/// Se ejecuta antes del ciclo, pero después de saber un poco sobre los controladores.
		/// No invoca LoadContent por lo que es seguro agregar componentes
		/// </summary>
		public override void Initialize ()
		{
			base.Initialize ();
			Grid.AddedObject += itemAdded;
			suscribtions ();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="GridControl"/> object.
		/// Unsusbribe to Grid's events; so it can be collected by GC.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="GridControl"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="GridControl"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="GridControl"/> so the garbage
		/// collector can reclaim the memory that the <see cref="GridControl"/> was occupying.</remarks>
		protected override void Dispose ()
		{
			Grid.AddedObject -= itemAdded;
			base.Dispose ();
		}

		#region Cámara

		/// <summary>
		/// Centra el campo visible en la dirección de una celda.
		/// </summary>
		/// <param name="p">P.</param>
		public void TryCenterOn (Point p)
		{
			var left = Math.Max (0, p.X - VisibleCells.Width / 2);
			var top = Math.Max (0, p.Y - VisibleCells.Height / 2);
			CurrentVisibleTopLeft = new Point (left, top);
		}

		/// <summary>
		/// Determina si una dirección de celda es visible actualmente.
		/// </summary>
		/// <param name="p">Dirección de celda.</param>
		public bool IsVisible (Point p)
		{
			return GetVisibilityBox ().Contains (p);
		}

		/// <summary>
		/// Gets a rectangle representing the edges (mod grid) of the view
		/// </summary>
		public Rectangle GetVisibilityBox ()
		{
			return new Rectangle (CurrentVisibleTopLeft, VisibleCells);
		}

		/// <summary>
		/// The size of the edge.
		/// Objects outside this area are considered as "centered enough"
		/// </summary>
		static Size _edgeSize = new Size (4, 3);

		/// <summary>
		/// Centers the view on a given object, if it is not centered enough.
		/// </summary>
		public void CenterIfNeeded (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");

			if (!IsInCenter (obj.Location, _edgeSize))
				TryCenterOn (obj.Location);
		}

		/// <summary>
		/// Determines if a given point is centered enough
		/// </summary>
		/// <returns><c>true</c> if the given point is centered enough; otherwise, <c>false</c>.</returns>
		/// <param name="p">Grid-based point</param>
		/// <param name="edge_size">Size of the "not centered" area</param>
		/// <seealso cref="CenterIfNeeded"/>
		/// <seealso cref="_edgeSize"/>
		bool IsInCenter (Point p, Size edge_size)
		{
			var edge = GetVisibilityBox ();
			edge.Inflate (-edge_size.Width, -edge_size.Height);
			return edge.Contains (p);
		}

		#endregion

		#region Component container

		void IComponentContainerComponent<IGridObject>.AddComponent (IGridObject component)
		{
			_objects.Add (component);
		}

		bool IComponentContainerComponent<IGridObject>.RemoveComponent (IGridObject component)
		{
			return _objects.Remove (component);
		}

		IEnumerable<IGridObject> IComponentContainerComponent<IGridObject>.Components
		{
			get
			{
				return _objects;
			}
		}

		#endregion

		#region CollectionObserve

		void itemAdded (object sender, IGridObject e)
		{
			if (e.Texture == null)
			{
				// cargar textura
				var cont = Game.Contenido;
				e.AddContent ();
				cont.Load ();
				e.InitializeContent ();
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="GridControl"/> class.
		/// </summary>
		/// <param name="grid">El tablero lógico</param>
		/// <param name="scr">Screen where this grid belongs to</param>
		public GridControl (LogicGrid grid, Moggle.Screens.IScreen scr)
			: base (scr)
		{
			Grid = grid;
		}
	}
}
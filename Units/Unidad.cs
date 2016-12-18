using System;
using System.Diagnostics;
using AoM;
using Cells;
using Cells.CellObjects;
using Cells.Collision;
using Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Skills;
using Units.Buffs;
using Units.Equipment;
using Units.Inteligencia;
using Units.Order;
using Units.Recursos;

namespace Units
{
	/// <summary>
	/// Game player
	/// </summary>
	public class Unidad : IUnidad, IDibujable, IGameComponent
	{
		/// <summary>
		/// Gets the name
		/// </summary>
		/// <value>The nombre.</value>
		public string Nombre { get; set; }

		/// <summary>
		/// Ocurrs when location changes
		/// </summary>
		public event EventHandler OnRelocation;

		/// <summary>
		/// Determines whether this instance can move to the specified destination.
		/// </summary>
		/// <returns><c>true</c> if this instance can move the specified destination; otherwise, <c>false</c>.</returns>
		/// <param name="destination">Destination.</param>
		public bool CanMove (Point destination)
		{
			return true;
		}

		void IGridMoveable.BeforeMoving (Point destination)
		{
		}

		void IGridMoveable.AfterMoving (Point destination)
		{
			OnRelocation?.Invoke (this, EventArgs.Empty);
		}

		/// <summary>
		/// Gets the team's id
		/// </summary>
		public TeamManager Team { get; set; }

		/// <summary>
		/// Desarga el contenido gráfico.
		/// </summary>
		public void UnloadContent ()
		{
		}

		/// <summary>
		/// Reestablece a esta unidad para que sea consistente con un nuevo tablero
		/// </summary>
		public void Reinitialize ()
		{
			var hi = Inteligencia as HumanIntelligence;
			if (hi != null)
				hi.Memory = new MemoryGrid (this);
		}

		/// <summary>
		/// Enqueues a primitive order 
		/// </summary>
		public void EnqueueOrder (IPrimitiveOrder order)
		{
			PrimitiveOrders.Queue (order);
		}

		bool IUpdateGridObject.IsReady { get { return PrimitiveOrders.IsIdle; } }

		/// <summary>
		/// Gets the inventory of this unit
		/// </summary>
		public Inventory Inventory { get; }

		IInventory IUnidad.Inventory { get { return Inventory; } }

		/// <summary>
		/// Gets the grid.
		/// </summary>
		/// <value>The grid.</value>
		public LogicGrid Grid { get; set; }

		LogicGrid IUnidad.MapGrid { get { return Grid; } }

		/// <summary>
		/// Gets the orders corresponding this unidad
		/// </summary>
		/// <value>The primitive orders.</value>
		public OrderQueue PrimitiveOrders { get; }

		IComponentContainerComponent<IControl> IControl.Container 
		{ get { return (IComponentContainerComponent<IControl>)Grid; } }

		/// <summary>
		/// Execute this unidad's turn
		/// </summary>
		public virtual void Execute ()
		{
			Inteligencia.DoAction ();
		}

		/// <summary>
		/// Devuelve la experiencia que esta unidad otorga al ser asesinada
		/// </summary>
		public float GetExperienceValue ()
		{
			return 1 + Exp.ExperienciaTotal * 0.15f;
		}

		/// <summary>
		/// Pass time
		/// </summary>
		/// <param name="time">Time.</param>
		public void PassTime (float time)
		{
			PrimitiveOrders.PassTime (time);
			Update (time);
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public void Initialize ()
		{
			(Inteligencia as IGameComponent)?.Initialize ();
		}

		/// <summary>
		/// Gets the time for the next action.
		/// </summary>
		public float NextActionTime
		{
			get
			{
				return PrimitiveOrders.ExpectedFirstOrderTerminationTime ();
			}
		}

		/// <summary>
		/// Gets the resources of this unit
		/// </summary>
		/// <value>The recursos.</value>
		public ManejadorRecursos Recursos { get; }

		/// <summary>
		/// Gets the buffs if this unit
		/// </summary>
		/// <value>The buffs.</value>
		public BuffManager Buffs { get; }

		/// <summary>
		/// Gets the equipment of this unit
		/// </summary>
		/// <value>The equipment.</value>
		public EquipmentManager Equipment { get; }

		/// <summary>
		/// Gets the skills of this unit
		/// </summary>
		/// <value>The skills.</value>
		public Units.Skills.SkillManager Skills { get; }

		/// <summary>
		/// Devuelve el manejador de experiencia.
		/// </summary>
		public ExpManager Exp { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="Units.Unidad"/> is habilitado.
		/// </summary>
		/// <value><c>true</c> if habilitado; otherwise, <c>false</c>.</value>
		public bool Habilitado { get { return RecursoHP.Valor > 0; } }

		bool IUpdateGridObject.Enabled { get { return Habilitado; } }

		/// <summary>
		/// Gets the HP
		/// </summary>
		public RecursoHP RecursoHP { get; private set; }

		const string TextureType = "swordman";

		/// <summary>
		/// Gets or sets the texture string.
		/// </summary>
		/// <value>The texture string.</value>
		public string TextureStr { get; protected set; }

		/// <summary>
		/// Gets the texture used to draw this object
		/// </summary>
		/// <value>The texture.</value>
		public Texture2D Texture { get; protected set; }

		/// <summary>
		/// Carga el contenido gráfico de la unidad, equipment e inventory
		/// </summary>
		public void LoadContent (ContentManager manager)
		{
			Texture = manager.Load<Texture2D> (TextureStr);
		}

		/// <summary>
		/// Determina si este objeto evita que otro objeto pueda ocupar esta misma celda.
		/// </summary>
		/// <param name="collObj">Coll object.</param>
		public bool Collision (IGridObject collObj)
		{
			if (!Habilitado)
				return false;
			
			var collUnid = collObj as IUnidad;
			if (collUnid != null)
			{
				if (collUnid.Habilitado)
					return true;
			}
			return false;
		}

		float hpRelativeValue
		{
			get { return RecursoHP.RelativeHp; }
		}

		/// <summary>
		/// Dibuja el objeto sobre un rectpangulo específico
		/// </summary>
		/// <param name="bat">Batch</param>
		/// <param name="area">Rectángulo de dibujo</param>
		public void Draw (SpriteBatch bat, Rectangle area)
		{
			if (Habilitado)
				ForceDraw (area, bat);
		}

		float IGridObject.Depth
		{
			get { return Depths.Unit; }
		}

		/// <summary>
		/// Draw the unidad, even if it not enabled
		/// </summary>
		/// <param name="bat">Batch</param>
		/// <param name="area">Rectángulo de dibujo</param>
		public void ForceDraw (Rectangle area, SpriteBatch bat)
		{
			bat.Draw (
				Texture,
				area, null, Team.TeamColor,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depths.Unit);

			// Barras
			const int ht = 3;
			int sepPoint = (int)(area.Width * hpRelativeValue);
			var rec = new Rectangle (
				          area.Left + sepPoint,
				          area.Bottom,
				          area.Width - sepPoint,
				          ht);

			bat.Draw (
				Juego.Textures.SolidTexture,
				destinationRectangle: rec,
				color: Color.DarkGray * 0.7f,
				layerDepth: Depths.Gui);
			var fgRect = new Rectangle (area.Left, rec.Top, sepPoint, ht);

			bat.Draw (
				Juego.Textures.SolidTexture,
				destinationRectangle: fgRect,
				color: Color.Red,
				layerDepth: Depths.Gui);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Units.Unidad"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Units.Unidad"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="Units.Unidad"/> in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="Units.Unidad"/> so the garbage collector can reclaim the memory that the
		/// <see cref="Units.Unidad"/> was occupying.</remarks>
		public void Dispose ()
		{
			(Inteligencia as IDisposable)?.Dispose ();
		}

		/// <summary>
		/// Gets the cell-based localization.
		/// </summary>
		public Point Location { get; set; }

		/// <summary>
		/// Intenta moverse o atacar hacia una dirección
		/// </summary>
		/// <returns><c>true</c>, if action was taken, <c>false</c> otherwise.</returns>
		/// <param name="dir">Direction</param>
		public bool MoveOrMelee (MovementDirectionEnum dir)
		{
			var desde = Location;
			// Intenta mover este objeto; si no puede, intenta atacar.
			if (!Grid.MoveCellObject (this, dir))
			{
				// Do melee
				var targetCell = Grid [Location + dir.AsDirectionalPoint ()];
				var target = targetCell.GetAliveUnidadHere ();
				if (target == null)
					return false;

				// Construct the order
				assertIsIdle ();// Unidad debe estar idle para llegar aquí
				var melee = Equipment.GetMeleeDamageType ();
				var eff = melee.GetEffect (this, target);
				eff.Execute ();

				// Asignar exp
				Exp.AddAssignation (ConstantesRecursos.CertezaMelee, 0.3f);
				Exp.AddAssignation (ConstantesRecursos.Fuerza, 0.1f);
				target.Exp.AddAssignation (ConstantesRecursos.EvasiónMelee, 0.3f);
			}
			else
			{
				assertIsIdle ();// Unidad debe estar idle para llegar aquí
				PrimitiveOrders.Queue (new CooldownOrder (
					this,
					calcularTiempoMov (
						desde,
						Location)));
			}
			return true;
		}

		[Conditional ("DEBUG")]
		internal void assertIsIdle ()
		{
			if (!PrimitiveOrders.IsIdle)
				throw new Exception ();
		}

		float calcularTiempoMov (Point desde, Point hasta)
		{
			var vel = Recursos.ValorRecurso (ConstantesRecursos.Velocidad);
			var cellOrig = Grid.GetCell (desde);
			var cellDest = Grid.GetCell (hasta);
			var peso = (cellOrig.PesoMovimiento () + cellDest.PesoMovimiento ()) / 2;

			if (desde.X != hasta.X && desde.Y != hasta.Y) // Mov inclinado
				peso *= 1.4f;

			return peso / vel;
		}

		/// <summary>
		/// Gets or sets the controller of this unidad
		/// </summary>
		/// <value>The inteligencia.</value>
		public IUnidadController Inteligencia { get; set; }

		/// <summary>
		/// Update
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public void Update (float gameTime)
		{
			if (Habilitado)
				ForceUpdate (gameTime);
		}

		/// <summary>
		/// Updates this unidad, even if it is not <see cref="Habilitado"/>
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		protected void ForceUpdate (float gameTime)
		{
			Recursos.Update (gameTime);
			Buffs.Update (gameTime);
		}

		System.Collections.Generic.IEnumerable<ICollisionRule> ICollidableGridObject.GetCollisionRules ()
		{
			// Does not stack
			yield return new DescriptCollitionRule (z => Habilitado && ((z as IUnidad)?.Habilitado ?? false));
		}

		/// <summary>
		/// Occurs when this unidad is killed.
		/// </summary>
		public event EventHandler Killed;

		void Death (object sender, EventArgs e)
		{
			OnDeath ();
		}

		/// <summary>
		/// Ocurre cuando Hp llega a cero.
		/// Invoca el evento <see cref="Killed"/>.
		/// Tira todos los objetos del <see cref="Inventory"/>.
		/// </summary>
		protected virtual void OnDeath ()
		{
			Killed?.Invoke (this, EventArgs.Empty);
			DropAllItems ();
		}

		/// <summary>
		/// Tira todos sus objetos al suelo
		/// </summary>
		public void DropAllItems ()
		{
			foreach (var it in Inventory.Items)
			{
				var obj = new GroundItem (it, Grid);
				obj.Location = Location;
				// Inicializar objeto y contenido
				obj.Initialize ();
				// This is now done automatically: 

				/*
				 * var cont = Grid.Game.Contenido;
				 * obj.AddContent (cont);
				 * cont.Load ();
				 * obj.InitializeContent (cont);
				 */

				// Agregar el objeto al grid
				obj.AddToGrid ();
			}

			// Eliminar todo del contenido
			Inventory.Items.Clear ();
		}


		/// <summary>
		/// Gets the name.
		/// </summary>
		public override string ToString ()
		{
			return Nombre;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Unidad"/> class.
		/// </summary>
		/// <param name="texture">Texture name</param>
		/// <param name="grid">Game grid</param>
		public Unidad (LogicGrid grid,
		               string texture = TextureType)
		{
			Grid = grid;
			Nombre = getNextName ();
			TextureStr = texture;
			Recursos = new ManejadorRecursos (this);
			PrimitiveOrders = new OrderQueue ();
			Equipment = new EquipmentManager (this);
			Buffs = new BuffManager (this);
			Exp = new ExpManager (this);
			Inventory = new Inventory ();
			Skills = new Units.Skills.SkillManager (this);
			inicializarRecursos ();
			RecursoHP.ReachedZero += Death;
		}

		void inicializarRecursos ()
		{
			RecursoHP = new RecursoHP (this)
			{
				Max = 5,
				Valor = 5
			};
			Recursos.Add (RecursoHP);
			Recursos.Add (new StatRecurso (ConstantesRecursos.Velocidad, this)
			{
				TasaRecuperaciónNormal = 1,
				TasaRecuperaciónMax = 0.5f,
				Base = 10,
				Max = 10,
				Valor = 10
			});
			Recursos.Add (new StatRecurso (ConstantesRecursos.Destreza, this)
			{
				TasaRecuperaciónNormal = 1,
				TasaRecuperaciónMax = 0.5f,
				Base = 7,
				Max = 7,
				Valor = 7
			});
			Recursos.Add (new StatRecurso (ConstantesRecursos.Fuerza, this)
			{
				TasaRecuperaciónNormal = 1,
				TasaRecuperaciónMax = 0.5f,
				Base = 10,
				Max = 10,
				Valor = 10
			});

			Recursos.Add (new RecursoEquilibro (this)
			{
				Regen = 1,
				Valor = 1
			});

			Recursos.Add (new StatRecurso (ConstantesRecursos.Visión, this)
			{
				TasaRecuperaciónNormal = 0.01f,
				TasaRecuperaciónMax = 0.01f,
				Base = 5,
				Max = 5,
				Valor = 5
			});
		}

		static int nextId;

		static string getNextName ()
		{
			return "Unidad " + nextId++;
		}
	}
}
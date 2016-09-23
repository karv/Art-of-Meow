using System;
using System.Diagnostics;
using AoM;
using Cells;
using Cells.CellObjects;
using Componentes;
using Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Units.Buffs;
using Units.Equipment;
using Units.Inteligencia;
using Units.Recursos;

namespace Units
{
	public class Unidad : IUnidad, IRelDraw
	{
		public int Equipo { get; set; }

		public void UnloadContent ()
		{
		}

		public Inventory Inventory { get; }

		IInventory IUnidad.Inventory { get { return Inventory; } }

		public Grid Grid { get; }

		public Moggle.Controles.IComponentContainerComponent<Moggle.Controles.IControl> Container
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public virtual void Execute ()
		{
			Inteligencia.DoAction ();
		}

		public void PassTime (float time)
		{
			NextActionTime -= time;
			Update (time);
		}

		public void Initialize ()
		{
			throw new NotImplementedException ();
		}

		float _nextActionTime;

		public float NextActionTime
		{
			get
			{
				return _nextActionTime;
			}
			set
			{
				if (value < 0)
					throw new Exception ();
				_nextActionTime = value;
			}
		}

		public ManejadorRecursos Recursos { get; }

		public BuffManager Buffs { get; }

		public EquipmentManager Equipment { get; }

		public bool Habilitado { get { return RecursoHP.Valor > 0; } }

		public RecursoHP RecursoHP { get; private set; }

		public Grid MapGrid { get; set; }

		public const string TextureType = "person";

		public string TextureStr { get; protected set; }

		public Texture2D Texture { get; protected set; }

		public void LoadContent (ContentManager content)
		{
			Texture = content.Load<Texture2D> (TextureStr);
			Equipment.LoadContent (content);
			Inventory.LoadContent (content);
		}

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

		public void Draw (Rectangle area, SpriteBatch bat)
		{
			if (Habilitado)
				ForceDraw (area, bat);
		}

		public void ForceDraw (Rectangle area, SpriteBatch bat)
		{
			bat.Draw (
				Texture,
				area, null, Color.White,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depths.Unidad);

			// Barras
			var rec = new Rectangle (area.Left, area.Bottom, area.Width, 3);

			bat.Draw (Juego.Textures.SolidTexture, rec, Color.Gray * 0.7f);
			var fgRect = new Rectangle (
				             rec.Location, 
				             new Point (
					             (int)(rec.Width * hpRelativeValue),
					             rec.Height));

			bat.Draw (Juego.Textures.SolidTexture, fgRect, Color.Red);
		}

		public void Dispose ()
		{
			Texture = null;
		}

		/// <summary>
		/// Gets the cell-based localization.
		/// </summary>
		public Point Location { get; set; }

		public void MeleeDamage (IUnidad target)
		{
			if (Equipo == target.Equipo)
				return;
			var hp = target.Recursos.GetRecurso (ConstantesRecursos.HP);
			var dmg = Recursos.ValorRecurso (ConstantesRecursos.DañoMelee) / 8;
			hp.Valor -= dmg;
		}

		/// <summary>
		/// Move or melee to a direction
		/// </summary>
		/// <returns><c>true</c>, if action was taken, <c>false</c> otherwise.</returns>
		/// <param name="dir">Direction</param>
		public bool MoveOrMelee (MovementDirectionEnum dir)
		{
			var desde = Location;
			// Intenta mover este objeto; si no puede, intenta atacar.
			if (!MapGrid.MoveCellObject (this, dir))
			{
				var targetCell = new Cell (MapGrid, Location + dir.AsDirectionalPoint ());
				var target = targetCell.GetUnidadHere ();
				if (target == null)
					return false;
				NextActionTime = calcularTiempoMelee ();
				var dex = Recursos.GetRecurso (ConstantesRecursos.Destreza) as StatRecurso;
				dex.Valor *= 0.8f;
				MeleeDamage (target);
			}
			else
			{
				NextActionTime = calcularTiempoMov (desde, Location);
			}
			return true;
		}

		float calcularTiempoMelee ()
		{
			var dex = Recursos.ValorRecurso (ConstantesRecursos.Destreza);
			return 1 / dex;
		}

		float calcularTiempoMov (Point desde, Point hasta)
		{
			var vel = Recursos.ValorRecurso (ConstantesRecursos.Velocidad);
			var cellOrig = MapGrid.GetCell (desde);
			var cellDest = MapGrid.GetCell (hasta);
			var peso = (cellOrig.PesoMovimiento () + cellDest.PesoMovimiento ()) / 2;

			if (desde.X != hasta.X && desde.Y != hasta.Y) // Mov inclinado
				peso *= 1.4f;

			Debug.WriteLine (
				string.Format (
					"Peso de movimiento de {0} a {1} por {2} en {3}",
					desde,
					hasta,
					this,
					peso),
				"Movimiento");
			return peso / vel;
		}

		public IIntelligence Inteligencia { get; set; }

		public void Update (float gameTime)
		{
			if (Habilitado)
				ForceUpdate (gameTime);
		}

		protected void ForceUpdate (float gameTime)
		{
			Recursos.Update (gameTime);
			Buffs.Update (gameTime);
		}

		public override string ToString ()
		{
			return string.Format ("IA: {0}", Inteligencia);
		}

		public Unidad (string texture = TextureType)
		{
			TextureStr = texture;
			Recursos = new ManejadorRecursos (this);
			Equipment = new EquipmentManager (this);
			Buffs = new BuffManager (this);
			Inventory = new Inventory ();
			inicializarRecursos ();
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

			Recursos.Add (new RecursoFml (
				ConstantesRecursos.Fuerza,
				this,
				ConstantesRecursos.DañoMelee));
		}
	}
}
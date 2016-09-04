using System;
using System.Diagnostics;
using Art_of_Meow;
using Cells;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using Units.Recursos;

namespace Units
{
	public class Unidad : IUnidad
	{
		public virtual void Execute ()
		{
			IA.DoAction ();
		}

		public void PassTime (TimeSpan time)
		{
			NextActionTime -= time;
		}

		public TimeSpan NextActionTime { get; set; }

		public ManejadorRecursos Recursos { get; }

		public bool Habilitado { get { return RecursoHP.Valor > 0; } }

		public readonly RecursoHP RecursoHP;

		public Grid MapGrid { get; set; }

		public const string TextureType = "person";

		public string TextureStr { get; protected set; }

		public Texture2D Texture { get; protected set; }

		public void LoadContent (ContentManager content)
		{
			Texture = content.Load<Texture2D> (TextureStr);
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

		public void Draw (RectangleF area, SpriteBatch bat)
		{
			if (Habilitado)
				ForceDraw (area, bat);
		}

		public void ForceDraw (RectangleF area, SpriteBatch bat)
		{
			// TODO: Invocar el métido extendido de MonoGame.Extended
			var ar = area.ToRectangle ();
			bat.Draw (
				Texture,
				ar, null, Color.White,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depths.Unidad);

			// Barras
			var rec = new Rectangle (ar.Left, ar.Bottom, ar.Width, 3);

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

		public Point Location { get; set; }

		public void MeleeDamage (IUnidad target)
		{
			var hp = target.Recursos.GetRecurso ("hp");
			hp.Valor -= 1;
		}

		/// <summary>
		/// Move or melee to a direction
		/// </summary>
		/// <returns><c>true</c>, if action was taken, <c>false</c> otherwise.</returns>
		/// <param name="dir">Direction</param>
		public bool MoveOrMelee (MovementDirectionEnum dir)
		{
			TimeSpan tiempoMov = TimeSpan.FromMinutes (1);
			TimeSpan tiempoMelee = TimeSpan.FromMinutes (2);
			#if DEBUG
			if (NextActionTime != TimeSpan.Zero)
			{
				Debug.WriteLine (
					"Se regresó el control a una unidad con NextActionTime > 0",
					"Unit movement");
			}
			#endif
			// Intenta mover este objeto; si no puede, intenta atacar.
			if (!MapGrid.MoveCellObject (this, dir))
			{
				var targetCell = new Cell (MapGrid, Location + dir.AsDirectionalPoint ());
				var target = targetCell.GetUnidadHere ();
				if (target == null)
					return false;
				NextActionTime = tiempoMelee;
				MeleeDamage (target);
			}
			else
			{
				NextActionTime = tiempoMov;
			}
			return true;
		}

		public IIntelligence IA { get; set; }

		public void Update (GameTime gameTime)
		{
			if (Habilitado)
				ForceUpdate (gameTime);
		}

		public void ForceUpdate (GameTime gameTime)
		{
			Recursos.Update (gameTime);
		}

		public Unidad (string texture = TextureType)
		{
			TextureStr = texture;
			Recursos = new ManejadorRecursos ();
			RecursoHP = new RecursoHP (this)
			{
				Max = 5,
				Valor = 5
			};
			Recursos.Add (RecursoHP);
		}
	}
}
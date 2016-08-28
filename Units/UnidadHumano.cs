using Cells.CellObjects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Cells;
using Units.Recursos;
using MonoGame.Extended;
using Art_of_Meow;

namespace Units
{
	public class UnidadHumano : IUnidad
	{
		public ManejadorRecursos Recursos { get; }

		public bool Habilitado { get { return RecursoHP.Valor > 0; } }

		public readonly RecursoEstático RecursoHP;

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
			return collObj is UnidadHumano; 
		}

		float hpRelativeValue
		{
			get { return RecursoHP.Valor / 5; }
		}

		public void Draw (Rectangle area, SpriteBatch bat)
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

		public Point Location { get; set; }

		public void MeleeDamage (IUnidad target)
		{
			var hp = target.Recursos.GetRecurso ("hp");
			hp.Valor -= 1;
			target.Die ();
		}

		/// <summary>
		/// Move or melee to a direction
		/// </summary>
		/// <returns><c>true</c>, if action was taken, <c>false</c> otherwise.</returns>
		/// <param name="dir">Direction</param>
		public bool MoveOrMelee (MovementDirectionEnum dir)
		{
			if (!MapGrid.MoveCellObject (this, dir))
			{
				var targetCell = new Cell (MapGrid, Location + dir.AsDirectionalPoint ());
				var target = targetCell.GetUnidadHere ();
				if (target == null)
					return false;
				MeleeDamage (target);
			}
			return true;
		}

		void IUpdate.Update (GameTime gameTime)
		{
			Recursos.Update (gameTime);
		}

		public UnidadHumano (string texture = TextureType)
		{
			TextureStr = texture;
			Recursos = new ManejadorRecursos ();
			RecursoHP = new RecursoEstático ("hp", this);
			RecursoHP.NombreCorto = "HP";
			RecursoHP.NombreLargo = "Hit points";
			RecursoHP.Valor = 5;
			Recursos.Add (RecursoHP);
		}
	}
}
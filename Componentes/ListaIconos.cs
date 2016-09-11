using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

namespace Componentes
{
	public interface IRelDraw
	{
		void Draw (RectangleF topLeft);
	}

	/// <summary>
	/// Control del GUI que maneja la visibilidad los recursos.
	/// </summary>
	public class ListaIconos : DSBC
	{
		/// <summary>
		/// Devuelve o establece la posición
		/// </summary>
		/// <value>The top left.</value>
		public Vector2 TopLeft { get; set; }

		/// <summary>
		/// Devuelve la lista de iconos
		/// </summary>
		/// <value>The iconos.</value>
		public List<IRelDraw> Iconos { get; protected set; }

		/// <summary>
		/// Devuelve o establece el tamaño de cada icono
		/// </summary>
		/// <value>The size of the icon.</value>
		public SizeF IconSize { get; set; }

		/// <summary>
		/// Devuelve o establece el espaciado vertical entre iconos
		/// </summary>
		/// <value>The V space.</value>
		public float VSpace { get; set; }

		public override IShapeF GetBounds ()
		{
			if (Iconos.Count == 0)
				return RectangleF.Empty;
			var size = new SizeF (
				           IconSize.Width,
				           IconSize.Height * Iconos.Count + VSpace * Iconos.Count - 1);
			return new RectangleF (TopLeft, size);
		}

		/// <summary>
		/// Dibuja el control
		/// </summary>
		public override void Draw (GameTime gameTime)
		{
			var iconTopLeft = new Vector2 (TopLeft.X, TopLeft.Y);
			foreach (var ic in Iconos)
			{
				var outputRect = new RectangleF (iconTopLeft, IconSize);
				ic.Draw (outputRect);
				iconTopLeft += new Vector2 (0, IconSize.Height + VSpace);
			}
		}

		/// <summary>
		/// Update lógico
		/// </summary>
		public override void Update (GameTime gameTime)
		{
		}

		public ListaIconos (IComponentContainerComponent<IGameComponent> cont)
			: base (cont)
		{
			Iconos = new List<IRelDraw> ();
			IconSize = new SizeF (16, 16);
		}
	}
}
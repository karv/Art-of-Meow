using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

namespace Componentes
{
	/// <summary>
	/// Control del GUI que maneja la visibilidad los recursos.
	/// </summary>
	public class ListaIconos : DSBC
	{
		/// <summary>
		/// Devuelve o establece la posición
		/// </summary>
		/// <value>The top left.</value>
		public Point TopLeft { get; set; }

		/// <summary>
		/// Devuelve la lista de iconos
		/// </summary>
		/// <value>The iconos.</value>
		public List<IDibujable> Iconos { get; protected set; }

		/// <summary>
		/// Devuelve o establece el tamaño de cada icono
		/// </summary>
		/// <value>The size of the icon.</value>
		public Size IconSize { get; set; }

		/// <summary>
		/// Devuelve o establece el espaciado vertical entre iconos
		/// </summary>
		/// <value>The V space.</value>
		public int VSpace { get; set; }

		/// <summary>
		/// Devuelve el límite gráfico del control.
		/// </summary>
		protected override IShapeF GetBounds ()
		{
			if (Iconos.Count == 0)
				return RectangleF.Empty;
			var size = new SizeF (
				           IconSize.Width,
				           IconSize.Height * Iconos.Count + VSpace * Iconos.Count - 1);
			return new RectangleF (TopLeft.ToVector2 (), size);
		}

		/// <summary>
		/// Dibuja el control
		/// </summary>
		protected override void Draw (GameTime gameTime)
		{
			var iconTopLeft = new Point (TopLeft.X, TopLeft.Y);
			foreach (var ic in Iconos)
			{
				var outputRect = new Rectangle (iconTopLeft, IconSize);
				ic.Draw (Screen.Batch, outputRect);
				iconTopLeft += new Point (0, IconSize.Height + VSpace);
			}
		}

		/// <summary>
		/// Update lógico
		/// </summary>
		public override void Update (GameTime gameTime)
		{
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Componentes.ListaIconos"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Componentes.ListaIconos"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[ListaIconos: TopLeft={0}, Iconos={1}]",
				TopLeft,
				Iconos);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Componentes.ListaIconos"/> class.
		/// </summary>
		/// <param name="cont">Container of this control</param>
		public ListaIconos (IComponentContainerComponent<IControl> cont)
			: base (cont)
		{
			Iconos = new List<IDibujable> ();
			IconSize = new Size (16, 16);
		}
	}
}
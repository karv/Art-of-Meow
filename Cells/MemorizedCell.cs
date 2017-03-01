using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;

namespace Cells
{
	/// <summary>
	/// Representa la información estática sobre el dibujado de un <see cref="Cell"/>
	/// </summary>
	public struct MemorizedCell : IDibujable
	{
		readonly List<Texture2D> objects;

		void IComponent.LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
		}

		void IGameComponent.Initialize ()
		{
		}

		public static Color DefaultMinimapColor = Color.Black;

		public Color MinimapColor { get; }

		/// <summary>
		/// Devuelve o establece la transparencia
		/// </summary>
		/// <value>The alpha.</value>
		public float Alpha { get; set; }

		bool hasData { get { return objects != null; } }

		/// <summary>
		/// Dibuja el objeto sobre un rectángulo específico
		/// </summary>
		public void Draw (SpriteBatch bat, Rectangle rect)
		{
			if (hasData)
				foreach (var x in objects)
					bat.Draw (x, rect, Color.White * Alpha);
		}

		/// <summary>
		/// Devuelve una celda vacía
		/// </summary>
		/// <value>The void cell.</value>
		public static MemorizedCell VoidCell { get; }

		internal MemorizedCell (IEnumerable<Texture2D> data, float alpha = 0.3f)
		{
			objects = new List<Texture2D> (data);
			Alpha = alpha;
			MinimapColor = DefaultMinimapColor;
		}


		internal MemorizedCell (IEnumerable<Texture2D> data, Color color, float alpha = 0.3f)
		{
			objects = new List<Texture2D> (data);
			Alpha = alpha;
			MinimapColor = color;
		}

		static MemorizedCell ()
		{
			VoidCell = default (MemorizedCell);
		}
	}
}
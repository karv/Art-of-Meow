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

		internal MemorizedCell (IEnumerable<Texture2D> data, float alpha = 1)
		{
			objects = new List<Texture2D> (data);
			Alpha = alpha;
		}
	}
	
}
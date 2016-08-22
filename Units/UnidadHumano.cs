using Cells.CellObjects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Units
{
	public class UnidadHumano : IUnidad
	{
		public const string TextureType = "person";

		public UnidadHumano (ContentManager content)
		{
			CellObject = new PersonCellObject (TextureType, content);
		}

		public UnidadHumano (Texture2D texture)
		{
			CellObject = new PersonCellObject (texture);
		}

		public PersonCellObject CellObject { get; }

		ICellObject IUnidad.CellObject { get; }
	}
}
using Cells.CellObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Cells;
using System;

namespace Units
{
	public interface IIntelligence
	{
		void DoAction ();
	}

	public class ChaseIntelligence  : IIntelligence
	{
		public ChaseIntelligence (Grid mapGrid)
		{
			MapGrid = mapGrid;
		}

		public Grid MapGrid { get; }

		public void DoAction ()
		{
			var goCell = MapGrid.FindCellAddr (z => z.ExistsReturn (w => w is UnidadHumano) != null);
			if (goCell.HasValue)
			{
				// TODO: Que avance hacia goCell.
			}
			throw new System.NotImplementedException ();
		}
	}

	public class UnidadArtificial : IUnidad
	{
		public const string TextureType = "person";

		public UnidadArtificial (ContentManager content,
		                         string texture = TextureType)
		{
			CellObject = new PersonCellObject (texture, content);
		}

		public UnidadArtificial (Texture2D texture)
		{
			CellObject = new PersonCellObject (texture);
		}

		public IIntelligence IA { get; set; }

		public PersonCellObject CellObject { get; }

		ICellObject ICellLocalizable.CellObject { get { return CellObject; } }
	}
}
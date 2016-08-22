using Cells.CellObjects;
using Microsoft.Xna.Framework.Content;
using Cells;

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
			var target = MapGrid.Objects.Find (z => z is UnidadHumano);
			// TODO: que se mueva.

		}
	}

	public class UnidadArtificial : UnidadHumano
	{
		public UnidadArtificial (ContentManager content,
		                         string texture = TextureType)
			: base (content, texture)
		{
		}

		public IIntelligence IA { get; set; }
	}
}
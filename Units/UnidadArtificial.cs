using System.Linq;
using Cells;

namespace Units
{
	public interface IIntelligence
	{
		void DoAction ();
	}

	public class ChaseIntelligence  : IIntelligence
	{
		public ChaseIntelligence (UnidadArtificial yo)
		{
			Yo = yo;
		}

		public Grid MapGrid { get { return Yo.MapGrid; } }

		public readonly UnidadArtificial Yo;

		public void DoAction ()
		{
			var target = MapGrid.Objects.FirstOrDefault (z => z is UnidadHumano);
			var dir = Yo.Location.GetDirectionTo (target.Location);
			if (dir == MovementDirectionEnum.NoMov)
				return;
			MapGrid.MoveCellObject (Yo, dir);
		}
	}

	public class UnidadArtificial : UnidadHumano
	{
		public UnidadArtificial (string texture = TextureType)
			: base (texture)
		{
			RecursoHP.Valor = 2;
		}

		public IIntelligence IA { get; set; }
	}
}
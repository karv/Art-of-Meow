using System.Linq;
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
		public ChaseIntelligence (UnidadArtificial yo)
		{
			Yo = yo;
		}

		public Grid MapGrid { get { return Yo.MapGrid; } }

		public readonly UnidadArtificial Yo;

		public void DoAction ()
		{
			Yo.NextActionTime = TimeSpan.FromMinutes (2);
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
			RecursoHP.Max = 3;
			RecursoHP.Fill ();
		}

		public IIntelligence IA { get; set; }

		public override void Execute ()
		{
			IA.DoAction ();
		}
	}
}
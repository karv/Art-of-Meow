using NUnit.Framework;
using Units.Buffs;
using Cells;

namespace Test
{
	[TestFixture]
	public class TestBuffManager
	{
		BuffManager Man;
		Units.Unidad Unit;
		Grid GameGrid;

		[SetUp]
		public void Setup ()
		{
			GameGrid = new Grid (0, 0, null);
			Unit = new Units.Unidad (GameGrid);
			Man = new BuffManager (Unit);
		}

		[Test]
		public void Hook ()
		{
			var buff = new PoisonBuff ();
			Man.Hook (buff);
		}

		[Test]
		public void UnHook ()
		{
			var buff = new PoisonBuff ();
			Man.Hook (buff);
			Man.UnHook (buff);
		}

		[Test]
		public void Count ()
		{
			Hook ();
			Assert.AreEqual (1, Man.Count);
		}

		[Test]
		public void HasBuff ()
		{
			Assert.False (Man.HasBuff ("Poison"));
			Hook ();
			Assert.True (Man.HasBuff ("Poison"));
		}
	}
}
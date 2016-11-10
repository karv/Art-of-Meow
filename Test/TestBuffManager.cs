using Cells;
using NUnit.Framework;
using Units.Buffs;

namespace Test
{
	[TestFixture]
	public class TestBuffManager
	{
		BuffManager Man;
		Units.Unidad Unit;
		LogicGrid GameGrid;

		[SetUp]
		public void Setup ()
		{
			GameGrid = new LogicGrid (0, 0);
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
			Assert.AreEqual (2, Man.Count); // recordar que equipmentbuff siempre se tiene
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
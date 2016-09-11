using NUnit.Framework;
using Units.Buffs;

namespace Test
{
	[TestFixture]
	public class TestBuffManager
	{
		BuffManager Man;
		Units.Unidad Unit;

		[SetUp]
		public void Setup ()
		{
			Unit = new Units.Unidad ();
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
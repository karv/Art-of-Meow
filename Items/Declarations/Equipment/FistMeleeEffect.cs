using System.Diagnostics;
using Debugging;
using Helper;
using Items.Declarations.Equipment;
using Skills;
using Units;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// The melee effect when the user has not equiped melee effect weapon
	/// </summary>
	public class FistMeleeEffect : IMeleeEffect
	{
		/// <summary>
		/// Causes melee effect on a target
		/// </summary>
		/// <param name="user">The user of the melee move</param>
		/// <param name="target">Target.</param>
		public IEffect GetEffect (IUnidad user, IUnidad target)
		{
			const float baseHit = 0.9f;
			var ret = MeleeEffectHelper.BuildDefaultMeleeEffect (
				          user,
				          target,
				          0.4f,
				          baseHit);
			
			Debug.WriteLine (
				string.Format (
					"Melee effect from {0} to {1} causing\n{2}",
					user,
					target,
					ret.DetailedInfo ()),
				DebugCategories.MeleeResolution
			);

			return ret;
		}
	}
}
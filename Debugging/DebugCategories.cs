#if DEBUG

namespace Debugging
{
	/// <summary>
	/// Exposes constants for debug output categories .
	/// </summary>
	public static class DebugCategories
	{
		/// <summary>
		/// For resolutions of effects.
		/// </summary>
		public const string EffectResolution = "Effect";

		/// <summary>
		/// When a resource is affected
		/// </summary>
		public const string ResourceAffected = "Recurso";

		/// <summary>
		/// Affecting the experience of a unit
		/// </summary>
		public const string Experience = "Experience";

		/// <summary>
		/// For resolutions of effects of <see cref="Items.Declarations.Equipment.IMeleeEffect"/>
		/// </summary>
		public const string MeleeResolution = "Melee effect";

		/// <summary>
		/// In the equipment system
		/// </summary>
		public const string Equipment = "Equipment";

		/// <summary>
		/// Item system
		/// </summary>
		public const string Item = "Item";

		/// <summary>
		/// Buff system
		/// </summary>
		public const string Buff = "Buff";

		/// <summary>
		/// About the UnidadFactory
		/// </summary>
		public const string UnitFactory = "Unit factory";

		/// <summary>
		/// Map generation
		/// </summary>
		public const string MapGeneration = "Map";
	}
}
#endif
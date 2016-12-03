using System;
using System.Collections.Generic;

namespace Items.Modifiers
{
	/// <summary>
	/// Provee métodos para generar <see cref="ItemModifier"/>
	/// </summary>
	public static class ItemModifierGenerator
	{
		/// <summary>
		/// Genera un <see cref="ItemModification"/>
		/// </summary>
		public static ItemModification GenerateItemModification ()
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Genera un <see cref="ItemModifier"/>
		/// </summary>
		public static ItemModifier GenerateItemModifier ()
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Get the Broken modifier
		/// </summary>
		/// <value>The broken.</value>
		public static ItemModifier Broken { get; }

		/// <summary>
		/// Gets a collection with all the modifiers
		/// </summary>
		public static IReadOnlyCollection<ItemModifier> Modifiers;

		static ItemModifierGenerator ()
		{
			var mods = new List<ItemModifier> ();
			var lowAtt = new ItemModification (ConstantesAtributos.Ataque, -1.2f);
			var lowHit = new ItemModification (ConstantesAtributos.Hit, -0.1f);
			var brokenMod = new ItemModifier (
				                "broken",
				                ItemModifierNameUsage.Prefix,
				                new [] { lowAtt, lowHit });

			Broken = brokenMod;
			mods.Add (Broken);

			Modifiers = mods.AsReadOnly ();
		}
	}
}
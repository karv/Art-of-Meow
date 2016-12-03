using System;

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

		public static ItemModifier GenerateItemModifier (string name)
		{
			// THINK: ¿convertirlo en una colección de getters estáticos?
			switch (name)
			{
				case "broken":
					var lowAtt = new ItemModification (ConstantesAtributos.Ataque, -1.2f);
					var lowHit = new ItemModification (ConstantesAtributos.Hit, 3f);
					var brokenMod = new ItemModifier (
						                "broken",
						                ItemModifierNameUsage.Prefix,
						                new [] { lowAtt, lowHit });

					return brokenMod;
			}

			throw new Exception ();
		}
	}
}
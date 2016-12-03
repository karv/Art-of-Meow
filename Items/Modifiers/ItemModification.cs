using System;


namespace Items.Modifiers
{
	public struct ItemModification
	{
		public string AttributeChangeName { get; }

		public float Delta { get; }

		public ItemModification (string attrChangeName, float delta)
		{
			AttributeChangeName = attrChangeName;
			Delta = delta;
		}

		public static ItemModification operator + (ItemModification left,
		                                           ItemModification right)
		{
			if (left.AttributeChangeName != right.AttributeChangeName)
				throw new InvalidOperationException ();
			return new ItemModification (
				left.AttributeChangeName,
				left.Delta + right.Delta);
		}
	}
}
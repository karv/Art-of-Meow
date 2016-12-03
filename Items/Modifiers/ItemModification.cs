
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
	}
}
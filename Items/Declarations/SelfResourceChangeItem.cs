using Skills;
using Units;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace Items.Declarations
{
	public class SelfResourceChangeItem : UsableItem
	{
		/// <summary>
		/// Resource quantity delta
		/// </summary>
		public float QtDelta { get; set; }

		public string ResourceName { get; set; }

		public bool ShowDeltaInGUI;

		protected override bool IsCastable (IUnidad user)
		{
			return true;
		}

		protected override bool IsVisible (IUnidad user)
		{
			return true;
		}

		override protected void GetInstance (IUnidad user)
		{
			{
				LastGeneratedInstance = new SkillInstance (this, user);
				LastGeneratedInstance.Effects.AddEffect (
					new RemoveItemEffect (user, user, this));
				LastGeneratedInstance.Effects.AddEffect (
					new ChangeRecurso (user, user, ResourceName, QtDelta){ ShowDeltaLabel = ShowDeltaInGUI });
			}
		}

		public override object Clone ()
		{
			return new SelfResourceChangeItem (NombreBase, QtDelta, ResourceName, TextureName)
			{
				ShowDeltaInGUI = ShowDeltaInGUI
			};
		}

		[JsonConstructor]
		public SelfResourceChangeItem (string NombreBase, float QtDelta, string ResourceName, string Texture)
			: base (NombreBase)
		{
			this.QtDelta = QtDelta;
			this.ResourceName = ResourceName;
			Color = Color.White;
			TextureName = Texture;
		}
	}
}
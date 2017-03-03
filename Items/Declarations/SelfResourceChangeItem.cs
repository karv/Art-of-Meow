using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Skills;
using Units;

namespace Items.Declarations
{
	/// <summary>
	/// An Item that can be used to change a resource of the user
	/// </summary>
	public class SelfResourceChangeItem : UsableItem
	{
		/// <summary>
		/// Resource quantity delta
		/// </summary>
		public float QtDelta { get; set; }

		/// <summary>
		/// Gets or sets the name of the changing resource
		/// </summary>
		public string ResourceName { get; set; }

		/// <summary>
		/// Determines if the delta value will be shown in GUI
		/// </summary>
		public bool ShowDeltaInGUI;

		/// <summary>
		/// Gets the value.
		/// </summary>
		public override float Value
		{
			get
			{
				return QtDelta + base.Value;
			}
		}

		/// <summary>
		/// Gets the tooltip info (shown in equipment screen
		/// </summary>
		public override string GetTooltipInfo ()
		{
			return string.Format ("{0}+={1}", ResourceName, QtDelta);
		}

		/// <summary>
		/// Determines whether this instance is castable by the specified user.
		/// ie. Is Enabled?
		/// </summary>
		protected override bool IsCastable (IUnidad user)
		{
			return true;
		}

		/// <summary>
		/// Determines whether this instance is visible for the specified user.
		/// </summary>
		protected override bool IsVisible (IUnidad user)
		{
			return true;
		}

		/// <summary>
		/// Gets the instance and stores it in <see cref="UsableItem.LastGeneratedInstance"/>
		/// </summary>
		override protected void GetInstance (IUnidad user)
		{
			{
				LastGeneratedInstance = new SkillInstance (this, user);
				LastGeneratedInstance.Effects.AddEffect (
					new RemoveItemEffect (user, user, this));
				var delta = QtDelta * (Modifiers.GetTotalModificationOf (AttributesNames.ItemEffect) + 1);
				LastGeneratedInstance.Effects.AddEffect (
					new ChangeRecurso (user, user, ResourceName, delta){ ShowDeltaLabel = ShowDeltaInGUI });
				AddCooldownEffect (user, LastGeneratedInstance.Effects);
			}
		}

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override object Clone ()
		{
			return new SelfResourceChangeItem (NombreBase, QtDelta, ResourceName, TextureName, CoolDownBase)
			{
				ShowDeltaInGUI = ShowDeltaInGUI,
				AllowedModNames = AllowedModNames
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.SelfResourceChangeItem"/> class.
		/// </summary>
		[JsonConstructor]
		public SelfResourceChangeItem (string NombreBase, float QtDelta, string ResourceName, string Texture, float Cooldown)
			: base (NombreBase, Cooldown)
		{
			this.QtDelta = QtDelta;
			this.ResourceName = ResourceName;
			Color = Color.White;
			TextureName = Texture;
		}
	}
}
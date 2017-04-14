using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Newtonsoft.Json;
using Skills;
using Units;
using Units.Recursos;

namespace Items.Declarations.Equipment.Skills
{
	/// <summary>
	/// First aid skill.
	/// </summary>
	public class FirstAidSkill : ISkill
	{
		/// <summary>
		/// Healing power
		/// </summary>
		public readonly float BasePower;
		/// <summary>
		/// The base cooldown.
		/// </summary>
		public readonly float BaseCooldown;
		/// <summary>
		/// The name of the skill
		/// </summary>
		public readonly string Name;
		/// <summary>
		/// Texture name of the icon
		/// </summary>
		public readonly string TextureName;

		#region ISkill implementation

		/// <summary>
		/// Devuelve la última instancia generada.
		/// </summary>
		public SkillInstance LastGeneratedInstance { get; protected set; }

		/// <summary>
		/// Build a skill instance
		/// </summary>
		/// <param name="user">User.</param>
		public void GetInstance (IUnidad user)
		{
			LastGeneratedInstance = new SkillInstance (this, user);
			var effect = new ChangeRecurso (user, user, ConstantesRecursos.HP, BasePower);
			var cdEffect = new GenerateCooldownEffect (user, user, BaseCooldown);
			LastGeneratedInstance.Effects.AddEffect (effect);
			LastGeneratedInstance.Effects.AddEffect (cdEffect);
		}

		/// <summary>
		/// Determines whether this skill is castable by the specified user.
		/// </summary>
		public bool IsCastable (IUnidad user)
		{
			return true;
		}

		/// <summary>
		/// Determines whether this instance is visible by the specified user.
		/// </summary>
		public bool IsVisible (IUnidad user)
		{
			return true;
		}

		/// <summary>
		/// Gets the value of the skill
		/// </summary>
		public float Value { get { return 10; } }

		/// <summary>
		/// Gets a value indicating whether this instance is learnable.
		/// </summary>
		public bool IsLearnable { get { return true; } }

		/// <summary>
		/// Gets the required skills.
		/// </summary>
		public string[] RequiredSkills { get { return new string[]{ }; } }

		#endregion

		#region IDibujable implementation

		/// <summary>
		/// Dibuja el objeto sobre un rectángulo específico
		/// </summary>
		void IDibujable.Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (texture, rect, Color.White);
		}

		#endregion

		#region IComponent implementation

		Texture2D texture;

		// TODO: as readonly
		const string textureName = "heal";

		/// <summary>
		/// Loads the content using a given manager
		/// </summary>
		public void LoadContent (ContentManager manager)
		{
			texture = manager.Load<Texture2D> (textureName);
		}

		#endregion

		#region IGameComponent implementation

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public void Initialize ()
		{
		}

		#endregion

		#region IIdentificable implementation

		string AoM.IIdentificable.Name { get { return Name; } }

		#endregion

		[JsonConstructor]
		FirstAidSkill (string Name, float Power, float Cooldown, string Texture)
		{
			if (Texture == null)
				throw new System.ArgumentNullException ("Texture");
			if (Name == null)
				throw new System.ArgumentNullException ("Name");
			
			this.Name = Name;
			BasePower = Power;
			BaseCooldown = Cooldown;
			TextureName = Texture;
		}
	}
}
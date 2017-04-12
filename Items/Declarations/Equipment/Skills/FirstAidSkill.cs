using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Skills;
using Units;
using Units.Recursos;

namespace Items.Declarations.Equipment.Skills
{
	public class FirstAidSkill : ISkill
	{
		public readonly float BasePower;
		public readonly float BaseCooldown;
		public readonly string Name;
		public readonly string TextureName;

		#region ISkill implementation

		/// <summary>
		/// Devuelve la última instancia generada.
		/// </summary>
		public SkillInstance LastGeneratedInstance { get; protected set; }

		public void GetInstance (IUnidad user)
		{
			LastGeneratedInstance = new SkillInstance (this, user);
			var effect = new ChangeRecurso (user, user, ConstantesRecursos.HP, BasePower);
			var cdEffect = new GenerateCooldownEffect (user, user, BaseCooldown);
			LastGeneratedInstance.Effects.AddEffect (effect);
			LastGeneratedInstance.Effects.AddEffect (cdEffect);
		}

		public bool IsCastable (IUnidad user)
		{
			return true;
		}

		public bool IsVisible (IUnidad user)
		{
			return true;
		}

		public float Value { get { return 10; } }

		public bool IsLearnable { get { return true; } }

		public string[] RequiredSkills { get { return new string[]{ }; } }

		#endregion

		#region IDibujable implementation

		public void Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (texture, rect, Color.White);
		}

		#endregion

		#region IComponent implementation

		Texture2D texture;

		const string textureName = "heal";

		public void LoadContent (ContentManager manager)
		{
			texture = manager.Load<Texture2D> (textureName);
		}

		#endregion

		#region IGameComponent implementation

		public void Initialize ()
		{
		}

		#endregion

		#region IIdentificable implementation

		string AoM.IIdentificable.Name { get { return Name; } }

		#endregion

		[JsonConstructor]
		public FirstAidSkill (string Name, float Power, float Cooldown, string Texture)
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
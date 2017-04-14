using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Newtonsoft.Json;
using Skills;
using Units;
using Units.Buffs;
using Units.Recursos;

namespace Items.Declarations.Equipment.Skills
{
	/// <summary>
	/// A skill taht generates a buff
	/// </summary>
	public class BuffSkill : ISkill
	{
		/// <summary>
		/// Texture for the skill icon
		/// </summary>
		public readonly string TextureString;
		/// <summary>
		/// Name of the invocation buff
		/// </summary>
		public readonly string BuffName;

		/// <summary>
		/// Colldown
		/// </summary>
		[JsonProperty ("Cooldown")]
		public float Cooldown;
		/// <summary>
		/// Chance
		/// </summary>
		[JsonProperty ("Chance")]
		public float Chance;
		/// <summary>
		/// Obny castable when relative HP is below that this, set to -1 to ignore
		/// </summary>
		[JsonProperty ("RelHpUpperLimit")]
		public float RelHpUpperLimit = -1;

		readonly Dictionary<string, float> deltas;

		#region ISkill implementation

		/// <summary>
		/// Build a skill instance
		/// </summary>
		public void GetInstance (IUnidad user)
		{
			LastGeneratedInstance = new SkillInstance (this, user);
			var buff = new StatsBuff (deltas, BuffName, TextureString);
			var eff = new AddBuffEffect (buff, user){ Chance = Chance };
			var cd = new GenerateCooldownEffect (user, user, Cooldown);
			LastGeneratedInstance.Effects.AddEffect (eff);
			LastGeneratedInstance.Effects.AddEffect (cd, true);
		}

		/// <summary>
		/// Determines whether this skill is castable by the specified user.
		/// </summary>
		public bool IsCastable (IUnidad user)
		{
			if (RelHpUpperLimit == -1)
				return true;
			var hp = (RecursoHP)(user.Recursos.GetRecurso (ConstantesRecursos.HP));
			return hp.RelativeHp < RelHpUpperLimit;
		}

		/// <summary>
		/// Determines whether this instance is visible by the specified user.
		/// </summary>
		public bool IsVisible (IUnidad user)
		{
			return true;
		}

		/// <summary>
		/// Devuelve la última instancia generada.
		/// </summary>
		[JsonIgnore]
		public SkillInstance LastGeneratedInstance { get; private set; }

		/// <summary>
		/// Gets the value of the skill
		/// </summary>
		/// <value>The value.</value>
		[JsonProperty ("Value")]
		public float Value { get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance is learnable.
		/// </summary>
		/// <value><c>true</c> if this instance is learnable; otherwise, <c>false</c>.</value>
		[JsonProperty ("Learnable")]
		public bool IsLearnable { get; set; }

		/// <summary>
		/// The required skills
		/// </summary>
		/// <value>The required skills.</value>
		[JsonProperty ("RequiredSkills")]
		public string[] RequiredSkills { get; set; }

		#endregion

		#region IDibujable implementation

		Texture2D texture;

		/// <summary>
		/// Dibuja el icono sobre un rectángulo específico
		/// </summary>
		void IDibujable.Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (texture, rect, Color.White);
		}

		#endregion

		#region IComponent implementation

		/// <summary>
		/// Loads the content using a given manager
		/// </summary>
		/// <param name="manager">Manager.</param>
		public void LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			texture = manager.Load<Texture2D> (TextureString);
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

		/// <summary>
		/// Gets the unique name
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; }

		#endregion

		[JsonConstructor]
		BuffSkill (string Name, 
		           string Texture,
		           string BuffName,
		           Dictionary<string, float> Stats)
		{
			this.Name = Name;
			TextureString = Texture;
			this.BuffName = BuffName;
			deltas = Stats;
			IsLearnable = false;
			Chance = 1;
			RequiredSkills = new string[]{ };
		}
	}
}
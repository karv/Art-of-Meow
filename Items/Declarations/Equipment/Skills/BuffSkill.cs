using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Skills;
using Units;
using Units.Buffs;
using Units.Recursos;

namespace Items.Declarations.Equipment.Skills
{
	public class BuffSkill : ISkill
	{
		public readonly string TextureString;
		public readonly string BuffName;
		[JsonProperty ("Cooldown")]
		public float Cooldown;
		[JsonProperty ("Chance")]
		public float Chance;
		[JsonProperty ("RelHpUpperLimit")]
		public float RelHpUpperLimit = -1;

		readonly Dictionary<string, float> deltas;

		#region ISkill implementation

		public void GetInstance (IUnidad user)
		{
			LastGeneratedInstance = new SkillInstance (this, user);
			var buff = new StatsBuff (deltas, BuffName, TextureString);
			var eff = new AddBuffEffect (buff, user){ Chance = this.Chance };
			var cd = new GenerateCooldownEffect (user, user, Cooldown);
			LastGeneratedInstance.Effects.AddEffect (eff);
			LastGeneratedInstance.Effects.AddEffect (cd, true);
		}

		public bool IsCastable (IUnidad user)
		{
			if (RelHpUpperLimit == -1)
				return true;
			var hp = (RecursoHP)(user.Recursos.GetRecurso (ConstantesRecursos.HP));
			return hp.RelativeHp < RelHpUpperLimit;
		}

		public bool IsVisible (IUnidad user)
		{
			return true;
		}

		[JsonIgnore]
		public SkillInstance LastGeneratedInstance { get; private set; }

		[JsonProperty ("Value")]
		public float Value { get; set; }

		[JsonProperty ("Learnable")]
		public bool IsLearnable { get; set; }

		[JsonProperty ("RequiderSkills")]
		public string[] RequiredSkills { get; set; }

		#endregion

		#region IDibujable implementation

		Texture2D texture;

		public void Draw (Microsoft.Xna.Framework.Graphics.SpriteBatch bat, Microsoft.Xna.Framework.Rectangle rect)
		{
			bat.Draw (texture, rect, Color.White);
		}

		#endregion

		#region IComponent implementation

		public void LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			texture = manager.Load<Texture2D> (TextureString);
		}

		#endregion

		#region IGameComponent implementation

		public void Initialize ()
		{
		}

		#endregion

		#region IIdentificable implementation

		public string Name { get; }

		#endregion

		[JsonConstructor]
		public BuffSkill (string Name, 
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
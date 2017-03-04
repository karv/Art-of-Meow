﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Skills;
using Units.Order;
using Units.Recursos;

namespace Units.Skills
{
	/// <summary>
	/// Skill that allows the user to heal ?-self
	/// </summary>
	public class SelfHealSkill : ISkill
	{
		/// <summary>
		/// Gets the unique name
		/// </summary>
		public  string Name { get; set; }

		#region ISkill implementation

		const float _heal = 10f;
		const float _mpCost = 3f;

		float ISkill.Value //TODO
		{ get { return 100; } }

		/// <summary>
		/// Devuelve la última instancia generada.
		/// </summary>
		/// <value>The last generated instance.</value>
		public SkillInstance LastGeneratedInstance { get; protected set; }

		/// <summary>
		/// Build a skill instance, and should be set in <see cref="LastGeneratedInstance"/>
		/// </summary>
		/// <param name="user">User of the skill</param>
		public void GetInstance (IUnidad user)
		{
			var ret = new SkillInstance (this, user);
			ret.Effects.AddEffect (new ChangeRecurso (
				user,
				user,
				ConstantesRecursos.MP,
				_mpCost));
			ret.Effects.AddEffect (new ChangeRecurso (
				user,
				user,
				ConstantesRecursos.HP,
				_heal));
			LastGeneratedInstance = ret;
		}

		/// <summary>
		/// Executes this skill
		/// </summary>
		/// <param name="user">The caster</param>
		public void Execute (IUnidad user)
		{
			user.EnqueueOrder (new ExecuteOrder (user, doEffect));
			Executed?.Invoke (this, EventArgs.Empty);
		}

		static void doEffect (IUnidad target)
		{
			var hpRec = target.Recursos.GetRecurso (ConstantesRecursos.HP);
			hpRec.Valor += _heal;
		}

		/// <summary>
		/// Determines whether this skill is castable by the specified user.
		/// </summary>
		/// <returns><c>true</c> if this skill is castable by the specified user; otherwise, <c>false</c>.</returns>
		/// <param name="user">User</param>
		public bool IsCastable (IUnidad user)
		{
			var mpRec = user.Recursos.GetRecurso (ConstantesRecursos.MP);
			return mpRec.Valor >= _mpCost;
		}

		/// <summary>
		/// Determines whether this instance is visible to the specified <see cref="IUnidad"/>
		/// </summary>
		/// <returns><c>true</c> if this instance is visible by the specified user; otherwise, <c>false</c></returns>
		/// <param name="user">User</param>
		public bool IsVisible (IUnidad user)
		{
			return true;
		}

		public bool IsLearnable { get { return true; } }

		public string[] RequieredSkills{ get { return new string[]{ }; } }

		/// <summary>
		/// Occurs when the eexecution finishes completly
		/// </summary>
		public event EventHandler Executed;

		#endregion

		#region IComponent implementation

		Texture2D _texture;

		const string TextureName = "heal";

		/// <summary>
		/// Carga la textura
		/// </summary>
		public void LoadContent (ContentManager manager)
		{
			_texture = manager.Load<Texture2D> (TextureName);
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

		#region IDibujable implementation

		/// <summary>
		/// Draws the icon on a rectangle
		/// </summary>
		/// <param name="bat">Batch</param>
		/// <param name="rect">Rectángulo de dibujo</param>
		public void Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (_texture, rect, Color.Red);
		}

		#endregion
	}
}
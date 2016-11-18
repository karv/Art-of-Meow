using Units.Skills;
using System.Collections.Generic;
using Units;
using Items;
using System;
using Skills;
using Units.Recursos;


namespace Skills
{
	/// <summary>
	/// Un objeto que puede producir un <see cref="IEffect"/>
	/// </summary>
	public interface IEffectAgent
	{
		
	}

	/// <summary>
	/// Un target de un <see cref="ITargetEffect"/>
	/// </summary>
	public interface ITarget
	{
		
	}

	/// <summary>
	/// Un efecto de un objeto.
	/// </summary>
	public interface IEffect
	{
		IEffectAgent Agent { get; }

		/// <summary>
		/// Runs the effect
		/// </summary>
		void Execute ();
	}

	/// <summary>
	/// Un efecto que tiene un target
	/// </summary>
	public interface ITargetEffect : IEffect
	{
		/// <summary>
		/// Gets the target of the effect
		/// </summary>
		/// <value>The target.</value>
		ITarget Target { get; }
	}

	public class SkillInstance
	{
		public ISkill Skill { get; }

		public IEffectAgent Agent { get; }

		readonly List<IEffect> _effects;

		public void AddEffect (IEffect effect)
		{
			_effects.Add (effect);
		}

		public void Execute ()
		{
			for (int i = 0; i < _effects.Count; i++)
				_effects [i].Execute ();
		}

		public SkillInstance (ISkill skill, IEffectAgent agent)
		{
			Skill = skill;
			Agent = agent;
			_effects = new List<IEffect> ();
		}
	}

	public class RemoveItemEffect : ITargetEffect
	{
		public void Execute ()
		{
			// Removes the item, it it does not exists: exception
			if (!Target.Inventory.Items.Remove (RemovingItem))
				throw new Exception ("Cannot execute effect");
		}

		/// <summary>
		/// Whose inventory gonna lose the item.
		/// </summary>
		public IUnidad Target { get; }

		ITarget ITargetEffect.Target { get { return Target; } }

		/// <summary>
		/// Item to remove
		/// </summary>
		public IItem RemovingItem { get; }

		/// <summary>
		/// What caused this effect
		/// </summary>
		public IEffectAgent Agent { get; }

		public RemoveItemEffect (IEffectAgent source,
		                         IUnidad target,
		                         IItem removingItem)
		{
			Agent = source;
			Target = target;
			RemovingItem = removingItem;
		}
	}

	public class ChangeRecurso : ITargetEffect
	{
		public void Execute ()
		{
			if (Parámetro == null)
				// Efecto es en recurso
				TargetRecurso.Valor += DeltaValor;
			else
				// Efecto es en parámetro
				Parámetro.Valor += DeltaValor;
		}

		public IRecurso TargetRecurso { get; }

		public IParámetroRecurso Parámetro { get; }

		public IUnidad Target { get; }

		ITarget ITargetEffect.Target { get { return Target; } }

		public IEffectAgent Agent { get; }

		public float DeltaValor { get; }

		public ChangeRecurso (IEffectAgent agent,
		                      IUnidad target,
		                      string recNombre,
		                      string recParámetro, 
		                      float deltaValor)
		{
			Agent = agent;
			Target = target;
			TargetRecurso = target.Recursos.GetRecurso (recNombre);
			Parámetro = TargetRecurso.ValorParámetro (recParámetro);
			DeltaValor = deltaValor;
		}

		public ChangeRecurso (IEffectAgent agent,
		                      IUnidad target,
		                      string recNombre, 
		                      float deltaValor)
		{
			Agent = agent;
			Target = target;
			TargetRecurso = target.Recursos.GetRecurso (recNombre);
			DeltaValor = deltaValor;
		}
	}
}
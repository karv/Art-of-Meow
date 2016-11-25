using System;
using System.Collections.Generic;
using Items;
using Skills;
using Units;
using Units.Recursos;
using Units.Skills;


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
		/// <summary>
		/// Quien causa el efecto
		/// </summary>
		IEffectAgent Agent { get; }

		/// <summary>
		/// Probabilidad de que ocurra
		/// </summary>
		double Chance { get; }

		/// <summary>
		/// Runs the effect
		/// </summary>
		void Execute ();

		/// <summary>
		/// Devuelve un <c>string</c> de una línea que describe este efecto como infobox
		/// </summary>
		string DetailedInfo ();
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

	/// <summary>
	/// Una instancia de skill.
	/// Se usa como feedback de datos y resultados del skill
	/// </summary>
	public class SkillInstance
	{
		/// <summary>
		/// Tipo de skill
		/// </summary>
		public ISkill Skill { get; }

		/// <summary>
		/// El agente o usuario del skill
		/// </summary>
		public IEffectAgent Agent { get; }

		readonly List<IEffect> _effects;

		/// <summary>
		/// Añadir un efecto
		/// </summary>
		public void AddEffect (IEffect effect)
		{
			_effects.Add (effect);
		}

		/// <summary>
		/// Ejecuta
		/// </summary>
		public void Execute ()
		{
			for (int i = 0; i < _effects.Count; i++)
				_effects [i].Execute ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.SkillInstance"/> class.
		/// </summary>
		/// <param name="skill">Skill.</param>
		/// <param name="agent">Agent.</param>
		public SkillInstance (ISkill skill, IEffectAgent agent)
		{
			Skill = skill;
			Agent = agent;
			_effects = new List<IEffect> ();
		}
	}

	/// <summary>
	/// Representa el efecto de un <see cref="ISkill"/> que consisnte en eliminar un item de un <see cref="IUnidad"/>
	/// </summary>
	public class RemoveItemEffect : ITargetEffect
	{
		/// <summary>
		/// Runs the effect
		/// </summary>
		public void Execute ()
		{
			// Removes the item, it it does not exists: exception
			if (!Target.Inventory.Items.Remove (RemovingItem))
				throw new Exception ("Cannot execute effect");
		}

		/// <summary>
		/// Devuelve un <c>string</c> de una línea que describe este efecto como infobox
		/// </summary>
		public string DetailedInfo ()
		{
			return string.Format ("{1}: Removes {0}", RemovingItem.Nombre, Chance);
		}

		/// <summary>
		/// Probabilidad de que ocurra
		/// </summary>
		public double Chance { get; set; }

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

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.RemoveItemEffect"/> class.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="target">Target.</param>
		/// <param name="removingItem">Removing item.</param>
		public RemoveItemEffect (IEffectAgent source,
		                         IUnidad target,
		                         IItem removingItem)
		{
			Agent = source;
			Target = target;
			RemovingItem = removingItem;
		}
	}

	/// <summary>
	/// Un efecto de cambio de recurso en un target
	/// </summary>
	public class ChangeRecurso : ITargetEffect
	{
		double _chance;

		/// <summary>
		/// Probabilidad de que ocurra
		/// </summary>
		/// <value>The chance.</value>
		public double Chance
		{
			get
			{
				return _chance;
			}
			set
			{
				_chance = value;
			}
		}

		/// <summary>
		/// Devuelve un <c>string</c> de una línea que describe este efecto como infobox
		/// </summary>
		public string DetailedInfo ()
		{
			if (Parámetro == null)
				return string.Format (
					"{3}: {0}'s {1} changed by {2}.",
					Target,
					TargetRecurso.NombreLargo,
					DeltaValor,
					Chance);
			return string.Format (
				"{4}: {0}'s {1}'s {2} changed by {3}.",
				Target,
				TargetRecurso.NombreLargo,
				Parámetro.NombreÚnico,
				DeltaValor,
				Chance);
		}

		/// <summary>
		/// Runs the effect
		/// </summary>
		public void Execute ()
		{
			if (Parámetro == null)
				// Efecto es en recurso
				TargetRecurso.Valor += DeltaValor;
			else
				// Efecto es en parámetro
				Parámetro.Valor += DeltaValor;
		}

		/// <summary>
		/// El recurso que es afectado.
		/// El valor de este recurso no es afectado, al menos que <see cref="Parámetro"/> sea <c>null</c>
		/// </summary>
		public IRecurso TargetRecurso { get; }

		/// <summary>
		/// Devuelve el parámetro del recurso afectado; 
		/// si es <c>null</c>, se afecta directamente a <see cref="TargetRecurso"/>
		/// </summary>
		/// <value>The parámetro.</value>
		public IParámetroRecurso Parámetro { get; }

		/// <summary>
		/// Devuelve la unidad que es afectada
		/// </summary>
		public IUnidad Target { get; }

		ITarget ITargetEffect.Target { get { return Target; } }

		/// <summary>
		/// Devuelve quien causa el efecto.
		/// </summary>
		public IEffectAgent Agent { get; }

		/// <summary>
		/// Devuelve la diferencia de valor que causa el recurso
		/// </summary>
		public float DeltaValor { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.ChangeRecurso"/> class.
		/// </summary>
		/// <param name="agent">Agent.</param>
		/// <param name="target">Target.</param>
		/// <param name="recNombre">Nombre del recurso</param>
		/// <param name="recParámetro">Nombre del parámetro</param>
		/// <param name="deltaValor">Cambio de valor</param>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.ChangeRecurso"/> class.
		/// </summary>
		/// <param name="agent">Agent.</param>
		/// <param name="target">Target.</param>
		/// <param name="recNombre">Nombre del recurso</param>
		/// <param name="deltaValor">Cambio de valor</param>
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
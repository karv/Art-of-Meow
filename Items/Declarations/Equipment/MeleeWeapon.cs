using System.Diagnostics;
using Helper;
using Skills;
using Units;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Representa una arma melee genérica
	/// </summary>
	public class MeleeWeapon : Equipment, IMeleeEffect
	{
		/// <summary>
		/// Devuelve el daño base
		/// </summary>
		/// <value>The base damage.</value>
		public float BaseDamage { get; }

		public float Damage ()
		{
			return BaseDamage + Modifiers.GetTotalModificationOf (ConstantesAtributos.Ataque);
		}

		/// <summary>
		/// Devuelve la certeza base
		/// </summary>
		/// <value>The base hit.</value>
		public float BaseHit { get; }

		public float Hit ()
		{
			return BaseHit + Modifiers.GetTotalModificationOf (ConstantesAtributos.Ataque);
		}

		#region IEquipment & Melee

		/// <summary>
		/// Gets the equipment slot.
		/// </summary>
		/// <value>The slot.</value>
		public override EquipSlot Slot { get { return EquipSlot.MainHand; } }

		/// <summary>
		/// Causes melee effect on a target
		/// </summary>
		/// <param name="user">The user of the melee move</param>
		/// <param name="target">Target.</param>
		public virtual IEffect GetEffect (IUnidad user, IUnidad target)
		{
			var ret = MeleeEffectHelper.BuildDefaultMeleeEffect (
				          user,
				          target,
				          Damage (),
				          Hit ());

			Debug.WriteLine (
				string.Format (
					"Melee effect from {0} to {1} causing\n{2}",
					user,
					target,
					ret.DetailedInfo ())
			);

			return ret;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.Equipment.MeleeWeapon"/> class.
		/// </summary>
		/// <param name="nombre">Nombre.</param>
		/// <param name="icon">Icon.</param>
		/// <param name = "baseDamage">Base melee damage</param>
		/// <param name = "baseHit">Base melee hit%</param>
		public MeleeWeapon (string nombre,
		                    string icon,
		                    float baseDamage,
		                    float baseHit)
			: base (nombre)
		{
			TextureName = icon;
			BaseDamage = baseDamage;
			BaseHit = baseHit;
		}
	}
}
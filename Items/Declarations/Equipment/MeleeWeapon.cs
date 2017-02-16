using System.Diagnostics;
using Debugging;
using Helper;
using Skills;
using Units;
using Newtonsoft.Json;

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
		public float BaseDamage { get; set; }

		/// <summary>
		/// Calcula el daño neto
		/// </summary>
		public float Damage ()
		{
			return BaseDamage + Modifiers.GetTotalModificationOf (AttributesNames.Ataque);
		}

		/// <summary>
		/// Devuelve la certeza base
		/// </summary>
		/// <value>The base hit.</value>
		public float BaseHit { get; set; }

		/// <summary>
		/// Calcula el hit chance neto
		/// </summary>
		public float Hit ()
		{
			return BaseHit + Modifiers.GetTotalModificationOf (AttributesNames.Hit);
		}

		/// <summary>
		/// Devuelve o establece la rapidez de uso (melee) del arma
		/// </summary>
		public float BaseSpeed { get; set; }

		/// <summary>
		/// Calcula el hit chance neto
		/// </summary>
		public float Speed ()
		{
			return BaseSpeed + Modifiers.GetTotalModificationOf (AttributesNames.Speed);
		}

		public override float Value
		{
			get
			{
				return base.Value + BaseSpeed * BaseDamage * BaseHit;
			}
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
				          Hit (), false);

			ret.AddEffect (
				new GenerateCooldownEffect (
					user,
					user,
					MeleeEffectHelper.CalcularTiempoMelee (user) / Speed ()),
				true);
			
			Debug.WriteLine (
				string.Format (
					"Melee effect from {0} to {1} causing\n{2}",
					user,
					target,
					ret.DetailedInfo ()), 
				DebugCategories.MeleeResolution
			);

			return ret;
		}

		#endregion

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override object Clone ()
		{
			return new MeleeWeapon (NombreBase, TextureName, BaseDamage, BaseHit)
			{
				Texture = Texture,
				TextureName = TextureName,
				BaseSpeed = BaseSpeed,
				Color = Color
			};
		}

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
			BaseSpeed = 1;
		}

		[JsonConstructor]
		MeleeWeapon (string NombreBase, string TextureName)
			: base (NombreBase)
		{
			this.TextureName = TextureName;
		}
	}
}
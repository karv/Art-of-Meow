using System;
using Units;
using Units.Order;
using Units.Recursos;
using Units.Skills;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Obsolete
	/// </summary>
	[ObsoleteAttribute]
	public class HealingSword : Sword, ISkillEquipment
	{
		readonly SelfHealSkill healSkill;

		/// <summary>
		/// Loads the texture
		/// </summary>
		protected override void AddContent ()
		{
			base.AddContent ();
			healSkill.AddContent ();
		}

		/// <summary>
		/// Initializes the content.
		/// </summary>
		protected override void InitializeContent ()
		{
			base.InitializeContent ();
			healSkill.InitializeContent ();
		}

		System.Collections.Generic.IEnumerable<ISkill> ISkillEquipment.GetEquipmentSkills ()
		{
			return new ISkill[] { healSkill };
		}

		/// <summary>
		/// </summary>
		public HealingSword ()
		{
			healSkill = new SelfHealSkill ();
		}
	}

	/// <summary>
	/// Una espada sin propiedades especiales
	/// </summary>
	public class Sword : Equipment, IMeleeEffect
	{
		#region IEquipment implementation

		/// <summary>
		/// Gets the equipment slot.
		/// </summary>
		/// <value>The slot.</value>
		public override EquipSlot Slot { get { return EquipSlot.MainHand; } }

		#endregion

		/// <summary>
		/// Causes melee effect on a target
		/// </summary>
		/// <param name="user">The user of the melee move</param>
		/// <param name="target">Target.</param>
		public void DoMeleeEffectOn (IUnidad user, IUnidad target)
		{
			var damage = user.Recursos.ValorRecurso (ConstantesRecursos.Fuerza) * 0.125f +
			             user.Recursos.ValorRecurso (ConstantesRecursos.Destreza) * 0.05f;
			user.EnqueueOrder (new MeleeDamageOrder (user, target, damage));
			user.EnqueueOrder (new CooldownOrder (
				user,
				calcularTiempoMelee ()));
		}

		float calcularTiempoMelee ()
		{
			var dex = UnidadOwner.Recursos.ValorRecurso (ConstantesRecursos.Destreza);
			return 1 / dex;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.Equipment.Sword"/> class.
		/// </summary>
		/// <param name="nombre">Nombre.</param>
		/// <param name="icon">Icon.</param>
		protected Sword (string nombre, string icon)
			: base (nombre)
		{
			TextureName = icon;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.Equipment.Sword"/> class.
		/// </summary>
		public Sword ()
			: this ("Sword", @"Items/katana")
		{
			Color = Microsoft.Xna.Framework.Color.Black;

		}
	}
}
using Units;
using Units.Buffs;
using Units.Order;
using Units.Recursos;
using Units.Skills;
using System;
using Moggle;

namespace Items.Declarations.Equipment
{
	[ObsoleteAttribute]
	public class HealingSword : Sword, ISkillEquipment
	{
		readonly SelfHealSkill healSkill = new SelfHealSkill ();

		/// <summary>
		/// Loads the texture
		/// </summary>
		/// <param name="manager">Manager.</param>
		protected override void AddContent (BibliotecaContenido manager)
		{
			base.AddContent (manager);
			healSkill.AddContent (manager);
		}

		/// <summary>
		/// Initializes the content.
		/// </summary>
		/// <param name="manager">Manager.</param>
		protected override void InitializeContent (BibliotecaContenido manager)
		{
			base.InitializeContent (manager);
			healSkill.InitializeContent (manager);
		}

		System.Collections.Generic.IEnumerable<ISkill> ISkillEquipment.GetEquipmentSkills ()
		{
			return new ISkill[] { healSkill };
		}
	}

	/// <summary>
	/// Una espada sin propiedades especiales
	/// </summary>
	public class Sword : Equipment, IBuffGenerating, IMeleeEffect
	{
		/// <summary>
		/// Enumera los stats y la cantidad que son modificados.
		/// </summary>
		/// <returns>The delta stat.</returns>
		public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, float>> GetDeltaStat ()
		{
			yield return new System.Collections.Generic.KeyValuePair<string, float> (
				ConstantesRecursos.DañoMelee,
				3);
		}

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
			user.EnqueueOrder (new MeleeDamageOrder (user, target));
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
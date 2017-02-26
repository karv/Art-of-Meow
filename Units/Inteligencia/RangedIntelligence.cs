using System.Linq;
using System;
using Items.Declarations.Equipment.Skills;
using Units.Order;

namespace Units.Inteligencia
{
	public class RangedIntelligence : AI
	{
		Unidad Target;

		Unidad GetTarget ()
		{
			return MapGrid.Objects.OfType<Unidad> ().FirstOrDefault (IsSelectableAsTarget);
		}

		void TryUpdateTarget ()
		{
			if (Target == null)
				Target = GetTarget ();
		}

		public int MaxRangeDist = 5;
		RangedSkill skill;

		RangedSkill getSkill ()
		{
			try
			{
				return ControlledUnidad.EnumerateAllSkills ().OfType<RangedSkill> ().First ();
				
			}
			catch (Exception ex)
			{
				throw new AIException ("Ranged AI cannot access ranged skill.", ex);
			}
			/*
			var wpn = ControlledUnidad.Equipment.EquipmentInSlot (EquipSlot.MainHand).OfType<ISkillEquipment> ();
			foreach (var w in wpn)
				foreach (var skl in w.GetEquipmentSkills ())
				{
					var rangedSkill = skl as RangedSkill;
					if (rangedSkill != null)
						return rangedSkill;
				}
				*/
		}

		protected override void AfterUnidadLink ()
		{
			skill = getSkill ();
		}

		protected override void DoAction ()
		{
			TryUpdateTarget ();
			if (Helper.Geometry.SquaredEucludeanDistance (ControlledUnidad.Location, Target.Location) <
			    Math.Pow (MaxRangeDist, 2))
			{
				if (ControlledUnidad.VisiblePoints ().Contains (Target.Location))
				{
					//Shot
					var instance = skill.BuildSkillInstance (ControlledUnidad, Target);
					instance.Execute ();
				}
				else
					ControlledUnidad.EnqueueOrder (new CooldownOrder (ControlledUnidad, 0.1f));
			}
			else
			{
				// get closer
				var dir = ControlledUnidad.Location.GetDirectionTo (Target.Location);
				if (!ControlledUnidad.MoveOrMelee (dir))
				{
					// No se puede moverse ni atacar hacia acá.
					// Solamente voy a esperar un poco
					ControlledUnidad.EnqueueOrder (new CooldownOrder (ControlledUnidad, 0.1f));
				}
			}
			//throw new NotImplementedException ();
		}
	}
}
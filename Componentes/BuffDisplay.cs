using Moggle.Controles;
using Units;
using System.Linq;

namespace Componentes
{
	public class BuffDisplay : Contenedor<IDibujable>
	{
		public IUnidad Unidad;

		/// <summary>
		/// Actualiza la lista de objetos.
		/// Se invoca automáticamente al cambiar los buffs de la unidad.
		/// </summary>
		public void UpdateObjetcs ()
		{
			Objetos = Unidad.Buffs.BuffOfType<IDibujable> ().ToList ();
		}

		/// <summary>
		/// Cancelar suscripción a Buffs
		/// </summary>
		protected override void Dispose ()
		{
			Unidad.Buffs.AddBuff -= updateObjetcs;
			Unidad.Buffs.RemoveBuff -= updateObjetcs;
		}

		void updateObjetcs (object sender, Units.Buffs.IBuff e)
		{
			UpdateObjetcs ();
		}

		public override void Draw (Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Draw (gameTime);
		}

		/// <summary>
		/// Se suscribe a eventos de cambio de Buffs
		/// </summary>
		public BuffDisplay (IComponentContainerComponent<IControl> cont,
		                    IUnidad unit)
			: base (cont)
		{
			Unidad = unit;
			TextureFondoName = "Interface//win_bg";
			UpdateObjetcs ();
			Unidad.Buffs.AddBuff += updateObjetcs;
			Unidad.Buffs.RemoveBuff += updateObjetcs;
		}
	}
}
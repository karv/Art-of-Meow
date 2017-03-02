using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Units;

namespace Componentes
{
	/// <summary>
	/// Un control que muestra buffs de un jugador
	/// </summary>
	public class BuffDisplay : Contenedor<IDibujable>
	{
		/// <summary>
		/// Unidad
		/// </summary>
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
		/// Se ejecuta antes del ciclo, pero después de saber un poco sobre los controladores.
		///  No invoca LoadContent por lo que es seguro agregar componentes
		/// </summary>
		public override void Initialize ()
		{
			Unidad.Buffs.ListChanged += updateObjetcs;
			UpdateObjetcs ();
		}

		/// <summary>
		/// Cancelar suscripción a Buffs
		/// </summary>
		protected override void Dispose ()
		{
			Unidad.Buffs.ListChanged -= updateObjetcs;
		}

		void updateObjetcs (object sender, EventArgs e)
		{
			UpdateObjetcs ();
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
			GridSize = new MonoGame.Extended.Size (3, 3);
			UpdateObjetcs ();
			BgColor = Color.Red * 0.3f;
		}
	}
}
using Moggle.Controles;
using Units;
using System.Linq;
using System;
using Microsoft.Xna.Framework.Content;

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

		public override void Initialize ()
		{
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

		protected override void Draw (Microsoft.Xna.Framework.GameTime gameTime)
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
			GridSize = new MonoGame.Extended.Size (3, 3);
			UpdateObjetcs ();
			Unidad.Buffs.ListChanged += updateObjetcs;
		}
	}
}
using System.Collections.Generic;
using Units.Buffs;

namespace Units.Buffs
{
	/// <summary>
	/// Un StatsBuff genérico
	/// </summary>
	public class StatsBuff : Buff, IStatsBuff
	{
		readonly IDictionary<string, float> deltas;

		/// <summary>
		/// Gets a value indicating if this buff is shown in the list of active buffs
		/// </summary>
		/// <value><c>true</c> if this instance is visible; otherwise, <c>false</c>.</value>
		public override bool IsVisible { get { return false; } }

		/// <summary>
		/// Devuelve la textura a usar
		/// </summary>
		public override string BaseTextureName { get; }

		/// <summary>
		/// Nombre
		/// </summary>
		public override string Nombre { get; }

		/// <summary>
		/// Devuelve la modificación (absoluta) de valor de un recurso.
		/// </summary>
		/// <returns>The delta.</returns>
		/// <param name="resName">Res name.</param>
		public float StatDelta (string resName)
		{
			float ret;
			return deltas.TryGetValue (resName, out ret) ? ret : 0;
		}

		/// <summary>
		/// Establece el valor de un recurso 
		/// </summary>
		public void SetValue (string resName, float value)
		{
			if (deltas.ContainsKey (resName))
				deltas [resName] = value;
			else
				deltas.Add (resName, value);
		}

		/// <summary>
		/// Cambia el valor de un recurso.
		/// </summary>
		public void DeltaValue (string resName, float delta)
		{
			if (deltas.ContainsKey (resName))
				deltas [resName] += delta;
			else
				deltas.Add (resName, delta);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Units.Buffs.StatsBuff"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Units.Buffs.StatsBuff"/>.</returns>
		public override string ToString ()
		{
			return base.ToString () + string.Format ("[StatsBuff: deltas={0}]", deltas);
		}

		/// <summary>
		/// </summary>
		/// <param name="nombre">Nombre.</param>
		/// <param name="textura">Textura.</param>
		public StatsBuff (string nombre, string textura)
		{
			deltas = new Dictionary<string, float> ();
			Nombre = nombre;
			BaseTextureName = textura;
		}

		/// <summary>
		/// </summary>
		/// <param name="delta">El estado inicial</param>
		/// <param name="nombre">Nombre.</param>
		/// <param name="textura">Textura.</param>
		public StatsBuff (IDictionary<string,float> delta,
		                  string nombre,
		                  string textura)
		{
			deltas = delta;
			Nombre = nombre;
			BaseTextureName = textura;
		}
	}
}
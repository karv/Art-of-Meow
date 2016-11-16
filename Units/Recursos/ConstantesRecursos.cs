using System;

namespace Units.Recursos
{
	/// <summary>
	/// Contains constant strings for resource names
	/// </summary>
	public static class ConstantesRecursos
	{
		/// <summary>
		/// Dextery. 
		/// Afecta rapidez de algunos ataques
		/// </summary>
		public const string Destreza = "dex";
		/// <summary>
		/// Velocidad. 
		/// Velocidad de movimiento
		/// </summary>
		public const string Velocidad = "vel";
		/// <summary>
		/// The fuerza.
		/// Afecta el daño melee
		/// </summary>
		public const string Fuerza = "str";
		/// <summary>
		/// The daño melee.
		/// </summary>
		[ObsoleteAttribute]
		public const string DañoMelee = "melee_dmg";
		/// <summary>
		/// The HP
		/// </summary>
		public const string HP = "hp";
		/// <summary>
		/// Magic points
		/// </summary>
		public const string MP = "mp";
		/// <summary>
		/// Certeza de ataques de rango
		/// </summary>
		public const string CertezaRango = "range_prec";
		/// <summary>
		/// Evasión de ataques de rango
		/// </summary>
		public const string EvasiónRango = "range_eva";
	}
}
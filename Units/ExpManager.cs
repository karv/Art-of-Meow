using System;
using System.Collections.Generic;
using Units.Recursos;

namespace Units
{
	/// <summary>
	/// Manejador de experiencia
	/// </summary>
	public class ExpManager
	{
		readonly Dictionary<IParámetroRecurso, float> _distribuciónExp;

		public IUnidad Unidad { get; }

		public float ExperienciaAcumulada { get; set; }

		void _normalizeDistDict ()
		{
			var suma = 0f;
			foreach (var x in _distribuciónExp.Values)
				suma += x;

			if (suma == 0)
				throw new Exception ("Cannot normalize vector zero.");

			foreach (var x in _distribuciónExp.Keys)
				_distribuciónExp [x] /= suma;
		}

		public void Flush ()
		{
			_normalizeDistDict ();
			foreach (var x in _distribuciónExp)
				x.Key.ReceiveExperience (ExperienciaAcumulada * x.Value);
			_distribuciónExp.Clear ();
		}

		public void AddAssignation (IParámetroRecurso par, float cant)
		{
			if (_distribuciónExp.ContainsKey (par))
				_distribuciónExp [par] += cant;
			else
				_distribuciónExp.Add (par, cant);
		}

		public void AddAssignation (IRecurso rec, float cant)
		{
			foreach (var x in rec.EnumerateParameters ())
				AddAssignation (x, cant);
		}


		public ExpManager (IUnidad unid)
		{
			Unidad = unid;
			_distribuciónExp = new Dictionary<IParámetroRecurso, float> ();
		}
	}
}
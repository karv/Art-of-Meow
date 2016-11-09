using System;
using Microsoft.Xna.Framework;

namespace Helper
{
	/// <summary>
	/// Maneja el valor que puede tener un objeto de valor cambiante
	/// </summary>
	public class RetardValue
	{
		float _chSpeed;

		/// <summary>
		/// Devuelve o establece la velocidad (en px/ms) con la que el valor visible puede cambiar
		/// </summary>
		/// <value>The change speed.</value>
		public float ChangeSpeed
		{
			get
			{
				return _chSpeed;
			}
			set
			{
				if (_chSpeed <= 0)
					throw new InvalidOperationException ("Speed must be positive.");
				_chSpeed = value;
			}
		}

		/// <summary>
		/// Devuelve o establece el valor visible
		/// </summary>
		public float VisibleValue { get; set; }

		public void UpdateTo (GameTime gameTime, float realValue)
		{
			// Creo que es mejor hacer esta comparación, ya que es la más común; 
			// es para evitar hacer la comparación < y > en todos estos casos.
			if (realValue == VisibleValue)
				return;
			if (realValue < VisibleValue)
			{
				// Reducir VisibleValue
				VisibleValue = Math.Max (
					realValue,
					VisibleValue - (float)gameTime.ElapsedGameTime.TotalMilliseconds * ChangeSpeed);
				return;
			}
			VisibleValue = Math.Min (
				realValue,
				VisibleValue + (float)gameTime.ElapsedGameTime.TotalMilliseconds * ChangeSpeed);
		}

		public RetardValue ()
		{
			_chSpeed = 1;
		}
	}
}
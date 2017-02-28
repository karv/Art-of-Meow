using System;

namespace Units.Inteligencia
{
	/// <summary>
	/// Exception of the artifical intelligence
	/// </summary>
	[Serializable]
	public class AIException : Exception
	{
		/// <summary>
		/// Unidad that the AI is controlling
		/// </summary>
		public IUnidad unidad;
		/// <summary>
		/// The AI
		/// </summary>
		public AI AI;

		/// <summary>
		/// </summary>
		public AIException ()
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		public AIException (string message)
			: base (message)
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public AIException (string message, Exception inner)
			: base (message, inner)
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected AIException (System.Runtime.Serialization.SerializationInfo info,
		                       System.Runtime.Serialization.StreamingContext context)
			: base (info,
			        context)
		{
		}
	}
}
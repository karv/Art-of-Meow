using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Units.Recursos;

namespace Units.Buffs
{
	/// <summary>
	/// Buff that raise movement speed
	/// </summary>
	public class HasteBuff : IStatsBuff, IDibujable, IComponent
	{
		float _speedDelta;
		readonly Helper.Timer timer;

		/// <summary>
		/// Gets a value indicating if this buff is shown in the list of active buffs
		/// This buff is visible
		/// </summary>
		public bool IsVisible { get { return true; } }

		/// <summary>
		/// Gets a value indicating if this buff has been initialized
		/// </summary>
		/// <value><c>true</c> if inicializado; otherwise, <c>false</c>.</value>
		public bool Inicializado { get; private set; }

		/// <summary>
		/// Gets or sets the effect in the <c>Velocidad</c> atribute
		/// </summary>
		/// <value>A <c>float</c> value representing the diference in the attribute</value>
		public float SpeedDelta
		{
			get
			{ 
				return _speedDelta; 
			}
			set
			{
				if (Inicializado)
					throw new InvalidOperationException ("Cannot set value after initialization");
				_speedDelta = value;
			}
		}

		/// <summary>
		/// Gets or sets how long the buff lasts
		/// </summary>
		/// <value>The duration, in game time</value>
		public float Duración
		{
			get{ return timer.ConteoInicial; }
			set{ timer.ConteoInicial = value; }
		}

		#region IStatsBuff implementation

		/// <summary>
		/// Devuelve la modificación (absoluta) de valor de un recurso.
		/// </summary>
		/// <returns>The delta.</returns>
		/// <param name="resName">Res name.</param>
		public float StatDelta (string resName)
		{
			return resName == ConstantesRecursos.Velocidad ? SpeedDelta : 0;
		}

		#endregion

		#region IBuff implementation

		/// <summary>
		/// Se invoca cuando está por desanclarse
		/// </summary>
		public void Terminating ()
		{
		}

		/// <summary>
		/// De invoca justo después de anclarse.
		/// </summary>
		public void Initialize ()
		{
			Inicializado = true;
			timer.Habilitado = true;
			timer.Reset ();
		}

		const string iconName = "clock";

		void IComponent.AddContent (Moggle.BibliotecaContenido manager)
		{
			manager.AddContent (iconName);
		}

		void IComponent.InitializeContent (Moggle.BibliotecaContenido manager)
		{
			icon = manager.GetContent<Texture2D> (iconName);
		}

		/// <summary>
		/// Name of the buff: Haste
		/// </summary>
		string IBuff.Nombre { get { return "Haste"; } }

		string IBuff.BaseTextureName { get { return iconName; } }

		/// <summary>
		/// El manejador de buffs
		/// </summary>
		/// <value>The manager.</value>
		public BuffManager Manager { get; set; }

		#endregion

		#region IInternalUpdate implementation

		/// <summary>
		/// Updates the buff
		/// </summary>
		public void Update (float gameTime)
		{
			timer.Update (gameTime);
		}

		#endregion

		Texture2D icon;

		void IDisposable.Dispose ()
		{
		}

		void IDibujable.Draw (SpriteBatch bat,
		                      Rectangle rect)
		{
			if (IsVisible)
				bat.Draw (icon, rect, Color.Green);
		}

		void timer_countdown (object sender, EventArgs e)
		{
			Debug.WriteLine (string.Format (
				"Desanclando buff {0} de {1}",
				this,
				Manager.HookedOn));
			Manager.UnHook (this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Buffs.HasteBuff"/> class.
		/// </summary>
		public HasteBuff ()
		{
			timer = new Helper.Timer ();
			timer.ConteoCero += timer_countdown;
		}
	}
}
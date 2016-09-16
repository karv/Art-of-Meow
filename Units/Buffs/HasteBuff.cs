using System;
using Units.Recursos;

namespace Units.Buffs
{
	public class HasteBuff : IStatsBuff
	{
		float _speedDelta;
		readonly Helper.Timer timer;

		public bool Inicializado { get; private set; }

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

		public float Duración
		{
			get{ return timer.ConteoInicial; }
			set{ timer.ConteoInicial = value; }
		}

		#region IStatsBuff implementation

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
		}

		public string Nombre { get { return "Haste"; } }

		public string BaseTextureName { get { return "Icons//haste"; } }

		public BuffManager Manager { get; set; }

		#endregion

		#region IInternalUpdate implementation

		public void Update (float gameTime)
		{
			timer.Update (gameTime);
		}

		#endregion

		
		void timer_countdown (object sender, EventArgs e)
		{
			Manager.UnHook (this);
		}

		public HasteBuff (BuffManager manager)
		{
			Manager = manager;
			timer = new Helper.Timer ();
			timer.ConteoCero += timer_countdown;
		}

	}
}
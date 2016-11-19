using System;
using AoM;
using Moggle.Controles;
using Moggle.Screens;
using System.Collections.Generic;

namespace Screens
{
	public interface IResponseScreen<TResp> : IScreen
	{
		event EventHandler<TResp> Response;
	}

	public class WaitResponseScreen<TInp, T> : IResponseScreen<T[]>
	{
		List<IResponseScreen<TInp>> _invocationList = new List<IResponseScreen<TInp>> ();
		List<Func<TInp, T>> _selector = new List<Func<TInp, T>> ();

		public void AddRequestArgument (IResponseScreen<TInp> scr,
		                                Func<TInp, T> selector)
		{
			_invocationList.Add (scr);
			_selector.Add (selector);
		}

		public void Exit ()
		{
			Juego.ScreenManager.ActiveThread.TerminateLast ();
			Response?.Invoke (this, Data);
		}

		public event EventHandler<T []> Response;

		public void Run ()
		{
			Data = new T[_invocationList.Count];
			this.Execute (new ScreenThread.ScreenStackOptions ());
			_invocationList [0].Execute (new ScreenThread.ScreenStackOptions ());
		}

		int _currentScreen;
		public T [] Data;

		#region IScreen implementation

		public void Draw ()
		{
		}


		public void Update (Microsoft.Xna.Framework.GameTime gameTime,
		                    ScreenThread currentThread)
		{
			//UpdateAction.Invoke ();
		}

		public Microsoft.Xna.Framework.Color? BgColor { get { return null; } }

		public Moggle.BibliotecaContenido Content
		{
			get
			{
				return Juego.Contenido;
			}
		}

		public Microsoft.Xna.Framework.Graphics.SpriteBatch Batch
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public Microsoft.Xna.Framework.Graphics.DisplayMode GetDisplayMode
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public Microsoft.Xna.Framework.Graphics.GraphicsDevice Device
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public Moggle.Game Juego { get { return Program.MyGame; } }

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
		}

		#endregion

		#region IComponentContainerComponent implementation

		public void AddComponent (IControl component)
		{
			throw new NotImplementedException ();
		}

		public bool RemoveComponent (IControl component)
		{
			throw new NotImplementedException ();
		}

		public IEnumerable<IControl> Components
		{
			get
			{
				yield break;
			}
		}


		#endregion

		#region IComponent implementation

		public void AddContent ()
		{
		}

		public void InitializeContent ()
		{
		}

		#endregion

		#region IGameComponent implementation

		public void Initialize ()
		{
			foreach (var x in _invocationList)
			{
				x.Response += delegate(object sender, TInp e)
				{
					Data [_currentScreen] = _selector [_currentScreen].Invoke (e);
					Juego.ScreenManager.ActiveThread.TerminateLast ();
					_currentScreen++; // Avanzar al siguiente estado

					if (Data.Length == _currentScreen)
					{
						// Ya terminé la lista de invocación
						Response?.Invoke (this, Data);
						Juego.ScreenManager.ActiveThread.TerminateLast ();
					}
					else
					{
						ScreenExt.Execute (_invocationList [_currentScreen], 
							new ScreenThread.ScreenStackOptions
							{
								DibujaBase = false,
								ActualizaBase = false
							});
					}
				};
			}
		}

		#endregion

		#region IReceptor implementation

		public bool RecibirSeñal (Tuple<MonoGame.Extended.InputListeners.KeyboardEventArgs, ScreenThread> signal)
		{
			return false;
		}

		#endregion

		#region IEmisor implementation

		public void MandarSeñal (MonoGame.Extended.InputListeners.KeyboardEventArgs key)
		{
		}

		#endregion

		#region IControl implementation

		public IComponentContainerComponent<IControl> Container { get { return Juego; } }

		#endregion
	}
}
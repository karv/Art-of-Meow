using System.Linq;
using Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Moggle.Screens;
using MonoGame.Extended;
using Units.Recursos;

namespace Componentes
{
	/// <summary>
	/// Lists the Recursos
	/// </summary>
	public class RecursoView : IComponent, IDrawable, IUpdate
	{
		ManejadorRecursos Recursos { get; }

		readonly RetardValue [] _suavizador;
		readonly IVisibleRecurso [] _recursos;

		#region IDrawable implementation

		event System.EventHandler<System.EventArgs> IDrawable.DrawOrderChanged
		{
			add
			{
				throw new System.NotImplementedException ();
			}
			remove
			{
				throw new System.NotImplementedException ();
			}
		}

		event System.EventHandler<System.EventArgs> IDrawable.VisibleChanged
		{
			add
			{
				throw new System.NotImplementedException ();
			}
			remove
			{
				throw new System.NotImplementedException ();
			}
		}

		void IDrawable.Draw (GameTime gameTime)
		{
			draw (gameTime);
		}

		int IDrawable.DrawOrder { get { return 0; } }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Componentes.RecursoView"/> is visible.
		/// </summary>
		/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
		public bool Visible { get; set; }

		#endregion

		#region IComponent implementation

		void IComponent.AddContent (Moggle.BibliotecaContenido manager)
		{
		}

		void IComponent.InitializeContent (Moggle.BibliotecaContenido manager)
		{
		}

		#endregion

		#region IDisposable implementation

		void System.IDisposable.Dispose ()
		{
		}

		#endregion

		#region IGameComponent implementation

		void IGameComponent.Initialize ()
		{
			for (int i = 0; i < _recursos.Length; i++)
				_suavizador [i] = new RetardValue
				{
					VisibleValue = _recursos [i].Valor,
					ChangeSpeed = 0.01f,
				};
		}

		#endregion

		/// <summary>
		/// Devuelve o establece la posición
		/// </summary>
		/// <value>The top left.</value>
		public Point TopLeft { get; set; }

		/// <summary>
		/// Devuelve o establece el tamaño de cada icono
		/// </summary>
		/// <value>The size of the icon.</value>
		public Size Size { get; set; }

		/// <summary>
		/// Devuelve o establece el espaciado vertical entre iconos
		/// </summary>
		/// <value>The V space.</value>
		public int VSpace { get; set; }

		IScreen screen { get; }

		/// <summary>
		/// Dibuja el objeto sobre un rectángulo específico
		/// </summary>
		/// <param name="batch">Batch</param>
		/// <param name="rect">Rectángulo de dibujo</param>
		/// <param name="index">Índice del recurso en dibujo</param>
		void draw (SpriteBatch batch, Rectangle rect, int index)
		{
			var ret = _suavizador [index];
			var rec = _recursos [index];

			var text = rec.TextureFill;

			var fullRect = new Rectangle (
				               rect.Location,
				               new Point (
					               (int)(rec.PctValue (rec.Valor) * rect.Width),
					               rect.Height));

			var deltaRect = new Rectangle (
				                rect.Location,
				                new Point (
					                (int)(rec.PctValue (ret.VisibleValue) * rect.Width),
					                rect.Height));

			batch.Draw (text, fullRect, rec.FullColor);
			batch.Draw (text, deltaRect, rec.DeltaColor);
		}

		/// <summary>
		/// Dibuja el control.
		/// </summary>
		void draw (GameTime gameTime)
		{
			var iconTopLeft = new Point (TopLeft.X, TopLeft.Y);
			for (int i = 0; i < _recursos.Length; i++)
			{
				var outputRect = new Rectangle (iconTopLeft, Size);
				draw (screen.Batch, outputRect, i);
				iconTopLeft += new Point (0, Size.Height + VSpace);
			}
		}

		/// <summary>
		/// Update lógico
		/// </summary>
		public void Update (GameTime gameTime)
		{
			for (int i = 0; i < _recursos.Length; i++)
				_suavizador [i].UpdateTo (gameTime, _recursos [i].Valor);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Componentes.RecursoView"/> class.
		/// </summary>
		/// <param name="scr">Screen</param>
		/// <param name="recs">The recursos that this control is going to show</param>
		public RecursoView (Screen scr,
		                    ManejadorRecursos recs)
		{
			screen = scr;
			Recursos = recs;
			Visible = true;
			_recursos = Recursos.Enumerate ().OfType<IVisibleRecurso> ().ToArray ();
			_suavizador = new RetardValue[_recursos.Length];

			Size = new Size (64, 12);
			TopLeft = new Point (3, 3);
		}
	}
}
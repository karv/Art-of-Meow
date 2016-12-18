using System.Linq;
using Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Moggle.Screens;
using MonoGame.Extended;
using Units.Recursos;
using Microsoft.Xna.Framework.Content;

namespace Componentes
{
	/// <summary>
	/// Lists the Recursos
	/// </summary>
	public class RecursoView : IControl, IDrawable, IUpdateable
	{
		ManejadorRecursos Recursos { get; }

		readonly RetardValue [] _suavizador;
		readonly IVisibleRecurso [] _recursos;
		readonly Texture2D [] _texture;
		Texture2D _contornoTextura;

		readonly int count;

		#region IDrawable implementation

		event System.EventHandler<System.EventArgs> IDrawable.DrawOrderChanged
		{
			add	{}
			remove {}
		}

		event System.EventHandler<System.EventArgs> IDrawable.VisibleChanged
		{
			add	{}
			remove {}
		}

		void IDrawable.Draw (GameTime gameTime)
		{
			draw ();
		}

		int IDrawable.DrawOrder { get { return 0; } }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Componentes.RecursoView"/> is visible.
		/// </summary>
		/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
		public bool Visible { get; set; }

		#endregion

		#region IComponent implementation

		void IComponent.LoadContent (ContentManager manager)
		{
			var textMaker = new Moggle.Textures.SimpleTextures (Screen.Device);
			_contornoTextura = textMaker.OutlineTexture (
				Size,
				Color.Black,
				Color.Transparent);

			for (int i = 0; i < count; i++)
				_texture [i] = manager.Load<Texture2D> (_recursos [i].TextureFill);
			
		}

		#endregion

		#region Update

		event System.EventHandler<System.EventArgs> IUpdateable.EnabledChanged
		{
			add	{}
			remove {}
		}

		event System.EventHandler<System.EventArgs> IUpdateable.UpdateOrderChanged
		{
			add	{}
			remove {}
		}

		bool IUpdateable.Enabled { get { return true; } }

		int IUpdateable.UpdateOrder { get { return 0; } }

		#endregion

		#region IGameComponent implementation

		IComponentContainerComponent<IControl> IControl.Container { get { return Screen; } }

		void IGameComponent.Initialize ()
		{
			for (int i = 0; i < _recursos.Length; i++)
				_suavizador [i] = new RetardValue
				{
					VisibleValue = _recursos [i].Valor,
					ChangeSpeed = 0.003f,
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

		/// <summary>
		/// Devuelve la <see cref="IScreen"/> a la que pertenece este RecursoView
		/// </summary>
		protected IScreen Screen { get; }

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

			var text = _texture [index];

			var deltaRect = new Rectangle (
				                rect.Location,
				                new Point (
					                (int)(rec.PctValue (ret.VisibleValue) * rect.Width),
					                rect.Height));

			batch.Draw (text, deltaRect, rec.FullColor);
			batch.Draw (_contornoTextura, rect, Color.Black);
		}

		/// <summary>
		/// Dibuja el control.
		/// </summary>
		void draw ()
		{
			var iconTopLeft = new Point (TopLeft.X, TopLeft.Y);
			for (int i = 0; i < _recursos.Length; i++)
			{
				if (_recursos [i].Visible)
				{
					var outputRect = new Rectangle (iconTopLeft, Size);
					draw (Screen.Batch, outputRect, i);
					iconTopLeft += new Point (0, Size.Height + VSpace);
				}
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
		public RecursoView (IScreen scr,
		                    ManejadorRecursos recs)
		{
			Screen = scr;
			Recursos = recs;
			Visible = true;
			_recursos = Recursos.Enumerate ().OfType<IVisibleRecurso> ().ToArray ();
			count = _recursos.Length;
			_suavizador = new RetardValue[count];
			_texture = new Texture2D[count];

			Size = new Size (64, 8);
			TopLeft = new Point (3, 3);
		}
	}
}
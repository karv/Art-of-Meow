using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle;
using Units.Order;
using Units.Recursos;

namespace Units.Skills
{
	/// <summary>
	/// Skill that allows the user to heal ?-self
	/// </summary>
	public class SelfHealSkill : ISkill
	{
		#region ISkill implementation

		const float _heal = 10f;
		const float _mpCost = 3f;

		static BibliotecaContenido content { get; }

		/// <summary>
		/// Executes this skill
		/// </summary>
		/// <param name="user">The caster</param>
		public void Execute (IUnidad user)
		{
			user.EnqueueOrder (new ExecuteOrder (user, doEffect));
		}

		static void doEffect (IUnidad target)
		{
			var hpRec = target.Recursos.GetRecurso (ConstantesRecursos.HP);
			hpRec.Valor += _heal;
		}

		/// <summary>
		/// Determines whether this skill is castable by the specified user.
		/// </summary>
		/// <returns><c>true</c> if this skill is castable by the specified user; otherwise, <c>false</c>.</returns>
		/// <param name="user">User</param>
		public bool IsCastable (IUnidad user)
		{
			var mpRec = user.Recursos.GetRecurso (ConstantesRecursos.MP);
			return mpRec.Valor >= _mpCost;
		}

		/// <summary>
		/// Determines whether this instance is visible to the specified <see cref="IUnidad"/>
		/// </summary>
		/// <returns><c>true</c> if this instance is visible by the specified user; otherwise, <c>false</c></returns>
		/// <param name="user">User</param>
		public bool IsVisible (IUnidad user)
		{
			return true;
		}

		#endregion

		#region IComponent implementation

		Texture2D _texture;

		const string TextureName = "heal";

		/// <summary>
		/// Loads the icon texture
		/// </summary>
		public void AddContent ()
		{
			content.AddContent (TextureName);
			InitializeContent ();
		}

		/// <summary>
		/// Carga la textura
		/// </summary>
		public void InitializeContent ()
		{
			_texture = content.GetContent<Texture2D> (TextureName);
		}

		#endregion

		#region IGameComponent implementation

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public void Initialize ()
		{
		}

		#endregion

		#region IDibujable implementation

		/// <summary>
		/// Draws the icon on a rectangle
		/// </summary>
		/// <param name="bat">Batch</param>
		/// <param name="rect">Rectángulo de dibujo</param>
		public void Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (_texture, rect, Color.Red);
		}

		#endregion
	}
}
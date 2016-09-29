using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Units.Recursos;

namespace Units.Skills
{
	public class SelfHealSkill : ISkill
	{
		#region ISkill implementation

		public void Execute (IUnidad user)
		{
			var hpRec = user.Recursos.GetRecurso (ConstantesRecursos.HP);
			hpRec.Valor += 10;
		}

		#endregion

		#region IComponent implementation

		Texture2D _texture;

		const string TextureName = "unit";

		public void LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			_texture = manager.Load<Texture2D> (TextureName);
		}

		public void UnloadContent ()
		{
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
			UnloadContent ();
		}

		#endregion

		#region IGameComponent implementation

		public void Initialize ()
		{
		}

		#endregion

		#region IDibujable implementation

		public void Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (_texture, rect, Color.White);
		}

		#endregion
	}
}
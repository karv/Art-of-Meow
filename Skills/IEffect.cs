
namespace Skills
{
	/// <summary>
	/// Un objeto que puede producir un <see cref="IEffect"/>
	/// </summary>
	public interface IEffectAgent
	{
		
	}

	/// <summary>
	/// Un target de un <see cref="ITargetEffect"/>
	/// </summary>
	public interface ITarget
	{
		
	}

	/// <summary>
	/// Un efecto de un objeto.
	/// </summary>
	public interface IEffect
	{
		IEffectAgent Agent { get; }

		void Execute ();
	}

	/// <summary>
	/// Un efecto que tiene un target
	/// </summary>
	public interface ITargetEffect : IEffect
	{
		ITarget Target { get; }
	}
}
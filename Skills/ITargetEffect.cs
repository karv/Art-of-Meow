using Skills;

namespace Skills
{
	/// <summary>
	/// Un efecto que tiene un target
	/// </summary>
	public interface ITargetEffect : IEffect
	{
		/// <summary>
		/// Gets the target of the effect
		/// </summary>
		/// <value>The target.</value>
		ITarget Target { get; }
	}
}
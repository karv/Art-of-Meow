using AoM;
using Moggle.Controles;
using Skills;

namespace Units.Skills
{
	/// <summary>
	/// Represents a skill
	/// </summary>
	public interface ISkill : IDibujable, IComponent, IIdentificable
	{
		/// <summary>
		/// Build a skill instance
		/// </summary>
		/// <remarks>
		/// Must prepare the value of the getter <see cref="LastGeneratedInstance"/>
		/// </remarks>
		/// <param name="user">User of the skill</param>
		void GetInstance (IUnidad user);

		/// <summary>
		/// Devuelve la última instancia generada.
		/// </summary>
		/// <value>The last generated instance.</value>
		SkillInstance LastGeneratedInstance { get; }

		/// <summary>
		/// Determines whether this skill is castable by the specified user.
		/// </summary>
		/// <returns><c>true</c> if this skill is castable by the specified user; otherwise, <c>false</c>.</returns>
		/// <param name="user">User</param>
		bool IsCastable (IUnidad user);

		/// <summary>
		/// Determines whether this instance is visible to the specified <see cref="IUnidad"/>
		/// </summary>
		/// <returns><c>true</c> if this instance is visible by the specified user; otherwise, <c>false</c></returns>
		/// <param name="user">User</param>
		bool IsVisible (IUnidad user);

		/// <summary>
		/// Gets the value of the skill
		/// </summary>
		float Value { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is learnable.
		/// </summary>
		bool IsLearnable { get; }

		/// <summary>
		/// Gets the requiered skills names to learn this skill via <see cref="LearningSystem"/>
		/// </summary>
		string[] RequiredSkills { get; }
	}
}
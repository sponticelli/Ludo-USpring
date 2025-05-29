using USpring.Core.Interfaces;

namespace USpring.Core
{

	/// <summary>
	/// Represents a single-dimensional spring system with float values.
	/// Inherits from the base `Spring` class and provides functionality
	/// for managing target, current value, velocity, clamping, force, and drag
	/// specific to a single float-based spring.
	/// </summary>
	[System.Serializable]
	public class SpringFloat : Spring, ISpringFloat
	{
		public const int SPRING_SIZE = 1;
		public const int X = 0;

		public SpringFloat() : base(SPRING_SIZE)
		{

		}

		public override bool HasValidSize()
		{
			bool res = springValues.Length == SPRING_SIZE;
			return res;
		}

		public override int GetSpringSize()
		{
			return SPRING_SIZE;
		}



		/// <summary>
		/// Gets the target value of the spring.
		/// </summary>
		/// <returns>The target value.</returns>
		public float GetTarget()
		{
			float res = springValues[X].GetTarget();
			return res;
		}

		/// <summary>
		/// Sets the target value of the spring.
		/// </summary>
		/// <param name="target">The target value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetTarget(float target)
		{
			springValues[X].SetTarget(target);
			return this;
		}

		/// <summary>
		/// Gets the current value of the spring.
		/// </summary>
		/// <returns>The current value.</returns>
		public float GetCurrentValue()
		{
			float res = springValues[X].GetCurrentValue();
			return res;
		}

		/// <summary>
		/// Sets the current value of the spring.
		/// </summary>
		/// <param name="value">The current value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetCurrentValue(float value)
		{
			springValues[X].SetCurrentValue(value);
			return this;
		}

		/// <summary>
		/// Gets the velocity of the spring.
		/// </summary>
		/// <returns>The velocity value.</returns>
		public float GetVelocity()
		{
			float res = springValues[X].GetVelocity();
			return res;
		}

		/// <summary>
		/// Adds to the current velocity of the spring.
		/// </summary>
		/// <param name="velocityToAdd">The velocity to add.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat AddVelocity(float velocityToAdd)
		{
			springValues[X].AddVelocity(velocityToAdd);
			return this;
		}

		/// <summary>
		/// Sets the velocity of the spring.
		/// </summary>
		/// <param name="velocity">The velocity value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetVelocity(float velocity)
		{
			springValues[X].SetVelocity(velocity);
			return this;
		}

		/// <summary>
		/// Sets the minimum value for clamping.
		/// </summary>
		/// <param name="minValue">The minimum value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetMinValue(float minValue)
		{
			SetMinValueByIndex(X, minValue);
			return this;
		}

		/// <summary>
		/// Gets the minimum value for clamping.
		/// </summary>
		/// <returns>The minimum value.</returns>
		public float GetMinValue()
		{
			return springValues[X].GetMinValue();
		}

		/// <summary>
		/// Sets the maximum value for clamping.
		/// </summary>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetMaxValue(float maxValue)
		{
			SetMaxValueByIndex(X, maxValue);
			return this;
		}

		/// <summary>
		/// Gets the maximum value for clamping.
		/// </summary>
		/// <returns>The maximum value.</returns>
		public float GetMaxValue()
		{
			return springValues[X].GetMaxValue();
		}

		/// <summary>
		/// Sets the minimum and maximum values for clamping in a single call.
		/// </summary>
		/// <param name="minValue">The minimum value.</param>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetClampRange(float minValue, float maxValue)
		{
			SetMinValueByIndex(X, minValue);
			SetMaxValueByIndex(X, maxValue);
			return this;
		}

		/// <summary>
		/// Enables or disables target clamping.
		/// </summary>
		/// <param name="enabled">Whether target clamping should be enabled.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetClampTarget(bool enabled)
		{
			SetClampTargetByIndex(X, enabled);
			return this;
		}


		/// <summary>
		/// Enables or disables current value clamping.
		/// </summary>
		/// <param name="enabled">Whether current value clamping should be enabled.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetClampCurrentValue(bool enabled)
		{
			SetClampCurrentValueByIndex(X, enabled);
			return this;
		}

		/// <summary>
		/// Enables or disables stopping the spring when the current value is clamped.
		/// </summary>
		/// <param name="enabled">Whether to stop the spring when the current value is clamped.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetStopOnClamp(bool enabled)
		{
			SetStopSpringOnCurrentValueClampByIndex(X, enabled);
			return this;
		}

		/// <summary>
		/// Gets whether the spring stops when the current value is clamped.
		/// </summary>
		/// <returns>True if the spring stops when the current value is clamped, false otherwise.</returns>
		public bool DoesStopOnClamp()
		{
			return springValues[X].GetStopOnClamp();
		}

		/// <summary>
		/// Configures clamping settings in a single call.
		/// </summary>
		/// <param name="clampTarget">Whether to clamp the target value.</param>
		/// <param name="clampCurrentValue">Whether to clamp the current value.</param>
		/// <param name="stopOnClamp">Whether to stop the spring when the current value is clamped.</param>
		/// <param name="minValue">The minimum value for clamping.</param>
		/// <param name="maxValue">The maximum value for clamping.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, float minValue, float maxValue)
		{
			SetClampTarget(clampTarget);
			SetClampCurrentValue(clampCurrentValue);
			SetStopOnClamp(stopOnClamp);
			SetMinValue(minValue);
			SetMaxValue(maxValue);
			return this;
		}

		/// <summary>
		/// Gets the force (stiffness) of the spring.
		/// </summary>
		/// <returns>The force value.</returns>
		public float GetForce()
		{
			float res = GetForceByIndex(X);
			return res;
		}

		/// <summary>
		/// Sets the force (stiffness) of the spring.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetForce(float force)
		{
			SetForceByIndex(X, force);
			return this;
		}

		/// <summary>
		/// Gets the drag (damping) of the spring.
		/// </summary>
		/// <returns>The drag value.</returns>
		public float GetDrag()
		{
			float res = GetDragByIndex(X);
			return res;
		}

		/// <summary>
		/// Sets the drag (damping) of the spring.
		/// </summary>
		/// <param name="drag">The drag value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat SetDrag(float drag)
		{
			SetDragByIndex(X, drag);
			return this;
		}

		/// <summary>
		/// Configures the spring in a single call.
		/// </summary>
		/// <param name="force">The force (stiffness) value.</param>
		/// <param name="drag">The drag (damping) value.</param>
		/// <param name="initialValue">The initial value of the spring.</param>
		/// <param name="target">The target value of the spring.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringFloat Configure(float force, float drag, float initialValue, float target)
		{
			SetForce(force);
			SetDrag(drag);
			SetCurrentValue(initialValue);
			SetTarget(target);
			return this;
		}

		/// <summary>
		/// Gets whether target clamping is enabled.
		/// </summary>
		/// <returns>True if target clamping is enabled, false otherwise.</returns>
		public bool IsClampTargetEnabled()
		{
			return springValues[X].GetClampTarget();
		}

		/// <summary>
		/// Gets whether current value clamping is enabled.
		/// </summary>
		/// <returns>True if current value clamping is enabled, false otherwise.</returns>
		public bool IsClampCurrentValueEnabled()
		{
			return springValues[X].GetClampCurrentValue();
		}

		/// <summary>
		/// Creates a new instance of ISpringFloat with the same configuration.
		/// </summary>
		/// <returns>A new instance of ISpringFloat with the same configuration.</returns>
		public ISpringFloat Clone()
		{
			SpringFloat clone = new SpringFloat();
			clone.commonForceAndDrag = this.commonForceAndDrag;
			clone.commonForce = this.commonForce;
			clone.commonDrag = this.commonDrag;
			clone.springEnabled = this.springEnabled;
			clone.clampingEnabled = this.clampingEnabled;
			clone.eventsEnabled = this.eventsEnabled;

			// Clone spring values
			clone.springValues[X].SetTarget(this.springValues[X].GetTarget());
			clone.springValues[X].SetCurrentValue(this.springValues[X].GetCurrentValue());
			clone.springValues[X].SetVelocity(this.springValues[X].GetVelocity());
			clone.springValues[X].SetForce(this.springValues[X].GetForce());
			clone.springValues[X].SetDrag(this.springValues[X].GetDrag());
			clone.springValues[X].SetMinValue(this.springValues[X].GetMinValue());
			clone.springValues[X].SetMaxValue(this.springValues[X].GetMaxValue());
			clone.springValues[X].SetClampTarget(this.springValues[X].GetClampTarget());
			clone.springValues[X].SetClampCurrentValue(this.springValues[X].GetClampCurrentValue());
			clone.springValues[X].SetStopOnClamp(this.springValues[X].GetStopOnClamp());

			return clone;
		}
	}
}
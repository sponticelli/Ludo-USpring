namespace USpring.Core.Interfaces
{
    /// <summary>
    /// Interface for a single-dimensional spring that animates float values.
    /// </summary>
    public interface ISpringFloat : ISpring
    {
        /// <summary>
        /// Sets the target value of the spring.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetTarget(float target);
        
        /// <summary>
        /// Gets the target value of the spring.
        /// </summary>
        /// <returns>The target value.</returns>
        float GetTarget();
        
        /// <summary>
        /// Sets the current value of the spring.
        /// </summary>
        /// <param name="value">The current value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetCurrentValue(float value);
        
        /// <summary>
        /// Gets the current value of the spring.
        /// </summary>
        /// <returns>The current value.</returns>
        float GetCurrentValue();
        
        /// <summary>
        /// Sets the velocity of the spring.
        /// </summary>
        /// <param name="velocity">The velocity value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetVelocity(float velocity);
        
        /// <summary>
        /// Gets the velocity of the spring.
        /// </summary>
        /// <returns>The velocity value.</returns>
        float GetVelocity();
        
        /// <summary>
        /// Adds to the current velocity of the spring.
        /// </summary>
        /// <param name="velocityToAdd">The velocity to add.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat AddVelocity(float velocityToAdd);
        
        /// <summary>
        /// Sets the force (stiffness) of the spring.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetForce(float force);
        
        /// <summary>
        /// Gets the force (stiffness) of the spring.
        /// </summary>
        /// <returns>The force value.</returns>
        float GetForce();
        
        /// <summary>
        /// Sets the drag (damping) of the spring.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetDrag(float drag);
        
        /// <summary>
        /// Gets the drag (damping) of the spring.
        /// </summary>
        /// <returns>The drag value.</returns>
        float GetDrag();
        
        /// <summary>
        /// Sets the minimum value for clamping.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetMinValue(float minValue);
        
        /// <summary>
        /// Gets the minimum value for clamping.
        /// </summary>
        /// <returns>The minimum value.</returns>
        float GetMinValue();
        
        /// <summary>
        /// Sets the maximum value for clamping.
        /// </summary>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetMaxValue(float maxValue);
        
        /// <summary>
        /// Gets the maximum value for clamping.
        /// </summary>
        /// <returns>The maximum value.</returns>
        float GetMaxValue();
        
        /// <summary>
        /// Sets the minimum and maximum values for clamping in a single call.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetClampRange(float minValue, float maxValue);
        
        /// <summary>
        /// Enables or disables target clamping.
        /// </summary>
        /// <param name="enabled">Whether target clamping should be enabled.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetClampTarget(bool enabled);
        
        /// <summary>
        /// Gets whether target clamping is enabled.
        /// </summary>
        /// <returns>True if target clamping is enabled, false otherwise.</returns>
        bool IsClampTargetEnabled();
        
        /// <summary>
        /// Enables or disables current value clamping.
        /// </summary>
        /// <param name="enabled">Whether current value clamping should be enabled.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetClampCurrentValue(bool enabled);
        
        /// <summary>
        /// Gets whether current value clamping is enabled.
        /// </summary>
        /// <returns>True if current value clamping is enabled, false otherwise.</returns>
        bool IsClampCurrentValueEnabled();
        
        /// <summary>
        /// Enables or disables stopping the spring when the current value is clamped.
        /// </summary>
        /// <param name="enabled">Whether to stop the spring when the current value is clamped.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat SetStopOnClamp(bool enabled);
        
        /// <summary>
        /// Gets whether the spring stops when the current value is clamped.
        /// </summary>
        /// <returns>True if the spring stops when the current value is clamped, false otherwise.</returns>
        bool DoesStopOnClamp();
        
        /// <summary>
        /// Configures clamping settings in a single call.
        /// </summary>
        /// <param name="clampTarget">Whether to clamp the target value.</param>
        /// <param name="clampCurrentValue">Whether to clamp the current value.</param>
        /// <param name="stopOnClamp">Whether to stop the spring when the current value is clamped.</param>
        /// <param name="minValue">The minimum value for clamping.</param>
        /// <param name="maxValue">The maximum value for clamping.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, float minValue, float maxValue);
        
        /// <summary>
        /// Configures the spring in a single call.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <param name="initialValue">The initial value of the spring.</param>
        /// <param name="target">The target value of the spring.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringFloat Configure(float force, float drag, float initialValue, float target);
        
        /// <summary>
        /// Creates a new instance of ISpringFloat with the same configuration.
        /// </summary>
        /// <returns>A new instance of ISpringFloat with the same configuration.</returns>
        ISpringFloat Clone();
    }
}

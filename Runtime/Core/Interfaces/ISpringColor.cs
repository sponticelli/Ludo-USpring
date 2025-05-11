using UnityEngine;

namespace USpring.Core.Interfaces
{
    /// <summary>
    /// Interface for a spring that animates Color values.
    /// </summary>
    public interface ISpringColor : ISpring
    {
        /// <summary>
        /// Sets the target color of the spring.
        /// </summary>
        /// <param name="target">The target color.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetTarget(Color target);
        
        /// <summary>
        /// Gets the target color of the spring.
        /// </summary>
        /// <returns>The target color.</returns>
        Color GetTarget();
        
        /// <summary>
        /// Sets the current color of the spring.
        /// </summary>
        /// <param name="value">The current color.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetCurrentValue(Color value);
        
        /// <summary>
        /// Gets the current color of the spring.
        /// </summary>
        /// <returns>The current color.</returns>
        Color GetCurrentValue();
        
        /// <summary>
        /// Sets the velocity of the spring.
        /// </summary>
        /// <param name="velocity">The velocity value as a Vector4 (r, g, b, a).</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetVelocity(Vector4 velocity);
        
        /// <summary>
        /// Gets the velocity of the spring.
        /// </summary>
        /// <returns>The velocity value as a Vector4 (r, g, b, a).</returns>
        Vector4 GetVelocity();
        
        /// <summary>
        /// Adds to the current velocity of the spring.
        /// </summary>
        /// <param name="velocityToAdd">The velocity to add as a Vector4 (r, g, b, a).</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor AddVelocity(Vector4 velocityToAdd);
        
        /// <summary>
        /// Sets the force (stiffness) of the spring.
        /// </summary>
        /// <param name="force">The force value as a Vector4 (r, g, b, a).</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetForce(Vector4 force);
        
        /// <summary>
        /// Sets the force (stiffness) of the spring using a uniform value for all components.
        /// </summary>
        /// <param name="uniformForce">The uniform force to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetForce(float uniformForce);
        
        /// <summary>
        /// Gets the force (stiffness) of the spring.
        /// </summary>
        /// <returns>The force value as a Vector4 (r, g, b, a).</returns>
        Vector4 GetForce();
        
        /// <summary>
        /// Sets the drag (damping) of the spring.
        /// </summary>
        /// <param name="drag">The drag value as a Vector4 (r, g, b, a).</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetDrag(Vector4 drag);
        
        /// <summary>
        /// Sets the drag (damping) of the spring using a uniform value for all components.
        /// </summary>
        /// <param name="uniformDrag">The uniform drag to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetDrag(float uniformDrag);
        
        /// <summary>
        /// Gets the drag (damping) of the spring.
        /// </summary>
        /// <returns>The drag value as a Vector4 (r, g, b, a).</returns>
        Vector4 GetDrag();
        
        /// <summary>
        /// Sets the minimum values for clamping.
        /// </summary>
        /// <param name="minValues">The minimum values as a Vector4 (r, g, b, a).</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetMinValues(Vector4 minValues);
        
        /// <summary>
        /// Sets the minimum values for clamping using a uniform value for all components.
        /// </summary>
        /// <param name="uniformMinValue">The uniform minimum value to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetMinValues(float uniformMinValue);
        
        /// <summary>
        /// Gets the minimum values for clamping.
        /// </summary>
        /// <returns>The minimum values as a Vector4 (r, g, b, a).</returns>
        Vector4 GetMinValues();
        
        /// <summary>
        /// Sets the maximum values for clamping.
        /// </summary>
        /// <param name="maxValues">The maximum values as a Vector4 (r, g, b, a).</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetMaxValues(Vector4 maxValues);
        
        /// <summary>
        /// Sets the maximum values for clamping using a uniform value for all components.
        /// </summary>
        /// <param name="uniformMaxValue">The uniform maximum value to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetMaxValues(float uniformMaxValue);
        
        /// <summary>
        /// Gets the maximum values for clamping.
        /// </summary>
        /// <returns>The maximum values as a Vector4 (r, g, b, a).</returns>
        Vector4 GetMaxValues();
        
        /// <summary>
        /// Sets the minimum and maximum values for clamping in a single call.
        /// </summary>
        /// <param name="minValues">The minimum values as a Vector4 (r, g, b, a).</param>
        /// <param name="maxValues">The maximum values as a Vector4 (r, g, b, a).</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetClampRange(Vector4 minValues, Vector4 maxValues);
        
        /// <summary>
        /// Sets the minimum and maximum values for clamping in a single call using uniform values.
        /// </summary>
        /// <param name="uniformMinValue">The uniform minimum value to use for all components.</param>
        /// <param name="uniformMaxValue">The uniform maximum value to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetClampRange(float uniformMinValue, float uniformMaxValue);
        
        /// <summary>
        /// Enables or disables target clamping.
        /// </summary>
        /// <param name="enabled">Whether target clamping should be enabled.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor SetClampTarget(bool enabled);
        
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
        ISpringColor SetClampCurrentValue(bool enabled);
        
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
        ISpringColor SetStopOnClamp(bool enabled);
        
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
        /// <param name="minValues">The minimum values as a Vector4 (r, g, b, a) for clamping.</param>
        /// <param name="maxValues">The maximum values as a Vector4 (r, g, b, a) for clamping.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector4 minValues, Vector4 maxValues);
        
        /// <summary>
        /// Configures the spring in a single call.
        /// </summary>
        /// <param name="force">The force (stiffness) value as a Vector4 (r, g, b, a).</param>
        /// <param name="drag">The drag (damping) value as a Vector4 (r, g, b, a).</param>
        /// <param name="initialValue">The initial color of the spring.</param>
        /// <param name="target">The target color of the spring.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor Configure(Vector4 force, Vector4 drag, Color initialValue, Color target);
        
        /// <summary>
        /// Configures the spring in a single call using uniform values for force and drag.
        /// </summary>
        /// <param name="uniformForce">The uniform force to use for all components.</param>
        /// <param name="uniformDrag">The uniform drag to use for all components.</param>
        /// <param name="initialValue">The initial color of the spring.</param>
        /// <param name="target">The target color of the spring.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringColor Configure(float uniformForce, float uniformDrag, Color initialValue, Color target);
        
        /// <summary>
        /// Creates a new instance of ISpringColor with the same configuration.
        /// </summary>
        /// <returns>A new instance of ISpringColor with the same configuration.</returns>
        ISpringColor Clone();
    }
}

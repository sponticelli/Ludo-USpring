using UnityEngine;

namespace USpring.Core.Interfaces
{
    /// <summary>
    /// Interface for a four-dimensional spring that animates Vector4 values.
    /// </summary>
    public interface ISpringVector4 : ISpring
    {
        /// <summary>
        /// Sets the target value of the spring.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetTarget(Vector4 target);
        
        /// <summary>
        /// Sets the target value of the spring using a uniform value for all components.
        /// </summary>
        /// <param name="uniformValue">The uniform value to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetTarget(float uniformValue);
        
        /// <summary>
        /// Gets the target value of the spring.
        /// </summary>
        /// <returns>The target value.</returns>
        Vector4 GetTarget();
        
        /// <summary>
        /// Sets the current value of the spring.
        /// </summary>
        /// <param name="value">The current value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetCurrentValue(Vector4 value);
        
        /// <summary>
        /// Sets the current value of the spring using a uniform value for all components.
        /// </summary>
        /// <param name="uniformValue">The uniform value to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetCurrentValue(float uniformValue);
        
        /// <summary>
        /// Gets the current value of the spring.
        /// </summary>
        /// <returns>The current value.</returns>
        Vector4 GetCurrentValue();
        
        /// <summary>
        /// Sets the velocity of the spring.
        /// </summary>
        /// <param name="velocity">The velocity value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetVelocity(Vector4 velocity);
        
        /// <summary>
        /// Sets the velocity of the spring using a uniform value for all components.
        /// </summary>
        /// <param name="uniformVelocity">The uniform velocity to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetVelocity(float uniformVelocity);
        
        /// <summary>
        /// Gets the velocity of the spring.
        /// </summary>
        /// <returns>The velocity value.</returns>
        Vector4 GetVelocity();
        
        /// <summary>
        /// Adds to the current velocity of the spring.
        /// </summary>
        /// <param name="velocityToAdd">The velocity to add.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 AddVelocity(Vector4 velocityToAdd);
        
        /// <summary>
        /// Adds to the current velocity of the spring using a uniform value for all components.
        /// </summary>
        /// <param name="uniformVelocityToAdd">The uniform velocity to add to all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 AddVelocity(float uniformVelocityToAdd);
        
        /// <summary>
        /// Sets the force (stiffness) of the spring.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetForce(Vector4 force);
        
        /// <summary>
        /// Sets the force (stiffness) of the spring using a uniform value for all components.
        /// </summary>
        /// <param name="uniformForce">The uniform force to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetForce(float uniformForce);
        
        /// <summary>
        /// Gets the force (stiffness) of the spring.
        /// </summary>
        /// <returns>The force value.</returns>
        Vector4 GetForce();
        
        /// <summary>
        /// Sets the drag (damping) of the spring.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetDrag(Vector4 drag);
        
        /// <summary>
        /// Sets the drag (damping) of the spring using a uniform value for all components.
        /// </summary>
        /// <param name="uniformDrag">The uniform drag to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetDrag(float uniformDrag);
        
        /// <summary>
        /// Gets the drag (damping) of the spring.
        /// </summary>
        /// <returns>The drag value.</returns>
        Vector4 GetDrag();
        
        /// <summary>
        /// Sets the minimum values for clamping.
        /// </summary>
        /// <param name="minValues">The minimum values.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetMinValues(Vector4 minValues);
        
        /// <summary>
        /// Sets the minimum values for clamping using a uniform value for all components.
        /// </summary>
        /// <param name="uniformMinValue">The uniform minimum value to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetMinValues(float uniformMinValue);
        
        /// <summary>
        /// Gets the minimum values for clamping.
        /// </summary>
        /// <returns>The minimum values.</returns>
        Vector4 GetMinValues();
        
        /// <summary>
        /// Sets the maximum values for clamping.
        /// </summary>
        /// <param name="maxValues">The maximum values.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetMaxValues(Vector4 maxValues);
        
        /// <summary>
        /// Sets the maximum values for clamping using a uniform value for all components.
        /// </summary>
        /// <param name="uniformMaxValue">The uniform maximum value to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetMaxValues(float uniformMaxValue);
        
        /// <summary>
        /// Gets the maximum values for clamping.
        /// </summary>
        /// <returns>The maximum values.</returns>
        Vector4 GetMaxValues();
        
        /// <summary>
        /// Sets the minimum and maximum values for clamping in a single call.
        /// </summary>
        /// <param name="minValues">The minimum values.</param>
        /// <param name="maxValues">The maximum values.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetClampRange(Vector4 minValues, Vector4 maxValues);
        
        /// <summary>
        /// Sets the minimum and maximum values for clamping in a single call using uniform values.
        /// </summary>
        /// <param name="uniformMinValue">The uniform minimum value to use for all components.</param>
        /// <param name="uniformMaxValue">The uniform maximum value to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetClampRange(float uniformMinValue, float uniformMaxValue);
        
        /// <summary>
        /// Enables or disables target clamping.
        /// </summary>
        /// <param name="enabled">Whether target clamping should be enabled.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 SetClampTarget(bool enabled);
        
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
        ISpringVector4 SetClampCurrentValue(bool enabled);
        
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
        ISpringVector4 SetStopOnClamp(bool enabled);
        
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
        /// <param name="minValues">The minimum values for clamping.</param>
        /// <param name="maxValues">The maximum values for clamping.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector4 minValues, Vector4 maxValues);
        
        /// <summary>
        /// Configures the spring in a single call.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <param name="initialValue">The initial value of the spring.</param>
        /// <param name="target">The target value of the spring.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 Configure(Vector4 force, Vector4 drag, Vector4 initialValue, Vector4 target);
        
        /// <summary>
        /// Configures the spring in a single call using uniform values for force and drag.
        /// </summary>
        /// <param name="uniformForce">The uniform force to use for all components.</param>
        /// <param name="uniformDrag">The uniform drag to use for all components.</param>
        /// <param name="initialValue">The initial value of the spring.</param>
        /// <param name="target">The target value of the spring.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringVector4 Configure(float uniformForce, float uniformDrag, Vector4 initialValue, Vector4 target);
        
        /// <summary>
        /// Creates a new instance of ISpringVector4 with the same configuration.
        /// </summary>
        /// <returns>A new instance of ISpringVector4 with the same configuration.</returns>
        ISpringVector4 Clone();
    }
}

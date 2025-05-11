using UnityEngine;

namespace USpring.Core.Interfaces
{
    /// <summary>
    /// Interface for a spring that animates Quaternion rotations.
    /// </summary>
    public interface ISpringRotation : ISpring
    {
        /// <summary>
        /// Sets the target rotation of the spring.
        /// </summary>
        /// <param name="target">The target rotation as a Quaternion.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetTarget(Quaternion target);
        
        /// <summary>
        /// Sets the target rotation of the spring using Euler angles.
        /// </summary>
        /// <param name="eulerAngles">The target rotation as Euler angles.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetTarget(Vector3 eulerAngles);
        
        /// <summary>
        /// Gets the target rotation of the spring.
        /// </summary>
        /// <returns>The target rotation as a Quaternion.</returns>
        Quaternion GetTarget();
        
        /// <summary>
        /// Gets the target rotation of the spring as Euler angles.
        /// </summary>
        /// <returns>The target rotation as Euler angles.</returns>
        Vector3 GetTargetEuler();
        
        /// <summary>
        /// Sets the current rotation of the spring.
        /// </summary>
        /// <param name="value">The current rotation as a Quaternion.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetCurrentValue(Quaternion value);
        
        /// <summary>
        /// Sets the current rotation of the spring using Euler angles.
        /// </summary>
        /// <param name="eulerAngles">The current rotation as Euler angles.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetCurrentValue(Vector3 eulerAngles);
        
        /// <summary>
        /// Gets the current rotation of the spring.
        /// </summary>
        /// <returns>The current rotation as a Quaternion.</returns>
        Quaternion GetCurrentValue();
        
        /// <summary>
        /// Gets the current rotation of the spring as Euler angles.
        /// </summary>
        /// <returns>The current rotation as Euler angles.</returns>
        Vector3 GetCurrentValueEuler();
        
        /// <summary>
        /// Sets the angular velocity of the spring.
        /// </summary>
        /// <param name="velocity">The angular velocity as a Vector3.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetVelocity(Vector3 velocity);
        
        /// <summary>
        /// Gets the angular velocity of the spring.
        /// </summary>
        /// <returns>The angular velocity as a Vector3.</returns>
        Vector3 GetVelocity();
        
        /// <summary>
        /// Adds to the current angular velocity of the spring.
        /// </summary>
        /// <param name="velocityToAdd">The angular velocity to add.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation AddVelocity(Vector3 velocityToAdd);
        
        /// <summary>
        /// Sets the force (stiffness) of the spring.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetForce(Vector3 force);
        
        /// <summary>
        /// Sets the force (stiffness) of the spring using a uniform value for all components.
        /// </summary>
        /// <param name="uniformForce">The uniform force to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetForce(float uniformForce);
        
        /// <summary>
        /// Gets the force (stiffness) of the spring.
        /// </summary>
        /// <returns>The force value.</returns>
        Vector3 GetForce();
        
        /// <summary>
        /// Sets the drag (damping) of the spring.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetDrag(Vector3 drag);
        
        /// <summary>
        /// Sets the drag (damping) of the spring using a uniform value for all components.
        /// </summary>
        /// <param name="uniformDrag">The uniform drag to use for all components.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetDrag(float uniformDrag);
        
        /// <summary>
        /// Gets the drag (damping) of the spring.
        /// </summary>
        /// <returns>The drag value.</returns>
        Vector3 GetDrag();
        
        /// <summary>
        /// Sets the minimum values for clamping in Euler angles.
        /// </summary>
        /// <param name="minValues">The minimum values in Euler angles.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetMinValues(Vector3 minValues);
        
        /// <summary>
        /// Gets the minimum values for clamping in Euler angles.
        /// </summary>
        /// <returns>The minimum values in Euler angles.</returns>
        Vector3 GetMinValues();
        
        /// <summary>
        /// Sets the maximum values for clamping in Euler angles.
        /// </summary>
        /// <param name="maxValues">The maximum values in Euler angles.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetMaxValues(Vector3 maxValues);
        
        /// <summary>
        /// Gets the maximum values for clamping in Euler angles.
        /// </summary>
        /// <returns>The maximum values in Euler angles.</returns>
        Vector3 GetMaxValues();
        
        /// <summary>
        /// Sets the minimum and maximum values for clamping in Euler angles in a single call.
        /// </summary>
        /// <param name="minValues">The minimum values in Euler angles.</param>
        /// <param name="maxValues">The maximum values in Euler angles.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetClampRange(Vector3 minValues, Vector3 maxValues);
        
        /// <summary>
        /// Enables or disables target clamping.
        /// </summary>
        /// <param name="enabled">Whether target clamping should be enabled.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation SetClampTarget(bool enabled);
        
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
        ISpringRotation SetClampCurrentValue(bool enabled);
        
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
        ISpringRotation SetStopOnClamp(bool enabled);
        
        /// <summary>
        /// Gets whether the spring stops when the current value is clamped.
        /// </summary>
        /// <returns>True if the spring stops when the current value is clamped, false otherwise.</returns>
        bool GetStopOnClamp();
        
        /// <summary>
        /// Configures clamping settings in a single call.
        /// </summary>
        /// <param name="clampTarget">Whether to clamp the target value.</param>
        /// <param name="clampCurrentValue">Whether to clamp the current value.</param>
        /// <param name="stopOnClamp">Whether to stop the spring when the current value is clamped.</param>
        /// <param name="minValues">The minimum values in Euler angles for clamping.</param>
        /// <param name="maxValues">The maximum values in Euler angles for clamping.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector3 minValues, Vector3 maxValues);
        
        /// <summary>
        /// Configures the spring in a single call.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <param name="initialValue">The initial rotation of the spring.</param>
        /// <param name="target">The target rotation of the spring.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation Configure(Vector3 force, Vector3 drag, Quaternion initialValue, Quaternion target);
        
        /// <summary>
        /// Configures the spring in a single call using uniform values for force and drag.
        /// </summary>
        /// <param name="uniformForce">The uniform force to use for all components.</param>
        /// <param name="uniformDrag">The uniform drag to use for all components.</param>
        /// <param name="initialValue">The initial rotation of the spring.</param>
        /// <param name="target">The target rotation of the spring.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringRotation Configure(float uniformForce, float uniformDrag, Quaternion initialValue, Quaternion target);
        
        /// <summary>
        /// Creates a new instance of ISpringRotation with the same configuration.
        /// </summary>
        /// <returns>A new instance of ISpringRotation with the same configuration.</returns>
        ISpringRotation Clone();
    }
}

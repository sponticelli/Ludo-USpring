using UnityEngine;

namespace USpring.Core.Interfaces
{
    /// <summary>
    /// Base interface for all spring types in the USpring system.
    /// Defines the common functionality shared by all springs.
    /// </summary>
    public interface ISpring
    {
        /// <summary>
        /// Initializes the spring with default values.
        /// </summary>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring Initialize();
        
        /// <summary>
        /// Updates the spring physics for the given time step.
        /// This method handles the complete update process including:
        /// - Updating the spring physics
        /// - Processing candidate values
        /// - Checking events
        /// </summary>
        /// <param name="deltaTime">The time step to use for the update.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring Update(float deltaTime);
        
        /// <summary>
        /// Updates the spring physics for the given time step with a custom force threshold.
        /// </summary>
        /// <param name="deltaTime">The time step to use for the update.</param>
        /// <param name="forceThreshold">The threshold for switching from semi-implicit to analytical integration.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring Update(float deltaTime, float forceThreshold);
        
        /// <summary>
        /// Immediately sets the spring to its target value and stops all motion.
        /// </summary>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring ReachEquilibrium();
        
        /// <summary>
        /// Enables or disables the spring.
        /// </summary>
        /// <param name="enabled">Whether the spring should be enabled.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring SetEnabled(bool enabled);
        
        /// <summary>
        /// Gets whether the spring is enabled.
        /// </summary>
        /// <returns>True if the spring is enabled, false otherwise.</returns>
        bool IsEnabled();
        
        /// <summary>
        /// Enables or disables common force and drag for all axes.
        /// </summary>
        /// <param name="enabled">Whether to use common force and drag.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring SetCommonForceAndDrag(bool enabled);
        
        /// <summary>
        /// Sets the common force (stiffness) value for all axes.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring SetCommonForce(float force);
        
        /// <summary>
        /// Gets the common force (stiffness) value.
        /// </summary>
        /// <returns>The common force value.</returns>
        float GetCommonForce();
        
        /// <summary>
        /// Sets the common drag (damping) value for all axes.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring SetCommonDrag(float drag);
        
        /// <summary>
        /// Gets the common drag (damping) value.
        /// </summary>
        /// <returns>The common drag value.</returns>
        float GetCommonDrag();
        
        /// <summary>
        /// Sets both common force and drag values in a single call.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <param name="drag">The drag value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring SetCommonForceAndDragValues(float force, float drag);
        
        /// <summary>
        /// Enables or disables clamping for the spring.
        /// </summary>
        /// <param name="enabled">Whether clamping should be enabled.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring SetClampingEnabled(bool enabled);
        
        /// <summary>
        /// Gets whether clamping is enabled for the spring.
        /// </summary>
        /// <returns>True if clamping is enabled, false otherwise.</returns>
        bool IsClampingEnabled();
        
        /// <summary>
        /// Enables or disables events for the spring.
        /// </summary>
        /// <param name="enabled">Whether events should be enabled.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpring SetEventsEnabled(bool enabled);
        
        /// <summary>
        /// Gets whether events are enabled for the spring.
        /// </summary>
        /// <returns>True if events are enabled, false otherwise.</returns>
        bool AreEventsEnabled();
        
        /// <summary>
        /// Gets the spring events instance for this spring.
        /// </summary>
        /// <returns>The spring events instance.</returns>
        SpringEvents GetEvents();
        
        /// <summary>
        /// Determines if the spring is close to stopping (velocity is below threshold).
        /// </summary>
        /// <returns>True if the spring is close to stopping, false otherwise.</returns>
        bool IsCloseToStopping();
        
        /// <summary>
        /// Determines if the spring has reached its target (current value is close enough to target).
        /// </summary>
        /// <returns>True if the spring has reached its target, false otherwise.</returns>
        bool IsOnTarget();
        
        /// <summary>
        /// Determines if the spring is currently clamped.
        /// </summary>
        /// <returns>True if the spring is clamped, false otherwise.</returns>
        bool IsClamped();
    }
}

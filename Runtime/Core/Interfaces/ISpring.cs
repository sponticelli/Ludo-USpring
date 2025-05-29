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
    }

    /// <summary>
    /// Generic interface for typed spring values.
    /// Provides type-safe access to spring properties and methods.
    /// </summary>
    /// <typeparam name="T">The type of value this spring manages (float, Vector3, etc.)</typeparam>
    public interface ISpringValue<T> : ISpring
    {
        /// <summary>
        /// Gets the current target value of the spring.
        /// </summary>
        /// <returns>The target value.</returns>
        T GetTarget();

        /// <summary>
        /// Sets the target value that the spring will move towards.
        /// </summary>
        /// <param name="target">The new target value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringValue<T> SetTarget(T target);

        /// <summary>
        /// Gets the current animated value of the spring.
        /// </summary>
        /// <returns>The current value.</returns>
        T GetCurrentValue();

        /// <summary>
        /// Sets the current value of the spring.
        /// </summary>
        /// <param name="currentValue">The new current value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringValue<T> SetCurrentValue(T currentValue);

        /// <summary>
        /// Gets the current velocity of the spring.
        /// </summary>
        /// <returns>The velocity value.</returns>
        T GetVelocity();

        /// <summary>
        /// Sets the velocity of the spring.
        /// </summary>
        /// <param name="velocity">The new velocity value.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringValue<T> SetVelocity(T velocity);

        /// <summary>
        /// Adds velocity to the current spring velocity.
        /// </summary>
        /// <param name="velocity">The velocity to add.</param>
        /// <returns>The spring instance for method chaining.</returns>
        ISpringValue<T> AddVelocity(T velocity);

        /// <summary>
        /// Gets the spring events for this spring.
        /// </summary>
        SpringEvents Events { get; }
    }
}

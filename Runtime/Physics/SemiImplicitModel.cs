using UnityEngine;
using USpring.Core;

namespace USpring.Physics
{
    /// <summary>
    /// Physics model that uses semi-implicit Euler integration.
    /// </summary>
    /// <remarks>
    /// Semi-implicit Euler integration is more stable than explicit Euler
    /// and computationally efficient for real-time applications.
    /// It's the primary method used for most spring updates.
    /// </remarks>
    public class SemiImplicitModel : IPhysicsModel
    {
        /// <summary>
        /// Updates the spring values using semi-implicit Euler integration.
        /// </summary>
        /// <param name="deltaTime">The time step to use for the update.</param>
        /// <param name="springValues">The spring values to update.</param>
        /// <param name="parameters">The physics parameters to use for the update.</param>
        public void UpdateSpringValues(float deltaTime, SpringValues springValues, PhysicsParameters parameters)
        {
            // Skip integration if deltaTime is zero
            if (deltaTime <= 0f)
            {
                return;
            }
            
            // Calculate distance to target (the "spring extension")
            float distanceToTarget = springValues.GetTarget() - springValues.GetCurrentValue();
            float currentVelocity = springValues.GetVelocity();

            // Semi-implicit integration step 1: Apply drag to velocity first
            // This is done as: v' = v/(1+drag*dt) which approximates v' = v*e^(-drag*dt)
            // It's a more stable form than explicit drag reduction: v' = v*(1-drag*dt)
            float dragFactor = 1.0f / (1.0f + parameters.Drag * deltaTime);
            currentVelocity *= dragFactor;

            // Semi-implicit integration step 2: Calculate spring force
            // F = -k*x where k is spring constant (force) and x is displacement
            float springForce = parameters.Force * distanceToTarget;

            // Semi-implicit integration step 3: Update velocity with spring force
            // v' = v + a*dt where a = F/m (mass is assumed to be 1)
            currentVelocity += springForce * deltaTime;

            // Semi-implicit integration step 4: Update position with new velocity
            // x' = x + v'*dt (using new velocity for semi-implicit)
            float newCandidateValue = springValues.GetCurrentValue() + currentVelocity * deltaTime;

            // Check for NaN or Infinity to prevent system instability
            if (!float.IsFinite(newCandidateValue) || !float.IsFinite(currentVelocity))
            {
                springValues.ReachEquilibrium();
                return;
            }

            // Update spring state
            springValues.SetVelocity(currentVelocity);
            springValues.SetCandidateValue(newCandidateValue);
        }

        /// <summary>
        /// Gets the name of the physics model.
        /// </summary>
        /// <returns>The name of the physics model.</returns>
        public string GetModelName()
        {
            return "Semi-Implicit Euler";
        }

        /// <summary>
        /// Gets a description of the physics model.
        /// </summary>
        /// <returns>A description of the physics model.</returns>
        public string GetModelDescription()
        {
            return "Semi-implicit Euler integration is more stable than explicit Euler " +
                   "and computationally efficient for real-time applications. " +
                   "It's the primary method used for most spring updates.";
        }

        /// <summary>
        /// Gets whether the physics model is suitable for the given parameters.
        /// </summary>
        /// <param name="parameters">The physics parameters to check.</param>
        /// <returns>True if the model is suitable, false otherwise.</returns>
        public bool IsSuitableForParameters(PhysicsParameters parameters)
        {
            // Semi-implicit is suitable for most cases, but not for extreme forces
            return parameters.Force <= parameters.IntegrationParameters.ForceThreshold;
        }
    }
}

using System.Runtime.CompilerServices;
using UnityEngine;
using USpring.Core;

namespace USpring.Physics
{
    /// <summary>
    /// Physics model that uses an analytical solution based on the damping ratio.
    /// </summary>
    /// <remarks>
    /// The analytical solution is more stable but computationally expensive.
    /// It's used for extreme force values where semi-implicit integration might become unstable.
    /// </remarks>
    public class AnalyticalModel : IPhysicsModel
    {
        /// <summary>
        /// Updates the spring values using an analytical solution based on the damping ratio.
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

            // Calculate angular frequency (ω) and damping ratio (ζ)
            float angularFrequency = Mathf.Sqrt(parameters.Force);
            float dampingRatio = parameters.Drag / (2f * angularFrequency);

            // Calculate analytical solution coefficients
            SpringAnalyticalSolution solution = CalculateSpringAnalyticalSolution(deltaTime, angularFrequency, dampingRatio);

            // Current position and velocity (relative to target)
            float currentPosRelative = springValues.GetCurrentValue() - springValues.GetTarget();
            float currentVelocity = springValues.GetVelocity();

            // Apply analytical solution to get new position and velocity
            float newPosition = (currentPosRelative * solution.PositionFactor) +
                                (currentVelocity * solution.VelocityFactor) +
                                springValues.GetTarget();

            float newVelocity = (currentPosRelative * solution.PositionToVelocityFactor) +
                                (currentVelocity * solution.VelocityDecayFactor);

            // Safety check for non-finite values
            if (!float.IsFinite(newPosition) || !float.IsFinite(newVelocity))
            {
                springValues.ReachEquilibrium();
                return;
            }

            // Update spring state
            springValues.SetVelocity(newVelocity);
            springValues.SetCandidateValue(newPosition);
        }

        /// <summary>
        /// Gets the name of the physics model.
        /// </summary>
        /// <returns>The name of the physics model.</returns>
        public string GetModelName()
        {
            return "Analytical Solution";
        }

        /// <summary>
        /// Gets a description of the physics model.
        /// </summary>
        /// <returns>A description of the physics model.</returns>
        public string GetModelDescription()
        {
            return "The analytical solution is more stable but computationally expensive. " +
                   "It's used for extreme force values where semi-implicit integration might become unstable.";
        }

        /// <summary>
        /// Gets whether the physics model is suitable for the given parameters.
        /// </summary>
        /// <param name="parameters">The physics parameters to check.</param>
        /// <returns>True if the model is suitable, false otherwise.</returns>
        public bool IsSuitableForParameters(PhysicsParameters parameters)
        {
            // Analytical is suitable for extreme forces or when explicitly requested
            return parameters.Force > parameters.IntegrationParameters.ForceThreshold ||
                   parameters.IntegrationParameters.AlwaysUseAnalyticalSolution;
        }

        /// <summary>
        /// Calculates the analytical solution coefficients for the spring.
        /// </summary>
        /// <param name="deltaTime">The time step to use for the calculation.</param>
        /// <param name="angularFrequency">The angular frequency of the spring.</param>
        /// <param name="dampingRatio">The damping ratio of the spring.</param>
        /// <returns>The analytical solution coefficients.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SpringAnalyticalSolution CalculateSpringAnalyticalSolution(float deltaTime,
            float angularFrequency, float dampingRatio)
        {
            const float epsilon = 0.0001f;

            // Force values into legal range
            if (dampingRatio < 0.0f) dampingRatio = 0.0f;
            if (angularFrequency < 0.0f) angularFrequency = 0.0f;

            // If there is no angular frequency, the spring will not move
            if (angularFrequency < epsilon)
            {
                return new SpringAnalyticalSolution(1.0f, 0.0f, 0.0f, 1.0f);
            }

            float positionFactor, velocityFactor, positionToVelocityFactor, velocityDecayFactor;

            if (dampingRatio > 1.0f + epsilon) // Over-damped
            {
                // Calculate constants for over-damped solution
                float za = -angularFrequency * dampingRatio;
                float zb = angularFrequency * Mathf.Sqrt(dampingRatio * dampingRatio - 1.0f);
                float z1 = za - zb;
                float z2 = za + zb;

                float expTerm1 = Mathf.Exp(z1 * deltaTime);
                float expTerm2 = Mathf.Exp(z2 * deltaTime);

                // Calculate solution factors
                float invTwiceZb = 1.0f / (2.0f * zb);
                positionFactor = (z2 * expTerm1 - z1 * expTerm2) * invTwiceZb;
                velocityFactor = (expTerm1 - expTerm2) * invTwiceZb;
                positionToVelocityFactor = (z1 * z2 * (expTerm2 - expTerm1)) * invTwiceZb;
                velocityDecayFactor = (z1 * expTerm2 - z2 * expTerm1) * invTwiceZb;
            }
            else if (dampingRatio < 1.0f - epsilon) // Under-damped
            {
                // Calculate constants for under-damped solution
                float omegaZeta = angularFrequency * dampingRatio;
                float alpha = angularFrequency * Mathf.Sqrt(1.0f - dampingRatio * dampingRatio);

                float expTerm = Mathf.Exp(-omegaZeta * deltaTime);
                float cosTerm = Mathf.Cos(alpha * deltaTime);
                float sinTerm = Mathf.Sin(alpha * deltaTime);

                // Calculate solution factors
                float invAlpha = 1.0f / alpha;
                positionFactor = expTerm * (cosTerm + (omegaZeta * sinTerm * invAlpha));
                velocityFactor = expTerm * sinTerm * invAlpha;
                positionToVelocityFactor = -expTerm * (sinTerm * alpha + omegaZeta * cosTerm);
                velocityDecayFactor = expTerm * (cosTerm - (omegaZeta * sinTerm * invAlpha));
            }
            else // Critically damped
            {
                // Calculate constants for critically damped solution
                float expTerm = Mathf.Exp(-angularFrequency * deltaTime);
                float timeExp = deltaTime * expTerm;

                // Calculate solution factors
                positionFactor = expTerm * (1.0f + angularFrequency * deltaTime);
                velocityFactor = timeExp;
                positionToVelocityFactor = -angularFrequency * angularFrequency * timeExp;
                velocityDecayFactor = expTerm * (1.0f - angularFrequency * deltaTime);
            }

            return new SpringAnalyticalSolution(positionFactor, velocityFactor, positionToVelocityFactor, velocityDecayFactor);
        }

        /// <summary>
        /// Struct containing the coefficients for the analytical solution.
        /// </summary>
        private readonly struct SpringAnalyticalSolution
        {
            /// <summary>
            /// Factor applied to the current position.
            /// </summary>
            public readonly float PositionFactor;

            /// <summary>
            /// Factor applied to the current velocity.
            /// </summary>
            public readonly float VelocityFactor;

            /// <summary>
            /// Factor for converting position to velocity.
            /// </summary>
            public readonly float PositionToVelocityFactor;

            /// <summary>
            /// Factor for velocity decay.
            /// </summary>
            public readonly float VelocityDecayFactor;

            /// <summary>
            /// Creates a new instance of SpringAnalyticalSolution.
            /// </summary>
            /// <param name="positionFactor">Factor applied to the current position.</param>
            /// <param name="velocityFactor">Factor applied to the current velocity.</param>
            /// <param name="positionToVelocityFactor">Factor for converting position to velocity.</param>
            /// <param name="velocityDecayFactor">Factor for velocity decay.</param>
            public SpringAnalyticalSolution(
                float positionFactor,
                float velocityFactor,
                float positionToVelocityFactor,
                float velocityDecayFactor)
            {
                PositionFactor = positionFactor;
                VelocityFactor = velocityFactor;
                PositionToVelocityFactor = positionToVelocityFactor;
                VelocityDecayFactor = velocityDecayFactor;
            }
        }
    }
}

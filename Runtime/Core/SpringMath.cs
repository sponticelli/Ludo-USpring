using UnityEngine;
using USpring.Physics;

namespace USpring.Core
{
    /// <summary>
    /// SpringMath provides the mathematical implementation for physics-based spring animations.
    /// </summary>
    /// <remarks>
    /// <para><b>SPRING PHYSICS OVERVIEW:</b></para>
    /// <para>The library implements a damped harmonic oscillator, which follows the second-order differential equation:</para>
    /// <code>ẍ + 2ζω₀ẋ + ω₀²x = ω₀²F</code>
    /// <list type="bullet">
    /// <item><description>x = position (distance from equilibrium)</description></item>
    /// <item><description>ẋ = velocity (first derivative of position)</description></item>
    /// <item><description>ẍ = acceleration (second derivative of position)</description></item>
    /// <item><description>ω₀ = angular frequency (√force in this implementation)</description></item>
    /// <item><description>ζ = damping ratio (drag/(2√force) in this implementation)</description></item>
    /// <item><description>F = external force or target position</description></item>
    /// </list>
    ///
    /// <para><b>INTEGRATION METHODS:</b></para>
    /// <list type="number">
    /// <item>
    /// <description><b>Semi-Implicit Euler (Default Method)</b></description>
    /// <para>The primary numerical integration method used for most cases. It's more stable than explicit Euler
    /// and computationally efficient for real-time applications.</para>
    /// <para>Implementation steps:</para>
    /// <list type="bullet">
    /// <item><description>Apply drag to current velocity: <c>v_drag = v / (1 + drag * dt)</c></description></item>
    /// <item><description>Calculate spring force: <c>f = force * (target - current)</c></description></item>
    /// <item><description>Update velocity with force: <c>v_new = v_drag + f * dt</c></description></item>
    /// <item><description>Update position with new velocity: <c>x_new = x + v_new * dt</c></description></item>
    /// </list>
    /// </item>
    /// <item>
    /// <description><b>Analytical Solution (For High Forces)</b></description>
    /// <para>For extreme spring forces where numerical stability might be an issue, the system
    /// switches to an analytical solution based on the damping ratio classification:</para>
    /// <list type="bullet">
    /// <item><description>Over-damped (ζ > 1): System returns to equilibrium without oscillation</description></item>
    /// <item><description>Under-damped (ζ < 1): System oscillates with decreasing amplitude</description></item>
    /// <item><description>Critically damped (ζ = 1): Fastest return to equilibrium without oscillation</description></item>
    /// </list>
    /// <para>The analytical solution computes position and velocity coefficients based on:</para>
    /// <code>
    /// x(t) = (current_pos * pos_factor) + (current_vel * vel_factor) + target
    /// v(t) = (current_pos * pos_to_vel_factor) + (current_vel * vel_decay_factor)
    /// </code>
    /// </item>
    /// </list>
    ///
    /// <para><b>DAMPING RATIO CLASSIFICATIONS:</b></para>
    /// <list type="number">
    /// <item>
    /// <description><b>Over-damped (ζ > 1):</b></description>
    /// <para>Solution uses two real exponential terms with different decay rates:</para>
    /// <code>
    /// z1, z2 = -ω₀ζ ∓ ω₀√(ζ² - 1)
    /// x(t) = c₁e^(z₁t) + c₂e^(z₂t)
    /// </code>
    /// </item>
    /// <item>
    /// <description><b>Under-damped (ζ < 1):</b></description>
    /// <para>Solution uses exponentially decaying sinusoids:</para>
    /// <code>
    /// α = ω₀√(1 - ζ²)  [damped angular frequency]
    /// x(t) = e^(-ζω₀t) * (A·cos(αt) + B·sin(αt))
    /// </code>
    /// </item>
    /// <item>
    /// <description><b>Critically damped (ζ = 1):</b></description>
    /// <para>Solution uses a modified exponential form:</para>
    /// <code>
    /// x(t) = (c₁ + c₂t)e^(-ω₀t)
    /// </code>
    /// </item>
    /// </list>
    ///
    /// <para><b>SAFETY FEATURES:</b></para>
    /// <list type="bullet">
    /// <item><description>Value clamping: Enforces minimum and maximum boundaries for positions</description></item>
    /// <item><description>Velocity clamping: Can optionally stop spring motion when position hits boundaries</description></item>
    /// <item><description>Non-finite detection: Handles numerical instabilities by setting springs to equilibrium</description></item>
    /// </list>
    /// </remarks>
    public static class SpringMath
    {
        /// <summary>
        /// Updates the spring physics for the given time step.
        /// </summary>
        /// <param name="deltaTime">The time step to use for the update.</param>
        /// <param name="spring">The spring to update.</param>
        /// <param name="forceThreshold">The threshold for switching from semi-implicit to analytical integration.</param>
        public static void UpdateSpring(float deltaTime, Spring spring, float forceThreshold = 7500f)
        {
            for (int i = 0; i < spring.springValues.Length; i++)
            {
                if (!spring.springValues[i].IsEnabled())
                {
                    continue;
                }

                // Get cached physics parameters
                PhysicsParameters parameters = spring.GetPhysicsParameters(i, forceThreshold);


                // Apply target clamping if enabled
                if (spring.clampingEnabled && spring.springValues[i].GetClampTarget())
                {
                    spring.springValues[i].ClampTarget();
                }

                try
                {
                    // Get the most suitable physics model for the parameters
                    IPhysicsModel model = PhysicsModelFactory.GetSuitableModel(parameters);

                    // Update spring values using the selected model
                    model.UpdateSpringValues(deltaTime, spring.springValues[i], parameters);

                    // Apply current value clamping if enabled
                    if (spring.clampingEnabled)
                    {
                        CheckCurrentValueClamping(spring.springValues[i]);
                    }
                }
                catch (PhysicsException ex)
                {
                    Debug.LogError($"Physics error in spring {spring.GetType().Name}: {ex.Message}");
                    spring.springValues[i].ReachEquilibrium();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Unexpected error in spring {spring.GetType().Name}: {ex.Message}");
                    spring.springValues[i].ReachEquilibrium();
                }
            }
        }

        /// <summary>
        /// Applies clamping to the current value if enabled.
        /// </summary>
        /// <param name="springValues">The spring values to check.</param>
        private static void CheckCurrentValueClamping(SpringValues springValues)
        {
            if (!springValues.IsOvershot() || !springValues.GetClampCurrentValue()) return;

            springValues.Clamp();

            if (springValues.GetStopOnClamp())
            {
                springValues.Stop();
            }
        }
    }
}
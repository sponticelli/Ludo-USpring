using System;
using UnityEngine;

namespace USpring.Physics
{
    /// <summary>
    /// Parameters for configuring spring integration.
    /// </summary>
    /// <remarks>
    /// This class encapsulates parameters related to the numerical integration
    /// method used for updating spring values.
    /// </remarks>
    [Serializable]
    public class IntegrationParameters
    {
        /// <summary>
        /// The threshold for switching from semi-implicit to analytical integration.
        /// When the force exceeds this value, the analytical solution is used for stability.
        /// </summary>
        [SerializeField] private float forceThreshold = 7500f;
        
        /// <summary>
        /// Whether to use a fixed update rate for spring simulation.
        /// </summary>
        [SerializeField] private bool useFixedUpdateRate = true;
        
        /// <summary>
        /// The fixed time step to use for spring simulation when useFixedUpdateRate is true.
        /// </summary>
        [SerializeField] private float fixedTimeStep = 0.02f;
        
        /// <summary>
        /// Whether to always use the analytical solution regardless of force.
        /// </summary>
        [SerializeField] private bool alwaysUseAnalyticalSolution = false;
        
        /// <summary>
        /// Gets or sets the threshold for switching from semi-implicit to analytical integration.
        /// </summary>
        public float ForceThreshold
        {
            get => forceThreshold;
            set => forceThreshold = value;
        }
        
        /// <summary>
        /// Gets or sets whether to use a fixed update rate for spring simulation.
        /// </summary>
        public bool UseFixedUpdateRate
        {
            get => useFixedUpdateRate;
            set => useFixedUpdateRate = value;
        }
        
        /// <summary>
        /// Gets or sets the fixed time step to use for spring simulation.
        /// </summary>
        public float FixedTimeStep
        {
            get => fixedTimeStep;
            set => fixedTimeStep = value;
        }
        
        /// <summary>
        /// Gets or sets whether to always use the analytical solution regardless of force.
        /// </summary>
        public bool AlwaysUseAnalyticalSolution
        {
            get => alwaysUseAnalyticalSolution;
            set => alwaysUseAnalyticalSolution = value;
        }
        
        /// <summary>
        /// Creates a copy of the integration parameters.
        /// </summary>
        /// <returns>A new instance of IntegrationParameters with the same values.</returns>
        public IntegrationParameters Clone()
        {
            return new IntegrationParameters
            {
                forceThreshold = forceThreshold,
                useFixedUpdateRate = useFixedUpdateRate,
                fixedTimeStep = fixedTimeStep,
                alwaysUseAnalyticalSolution = alwaysUseAnalyticalSolution
            };
        }
    }
}

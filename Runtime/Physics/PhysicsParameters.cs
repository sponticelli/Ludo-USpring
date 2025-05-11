using System;
using UnityEngine;

namespace USpring.Physics
{
    /// <summary>
    /// Parameters for configuring spring physics.
    /// </summary>
    /// <remarks>
    /// This class encapsulates all parameters needed for spring physics calculations,
    /// including force (stiffness), drag (damping), and integration settings.
    /// </remarks>
    [Serializable]
    public class PhysicsParameters
    {
        /// <summary>
        /// The force (stiffness) of the spring.
        /// Higher values make the spring stiffer and respond more quickly.
        /// </summary>
        [SerializeField] private float force = 150f;
        
        /// <summary>
        /// The drag (damping) of the spring.
        /// Higher values reduce oscillation but make the spring slower to reach its target.
        /// </summary>
        [SerializeField] private float drag = 10f;
        
        /// <summary>
        /// The integration parameters for the spring.
        /// </summary>
        [SerializeField] private IntegrationParameters integrationParameters = new IntegrationParameters();
        
        /// <summary>
        /// The clamping parameters for the spring.
        /// </summary>
        [SerializeField] private ClampingParameters clampingParameters = new ClampingParameters();
        
        /// <summary>
        /// Gets or sets the force (stiffness) of the spring.
        /// </summary>
        public float Force
        {
            get => force;
            set => force = value;
        }
        
        /// <summary>
        /// Gets or sets the drag (damping) of the spring.
        /// </summary>
        public float Drag
        {
            get => drag;
            set => drag = value;
        }
        
        /// <summary>
        /// Gets the integration parameters for the spring.
        /// </summary>
        public IntegrationParameters IntegrationParameters => integrationParameters;
        
        /// <summary>
        /// Gets the clamping parameters for the spring.
        /// </summary>
        public ClampingParameters ClampingParameters => clampingParameters;
        
        /// <summary>
        /// Creates a new instance of PhysicsParameters with default values.
        /// </summary>
        public PhysicsParameters()
        {
        }
        
        /// <summary>
        /// Creates a new instance of PhysicsParameters with the specified force and drag.
        /// </summary>
        /// <param name="force">The force (stiffness) of the spring.</param>
        /// <param name="drag">The drag (damping) of the spring.</param>
        public PhysicsParameters(float force, float drag)
        {
            this.force = force;
            this.drag = drag;
        }
        
        /// <summary>
        /// Creates a copy of the physics parameters.
        /// </summary>
        /// <returns>A new instance of PhysicsParameters with the same values.</returns>
        public PhysicsParameters Clone()
        {
            return new PhysicsParameters
            {
                force = force,
                drag = drag,
                integrationParameters = integrationParameters.Clone(),
                clampingParameters = clampingParameters.Clone()
            };
        }
    }
}

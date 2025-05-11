using USpring.Core;

namespace USpring.Physics
{
    /// <summary>
    /// Interface for physics models that can be used to update spring values.
    /// </summary>
    /// <remarks>
    /// Physics models implement different integration methods for updating spring values.
    /// Each model has its own strengths and weaknesses in terms of stability, accuracy, and performance.
    /// </remarks>
    public interface IPhysicsModel
    {
        /// <summary>
        /// Updates the spring values using the physics model.
        /// </summary>
        /// <param name="deltaTime">The time step to use for the update.</param>
        /// <param name="springValues">The spring values to update.</param>
        /// <param name="parameters">The physics parameters to use for the update.</param>
        void UpdateSpringValues(float deltaTime, SpringValues springValues, PhysicsParameters parameters);
        
        /// <summary>
        /// Gets the name of the physics model.
        /// </summary>
        /// <returns>The name of the physics model.</returns>
        string GetModelName();
        
        /// <summary>
        /// Gets a description of the physics model.
        /// </summary>
        /// <returns>A description of the physics model.</returns>
        string GetModelDescription();
        
        /// <summary>
        /// Gets whether the physics model is suitable for the given parameters.
        /// </summary>
        /// <param name="parameters">The physics parameters to check.</param>
        /// <returns>True if the model is suitable, false otherwise.</returns>
        bool IsSuitableForParameters(PhysicsParameters parameters);
    }
}

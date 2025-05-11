using System.Collections.Generic;

namespace USpring.Physics
{
    /// <summary>
    /// Factory for creating physics models.
    /// </summary>
    /// <remarks>
    /// This class provides methods for creating and selecting physics models
    /// based on the physics parameters.
    /// </remarks>
    public static class PhysicsModelFactory
    {
        private static readonly List<IPhysicsModel> registeredModels = new List<IPhysicsModel>();
        
        // Initialize the factory with default models
        static PhysicsModelFactory()
        {
            RegisterModel(new SemiImplicitModel());
            RegisterModel(new AnalyticalModel());
        }
        
        /// <summary>
        /// Registers a physics model with the factory.
        /// </summary>
        /// <param name="model">The physics model to register.</param>
        public static void RegisterModel(IPhysicsModel model)
        {
            if (!registeredModels.Contains(model))
            {
                registeredModels.Add(model);
            }
        }
        
        /// <summary>
        /// Gets all registered physics models.
        /// </summary>
        /// <returns>A list of all registered physics models.</returns>
        public static List<IPhysicsModel> GetAllModels()
        {
            return new List<IPhysicsModel>(registeredModels);
        }
        
        /// <summary>
        /// Gets the most suitable physics model for the given parameters.
        /// </summary>
        /// <param name="parameters">The physics parameters to use for selecting a model.</param>
        /// <returns>The most suitable physics model.</returns>
        public static IPhysicsModel GetSuitableModel(PhysicsParameters parameters)
        {
            // If always use analytical solution is enabled, return the analytical model
            if (parameters.IntegrationParameters.AlwaysUseAnalyticalSolution)
            {
                foreach (var model in registeredModels)
                {
                    if (model is AnalyticalModel)
                    {
                        return model;
                    }
                }
            }
            
            // Check if force exceeds threshold
            if (parameters.Force > parameters.IntegrationParameters.ForceThreshold)
            {
                foreach (var model in registeredModels)
                {
                    if (model is AnalyticalModel)
                    {
                        return model;
                    }
                }
            }
            
            // Default to semi-implicit model
            foreach (var model in registeredModels)
            {
                if (model is SemiImplicitModel)
                {
                    return model;
                }
            }
            
            // If no suitable model is found, return the first registered model
            return registeredModels.Count > 0 ? registeredModels[0] : new SemiImplicitModel();
        }
        
        /// <summary>
        /// Creates a new instance of the semi-implicit model.
        /// </summary>
        /// <returns>A new instance of the semi-implicit model.</returns>
        public static IPhysicsModel CreateSemiImplicitModel()
        {
            return new SemiImplicitModel();
        }
        
        /// <summary>
        /// Creates a new instance of the analytical model.
        /// </summary>
        /// <returns>A new instance of the analytical model.</returns>
        public static IPhysicsModel CreateAnalyticalModel()
        {
            return new AnalyticalModel();
        }
    }
}

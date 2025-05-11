using System.Collections.Generic;
using USpring.Core;

namespace USpring.Physics
{
    /// <summary>
    /// Validator for physics parameters and spring values.
    /// </summary>
    /// <remarks>
    /// This class provides methods for validating physics parameters and spring values
    /// to ensure they are within valid ranges and configurations.
    /// </remarks>
    public static class PhysicsValidator
    {
        /// <summary>
        /// Validates the physics parameters.
        /// </summary>
        /// <param name="parameters">The physics parameters to validate.</param>
        /// <returns>A list of validation errors, or an empty list if the parameters are valid.</returns>
        public static List<string> ValidateParameters(PhysicsParameters parameters)
        {
            var errors = new List<string>();
            
            // Validate force
            if (parameters.Force < 0f)
            {
                errors.Add($"Force must be non-negative. Current value: {parameters.Force}");
            }
            
            // Validate drag
            if (parameters.Drag < 0f)
            {
                errors.Add($"Drag must be non-negative. Current value: {parameters.Drag}");
            }
            
            // Validate integration parameters
            if (parameters.IntegrationParameters.ForceThreshold < 0f)
            {
                errors.Add($"Force threshold must be non-negative. Current value: {parameters.IntegrationParameters.ForceThreshold}");
            }
            
            if (parameters.IntegrationParameters.FixedTimeStep <= 0f)
            {
                errors.Add($"Fixed time step must be positive. Current value: {parameters.IntegrationParameters.FixedTimeStep}");
            }
            
            // Validate clamping parameters
            if (parameters.ClampingParameters.MinValue > parameters.ClampingParameters.MaxValue)
            {
                errors.Add($"Min value must be less than or equal to max value. Current values: Min={parameters.ClampingParameters.MinValue}, Max={parameters.ClampingParameters.MaxValue}");
            }
            
            return errors;
        }
        
        /// <summary>
        /// Validates the spring values.
        /// </summary>
        /// <param name="springValues">The spring values to validate.</param>
        /// <returns>A list of validation errors, or an empty list if the spring values are valid.</returns>
        public static List<string> ValidateSpringValues(SpringValues springValues)
        {
            var errors = new List<string>();
            
            // Validate force
            if (springValues.GetForce() < 0f)
            {
                errors.Add($"Force must be non-negative. Current value: {springValues.GetForce()}");
            }
            
            // Validate drag
            if (springValues.GetDrag() < 0f)
            {
                errors.Add($"Drag must be non-negative. Current value: {springValues.GetDrag()}");
            }
            
            // Validate min/max values
            if (springValues.GetMinValue() > springValues.GetMaxValue())
            {
                errors.Add($"Min value must be less than or equal to max value. Current values: Min={springValues.GetMinValue()}, Max={springValues.GetMaxValue()}");
            }
            
            return errors;
        }
        
        /// <summary>
        /// Checks if the spring values are valid.
        /// </summary>
        /// <param name="springValues">The spring values to check.</param>
        /// <returns>True if the spring values are valid, false otherwise.</returns>
        public static bool AreSpringValuesValid(SpringValues springValues)
        {
            return ValidateSpringValues(springValues).Count == 0;
        }
        
        /// <summary>
        /// Checks if the physics parameters are valid.
        /// </summary>
        /// <param name="parameters">The physics parameters to check.</param>
        /// <returns>True if the parameters are valid, false otherwise.</returns>
        public static bool AreParametersValid(PhysicsParameters parameters)
        {
            return ValidateParameters(parameters).Count == 0;
        }
    }
}

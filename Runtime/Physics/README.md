# USpring Physics Module

This module contains the physics implementation for the USpring system. It provides a flexible and extensible way to implement different physics models for spring-based animations.

## Overview

The physics module separates the physics implementation from the core spring classes, allowing for:

1. **Multiple Physics Models**: Different integration methods can be implemented and used interchangeably
2. **Better Configuration**: Physics parameters are encapsulated in dedicated classes
3. **Improved Error Handling**: Validation and error handling are centralized
4. **Extensibility**: New physics models can be added without modifying the core spring classes
5. **Performance**: Parameter caching reduces garbage collection pressure

## Key Components

### Physics Models

- **`IPhysicsModel`**: Interface for all physics models
- **`SemiImplicitModel`**: Implementation of semi-implicit Euler integration (default)
- **`AnalyticalModel`**: Implementation of analytical solution for extreme forces

### Parameter Classes

- **`PhysicsParameters`**: Main class for configuring spring physics
- **`IntegrationParameters`**: Parameters for numerical integration
- **`ClampingParameters`**: Parameters for value clamping

### Factory and Utilities

- **`PhysicsModelFactory`**: Factory for creating and selecting physics models
- **`PhysicsValidator`**: Validator for physics parameters and spring values
- **`PhysicsException`**: Custom exception for physics-related errors

## Integration with Core System

The physics module is used by the `SpringMath` class, which acts as a bridge between the core spring classes and the physics models:

1. `Spring` class maintains cached `PhysicsParameters` for each spring value
2. `SpringMath.UpdateSpring()` gets the appropriate physics model from `PhysicsModelFactory`
3. The selected physics model updates the spring values based on the parameters
4. Error handling catches and manages any numerical instabilities

## Creating a Custom Physics Model

To create a custom physics model:

```csharp
public class CustomPhysicsModel : IPhysicsModel
{
    public void UpdateSpringValues(float deltaTime, SpringValues springValues, PhysicsParameters parameters)
    {
        // Your custom physics implementation
    }

    public string GetModelName()
    {
        return "Custom Physics Model";
    }

    public string GetModelDescription()
    {
        return "A custom physics model for special cases.";
    }

    public bool IsSuitableForParameters(PhysicsParameters parameters)
    {
        // Logic to determine when this model should be used
        return parameters.Force > 1000f && parameters.Drag < 5f;
    }
}

// Register the model with the factory
PhysicsModelFactory.RegisterModel(new CustomPhysicsModel());
```

## Physics Model Selection

The `PhysicsModelFactory` automatically selects the most suitable model based on the physics parameters:

1. If `AlwaysUseAnalyticalSolution` is enabled, the `AnalyticalModel` is used
2. If the force exceeds the threshold (`ForceThreshold`), the `AnalyticalModel` is used
3. Otherwise, the `SemiImplicitModel` is used as the default

This automatic selection ensures stability for extreme parameter values while maintaining performance for typical use cases.

## Performance Optimizations

1. **Parameter Caching**: The `Spring` class maintains cached `PhysicsParameters` objects to avoid creating new ones on every update
2. **Selective Updates**: Only enabled spring values are updated
3. **Model Selection**: The most efficient model is used for each parameter set
4. **Error Recovery**: Springs automatically recover from numerical instabilities

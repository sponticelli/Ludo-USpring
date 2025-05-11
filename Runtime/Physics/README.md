# USpring Physics Module

This module contains the physics implementation for the USpring system. It provides a flexible and extensible way to implement different physics models for spring-based animations.

## Overview

The physics module separates the physics implementation from the core spring classes, allowing for:

1. **Multiple Physics Models**: Different integration methods can be implemented and used interchangeably.
2. **Better Configuration**: Physics parameters are encapsulated in dedicated classes.
3. **Improved Error Handling**: Validation and error handling are centralized.
4. **Extensibility**: New physics models can be added without modifying the core spring classes.

## Key Components

### Physics Models

- `IPhysicsModel`: Interface for all physics models.
- `SemiImplicitModel`: Implementation of semi-implicit Euler integration (default).
- `AnalyticalModel`: Implementation of analytical solution for extreme forces.

### Parameter Classes

- `PhysicsParameters`: Main class for configuring spring physics.
- `IntegrationParameters`: Parameters for numerical integration.
- `ClampingParameters`: Parameters for value clamping.

### Factory and Utilities

- `PhysicsModelFactory`: Factory for creating and selecting physics models.
- `PhysicsValidator`: Validator for physics parameters and spring values.
- `PhysicsException`: Custom exception for physics-related errors.

## Usage

The physics module is used internally by the `SpringMath` class. You don't need to interact with it directly unless you want to extend the system with custom physics models.

### Creating a Custom Physics Model

To create a custom physics model:

1. Implement the `IPhysicsModel` interface.
2. Register your model with the `PhysicsModelFactory`.
3. The factory will automatically select your model when appropriate.

Example:

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

// Register the model
PhysicsModelFactory.RegisterModel(new CustomPhysicsModel());
```

## Benefits of the Refactoring

1. **Separation of Concerns**: Physics calculations are separated from spring management.
2. **Improved Maintainability**: Each physics model is self-contained and can be tested independently.
3. **Better Error Handling**: Centralized validation and error handling.
4. **Extensibility**: New physics models can be added without modifying existing code.
5. **Configurability**: Physics parameters are encapsulated in dedicated classes.
6. **Performance Optimization**: Physics parameters are cached and only recreated when necessary, reducing garbage collection pressure.

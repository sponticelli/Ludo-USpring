# USpring Architecture Overview

## Introduction

USpring is a physically-based spring animation system for Unity that simulates damped harmonic oscillation. It provides a more natural and responsive alternative to traditional interpolation methods, allowing for organic, bouncy animations with configurable stiffness and damping.

This document provides a high-level overview of the USpring architecture, explaining the core components and how they interact.

## Architectural Layers

USpring follows a layered architecture with clear separation of concerns:

### 1. Core Interfaces

The foundation of USpring is a set of interfaces that define the contract for different spring types:

- **`ISpring`**: Base interface for all spring types with common functionality
- **`ISpringFloat`**, **`ISpringVector2`**, **`ISpringVector3`**, **`ISpringVector4`**: Type-specific interfaces
- **`ISpringColor`**: Interface for color animation
- **`ISpringRotation`**: Special interface for quaternion rotation with proper interpolation

These interfaces enable a fluent API design and provide a consistent contract for all spring implementations.

### 2. Core Spring Classes

The core spring classes implement the interfaces and provide the basic functionality:

- **`Spring`**: Abstract base class implementing `ISpring`
- **`SpringFloat`**, **`SpringVector2`**, **`SpringVector3`**, **`SpringVector4`**: Type-specific implementations
- **`SpringColor`**: Implementation for color animation
- **`SpringRotation`**: Special implementation for quaternion rotation
- **`SpringValues`**: Represents a single spring-driven value with its properties
- **`SpringFactory`**: Factory class for creating spring instances with a fluent API

### 3. Physics System

The physics system is responsible for the mathematical simulation of springs:

- **`SpringMath`**: Coordinates the physics simulation
- **`IPhysicsModel`**: Interface for physics models
- **`SemiImplicitModel`**: Default model using semi-implicit Euler integration
- **`AnalyticalModel`**: Alternative model for extreme force values
- **`PhysicsParameters`**, **`IntegrationParameters`**, **`ClampingParameters`**: Parameter classes
- **`PhysicsModelFactory`**: Factory for creating and selecting physics models

### 4. Unity Components

Unity components wrap the core springs for easy use in the Inspector:

- **`SpringComponent`**: Abstract base class for all spring components
- **`TransformSpringComponent`**: Animates Transform properties
- **`ColorSpringComponent`**: Animates colors
- **`CamFovOrSizeSpringComponent`**: Animates camera properties
- Many other specialized components for different use cases

### 5. Utility Systems

Supporting systems that enhance the functionality:

- **`SpringEvents`**: Event system for responding to spring state changes
- **`SpringSettingsData`** / **`SpringSettingsProvider`**: Global configuration
- **`SpringSkeleton`** / **`SpringBone`**: Skeletal animation system

## Data Flow

1. **Initialization**: Springs are created and initialized with initial values and parameters
2. **Target Setting**: Target values are set through code or by following target objects
3. **Physics Update**: Springs are updated using the appropriate physics model
4. **Value Processing**: Candidate values are processed and applied
5. **Event Triggering**: Events are triggered based on state changes
6. **Value Application**: Current values are applied to Unity objects

## Key Design Patterns

USpring utilizes several design patterns:

1. **Factory Pattern**: `SpringFactory` creates pre-configured springs
2. **Strategy Pattern**: Different physics models implement the same interface
3. **Decorator Pattern**: Springs can be configured with different behaviors
4. **Observer Pattern**: Spring events notify subscribers of state changes
5. **Dependency Injection**: `SpringSettingsProvider` provides global settings

## Extension Points

USpring is designed to be extensible through several mechanisms:

1. **Custom Spring Types**: Inherit from `Spring` to create new spring types
2. **Custom Components**: Inherit from `SpringComponent` to create new components
3. **Custom Physics Models**: Implement `IPhysicsModel` to create new integration methods
4. **Event Handling**: Subscribe to spring events for custom behaviors
5. **Parameter Configuration**: Extend parameter classes for custom configurations

## Performance Considerations

USpring is designed for performance with several optimizations:

1. **Model Selection**: Automatically selects the appropriate physics model
2. **Parameter Caching**: Avoids creating new parameter objects on every update
3. **Selective Updates**: Only updates enabled springs
4. **Analytical Solution**: Uses a more stable solution for extreme cases
5. **Error Handling**: Gracefully handles numerical instabilities

## Best Practices

1. **Choose the Right Spring Type**: Use the appropriate spring type for your data (e.g., `SpringFloat` for single values, `SpringVector3` for positions).
2. **Configure Force and Drag**: Adjust force and drag values to achieve the desired animation feel. Higher force means faster response, higher drag means less oscillation.
3. **Use Clamping When Needed**: Enable clamping for values with defined ranges (e.g., 0-1 for alpha).
4. **Consider Update Timing**: Use `Update` for most visual elements, `FixedUpdate` for physics-related springs, and `LateUpdate` for camera or follow behaviors.
5. **Use Events Sparingly**: Only enable events when you need to respond to spring state changes.

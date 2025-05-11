# USpring Architecture Overview

## Introduction

USpring is a physically-based spring animation system for Unity that simulates damped harmonic oscillation. It provides a more natural and responsive alternative to traditional interpolation methods, allowing for organic, bouncy animations with configurable stiffness and damping.

This document provides a high-level overview of the USpring architecture, explaining the core components and how they interact.

## Core Architecture

USpring follows a layered architecture with the following main components:

1. **Core Spring Classes**: The foundation of the system, implementing the spring physics model
2. **Spring Components**: Unity components that wrap the core springs for easy use in the Inspector
3. **Spring Math**: The mathematical implementation of the spring physics simulation
4. **Spring Events**: A system for responding to spring state changes
5. **Spring Settings**: Global configuration for the spring system

### Layer 1: Core Spring Classes

The core spring classes are the foundation of the USpring system. They implement the spring physics model and provide the basic functionality for spring-based animation.

#### Key Classes:

- **`Spring`**: The abstract base class for all spring types. Manages common spring properties like force (stiffness) and drag (damping).
- **`SpringFloat`**: A single-dimensional spring for animating float values.
- **`SpringVector2`**: A two-dimensional spring for animating Vector2 values.
- **`SpringVector3`**: A three-dimensional spring for animating Vector3 values.
- **`SpringVector4`**: A four-dimensional spring for animating Vector4 values.
- **`SpringColor`**: A spring for animating Color values (internally uses SpringVector4).
- **`SpringRotation`**: A special spring for animating Quaternion rotations with proper interpolation.
- **`SpringValues`**: Represents a single spring-driven value with its own properties (target, current value, velocity, etc.).

### Layer 2: Spring Components

Spring Components are Unity MonoBehaviour components that wrap the core springs, making them easy to use in the Unity Inspector and providing integration with Unity's GameObject system.

#### Key Components:

- **`SpringComponent`**: The abstract base class for all spring components.
- **`TransformSpringComponent`**: Animates Transform properties (position, rotation, scale).
- **`ColorSpringComponent`**: Animates colors of renderers or UI elements.
- **`CamFovOrSizeSpringComponent`**: Animates camera field of view or orthographic size.
- **`FloatSpringComponent`**: A general-purpose component for animating float values.
- **`SpringPulse`**: Creates pulsing animations using springs.

### Layer 3: Physics Models

The physics models implement different integration methods for updating spring values. Each model has its own strengths and weaknesses in terms of stability, accuracy, and performance.

#### Key Models:

- **`SemiImplicitModel`**: Implements semi-implicit Euler integration, providing a good balance of performance and stability for most cases.
- **`AnalyticalModel`**: Implements an analytical solution based on the damping ratio, providing better stability for extreme force values.

#### Key Features:

- **Model Selection**: The `PhysicsModelFactory` automatically selects the most suitable model based on the physics parameters.
- **Parameter Encapsulation**: Physics parameters are encapsulated in dedicated classes (`PhysicsParameters`, `IntegrationParameters`, `ClampingParameters`).
- **Validation**: The `PhysicsValidator` ensures that physics parameters are valid.
- **Error Handling**: Improved error handling with the `PhysicsException` class.

### Layer 4: Spring Math

The `SpringMath` class coordinates the physics simulation by selecting the appropriate physics model and applying clamping. It acts as a bridge between the core spring classes and the physics models.

### Layer 5: Spring Events

The `SpringEvents` class provides a system for responding to spring state changes through events.

#### Key Events:

- **`OnTargetReached`**: Triggered when the spring reaches its target value.
- **`OnCurrentValueChanged`**: Triggered when the spring's current value changes significantly.
- **`OnClampingApplied`**: Triggered when clamping is applied to the spring's value.

### Layer 6: Spring Settings

The `SpringSettingsData` and `SpringSettingsProvider` classes provide global configuration for the spring system.

#### Key Settings:

- **`MaxForceBeforeAnalyticalIntegration`**: The threshold for switching from semi-implicit to analytical integration.
- **`DoFixedUpdateRate`**: Whether to use a fixed update rate for spring simulation.
- **`SpringFixedTimeStep`**: The fixed time step to use for spring simulation.

## Data Flow

1. **Target Setting**: The target value is set either through code or by following a target object.
2. **Spring Update**: The spring is updated using `SpringMath.UpdateSpring()`, which calculates new candidate values based on the current state, target, force, and drag.
3. **Candidate Processing**: The candidate values are applied to the current values using `ProcessCandidateValue()`.
4. **Event Checking**: Events are checked and triggered using `CheckEvents()`.
5. **Value Application**: The current values are applied to the target object (e.g., Transform, Renderer).

## Extension Points

USpring is designed to be extensible. Here are the main extension points:

1. **Creating Custom Spring Types**: Inherit from `Spring` to create new spring types for custom data structures.
2. **Creating Custom Spring Components**: Inherit from `SpringComponent` to create new components that use springs in unique ways.
3. **Custom Event Handling**: Subscribe to spring events to create custom responses to spring state changes.
4. **Custom Physics Models**: Implement the `IPhysicsModel` interface to create custom integration methods.
5. **Custom Parameter Classes**: Extend the parameter classes to add custom configuration options.

## Best Practices

1. **Choose the Right Spring Type**: Use the appropriate spring type for your data (e.g., `SpringFloat` for single values, `SpringVector3` for positions).
2. **Configure Force and Drag**: Adjust force and drag values to achieve the desired animation feel. Higher force means faster response, higher drag means less oscillation.
3. **Use Clamping When Needed**: Enable clamping for values with defined ranges (e.g., 0-1 for alpha).
4. **Consider Update Timing**: Use `Update` for most visual elements, `FixedUpdate` for physics-related springs, and `LateUpdate` for camera or follow behaviors.
5. **Use Events Sparingly**: Only enable events when you need to respond to spring state changes.

## Conclusion

USpring provides a powerful and flexible system for creating natural, physically-based animations in Unity. By understanding its architecture and following best practices, you can create responsive and organic animations for a wide range of use cases.

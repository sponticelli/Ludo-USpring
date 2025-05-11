# USpring: Unity Spring Physics Library

## Introduction

USpring is a versatile library for adding physically-based spring animations to Unity projects. It replaces traditional interpolation methods with a damped harmonic oscillator model, enabling smooth, responsive, and natural-feeling motion for various properties.

**Key Features:**

* **Natural Motion:** Creates organic, bouncy animations superior to linear interpolation.
* **Responsive:** Springs dynamically react to changes in target values.
* **Component-Based:** Offers easy-to-use components for common Unity properties (Transform, UI, Color, etc.).
* **Scriptable:** Provides core spring classes (`SpringFloat`, `SpringVector3`, etc.) for direct use in scripts.
* **Skeletal System:** Includes `SpringSkeleton` and `SpringBone` for physics-based secondary animation on character rigs.
* **Customizable:** Fine-tune spring behavior with Force, Drag, Clamping, and Events.
* **Efficient:** Utilizes stable numerical integration suitable for real-time use.

## Quick Start

The most straightforward way to use USpring is via its pre-built components:

1. Select the GameObject to animate
2. Add a spring component (e.g., `TransformSpringComponent`, `ColorSpringComponent`)
3. Configure the spring parameters (Force, Drag) in the Inspector
4. Control the spring from your scripts:

```csharp
using UnityEngine;
using USpring.Components;

public class Example : MonoBehaviour
{
    [SerializeField] private TransformSpringComponent springComponent;

    void Update()
    {
        // Set the target position
        springComponent.SetTargetPosition(targetPosition);

        // Add velocity for a bouncy effect
        if (Input.GetKeyDown(KeyCode.Space))
        {
            springComponent.AddVelocityPosition(Vector3.up * 5f);
        }
    }
}
```

## Documentation

For detailed documentation, see the [Documentation](Documentation/README.md) folder:

- [Architecture Overview](Documentation/USpring-Architecture-Overview.md)
- [API Documentation](Documentation/USpring-API-Documentation.md)
- [Usage Guide](Documentation/USpring-Usage-Guide.md)
- [Patterns & Anti-Patterns](Documentation/USpring-Patterns-AntiPatterns.md)

## Core Concepts

USpring simulates a damped harmonic oscillator with two key parameters:

- **Force:** Controls the stiffness of the spring (higher = faster response)
- **Drag:** Controls the damping factor (higher = less oscillation)

Each spring maintains three key properties:

- **Target:** The desired end value the spring is moving towards
- **CurrentValue:** The current animated value at any given time
- **Velocity:** The current rate of change of the CurrentValue

## Advanced Usage

For maximum control, you can use the core spring classes directly with the fluent API:

```csharp
using USpring.Core;
using USpring.Core.Interfaces;

// Create a spring using the factory
ISpringVector3 positionSpring = SpringFactory.CreateVector3WithForceAndDrag(150f, 10f)
    .SetTarget(transform.position)
    .SetCurrentValue(transform.position);

// Update the spring
void Update()
{
    positionSpring.Update(Time.deltaTime);
    transform.position = positionSpring.GetCurrentValue();
}
```

## Physics Models

USpring uses two main physics models:

1. **Semi-Implicit Euler** (default): Efficient and stable for most cases
2. **Analytical Solution**: Used for extreme force values to ensure stability

The system automatically selects the appropriate model based on the spring parameters.
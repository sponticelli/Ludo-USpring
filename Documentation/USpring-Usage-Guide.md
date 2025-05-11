# USpring Usage Guide

## Table of Contents

1. [Introduction](#introduction)
2. [Getting Started with Spring Components](#getting-started-with-spring-components)
   - [Adding a Spring Component](#adding-a-spring-component)
   - [Configuring Spring Properties](#configuring-spring-properties)
   - [Controlling Springs from Script](#controlling-springs-from-script)
3. [Common Use Cases](#common-use-cases)
   - [Animating UI Elements](#animating-ui-elements)
   - [Camera Effects](#camera-effects)
   - [Object Movement](#object-movement)
   - [Color Transitions](#color-transitions)
4. [Advanced Usage: Core Springs](#advanced-usage-core-springs)
   - [Creating and Initializing Springs](#creating-and-initializing-springs)
   - [Updating Springs](#updating-springs)
   - [Handling Spring Events](#handling-spring-events)
5. [Performance Considerations](#performance-considerations)
6. [Troubleshooting](#troubleshooting)

## Introduction

USpring is a physically-based spring animation system for Unity that simulates damped harmonic oscillation. It provides a more natural and responsive alternative to traditional interpolation methods, allowing for organic, bouncy animations with configurable stiffness and damping.

This guide will walk you through the process of using USpring in your Unity projects, from basic usage with pre-built components to advanced usage with core spring classes.

## Getting Started with Spring Components

The easiest way to use USpring is through its pre-built components, which provide a user-friendly interface in the Unity Inspector.

### Adding a Spring Component

1. Select the GameObject you want to animate.
2. Click "Add Component" in the Inspector.
3. Search for "Spring" to see all available spring components.
4. Choose the appropriate component for your needs:
   - `TransformSpringComponent` for animating position, rotation, or scale
   - `ColorSpringComponent` for animating colors
   - `CamFovOrSizeSpringComponent` for animating camera field of view or orthographic size
   - `FloatSpringComponent` for animating arbitrary float values
   - `SpringPulse` for creating pulsing animations

### Configuring Spring Properties

Once you've added a spring component, you can configure its properties in the Inspector:

#### General Properties

- **Does Auto Initialize**: When enabled, the component will automatically initialize on Awake.
- **Use Scaled Time**: When enabled, uses Time.deltaTime; when disabled, uses Time.unscaledDeltaTime.
- **Always Use Analytical Solution**: When enabled, always uses the analytical solution for spring integration (more stable but less efficient).
- **Has Custom Initial Values**: When enabled, uses custom initial values instead of the current values.
- **Has Custom Target**: When enabled, uses custom target values instead of automatically determining them.

#### Spring-Specific Properties

Each spring component has its own specific properties. For example, `TransformSpringComponent` has:

- **Space Type**: Determines whether to use world space or local space for position and rotation.
- **Follower Transform**: The Transform that will be animated by the spring.
- **Use Transform As Target**: When enabled, uses targetTransform as the target for position, rotation, and scale.
- **Target Transform**: The Transform to use as the target when useTransformAsTarget is enabled.

#### Spring Settings

Each spring value (e.g., position, rotation, scale) has its own settings:

- **Spring Enabled**: When enabled, the spring simulation is active for this value.
- **Clamping Enabled**: When enabled, value clamping is active for this value.
- **Common Force & Drag**: When enabled, uses the same force and drag values for all axes.
- **Common Force**: The force (stiffness) value used when Common Force & Drag is enabled.
- **Common Drag**: The drag (damping) value used when Common Force & Drag is enabled.
- **Events Enabled**: When enabled, spring events are active for this value.

### Controlling Springs from Script

You can control spring components from script using their public methods:

```csharp
using UnityEngine;
using USpring.Components;

public class SpringController : MonoBehaviour
{
    [SerializeField] private TransformSpringComponent transformSpring;
    [SerializeField] private Transform target;

    void Start()
    {
        // Make sure the spring is initialized
        if (!transformSpring.IsInitialized)
        {
            transformSpring.Initialize();
        }
    }

    void Update()
    {
        // Set the target position
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transformSpring.SetTargetPosition(target.position);
        }

        // Add velocity for a bouncy effect
        if (Input.GetKeyDown(KeyCode.B))
        {
            transformSpring.AddVelocityPosition(Vector3.up * 5f);
        }

        // Immediately reach the target
        if (Input.GetKeyDown(KeyCode.R))
        {
            transformSpring.ReachEquilibriumPosition();
        }
    }
}
```

## Common Use Cases

### Animating UI Elements

USpring can be used to create responsive and natural-feeling UI animations:

```csharp
using UnityEngine;
using UnityEngine.UI;
using USpring.Components;

public class ResponsiveButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TransformSpringComponent springComponent;
    [SerializeField] private Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] private Vector3 pressedScale = new Vector3(0.9f, 0.9f, 0.9f);
    private Vector3 defaultScale;

    void Start()
    {
        defaultScale = transform.localScale;

        button.onClick.AddListener(OnClick);

        // Set up event triggers for hover and press
        var eventTrigger = button.gameObject.AddComponent<EventTrigger>();

        var pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        pointerEnter.callback.AddListener((data) => OnPointerEnter());
        eventTrigger.triggers.Add(pointerEnter);

        var pointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        pointerExit.callback.AddListener((data) => OnPointerExit());
        eventTrigger.triggers.Add(pointerExit);

        var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        pointerDown.callback.AddListener((data) => OnPointerDown());
        eventTrigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        pointerUp.callback.AddListener((data) => OnPointerUp());
        eventTrigger.triggers.Add(pointerUp);
    }

    void OnPointerEnter()
    {
        springComponent.SetTargetScale(hoverScale);
    }

    void OnPointerExit()
    {
        springComponent.SetTargetScale(defaultScale);
    }

    void OnPointerDown()
    {
        springComponent.SetTargetScale(pressedScale);
    }

    void OnPointerUp()
    {
        springComponent.SetTargetScale(hoverScale);
    }

    void OnClick()
    {
        // Add a little bounce effect on click
        springComponent.AddVelocityScale(Vector3.one * 0.5f);
    }
}
```

### Camera Effects

USpring can be used to create smooth camera movements and effects:

#### Basic Camera Follow

```csharp
using UnityEngine;
using USpring.Components;

public class CameraController : MonoBehaviour
{
    [SerializeField] private TransformSpringComponent cameraSpring;
    [SerializeField] private Transform target;
    [SerializeField] private float followDistance = 5f;
    [SerializeField] private float followHeight = 2f;
    [SerializeField] private float shakeStrength = 0.5f;

    void Update()
    {
        // Calculate the desired camera position
        Vector3 targetPosition = target.position - target.forward * followDistance + Vector3.up * followHeight;

        // Set the target position for the spring
        cameraSpring.SetTargetPosition(targetPosition);

        // Look at the target
        transform.LookAt(target);

        // Shake the camera when the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShakeCamera();
        }
    }

    void ShakeCamera()
    {
        // Add random velocity to create a shake effect
        Vector3 shakeVelocity = new Vector3(
            Random.Range(-shakeStrength, shakeStrength),
            Random.Range(-shakeStrength, shakeStrength),
            Random.Range(-shakeStrength, shakeStrength)
        );

        cameraSpring.AddVelocityPosition(shakeVelocity);
    }
}
```

#### Advanced Camera Shake Controller

For more sophisticated camera shake effects, you can create a dedicated controller that responds to game events:

```csharp
using UnityEngine;
using USpring.Components;

public class CameraShakeController : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float baseShakeStrength = 0.3f;
    [SerializeField] private bool shakeOnlyX = false;
    [SerializeField] private float specialEventMultiplier = 1.5f;
    [SerializeField] private float minorEventShakeStrength = 0.1f;

    [Header("Spring Configuration")]
    [SerializeField] private float springForce = 30f;
    [SerializeField] private float springDrag = 5f;

    [Header("References")]
    [SerializeField] private TransformSpringComponent cameraSpring;

    private void Start()
    {
        // If no spring component is assigned, try to get or add one
        if (cameraSpring == null)
        {
            cameraSpring = GetComponent<TransformSpringComponent>();
            if (cameraSpring == null)
            {
                cameraSpring = gameObject.AddComponent<TransformSpringComponent>();

                // Configure the spring for camera shake
                cameraSpring.SetCommonForcePosition(springForce);
                cameraSpring.SetCommonDragPosition(springDrag);
            }
        }

        // Subscribe to game events that should trigger camera shake
        // Example: GameEventSystem.OnEnemyDestroyed += OnEnemyDestroyed;
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        // Example: GameEventSystem.OnEnemyDestroyed -= OnEnemyDestroyed;
    }

    // Example event handler
    private void OnEnemyDestroyed(EnemyDestroyedEventArgs args)
    {
        // Determine shake strength based on enemy type
        float shakeStrength = baseShakeStrength;

        // Apply a stronger shake for special enemies
        if (args.IsSpecialEnemy)
        {
            shakeStrength *= specialEventMultiplier;
        }

        ApplyShake(shakeStrength);
    }

    // Example event handler for minor events
    private void OnMinorEvent()
    {
        // Apply a very small shake for minor events
        ApplyShake(minorEventShakeStrength);
    }

    public void ApplyShake(float strength)
    {
        // Apply a random velocity to create the shake effect
        Vector3 shakeVelocity;

        if (shakeOnlyX)
        {
            // Shake only horizontally
            shakeVelocity = new Vector3(Random.Range(-strength, strength), 0, 0);
        }
        else
        {
            // Shake in both X and Y directions
            shakeVelocity = new Vector3(
                Random.Range(-strength, strength),
                Random.Range(-strength, strength),
                0);
        }

        // Apply the velocity to the spring
        cameraSpring.AddVelocityPosition(shakeVelocity);
    }
}
```

### Object Movement

USpring can be used to create natural-feeling object movements:

```csharp
using UnityEngine;
using USpring.Components;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private TransformSpringComponent transformSpring;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Update()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate move direction
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Move the character
        if (moveDirection.magnitude > 0.1f)
        {
            // Calculate target position
            Vector3 targetPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

            // Set the target position for the spring
            transformSpring.SetTargetPosition(targetPosition);

            // Rotate to face the move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transformSpring.SetTargetRotation(targetRotation);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Add upward velocity for jumping
            transformSpring.AddVelocityPosition(Vector3.up * jumpForce);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the character is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
```

### Color Transitions

USpring can be used to create smooth color transitions:

```csharp
using UnityEngine;
using UnityEngine.UI;
using USpring.Components;

public class ColorTransition : MonoBehaviour
{
    [SerializeField] private ColorSpringComponent colorSpring;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private Color pressedColor = Color.red;
    [SerializeField] private Button button;

    void Start()
    {
        // Set up button events
        button.onClick.AddListener(OnClick);

        var eventTrigger = button.gameObject.AddComponent<EventTrigger>();

        var pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        pointerEnter.callback.AddListener((data) => OnPointerEnter());
        eventTrigger.triggers.Add(pointerEnter);

        var pointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        pointerExit.callback.AddListener((data) => OnPointerExit());
        eventTrigger.triggers.Add(pointerExit);

        var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        pointerDown.callback.AddListener((data) => OnPointerDown());
        eventTrigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        pointerUp.callback.AddListener((data) => OnPointerUp());
        eventTrigger.triggers.Add(pointerUp);
    }

    void OnPointerEnter()
    {
        colorSpring.SetTarget(highlightColor);
    }

    void OnPointerExit()
    {
        colorSpring.SetTarget(normalColor);
    }

    void OnPointerDown()
    {
        colorSpring.SetTarget(pressedColor);
    }

    void OnPointerUp()
    {
        colorSpring.SetTarget(highlightColor);
    }

    void OnClick()
    {
        // Flash to white and back to the current color
        Color currentColor = colorSpring.GetCurrentValue();
        colorSpring.SetTarget(Color.white);

        // Use a coroutine to return to the previous color after a delay
        StartCoroutine(ReturnToColorAfterDelay(currentColor, 0.1f));
    }

    System.Collections.IEnumerator ReturnToColorAfterDelay(Color color, float delay)
    {
        yield return new WaitForSeconds(delay);
        colorSpring.SetTarget(color);
    }
}
```

## Advanced Usage: Core Springs

For more control or unique use cases, you can use the core spring classes directly with the new fluent API.

### Creating and Initializing Springs with the Factory

The easiest way to create springs is to use the `SpringFactory` class, which provides a fluent API for creating and configuring springs:

```csharp
using UnityEngine;
using USpring.Core;
using USpring.Core.Interfaces;

public class CustomSpringExample : MonoBehaviour
{
    // Spring instances
    private ISpringFloat heightSpring;
    private ISpringVector3 positionSpring;
    private ISpringRotation rotationSpring;

    void Awake()
    {
        // Create a float spring with default settings
        heightSpring = SpringFactory.CreateFloat()
            .SetTarget(transform.position.y)
            .SetCurrentValue(transform.position.y);

        // Create a vector3 spring with custom force and drag
        positionSpring = SpringFactory.CreateVector3WithForceAndDrag(150f, 10f)
            .SetTarget(transform.position)
            .SetCurrentValue(transform.position);

        // Create a rotation spring with target and current values
        rotationSpring = SpringFactory.CreateRotation(transform.rotation, transform.rotation)
            .SetCommonForce(150f)
            .SetCommonDrag(10f);

        // Use preset springs for common use cases
        ISpringFloat uiSpring = SpringFactory.CreateUIPreset();
        ISpringVector3 cameraSpring = SpringFactory.CreateCameraPreset();
        ISpringFloat bouncySpring = SpringFactory.CreateBouncyPreset();
    }
}
```

### Creating and Initializing Springs Manually

You can also create and configure springs manually using the fluent API:

```csharp
using UnityEngine;
using USpring.Core;
using USpring.Core.Interfaces;

public class CustomSpringExample : MonoBehaviour
{
    // Spring instances
    private ISpringFloat heightSpring;
    private ISpringVector3 positionSpring;
    private ISpringRotation rotationSpring;

    // Configuration
    [SerializeField] private float springForce = 150f;
    [SerializeField] private float springDrag = 10f;

    void Awake()
    {
        // Create and initialize the height spring with fluent API
        heightSpring = new SpringFloat()
            .SetCommonForce(springForce)
            .SetCommonDrag(springDrag)
            .SetCurrentValue(transform.position.y)
            .SetTarget(transform.position.y)
            .Initialize();

        // Create and initialize the position spring with fluent API
        positionSpring = new SpringVector3()
            .SetCommonForceAndDragValues(springForce, springDrag)
            .SetCurrentValue(transform.position)
            .SetTarget(transform.position)
            .Initialize();

        // Create and initialize the rotation spring with fluent API
        rotationSpring = new SpringRotation()
            .SetCommonForce(springForce)
            .SetCommonDrag(springDrag)
            .SetCurrentValue(transform.rotation)
            .SetTarget(transform.rotation)
            .Initialize();
    }
}
```

### Updating Springs

With the new fluent API, updating springs is much simpler:

```csharp
using UnityEngine;
using USpring.Core;
using USpring.Core.Interfaces;

public class CustomSpringExample : MonoBehaviour
{
    // ... (previous code)

    void Update()
    {
        // Update the springs with a single call
        float deltaTime = Time.deltaTime;

        // Update the height spring
        heightSpring.Update(deltaTime);

        // Update the position spring
        positionSpring.Update(deltaTime);

        // Update the rotation spring
        rotationSpring.Update(deltaTime);

        // Apply the spring values to the transform
        Vector3 position = transform.position;
        position.y = heightSpring.GetCurrentValue();
        transform.position = position;

        // Or use the position spring directly
        // transform.position = positionSpring.GetCurrentValue();

        // Apply the rotation
        transform.rotation = rotationSpring.GetCurrentValue();
    }
}
```

You can also use a custom force threshold for the update:

```csharp
// Update with a custom force threshold
heightSpring.Update(deltaTime, 5000f);
```

Or chain multiple operations together:

```csharp
// Chain operations together
heightSpring
    .SetTarget(newTarget)
    .SetCommonForce(newForce)
    .Update(deltaTime);
```

### Handling Spring Events

With the new fluent API, handling events is more straightforward:

```csharp
using UnityEngine;
using USpring.Core;
using USpring.Core.Interfaces;

public class CustomSpringExample : MonoBehaviour
{
    // ... (previous code)

    void Awake()
    {
        // ... (previous initialization code)

        // Enable events with fluent API
        heightSpring.SetEventsEnabled(true);

        // Subscribe to events
        heightSpring.GetEvents().OnTargetReached += OnHeightTargetReached;
        heightSpring.GetEvents().OnCurrentValueChanged += OnHeightValueChanged;
        heightSpring.GetEvents().OnClampingApplied += OnHeightClampingApplied;
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (heightSpring != null)
        {
            var events = heightSpring.GetEvents();
            if (events != null)
            {
                events.OnTargetReached -= OnHeightTargetReached;
                events.OnCurrentValueChanged -= OnHeightValueChanged;
                events.OnClampingApplied -= OnHeightClampingApplied;
            }
        }
    }

    void OnHeightTargetReached()
    {
        Debug.Log("Height target reached!");
    }

    void OnHeightValueChanged()
    {
        // This will be called frequently, so be careful with performance-intensive operations
        Debug.Log("Height value changed: " + heightSpring.GetCurrentValue());
    }

    void OnHeightClampingApplied()
    {
        Debug.Log("Height clamping applied!");
    }
}
```

## Performance Considerations

1. **Use Common Force & Drag**: When possible, use common force and drag values for all axes to reduce calculations.
2. **Disable Unused Springs**: Set `springEnabled = false` for any spring values you're not using.
3. **Disable Events When Not Needed**: Only enable events when you actually need them.
4. **Choose the Right Update Loop**: Use `Update` for most visual elements, `FixedUpdate` for physics-related springs, and `LateUpdate` for camera or follow behaviors.
5. **Consider Using Fixed Time Step**: For critical animations, consider using a fixed time step by enabling `doFixedUpdateRate` in the SpringSettingsData.
6. **Avoid Extreme Force Values**: Very high force values can cause instability and may require the more expensive analytical solution.

## Troubleshooting

### Spring Not Moving

1. Check if `springEnabled` is true.
2. Verify that the target value is different from the current value.
3. Make sure the force value is high enough to cause visible movement.
4. Check if the spring is clamped and unable to move further.

### Spring Oscillating Too Much

1. Increase the drag value to add more damping.
2. Decrease the force value to reduce the spring's stiffness.
3. Make sure you're not adding velocity every frame.

### Spring Moving Too Slowly

1. Increase the force value to make the spring stiffer.
2. Decrease the drag value to reduce damping.
3. Consider using `ReachEquilibrium()` if you need an immediate transition.

### Spring Values Becoming NaN or Infinity

1. Check for division by zero or other mathematical errors.
2. Avoid extremely high force values or extremely low drag values.
3. Make sure your time step (deltaTime) is reasonable.
4. Consider using the analytical solution by setting `alwaysUseAnalyticalSolution = true`.

### Component Not Initializing

1. Check if `doesAutoInitialize` is true.
2. Verify that the component passes the `IsValidSpringComponent()` check.
3. Try manually calling `Initialize()` in your script.

### Events Not Firing

1. Make sure `eventsEnabled` is true.
2. Verify that you've subscribed to the events correctly.
3. Check if the conditions for the event are being met (e.g., the spring is actually reaching its target).

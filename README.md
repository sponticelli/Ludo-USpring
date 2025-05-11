# USpring: Unity Spring Physics Library

## 1. Introduction

USpring is a versatile library for adding physically-based spring animations to Unity projects. It replaces traditional interpolation methods with a damped harmonic oscillator model, enabling smooth, responsive, and natural-feeling motion for various properties.

**Key Features:**

* **Natural Motion:** Creates organic, bouncy animations superior to linear interpolation.
* **Responsive:** Springs dynamically react to changes in target values.
* **Component-Based:** Offers easy-to-use components for common Unity properties (Transform, UI, Color, etc.).
* **Scriptable:** Provides core spring classes (`SpringFloat`, `SpringVector3`, etc.) for direct use in scripts.
* **Skeletal System:** Includes `SpringSkeleton` and `SpringBone` for physics-based secondary animation on character rigs.
* **Customizable:** Fine-tune spring behavior with Force, Drag, Clamping, and Events.
* **Efficient:** Utilizes stable numerical integration suitable for real-time use.

## 2. Core Concepts

### 2.1. Spring Physics Model

USpring simulates a damped harmonic oscillator. This means an object animated by a spring will accelerate towards its `Target` value, potentially overshoot, oscillate, and eventually settle due to damping.

### 2.2. Key Parameters

Every spring axis/channel can be configured (unless using `Common Force & Drag`):

* **Force (`force`):** Controls the stiffness or strength of the spring. Higher values result in quicker acceleration towards the target and potentially more oscillation.
* **Drag (`drag`):** Represents the damping factor. Higher values cause the spring to lose energy faster, resulting in less oscillation and a quicker settling time.

### 2.3. Core Spring Types (Runtime/Core)

USpring provides several core classes for different data types, all inheriting from `Spring`:

* `SpringFloat`: Animates single `float` values.
* `SpringVector2`: Animates `Vector2` values.
* `SpringVector3`: Animates `Vector3` values.
* `SpringVector4`: Animates `Vector4` values.
* `SpringColor`: Animates `Color` values (internally uses a `SpringVector4`).
* `SpringRotation`: Animates `Quaternion` rotations, ensuring smooth interpolation.

### 2.4. State Properties

Each spring instance maintains its state:

* `Target`: The desired end value the spring is moving towards.
* `CurrentValue`: The current animated value of the spring at any given time.
* `Velocity`: The current rate of change of the `CurrentValue`.

### 2.5. Global Settings (`SpringSettingsData` / `SpringSettingsProvider`)

Global settings like the integration method threshold (`maxForceBeforeAnalyticalIntegration`) and fixed timestep usage (`doFixedUpdateRate`, `springFixedTimeStep`) can be managed via a `SpringSettingsData` ScriptableObject and accessed through the `SpringSettingsProvider` component using dependency injection (`ISpringSettingsProvider`).

## 3. Getting Started: Using Spring Components

The most straightforward way to use USpring is via its pre-built `SpringComponent` derivatives located in `Runtime/Components`.

### 3.1. Adding a Spring Component

1.  Select the GameObject to animate.
2.  Click "Add Component" in the Inspector.
3.  Search for "Spring" or navigate the `Ludo/USpring/Components` menu.
4.  Choose the appropriate component (e.g., `TransformSpringComponent`, `ColorSpringComponent`).

### 3.2. Configuration

Components expose settings in the Inspector:

* **General Properties:** Enable/disable auto-initialization, select time scale (Scaled/Unscaled).
* **Targeting:** Define what the component affects (e.g., `followerTransform`, `autoUpdatedLight`). Some components can follow a `targetTransform`.
* **Initial Values:** Option to override default start values (`hasCustomInitialValues`) and target values (`hasCustomTarget`).
* **Spring Settings:** Configure individual springs (e.g., Position, Rotation, Scale for `TransformSpringComponent`). Includes enabling/disabling springs, setting Force/Drag (common or per-axis), enabling clamping, and configuring events.

### 3.3. Controlling from Script

Components provide public methods to interact with their underlying springs.

**Example:** Making a `TransformSpringComponent`'s position follow a target `Vector3`:

```csharp
using UnityEngine;
using USpring.Components; // Make sure to include this namespace

public class FollowTarget : MonoBehaviour
{
    public TransformSpringComponent springComponent;
    public Vector3 targetPosition;

    void Update()
    {
        // Update the target position (e.g., based on input or another object)
        // targetPosition = ...; 

        if (springComponent != null)
        {
            // Set the position spring's target
            springComponent.SetTargetPosition(targetPosition);

            // Optionally, add instantaneous velocity
            if (Input.GetKeyDown(KeyCode.Space))
            {
                springComponent.AddVelocityPosition(Vector3.up * 5f);
            }
        }
    }
}
```

## 4. Available Components (Runtime/Components)

* `AnchoredPositionSpringComponent`: Animates `RectTransform.anchoredPosition`.
* `AudioSourceSpringComponent`: Animates `AudioSource` Volume and Pitch.
* `CamFovOrSizeSpringComponent`: Animates Camera Field of View (Perspective) or Orthographic Size.
* `ColorSpringComponent`: Animates the color of a `Renderer` material or UI `Graphic`.
* `FloatSpringComponent`: A general-purpose component animating a single `float`. Useful as a driver for other systems.
* `LightIntensitySpringComponent`: Animates `Light.intensity`.
* `RigidbodySpringComponent`: Drives a `Rigidbody`'s position and rotation towards targets. (Uses `FixedUpdate`).
* `RotationSpringComponent`: Animates a single `Quaternion` rotation value.
* `SetStateTransformSpringComponent`: Utility to set initial/target states of a `TransformSpringComponent` on `Start`, `OnEnable`, or `Update`.
* `ShaderFloatSpringComponent`: Animates a specific float property in a `Material`.
* `TransformSpringComponent`: Animates `Transform` Position, Rotation, and Scale (World or Local space).
* `UiSliderSpringComponent`: Animates the `fillAmount` of a UI `Image` (useful for health bars, progress bars).
* `UiToolkitSpringComponent`: Integrates with UI Toolkit by animating a helper transform linked to a `VisualElement`.
* `Vector2SpringComponent`: General-purpose `Vector2` spring.
* `Vector3SpringComponent`: General-purpose `Vector3` spring.
* `Vector4SpringComponent`: General-purpose `Vector4` spring.

## 5. Advanced Usage: Scripting with Core Springs

For maximum control or unique use cases, instantiate and manage the core spring classes (`SpringFloat`, `SpringVector3`, etc.) directly.

### 5.1. Creating and Updating Springs

```csharp
using UnityEngine;
using USpring.Core; // Required namespace

public class ManualSpringExample : MonoBehaviour
{
    // Configuration (can be public or private)
    public float springForce = 150f;
    public float springDrag = 10f;

    // Core Spring Instance
    private SpringFloat mySpring;

    // State
    private float targetValue = 0f;
    private float currentValue = 0f;

    void Awake()
    {
        // Get initial value if needed
        // float initialValue = GetComponent<SomeComponent>().someValue; 
        float initialValue = 0f; 

        // Create the spring
        mySpring = new SpringFloat();
        mySpring.SetCurrentValue(initialValue); // Set starting point
        mySpring.SetTarget(initialValue);      // Set initial target
        mySpring.SetCommonForce(springForce); // Use common settings
        mySpring.SetCommonDrag(springDrag);
        // Or set per-axis: mySpring.SetForce(springForce); mySpring.SetDrag(springDrag);
        mySpring.Initialize(); // Initializes events etc.
    }

    void Update()
    {
        // 1. Update Target Value (Based on game logic)
        if (Input.GetKeyDown(KeyCode.A)) targetValue = 10f;
        if (Input.GetKeyDown(KeyCode.S)) targetValue = -5f;
        mySpring.SetTarget(targetValue);

        // 2. Update Spring Parameters (Optional, if they change)
        // mySpring.SetCommonForce(newForce); 
        // mySpring.SetCommonDrag(newDrag);

        // 3. Update the Spring Simulation
        // Choose appropriate deltaTime (Time.deltaTime or Time.unscaledDeltaTime)
        SpringMath.UpdateSpring(Time.deltaTime, mySpring); 
        mySpring.ProcessCandidateValue(); // Applies calculated value
        mySpring.CheckEvents();           // Fires events if enabled

        // 4. Get the Current Animated Value
        currentValue = mySpring.GetCurrentValue();

        // 5. Apply the Value
        // Example: Apply to light intensity
        //GetComponent<Light>().intensity = currentValue;
        Debug.Log($"Current Spring Value: {currentValue}");
    }
}

```

### 5.2. Key Methods (Core Spring Types)

* `Initialize()`: Sets up the spring's internal state and events. Call after creation.
* `SetTarget(T value)`: Sets the desired end value.
* `SetCurrentValue(T value)`: Immediately sets the current value, bypassing the spring simulation.
* `AddVelocity(T velocity)`: Adds an instantaneous velocity impulse.
* `ReachEquilibrium()`: Immediately sets the current value to the target and stops motion.
* `SetCommonForce(float)`, `SetCommonDrag(float)`: Set parameters for all axes.
* `SetForce(T force)`, `SetDrag(T drag)`: Set per-axis parameters (requires `commonForceAndDrag = false`).
* `GetCurrentValue()`: Gets the current animated value.
* `GetVelocity()`: Gets the current velocity.
* `ProcessCandidateValue()`: Applies the result of the internal simulation step (called after `SpringMath.UpdateSpring`).
* `CheckEvents()`: Checks and invokes events like `OnTargetReached`.

## 6. Skeletal Animation (Runtime/Skeletal)

USpring includes a system for applying secondary spring physics to bone hierarchies.

* **`SpringSkeleton`**: The main component placed on the root of the animated hierarchy. Manages overall settings (inertia, force/drag curves based on depth) and updates the bones.
* **`SpringBone`**: Added automatically (via the `SpringSkeleton` editor) to each bone transform. Contains a `RotationSpringComponent` and handles inertia calculations based on parent movement and propagates updates.
* **`SpringBoneAttachable`**: A variant of `SpringBone` for objects that should follow a bone's position and rotation (e.g., accessories).

Configure the `SpringSkeleton` component via the Inspector to set up spring behavior across the character's bones.

## 7. Technical Details (`SpringMath`)

The core simulation happens in `SpringMath.UpdateSpring`. It primarily uses a **semi-implicit Euler integration** method, which is generally stable and efficient for real-time simulation. For scenarios with extremely high forces (configurable via `SpringSettingsData`), it can optionally switch to an **analytical solution** based on the damping ratio (over-damped, under-damped, critically damped) to guarantee stability.

## 8. Best Practices

* **Use Components First:** Start with the provided `SpringComponent`s for simplicity.
* **Script for Control:** Use core spring classes when components aren't sufficient or for complex interactions.
* **Choose Update Loop:** Use `FixedUpdate` for `Rigidbody` springs, `LateUpdate` for camera/tracking, `Update` for most visual elements.
* **Tune Parameters:** Experiment with `Force` and `Drag` to achieve the desired feel. Start low and increase gradually.
* **Consider Clamping:** Enable clamping (`clampingEnabled` and specific clamp flags) for values with defined ranges (e.g., 0-1 for alpha).
* **Use `RotationSpring`:** Always prefer `RotationSpring` or components using it (`TransformSpringComponent`, `RotationSpringComponent`) for smooth rotations.
* **Add Velocity:** Use `AddVelocity(...)` methods for impacts, reactions, or sudden bursts of motion.
* **Initialization:** Ensure `SpringComponent.Initialize()` or the core `Spring.Initialize()` is called before use, especially if `doesAutoInitialize` is off or springs are created manually.
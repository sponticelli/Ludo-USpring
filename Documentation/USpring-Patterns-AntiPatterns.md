# USpring Patterns and Anti-Patterns

This document outlines recommended patterns and anti-patterns when using the USpring system. Following these guidelines will help you create efficient, maintainable, and effective spring-based animations.

## Table of Contents

1. [Recommended Patterns](#recommended-patterns)
   - [Component Usage Patterns](#component-usage-patterns)
   - [Core Spring Usage Patterns](#core-spring-usage-patterns)
   - [Performance Optimization Patterns](#performance-optimization-patterns)
   - [Architecture Patterns](#architecture-patterns)
2. [Anti-Patterns to Avoid](#anti-patterns-to-avoid)
   - [Component Usage Anti-Patterns](#component-usage-anti-patterns)
   - [Core Spring Usage Anti-Patterns](#core-spring-usage-anti-patterns)
   - [Performance Anti-Patterns](#performance-anti-patterns)
   - [Architecture Anti-Patterns](#architecture-anti-patterns)

## Recommended Patterns

### Component Usage Patterns

#### Use the Right Component for the Job

**Pattern**: Choose the most specific spring component for your needs.

**Example**:
```csharp
// Good: Using the specific component for the job
[SerializeField] private TransformSpringComponent transformSpring;
[SerializeField] private ColorSpringComponent colorSpring;
[SerializeField] private CamFovOrSizeSpringComponent cameraSpring;
```

**Explanation**: Each spring component is optimized for its specific use case. Using the right component ensures better performance and easier configuration.

#### Initialize Components in Awake or Start

**Pattern**: Ensure spring components are initialized before use.

**Example**:
```csharp
void Awake()
{
    // Good: Checking if the component is initialized and initializing if needed
    if (!springComponent.IsInitialized)
    {
        springComponent.Initialize();
    }
}
```

**Explanation**: Spring components need to be initialized before they can be used. The `doesAutoInitialize` property handles this automatically, but it's good practice to check and initialize manually if needed.

#### Use Events for Responsive Behavior

**Pattern**: Use spring events to trigger actions when springs reach specific states.

**Example**:
```csharp
void Start()
{
    // Good: Using events for responsive behavior
    springComponent.PositionEvents.OnTargetReached += OnPositionTargetReached;
}

void OnPositionTargetReached()
{
    // Trigger an action when the position target is reached
    PlaySound();
    SpawnParticles();
}
```

**Explanation**: Spring events allow you to create responsive behaviors that react to spring state changes, making your animations more interactive and dynamic.

### Core Spring Usage Patterns

#### Separate Spring Creation and Update

**Pattern**: Separate spring creation and initialization from spring updates.

**Example**:
```csharp
// Good: Separating spring creation and initialization
void Awake()
{
    mySpring = new SpringFloat();
    mySpring.SetCommonForce(springForce);
    mySpring.SetCommonDrag(springDrag);
    mySpring.SetCurrentValue(initialValue);
    mySpring.SetTarget(initialValue);
    mySpring.Initialize();
}

// Good: Updating the spring in the appropriate update method
void Update()
{
    SpringMath.UpdateSpring(Time.deltaTime, mySpring);
    mySpring.ProcessCandidateValue();
    mySpring.CheckEvents();
    
    // Apply the spring value
    float currentValue = mySpring.GetCurrentValue();
    ApplyValue(currentValue);
}
```

**Explanation**: Separating spring creation and update logic makes your code more organized and easier to maintain.

#### Use the Complete Spring Update Sequence

**Pattern**: Always use the complete spring update sequence.

**Example**:
```csharp
// Good: Using the complete spring update sequence
void Update()
{
    // 1. Update the spring physics
    SpringMath.UpdateSpring(Time.deltaTime, mySpring);
    
    // 2. Process the candidate value
    mySpring.ProcessCandidateValue();
    
    // 3. Check and trigger events
    mySpring.CheckEvents();
    
    // 4. Apply the spring value
    float currentValue = mySpring.GetCurrentValue();
    ApplyValue(currentValue);
}
```

**Explanation**: The complete spring update sequence ensures that the spring physics, value updates, and events are all processed correctly.

#### Clean Up Event Subscriptions

**Pattern**: Always unsubscribe from spring events when they're no longer needed.

**Example**:
```csharp
// Good: Cleaning up event subscriptions
void OnDestroy()
{
    if (mySpring != null && mySpring.springEvents != null)
    {
        mySpring.springEvents.OnTargetReached -= OnTargetReached;
        mySpring.springEvents.OnCurrentValueChanged -= OnCurrentValueChanged;
    }
}
```

**Explanation**: Failing to unsubscribe from events can lead to memory leaks and null reference exceptions.

### Performance Optimization Patterns

#### Disable Unused Springs

**Pattern**: Disable springs that aren't currently needed.

**Example**:
```csharp
// Good: Disabling unused springs
void DisableUnusedSprings()
{
    // If we're only animating position, disable rotation and scale springs
    transformSpring.rotationSpring.springEnabled = false;
    transformSpring.scaleSpring.springEnabled = false;
}
```

**Explanation**: Disabled springs don't perform physics calculations, saving CPU time.

#### Use Common Force and Drag

**Pattern**: Use common force and drag values when appropriate.

**Example**:
```csharp
// Good: Using common force and drag for simpler configuration
void ConfigureSpring()
{
    mySpring.SetCommonForceAndDragEnabled(true);
    mySpring.SetCommonForce(150f);
    mySpring.SetCommonDrag(10f);
}
```

**Explanation**: Using common force and drag values simplifies configuration and can improve performance by reducing the number of calculations.

#### Choose the Right Update Method

**Pattern**: Use the appropriate update method for your spring updates.

**Example**:
```csharp
// Good: Using FixedUpdate for physics-related springs
void FixedUpdate()
{
    SpringMath.UpdateSpring(Time.fixedDeltaTime, physicsSpring);
    physicsSpring.ProcessCandidateValue();
    physicsSpring.CheckEvents();
    
    // Apply the spring value to a physics object
    rigidbody.position = physicsSpring.GetCurrentValue();
}

// Good: Using LateUpdate for camera-related springs
void LateUpdate()
{
    SpringMath.UpdateSpring(Time.deltaTime, cameraSpring);
    cameraSpring.ProcessCandidateValue();
    cameraSpring.CheckEvents();
    
    // Apply the spring value to the camera
    camera.transform.position = cameraSpring.GetCurrentValue();
}
```

**Explanation**: Using the appropriate update method ensures that your springs work correctly with other systems in Unity.

### Architecture Patterns

#### Spring Manager for Global Springs

**Pattern**: Use a spring manager to handle global springs.

**Example**:
```csharp
// Good: Using a spring manager for global springs
public class SpringManager : MonoBehaviour
{
    private static SpringManager instance;
    
    [SerializeField] private SpringSettingsData springSettings;
    
    private Dictionary<string, Spring> globalSprings = new Dictionary<string, Spring>();
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        // Update all global springs
        foreach (var spring in globalSprings.Values)
        {
            if (spring.springEnabled)
            {
                SpringMath.UpdateSpring(Time.deltaTime, spring);
                spring.ProcessCandidateValue();
                spring.CheckEvents();
            }
        }
    }
    
    public static void RegisterGlobalSpring(string key, Spring spring)
    {
        if (instance != null)
        {
            instance.globalSprings[key] = spring;
        }
    }
    
    public static Spring GetGlobalSpring(string key)
    {
        if (instance != null && instance.globalSprings.ContainsKey(key))
        {
            return instance.globalSprings[key];
        }
        
        return null;
    }
}
```

**Explanation**: A spring manager can help you organize and update global springs that need to persist across scenes or be accessed from multiple objects.

#### Spring Factory for Common Spring Types

**Pattern**: Use a spring factory to create pre-configured springs.

**Example**:
```csharp
// Good: Using a spring factory for common spring types
public static class SpringFactory
{
    public static SpringFloat CreateUISpring(float initialValue = 0f)
    {
        SpringFloat spring = new SpringFloat();
        spring.SetCommonForce(200f); // Higher force for snappy UI
        spring.SetCommonDrag(15f);   // Higher drag for less oscillation
        spring.SetCurrentValue(initialValue);
        spring.SetTarget(initialValue);
        spring.Initialize();
        
        return spring;
    }
    
    public static SpringVector3 CreateCameraSpring(Vector3 initialPosition)
    {
        SpringVector3 spring = new SpringVector3();
        spring.SetCommonForce(50f);  // Lower force for smoother camera
        spring.SetCommonDrag(8f);    // Balanced drag for some follow-through
        spring.SetCurrentValue(initialPosition);
        spring.SetTarget(initialPosition);
        spring.Initialize();
        
        return spring;
    }
    
    // Add more factory methods for other common spring types
}
```

**Explanation**: A spring factory can help you create consistently configured springs for common use cases, reducing code duplication and ensuring consistent behavior.

## Anti-Patterns to Avoid

### Component Usage Anti-Patterns

#### Manually Setting Spring Values

**Anti-Pattern**: Directly setting spring values instead of using the provided methods.

**Example**:
```csharp
// Bad: Directly setting spring values
void Update()
{
    // Don't do this!
    transformSpring.positionSpring.springValues[0].SetCurrentValue(newXPosition);
}

// Good: Using the provided methods
void Update()
{
    Vector3 currentPosition = transformSpring.GetCurrentValuePosition();
    currentPosition.x = newXPosition;
    transformSpring.SetCurrentValuePosition(currentPosition);
}
```

**Explanation**: Directly setting spring values bypasses important validation and event triggering, potentially leading to unexpected behavior.

#### Ignoring Initialization

**Anti-Pattern**: Using spring components without ensuring they're initialized.

**Example**:
```csharp
// Bad: Using a spring component without checking initialization
void Start()
{
    // This might not work if the component isn't initialized!
    transformSpring.SetTargetPosition(targetPosition);
}

// Good: Checking initialization before use
void Start()
{
    if (!transformSpring.IsInitialized)
    {
        transformSpring.Initialize();
    }
    
    transformSpring.SetTargetPosition(targetPosition);
}
```

**Explanation**: Uninitialized spring components won't work correctly. Always ensure components are initialized before use.

#### Mixing Local and World Space

**Anti-Pattern**: Mixing local and world space values when using transform springs.

**Example**:
```csharp
// Bad: Mixing local and world space
void Update()
{
    // Don't mix spaces like this!
    if (transformSpring.spaceType == SpaceType.WorldSpace)
    {
        transformSpring.SetTargetPosition(transform.localPosition);
    }
}

// Good: Using consistent spaces
void Update()
{
    if (transformSpring.spaceType == SpaceType.WorldSpace)
    {
        transformSpring.SetTargetPosition(transform.position);
    }
    else
    {
        transformSpring.SetTargetPosition(transform.localPosition);
    }
}
```

**Explanation**: Mixing local and world space values leads to unexpected behavior. Always use the appropriate space for your spring component.

### Core Spring Usage Anti-Patterns

#### Incomplete Spring Update

**Anti-Pattern**: Not using the complete spring update sequence.

**Example**:
```csharp
// Bad: Incomplete spring update
void Update()
{
    // Missing ProcessCandidateValue and CheckEvents!
    SpringMath.UpdateSpring(Time.deltaTime, mySpring);
    
    // This won't work correctly
    float currentValue = mySpring.GetCurrentValue();
    ApplyValue(currentValue);
}

// Good: Complete spring update
void Update()
{
    SpringMath.UpdateSpring(Time.deltaTime, mySpring);
    mySpring.ProcessCandidateValue();
    mySpring.CheckEvents();
    
    float currentValue = mySpring.GetCurrentValue();
    ApplyValue(currentValue);
}
```

**Explanation**: Skipping steps in the spring update sequence can lead to incorrect behavior, such as values not updating or events not firing.

#### Ignoring Event Cleanup

**Anti-Pattern**: Not unsubscribing from spring events.

**Example**:
```csharp
// Bad: Not unsubscribing from events
void OnEnable()
{
    mySpring.springEvents.OnTargetReached += OnTargetReached;
}

// Missing OnDisable or OnDestroy to unsubscribe!

// Good: Proper event subscription and cleanup
void OnEnable()
{
    mySpring.springEvents.OnTargetReached += OnTargetReached;
}

void OnDisable()
{
    mySpring.springEvents.OnTargetReached -= OnTargetReached;
}
```

**Explanation**: Failing to unsubscribe from events can lead to memory leaks and null reference exceptions when the object is destroyed.

#### Recreating Springs Every Frame

**Anti-Pattern**: Creating new spring instances every frame.

**Example**:
```csharp
// Bad: Creating new springs every frame
void Update()
{
    // Don't do this!
    SpringFloat tempSpring = new SpringFloat();
    tempSpring.SetCommonForce(150f);
    tempSpring.SetCommonDrag(10f);
    tempSpring.SetCurrentValue(currentValue);
    tempSpring.SetTarget(targetValue);
    tempSpring.Initialize();
    
    SpringMath.UpdateSpring(Time.deltaTime, tempSpring);
    tempSpring.ProcessCandidateValue();
    
    float newValue = tempSpring.GetCurrentValue();
    ApplyValue(newValue);
}

// Good: Creating springs once and reusing them
private SpringFloat mySpring;

void Awake()
{
    mySpring = new SpringFloat();
    mySpring.SetCommonForce(150f);
    mySpring.SetCommonDrag(10f);
    mySpring.SetCurrentValue(initialValue);
    mySpring.SetTarget(initialValue);
    mySpring.Initialize();
}

void Update()
{
    mySpring.SetTarget(targetValue);
    
    SpringMath.UpdateSpring(Time.deltaTime, mySpring);
    mySpring.ProcessCandidateValue();
    mySpring.CheckEvents();
    
    float newValue = mySpring.GetCurrentValue();
    ApplyValue(newValue);
}
```

**Explanation**: Creating new spring instances every frame is inefficient and prevents the spring from maintaining state between frames, defeating the purpose of spring-based animation.

### Performance Anti-Patterns

#### Excessive Force Values

**Anti-Pattern**: Using extremely high force values.

**Example**:
```csharp
// Bad: Using excessive force values
void ConfigureSpring()
{
    // Don't use extremely high values like this!
    mySpring.SetCommonForce(10000f);
    mySpring.SetCommonDrag(5f);
}

// Good: Using reasonable force values
void ConfigureSpring()
{
    mySpring.SetCommonForce(150f);
    mySpring.SetCommonDrag(10f);
}
```

**Explanation**: Extremely high force values can cause instability and may require the more expensive analytical solution, hurting performance.

#### Updating Disabled Springs

**Anti-Pattern**: Updating springs that are disabled or not visible.

**Example**:
```csharp
// Bad: Updating springs that aren't needed
void Update()
{
    // Don't update springs for objects that aren't visible!
    foreach (var spring in allSprings)
    {
        SpringMath.UpdateSpring(Time.deltaTime, spring);
        spring.ProcessCandidateValue();
        spring.CheckEvents();
    }
}

// Good: Only updating springs that are needed
void Update()
{
    foreach (var spring in allSprings)
    {
        if (spring.springEnabled && IsVisible(spring))
        {
            SpringMath.UpdateSpring(Time.deltaTime, spring);
            spring.ProcessCandidateValue();
            spring.CheckEvents();
        }
    }
}
```

**Explanation**: Updating springs that aren't needed wastes CPU time. Only update springs that are enabled and visible.

#### Enabling Events When Not Needed

**Anti-Pattern**: Enabling events when they're not needed.

**Example**:
```csharp
// Bad: Enabling events when they're not needed
void ConfigureSpring()
{
    // Don't enable events if you're not using them!
    mySpring.eventsEnabled = true;
}

// Good: Only enabling events when they're needed
void ConfigureSpring()
{
    // Only enable events if you're actually subscribing to them
    mySpring.eventsEnabled = needEvents;
    
    if (needEvents)
    {
        mySpring.springEvents.OnTargetReached += OnTargetReached;
    }
}
```

**Explanation**: Enabled events perform additional checks every frame, which can impact performance if you're not actually using the events.

### Architecture Anti-Patterns

#### Tightly Coupling Springs to Game Logic

**Anti-Pattern**: Tightly coupling spring logic to game logic.

**Example**:
```csharp
// Bad: Tightly coupling springs to game logic
public class Player : MonoBehaviour
{
    private SpringVector3 positionSpring;
    
    void Awake()
    {
        positionSpring = new SpringVector3();
        positionSpring.SetCommonForce(150f);
        positionSpring.SetCommonDrag(10f);
        positionSpring.SetCurrentValue(transform.position);
        positionSpring.SetTarget(transform.position);
        positionSpring.Initialize();
    }
    
    void Update()
    {
        // Game logic mixed with spring logic
        HandleInput();
        UpdateHealth();
        CheckCollisions();
        
        // Spring logic
        SpringMath.UpdateSpring(Time.deltaTime, positionSpring);
        positionSpring.ProcessCandidateValue();
        positionSpring.CheckEvents();
        
        transform.position = positionSpring.GetCurrentValue();
    }
}

// Good: Separating springs into a dedicated component
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private TransformSpringComponent transformSpring;
    
    void Update()
    {
        // Only movement logic here
        Vector3 targetPosition = CalculateTargetPosition();
        transformSpring.SetTargetPosition(targetPosition);
    }
}
```

**Explanation**: Tightly coupling spring logic to game logic makes your code harder to maintain and understand. Separate concerns by using dedicated components or classes for spring-based animation.

#### Global Spring Configuration

**Anti-Pattern**: Using the same spring configuration for all springs.

**Example**:
```csharp
// Bad: Using the same configuration for all springs
public static class GlobalSpringConfig
{
    public static float Force = 150f;
    public static float Drag = 10f;
}

// Using the global configuration everywhere
void ConfigureSpring()
{
    mySpring.SetCommonForce(GlobalSpringConfig.Force);
    mySpring.SetCommonDrag(GlobalSpringConfig.Drag);
}

// Good: Using appropriate configurations for different use cases
void ConfigureUISpring()
{
    uiSpring.SetCommonForce(200f); // Higher force for snappy UI
    uiSpring.SetCommonDrag(15f);   // Higher drag for less oscillation
}

void ConfigureCameraSpring()
{
    cameraSpring.SetCommonForce(50f);  // Lower force for smoother camera
    cameraSpring.SetCommonDrag(8f);    // Balanced drag for some follow-through
}
```

**Explanation**: Different types of animations require different spring configurations. Using the same configuration for everything leads to suboptimal animations.

#### Ignoring Spring Lifecycle

**Anti-Pattern**: Ignoring the spring lifecycle.

**Example**:
```csharp
// Bad: Ignoring the spring lifecycle
public class BadSpringUser : MonoBehaviour
{
    private SpringFloat mySpring;
    
    // Missing Awake or Start to initialize the spring!
    
    void Update()
    {
        // This will cause a NullReferenceException!
        SpringMath.UpdateSpring(Time.deltaTime, mySpring);
    }
    
    // Missing OnDestroy to clean up event subscriptions!
}

// Good: Respecting the spring lifecycle
public class GoodSpringUser : MonoBehaviour
{
    private SpringFloat mySpring;
    
    void Awake()
    {
        // Initialize the spring
        mySpring = new SpringFloat();
        mySpring.SetCommonForce(150f);
        mySpring.SetCommonDrag(10f);
        mySpring.SetCurrentValue(0f);
        mySpring.SetTarget(0f);
        mySpring.Initialize();
        
        // Subscribe to events if needed
        mySpring.eventsEnabled = true;
        mySpring.springEvents.OnTargetReached += OnTargetReached;
    }
    
    void Update()
    {
        // Update the spring
        SpringMath.UpdateSpring(Time.deltaTime, mySpring);
        mySpring.ProcessCandidateValue();
        mySpring.CheckEvents();
        
        // Apply the spring value
        float currentValue = mySpring.GetCurrentValue();
        ApplyValue(currentValue);
    }
    
    void OnDestroy()
    {
        // Clean up event subscriptions
        if (mySpring != null && mySpring.springEvents != null)
        {
            mySpring.springEvents.OnTargetReached -= OnTargetReached;
        }
    }
    
    void OnTargetReached()
    {
        Debug.Log("Target reached!");
    }
}
```

**Explanation**: Springs have a lifecycle that includes initialization, updates, and cleanup. Ignoring this lifecycle can lead to null reference exceptions and memory leaks.

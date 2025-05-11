# USpring API Documentation

## Table of Contents

1. [Core Spring Interfaces](#core-spring-interfaces)
   - [ISpring](#ispring)
   - [ISpringFloat](#ispringfloat)
   - [ISpringVector2](#ispringvector2)
   - [ISpringVector3](#ispringvector3)
   - [ISpringVector4](#ispringvector4)
   - [ISpringRotation](#ispringrotation)
   - [ISpringColor](#ispringcolor)
2. [Core Spring Classes](#core-spring-classes)
   - [Spring](#spring)
   - [SpringFloat](#springfloat)
   - [SpringVector2](#springvector2)
   - [SpringVector3](#springvector3)
   - [SpringVector4](#springvector4)
   - [SpringRotation](#springrotation)
   - [SpringColor](#springcolor)
   - [SpringValues](#springvalues)
   - [SpringEvents](#springevents)
   - [SpringFactory](#springfactory)
3. [Physics System](#physics-system)
   - [SpringMath](#springmath)
   - [IPhysicsModel](#iphysicsmodel)
   - [SemiImplicitModel](#semiimplicitmodel)
   - [AnalyticalModel](#analyticalmodel)
   - [PhysicsParameters](#physicsparameters)
   - [PhysicsModelFactory](#physicsmodelfactory)
4. [Spring Components](#spring-components)
   - [SpringComponent](#springcomponent)
   - [TransformSpringComponent](#transformspringcomponent)
   - [ColorSpringComponent](#colorspringcomponent)
   - [CamFovOrSizeSpringComponent](#camfovorsizespringcomponent)
   - [Other Components](#other-components)
5. [Settings and Configuration](#settings-and-configuration)
   - [SpringSettingsData](#springsettingsdata)
   - [SpringSettingsProvider](#springsettingsprovider)

## Core Spring Interfaces

### ISpring

`ISpring` is the base interface for all spring types. It provides common functionality for spring-based animation.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `Initialize()` | `ISpring` | Initializes the spring with default values. |
| `Update(float deltaTime)` | `ISpring` | Updates the spring physics for the given time step. |
| `Update(float deltaTime, float forceThreshold)` | `ISpring` | Updates the spring physics with a custom force threshold. |
| `ReachEquilibrium()` | `ISpring` | Immediately sets the spring to its target value and stops all motion. |
| `SetEnabled(bool enabled)` | `ISpring` | Enables or disables the spring. |
| `IsEnabled()` | `bool` | Gets whether the spring is enabled. |
| `SetCommonForceAndDrag(bool enabled)` | `ISpring` | Enables or disables common force and drag for all axes. |
| `SetCommonForce(float force)` | `ISpring` | Sets the common force (stiffness) value. |
| `GetCommonForce()` | `float` | Gets the common force (stiffness) value. |
| `SetCommonDrag(float drag)` | `ISpring` | Sets the common drag (damping) value. |
| `GetCommonDrag()` | `float` | Gets the common drag (damping) value. |
| `SetCommonForceAndDragValues(float force, float drag)` | `ISpring` | Sets both common force and drag values in a single call. |
| `SetClampingEnabled(bool enabled)` | `ISpring` | Enables or disables clamping for the spring. |
| `IsClampingEnabled()` | `bool` | Gets whether clamping is enabled for the spring. |
| `SetEventsEnabled(bool enabled)` | `ISpring` | Enables or disables events for the spring. |
| `AreEventsEnabled()` | `bool` | Gets whether events are enabled for the spring. |
| `GetEvents()` | `SpringEvents` | Gets the spring events instance for this spring. |
| `IsCloseToStopping()` | `bool` | Determines if the spring is close to stopping. |
| `IsOnTarget()` | `bool` | Determines if the spring has reached its target. |
| `IsClamped()` | `bool` | Determines if the spring is currently clamped. |

### ISpringFloat

`ISpringFloat` is the interface for a single-dimensional spring that animates float values. It inherits from `ISpring`.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `SetTarget(float target)` | `ISpringFloat` | Sets the target value of the spring. |
| `GetTarget()` | `float` | Gets the target value of the spring. |
| `SetCurrentValue(float value)` | `ISpringFloat` | Sets the current value of the spring. |
| `GetCurrentValue()` | `float` | Gets the current value of the spring. |
| `SetVelocity(float velocity)` | `ISpringFloat` | Sets the velocity of the spring. |
| `GetVelocity()` | `float` | Gets the velocity of the spring. |
| `AddVelocity(float velocityToAdd)` | `ISpringFloat` | Adds to the current velocity of the spring. |
| `SetForce(float force)` | `ISpringFloat` | Sets the force (stiffness) of the spring. |
| `GetForce()` | `float` | Gets the force (stiffness) of the spring. |
| `SetDrag(float drag)` | `ISpringFloat` | Sets the drag (damping) of the spring. |
| `GetDrag()` | `float` | Gets the drag (damping) of the spring. |
| `SetMinValue(float minValue)` | `ISpringFloat` | Sets the minimum value for clamping. |
| `GetMinValue()` | `float` | Gets the minimum value for clamping. |
| `SetMaxValue(float maxValue)` | `ISpringFloat` | Sets the maximum value for clamping. |
| `GetMaxValue()` | `float` | Gets the maximum value for clamping. |
| `SetClampRange(float minValue, float maxValue)` | `ISpringFloat` | Sets the minimum and maximum values for clamping in a single call. |
| `SetClampTarget(bool enabled)` | `ISpringFloat` | Enables or disables target clamping. |
| `IsClampTargetEnabled()` | `bool` | Gets whether target clamping is enabled. |
| `SetClampCurrentValue(bool enabled)` | `ISpringFloat` | Enables or disables current value clamping. |
| `IsClampCurrentValueEnabled()` | `bool` | Gets whether current value clamping is enabled. |
| `SetStopOnClamp(bool enabled)` | `ISpringFloat` | Enables or disables stopping the spring when the current value is clamped. |
| `DoesStopOnClamp()` | `bool` | Gets whether the spring stops when the current value is clamped. |
| `ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, float minValue, float maxValue)` | `ISpringFloat` | Configures clamping settings in a single call. |
| `Configure(float force, float drag, float initialValue, float target)` | `ISpringFloat` | Configures the spring in a single call. |
| `Clone()` | `ISpringFloat` | Creates a new instance of ISpringFloat with the same configuration. |

### ISpringVector3

`ISpringVector3` is the interface for a three-dimensional spring that animates Vector3 values. It inherits from `ISpring`.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `SetTarget(Vector3 target)` | `ISpringVector3` | Sets the target value of the spring. |
| `SetTarget(float uniformValue)` | `ISpringVector3` | Sets the target value using a uniform value for all components. |
| `GetTarget()` | `Vector3` | Gets the target value of the spring. |
| `SetCurrentValue(Vector3 value)` | `ISpringVector3` | Sets the current value of the spring. |
| `SetCurrentValue(float uniformValue)` | `ISpringVector3` | Sets the current value using a uniform value for all components. |
| `GetCurrentValue()` | `Vector3` | Gets the current value of the spring. |
| `SetVelocity(Vector3 velocity)` | `ISpringVector3` | Sets the velocity of the spring. |
| `SetVelocity(float uniformVelocity)` | `ISpringVector3` | Sets the velocity using a uniform value for all components. |
| `GetVelocity()` | `Vector3` | Gets the velocity of the spring. |
| `AddVelocity(Vector3 velocityToAdd)` | `ISpringVector3` | Adds to the current velocity of the spring. |
| `AddVelocity(float uniformVelocityToAdd)` | `ISpringVector3` | Adds to the current velocity using a uniform value for all components. |
| `SetForce(Vector3 force)` | `ISpringVector3` | Sets the force (stiffness) of the spring. |
| `SetForce(float uniformForce)` | `ISpringVector3` | Sets the force using a uniform value for all components. |
| `GetForce()` | `Vector3` | Gets the force (stiffness) of the spring. |
| `SetDrag(Vector3 drag)` | `ISpringVector3` | Sets the drag (damping) of the spring. |
| `SetDrag(float uniformDrag)` | `ISpringVector3` | Sets the drag using a uniform value for all components. |
| `GetDrag()` | `Vector3` | Gets the drag (damping) of the spring. |
| `SetMinValues(Vector3 minValues)` | `ISpringVector3` | Sets the minimum values for clamping. |
| `SetMinValues(float uniformMinValue)` | `ISpringVector3` | Sets the minimum values using a uniform value for all components. |
| `GetMinValues()` | `Vector3` | Gets the minimum values for clamping. |
| `SetMaxValues(Vector3 maxValues)` | `ISpringVector3` | Sets the maximum values for clamping. |
| `SetMaxValues(float uniformMaxValue)` | `ISpringVector3` | Sets the maximum values using a uniform value for all components. |
| `GetMaxValues()` | `Vector3` | Gets the maximum values for clamping. |
| `SetClampRange(Vector3 minValues, Vector3 maxValues)` | `ISpringVector3` | Sets the minimum and maximum values for clamping in a single call. |
| `SetClampRange(float uniformMinValue, float uniformMaxValue)` | `ISpringVector3` | Sets the minimum and maximum values using uniform values for all components. |
| `SetClampTarget(bool enabled)` | `ISpringVector3` | Enables or disables target clamping. |
| `IsClampTargetEnabled()` | `bool` | Gets whether target clamping is enabled. |
| `SetClampCurrentValue(bool enabled)` | `ISpringVector3` | Enables or disables current value clamping. |
| `IsClampCurrentValueEnabled()` | `bool` | Gets whether current value clamping is enabled. |
| `SetStopOnClamp(bool enabled)` | `ISpringVector3` | Enables or disables stopping the spring when the current value is clamped. |
| `DoesStopOnClamp()` | `bool` | Gets whether the spring stops when the current value is clamped. |
| `ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector3 minValues, Vector3 maxValues)` | `ISpringVector3` | Configures clamping settings in a single call. |
| `Configure(Vector3 force, Vector3 drag, Vector3 initialValue, Vector3 target)` | `ISpringVector3` | Configures the spring in a single call. |
| `Configure(float uniformForce, float uniformDrag, Vector3 initialValue, Vector3 target)` | `ISpringVector3` | Configures the spring using uniform values for force and drag. |
| `Clone()` | `ISpringVector3` | Creates a new instance of ISpringVector3 with the same configuration. |

### ISpringRotation

`ISpringRotation` is the interface for a spring that animates Quaternion rotations. It inherits from `ISpring`.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `SetTarget(Quaternion target)` | `ISpringRotation` | Sets the target rotation of the spring. |
| `SetTarget(Vector3 eulerAngles)` | `ISpringRotation` | Sets the target rotation using Euler angles. |
| `GetTarget()` | `Quaternion` | Gets the target rotation of the spring. |
| `GetTargetEuler()` | `Vector3` | Gets the target rotation as Euler angles. |
| `SetCurrentValue(Quaternion value)` | `ISpringRotation` | Sets the current rotation of the spring. |
| `SetCurrentValue(Vector3 eulerAngles)` | `ISpringRotation` | Sets the current rotation using Euler angles. |
| `GetCurrentValue()` | `Quaternion` | Gets the current rotation of the spring. |
| `GetCurrentValueEuler()` | `Vector3` | Gets the current rotation as Euler angles. |
| `SetVelocity(Vector3 velocity)` | `ISpringRotation` | Sets the angular velocity of the spring. |
| `GetVelocity()` | `Vector3` | Gets the angular velocity of the spring. |
| `AddVelocity(Vector3 velocityToAdd)` | `ISpringRotation` | Adds to the current angular velocity of the spring. |
| `SetForce(Vector3 force)` | `ISpringRotation` | Sets the force (stiffness) of the spring. |
| `SetForce(float uniformForce)` | `ISpringRotation` | Sets the force using a uniform value for all components. |
| `GetForce()` | `Vector3` | Gets the force (stiffness) of the spring. |
| `SetDrag(Vector3 drag)` | `ISpringRotation` | Sets the drag (damping) of the spring. |
| `SetDrag(float uniformDrag)` | `ISpringRotation` | Sets the drag using a uniform value for all components. |
| `GetDrag()` | `Vector3` | Gets the drag (damping) of the spring. |
| `SetMinValues(Vector3 minValues)` | `ISpringRotation` | Sets the minimum values for clamping in Euler angles. |
| `GetMinValues()` | `Vector3` | Gets the minimum values for clamping in Euler angles. |
| `SetMaxValues(Vector3 maxValues)` | `ISpringRotation` | Sets the maximum values for clamping in Euler angles. |
| `GetMaxValues()` | `Vector3` | Gets the maximum values for clamping in Euler angles. |
| `SetClampRange(Vector3 minValues, Vector3 maxValues)` | `ISpringRotation` | Sets the minimum and maximum values for clamping in Euler angles in a single call. |
| `SetClampTarget(bool enabled)` | `ISpringRotation` | Enables or disables target clamping. |
| `IsClampTargetEnabled()` | `bool` | Gets whether target clamping is enabled. |
| `SetClampCurrentValue(bool enabled)` | `ISpringRotation` | Enables or disables current value clamping. |
| `IsClampCurrentValueEnabled()` | `bool` | Gets whether current value clamping is enabled. |
| `SetStopOnClamp(bool enabled)` | `ISpringRotation` | Enables or disables stopping the spring when the current value is clamped. |
| `DoesStopOnClamp()` | `bool` | Gets whether the spring stops when the current value is clamped. |
| `ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector3 minValues, Vector3 maxValues)` | `ISpringRotation` | Configures clamping settings in a single call. |
| `Configure(Vector3 force, Vector3 drag, Quaternion initialValue, Quaternion target)` | `ISpringRotation` | Configures the spring in a single call. |
| `Configure(float uniformForce, float uniformDrag, Quaternion initialValue, Quaternion target)` | `ISpringRotation` | Configures the spring using uniform values for force and drag. |
| `Clone()` | `ISpringRotation` | Creates a new instance of ISpringRotation with the same configuration. |

### ISpringColor

`ISpringColor` is the interface for a spring that animates Color values. It inherits from `ISpring`.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `SetTarget(Color target)` | `ISpringColor` | Sets the target color of the spring. |
| `GetTarget()` | `Color` | Gets the target color of the spring. |
| `SetCurrentValue(Color value)` | `ISpringColor` | Sets the current color of the spring. |
| `GetCurrentValue()` | `Color` | Gets the current color of the spring. |
| `SetVelocity(Vector4 velocity)` | `ISpringColor` | Sets the velocity of the spring. |
| `GetVelocity()` | `Vector4` | Gets the velocity of the spring. |
| `AddVelocity(Vector4 velocityToAdd)` | `ISpringColor` | Adds to the current velocity of the spring. |
| `SetForce(Vector4 force)` | `ISpringColor` | Sets the force (stiffness) of the spring. |
| `SetForce(float uniformForce)` | `ISpringColor` | Sets the force using a uniform value for all components. |
| `GetForce()` | `Vector4` | Gets the force (stiffness) of the spring. |
| `SetDrag(Vector4 drag)` | `ISpringColor` | Sets the drag (damping) of the spring. |
| `SetDrag(float uniformDrag)` | `ISpringColor` | Sets the drag using a uniform value for all components. |
| `GetDrag()` | `Vector4` | Gets the drag (damping) of the spring. |
| `SetMinValues(Vector4 minValues)` | `ISpringColor` | Sets the minimum values for clamping. |
| `SetMinValues(float uniformMinValue)` | `ISpringColor` | Sets the minimum values using a uniform value for all components. |
| `GetMinValues()` | `Vector4` | Gets the minimum values for clamping. |
| `SetMaxValues(Vector4 maxValues)` | `ISpringColor` | Sets the maximum values for clamping. |
| `SetMaxValues(float uniformMaxValue)` | `ISpringColor` | Sets the maximum values using a uniform value for all components. |
| `GetMaxValues()` | `Vector4` | Gets the maximum values for clamping. |
| `SetClampRange(Vector4 minValues, Vector4 maxValues)` | `ISpringColor` | Sets the minimum and maximum values for clamping in a single call. |
| `SetClampRange(float uniformMinValue, float uniformMaxValue)` | `ISpringColor` | Sets the minimum and maximum values using uniform values for all components. |
| `SetClampTarget(bool enabled)` | `ISpringColor` | Enables or disables target clamping. |
| `IsClampTargetEnabled()` | `bool` | Gets whether target clamping is enabled. |
| `SetClampCurrentValue(bool enabled)` | `ISpringColor` | Enables or disables current value clamping. |
| `IsClampCurrentValueEnabled()` | `bool` | Gets whether current value clamping is enabled. |
| `SetStopOnClamp(bool enabled)` | `ISpringColor` | Enables or disables stopping the spring when the current value is clamped. |
| `DoesStopOnClamp()` | `bool` | Gets whether the spring stops when the current value is clamped. |
| `ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector4 minValues, Vector4 maxValues)` | `ISpringColor` | Configures clamping settings in a single call. |
| `Configure(Vector4 force, Vector4 drag, Color initialValue, Color target)` | `ISpringColor` | Configures the spring in a single call. |
| `Configure(float uniformForce, float uniformDrag, Color initialValue, Color target)` | `ISpringColor` | Configures the spring using uniform values for force and drag. |
| `Clone()` | `ISpringColor` | Creates a new instance of ISpringColor with the same configuration. |

## Core Spring Classes

### Spring

`Spring` is the abstract base class for all spring types. It provides common functionality for spring-based animation.

#### Properties

| Name | Type | Description |
|------|------|-------------|
| `commonForceAndDrag` | `bool` | When true, uses the same force and drag values for all axes. |
| `commonForce` | `float` | The common force (stiffness) value used when `commonForceAndDrag` is true. |
| `commonDrag` | `float` | The common drag (damping) value used when `commonForceAndDrag` is true. |
| `springEnabled` | `bool` | Controls whether the spring simulation is active. |
| `clampingEnabled` | `bool` | Controls whether value clamping is enabled. |
| `eventsEnabled` | `bool` | Controls whether spring events are enabled. |
| `springEvents` | `SpringEvents` | The SpringEvents instance that manages event handling for this spring. |

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `Initialize()` | `void` | Initializes the spring by setting up each spring value and configuring the spring events. |
| `ProcessCandidateValue()` | `void` | Applies the candidate values calculated during the spring update to the current values. |
| `CheckEvents()` | `void` | Checks and triggers any spring events that should fire based on the current state. |
| `ReachEquilibrium()` | `void` | Immediately sets all spring values to their target values and stops all motion. |
| `IsCloseToStopping()` | `bool` | Determines if the spring is close to stopping (velocity is below threshold). |
| `IsOnTarget()` | `bool` | Determines if the spring has reached its target (current value is close enough to target). |
| `HasCurrentValueChanged()` | `bool` | Determines if any of the spring values have changed significantly since the last update. |
| `SetCommonForce(float force)` | `void` | Sets the common force (stiffness) value used when `commonForceAndDrag` is true. |
| `SetCommonDrag(float drag)` | `void` | Sets the common drag (damping) value used when `commonForceAndDrag` is true. |
| `SetClampingEnabled(bool clampingEnabled)` | `void` | Enables or disables clamping for all spring values. |

### SpringFloat

`SpringFloat` is a single-dimensional spring for animating float values. It inherits from `Spring`.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `GetTarget()` | `float` | Gets the target value of the spring. |
| `SetTarget(float target)` | `void` | Sets the target value of the spring. |
| `GetCurrentValue()` | `float` | Gets the current value of the spring. |
| `SetCurrentValue(float currentValue)` | `void` | Sets the current value of the spring. |
| `GetVelocity()` | `float` | Gets the current velocity of the spring. |
| `SetVelocity(float velocity)` | `void` | Sets the current velocity of the spring. |
| `AddVelocity(float velocityToAdd)` | `void` | Adds to the current velocity of the spring. |

### SpringVector3

`SpringVector3` is a three-dimensional spring for animating Vector3 values. It inherits from `Spring`.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `GetTarget()` | `Vector3` | Gets the target value of the spring. |
| `SetTarget(Vector3 target)` | `void` | Sets the target value of the spring. |
| `GetCurrentValue()` | `Vector3` | Gets the current value of the spring. |
| `SetCurrentValue(Vector3 currentValue)` | `void` | Sets the current value of the spring. |
| `GetVelocity()` | `Vector3` | Gets the current velocity of the spring. |
| `SetVelocity(Vector3 velocity)` | `void` | Sets the current velocity of the spring. |
| `AddVelocity(Vector3 velocityToAdd)` | `void` | Adds to the current velocity of the spring. |

### SpringRotation

`SpringRotation` is a special spring for animating Quaternion rotations with proper interpolation. It inherits from `Spring`.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `GetTarget()` | `Quaternion` | Gets the target rotation of the spring. |
| `SetTarget(Quaternion target)` | `void` | Sets the target rotation of the spring. |
| `SetTarget(Vector3 eulerAngles)` | `void` | Sets the target rotation of the spring using Euler angles. |
| `GetCurrentValue()` | `Quaternion` | Gets the current rotation of the spring. |
| `SetCurrentValue(Quaternion currentValue)` | `void` | Sets the current rotation of the spring. |
| `SetCurrentValue(Vector3 eulerAngles)` | `void` | Sets the current rotation of the spring using Euler angles. |
| `GetVelocity()` | `Vector3` | Gets the current angular velocity of the spring. |
| `SetVelocity(Vector3 velocity)` | `void` | Sets the current angular velocity of the spring. |
| `AddVelocity(Vector3 velocityToAdd)` | `void` | Adds to the current angular velocity of the spring. |

### SpringValues

`SpringValues` represents a single spring-driven value with its own properties.

#### Properties

| Name | Type | Description |
|------|------|-------------|
| `clampTarget` | `bool` | When true, the target value will be clamped to the min/max range. |
| `clampCurrentValue` | `bool` | When true, the current value will be clamped to the min/max range. |
| `stopSpringOnCurrentValueClamp` | `bool` | When true, the spring will stop when the current value is clamped. |
| `minValue` | `float` | The minimum value for clamping. |
| `maxValue` | `float` | The maximum value for clamping. |
| `update` | `bool` | Controls whether this spring value is updated. |
| `force` | `float` | The force (stiffness) value for this spring value. |
| `drag` | `float` | The drag (damping) value for this spring value. |

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `ReachEquilibrium()` | `void` | Immediately sets the current value to the target value and stops all motion. |
| `SetTarget(float target)` | `void` | Sets the target value. |
| `GetTarget()` | `float` | Gets the target value. |
| `Initialize()` | `void` | Initializes the spring value with its initial value. |
| `AddVelocity(float velocity)` | `void` | Adds to the current velocity. |
| `Stop()` | `void` | Stops all motion by setting velocity to zero. |
| `ApplyCandidateValue()` | `void` | Applies the candidate value to the current value. |
| `GetCurrentValue()` | `float` | Gets the current value. |
| `GetVelocity()` | `float` | Gets the current velocity. |
| `SetVelocity(float velocity)` | `void` | Sets the current velocity. |

### SpringEvents

`SpringEvents` provides a system for responding to spring state changes through events.

#### Events

| Name | Type | Description |
|------|------|-------------|
| `OnTargetReached` | `Action` | Triggered when the spring reaches its target value. |
| `OnCurrentValueChanged` | `Action` | Triggered when the spring's current value changes significantly. |
| `OnClampingApplied` | `Action` | Triggered when clamping is applied to the spring's value. |

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `SetSpring(Spring spring)` | `void` | Sets the spring that this events instance is associated with. |
| `CheckEvents()` | `void` | Checks and triggers any events that should fire based on the current state. |

### SpringFactory

`SpringFactory` is a static class that provides factory methods for creating spring instances with a fluent API.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `CreateFloat()` | `ISpringFloat` | Creates a new SpringFloat instance with default settings. |
| `CreateFloat(float target)` | `ISpringFloat` | Creates a new SpringFloat instance with the specified target value. |
| `CreateFloat(float target, float currentValue)` | `ISpringFloat` | Creates a new SpringFloat instance with the specified target and current values. |
| `CreateFloatWithForceAndDrag(float force, float drag)` | `ISpringFloat` | Creates a new SpringFloat instance with the specified force and drag values. |
| `CreateFloatWithForceAndDrag(float force, float drag, float target, float currentValue)` | `ISpringFloat` | Creates a new SpringFloat instance with the specified force, drag, target, and current values. |
| `CreateVector3()` | `ISpringVector3` | Creates a new SpringVector3 instance with default settings. |
| `CreateVector3(Vector3 target)` | `ISpringVector3` | Creates a new SpringVector3 instance with the specified target value. |
| `CreateVector3(Vector3 target, Vector3 currentValue)` | `ISpringVector3` | Creates a new SpringVector3 instance with the specified target and current values. |
| `CreateVector3WithForceAndDrag(float force, float drag)` | `ISpringVector3` | Creates a new SpringVector3 instance with the specified force and drag values. |
| `CreateVector3WithForceAndDrag(float force, float drag, Vector3 target, Vector3 currentValue)` | `ISpringVector3` | Creates a new SpringVector3 instance with the specified force, drag, target, and current values. |
| `CreateRotation()` | `ISpringRotation` | Creates a new SpringRotation instance with default settings. |
| `CreateRotation(Quaternion target)` | `ISpringRotation` | Creates a new SpringRotation instance with the specified target value. |
| `CreateRotation(Quaternion target, Quaternion currentValue)` | `ISpringRotation` | Creates a new SpringRotation instance with the specified target and current values. |
| `CreateRotationWithForceAndDrag(float force, float drag)` | `ISpringRotation` | Creates a new SpringRotation instance with the specified force and drag values. |
| `CreateRotationWithForceAndDrag(float force, float drag, Quaternion target, Quaternion currentValue)` | `ISpringRotation` | Creates a new SpringRotation instance with the specified force, drag, target, and current values. |
| `CreateColor()` | `ISpringColor` | Creates a new SpringColor instance with default settings. |
| `CreateColor(Color target)` | `ISpringColor` | Creates a new SpringColor instance with the specified target value. |
| `CreateColor(Color target, Color currentValue)` | `ISpringColor` | Creates a new SpringColor instance with the specified target and current values. |
| `CreateColorWithForceAndDrag(float force, float drag)` | `ISpringColor` | Creates a new SpringColor instance with the specified force and drag values. |
| `CreateColorWithForceAndDrag(float force, float drag, Color target, Color currentValue)` | `ISpringColor` | Creates a new SpringColor instance with the specified force, drag, target, and current values. |
| `CreateUIPreset()` | `ISpringFloat` | Creates a preset spring for UI animations (snappy with minimal oscillation). |
| `CreateCameraPreset()` | `ISpringVector3` | Creates a preset spring for camera animations (smooth with some follow-through). |
| `CreateBouncyPreset()` | `ISpringFloat` | Creates a preset spring for bouncy animations (low drag for more oscillation). |
| `CreateSmoothPreset()` | `ISpringFloat` | Creates a preset spring for smooth animations (low force for slower response). |

## Spring Components

### SpringComponent

`SpringComponent` is the abstract base class for all spring components. It provides common functionality for spring-based animation components.

#### Properties

| Name | Type | Description |
|------|------|-------------|
| `doesAutoInitialize` | `bool` | When true, the component will automatically initialize on Awake. |
| `useScaledTime` | `bool` | When true, uses Time.deltaTime; when false, uses Time.unscaledDeltaTime. |
| `alwaysUseAnalyticalSolution` | `bool` | When true, always uses the analytical solution for spring integration. |
| `hasCustomInitialValues` | `bool` | When true, uses custom initial values instead of the current values. |
| `hasCustomTarget` | `bool` | When true, uses custom target values instead of automatically determining them. |

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `Initialize()` | `void` | Initializes the component and its springs. |
| `ReachEquilibrium()` | `void` | Immediately sets all spring values to their target values and stops all motion. |
| `IsInitialized` | `bool` | Gets whether the component has been initialized. |
| `IsValidSpringComponent()` | `bool` | Determines if the component has all necessary references and is properly configured. |

### TransformSpringComponent

`TransformSpringComponent` animates Transform properties (position, rotation, scale). It inherits from `SpringComponent`.

#### Properties

| Name | Type | Description |
|------|------|-------------|
| `spaceType` | `SpaceType` | Determines whether to use world space or local space for position and rotation. |
| `followerTransform` | `Transform` | The Transform that will be animated by the spring. |
| `useTransformAsTarget` | `bool` | When true, uses targetTransform as the target for position, rotation, and scale. |
| `targetTransform` | `Transform` | The Transform to use as the target when useTransformAsTarget is true. |

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `SetTargetPosition(Vector3 target)` | `void` | Sets the target position. |
| `GetCurrentValuePosition()` | `Vector3` | Gets the current position. |
| `SetVelocityPosition(Vector3 velocity)` | `void` | Sets the current position velocity. |
| `AddVelocityPosition(Vector3 velocityToAdd)` | `void` | Adds to the current position velocity. |
| `SetTargetRotation(Quaternion target)` | `void` | Sets the target rotation. |
| `GetCurrentValueRotation()` | `Quaternion` | Gets the current rotation. |
| `SetVelocityRotation(Vector3 velocity)` | `void` | Sets the current rotation velocity. |
| `AddVelocityRotation(Vector3 velocityToAdd)` | `void` | Adds to the current rotation velocity. |
| `SetTargetScale(Vector3 target)` | `void` | Sets the target scale. |
| `GetCurrentValueScale()` | `Vector3` | Gets the current scale. |
| `SetVelocityScale(Vector3 velocity)` | `void` | Sets the current scale velocity. |
| `AddVelocityScale(Vector3 velocityToAdd)` | `void` | Adds to the current scale velocity. |

### ColorSpringComponent

`ColorSpringComponent` animates colors of renderers or UI elements. It inherits from `SpringComponent`.

#### Properties

| Name | Type | Description |
|------|------|-------------|
| `autoUpdate` | `bool` | When true, automatically updates the target renderer or UI element. |
| `autoUpdatedObjectIsRenderer` | `bool` | When true, updates a Renderer; when false, updates a UI Graphic. |
| `autoUpdatedRenderer` | `Renderer` | The Renderer to update when autoUpdate is true and autoUpdatedObjectIsRenderer is true. |
| `autoUpdatedUiGraphic` | `Graphic` | The UI Graphic to update when autoUpdate is true and autoUpdatedObjectIsRenderer is false. |

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `SetTarget(Color target)` | `void` | Sets the target color. |
| `GetCurrentValue()` | `Color` | Gets the current color. |
| `SetVelocity(Vector4 velocity)` | `void` | Sets the current color velocity. |
| `AddVelocity(Vector4 velocityToAdd)` | `void` | Adds to the current color velocity. |

### CamFovOrSizeSpringComponent

`CamFovOrSizeSpringComponent` animates camera field of view or orthographic size. It inherits from `SpringComponent`.

#### Properties

| Name | Type | Description |
|------|------|-------------|
| `autoUpdatedCamera` | `Camera` | The Camera to update. |

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `SetTarget(float target)` | `void` | Sets the target field of view or orthographic size. |
| `GetCurrentValue()` | `float` | Gets the current field of view or orthographic size. |
| `SetVelocity(float velocity)` | `void` | Sets the current velocity. |
| `AddVelocity(float velocityToAdd)` | `void` | Adds to the current velocity. |

### SpringPulse

`SpringPulse` creates pulsing animations using springs. It requires a `TransformSpringComponent`.

#### Properties

| Name | Type | Description |
|------|------|-------------|
| `autoStart` | `bool` | When true, automatically starts the pulse animation on enable. |
| `loop` | `bool` | When true, the pulse animation loops continuously. |
| `baseScale` | `Vector3` | The base scale to return to after each pulse. |
| `pulseScale` | `Vector3` | The scale to pulse to. |
| `pulseInterval` | `float` | The time between pulses when looping. |
| `addVelocity` | `bool` | When true, adds velocity for a more bouncy effect. |
| `velocityScale` | `float` | The scale factor for the added velocity. |

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `StartPulse()` | `void` | Starts the pulse animation. |
| `StopPulse()` | `void` | Stops the pulse animation. |
| `TriggerPulse()` | `void` | Triggers a single pulse. |

## Spring Math

### SpringMath

`SpringMath` contains the mathematical implementation of the spring physics simulation.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `UpdateSpring(float deltaTime, Spring spring, float forceThreshold = 7500f)` | `void` | Updates the spring simulation for the given time step. |
| `SemiImplicitIntegration(float deltaTime, float force, float drag, SpringValues springValues)` | `void` | Updates the spring using semi-implicit Euler integration. |
| `CoefficientBasedIntegration(float deltaTime, float force, float drag, SpringValues springValues)` | `void` | Updates the spring using an analytical solution based on the damping ratio. |
| `CheckTargetClamping(SpringValues springValues, bool clampingEnabled)` | `void` | Applies clamping to the target value if enabled. |
| `CheckCurrentValueClamping(SpringValues springValues, bool clampingEnabled)` | `void` | Applies clamping to the current value if enabled. |

## Spring Settings

### SpringSettingsData

`SpringSettingsData` is a ScriptableObject that provides global configuration for the spring system.

#### Properties

| Name | Type | Description |
|------|------|-------------|
| `maxForceBeforeAnalyticalIntegration` | `float` | The threshold for switching from semi-implicit to analytical integration. |
| `doFixedUpdateRate` | `bool` | When true, uses a fixed update rate for spring simulation. |
| `springFixedTimeStep` | `float` | The fixed time step to use for spring simulation when doFixedUpdateRate is true. |

### SpringSettingsProvider

`SpringSettingsProvider` provides access to the global spring settings through dependency injection.

#### Methods

| Name | Return Type | Description |
|------|-------------|-------------|
| `GetMaxForceBeforeAnalyticalIntegration()` | `float` | Gets the threshold for switching from semi-implicit to analytical integration. |
| `GetDoFixedUpdateRate()` | `bool` | Gets whether to use a fixed update rate for spring simulation. |
| `GetSpringFixedTimeStep()` | `float` | Gets the fixed time step to use for spring simulation when doFixedUpdateRate is true. |

using UnityEngine;
using USpring.Core.Interfaces;

namespace USpring.Core
{
    /// <summary>
    /// Factory class for creating spring instances with a fluent API.
    /// </summary>
    public static class SpringFactory
    {
        /// <summary>
        /// Creates a new SpringFloat instance with default settings.
        /// </summary>
        /// <returns>A new SpringFloat instance.</returns>
        public static ISpringFloat CreateFloat()
        {
            return (ISpringFloat)new SpringFloat().Initialize();
        }

        /// <summary>
        /// Creates a new SpringFloat instance with the specified target value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <returns>A new SpringFloat instance.</returns>
        public static ISpringFloat CreateFloat(float target)
        {
            return (ISpringFloat)new SpringFloat()
                .SetTarget(target)
                .SetCurrentValue(target)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringFloat instance with the specified target and current values.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>A new SpringFloat instance.</returns>
        public static ISpringFloat CreateFloat(float target, float currentValue)
        {
            return (ISpringFloat)new SpringFloat()
                .SetTarget(target)
                .SetCurrentValue(currentValue)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringFloat instance with the specified force and drag values.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <returns>A new SpringFloat instance.</returns>
        public static ISpringFloat CreateFloatWithForceAndDrag(float force, float drag)
        {
            return (ISpringFloat)new SpringFloat()
                .SetCommonForce(force)
                .SetCommonDrag(drag)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringFloat instance with the specified force, drag, target, and current values.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <param name="target">The target value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>A new SpringFloat instance.</returns>
        public static ISpringFloat CreateFloatWithForceAndDrag(float force, float drag, float target, float currentValue)
        {
            return (ISpringFloat)new SpringFloat()
                .SetTarget(target)
                .SetCurrentValue(currentValue)
                .SetCommonForce(force)
                .SetCommonDrag(drag)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringVector3 instance with default settings.
        /// </summary>
        /// <returns>A new SpringVector3 instance.</returns>
        public static ISpringVector3 CreateVector3()
        {
            return (ISpringVector3)new SpringVector3().Initialize();
        }

        /// <summary>
        /// Creates a new SpringVector3 instance with the specified target value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <returns>A new SpringVector3 instance.</returns>
        public static ISpringVector3 CreateVector3(Vector3 target)
        {
            return (ISpringVector3)new SpringVector3()
                .SetCurrentValue(target)
                .SetTarget(target)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringVector3 instance with the specified target and current values.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>A new SpringVector3 instance.</returns>
        public static ISpringVector3 CreateVector3(Vector3 target, Vector3 currentValue)
        {
            return (ISpringVector3)new SpringVector3()
                .SetTarget(target)
                .SetCurrentValue(currentValue)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringVector3 instance with the specified force and drag values.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <returns>A new SpringVector3 instance.</returns>
        public static ISpringVector3 CreateVector3WithForceAndDrag(float force, float drag)
        {
            return (ISpringVector3)new SpringVector3()
                .SetCommonForce(force)
                .SetCommonDrag(drag)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringVector3 instance with the specified force, drag, target, and current values.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <param name="target">The target value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>A new SpringVector3 instance.</returns>
        public static ISpringVector3 CreateVector3WithForceAndDrag(float force, float drag, Vector3 target, Vector3 currentValue)
        {
            return (ISpringVector3)(new SpringVector3()
                .SetTarget(target)
                .SetCurrentValue(currentValue)
                .SetCommonForce(force)
                .SetCommonDrag(drag)
                .Initialize());
        }

        /// <summary>
        /// Creates a new SpringRotation instance with default settings.
        /// </summary>
        /// <returns>A new SpringRotation instance.</returns>
        public static ISpringRotation CreateRotation()
        {
            return (ISpringRotation)new SpringRotation().Initialize();
        }

        /// <summary>
        /// Creates a new SpringRotation instance with the specified target value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <returns>A new SpringRotation instance.</returns>
        public static ISpringRotation CreateRotation(Quaternion target)
        {
            return (ISpringRotation)new SpringRotation()
                .SetTarget(target)
                .SetCurrentValue(target)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringRotation instance with the specified target and current values.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>A new SpringRotation instance.</returns>
        public static ISpringRotation CreateRotation(Quaternion target, Quaternion currentValue)
        {
            return (ISpringRotation)new SpringRotation()
                .SetTarget(target)
                .SetCurrentValue(currentValue)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringRotation instance with the specified force and drag values.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <returns>A new SpringRotation instance.</returns>
        public static ISpringRotation CreateRotationWithForceAndDrag(float force, float drag)
        {
            return (ISpringRotation)new SpringRotation()
                .SetCommonForce(force)
                .SetCommonDrag(drag)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringRotation instance with the specified force, drag, target, and current values.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <param name="target">The target value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>A new SpringRotation instance.</returns>
        public static ISpringRotation CreateRotationWithForceAndDrag(float force, float drag, Quaternion target, Quaternion currentValue)
        {
            return (ISpringRotation)new SpringRotation()
                .SetTarget(target)
                .SetCurrentValue(currentValue)
                .SetCommonForce(force)
                .SetCommonDrag(drag)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringColor instance with default settings.
        /// </summary>
        /// <returns>A new SpringColor instance.</returns>
        public static ISpringColor CreateColor()
        {
            return (ISpringColor)new SpringColor().Initialize();
        }

        /// <summary>
        /// Creates a new SpringColor instance with the specified target value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <returns>A new SpringColor instance.</returns>
        public static ISpringColor CreateColor(Color target)
        {
            return (ISpringColor)new SpringColor()
                .SetTarget(target)
                .SetCurrentValue(target)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringColor instance with the specified target and current values.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>A new SpringColor instance.</returns>
        public static ISpringColor CreateColor(Color target, Color currentValue)
        {
            return (ISpringColor)new SpringColor()
                .SetTarget(target)
                .SetCurrentValue(currentValue)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringColor instance with the specified force and drag values.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <returns>A new SpringColor instance.</returns>
        public static ISpringColor CreateColorWithForceAndDrag(float force, float drag)
        {
            return (ISpringColor)new SpringColor()
                .SetCommonForce(force)
                .SetCommonDrag(drag)
                .Initialize();
        }

        /// <summary>
        /// Creates a new SpringColor instance with the specified force, drag, target, and current values.
        /// </summary>
        /// <param name="force">The force (stiffness) value.</param>
        /// <param name="drag">The drag (damping) value.</param>
        /// <param name="target">The target value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>A new SpringColor instance.</returns>
        public static ISpringColor CreateColorWithForceAndDrag(float force, float drag, Color target, Color currentValue)
        {
            return (ISpringColor)new SpringColor()
                .SetTarget(target)
                .SetCurrentValue(currentValue)
                .SetCommonForce(force)
                .SetCommonDrag(drag)
                .Initialize();
        }

        /// <summary>
        /// Creates a preset spring for UI animations (snappy with minimal oscillation).
        /// </summary>
        /// <returns>A new SpringFloat instance configured for UI animations.</returns>
        public static ISpringFloat CreateUIPreset()
        {
            return (ISpringFloat)new SpringFloat()
                .SetCommonForce(200f)
                .SetCommonDrag(15f)
                .Initialize();
        }

        /// <summary>
        /// Creates a preset spring for camera animations (smooth with some follow-through).
        /// </summary>
        /// <returns>A new SpringVector3 instance configured for camera animations.</returns>
        public static ISpringVector3 CreateCameraPreset()
        {
            return (ISpringVector3)new SpringVector3()
                .SetCommonForce(50f)
                .SetCommonDrag(8f)
                .Initialize();
        }

        /// <summary>
        /// Creates a preset spring for bouncy animations (low drag for more oscillation).
        /// </summary>
        /// <returns>A new SpringFloat instance configured for bouncy animations.</returns>
        public static ISpringFloat CreateBouncyPreset()
        {
            return (ISpringFloat)new SpringFloat()
                .SetCommonForce(150f)
                .SetCommonDrag(5f)
                .Initialize();
        }

        /// <summary>
        /// Creates a preset spring for smooth animations (low force for slower response).
        /// </summary>
        /// <returns>A new SpringFloat instance configured for smooth animations.</returns>
        public static ISpringFloat CreateSmoothPreset()
        {
            return (ISpringFloat)new SpringFloat()
                .SetCommonForce(50f)
                .SetCommonDrag(10f)
                .Initialize();
        }
    }
}
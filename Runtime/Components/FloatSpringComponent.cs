using UnityEngine;
using USpring.Core;
using USpring.Core.Interfaces;

namespace USpring.Components
{
    /// <summary>
    /// Improved float spring component with reduced boilerplate code and better architecture.
    /// Broad purpose spring class that can be used to animate any float value.
    /// </summary>
    [AddComponentMenu("Ludo/USpring/Components/Float Spring")]
    public class FloatSpringComponent : SpringComponent
    {
        [Header("Spring Configuration")]
        [SerializeField] private float initialValue = 0f;
        [SerializeField] private float targetValue = 0f;

        [Header("Force and Drag")]
        [SerializeField] private bool useCommonForceAndDrag = true;
        [SerializeField] private float commonForce = 150f;
        [SerializeField] private float commonDrag = 10f;

        [Header("Clamping")]
        [SerializeField] private bool enableClamping = false;
        [SerializeField] private float minValue = 0f;
        [SerializeField] private float maxValue = 100f;
        [SerializeField] private bool clampCurrentValue = true;
        [SerializeField] private bool clampTarget = true;
        [SerializeField] private bool stopOnClamp = false;

        [SerializeField] private SpringFloat spring = new SpringFloat();

        /// <summary>
        /// Gets the main spring managed by this component.
        /// </summary>
        public SpringFloat Spring => spring;

        /// <summary>
        /// Gets the spring events for the main spring.
        /// </summary>
        public SpringEvents Events => spring.springEvents;

        private void Awake()
        {
            // Initialize spring if it's null
            if (spring == null)
            {
                spring = new SpringFloat();
            }
        }

        public override void Initialize()
        {
            // Configure spring before initialization
            ConfigureSpring();
            base.Initialize();
        }

        private void ConfigureSpring()
        {
            if (spring == null) return;

            // Configure force and drag
            if (useCommonForceAndDrag)
            {
                spring.SetCommonForceAndDrag(true);
                spring.SetCommonForce(commonForce);
                spring.SetCommonDrag(commonDrag);
            }

            // Configure clamping
            spring.SetClampingEnabled(enableClamping);
            if (enableClamping)
            {
                spring.SetMinValue(minValue);
                spring.SetMaxValue(maxValue);
                spring.SetClampCurrentValue(clampCurrentValue);
                spring.SetClampTarget(clampTarget);
                spring.SetStopOnClamp(stopOnClamp);
            }
        }

        protected override void RegisterSprings()
        {
            if (spring != null)
            {
                RegisterSpring(spring);
            }
        }

        protected override void SetCurrentValueByDefault()
        {
            spring?.SetCurrentValue(initialValue);
        }

        protected override void SetTargetByDefault()
        {
            spring?.SetTarget(targetValue);
        }

        /// <summary>
        /// Gets the current target value of the spring.
        /// </summary>
        public float GetTarget() => spring?.GetTarget() ?? targetValue;

        /// <summary>
        /// Sets the target value that the spring will move towards.
        /// </summary>
        /// <param name="target">The new target value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public virtual FloatSpringComponent SetTarget(float target)
        {
            targetValue = target;
            spring?.SetTarget(target);
            return this;
        }

        /// <summary>
        /// Gets the current animated value of the spring.
        /// </summary>
        public float GetCurrentValue() => spring?.GetCurrentValue() ?? initialValue;

        /// <summary>
        /// Sets the current value of the spring.
        /// </summary>
        /// <param name="currentValue">The new current value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public virtual FloatSpringComponent SetCurrentValue(float currentValue)
        {
            initialValue = currentValue;
            spring?.SetCurrentValue(currentValue);
            return this;
        }

        /// <summary>
        /// Gets the current velocity of the spring.
        /// </summary>
        public float GetVelocity() => spring?.GetVelocity() ?? 0f;

        /// <summary>
        /// Sets the velocity of the spring.
        /// </summary>
        /// <param name="velocity">The new velocity value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public virtual FloatSpringComponent SetVelocity(float velocity)
        {
            spring?.SetVelocity(velocity);
            return this;
        }

        /// <summary>
        /// Adds velocity to the current spring velocity.
        /// </summary>
        /// <param name="velocity">The velocity to add.</param>
        /// <returns>This component instance for method chaining.</returns>
        public virtual FloatSpringComponent AddVelocity(float velocity)
        {
            spring?.AddVelocity(velocity);
            return this;
        }

        /// <summary>
        /// Immediately sets the spring to its target value and stops all motion.
        /// </summary>
        public override void ReachEquilibrium()
        {
            base.ReachEquilibrium();
        }

        /// <summary>
        /// Immediately sets the spring to its target value and stops all motion.
        /// </summary>
        /// <returns>This component instance for method chaining.</returns>
        public virtual FloatSpringComponent ReachEquilibriumAndReturn()
        {
            ReachEquilibrium();
            return this;
        }

        /// <summary>
        /// Sets the common force (stiffness) value.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public FloatSpringComponent SetCommonForce(float force)
        {
            commonForce = force;
            if (spring != null && useCommonForceAndDrag)
            {
                spring.SetCommonForceAndDrag(true);
                spring.SetCommonForce(force);
            }
            return this;
        }

        /// <summary>
        /// Sets the common drag (damping) value.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public FloatSpringComponent SetCommonDrag(float drag)
        {
            commonDrag = drag;
            if (spring != null && useCommonForceAndDrag)
            {
                spring.SetCommonForceAndDrag(true);
                spring.SetCommonDrag(drag);
            }
            return this;
        }

        /// <summary>
        /// Sets both common force and drag values.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <param name="drag">The drag value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public FloatSpringComponent SetCommonForceAndDrag(float force, float drag)
        {
            SetCommonForce(force);
            SetCommonDrag(drag);
            return this;
        }

        /// <summary>
        /// Gets the common force value.
        /// </summary>
        /// <returns>The common force value.</returns>
        public float GetCommonForce()
        {
            return spring?.GetCommonForce() ?? commonForce;
        }

        /// <summary>
        /// Gets the common drag value.
        /// </summary>
        /// <returns>The common drag value.</returns>
        public float GetCommonDrag()
        {
            return spring?.GetCommonDrag() ?? commonDrag;
        }

        /// <summary>
        /// Gets the force value.
        /// </summary>
        /// <returns>The force value.</returns>
        public float GetForce()
        {
            return spring?.GetForce() ?? commonForce;
        }

        /// <summary>
        /// Sets the force value.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public FloatSpringComponent SetForce(float force)
        {
            spring?.SetForce(force);
            return this;
        }

        /// <summary>
        /// Gets the drag value.
        /// </summary>
        /// <returns>The drag value.</returns>
        public float GetDrag()
        {
            return spring?.GetDrag() ?? commonDrag;
        }

        /// <summary>
        /// Sets the drag value.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public FloatSpringComponent SetDrag(float drag)
        {
            spring?.SetDrag(drag);
            return this;
        }

        /// <summary>
        /// Sets the minimum value for clamping.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public FloatSpringComponent SetMinValues(float min)
        {
            minValue = min;
            spring?.SetMinValue(min);
            return this;
        }

        /// <summary>
        /// Sets the maximum value for clamping.
        /// </summary>
        /// <param name="max">The maximum value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public FloatSpringComponent SetMaxValues(float max)
        {
            maxValue = max;
            spring?.SetMaxValue(max);
            return this;
        }

        /// <summary>
        /// Sets whether to clamp the current value.
        /// </summary>
        /// <param name="clamp">Whether to clamp the current value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public FloatSpringComponent SetClampCurrentValues(bool clamp)
        {
            clampCurrentValue = clamp;
            spring?.SetClampCurrentValue(clamp);
            return this;
        }

        /// <summary>
        /// Sets whether to clamp the target value.
        /// </summary>
        /// <param name="clamp">Whether to clamp the target value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public FloatSpringComponent SetClampTarget(bool clamp)
        {
            clampTarget = clamp;
            spring?.SetClampTarget(clamp);
            return this;
        }

        /// <summary>
        /// Sets whether to stop the spring when clamping occurs.
        /// </summary>
        /// <param name="stop">Whether to stop on clamp.</param>
        /// <returns>This component instance for method chaining.</returns>
        public FloatSpringComponent StopSpringOnClamp(bool stop)
        {
            stopOnClamp = stop;
            spring?.SetStopOnClamp(stop);
            return this;
        }

        public override bool IsValidSpringComponent()
        {
            // FloatSpringComponent has no additional validation requirements
            return true;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            // Set reasonable defaults
            initialValue = 0f;
            targetValue = 0f;
            useCommonForceAndDrag = true;
            commonForce = 150f;
            commonDrag = 10f;
            enableClamping = false;
            minValue = 0f;
            maxValue = 100f;
            clampCurrentValue = true;
            clampTarget = true;
            stopOnClamp = false;
        }

        private void OnValidate()
        {
            // Ensure min/max values are in correct order
            if (minValue > maxValue)
            {
                float temp = minValue;
                minValue = maxValue;
                maxValue = temp;
            }

            // Apply changes to spring if it exists
            if (spring != null && Application.isPlaying)
            {
                ConfigureSpring();
            }
        }

        internal override Spring[] GetSpringsArray()
        {
            if (spring != null)
            {
                return new Spring[] { spring };
            }
            return new Spring[0];
        }
#endif
    }
}
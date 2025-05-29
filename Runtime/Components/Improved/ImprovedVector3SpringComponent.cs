using UnityEngine;
using USpring.Core;
using USpring.Core.Interfaces;

namespace USpring.Components.Improved
{
    /// <summary>
    /// Improved Vector3 spring component using the new generic base class.
    /// Demonstrates clean, type-safe Vector3 spring management with minimal boilerplate.
    /// </summary>
    [AddComponentMenu("Ludo/USpring/Components/Improved/Vector3 Spring")]
    public class ImprovedVector3SpringComponent : SpringComponent
    {
        [Header("Initial Values")]
        [SerializeField] private Vector3 initialValue = Vector3.zero;
        [SerializeField] private Vector3 targetValue = Vector3.zero;

        [Header("Force and Drag Configuration")]
        [SerializeField] private bool useCommonForceAndDrag = true;
        [SerializeField] private float commonForce = 150f;
        [SerializeField] private float commonDrag = 10f;

        [Header("Per-Axis Configuration")]
        [SerializeField] private Vector3 forcePerAxis = new Vector3(150f, 150f, 150f);
        [SerializeField] private Vector3 dragPerAxis = new Vector3(10f, 10f, 10f);

        [Header("Clamping")]
        [SerializeField] private bool enableClamping = false;
        [SerializeField] private Vector3 minValues = Vector3.zero;
        [SerializeField] private Vector3 maxValues = Vector3.one * 100f;
        [SerializeField] private bool clampCurrentValue = true;
        [SerializeField] private bool clampTarget = true;
        [SerializeField] private Vector3 stopOnClamp = Vector3.zero; // Per-axis stop on clamp

        [SerializeField] private SpringVector3 spring = new SpringVector3();

        /// <summary>
        /// Gets the main spring managed by this component.
        /// </summary>
        public SpringVector3 Spring => spring;

        /// <summary>
        /// Gets the spring events for the main spring.
        /// </summary>
        public SpringEvents Events => spring.springEvents;

        private void Awake()
        {
            // Initialize spring if it's null
            if (spring == null)
            {
                spring = new SpringVector3();
            }
        }

        public override void Initialize()
        {
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
            else
            {
                spring.SetCommonForceAndDrag(false);
                spring.SetForce(forcePerAxis);
                spring.SetDrag(dragPerAxis);
            }

            // Configure clamping
            spring.SetClampingEnabled(enableClamping);
            if (enableClamping)
            {
                spring.SetMinValues(minValues);
                spring.SetMaxValues(maxValues);
                spring.SetClampCurrentValues(clampCurrentValue, clampCurrentValue, clampCurrentValue);
                spring.SetClampTarget(clampTarget, clampTarget, clampTarget);
                spring.StopSpringOnClamp(stopOnClamp.x > 0, stopOnClamp.y > 0, stopOnClamp.z > 0);
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
        public Vector3 GetTarget() => spring?.GetTarget() ?? targetValue;

        /// <summary>
        /// Sets the target value that the spring will move towards.
        /// </summary>
        /// <param name="target">The new target value.</param>
        public virtual void SetTarget(Vector3 target)
        {
            targetValue = target;
            spring?.SetTarget(target);
        }

        /// <summary>
        /// Gets the current animated value of the spring.
        /// </summary>
        public Vector3 GetCurrentValue() => spring?.GetCurrentValue() ?? initialValue;

        /// <summary>
        /// Sets the current value of the spring.
        /// </summary>
        /// <param name="currentValue">The new current value.</param>
        public virtual void SetCurrentValue(Vector3 currentValue)
        {
            initialValue = currentValue;
            spring?.SetCurrentValue(currentValue);
        }

        /// <summary>
        /// Gets the current velocity of the spring.
        /// </summary>
        public Vector3 GetVelocity() => spring?.GetVelocity() ?? Vector3.zero;

        /// <summary>
        /// Sets the velocity of the spring.
        /// </summary>
        /// <param name="velocity">The new velocity value.</param>
        public virtual void SetVelocity(Vector3 velocity) => spring?.SetVelocity(velocity);

        /// <summary>
        /// Adds velocity to the current spring velocity.
        /// </summary>
        /// <param name="velocity">The velocity to add.</param>
        public virtual void AddVelocity(Vector3 velocity) => spring?.AddVelocity(velocity);

        /// <summary>
        /// Immediately sets the spring to its target value and stops all motion.
        /// </summary>
        public virtual void ReachEquilibrium() => spring?.ReachEquilibrium();

        /// <summary>
        /// Sets the common force for all axes.
        /// </summary>
        /// <param name="force">The force value.</param>
        public void SetCommonForce(float force)
        {
            commonForce = force;
            if (spring != null && useCommonForceAndDrag)
            {
                spring.SetCommonForceAndDrag(true);
                spring.SetCommonForce(force);
            }
        }

        /// <summary>
        /// Sets the common drag for all axes.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        public void SetCommonDrag(float drag)
        {
            commonDrag = drag;
            if (spring != null && useCommonForceAndDrag)
            {
                spring.SetCommonForceAndDrag(true);
                spring.SetCommonDrag(drag);
            }
        }

        /// <summary>
        /// Sets force per axis.
        /// </summary>
        /// <param name="force">The force vector.</param>
        public void SetForcePerAxis(Vector3 force)
        {
            forcePerAxis = force;
            if (spring != null && !useCommonForceAndDrag)
            {
                spring.SetForce(force);
            }
        }

        /// <summary>
        /// Sets drag per axis.
        /// </summary>
        /// <param name="drag">The drag vector.</param>
        public void SetDragPerAxis(Vector3 drag)
        {
            dragPerAxis = drag;
            if (spring != null && !useCommonForceAndDrag)
            {
                spring.SetDrag(drag);
            }
        }

        /// <summary>
        /// Gets the current force values.
        /// </summary>
        /// <returns>The force vector.</returns>
        public Vector3 GetForce()
        {
            return spring?.GetForce() ?? (useCommonForceAndDrag ? Vector3.one * commonForce : forcePerAxis);
        }

        /// <summary>
        /// Gets the current drag values.
        /// </summary>
        /// <returns>The drag vector.</returns>
        public Vector3 GetDrag()
        {
            return spring?.GetDrag() ?? (useCommonForceAndDrag ? Vector3.one * commonDrag : dragPerAxis);
        }

        /// <summary>
        /// Sets the minimum values for clamping.
        /// </summary>
        /// <param name="min">The minimum values.</param>
        public void SetMinValues(Vector3 min)
        {
            minValues = min;
            spring?.SetMinValues(min);
        }

        /// <summary>
        /// Sets the maximum values for clamping.
        /// </summary>
        /// <param name="max">The maximum values.</param>
        public void SetMaxValues(Vector3 max)
        {
            maxValues = max;
            spring?.SetMaxValues(max);
        }

        /// <summary>
        /// Sets clamping for current values per axis.
        /// </summary>
        /// <param name="clampX">Clamp X axis.</param>
        /// <param name="clampY">Clamp Y axis.</param>
        /// <param name="clampZ">Clamp Z axis.</param>
        public void SetClampCurrentValues(bool clampX, bool clampY, bool clampZ)
        {
            spring?.SetClampCurrentValues(clampX, clampY, clampZ);
        }

        /// <summary>
        /// Sets clamping for target values per axis.
        /// </summary>
        /// <param name="clampX">Clamp X axis.</param>
        /// <param name="clampY">Clamp Y axis.</param>
        /// <param name="clampZ">Clamp Z axis.</param>
        public void SetClampTarget(bool clampX, bool clampY, bool clampZ)
        {
            spring?.SetClampTarget(clampX, clampY, clampZ);
        }

        /// <summary>
        /// Sets stop on clamp per axis.
        /// </summary>
        /// <param name="stopX">Stop on clamp X axis.</param>
        /// <param name="stopY">Stop on clamp Y axis.</param>
        /// <param name="stopZ">Stop on clamp Z axis.</param>
        public void StopSpringOnClamp(bool stopX, bool stopY, bool stopZ)
        {
            stopOnClamp = new Vector3(stopX ? 1 : 0, stopY ? 1 : 0, stopZ ? 1 : 0);
            spring?.StopSpringOnClamp(stopX, stopY, stopZ);
        }

        public override bool IsValidSpringComponent()
        {
            // Vector3SpringComponent has no additional validation requirements
            return true;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            // Set reasonable defaults
            initialValue = Vector3.zero;
            targetValue = Vector3.zero;
            useCommonForceAndDrag = true;
            commonForce = 150f;
            commonDrag = 10f;
            forcePerAxis = new Vector3(150f, 150f, 150f);
            dragPerAxis = new Vector3(10f, 10f, 10f);
            enableClamping = false;
            minValues = Vector3.zero;
            maxValues = Vector3.one * 100f;
            clampCurrentValue = true;
            clampTarget = true;
            stopOnClamp = Vector3.zero;
        }

        private void OnValidate()
        {
            // Ensure min/max values are in correct order
            minValues = Vector3.Min(minValues, maxValues);
            maxValues = Vector3.Max(minValues, maxValues);

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

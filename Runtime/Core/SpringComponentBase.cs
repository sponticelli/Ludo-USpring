using UnityEngine;
using System.Collections.Generic;
using Ludo.UnityInject;
using USpring.Core.Interfaces;

namespace USpring.Core
{
    /// <summary>
    /// Generic base class for spring components that manage a specific type of spring value.
    /// Reduces code duplication by providing common functionality for typed spring components.
    /// </summary>
    /// <typeparam name="TSpring">The type of spring this component manages</typeparam>
    /// <typeparam name="TValue">The type of value the spring animates</typeparam>
    public abstract class SpringComponentBase<TSpring, TValue> : SpringComponent 
        where TSpring : class, ISpringValue<TValue>
    {
        [SerializeField] protected TSpring spring;

        /// <summary>
        /// Gets the main spring managed by this component.
        /// </summary>
        public TSpring Spring => spring;

        /// <summary>
        /// Gets the spring events for the main spring.
        /// </summary>
        public SpringEvents Events => spring.Events;

        /// <summary>
        /// Gets the current target value of the spring.
        /// </summary>
        public TValue GetTarget() => spring.GetTarget();

        /// <summary>
        /// Sets the target value that the spring will move towards.
        /// </summary>
        /// <param name="target">The new target value.</param>
        public virtual void SetTarget(TValue target) => spring.SetTarget(target);

        /// <summary>
        /// Gets the current animated value of the spring.
        /// </summary>
        public TValue GetCurrentValue() => spring.GetCurrentValue();

        /// <summary>
        /// Sets the current value of the spring.
        /// </summary>
        /// <param name="currentValue">The new current value.</param>
        public virtual void SetCurrentValue(TValue currentValue) => spring.SetCurrentValue(currentValue);

        /// <summary>
        /// Gets the current velocity of the spring.
        /// </summary>
        public TValue GetVelocity() => spring.GetVelocity();

        /// <summary>
        /// Sets the velocity of the spring.
        /// </summary>
        /// <param name="velocity">The new velocity value.</param>
        public virtual void SetVelocity(TValue velocity) => spring.SetVelocity(velocity);

        /// <summary>
        /// Adds velocity to the current spring velocity.
        /// </summary>
        /// <param name="velocity">The velocity to add.</param>
        public virtual void AddVelocity(TValue velocity) => spring.AddVelocity(velocity);

        /// <summary>
        /// Immediately sets the spring to its target value and stops all motion.
        /// </summary>
        public virtual void ReachEquilibrium() => spring.ReachEquilibrium();

        protected override void RegisterSprings()
        {
            if (spring != null)
            {
                RegisterSpring(spring as Spring);
            }
        }

        protected override void SetCurrentValueByDefault()
        {
            var defaultValue = GetDefaultValue();
            SetCurrentValue(defaultValue);
        }

        protected override void SetTargetByDefault()
        {
            var defaultValue = GetDefaultValue();
            SetTarget(defaultValue);
        }

        /// <summary>
        /// Gets the default value for this spring type.
        /// Override this method to provide component-specific default values.
        /// </summary>
        /// <returns>The default value for this spring type.</returns>
        protected abstract TValue GetDefaultValue();

        /// <summary>
        /// Validates that the spring component is properly configured.
        /// Override this method to provide component-specific validation logic.
        /// </summary>
        /// <returns>True if the component is valid, false otherwise.</returns>
        public override bool IsValidSpringComponent()
        {
            if (spring == null)
            {
                AddErrorReason($"Spring is null in {GetType().Name}");
                return false;
            }
            return ValidateComponent();
        }

        /// <summary>
        /// Override this method to provide additional component-specific validation.
        /// </summary>
        /// <returns>True if the component is valid, false otherwise.</returns>
        protected virtual bool ValidateComponent()
        {
            return true;
        }

#if UNITY_EDITOR
        internal override Spring[] GetSpringsArray()
        {
            if (spring != null)
            {
                return new Spring[] { spring as Spring };
            }
            return new Spring[0];
        }
#endif
    }

    /// <summary>
    /// Specialized base class for spring components that automatically update a target object.
    /// Provides additional functionality for components that need to apply spring values to Unity objects.
    /// </summary>
    /// <typeparam name="TSpring">The type of spring this component manages</typeparam>
    /// <typeparam name="TValue">The type of value the spring animates</typeparam>
    /// <typeparam name="TTarget">The type of target object to update</typeparam>
    public abstract class AutoUpdateSpringComponentBase<TSpring, TValue, TTarget> : SpringComponentBase<TSpring, TValue>
        where TSpring : class, ISpringValue<TValue>
        where TTarget : class
    {
        [SerializeField] protected TTarget targetObject;

        /// <summary>
        /// Gets or sets the target object that will be automatically updated.
        /// </summary>
        public TTarget TargetObject
        {
            get => targetObject;
            set => targetObject = value;
        }

        protected virtual void Update()
        {
            if (!initialized || targetObject == null)
            {
                return;
            }

            UpdateTargetObject();
        }

        /// <summary>
        /// Updates the target object with the current spring value.
        /// Override this method to define how the spring value is applied to the target object.
        /// </summary>
        protected abstract void UpdateTargetObject();

        /// <summary>
        /// Gets the current value from the target object.
        /// Override this method to define how to read the current value from the target object.
        /// </summary>
        /// <returns>The current value from the target object.</returns>
        protected abstract TValue GetValueFromTarget();

        protected override TValue GetDefaultValue()
        {
            if (targetObject != null)
            {
                return GetValueFromTarget();
            }
            return GetFallbackDefaultValue();
        }

        /// <summary>
        /// Gets a fallback default value when no target object is available.
        /// Override this method to provide a sensible default value.
        /// </summary>
        /// <returns>The fallback default value.</returns>
        protected abstract TValue GetFallbackDefaultValue();

        protected override bool ValidateComponent()
        {
            if (targetObject == null)
            {
                AddErrorReason($"Target object is null in {GetType().Name}");
                return false;
            }
            return base.ValidateComponent();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            if (targetObject == null)
            {
                TryAutoAssignTarget();
            }
        }

        /// <summary>
        /// Attempts to automatically assign a target object.
        /// Override this method to provide component-specific auto-assignment logic.
        /// </summary>
        protected virtual void TryAutoAssignTarget()
        {
            targetObject = GetComponent<TTarget>();
        }
#endif
    }
}

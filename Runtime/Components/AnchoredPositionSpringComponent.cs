using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
    [AddComponentMenu("Ludo/USpring/Components/Anchored Position Spring")]
    public class AnchoredPositionSpringComponent : SpringComponent
    {
        [SerializeField] private SpringVector2 anchoredPositionSpring = new SpringVector2();

        [SerializeField] private bool useTransformAsTarget = false;
        [SerializeField] private RectTransform followRectTransform;
        [SerializeField] private RectTransform targetRectTransform;
        [SerializeField] private Vector2 anchoredPositionTarget;


        public SpringEvents Events => anchoredPositionSpring.springEvents;
        public Vector2 GetTarget() => anchoredPositionSpring.GetTarget();

        /// <summary>
        /// Sets the target anchored position that the spring will move towards.
        /// </summary>
        /// <param name="target">The new target anchored position.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AnchoredPositionSpringComponent SetTarget(Vector2 target)
        {
            anchoredPositionSpring.SetTarget(target);
            SetTargetInternal(target);
            return this;
        }

        /// <summary>
        /// Sets the target anchored position using a single float for both axes.
        /// </summary>
        /// <param name="target">The target value for both axes.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AnchoredPositionSpringComponent SetTarget(float target) => SetTarget(Vector2.one * target);

        public Vector2 GetCurrentValue() => anchoredPositionSpring.GetCurrentValue();

        /// <summary>
        /// Sets the current anchored position of the spring.
        /// </summary>
        /// <param name="currentValues">The new current anchored position.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AnchoredPositionSpringComponent SetCurrentValue(Vector2 currentValues)
        {
            anchoredPositionSpring.SetCurrentValue(currentValues);
            SetCurrentValueInternal(currentValues);
            return this;
        }

        /// <summary>
        /// Sets the current anchored position using a single float for both axes.
        /// </summary>
        /// <param name="currentValues">The current value for both axes.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AnchoredPositionSpringComponent SetCurrentValue(float currentValues) => SetCurrentValue(Vector2.one * currentValues);

        public Vector2 GetVelocity() => anchoredPositionSpring.GetVelocity();

        /// <summary>
        /// Sets the velocity of the anchored position spring.
        /// </summary>
        /// <param name="velocity">The new velocity value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AnchoredPositionSpringComponent SetVelocity(Vector2 velocity)
        {
            anchoredPositionSpring.SetVelocity(velocity);
            return this;
        }

        /// <summary>
        /// Sets the velocity using a single float for both axes.
        /// </summary>
        /// <param name="velocity">The velocity for both axes.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AnchoredPositionSpringComponent SetVelocity(float velocity) => SetVelocity(Vector2.one * velocity);

        /// <summary>
        /// Adds velocity to the current anchored position spring velocity.
        /// </summary>
        /// <param name="velocityToAdd">The velocity to add.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AnchoredPositionSpringComponent AddVelocity(Vector2 velocityToAdd)
        {
            anchoredPositionSpring.AddVelocity(velocityToAdd);
            return this;
        }

        public override void ReachEquilibrium()
        {
            base.ReachEquilibrium();
            ReachEquilibriumInternal();
        }

        public Vector2 GetForce() => anchoredPositionSpring.GetForce();
        public void SetForce(Vector2 force) => anchoredPositionSpring.SetForce(force);
        public void SetForce(float force) => SetForce(Vector2.one * force);
        public Vector2 GetDrag() => anchoredPositionSpring.GetDrag();
        public void SetDrag(Vector2 drag) => anchoredPositionSpring.SetDrag(drag);
        public void SetDrag(float drag) => SetDrag(Vector2.one * drag);
        public float GetCommonForce() => anchoredPositionSpring.GetCommonForce();
        public float GetCommonDrag() => anchoredPositionSpring.GetCommonDrag();

        public void SetCommonForce(float force)
        {
            anchoredPositionSpring.SetCommonForceAndDrag(true);
            anchoredPositionSpring.SetCommonForce(force);
        }

        public void SetCommonDrag(float drag)
        {
            anchoredPositionSpring.SetCommonForceAndDrag(true);
            anchoredPositionSpring.SetCommonDrag(drag);
        }

        public void SetCommonForceAndDrag(float force, float drag)
        {
            SetCommonForce(force);
            SetCommonDrag(drag);
        }

        public void SetMinValues(Vector2 minValue) => anchoredPositionSpring.SetMinValue(minValue);
        public void SetMinValues(float minValue) => SetMinValues(Vector2.one * minValue);
        public void SetMaxValues(Vector2 maxValue) => anchoredPositionSpring.SetMaxValues(maxValue);
        public void SetMaxValues(float maxValue) => SetMaxValues(Vector2.one * maxValue);

        public void SetClampCurrentValues(bool clampTargetX, bool clampTargetY) =>
            anchoredPositionSpring.SetClampCurrentValues(clampTargetX, clampTargetY);

        public void SetClampTarget(bool clampTargetX, bool clampTargetY) =>
            anchoredPositionSpring.SetClampTarget(clampTargetX, clampTargetY);

        public void StopSpringOnClamp(bool stopX, bool stopY) => anchoredPositionSpring.StopSpringOnClamp(stopX, stopY);


        protected override void RegisterSprings()
        {
            RegisterSpring(anchoredPositionSpring);
        }

        protected override void SetCurrentValueByDefault()
        {
            SetCurrentValue(followRectTransform.anchoredPosition);
        }

        protected override void SetTargetByDefault()
        {
            SetTarget(followRectTransform.anchoredPosition);
        }

        private void UpdateSpringSetTarget()
        {
            anchoredPositionSpring.SetTarget(useTransformAsTarget
                ? targetRectTransform.anchoredPosition
                : anchoredPositionTarget);
        }

        private void SetTargetInternal(Vector2 target)
        {
            if (useTransformAsTarget)
            {
                targetRectTransform.anchoredPosition = target;
            }
            else
            {
                anchoredPositionTarget = target;
            }
        }

        private void SetCurrentValueInternal(Vector2 currentValue)
        {
            UpdateFollowerTransform();
        }

        private void ReachEquilibriumInternal()
        {
            UpdateFollowerTransform();
        }

        public void Update()
        {
            if (!initialized)
            {
                return;
            }

            RefreshAnchoredPositionTarget();
            UpdateSpringSetTarget();
            UpdateFollowerTransform();
        }

        private void RefreshAnchoredPositionTarget()
        {
            if (useTransformAsTarget)
            {
                anchoredPositionTarget = targetRectTransform.anchoredPosition;
            }
        }

        private void UpdateFollowerTransform()
        {
            followRectTransform.anchoredPosition = anchoredPositionSpring.GetCurrentValue();
        }

        public override bool IsValidSpringComponent()
        {
            bool res = true;

            if (useTransformAsTarget && targetRectTransform == null)
            {
                AddErrorReason($"{gameObject.name} useTransformAsTarget is enabled but targetRectTransform is null");
                res = false;
            }

            if (followRectTransform == null)
            {
                AddErrorReason($"{gameObject.name} followRectTransform cannot be null");
                res = false;
            }

            return res;
        }

        private void OnValidate()
        {
            if (followRectTransform == null)
            {
                followRectTransform = GetComponent<RectTransform>();
            }
        }

#if UNITY_EDITOR
        internal override Spring[] GetSpringsArray()
        {
            Spring[] res = new Spring[]
            {
                anchoredPositionSpring
            };

            return res;
        }
#endif
    }
}
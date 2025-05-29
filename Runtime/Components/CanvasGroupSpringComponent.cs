using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
    [AddComponentMenu("Ludo/USpring/Components/Canvas Group Spring")]
    public partial class CanvasGroupSpringComponent : SpringComponent
    {
        [SerializeField] private SpringFloat alphaSpring = new SpringFloat();

        [SerializeField] private CanvasGroup autoUpdatedCanvasGroup;

        [Header("Blocks Raycasts & Interactable")]
        [SerializeField] private bool updateBlocksRaycasts = true;
        [SerializeField] private bool updateInteractable = true;
        [SerializeField] private float blocksRaycastsThreshold = 0.1f;
        [SerializeField] private float interactableThreshold = 0.1f;

        public SpringEvents Events => alphaSpring.springEvents;
        public float GetTarget() => alphaSpring.GetTarget();

        /// <summary>
        /// Sets the target alpha that the spring will move towards.
        /// </summary>
        /// <param name="target">The new target alpha value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetTarget(float target)
        {
            alphaSpring.SetTarget(target);
            return this;
        }

        public float GetCurrentValue() => alphaSpring.GetCurrentValue();

        /// <summary>
        /// Sets the current alpha of the spring.
        /// </summary>
        /// <param name="currentValues">The new current alpha value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetCurrentValue(float currentValues)
        {
            alphaSpring.SetCurrentValue(currentValues);
            SetCurrentValueInternal(currentValues);
            return this;
        }

        public float GetVelocity() => alphaSpring.GetVelocity();

        /// <summary>
        /// Sets the velocity of the alpha spring.
        /// </summary>
        /// <param name="velocity">The new velocity value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetVelocity(float velocity)
        {
            alphaSpring.SetVelocity(velocity);
            return this;
        }

        /// <summary>
        /// Adds velocity to the current alpha spring velocity.
        /// </summary>
        /// <param name="velocityToAdd">The velocity to add.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent AddVelocity(float velocityToAdd)
        {
            alphaSpring.AddVelocity(velocityToAdd);
            return this;
        }

        public override void ReachEquilibrium()
        {
            base.ReachEquilibrium();
            ReachEquilibriumInternal();
        }

        public float GetForce() => alphaSpring.GetForce();

        /// <summary>
        /// Sets the force value for the alpha spring.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetForce(float force)
        {
            alphaSpring.SetForce(force);
            return this;
        }

        public float GetDrag() => alphaSpring.GetDrag();

        /// <summary>
        /// Sets the drag value for the alpha spring.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetDrag(float drag)
        {
            alphaSpring.SetDrag(drag);
            return this;
        }

        /// <summary>
        /// Sets the minimum value for alpha clamping.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetMinValues(float minValue)
        {
            alphaSpring.SetMinValue(minValue);
            return this;
        }

        /// <summary>
        /// Sets the maximum value for alpha clamping.
        /// </summary>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetMaxValues(float maxValue)
        {
            alphaSpring.SetMaxValue(maxValue);
            return this;
        }

        /// <summary>
        /// Sets whether to clamp the current alpha value.
        /// </summary>
        /// <param name="clamp">Whether to clamp the current value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetClampCurrentValues(bool clamp)
        {
            alphaSpring.SetClampCurrentValue(clamp);
            return this;
        }

        /// <summary>
        /// Sets whether to clamp the target alpha value.
        /// </summary>
        /// <param name="clamp">Whether to clamp the target value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetClampTarget(bool clamp)
        {
            alphaSpring.SetClampTarget(clamp);
            return this;
        }

        /// <summary>
        /// Sets whether to stop the alpha spring when clamping occurs.
        /// </summary>
        /// <param name="stop">Whether to stop on clamp.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent StopSpringOnClamp(bool stop)
        {
            alphaSpring.SetStopOnClamp(stop);
            return this;
        }

        public float GetCommonForce() => alphaSpring.GetCommonForce();
        public float GetCommonDrag() => alphaSpring.GetCommonDrag();

        /// <summary>
        /// Sets the common force (stiffness) value for alpha.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetCommonForce(float force)
        {
            alphaSpring.SetCommonForceAndDrag(true);
            alphaSpring.SetCommonForce(force);
            return this;
        }

        /// <summary>
        /// Sets the common drag (damping) value for alpha.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetCommonDrag(float drag)
        {
            alphaSpring.SetCommonForceAndDrag(true);
            alphaSpring.SetCommonDrag(drag);
            return this;
        }

        /// <summary>
        /// Sets both common force and drag values for alpha.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <param name="drag">The drag value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public CanvasGroupSpringComponent SetCommonForceAndDrag(float force, float drag)
        {
            SetCommonForce(force);
            SetCommonDrag(drag);
            return this;
        }

        protected override void RegisterSprings()
        {
            RegisterSpring(alphaSpring);
        }

        protected override void SetCurrentValueByDefault()
        {
            alphaSpring.SetCurrentValue(autoUpdatedCanvasGroup.alpha);
        }

        protected override void SetTargetByDefault()
        {
            alphaSpring.SetTarget(autoUpdatedCanvasGroup.alpha);
        }

        public void Update()
        {
            if (!initialized) { return; }

            UpdateCanvasGroup();
        }

        private void UpdateCanvasGroup()
        {
            float alpha = alphaSpring.GetCurrentValue();
            autoUpdatedCanvasGroup.alpha = alpha;

            // Update blocksRaycasts and interactable based on alpha thresholds
            if (updateBlocksRaycasts)
            {
                autoUpdatedCanvasGroup.blocksRaycasts = alpha >= blocksRaycastsThreshold;
            }

            if (updateInteractable)
            {
                autoUpdatedCanvasGroup.interactable = alpha >= interactableThreshold;
            }
        }

        public override bool IsValidSpringComponent()
        {
            bool res = true;

            if (autoUpdatedCanvasGroup == null)
            {
                AddErrorReason($"{gameObject.name} autoUpdatedCanvasGroup is null.");
                res = false;
            }

            return res;
        }

        private void ReachEquilibriumInternal()
        {
            UpdateCanvasGroup();
        }

        private void SetCurrentValueInternal(float currentValues)
        {
            UpdateCanvasGroup();
        }

        // Additional methods for direct access
        public void SetBlocksRaycasts(bool blocksRaycasts)
        {
            updateBlocksRaycasts = false; // Disable automatic update
            autoUpdatedCanvasGroup.blocksRaycasts = blocksRaycasts;
        }

        public void SetInteractable(bool interactable)
        {
            updateInteractable = false; // Disable automatic update
            autoUpdatedCanvasGroup.interactable = interactable;
        }

        public void SetBlocksRaycastsThreshold(float threshold)
        {
            blocksRaycastsThreshold = Mathf.Clamp01(threshold);
        }

        public void SetInteractableThreshold(float threshold)
        {
            interactableThreshold = Mathf.Clamp01(threshold);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            if (autoUpdatedCanvasGroup == null)
            {
                autoUpdatedCanvasGroup = GetComponent<CanvasGroup>();
            }

            // Set default clamping for alpha (0-1)
            alphaSpring.SetClampCurrentValue(true);
            alphaSpring.SetClampTarget(true);
            alphaSpring.SetMinValue(0f);
            alphaSpring.SetMaxValue(1f);

            // Default spring values suitable for UI
            alphaSpring.SetCommonForceAndDrag(true);
            alphaSpring.SetCommonForce(80f);
            alphaSpring.SetCommonDrag(10f);
        }

        internal override Spring[] GetSpringsArray()
        {
            Spring[] res = new Spring[]
            {
                alphaSpring
            };

            return res;
        }
#endif
    }
}
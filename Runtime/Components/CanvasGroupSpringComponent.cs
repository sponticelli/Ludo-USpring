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
        public void SetTarget(float target) => alphaSpring.SetTarget(target);
        public float GetCurrentValue() => alphaSpring.GetCurrentValue();
        public void SetCurrentValue(float currentValues)
        {
            alphaSpring.SetCurrentValue(currentValues);
            SetCurrentValueInternal(currentValues);
        }
        public float GetVelocity() => alphaSpring.GetVelocity();
        public void SetVelocity(float velocity) => alphaSpring.SetVelocity(velocity);
        public void AddVelocity(float velocityToAdd) => alphaSpring.AddVelocity(velocityToAdd);
        public override void ReachEquilibrium()
        {
            base.ReachEquilibrium();
            ReachEquilibriumInternal();
        }
        public float GetForce() => alphaSpring.GetForce();
        public void SetForce(float force) => alphaSpring.SetForce(force);
        public float GetDrag() => alphaSpring.GetDrag();
        public void SetDrag(float drag) => alphaSpring.SetDrag(drag);
        public void SetMinValues(float minValue) => alphaSpring.SetMinValue(minValue);
        public void SetMaxValues(float maxValue) => alphaSpring.SetMaxValue(maxValue);
        public void SetClampCurrentValues(bool clamp) => alphaSpring.SetClampCurrentValue(clamp);
        public void SetClampTarget(bool clamp) => alphaSpring.SetClampTarget(clamp);
        public void StopSpringOnClamp(bool stop) => alphaSpring.SetStopOnClamp(stop);
        public float GetCommonForce() => alphaSpring.GetCommonForce();
        public float GetCommonDrag() => alphaSpring.GetCommonDrag();
        public void SetCommonForce(float force)
        {
            alphaSpring.SetCommonForceAndDrag(true);
            alphaSpring.SetCommonForce(force);
        }
        public void SetCommonDrag(float drag)
        {
            alphaSpring.SetCommonForceAndDrag(true);
            alphaSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDrag(float force, float drag)
        {
            SetCommonForce(force);
            SetCommonDrag(drag);
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
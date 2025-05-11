using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
    [AddComponentMenu("Ludo/USpring/Components/Sprite Renderer Spring")]
    public partial class SpriteRendererSpringComponent : SpringComponent
    {
        [SerializeField] private SpringColor colorSpring = new SpringColor();
        [SerializeField] private SpringVector2 sizeSpring = new SpringVector2();
        
        [SerializeField] private SpriteRenderer autoUpdatedSpriteRenderer;
        [SerializeField] private bool updateSize = true;
        
        #region Color Spring Methods
        public SpringEvents ColorEvents => colorSpring.springEvents;
        public Color GetTargetColor() => colorSpring.GetTargetColor();
        public void SetTargetColor(Color target) => colorSpring.SetTarget(target);
        public Color GetCurrentValueColor() => colorSpring.GetCurrentColor();
        public void SetCurrentValueColor(Color currentValues)
        {
            colorSpring.SetCurrentValue(currentValues);
            UpdateSpriteRenderer();
        }
        public Vector4 GetVelocityColor() => colorSpring.GetVelocity();
        public void SetVelocityColor(Vector4 velocity) => colorSpring.SetVelocity(velocity);
        public void AddVelocityColor(Vector4 velocityToAdd) => colorSpring.AddVelocity(velocityToAdd);
        public void ReachEquilibriumColor() => colorSpring.ReachEquilibrium();
        public Vector4 GetForceColor() => colorSpring.GetForce();
        public void SetForceColor(Vector4 force) => colorSpring.SetForce(force);
        public void SetForceColor(float force) => colorSpring.SetForce(force);
        public Vector4 GetDragColor() => colorSpring.GetDrag();
        public void SetDragColor(Vector4 drag) => colorSpring.SetDrag(drag);
        public void SetDragColor(float drag) => colorSpring.SetDrag(drag);
        public float GetCommonForceColor() => colorSpring.GetCommonForce();
        public float GetCommonDragColor() => colorSpring.GetCommonDrag();
        public void SetCommonForceColor(float force)
        {
            colorSpring.SetCommonForceAndDrag(true);
            colorSpring.SetCommonForce(force);
        }
        public void SetCommonDragColor(float drag)
        {
            colorSpring.SetCommonForceAndDrag(true);
            colorSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragColor(float force, float drag)
        {
            SetCommonForceColor(force);
            SetCommonDragColor(drag);
        }
        public void SetMinValuesColor(Vector4 minValue) => colorSpring.SetMinValues(minValue);
        public void SetMaxValuesColor(Vector4 maxValue) => colorSpring.SetMaxValues(maxValue);
        public void SetClampCurrentValuesColor(bool clampR, bool clampG, bool clampB, bool clampA) => 
            colorSpring.SetClampCurrentValues(clampR, clampG, clampB, clampA);
        public void SetClampTargetColor(bool clampR, bool clampG, bool clampB, bool clampA) => 
            colorSpring.SetClampTarget(clampR, clampG, clampB, clampA);
        public void StopSpringOnClampColor(bool stopR, bool stopG, bool stopB, bool stopA) => 
            colorSpring.StopSpringOnClamp(stopR, stopG, stopB, stopA);
        #endregion
        
        #region Size Spring Methods
        public SpringEvents SizeEvents => sizeSpring.springEvents;
        public Vector2 GetTargetSize() => sizeSpring.GetTarget();
        public void SetTargetSize(Vector2 target) => sizeSpring.SetTarget(target);
        public void SetTargetSize(float target) => sizeSpring.SetTarget(Vector2.one * target);
        public Vector2 GetCurrentValueSize() => sizeSpring.GetCurrentValue();
        public void SetCurrentValueSize(Vector2 currentValues)
        {
            sizeSpring.SetCurrentValue(currentValues);
            UpdateSpriteRenderer();
        }
        public void SetCurrentValueSize(float currentValues) => SetCurrentValueSize(Vector2.one * currentValues);
        public Vector2 GetVelocitySize() => sizeSpring.GetVelocity();
        public void SetVelocitySize(Vector2 velocity) => sizeSpring.SetVelocity(velocity);
        public void SetVelocitySize(float velocity) => sizeSpring.SetVelocity(Vector2.one * velocity);
        public void AddVelocitySize(Vector2 velocityToAdd) => sizeSpring.AddVelocity(velocityToAdd);
        public void ReachEquilibriumSize() => sizeSpring.ReachEquilibrium();
        public Vector2 GetForceSize() => sizeSpring.GetForce();
        public void SetForceSize(Vector2 force) => sizeSpring.SetForce(force);
        public void SetForceSize(float force) => sizeSpring.SetForce(force);
        public Vector2 GetDragSize() => sizeSpring.GetDrag();
        public void SetDragSize(Vector2 drag) => sizeSpring.SetDrag(drag);
        public void SetDragSize(float drag) => sizeSpring.SetDrag(drag);
        public float GetCommonForceSize() => sizeSpring.GetCommonForce();
        public float GetCommonDragSize() => sizeSpring.GetCommonDrag();
        public void SetCommonForceSize(float force)
        {
            sizeSpring.SetCommonForceAndDrag(true);
            sizeSpring.SetCommonForce(force);
        }
        public void SetCommonDragSize(float drag)
        {
            sizeSpring.SetCommonForceAndDrag(true);
            sizeSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragSize(float force, float drag)
        {
            SetCommonForceSize(force);
            SetCommonDragSize(drag);
        }
        public void SetMinValuesSize(Vector2 minValue) => sizeSpring.SetMinValue(minValue);
        public void SetMinValuesSize(float minValue) => sizeSpring.SetMinValues(minValue);
        public void SetMaxValuesSize(Vector2 maxValue) => sizeSpring.SetMaxValues(maxValue);
        public void SetMaxValuesSize(float maxValue) => sizeSpring.SetMaxValues(maxValue);
        public void SetClampCurrentValuesSize(bool clampTargetX, bool clampTargetY) => 
            sizeSpring.SetClampCurrentValues(clampTargetX, clampTargetY);
        public void SetClampTargetSize(bool clampTargetX, bool clampTargetY) => 
            sizeSpring.SetClampTarget(clampTargetX, clampTargetY);
        public void StopSpringOnClampSize(bool stopX, bool stopY) => 
            sizeSpring.StopSpringOnClamp(stopX, stopY);
        #endregion
        
        #region Flip Methods (Non-Spring)
        // Flip methods don't use springs but are included for convenience
        [Header("Flip Settings")]
        [SerializeField] private bool animateFlipX = false;
        [SerializeField] private bool animateFlipY = false;
        [SerializeField] private float flipThreshold = 0.5f;

        private bool targetFlipX = false;
        private bool targetFlipY = false;
        
        public void SetTargetFlipX(bool flip)
        {
            if (animateFlipX)
            {
                targetFlipX = flip;
                // When changing flip target, adjust the X velocity to create a smooth transition
                AddVelocitySizeForFlip(flip, true);
            }
            else
            {
                autoUpdatedSpriteRenderer.flipX = flip;
            }
        }
        
        public void SetTargetFlipY(bool flip)
        {
            if (animateFlipY)
            {
                targetFlipY = flip;
                // When changing flip target, adjust the Y velocity to create a smooth transition
                AddVelocitySizeForFlip(flip, false);
            }
            else
            {
                autoUpdatedSpriteRenderer.flipY = flip;
            }
        }
        
        private void AddVelocitySizeForFlip(bool flip, bool isXAxis)
        {
            // Add a velocity impulse to make the flip animation feel more natural
            Vector2 velocityImpulse = Vector2.zero;
            if (isXAxis)
            {
                velocityImpulse.x = flip ? -4f : 4f;
            }
            else
            {
                velocityImpulse.y = flip ? -4f : 4f;
            }
            
            AddVelocitySize(velocityImpulse);
        }
        #endregion
        
        public override void ReachEquilibrium()
        {
            base.ReachEquilibrium();
            UpdateSpriteRenderer();
        }
        
        protected override void RegisterSprings()
        {
            RegisterSpring(colorSpring);
            RegisterSpring(sizeSpring);
        }

        protected override void SetCurrentValueByDefault()
        {
            SetCurrentValueColor(autoUpdatedSpriteRenderer.color);
            if (updateSize)
            {
                SetCurrentValueSize(transform.localScale);
            }
        }

        protected override void SetTargetByDefault()
        {
            SetTargetColor(autoUpdatedSpriteRenderer.color);
            if (updateSize)
            {
                SetTargetSize(transform.localScale);
            }
            
            // Initialize flip state
            targetFlipX = autoUpdatedSpriteRenderer.flipX;
            targetFlipY = autoUpdatedSpriteRenderer.flipY;
        }

        public void Update()
        {
            if (!initialized) { return; }

            UpdateSpriteRenderer();
        }

        private void UpdateSpriteRenderer()
        {
            // Update color
            autoUpdatedSpriteRenderer.color = colorSpring.GetCurrentColor();
            
            // Update size
            if (updateSize)
            {
                Vector2 currentSize = sizeSpring.GetCurrentValue();
                Vector3 newScale = transform.localScale;
                newScale.x = currentSize.x;
                newScale.y = currentSize.y;
                transform.localScale = newScale;
                
                // Handle flipping based on scale sign
                if (animateFlipX)
                {
                    // Only flip when we cross the threshold to avoid flickering
                    if (Mathf.Abs(currentSize.x) < flipThreshold)
                    {
                        // We're in the middle of a flip animation, do nothing
                    }
                    else
                    {
                        // Scale is clearly positive or negative, set the flip state
                        autoUpdatedSpriteRenderer.flipX = targetFlipX;
                    }
                }
                
                if (animateFlipY)
                {
                    if (Mathf.Abs(currentSize.y) < flipThreshold)
                    {
                        // We're in the middle of a flip animation, do nothing
                    }
                    else
                    {
                        // Scale is clearly positive or negative, set the flip state
                        autoUpdatedSpriteRenderer.flipY = targetFlipY;
                    }
                }
            }
        }

        public override bool IsValidSpringComponent()
        {
            bool res = true;

            if (autoUpdatedSpriteRenderer == null)
            {
                AddErrorReason($"{gameObject.name} autoUpdatedSpriteRenderer is null.");
                res = false;
            }

            return res;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            if (autoUpdatedSpriteRenderer == null)
            {
                autoUpdatedSpriteRenderer = GetComponent<SpriteRenderer>();
            }
            
            // Set up default spring values for color
            colorSpring.SetCommonForceAndDrag(true);
            colorSpring.SetCommonForce(50f);
            colorSpring.SetCommonDrag(10f);
            colorSpring.SetClampCurrentValues(true, true, true, true);
            colorSpring.SetMinValues(new Vector4(0, 0, 0, 0));
            colorSpring.SetMaxValues(new Vector4(1, 1, 1, 1));
            
            // Set up default spring values for size
            sizeSpring.SetCommonForceAndDrag(true);
            sizeSpring.SetCommonForce(80f);
            sizeSpring.SetCommonDrag(8f);
            
            // Set initial values
            if (autoUpdatedSpriteRenderer != null)
            {
                SetCurrentValueByDefault();
                SetTargetByDefault();
            }
        }

        internal override Spring[] GetSpringsArray()
        {
            Spring[] res = new Spring[]
            {
                colorSpring,
                sizeSpring
            };

            return res;
        }
#endif
    }
}
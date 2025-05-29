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

        /// <summary>
        /// Sets the target color that the spring will move towards.
        /// </summary>
        /// <param name="target">The new target color.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetTargetColor(Color target)
        {
            colorSpring.SetTarget(target);
            return this;
        }

        public Color GetCurrentValueColor() => colorSpring.GetCurrentColor();

        /// <summary>
        /// Sets the current color of the spring.
        /// </summary>
        /// <param name="currentValues">The new current color.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetCurrentValueColor(Color currentValues)
        {
            colorSpring.SetCurrentValue(currentValues);
            UpdateSpriteRenderer();
            return this;
        }

        public Vector4 GetVelocityColor() => colorSpring.GetVelocity();

        /// <summary>
        /// Sets the velocity of the color spring.
        /// </summary>
        /// <param name="velocity">The new velocity value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetVelocityColor(Vector4 velocity)
        {
            colorSpring.SetVelocity(velocity);
            return this;
        }

        /// <summary>
        /// Adds velocity to the current color spring velocity.
        /// </summary>
        /// <param name="velocityToAdd">The velocity to add.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent AddVelocityColor(Vector4 velocityToAdd)
        {
            colorSpring.AddVelocity(velocityToAdd);
            return this;
        }

        /// <summary>
        /// Immediately sets the color spring to its target value and stops all motion.
        /// </summary>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent ReachEquilibriumColor()
        {
            colorSpring.ReachEquilibrium();
            return this;
        }
        public Vector4 GetForceColor() => colorSpring.GetForce();

        /// <summary>
        /// Sets the force values for the color spring.
        /// </summary>
        /// <param name="force">The force vector.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetForceColor(Vector4 force)
        {
            colorSpring.SetForce(force);
            return this;
        }

        /// <summary>
        /// Sets the force using a single float for all color channels.
        /// </summary>
        /// <param name="force">The force for all channels.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetForceColor(float force)
        {
            colorSpring.SetForce(force);
            return this;
        }

        public Vector4 GetDragColor() => colorSpring.GetDrag();

        /// <summary>
        /// Sets the drag values for the color spring.
        /// </summary>
        /// <param name="drag">The drag vector.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetDragColor(Vector4 drag)
        {
            colorSpring.SetDrag(drag);
            return this;
        }

        /// <summary>
        /// Sets the drag using a single float for all color channels.
        /// </summary>
        /// <param name="drag">The drag for all channels.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetDragColor(float drag)
        {
            colorSpring.SetDrag(drag);
            return this;
        }

        public float GetCommonForceColor() => colorSpring.GetCommonForce();
        public float GetCommonDragColor() => colorSpring.GetCommonDrag();

        /// <summary>
        /// Sets the common force (stiffness) value for color.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetCommonForceColor(float force)
        {
            colorSpring.SetCommonForceAndDrag(true);
            colorSpring.SetCommonForce(force);
            return this;
        }

        /// <summary>
        /// Sets the common drag (damping) value for color.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetCommonDragColor(float drag)
        {
            colorSpring.SetCommonForceAndDrag(true);
            colorSpring.SetCommonDrag(drag);
            return this;
        }

        /// <summary>
        /// Sets both common force and drag values for color.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <param name="drag">The drag value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetCommonForceAndDragColor(float force, float drag)
        {
            SetCommonForceColor(force);
            SetCommonDragColor(drag);
            return this;
        }

        /// <summary>
        /// Sets the minimum values for color clamping.
        /// </summary>
        /// <param name="minValue">The minimum values.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetMinValuesColor(Vector4 minValue)
        {
            colorSpring.SetMinValues(minValue);
            return this;
        }

        /// <summary>
        /// Sets the maximum values for color clamping.
        /// </summary>
        /// <param name="maxValue">The maximum values.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetMaxValuesColor(Vector4 maxValue)
        {
            colorSpring.SetMaxValues(maxValue);
            return this;
        }

        /// <summary>
        /// Sets clamping for current color values per channel.
        /// </summary>
        /// <param name="clampR">Clamp red channel.</param>
        /// <param name="clampG">Clamp green channel.</param>
        /// <param name="clampB">Clamp blue channel.</param>
        /// <param name="clampA">Clamp alpha channel.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetClampCurrentValuesColor(bool clampR, bool clampG, bool clampB, bool clampA)
        {
            colorSpring.SetClampCurrentValues(clampR, clampG, clampB, clampA);
            return this;
        }

        /// <summary>
        /// Sets clamping for target color values per channel.
        /// </summary>
        /// <param name="clampR">Clamp red channel.</param>
        /// <param name="clampG">Clamp green channel.</param>
        /// <param name="clampB">Clamp blue channel.</param>
        /// <param name="clampA">Clamp alpha channel.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetClampTargetColor(bool clampR, bool clampG, bool clampB, bool clampA)
        {
            colorSpring.SetClampTarget(clampR, clampG, clampB, clampA);
            return this;
        }

        /// <summary>
        /// Sets stop on clamp per color channel.
        /// </summary>
        /// <param name="stopR">Stop on clamp red channel.</param>
        /// <param name="stopG">Stop on clamp green channel.</param>
        /// <param name="stopB">Stop on clamp blue channel.</param>
        /// <param name="stopA">Stop on clamp alpha channel.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent StopSpringOnClampColor(bool stopR, bool stopG, bool stopB, bool stopA)
        {
            colorSpring.StopSpringOnClamp(stopR, stopG, stopB, stopA);
            return this;
        }
        #endregion

        #region Size Spring Methods
        public SpringEvents SizeEvents => sizeSpring.springEvents;
        public Vector2 GetTargetSize() => sizeSpring.GetTarget();

        /// <summary>
        /// Sets the target size that the spring will move towards.
        /// </summary>
        /// <param name="target">The new target size.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetTargetSize(Vector2 target)
        {
            sizeSpring.SetTarget(target);
            return this;
        }

        /// <summary>
        /// Sets the target size using a single float for both axes.
        /// </summary>
        /// <param name="target">The target value for both axes.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetTargetSize(float target) => SetTargetSize(Vector2.one * target);

        public Vector2 GetCurrentValueSize() => sizeSpring.GetCurrentValue();

        /// <summary>
        /// Sets the current size of the spring.
        /// </summary>
        /// <param name="currentValues">The new current size.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetCurrentValueSize(Vector2 currentValues)
        {
            sizeSpring.SetCurrentValue(currentValues);
            UpdateSpriteRenderer();
            return this;
        }

        /// <summary>
        /// Sets the current size using a single float for both axes.
        /// </summary>
        /// <param name="currentValues">The current value for both axes.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetCurrentValueSize(float currentValues) => SetCurrentValueSize(Vector2.one * currentValues);

        public Vector2 GetVelocitySize() => sizeSpring.GetVelocity();

        /// <summary>
        /// Sets the velocity of the size spring.
        /// </summary>
        /// <param name="velocity">The new velocity value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetVelocitySize(Vector2 velocity)
        {
            sizeSpring.SetVelocity(velocity);
            return this;
        }

        /// <summary>
        /// Sets the velocity using a single float for both size axes.
        /// </summary>
        /// <param name="velocity">The velocity for both axes.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent SetVelocitySize(float velocity) => SetVelocitySize(Vector2.one * velocity);

        /// <summary>
        /// Adds velocity to the current size spring velocity.
        /// </summary>
        /// <param name="velocityToAdd">The velocity to add.</param>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent AddVelocitySize(Vector2 velocityToAdd)
        {
            sizeSpring.AddVelocity(velocityToAdd);
            return this;
        }

        /// <summary>
        /// Immediately sets the size spring to its target value and stops all motion.
        /// </summary>
        /// <returns>This component instance for method chaining.</returns>
        public SpriteRendererSpringComponent ReachEquilibriumSize()
        {
            sizeSpring.ReachEquilibrium();
            return this;
        }
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
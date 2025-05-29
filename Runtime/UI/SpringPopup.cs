using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using USpring.Components; // Assuming AnchoredPositionSpringComponent is in this namespace
using USpring.Core;

namespace USpring.UI
{
    /// <summary>
    /// Adds spring-based popup animations to UI elements or standard Transforms.
    /// Uses AnchoredPositionSpringComponent for UI position, TransformSpringComponent otherwise.
    /// Perfect for notifications, tooltips, achievements, etc.
    /// </summary>
    [AddComponentMenu("Ludo/USpring/UI/Spring Popup")]
    [RequireComponent(typeof(TransformSpringComponent))] // Still required for Scale/Rotation
    public class SpringPopup : MonoBehaviour
    {
        // --- Enums remain the same ---
        [System.Serializable]
        public enum PopupState
        {
            Hidden,
            Showing,
            Visible,
            Hiding
        }

        [System.Serializable]
        public enum AnimationType
        {
            Scale,
            Position,
            Rotation,
            Combined
        }

        [System.Serializable]
        public enum PopupPreset
        {
            Custom,
            Bounce,
            Elastic,
            Snappy,
            Wobbly,
            Subtle
        }

        // --- Behavior Settings remain mostly the same ---
        [Header("Behavior")]
        [Tooltip("Current state of the popup")]
        [SerializeField] private PopupState currentState = PopupState.Hidden;

        [Tooltip("Animation type to use")]
#pragma warning disable 0414 // Field assigned but never used - used by editor
        [SerializeField] private AnimationType animationType = AnimationType.Scale;
#pragma warning restore 0414

        [Tooltip("Preset animation styles")]
        [SerializeField] private PopupPreset preset = PopupPreset.Custom;

        [Tooltip("Auto-show when this object is enabled")]
        [SerializeField] private bool showOnEnable = true;

        [Tooltip("Auto-hide when this object is disabled")]
        [SerializeField] private bool hideOnDisable = true;

        [Tooltip("Delay before showing popup (seconds)")]
        [SerializeField] private float showDelay = 0f;

        [Tooltip("Delay before hiding popup (seconds)")]
        [SerializeField] private float hideDelay = 0f;

        [Tooltip("Auto-hide after duration (0 = stay visible)")]
        [SerializeField] private float autoHideDuration = 0f;

        // --- Spring Settings - Split for clarity ---
        [Header("General Spring Settings")]
        [Tooltip("Spring force for show animation (higher = faster)")]
        [SerializeField] private float showForce = 120f;

        [Tooltip("Spring drag for show animation (higher = less bouncy)")]
        [SerializeField] private float showDrag = 9f;

        [Tooltip("Spring force for hide animation (higher = faster)")]
        [SerializeField] private float hideForce = 100f;

        [Tooltip("Spring drag for hide animation (higher = less bouncy)")]
        [SerializeField] private float hideDrag = 12f;

        [Tooltip("Add velocity impulse when showing for extra bounce")]
        [SerializeField] private bool addShowVelocity = true;

        [Tooltip("Add velocity impulse when hiding for extra bounce")]
        [SerializeField] private bool addHideVelocity = false;

        [Tooltip("Velocity impulse multiplier for show animation")]
        [SerializeField] private float showVelocityMultiplier = 1.5f;

        [Tooltip("Velocity impulse multiplier for hide animation")]
        [SerializeField] private float hideVelocityMultiplier = 0.5f;

        // --- Animation Properties remain the same ---
        [Header("Scale Animation")]
        [Tooltip("Enable scale animation")]
        [SerializeField] private bool useScaleAnimation = true;
        [Tooltip("Hidden scale (typically zero or very small)")]
        [SerializeField] private Vector3 hiddenScale = Vector3.zero;
        [Tooltip("Visible scale (typically Vector3.one)")]
        [SerializeField] private Vector3 visibleScale = Vector3.one;
        [Tooltip("Overshoot scale multiplier for bouncy effect")]
        [Range(1f, 1.5f)]
        [SerializeField] private float scaleOvershoot = 1.15f;

        [Header("Position Animation")]
        [Tooltip("Enable position animation")]
        [SerializeField] private bool usePositionAnimation = false;
        [Tooltip("Hidden position offset (Anchored Position for UI, Local Position for Transform)")]
        [SerializeField] private Vector3 hiddenPositionOffset = new Vector3(0, -100, 0); // Keep Vector3 for editor convenience, use x/y for Vector2 spring
        [Tooltip("Visible position offset (Anchored Position for UI, Local Position for Transform)")]
        [SerializeField] private Vector3 visiblePosition = Vector3.zero; // Keep Vector3 for editor convenience

        [Header("Rotation Animation")]
        [Tooltip("Enable rotation animation")]
        [SerializeField] private bool useRotationAnimation = false;
        [Tooltip("Hidden rotation in Euler angles")]
        [SerializeField] private Vector3 hiddenRotation = new Vector3(0, 0, -90);
        [Tooltip("Visible rotation in Euler angles")]
        [SerializeField] private Vector3 visibleRotation = Vector3.zero;
        [Tooltip("Overshoot rotation in degrees")]
        [SerializeField] private float rotationOvershoot = 15f;

        // --- Events remain the same ---
        [Header("Events")]
        [SerializeField] private UnityEvent onShow = new UnityEvent();
        [SerializeField] private UnityEvent onShown = new UnityEvent();
        [SerializeField] private UnityEvent onHide = new UnityEvent();
        [SerializeField] private UnityEvent onHidden = new UnityEvent();

        // --- Component References ---
        private TransformSpringComponent transformSpringComponent; // For Scale, Rotation, and non-UI Position
        private AnchoredPositionSpringComponent anchoredPositionSpringComponent; // For UI Position
        private RectTransform rectTransform;
        private Vector3 startLocalPosition; // For non-UI elements
        private Vector2 startAnchoredPosition; // For UI elements

        // --- Coroutines remain the same ---
        private Coroutine autoHideCoroutine;
        private Coroutine showCoroutine;
        private Coroutine hideCoroutine;

        // --- State Flags ---
        private bool isUiElement = false;

        // --- Public Accessors remain the same ---
        public UnityEvent OnShow => onShow;
        public UnityEvent OnShown => onShown;
        public UnityEvent OnHide => onHide;
        public UnityEvent OnHidden => onHidden;
        public PopupState State => currentState;

        private void Awake()
        {
            InitializeComponents();
            ApplyPreset(); // Apply preset after components are initialized
        }

        private void OnEnable()
        {
            // Ensure components are initialized if Awake hasn't run yet or was skipped
            if (transformSpringComponent == null || (isUiElement && anchoredPositionSpringComponent == null && usePositionAnimation))
            {
                InitializeComponents();
            }

            if (showOnEnable && (currentState == PopupState.Hidden || currentState == PopupState.Hiding))
            {
                Show();
            }
        }

        private void OnDisable()
        {
            if (hideOnDisable && (currentState == PopupState.Visible || currentState == PopupState.Showing))
            {
                // Force immediate hiding when disabled
                StopAllCoroutines();
                SetHiddenState(false); // Set state without animation
            }
        }

        /// <summary>
        /// Initialize required components and cache starting values.
        /// </summary>
        private void InitializeComponents()
        {
            // --- Get TransformSpringComponent (Required) ---
            transformSpringComponent = GetComponent<TransformSpringComponent>();
            if (transformSpringComponent != null && !transformSpringComponent.IsInitialized)
            {
                transformSpringComponent.Initialize();
            }
            else if (transformSpringComponent == null)
            {
                 Debug.LogError($"SpringPopup on {gameObject.name} requires a TransformSpringComponent.", this);
                 enabled = false; // Disable if required component is missing
                 return;
            }

            // --- Check if it's a UI Element ---
            rectTransform = GetComponent<RectTransform>();
            isUiElement = (rectTransform != null);

            // --- Cache Initial Position ---
            if (isUiElement)
            {
                startAnchoredPosition = rectTransform.anchoredPosition;
                // Disable the position part of TransformSpringComponent for UI elements if animating position
                if (usePositionAnimation)
                {
                    transformSpringComponent.PositionSpring.springEnabled = false;
                }
            }
            else
            {
                startLocalPosition = transform.localPosition;
            }

            // --- Get or Add AnchoredPositionSpringComponent for UI Position ---
            if (isUiElement && usePositionAnimation)
            {
                anchoredPositionSpringComponent = GetComponent<AnchoredPositionSpringComponent>();
                if (anchoredPositionSpringComponent == null)
                {
                    Debug.LogWarning($"SpringPopup on {gameObject.name} requires an AnchoredPositionSpringComponent for UI position animation. Adding it now.", this);
                }
                else if (!anchoredPositionSpringComponent.IsInitialized)
                {
                    anchoredPositionSpringComponent.Initialize();
                }
            }

            // --- Set Initial State ---
            // Call ApplyPreset here AFTER components are potentially added/initialized
            ApplyPreset(); // Re-apply preset to ensure forces/drags are set on potentially newly added component

            if (currentState == PopupState.Hidden)
            {
                SetHiddenState(false); // Set initial state without animation or events
            }
            else if (currentState == PopupState.Visible)
            {
                SetVisibleState(false); // Set initial state without animation or events
            }
        }

        /// <summary>
        /// Apply preset values to the relevant spring components.
        /// </summary>
        private void ApplyPreset()
        {
            if (preset == PopupPreset.Custom) return;

            // Preset values - same as before
            switch (preset)
            {
                 case PopupPreset.Bounce:
                    showForce = 100f; showDrag = 7f; hideForce = 80f; hideDrag = 10f;
                    addShowVelocity = true; addHideVelocity = false; showVelocityMultiplier = 1.5f; hideVelocityMultiplier = 0.5f;
                    scaleOvershoot = 1.25f; rotationOvershoot = 20f;
                    break;
                case PopupPreset.Elastic:
                    showForce = 60f; showDrag = 5f; hideForce = 70f; hideDrag = 8f;
                    addShowVelocity = true; addHideVelocity = false; showVelocityMultiplier = 1.8f; hideVelocityMultiplier = 0.5f;
                    scaleOvershoot = 1.3f; rotationOvershoot = 25f;
                    break;
                case PopupPreset.Snappy:
                    showForce = 150f; showDrag = 14f; hideForce = 150f; hideDrag = 14f;
                    addShowVelocity = false; addHideVelocity = false; showVelocityMultiplier = 0f; hideVelocityMultiplier = 0f;
                    scaleOvershoot = 1.05f; rotationOvershoot = 5f;
                    break;
                case PopupPreset.Wobbly:
                    showForce = 70f; showDrag = 4f; hideForce = 50f; hideDrag = 5f;
                    addShowVelocity = true; addHideVelocity = true; showVelocityMultiplier = 2f; hideVelocityMultiplier = 1f;
                    scaleOvershoot = 1.2f; rotationOvershoot = 30f;
                    break;
                case PopupPreset.Subtle:
                    showForce = 80f; showDrag = 10f; hideForce = 80f; hideDrag = 10f;
                    addShowVelocity = false; addHideVelocity = false; showVelocityMultiplier = 0.5f; hideVelocityMultiplier = 0.5f;
                    scaleOvershoot = 1.05f; rotationOvershoot = 5f;
                    break;
            }

            // Apply the forces/drags to the active springs
            ConfigureSpringForShowing(); // Apply show settings initially
        }

        /// <summary>
        /// Show the popup with animation.
        /// </summary>
        public void Show()
        {
            if (showCoroutine != null) StopCoroutine(showCoroutine);
            if (hideCoroutine != null) StopCoroutine(hideCoroutine);
            if (autoHideCoroutine != null) StopCoroutine(autoHideCoroutine);

            showCoroutine = StartCoroutine(ShowSequence());
        }

        /// <summary>
        /// Hide the popup with animation.
        /// </summary>
        public void Hide()
        {
            if (showCoroutine != null) StopCoroutine(showCoroutine);
            if (hideCoroutine != null) StopCoroutine(hideCoroutine);
            if (autoHideCoroutine != null) StopCoroutine(autoHideCoroutine);

            hideCoroutine = StartCoroutine(HideSequence());
        }

        /// <summary>
        /// Show animation sequence.
        /// </summary>
        private IEnumerator ShowSequence()
        {
            if (showDelay > 0) yield return new WaitForSeconds(showDelay);

            currentState = PopupState.Showing;
            onShow.Invoke();
            ConfigureSpringForShowing(); // Apply show forces/drags

            // --- Calculate Targets (including overshoot) ---
            Vector3 targetScale = visibleScale;
            Vector3 targetRotationEuler = visibleRotation;
            Vector3 targetPositionOffset = visiblePosition; // Use Vector3 for consistency

            // Apply overshoot effect for scale if enabled
            if (useScaleAnimation && scaleOvershoot > 1f)
            {
                Vector3 scaleDirection = (visibleScale - hiddenScale).normalized;
                targetScale = visibleScale + scaleDirection * (scaleOvershoot - 1f);
            }

            // Apply overshoot effect for rotation if enabled
            if (useRotationAnimation && rotationOvershoot > 0f)
            {
                Vector3 rotDirection = (visibleRotation - hiddenRotation).normalized;
                targetRotationEuler = visibleRotation + rotDirection * rotationOvershoot;
            }

            // --- Set Spring Targets ---
            if (useScaleAnimation)
            {
                transformSpringComponent.SetTargetScale(targetScale);
                if (addShowVelocity)
                {
                    Vector3 velocityDirection = (targetScale - hiddenScale).normalized;
                    transformSpringComponent.AddVelocityScale(velocityDirection * showVelocityMultiplier);
                }
            }

            if (usePositionAnimation)
            {
                if (isUiElement && anchoredPositionSpringComponent != null)
                {
                    // Use AnchoredPositionSpringComponent for UI
                    Vector2 targetAnchoredPos = startAnchoredPosition + (Vector2)targetPositionOffset; // Cast offset
                    anchoredPositionSpringComponent.SetTarget(targetAnchoredPos);
                    if (addShowVelocity)
                    {
                        Vector2 velocityDirection = ((Vector2)targetPositionOffset - (Vector2)hiddenPositionOffset).normalized;
                        anchoredPositionSpringComponent.AddVelocity(velocityDirection * showVelocityMultiplier);
                    }
                }
                else if (!isUiElement)
                {
                    // Use TransformSpringComponent for non-UI
                    Vector3 targetLocalPos = startLocalPosition + targetPositionOffset;
                    transformSpringComponent.SetTargetPosition(targetLocalPos);
                     if (addShowVelocity)
                    {
                        Vector3 velocityDirection = (targetPositionOffset - hiddenPositionOffset).normalized;
                        transformSpringComponent.AddVelocityPosition(velocityDirection * showVelocityMultiplier);
                    }
                }
            }

            if (useRotationAnimation)
            {
                transformSpringComponent.SetTargetRotation(Quaternion.Euler(targetRotationEuler));
                if (addShowVelocity)
                {
                    Vector3 velocityDirection = (targetRotationEuler - hiddenRotation).normalized;
                    transformSpringComponent.AddVelocityRotation(velocityDirection * showVelocityMultiplier);
                }
            }

            // --- Wait for Overshoot/Settling ---
            yield return new WaitForSeconds(0.5f); // Adjust timing as needed

            // --- Set Final Targets (without overshoot) ---
            if (useScaleAnimation) transformSpringComponent.SetTargetScale(visibleScale);

            if (usePositionAnimation)
            {
                 if (isUiElement && anchoredPositionSpringComponent != null)
                 {
                     anchoredPositionSpringComponent.SetTarget(startAnchoredPosition + (Vector2)visiblePosition);
                 }
                 else if (!isUiElement)
                 {
                     transformSpringComponent.SetTargetPosition(startLocalPosition + visiblePosition);
                 }
            }

            if (useRotationAnimation) transformSpringComponent.SetTargetRotation(Quaternion.Euler(visibleRotation));

            yield return new WaitForSeconds(0.3f); // Wait a bit more

            // --- Finalize State ---
            currentState = PopupState.Visible;
            onShown.Invoke();

            if (autoHideDuration > 0)
            {
                autoHideCoroutine = StartCoroutine(AutoHideSequence());
            }
            showCoroutine = null;
        }

        /// <summary>
        /// Hide animation sequence.
        /// </summary>
        private IEnumerator HideSequence()
        {
            if (hideDelay > 0) yield return new WaitForSeconds(hideDelay);

            currentState = PopupState.Hiding;
            onHide.Invoke();
            ConfigureSpringForHiding(); // Apply hide forces/drags

            // --- Set Spring Targets to Hidden Values ---
            if (useScaleAnimation)
            {
                transformSpringComponent.SetTargetScale(hiddenScale);
                if (addHideVelocity)
                {
                    Vector3 velocityDirection = (hiddenScale - visibleScale).normalized;
                    transformSpringComponent.AddVelocityScale(velocityDirection * hideVelocityMultiplier);
                }
            }

            if (usePositionAnimation)
            {
                 if (isUiElement && anchoredPositionSpringComponent != null)
                 {
                    // Use AnchoredPositionSpringComponent for UI
                    Vector2 targetAnchoredPos = startAnchoredPosition + (Vector2)hiddenPositionOffset; // Cast offset
                    anchoredPositionSpringComponent.SetTarget(targetAnchoredPos);
                     if (addHideVelocity)
                    {
                        Vector2 velocityDirection = ((Vector2)hiddenPositionOffset - (Vector2)visiblePosition).normalized;
                        anchoredPositionSpringComponent.AddVelocity(velocityDirection * hideVelocityMultiplier);
                    }
                 }
                 else if (!isUiElement)
                 {
                     // Use TransformSpringComponent for non-UI
                     Vector3 targetLocalPos = startLocalPosition + hiddenPositionOffset;
                     transformSpringComponent.SetTargetPosition(targetLocalPos);
                     if (addHideVelocity)
                     {
                         Vector3 velocityDirection = (hiddenPositionOffset - visiblePosition).normalized;
                         transformSpringComponent.AddVelocityPosition(velocityDirection * hideVelocityMultiplier);
                     }
                 }
            }

            if (useRotationAnimation)
            {
                transformSpringComponent.SetTargetRotation(Quaternion.Euler(hiddenRotation));
                 if (addHideVelocity)
                {
                    Vector3 velocityDirection = (hiddenRotation - visibleRotation).normalized;
                    transformSpringComponent.AddVelocityRotation(velocityDirection * hideVelocityMultiplier);
                }
            }

            // --- Wait for Animation ---
            yield return new WaitForSeconds(0.5f); // Adjust timing as needed

            // --- Finalize State ---
            currentState = PopupState.Hidden;
            onHidden.Invoke();
            hideCoroutine = null;
        }

        /// <summary>
        /// Auto-hide sequence.
        /// </summary>
        private IEnumerator AutoHideSequence()
        {
            yield return new WaitForSeconds(autoHideDuration);
            Hide();
            autoHideCoroutine = null;
        }

        /// <summary>
        /// Configure spring components for showing animation.
        /// </summary>
        private void ConfigureSpringForShowing()
        {
            // Configure TransformSpringComponent (Scale/Rotation/Non-UI Position)
            if (transformSpringComponent != null)
            {
                if (useScaleAnimation) transformSpringComponent.SetCommonForceAndDragScale(showForce, showDrag);
                if (useRotationAnimation) transformSpringComponent.SetCommonForceAndDragRotation(showForce, showDrag);
                if (usePositionAnimation && !isUiElement) transformSpringComponent.SetCommonForceAndDragPosition(showForce, showDrag);
            }

            // Configure AnchoredPositionSpringComponent (UI Position)
            if (isUiElement && usePositionAnimation && anchoredPositionSpringComponent != null)
            {
                anchoredPositionSpringComponent.SetCommonForceAndDrag(showForce, showDrag);
            }
        }

        /// <summary>
        /// Configure spring components for hiding animation.
        /// </summary>
        private void ConfigureSpringForHiding()
        {
             // Configure TransformSpringComponent (Scale/Rotation/Non-UI Position)
            if (transformSpringComponent != null)
            {
                if (useScaleAnimation) transformSpringComponent.SetCommonForceAndDragScale(hideForce, hideDrag);
                if (useRotationAnimation) transformSpringComponent.SetCommonForceAndDragRotation(hideForce, hideDrag);
                if (usePositionAnimation && !isUiElement) transformSpringComponent.SetCommonForceAndDragPosition(hideForce, hideDrag);
            }

            // Configure AnchoredPositionSpringComponent (UI Position)
            if (isUiElement && usePositionAnimation && anchoredPositionSpringComponent != null)
            {
                anchoredPositionSpringComponent.SetCommonForceAndDrag(hideForce, hideDrag);
            }
        }

        /// <summary>
        /// Set hidden state immediately (no animation).
        /// </summary>
        public void SetHiddenState(bool fireEvents = true)
        {
            // Ensure components are valid
            if (transformSpringComponent == null || (isUiElement && anchoredPositionSpringComponent == null && usePositionAnimation))
            {
                 InitializeComponents(); // Attempt re-initialization if needed
                 if (transformSpringComponent == null) return; // Still failed
            }

            // Set TransformSpringComponent values
            if (useScaleAnimation)
            {
                transformSpringComponent.SetCurrentValueScale(hiddenScale);
                transformSpringComponent.SetTargetScale(hiddenScale); // Keep target consistent
            }
            if (useRotationAnimation)
            {
                transformSpringComponent.SetCurrentValueRotation(Quaternion.Euler(hiddenRotation));
                transformSpringComponent.SetTargetRotation(Quaternion.Euler(hiddenRotation));
            }
            if (usePositionAnimation && !isUiElement)
            {
                transformSpringComponent.SetCurrentValuePosition(startLocalPosition + hiddenPositionOffset);
                transformSpringComponent.SetTargetPosition(startLocalPosition + hiddenPositionOffset);
            }

            // Set AnchoredPositionSpringComponent values
            if (usePositionAnimation && isUiElement && anchoredPositionSpringComponent != null)
            {
                Vector2 hiddenAnchoredPos = startAnchoredPosition + (Vector2)hiddenPositionOffset;
                anchoredPositionSpringComponent.SetCurrentValue(hiddenAnchoredPos);
                anchoredPositionSpringComponent.SetTarget(hiddenAnchoredPos);
            }

            currentState = PopupState.Hidden;
            if (fireEvents) onHidden.Invoke();
        }

        /// <summary>
        /// Set visible state immediately (no animation).
        /// </summary>
        public void SetVisibleState(bool fireEvents = true)
        {
             // Ensure components are valid
            if (transformSpringComponent == null || (isUiElement && anchoredPositionSpringComponent == null && usePositionAnimation))
            {
                 InitializeComponents(); // Attempt re-initialization if needed
                 if (transformSpringComponent == null) return; // Still failed
            }

            // Set TransformSpringComponent values
            if (useScaleAnimation)
            {
                transformSpringComponent.SetCurrentValueScale(visibleScale);
                transformSpringComponent.SetTargetScale(visibleScale);
            }
            if (useRotationAnimation)
            {
                transformSpringComponent.SetCurrentValueRotation(Quaternion.Euler(visibleRotation));
                transformSpringComponent.SetTargetRotation(Quaternion.Euler(visibleRotation));
            }
            if (usePositionAnimation && !isUiElement)
            {
                transformSpringComponent.SetCurrentValuePosition(startLocalPosition + visiblePosition);
                transformSpringComponent.SetTargetPosition(startLocalPosition + visiblePosition);
            }

            // Set AnchoredPositionSpringComponent values
            if (usePositionAnimation && isUiElement && anchoredPositionSpringComponent != null)
            {
                Vector2 visibleAnchoredPos = startAnchoredPosition + (Vector2)visiblePosition;
                anchoredPositionSpringComponent.SetCurrentValue(visibleAnchoredPos);
                anchoredPositionSpringComponent.SetTarget(visibleAnchoredPos);
            }

            currentState = PopupState.Visible;
            if (fireEvents) onShown.Invoke();
        }

        /// <summary>
        /// Toggle popup state.
        /// </summary>
        public void Toggle()
        {
            if (currentState == PopupState.Hidden || currentState == PopupState.Hiding)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        /// <summary>
        /// Update animation settings (forces/drags).
        /// </summary>
        public void UpdateAnimationSettings(float showForce, float showDrag, float hideForce, float hideDrag)
        {
            this.showForce = showForce;
            this.showDrag = showDrag;
            this.hideForce = hideForce;
            this.hideDrag = hideDrag;

            // Re-apply settings based on current state
            if (currentState == PopupState.Showing || currentState == PopupState.Visible)
            {
                ConfigureSpringForShowing();
            }
            else
            {
                ConfigureSpringForHiding();
            }
        }

        /// <summary>
        /// Apply a preset.
        /// </summary>
        public void SetPreset(PopupPreset newPreset)
        {
            preset = newPreset;
            ApplyPreset();
             // Re-apply settings based on current state after preset change
            if (currentState == PopupState.Showing || currentState == PopupState.Visible)
            {
                ConfigureSpringForShowing();
            }
            else
            {
                ConfigureSpringForHiding();
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Reset to default values in the editor.
        /// </summary>
        private void Reset()
        {
            // Try to find a RectTransform component
            rectTransform = GetComponent<RectTransform>();
            isUiElement = (rectTransform != null);

            // Set default animation type based on context
            if (isUiElement)
            {
                // UI object - default to scale
                useScaleAnimation = true;
                usePositionAnimation = false;
                useRotationAnimation = false;
                animationType = AnimationType.Scale;
            }
            else
            {
                // 3D/2D object - default to combined
                useScaleAnimation = true;
                usePositionAnimation = true;
                useRotationAnimation = true;
                animationType = AnimationType.Combined;
            }

            // Ensure TransformSpringComponent exists
            if (GetComponent<TransformSpringComponent>() == null)
            {
                gameObject.AddComponent<TransformSpringComponent>();
            }
            // Note: We don't add AnchoredPositionSpringComponent here automatically,
            // it will be added at runtime if needed by InitializeComponents.
        }
#endif
    }
}
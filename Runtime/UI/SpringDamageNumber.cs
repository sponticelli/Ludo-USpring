using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using USpring.Components;
using USpring.Core;

namespace USpring.UI
{
    /// <summary>
    /// Displays damage numbers with spring physics in 3D space, supporting stacked damage and configurable animations.
    /// </summary>
    [AddComponentMenu("Ludo/USpring/UI/Spring Damage Number (3D)")]
    public class SpringDamageNumber3D : MonoBehaviour
    {
        #region Settings
        [Header("References")]
        [SerializeField] private TextMeshPro damageText;
        [SerializeField] private TransformSpringComponent springComponent;

        [Header("Appearance")]
        [SerializeField] private bool useColorGradient = true;
        [SerializeField] private Gradient damageColorGradient;
        [SerializeField] private Color defaultDamageColor = Color.white;
        [SerializeField] private Color criticalDamageColor = Color.yellow;
        [SerializeField] private bool showPlusSymbol = true;
        [SerializeField] private float baseSize = 1f;
        [SerializeField] private float criticalSizeMultiplier = 1.5f;
        [SerializeField] private float stackedSizeMultiplier = 1.2f;

        [Header("Animation")]
        [SerializeField] private float lifetime = 2f;
        [SerializeField] private float fadeDelay = 0.5f;
        [SerializeField] private float initialVelocityMin = 1f;
        [SerializeField] private float initialVelocityMax = 3f;
        [SerializeField] private float initialForce = 80f;
        [SerializeField] private float initialDrag = 8f;
        [SerializeField] private Vector2 spreadAngleRange = new Vector2(-30f, 30f);
        [SerializeField] private bool applyGravity = true;
        [SerializeField] private float gravityStrength = 2f;
        [SerializeField] private bool wobbleOnCreate = true;
        [SerializeField] private float wobbleStrength = 0.3f;

        [Header("Stacking")]
        [SerializeField] private bool enableStacking = true;
        [SerializeField] private float stackRadius = 1f;
        [SerializeField] private float stackDuration = 0.5f;
#pragma warning disable 0414 // Field assigned but never used - intended for future stacking position offset feature
        [SerializeField] private float stackOffset = 0.2f;
#pragma warning restore 0414

        [Header("Events")]
        [SerializeField] private UnityEvent onDestroy;
        #endregion

        #region Private Variables
        private float currentDamage;
        private bool isCritical;
        private float originalLifetime;
        private float currentLifetime;
        private bool isStacked;
        private int numStacks;
        private Coroutine fadeCoroutine;
        private List<float> stackedDamages = new List<float>();
        private static Dictionary<Transform, List<SpringDamageNumber3D>> activeNumbersByTarget = new Dictionary<Transform, List<SpringDamageNumber3D>>();
        private Transform targetTransform;
        private Color originalColor;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (springComponent == null)
                springComponent = GetComponent<TransformSpringComponent>();

            if (damageText == null)
                damageText = GetComponentInChildren<TextMeshPro>();

            // Ensure we have required components
            if (springComponent == null)
                springComponent = gameObject.AddComponent<TransformSpringComponent>();

            // Initialize spring settings
            springComponent.SetCommonForceAndDragPosition(initialForce, initialDrag);
            springComponent.SetCommonForceAndDragScale(initialForce, initialDrag);
            springComponent.SetCommonForceAndDragRotation(initialForce * 0.5f, initialDrag * 1.5f);

            // Initialize with zero scale
            transform.localScale = Vector3.zero;

            // Store the original color
            if (damageText != null)
                originalColor = damageText.color;
        }

        private void OnEnable()
        {
            currentLifetime = lifetime;
        }

        private void Start()
        {
            if (springComponent != null && !springComponent.IsInitialized)
                springComponent.Initialize();
        }

        private void Update()
        {
            // Apply gravity
            if (applyGravity && springComponent != null)
            {
                Vector3 gravityVelocity = new Vector3(0, -gravityStrength, 0);
                springComponent.AddVelocityPosition(gravityVelocity * Time.deltaTime);
            }

            // Update lifetime
            currentLifetime -= Time.deltaTime;
            if (currentLifetime <= 0)
            {
                ReturnToPool();
            }
        }

        private void OnDisable()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }

            // Remove from active numbers list
            if (targetTransform != null && activeNumbersByTarget.ContainsKey(targetTransform))
            {
                activeNumbersByTarget[targetTransform].Remove(this);
                if (activeNumbersByTarget[targetTransform].Count == 0)
                    activeNumbersByTarget.Remove(targetTransform);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Shows a damage number at the specified position
        /// </summary>
        public void ShowDamage(float damage, Vector3 position, Transform target = null, bool critical = false)
        {
            // Store reference to target
            targetTransform = target;

            // Set initial position
            transform.position = position;

            springComponent.SetVelocityPosition(Vector3.zero);
            springComponent.SetCurrentValuePosition(Vector3.zero);
            springComponent.SetTargetPosition(position);

            // Set initial properties
            currentDamage = damage;
            isCritical = critical;
            isStacked = false;
            numStacks = 1;
            stackedDamages.Clear();
            stackedDamages.Add(damage);
            originalLifetime = lifetime;
            currentLifetime = lifetime;

            // Update text
            UpdateDamageText();

            // Apply scaling based on damage type
            Vector3 targetScale = Vector3.one * baseSize;
            if (critical)
                targetScale *= criticalSizeMultiplier;

            springComponent.SetTargetScale(targetScale);

            // Apply animation
            AnimateAppearance();

            // Start fade coroutine
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeOut());

            // Check for stacking with existing numbers
            if (enableStacking && target != null)
            {
                // Add to active numbers
                if (!activeNumbersByTarget.ContainsKey(target))
                    activeNumbersByTarget[target] = new List<SpringDamageNumber3D>();

                activeNumbersByTarget[target].Add(this);

                // Try stack with existing numbers
                TryStackWithExistingNumbers();
            }
        }

        /// <summary>
        /// Stacks additional damage onto this number
        /// </summary>
        public void StackDamage(float additionalDamage, bool critical = false)
        {
            if (!enableStacking)
                return;

            // Update properties
            float oldDamage = currentDamage;
            currentDamage += additionalDamage;
            stackedDamages.Add(additionalDamage);
            numStacks++;
            isStacked = true;

            // Critical if either the original or the added damage is critical
            isCritical = isCritical || critical;

            // Update text
            UpdateDamageText();

            // Create small bump animation
            Vector3 currentScale = transform.localScale;
            Vector3 targetScale = Vector3.one * (baseSize * (isCritical ? criticalSizeMultiplier : 1f) * (isStacked ? stackedSizeMultiplier : 1f));
            springComponent.SetTargetScale(targetScale);
            springComponent.AddVelocityScale(Vector3.one * 0.5f);

            // Add small upward bump
            springComponent.AddVelocityPosition(Vector3.up * 0.5f);

            // Reset lifetime
            currentLifetime = Mathf.Max(currentLifetime, originalLifetime * 0.7f);

            // Restart fade coroutine
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeOut());
        }
        #endregion

        #region Private Methods
        private void AnimateAppearance()
        {
            // Reset transform
            springComponent.SetCurrentValueScale(Vector3.zero);

            // Calculate random direction
            float angle = Random.Range(spreadAngleRange.x, spreadAngleRange.y);
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;

            // Apply initial velocity
            float velocity = Random.Range(initialVelocityMin, initialVelocityMax);
            springComponent.SetVelocityPosition(direction * velocity);

            // Add wobble if enabled
            if (wobbleOnCreate)
            {
                float randomAngle = Random.Range(-15f, 15f);
                springComponent.SetVelocityRotation(new Vector3(0, 0, randomAngle * wobbleStrength));
            }
        }

        private void UpdateDamageText()
        {
            // Format damage number
            string prefix = showPlusSymbol && currentDamage > 0 ? "+" : "";
            string text = prefix + currentDamage.ToString("0");

            // Add stack indicator if stacked
            if (isStacked && numStacks > 1)
            {
                text += $" x{numStacks}";
            }

            damageText.text = text;

            // Set color based on settings
            if (useColorGradient)
            {
                // Use normalized damage value for gradient lookup
                float normalizedDamage = Mathf.Clamp01(currentDamage / 100f);
                damageText.color = damageColorGradient.Evaluate(normalizedDamage);
            }
            else
            {
                damageText.color = isCritical ? criticalDamageColor : defaultDamageColor;
            }
        }

        private void TryStackWithExistingNumbers()
        {
            if (targetTransform == null || !activeNumbersByTarget.ContainsKey(targetTransform))
                return;

            List<SpringDamageNumber3D> numbers = activeNumbersByTarget[targetTransform];

            // Check for nearby numbers to stack with
            foreach (SpringDamageNumber3D otherNumber in numbers)
            {
                if (otherNumber == this || otherNumber == null)
                    continue;

                // Check if the number is close enough and recent enough to stack
                float distance = Vector3.Distance(transform.position, otherNumber.transform.position);
                if (distance <= stackRadius && (otherNumber.currentLifetime >= (originalLifetime - stackDuration)))
                {
                    // Stack with this number
                    otherNumber.StackDamage(currentDamage, isCritical);

                    // Remove this number as it's been stacked
                    activeNumbersByTarget[targetTransform].Remove(this);
                    ReturnToPool();
                    return;
                }
            }
        }

        private void ReturnToPool()
        {
            onDestroy?.Invoke();
        }

        private IEnumerator FadeOut()
        {
            // Wait for fade delay
            yield return new WaitForSeconds(fadeDelay);

            // Calculate fade duration
            float fadeDuration = lifetime - fadeDelay;
            float elapsedTime = 0f;

            // Fade out
            while (elapsedTime < fadeDuration)
            {
                float normalizedTime = elapsedTime / fadeDuration;
                damageText.color = Color.Lerp(originalColor, Color.clear, normalizedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure fully faded
            damageText.color = Color.clear;
            fadeCoroutine = null;
        }
        #endregion
    }
}
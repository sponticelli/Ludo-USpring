using UnityEngine;
using UnityEngine.UI;
using USpring.Components;
using USpring.Core;

namespace USpring.UI
{
    /// <summary>
    /// A health bar component with physically-based spring animation for the delayed/trailing effect.
    /// When health decreases, the main bar updates immediately while the background bar follows with spring physics.
    /// When health increases, the main bar follows the background bar with spring physics.
    /// </summary>
    [AddComponentMenu("Ludo/USpring/UI/Spring Health Bar")]
    public class SpringHealthBar : MonoBehaviour
    {
        [Header("Bar References")]
        [SerializeField] private Image mainBar;
        [SerializeField] private Image delayedBar;
        
        [Header("Spring Settings")]
        [Tooltip("Spring force for the delayed bar (higher = faster reaction)")]
        [SerializeField] private float delayedBarForce = 5f;
        [Tooltip("Spring drag for the delayed bar (higher = less bouncy)")]
        [SerializeField] private float delayedBarDrag = 4f;
        
        [Tooltip("Spring force for the main bar when increasing (higher = faster reaction)")]
        [SerializeField] private float increasingMainBarForce = 10f;
        [Tooltip("Spring drag for the main bar when increasing (higher = less bouncy)")]
        [SerializeField] private float increasingMainBarDrag = 5f;
        
        [Header("Appearance")]
        [Tooltip("Whether to use a gradient for the main bar color based on health")]
        [SerializeField] private bool useGradient = false;
        [Tooltip("Gradient for the main bar when using gradient mode (left = 0 health, right = full health)")]
        [SerializeField] private Gradient healthGradient;
        [Tooltip("Color for the main bar when health is stable (used when not using gradient)")]
        [SerializeField] private Color mainBarColor = Color.green;
        [Tooltip("Color for the main bar when health is decreasing (used when not using gradient)")]
        [SerializeField] private Color decreasingMainBarColor = Color.red;
        [Tooltip("Color for the delayed bar when health is decreasing")]
        [SerializeField] private Color delayedBarDecreasingColor = new Color(0.5f, 0.8f, 0.5f);
        [Tooltip("Color for the delayed bar when health is increasing")]
        [SerializeField] private Color delayedBarIncreasingColor = new Color(0.7f, 0.9f, 0.7f);
        [Tooltip("Whether to perform color transitions using springs")]
        [SerializeField] private bool useSpringColorTransitions = true;
        
        [Header("Behavior")]
        [Tooltip("Current health ratio (0-1)")]
        [Range(0, 1)]
        [SerializeField] private float healthRatio = 1.0f;
        [Tooltip("Maximum health value")]
        [SerializeField] private float maxHealth = 100f;
        [Tooltip("Minimum health value")]
        [SerializeField] private float minHealth = 0f;
        [Tooltip("Whether to animate when health increases")]
        [SerializeField] private bool animateHealthIncrease = true;
        [Tooltip("Whether to animate when health decreases")]
        [SerializeField] private bool animateHealthDecrease = true;
        
        // Spring components for animation
        private SpringFloat mainBarSpring;
        private SpringFloat delayedBarSpring;
        private SpringColor mainBarColorSpring;
        
        // Cache for tracking health direction changes
        private float previousRatio = 1.0f;
        private bool isHealthDecreasing = false;
        
        // Properties
        public float CurrentHealth
        {
            get { return minHealth + (maxHealth - minHealth) * healthRatio; }
            set { SetHealth(value); }
        }
        
        public float MaxHealth
        {
            get { return maxHealth; }
            set 
            { 
                // Store current health
                float currentHealth = CurrentHealth;
                
                // Update max health
                maxHealth = Mathf.Max(value, minHealth);
                
                // Re-apply current health to update ratio
                CurrentHealth = currentHealth;
            }
        }
        
        private void Awake()
        {
            // Initialize springs
            InitializeSprings();
            
            // Set initial values
            previousRatio = healthRatio;
            UpdateBars(healthRatio);
        }
        
        private void Update()
        {
            // Update spring physics simulation
            float deltaTime = Time.deltaTime;
            
            // Update main bar spring
            if (mainBarSpring != null)
            {
                // Update the spring physics 
                SpringMath.UpdateSpring(deltaTime, mainBarSpring);
                mainBarSpring.ProcessCandidateValue();
                mainBarSpring.CheckEvents();
                
                // Apply to UI if bar exists
                if (mainBar != null)
                {
                    mainBar.fillAmount = mainBarSpring.GetCurrentValue();
                }
            }
            
            // Update delayed bar spring
            if (delayedBarSpring != null)
            {
                // Update the spring physics
                SpringMath.UpdateSpring(deltaTime, delayedBarSpring);
                delayedBarSpring.ProcessCandidateValue();
                delayedBarSpring.CheckEvents();
                
                // Apply to UI if bar exists
                if (delayedBar != null)
                {
                    delayedBar.fillAmount = delayedBarSpring.GetCurrentValue();
                }
            }
            
            // Update color spring
            if (mainBarColorSpring != null)
            {
                // Update the spring physics
                SpringMath.UpdateSpring(deltaTime, mainBarColorSpring);
                mainBarColorSpring.ProcessCandidateValue();
                mainBarColorSpring.CheckEvents();
                
                // Apply to UI if using spring color transitions
                if (useSpringColorTransitions && mainBar != null)
                {
                    mainBar.color = mainBarColorSpring.GetCurrentColor();
                }
            }
        }
        
        /// <summary>
        /// Initialize spring components
        /// </summary>
        private void InitializeSprings()
        {
            // Create main bar spring
            mainBarSpring = new SpringFloat();
            mainBarSpring.SetCommonForceAndDrag(true);
            mainBarSpring.SetCommonForce(increasingMainBarForce);
            mainBarSpring.SetCommonDrag(increasingMainBarDrag);
            mainBarSpring.SetCurrentValue(healthRatio);
            mainBarSpring.SetTarget(healthRatio);
            
            // Set clamp min/max values to ensure health stays within 0-1 range
            mainBarSpring.SetClampCurrentValue(true);
            mainBarSpring.SetClampTarget(true);
            mainBarSpring.SetMinValue(0f);
            mainBarSpring.SetMaxValue(1f);
            
            mainBarSpring.Initialize();
            
            // Create delayed bar spring
            delayedBarSpring = new SpringFloat();
            delayedBarSpring.SetCommonForceAndDrag(true);
            delayedBarSpring.SetCommonForce(delayedBarForce);
            delayedBarSpring.SetCommonDrag(delayedBarDrag);
            delayedBarSpring.SetCurrentValue(healthRatio);
            delayedBarSpring.SetTarget(healthRatio);
            
            // Set clamp min/max values to ensure health stays within 0-1 range
            delayedBarSpring.SetClampCurrentValue(true);
            delayedBarSpring.SetClampTarget(true);
            delayedBarSpring.SetMinValue(0f);
            delayedBarSpring.SetMaxValue(1f);
            
            delayedBarSpring.Initialize();
            
            // Create color spring
            mainBarColorSpring = new SpringColor();
            mainBarColorSpring.SetCommonForceAndDrag(true);
            mainBarColorSpring.SetCommonForce(8f);
            mainBarColorSpring.SetCommonDrag(5f);
            mainBarColorSpring.SetCurrentValue(mainBarColor);
            mainBarColorSpring.SetTarget(mainBarColor);
            mainBarColorSpring.Initialize();
            
            // Set initial colors
            if (mainBar != null)
            {
                if (useGradient && healthGradient != null)
                {
                    mainBar.color = healthGradient.Evaluate(healthRatio);
                }
                else
                {
                    mainBar.color = mainBarColor;
                }
            }
            
            if (delayedBar != null)
            {
                // Initially, we're neither increasing nor decreasing, so use the decreasing color as default
                delayedBar.color = delayedBarDecreasingColor;
            }
        }
        
        /// <summary>
        /// Set health value and update bar display
        /// </summary>
        public void SetHealth(float value)
        {
            // Calculate new health ratio
            float newValue = Mathf.Clamp(value, minHealth, maxHealth);
            float newRatio = (newValue - minHealth) / (maxHealth - minHealth);
            
            // Track if health is increasing or decreasing
            isHealthDecreasing = newRatio < previousRatio;
            
            // Update health ratio
            healthRatio = newRatio;
            
            // Update bar display
            UpdateBars(newRatio);
            
            // Store for next comparison
            previousRatio = newRatio;
        }
        
        /// <summary>
        /// Add health value to current health
        /// </summary>
        public void AddHealth(float value)
        {
            SetHealth(CurrentHealth + value);
        }
        
        /// <summary>
        /// Remove health value from current health
        /// </summary>
        public void RemoveHealth(float value)
        {
            SetHealth(CurrentHealth - value);
        }
        
        /// <summary>
        /// Set health as percentage of max health (0-1)
        /// </summary>
        public void SetHealthRatio(float ratio)
        {
            // Track if health is increasing or decreasing
            isHealthDecreasing = ratio < previousRatio;
            
            // Update health ratio
            healthRatio = Mathf.Clamp01(ratio);
            
            // Update bar display
            UpdateBars(healthRatio);
            
            // Store for next comparison
            previousRatio = healthRatio;
        }
        
        /// <summary>
        /// Update bar display based on current health ratio
        /// </summary>
        private void UpdateBars(float ratio)
        {
            if (isHealthDecreasing)
            {
                // HEALTH DECREASING:
                // Main bar moves immediately to new value
                // Delayed bar follows with spring physics
                
                if (mainBarSpring != null)
                {
                    // Immediately set main bar to new ratio value
                    mainBarSpring.SetCurrentValue(ratio);
                    mainBarSpring.SetTarget(ratio);
                }
                
                if (delayedBarSpring != null && animateHealthDecrease)
                {
                    // Set delayed bar target to the same value, but let spring physics animate it
                    delayedBarSpring.SetTarget(ratio);
                }
                else if (delayedBarSpring != null)
                {
                    // If not animating health decrease, immediately set delayed bar too
                    delayedBarSpring.SetCurrentValue(ratio);
                    delayedBarSpring.SetTarget(ratio);
                }
                
                // Change main bar color
                if (useGradient && healthGradient != null)
                {
                    // For gradient mode, we always use the gradient color based on health ratio
                    if (useSpringColorTransitions && mainBarColorSpring != null)
                    {
                        mainBarColorSpring.SetTarget(healthGradient.Evaluate(ratio));
                    }
                    else if (mainBar != null)
                    {
                        mainBar.color = healthGradient.Evaluate(ratio);
                    }
                }
                else
                {
                    // For non-gradient mode, we use the decreasing color
                    if (useSpringColorTransitions && mainBarColorSpring != null)
                    {
                        mainBarColorSpring.SetTarget(decreasingMainBarColor);
                    }
                    else if (mainBar != null)
                    {
                        mainBar.color = decreasingMainBarColor;
                    }
                }
                
                // Set delayed bar color for decreasing
                if (delayedBar != null)
                {
                    delayedBar.color = delayedBarDecreasingColor;
                }
            }
            else
            {
                // HEALTH INCREASING:
                // Delayed bar moves immediately to new value
                // Main bar follows with spring physics
                
                if (delayedBarSpring != null)
                {
                    // Immediately set delayed bar to new ratio value
                    delayedBarSpring.SetCurrentValue(ratio);
                    delayedBarSpring.SetTarget(ratio);
                }
                
                if (mainBarSpring != null && animateHealthIncrease)
                {
                    // Set main bar target to the same value, but let spring physics animate it
                    mainBarSpring.SetTarget(ratio);
                }
                else if (mainBarSpring != null)
                {
                    // If not animating health increase, immediately set main bar too
                    mainBarSpring.SetCurrentValue(ratio);
                    mainBarSpring.SetTarget(ratio);
                }
                
                // Change main bar color
                if (useGradient && healthGradient != null)
                {
                    // For gradient mode, we always use the gradient color based on health ratio
                    if (useSpringColorTransitions && mainBarColorSpring != null)
                    {
                        mainBarColorSpring.SetTarget(healthGradient.Evaluate(ratio));
                    }
                    else if (mainBar != null)
                    {
                        mainBar.color = healthGradient.Evaluate(ratio);
                    }
                }
                else
                {
                    // For non-gradient mode, we use the normal color
                    if (useSpringColorTransitions && mainBarColorSpring != null)
                    {
                        mainBarColorSpring.SetTarget(mainBarColor);
                    }
                    else if (mainBar != null)
                    {
                        mainBar.color = mainBarColor;
                    }
                }
                
                // Set delayed bar color for increasing
                if (delayedBar != null)
                {
                    delayedBar.color = delayedBarIncreasingColor;
                }
            }
            
            // If no animation for either direction, just update both bars immediately
            if (!animateHealthIncrease && !animateHealthDecrease)
            {
                if (mainBarSpring != null)
                {
                    mainBarSpring.SetCurrentValue(ratio);
                    mainBarSpring.SetTarget(ratio);
                }
                
                if (delayedBarSpring != null)
                {
                    delayedBarSpring.SetCurrentValue(ratio);
                    delayedBarSpring.SetTarget(ratio);
                }
            }
        }
        
        /// <summary>
        /// Update spring settings
        /// </summary>
        public void UpdateSpringSettings(float delayedForce, float delayedDrag, float increasingForce, float increasingDrag)
        {
            // Update spring settings
            delayedBarForce = delayedForce;
            delayedBarDrag = delayedDrag;
            increasingMainBarForce = increasingForce;
            increasingMainBarDrag = increasingDrag;
            
            // Apply to springs if initialized
            if (delayedBarSpring != null)
            {
                delayedBarSpring.SetCommonForce(delayedBarForce);
                delayedBarSpring.SetCommonDrag(delayedBarDrag);
            }
            
            if (mainBarSpring != null)
            {
                mainBarSpring.SetCommonForce(increasingMainBarForce);
                mainBarSpring.SetCommonDrag(increasingMainBarDrag);
            }
        }
        
        /// <summary>
        /// Update bar colors
        /// </summary>
        public void UpdateColors(Color mainColor, Color decreasingColor, Color delayedDecreasingColor, Color delayedIncreasingColor)
        {
            mainBarColor = mainColor;
            decreasingMainBarColor = decreasingColor;
            delayedBarDecreasingColor = delayedDecreasingColor;
            delayedBarIncreasingColor = delayedIncreasingColor;
            
            // Apply to delayed bar immediately based on current state
            if (delayedBar != null)
            {
                delayedBar.color = isHealthDecreasing ? delayedBarDecreasingColor : delayedBarIncreasingColor;
            }
            
            // Apply to main bar based on current state and gradient setting
            if (!useGradient)
            {
                if (mainBarColorSpring != null && useSpringColorTransitions)
                {
                    Color targetColor = isHealthDecreasing ? decreasingMainBarColor : mainBarColor;
                    mainBarColorSpring.SetTarget(targetColor);
                }
                else if (mainBar != null)
                {
                    Color targetColor = isHealthDecreasing ? decreasingMainBarColor : mainBarColor;
                    mainBar.color = targetColor;
                }
            }
        }
        
        /// <summary>
        /// Set the gradient for health-based coloring
        /// </summary>
        public void SetHealthGradient(Gradient gradient, bool useGradientMode)
        {
            healthGradient = gradient;
            useGradient = useGradientMode;
            
            // Update colors immediately if using gradient mode
            if (useGradient && healthGradient != null)
            {
                Color gradientColor = healthGradient.Evaluate(healthRatio);
                
                if (mainBarColorSpring != null && useSpringColorTransitions)
                {
                    mainBarColorSpring.SetTarget(gradientColor);
                }
                else if (mainBar != null)
                {
                    mainBar.color = gradientColor;
                }
            }
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Reset component with default values
        /// </summary>
        private void Reset()
        {
            // Try to locate existing Image components in children
            Image[] images = GetComponentsInChildren<Image>();
            
            if (images.Length >= 2)
            {
                // Assume first is main bar, second is delayed bar
                mainBar = images[0];
                delayedBar = images[1];
            }
            else if (images.Length == 1)
            {
                // If only one image, use it as main bar
                mainBar = images[0];
                
                // Create a GameObject for the delayed bar
                GameObject delayedBarGO = new GameObject("DelayedBar");
                delayedBarGO.transform.SetParent(transform);
                delayedBarGO.transform.localPosition = Vector3.zero;
                delayedBarGO.transform.localRotation = Quaternion.identity;
                delayedBarGO.transform.localScale = Vector3.one;
                
                // Add Image component and set it up
                delayedBar = delayedBarGO.AddComponent<Image>();
                if (mainBar != null)
                {
                    // Copy properties from main bar
                    delayedBar.sprite = mainBar.sprite;
                    delayedBar.type = mainBar.type;
                    delayedBar.fillMethod = mainBar.fillMethod;
                    delayedBar.fillOrigin = mainBar.fillOrigin;
                    delayedBar.fillAmount = mainBar.fillAmount;
                    
                    // Set delayedBar as sibling before mainBar
                    delayedBarGO.transform.SetSiblingIndex(mainBar.transform.GetSiblingIndex());
                }
            }
            
            // Set initial colors
            if (mainBar != null)
            {
                mainBar.color = mainBarColor;
            }
            
            if (delayedBar != null)
            {
                delayedBar.color = delayedBarDecreasingColor;
            }
        }
#endif
    }
}
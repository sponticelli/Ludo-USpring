using UnityEngine;
using USpring.Components;
using System.Collections;

namespace USpring.UI
{
    /// <summary>
    /// Makes objects pulse in scale using spring physics.
    /// Great for attention-grabbing UI elements, "breathable" objects, or interaction feedback.
    /// </summary>
    [AddComponentMenu("Ludo/USpring/Components/Spring Pulse")]
    public class SpringPulse : MonoBehaviour
    {
        [SerializeField] private TransformSpringComponent springComponent;
        
        [Header("Pulse Settings")]
        [Tooltip("Should the pulse effect automatically start on enable")]
        [SerializeField] private bool autoStart = true;
        
        [Tooltip("Should the pulse loop continuously")]
        [SerializeField] private bool loop = true;
        
        [Tooltip("The base scale to return to")]
        [SerializeField] private Vector3 baseScale = Vector3.one;
        
        [Tooltip("The scale to pulse toward")]
        [SerializeField] private Vector3 pulseScale = new Vector3(1.2f, 1.2f, 1.2f);
        
        [Tooltip("Time in seconds between pulses")]
        [SerializeField] private float pulseInterval = 1.0f;
        
        [Tooltip("Add random variation to pulse interval")]
        [SerializeField] private float randomVariation = 0.1f;
        
        [Header("Spring Settings")]
        [Tooltip("Spring force (higher values = faster, more reactive)")]
        [SerializeField] private float springForce = 70f;
        
        [Tooltip("Spring drag (higher values = less bouncy)")]
        [SerializeField] private float springDrag = 7f;
        
        [Tooltip("Add velocity during transitions (creates more bounce)")]
        [SerializeField] private bool addVelocity = true;
        
        [Tooltip("Velocity impulse strength")]
        [SerializeField] private float velocityScale = 2f;
        
        private Coroutine pulseRoutine;
        private bool isPulsing = false;
        
        private void Awake()
        {
            // Automatically connect transform spring if not set
            if (springComponent == null)
            {
                springComponent = GetComponent<TransformSpringComponent>();
                
                // Create one if it doesn't exist
                if (springComponent == null)
                {
                    springComponent = gameObject.AddComponent<TransformSpringComponent>();
                }
            }
            
            // Configure spring with our settings
            ConfigureSpring();
        }
        
        private void OnEnable()
        {
            if (autoStart)
            {
                StartPulsing();
            }
        }
        
        private void OnDisable()
        {
            StopPulsing();
        }
        
        /// <summary>
        /// Configure spring with custom settings
        /// </summary>
        private void ConfigureSpring()
        {
            if (springComponent == null) return;
            
            // Initialize the spring if it hasn't been
            if (!springComponent.IsInitialized)
            {
                springComponent.Initialize();
            }
            
            // Configure spring parameters
            springComponent.SetCommonForceAndDragScale(springForce, springDrag);
            
            // Set initial state
            springComponent.SetTargetScale(baseScale);
            springComponent.SetCurrentValueScale(baseScale);
        }
        
        /// <summary>
        /// Start the continuous pulsing effect
        /// </summary>
        public void StartPulsing()
        {
            if (isPulsing) return;
            
            StopPulsing(); // Clear any existing routine
            isPulsing = true;
            pulseRoutine = StartCoroutine(PulseRoutine());
        }
        
        /// <summary>
        /// Stop the continuous pulsing effect
        /// </summary>
        public void StopPulsing()
        {
            if (pulseRoutine != null)
            {
                StopCoroutine(pulseRoutine);
                pulseRoutine = null;
            }
            
            isPulsing = false;
            
            // Optionally return to base scale
            if (springComponent != null && springComponent.IsInitialized)
            {
                springComponent.SetTargetScale(baseScale);
            }
        }
        
        /// <summary>
        /// Trigger a single pulse effect
        /// </summary>
        public void TriggerPulse()
        {
            if (springComponent == null || !springComponent.IsInitialized) return;
            
            // Set the target to pulse scale
            springComponent.SetTargetScale(pulseScale);
            
            // Add velocity for extra bounce
            if (addVelocity)
            {
                Vector3 velocityImpulse = (pulseScale - baseScale) * velocityScale;
                springComponent.AddVelocityScale(velocityImpulse);
            }
            
            // Start a coroutine to return to base scale
            StartCoroutine(ReturnToBaseScaleAfterDelay(pulseInterval * 0.5f));
        }
        
        /// <summary>
        /// Update spring settings at runtime
        /// </summary>
        public void UpdateSpringSettings(float force, float drag)
        {
            springForce = force;
            springDrag = drag;
            
            if (springComponent != null && springComponent.IsInitialized)
            {
                springComponent.SetCommonForceAndDragScale(force, drag);
            }
        }
        
        /// <summary>
        /// Update pulse settings at runtime
        /// </summary>
        public void UpdatePulseSettings(Vector3 baseScale, Vector3 pulseScale, float interval, bool shouldLoop)
        {
            this.baseScale = baseScale;
            this.pulseScale = pulseScale;
            this.pulseInterval = interval;
            this.loop = shouldLoop;
            
            // If we're not looping anymore, stop the routine
            if (!shouldLoop && isPulsing)
            {
                StopPulsing();
            }
            // If we started looping, start the routine
            else if (shouldLoop && !isPulsing)
            {
                StartPulsing();
            }
        }
        
        /// <summary>
        /// Coroutine to handle continuous pulsing
        /// </summary>
        private IEnumerator PulseRoutine()
        {
            while (true)
            {
                // Pulse toward the pulse scale
                springComponent.SetTargetScale(pulseScale);
                
                // Add velocity for extra bounce
                if (addVelocity)
                {
                    Vector3 velocityImpulse = (pulseScale - baseScale) * velocityScale;
                    springComponent.AddVelocityScale(velocityImpulse);
                }
                
                // Wait half the interval
                float variableDelay = pulseInterval * 0.5f;
                if (randomVariation > 0)
                {
                    variableDelay += Random.Range(-randomVariation, randomVariation);
                }
                yield return new WaitForSeconds(Mathf.Max(0.1f, variableDelay));
                
                // Return to base scale
                springComponent.SetTargetScale(baseScale);
                
                // Add return velocity for extra bounce
                if (addVelocity)
                {
                    Vector3 velocityImpulse = (baseScale - pulseScale) * velocityScale;
                    springComponent.AddVelocityScale(velocityImpulse);
                }
                
                // Wait the rest of the interval before next pulse
                variableDelay = pulseInterval;
                if (randomVariation > 0)
                {
                    variableDelay += Random.Range(-randomVariation, randomVariation);
                }
                yield return new WaitForSeconds(Mathf.Max(0.1f, variableDelay));
                
                // If not looping, exit after one cycle
                if (!loop)
                {
                    isPulsing = false;
                    pulseRoutine = null;
                    yield break;
                }
            }
        }
        
        /// <summary>
        /// Coroutine to return to base scale after a delay
        /// </summary>
        private IEnumerator ReturnToBaseScaleAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            // Return to base scale
            springComponent.SetTargetScale(baseScale);
            
            // Add velocity for extra bounce
            if (addVelocity)
            {
                Vector3 velocityImpulse = (baseScale - pulseScale) * velocityScale;
                springComponent.AddVelocityScale(velocityImpulse);
            }
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            // Try to find existing spring component
            springComponent = GetComponent<TransformSpringComponent>();
        }
#endif
    }
}
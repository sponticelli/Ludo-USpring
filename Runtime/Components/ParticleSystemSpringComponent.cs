using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
    [AddComponentMenu("Ludo/USpring/Components/Particle System Spring")]
    public partial class ParticleSystemSpringComponent : SpringComponent
    {
        [SerializeField] private ParticleSystem autoUpdatedParticleSystem;

        // Springs for various particle system properties
        [SerializeField] private SpringFloat emissionRateSpring = new SpringFloat();
        [SerializeField] private SpringFloat startLifetimeSpring = new SpringFloat();
        [SerializeField] private SpringFloat startSpeedSpring = new SpringFloat();
        [SerializeField] private SpringFloat startSizeSpring = new SpringFloat();
        [SerializeField] private SpringColor startColorSpring = new SpringColor();
        [SerializeField] private SpringFloat simulationSpeedSpring = new SpringFloat();

        // Toggle which properties to animate
        [SerializeField] private bool animateEmissionRate = true;
        [SerializeField] private bool animateStartLifetime = false;
        [SerializeField] private bool animateStartSpeed = false;
        [SerializeField] private bool animateStartSize = false;
        [SerializeField] private bool animateStartColor = false;
        [SerializeField] private bool animateSimulationSpeed = false;

        // Cache for particle system modules
        private ParticleSystem.MainModule mainModule;
        private ParticleSystem.EmissionModule emissionModule;

        // Cached current emission rate for cases where emission is using curves/random between two constants
        private float cachedEmissionRate;
        private bool emissionModuleEnabled;

        #region Emission Rate Spring Methods
        public SpringEvents EmissionRateEvents => emissionRateSpring.springEvents;
        public float GetTargetEmissionRate() => emissionRateSpring.GetTarget();
        public void SetTargetEmissionRate(float target) => emissionRateSpring.SetTarget(target);
        public float GetCurrentEmissionRate() => emissionRateSpring.GetCurrentValue();
        public void SetCurrentEmissionRate(float value) => emissionRateSpring.SetCurrentValue(value);
        public float GetVelocityEmissionRate() => emissionRateSpring.GetVelocity();
        public void SetVelocityEmissionRate(float velocity) => emissionRateSpring.SetVelocity(velocity);
        public void AddVelocityEmissionRate(float velocityToAdd) => emissionRateSpring.AddVelocity(velocityToAdd);
        public void ReachEquilibriumEmissionRate() => emissionRateSpring.ReachEquilibrium();
        public float GetForceEmissionRate() => emissionRateSpring.GetForce();
        public void SetForceEmissionRate(float force) => emissionRateSpring.SetForce(force);
        public float GetDragEmissionRate() => emissionRateSpring.GetDrag();
        public void SetDragEmissionRate(float drag) => emissionRateSpring.SetDrag(drag);
        public float GetCommonForceEmissionRate() => emissionRateSpring.GetCommonForce();
        public float GetCommonDragEmissionRate() => emissionRateSpring.GetCommonDrag();
        public void SetCommonForceEmissionRate(float force)
        {
            emissionRateSpring.SetCommonForceAndDrag(true);
            emissionRateSpring.SetCommonForce(force);
        }
        public void SetCommonDragEmissionRate(float drag)
        {
            emissionRateSpring.SetCommonForceAndDrag(true);
            emissionRateSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragEmissionRate(float force, float drag)
        {
            SetCommonForceEmissionRate(force);
            SetCommonDragEmissionRate(drag);
        }
        #endregion

        #region Start Lifetime Spring Methods
        public SpringEvents StartLifetimeEvents => startLifetimeSpring.springEvents;
        public float GetTargetStartLifetime() => startLifetimeSpring.GetTarget();
        public void SetTargetStartLifetime(float target) => startLifetimeSpring.SetTarget(target);
        public float GetCurrentStartLifetime() => startLifetimeSpring.GetCurrentValue();
        public void SetCurrentStartLifetime(float value) => startLifetimeSpring.SetCurrentValue(value);
        public float GetVelocityStartLifetime() => startLifetimeSpring.GetVelocity();
        public void SetVelocityStartLifetime(float velocity) => startLifetimeSpring.SetVelocity(velocity);
        public void AddVelocityStartLifetime(float velocityToAdd) => startLifetimeSpring.AddVelocity(velocityToAdd);
        public void ReachEquilibriumStartLifetime() => startLifetimeSpring.ReachEquilibrium();
        public float GetForceStartLifetime() => startLifetimeSpring.GetForce();
        public void SetForceStartLifetime(float force) => startLifetimeSpring.SetForce(force);
        public float GetDragStartLifetime() => startLifetimeSpring.GetDrag();
        public void SetDragStartLifetime(float drag) => startLifetimeSpring.SetDrag(drag);
        public float GetCommonForceStartLifetime() => startLifetimeSpring.GetCommonForce();
        public float GetCommonDragStartLifetime() => startLifetimeSpring.GetCommonDrag();
        public void SetCommonForceStartLifetime(float force)
        {
            startLifetimeSpring.SetCommonForceAndDrag(true);
            startLifetimeSpring.SetCommonForce(force);
        }
        public void SetCommonDragStartLifetime(float drag)
        {
            startLifetimeSpring.SetCommonForceAndDrag(true);
            startLifetimeSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragStartLifetime(float force, float drag)
        {
            SetCommonForceStartLifetime(force);
            SetCommonDragStartLifetime(drag);
        }
        #endregion

        #region Start Speed Spring Methods
        public SpringEvents StartSpeedEvents => startSpeedSpring.springEvents;
        public float GetTargetStartSpeed() => startSpeedSpring.GetTarget();
        public void SetTargetStartSpeed(float target) => startSpeedSpring.SetTarget(target);
        public float GetCurrentStartSpeed() => startSpeedSpring.GetCurrentValue();
        public void SetCurrentStartSpeed(float value) => startSpeedSpring.SetCurrentValue(value);
        public float GetVelocityStartSpeed() => startSpeedSpring.GetVelocity();
        public void SetVelocityStartSpeed(float velocity) => startSpeedSpring.SetVelocity(velocity);
        public void AddVelocityStartSpeed(float velocityToAdd) => startSpeedSpring.AddVelocity(velocityToAdd);
        public void ReachEquilibriumStartSpeed() => startSpeedSpring.ReachEquilibrium();
        public float GetForceStartSpeed() => startSpeedSpring.GetForce();
        public void SetForceStartSpeed(float force) => startSpeedSpring.SetForce(force);
        public float GetDragStartSpeed() => startSpeedSpring.GetDrag();
        public void SetDragStartSpeed(float drag) => startSpeedSpring.SetDrag(drag);
        public float GetCommonForceStartSpeed() => startSpeedSpring.GetCommonForce();
        public float GetCommonDragStartSpeed() => startSpeedSpring.GetCommonDrag();
        public void SetCommonForceStartSpeed(float force)
        {
            startSpeedSpring.SetCommonForceAndDrag(true);
            startSpeedSpring.SetCommonForce(force);
        }
        public void SetCommonDragStartSpeed(float drag)
        {
            startSpeedSpring.SetCommonForceAndDrag(true);
            startSpeedSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragStartSpeed(float force, float drag)
        {
            SetCommonForceStartSpeed(force);
            SetCommonDragStartSpeed(drag);
        }
        #endregion

        #region Start Size Spring Methods
        public SpringEvents StartSizeEvents => startSizeSpring.springEvents;
        public float GetTargetStartSize() => startSizeSpring.GetTarget();
        public void SetTargetStartSize(float target) => startSizeSpring.SetTarget(target);
        public float GetCurrentStartSize() => startSizeSpring.GetCurrentValue();
        public void SetCurrentStartSize(float value) => startSizeSpring.SetCurrentValue(value);
        public float GetVelocityStartSize() => startSizeSpring.GetVelocity();
        public void SetVelocityStartSize(float velocity) => startSizeSpring.SetVelocity(velocity);
        public void AddVelocityStartSize(float velocityToAdd) => startSizeSpring.AddVelocity(velocityToAdd);
        public void ReachEquilibriumStartSize() => startSizeSpring.ReachEquilibrium();
        public float GetForceStartSize() => startSizeSpring.GetForce();
        public void SetForceStartSize(float force) => startSizeSpring.SetForce(force);
        public float GetDragStartSize() => startSizeSpring.GetDrag();
        public void SetDragStartSize(float drag) => startSizeSpring.SetDrag(drag);
        public float GetCommonForceStartSize() => startSizeSpring.GetCommonForce();
        public float GetCommonDragStartSize() => startSizeSpring.GetCommonDrag();
        public void SetCommonForceStartSize(float force)
        {
            startSizeSpring.SetCommonForceAndDrag(true);
            startSizeSpring.SetCommonForce(force);
        }
        public void SetCommonDragStartSize(float drag)
        {
            startSizeSpring.SetCommonForceAndDrag(true);
            startSizeSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragStartSize(float force, float drag)
        {
            SetCommonForceStartSize(force);
            SetCommonDragStartSize(drag);
        }
        #endregion

        #region Start Color Spring Methods
        public SpringEvents StartColorEvents => startColorSpring.springEvents;
        public Color GetTargetStartColor() => startColorSpring.GetTargetColor();
        public void SetTargetStartColor(Color target) => startColorSpring.SetTarget(target);
        public Color GetCurrentStartColor() => startColorSpring.GetCurrentColor();
        public void SetCurrentStartColor(Color value) => startColorSpring.SetCurrentValue(value);
        public Vector4 GetVelocityStartColor() => startColorSpring.GetVelocity();
        public void SetVelocityStartColor(Vector4 velocity) => startColorSpring.SetVelocity(velocity);
        public void AddVelocityStartColor(Vector4 velocityToAdd) => startColorSpring.AddVelocity(velocityToAdd);
        public void ReachEquilibriumStartColor() => startColorSpring.ReachEquilibrium();
        public Vector4 GetForceStartColor() => startColorSpring.GetForce();
        public void SetForceStartColor(Vector4 force) => startColorSpring.SetForce(force);
        public void SetForceStartColor(float force) => startColorSpring.SetForce(force);
        public Vector4 GetDragStartColor() => startColorSpring.GetDrag();
        public void SetDragStartColor(Vector4 drag) => startColorSpring.SetDrag(drag);
        public void SetDragStartColor(float drag) => startColorSpring.SetDrag(drag);
        public float GetCommonForceStartColor() => startColorSpring.GetCommonForce();
        public float GetCommonDragStartColor() => startColorSpring.GetCommonDrag();
        public void SetCommonForceStartColor(float force)
        {
            startColorSpring.SetCommonForceAndDrag(true);
            startColorSpring.SetCommonForce(force);
        }
        public void SetCommonDragStartColor(float drag)
        {
            startColorSpring.SetCommonForceAndDrag(true);
            startColorSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragStartColor(float force, float drag)
        {
            SetCommonForceStartColor(force);
            SetCommonDragStartColor(drag);
        }
        #endregion

        #region Simulation Speed Spring Methods
        public SpringEvents SimulationSpeedEvents => simulationSpeedSpring.springEvents;
        public float GetTargetSimulationSpeed() => simulationSpeedSpring.GetTarget();
        public void SetTargetSimulationSpeed(float target) => simulationSpeedSpring.SetTarget(target);
        public float GetCurrentSimulationSpeed() => simulationSpeedSpring.GetCurrentValue();
        public void SetCurrentSimulationSpeed(float value) => simulationSpeedSpring.SetCurrentValue(value);
        public float GetVelocitySimulationSpeed() => simulationSpeedSpring.GetVelocity();
        public void SetVelocitySimulationSpeed(float velocity) => simulationSpeedSpring.SetVelocity(velocity);
        public void AddVelocitySimulationSpeed(float velocityToAdd) => simulationSpeedSpring.AddVelocity(velocityToAdd);
        public void ReachEquilibriumSimulationSpeed() => simulationSpeedSpring.ReachEquilibrium();
        public float GetForceSimulationSpeed() => simulationSpeedSpring.GetForce();
        public void SetForceSimulationSpeed(float force) => simulationSpeedSpring.SetForce(force);
        public float GetDragSimulationSpeed() => simulationSpeedSpring.GetDrag();
        public void SetDragSimulationSpeed(float drag) => simulationSpeedSpring.SetDrag(drag);
        public float GetCommonForceSimulationSpeed() => simulationSpeedSpring.GetCommonForce();
        public float GetCommonDragSimulationSpeed() => simulationSpeedSpring.GetCommonDrag();
        public void SetCommonForceSimulationSpeed(float force)
        {
            simulationSpeedSpring.SetCommonForceAndDrag(true);
            simulationSpeedSpring.SetCommonForce(force);
        }
        public void SetCommonDragSimulationSpeed(float drag)
        {
            simulationSpeedSpring.SetCommonForceAndDrag(true);
            simulationSpeedSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragSimulationSpeed(float force, float drag)
        {
            SetCommonForceSimulationSpeed(force);
            SetCommonDragSimulationSpeed(drag);
        }
        #endregion

        public override void Initialize()
        {
            if (autoUpdatedParticleSystem == null)
            {
                Debug.LogError($"ParticleSystemSpringComponent on {gameObject.name} has no ParticleSystem assigned!");
                return;
            }

            // Cache modules
            mainModule = autoUpdatedParticleSystem.main;
            emissionModule = autoUpdatedParticleSystem.emission;
            emissionModuleEnabled = emissionModule.enabled;

            // Cache emission rate if module is enabled
            if (emissionModuleEnabled)
            {
                cachedEmissionRate = emissionModule.rateOverTime.constant;
            }

            base.Initialize();
        }

        protected override void RegisterSprings()
        {
            if (animateEmissionRate) RegisterSpring(emissionRateSpring);
            if (animateStartLifetime) RegisterSpring(startLifetimeSpring);
            if (animateStartSpeed) RegisterSpring(startSpeedSpring);
            if (animateStartSize) RegisterSpring(startSizeSpring);
            if (animateStartColor) RegisterSpring(startColorSpring);
            if (animateSimulationSpeed) RegisterSpring(simulationSpeedSpring);
        }

        protected override void SetCurrentValueByDefault()
        {
            if (animateEmissionRate && emissionModuleEnabled)
            {
                emissionRateSpring.SetCurrentValue(cachedEmissionRate);
            }

            if (animateStartLifetime)
            {
                startLifetimeSpring.SetCurrentValue(mainModule.startLifetime.constant);
            }

            if (animateStartSpeed)
            {
                startSpeedSpring.SetCurrentValue(mainModule.startSpeed.constant);
            }

            if (animateStartSize)
            {
                startSizeSpring.SetCurrentValue(mainModule.startSize.constant);
            }

            if (animateStartColor)
            {
                startColorSpring.SetCurrentValue(mainModule.startColor.color);
            }

            if (animateSimulationSpeed)
            {
                simulationSpeedSpring.SetCurrentValue(mainModule.simulationSpeed);
            }
        }

        protected override void SetTargetByDefault()
        {
            if (animateEmissionRate && emissionModuleEnabled)
            {
                emissionRateSpring.SetTarget(cachedEmissionRate);
            }

            if (animateStartLifetime)
            {
                startLifetimeSpring.SetTarget(mainModule.startLifetime.constant);
            }

            if (animateStartSpeed)
            {
                startSpeedSpring.SetTarget(mainModule.startSpeed.constant);
            }

            if (animateStartSize)
            {
                startSizeSpring.SetTarget(mainModule.startSize.constant);
            }

            if (animateStartColor)
            {
                startColorSpring.SetTarget(mainModule.startColor.color);
            }

            if (animateSimulationSpeed)
            {
                simulationSpeedSpring.SetTarget(mainModule.simulationSpeed);
            }
        }

        public void Update()
        {
            if (!initialized) return;
            UpdateParticleSystemProperties();
        }

        private void UpdateParticleSystemProperties()
        {
            // Update ParticleSystem properties based on spring values
            if (animateEmissionRate && emissionModuleEnabled)
            {
                var rate = emissionModule.rateOverTime;
                rate.constant = emissionRateSpring.GetCurrentValue();
                emissionModule.rateOverTime = rate;
            }

            if (animateStartLifetime)
            {
                var lifetime = mainModule.startLifetime;
                lifetime.constant = startLifetimeSpring.GetCurrentValue();
                mainModule.startLifetime = lifetime;
            }

            if (animateStartSpeed)
            {
                var speed = mainModule.startSpeed;
                speed.constant = startSpeedSpring.GetCurrentValue();
                mainModule.startSpeed = speed;
            }

            if (animateStartSize)
            {
                var size = mainModule.startSize;
                size.constant = startSizeSpring.GetCurrentValue();
                mainModule.startSize = size;
            }

            if (animateStartColor)
            {
                var color = mainModule.startColor;
                color.color = startColorSpring.GetCurrentColor();
                mainModule.startColor = color;
            }

            if (animateSimulationSpeed)
            {
                mainModule.simulationSpeed = simulationSpeedSpring.GetCurrentValue();
            }
        }

        /// <summary>
        /// Immediately sets all springs to their target values and stops all motion.
        /// </summary>
        public override void ReachEquilibrium()
        {
            base.ReachEquilibrium();
            UpdateParticleSystemProperties();
        }

        public override bool IsValidSpringComponent()
        {
            bool res = true;

            if (autoUpdatedParticleSystem == null)
            {
                AddErrorReason($"{gameObject.name} autoUpdatedParticleSystem is null.");
                res = false;
            }

            // Check if at least one property is selected for animation
            bool anyPropertyAnimated = animateEmissionRate || animateStartLifetime ||
                                     animateStartSpeed || animateStartSize ||
                                     animateStartColor || animateSimulationSpeed;

            if (!anyPropertyAnimated)
            {
                AddErrorReason($"{gameObject.name} has no particle system properties selected to animate.");
                res = false;
            }

            return res;
        }

        // Set animation toggle methods
        public void SetAnimateEmissionRate(bool animate)
        {
            animateEmissionRate = animate;
            if (initialized)
            {
                // Re-initialize to register/unregister the spring
                Initialize();
            }
        }

        public void SetAnimateStartLifetime(bool animate)
        {
            animateStartLifetime = animate;
            if (initialized)
            {
                Initialize();
            }
        }

        public void SetAnimateStartSpeed(bool animate)
        {
            animateStartSpeed = animate;
            if (initialized)
            {
                Initialize();
            }
        }

        public void SetAnimateStartSize(bool animate)
        {
            animateStartSize = animate;
            if (initialized)
            {
                Initialize();
            }
        }

        public void SetAnimateStartColor(bool animate)
        {
            animateStartColor = animate;
            if (initialized)
            {
                Initialize();
            }
        }

        public void SetAnimateSimulationSpeed(bool animate)
        {
            animateSimulationSpeed = animate;
            if (initialized)
            {
                Initialize();
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            if (autoUpdatedParticleSystem == null)
            {
                autoUpdatedParticleSystem = GetComponent<ParticleSystem>();
            }

            // Setup springs with reasonable defaults
            SetupDefaultSpringValues();
        }

        private void SetupDefaultSpringValues()
        {
            // Default values for emission rate spring
            emissionRateSpring.SetCommonForceAndDrag(true);
            emissionRateSpring.SetCommonForce(100f);
            emissionRateSpring.SetCommonDrag(10f);
            emissionRateSpring.SetClampCurrentValue(true);
            emissionRateSpring.SetMinValue(0f);

            // Default values for lifetime spring
            startLifetimeSpring.SetCommonForceAndDrag(true);
            startLifetimeSpring.SetCommonForce(80f);
            startLifetimeSpring.SetCommonDrag(8f);
            startLifetimeSpring.SetClampCurrentValue(true);
            startLifetimeSpring.SetMinValue(0f);

            // Default values for speed spring
            startSpeedSpring.SetCommonForceAndDrag(true);
            startSpeedSpring.SetCommonForce(80f);
            startSpeedSpring.SetCommonDrag(8f);
            startSpeedSpring.SetClampCurrentValue(true);
            startSpeedSpring.SetMinValue(0f);

            // Default values for size spring
            startSizeSpring.SetCommonForceAndDrag(true);
            startSizeSpring.SetCommonForce(80f);
            startSizeSpring.SetCommonDrag(8f);
            startSizeSpring.SetClampCurrentValue(true);
            startSizeSpring.SetMinValue(0f);

            // Default values for color spring
            startColorSpring.SetCommonForceAndDrag(true);
            startColorSpring.SetCommonForce(80f);
            startColorSpring.SetCommonDrag(8f);
            startColorSpring.SetClampCurrentValues(true, true, true, true);
            startColorSpring.SetMinValues(new Vector4(0, 0, 0, 0));
            startColorSpring.SetMaxValues(new Vector4(1, 1, 1, 1));

            // Default values for simulation speed spring
            simulationSpeedSpring.SetCommonForceAndDrag(true);
            simulationSpeedSpring.SetCommonForce(80f);
            simulationSpeedSpring.SetCommonDrag(8f);
            simulationSpeedSpring.SetClampCurrentValue(true);
            simulationSpeedSpring.SetMinValue(0f);
        }

        internal override Spring[] GetSpringsArray()
        {
            // Only return the springs that are being animated
            System.Collections.Generic.List<Spring> springs = new System.Collections.Generic.List<Spring>();

            if (animateEmissionRate) springs.Add(emissionRateSpring);
            if (animateStartLifetime) springs.Add(startLifetimeSpring);
            if (animateStartSpeed) springs.Add(startSpeedSpring);
            if (animateStartSize) springs.Add(startSizeSpring);
            if (animateStartColor) springs.Add(startColorSpring);
            if (animateSimulationSpeed) springs.Add(simulationSpeedSpring);

            return springs.ToArray();
        }
#endif
    }
}
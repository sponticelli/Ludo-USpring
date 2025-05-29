using UnityEngine;
using USpring.Core;
using System.Collections.Generic;

namespace USpring.Components
{
    /// <summary>
    /// Improved AudioSource spring component that manages multiple springs for volume and pitch.
    /// Demonstrates the new architecture with reduced code duplication and better validation.
    /// </summary>
    [AddComponentMenu("Ludo/USpring/Components/Audio Source Spring")]
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceSpringComponent : SpringComponent
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource audioSource;

        [Header("Volume Spring")]
        [SerializeField] private SpringFloat volumeSpring = new SpringFloat();
        [SerializeField] private bool enableVolumeSpring = true;

        [Header("Pitch Spring")]
        [SerializeField] private SpringFloat pitchSpring = new SpringFloat();
        [SerializeField] private bool enablePitchSpring = true;

        [Header("Spring Configuration")]
        [SerializeField] private bool useCommonSettings = true;
        [SerializeField] private float commonForce = 150f;
        [SerializeField] private float commonDrag = 10f;

        [Header("Volume Clamping")]
        [SerializeField] private bool clampVolume = true;
        [SerializeField] private float minVolume = 0f;
        [SerializeField] private float maxVolume = 1f;

        [Header("Pitch Clamping")]
        [SerializeField] private bool clampPitch = true;
        [SerializeField] private float minPitch = 0.1f;
        [SerializeField] private float maxPitch = 3f;

        /// <summary>
        /// Gets the volume spring events.
        /// </summary>
        public SpringEvents VolumeEvents => volumeSpring.springEvents;

        /// <summary>
        /// Gets the pitch spring events.
        /// </summary>
        public SpringEvents PitchEvents => pitchSpring.springEvents;

        /// <summary>
        /// Gets or sets the AudioSource component.
        /// </summary>
        public AudioSource AudioSource
        {
            get => audioSource;
            set
            {
                audioSource = value;
                InvalidateValidation();
            }
        }

        private void Awake()
        {
            // Auto-assign AudioSource if not set
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        public override void Initialize()
        {
            ConfigureSprings();
            base.Initialize();
        }

        private void ConfigureSprings()
        {
            // Configure volume spring
            if (enableVolumeSpring)
            {
                ConfigureVolumeSpring();
            }

            // Configure pitch spring
            if (enablePitchSpring)
            {
                ConfigurePitchSpring();
            }
        }

        private void ConfigureVolumeSpring()
        {
            if (useCommonSettings)
            {
                volumeSpring.SetCommonForceAndDrag(true);
                volumeSpring.SetCommonForce(commonForce);
                volumeSpring.SetCommonDrag(commonDrag);
            }

            if (clampVolume)
            {
                volumeSpring.SetClampingEnabled(true);
                volumeSpring.SetMinValue(minVolume);
                volumeSpring.SetMaxValue(maxVolume);
                volumeSpring.SetClampCurrentValue(true);
                volumeSpring.SetClampTarget(true);
            }
        }

        private void ConfigurePitchSpring()
        {
            if (useCommonSettings)
            {
                pitchSpring.SetCommonForceAndDrag(true);
                pitchSpring.SetCommonForce(commonForce);
                pitchSpring.SetCommonDrag(commonDrag);
            }

            if (clampPitch)
            {
                pitchSpring.SetClampingEnabled(true);
                pitchSpring.SetMinValue(minPitch);
                pitchSpring.SetMaxValue(maxPitch);
                pitchSpring.SetClampCurrentValue(true);
                pitchSpring.SetClampTarget(true);
            }
        }

        protected override void RegisterSprings()
        {
            if (enableVolumeSpring)
            {
                RegisterSpring(volumeSpring);
            }

            if (enablePitchSpring)
            {
                RegisterSpring(pitchSpring);
            }
        }

        protected override void SetCurrentValueByDefault()
        {
            if (audioSource != null)
            {
                if (enableVolumeSpring)
                {
                    volumeSpring.SetCurrentValue(audioSource.volume);
                }

                if (enablePitchSpring)
                {
                    pitchSpring.SetCurrentValue(audioSource.pitch);
                }
            }
        }

        protected override void SetTargetByDefault()
        {
            if (audioSource != null)
            {
                if (enableVolumeSpring)
                {
                    volumeSpring.SetTarget(audioSource.volume);
                }

                if (enablePitchSpring)
                {
                    pitchSpring.SetTarget(audioSource.pitch);
                }
            }
        }

        public override bool IsValidSpringComponent()
        {
            if (audioSource == null)
            {
                AddErrorReason("AudioSource is null");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Immediately sets all springs (volume and pitch) to their target values and stops all motion.
        /// </summary>
        public override void ReachEquilibrium()
        {
            base.ReachEquilibrium();
            UpdateAudioSource();
        }

        public void Update()
        {
            if (!initialized || audioSource == null)
            {
                return;
            }

            UpdateAudioSource();
        }

        private void UpdateAudioSource()
        {
            if (enableVolumeSpring)
            {
                audioSource.volume = volumeSpring.GetCurrentValue();
            }

            if (enablePitchSpring)
            {
                audioSource.pitch = pitchSpring.GetCurrentValue();
            }
        }

        #region Volume Spring API
        public float GetTargetVolume() => volumeSpring.GetTarget();
        /// <summary>
        /// Sets the target volume that the spring will move towards.
        /// </summary>
        /// <param name="target">The new target volume.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent SetTargetVolume(float target)
        {
            volumeSpring.SetTarget(target);
            return this;
        }

        public float GetCurrentValueVolume() => volumeSpring.GetCurrentValue();

        /// <summary>
        /// Sets the current volume of the spring.
        /// </summary>
        /// <param name="value">The new current volume.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent SetCurrentValueVolume(float value)
        {
            volumeSpring.SetCurrentValue(value);
            return this;
        }

        public float GetVelocityVolume() => volumeSpring.GetVelocity();

        /// <summary>
        /// Sets the velocity of the volume spring.
        /// </summary>
        /// <param name="velocity">The new velocity value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent SetVelocityVolume(float velocity)
        {
            volumeSpring.SetVelocity(velocity);
            return this;
        }

        /// <summary>
        /// Adds velocity to the current volume spring velocity.
        /// </summary>
        /// <param name="velocity">The velocity to add.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent AddVelocityVolume(float velocity)
        {
            volumeSpring.AddVelocity(velocity);
            return this;
        }

        /// <summary>
        /// Immediately sets the volume spring to its target value and stops all motion.
        /// </summary>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent ReachEquilibriumVolume()
        {
            volumeSpring.ReachEquilibrium();
            return this;
        }
        public float GetForceVolume() => volumeSpring.GetForce();
        /// <summary>
        /// Sets the force value for the volume spring.
        /// </summary>
        /// <param name="force">The force value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent SetForceVolume(float force)
        {
            volumeSpring.SetForce(force);
            return this;
        }

        public float GetDragVolume() => volumeSpring.GetDrag();

        /// <summary>
        /// Sets the drag value for the volume spring.
        /// </summary>
        /// <param name="drag">The drag value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent SetDragVolume(float drag)
        {
            volumeSpring.SetDrag(drag);
            return this;
        }

        /// <summary>
        /// Sets the minimum value for volume clamping.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent SetMinValuesVolume(float minValue)
        {
            volumeSpring.SetMinValue(minValue);
            return this;
        }

        /// <summary>
        /// Sets the maximum value for volume clamping.
        /// </summary>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent SetMaxValuesVolume(float maxValue)
        {
            volumeSpring.SetMaxValue(maxValue);
            return this;
        }

        /// <summary>
        /// Sets whether to clamp the current volume value.
        /// </summary>
        /// <param name="clamp">Whether to clamp the current value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent SetClampCurrentValuesVolume(bool clamp)
        {
            volumeSpring.SetClampCurrentValue(clamp);
            return this;
        }

        /// <summary>
        /// Sets whether to clamp the target volume value.
        /// </summary>
        /// <param name="clamp">Whether to clamp the target value.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent SetClampTargetVolume(bool clamp)
        {
            volumeSpring.SetClampTarget(clamp);
            return this;
        }

        /// <summary>
        /// Sets whether to stop the volume spring when clamping occurs.
        /// </summary>
        /// <param name="stop">Whether to stop on clamp.</param>
        /// <returns>This component instance for method chaining.</returns>
        public AudioSourceSpringComponent StopSpringOnClampVolume(bool stop)
        {
            volumeSpring.SetStopOnClamp(stop);
            return this;
        }
        public float GetCommonForceVolume() => volumeSpring.GetCommonForce();
        public float GetCommonDragVolume() => volumeSpring.GetCommonDrag();
        public void SetCommonForceVolume(float force)
        {
            volumeSpring.SetCommonForceAndDrag(true);
            volumeSpring.SetCommonForce(force);
        }
        public void SetCommonDragVolume(float drag)
        {
            volumeSpring.SetCommonForceAndDrag(true);
            volumeSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragVolume(float force, float drag)
        {
            SetCommonForceVolume(force);
            SetCommonDragVolume(drag);
        }
        #endregion

        #region Pitch Spring API
        public float GetTargetPitch() => pitchSpring.GetTarget();
        public void SetTargetPitch(float target) => pitchSpring.SetTarget(target);
        public float GetCurrentValuePitch() => pitchSpring.GetCurrentValue();
        public void SetCurrentValuePitch(float value) => pitchSpring.SetCurrentValue(value);
        public float GetVelocityPitch() => pitchSpring.GetVelocity();
        public void SetVelocityPitch(float velocity) => pitchSpring.SetVelocity(velocity);
        public void AddVelocityPitch(float velocity) => pitchSpring.AddVelocity(velocity);
        public void ReachEquilibriumPitch() => pitchSpring.ReachEquilibrium();
        public float GetForcePitch() => pitchSpring.GetForce();
        public void SetForcePitch(float force) => pitchSpring.SetForce(force);
        public float GetDragPitch() => pitchSpring.GetDrag();
        public void SetDragPitch(float drag) => pitchSpring.SetDrag(drag);
        public void SetMinValuesPitch(float minValue) => pitchSpring.SetMinValue(minValue);
        public void SetMaxValuesPitch(float maxValue) => pitchSpring.SetMaxValue(maxValue);
        public void SetClampCurrentValuesPitch(bool clamp) => pitchSpring.SetClampCurrentValue(clamp);
        public void SetClampTargetPitch(bool clamp) => pitchSpring.SetClampTarget(clamp);
        public void StopSpringOnClampPitch(bool stop) => pitchSpring.SetStopOnClamp(stop);
        public float GetCommonForcePitch() => pitchSpring.GetCommonForce();
        public float GetCommonDragPitch() => pitchSpring.GetCommonDrag();
        public void SetCommonForcePitch(float force)
        {
            pitchSpring.SetCommonForceAndDrag(true);
            pitchSpring.SetCommonForce(force);
        }
        public void SetCommonDragPitch(float drag)
        {
            pitchSpring.SetCommonForceAndDrag(true);
            pitchSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragPitch(float force, float drag)
        {
            SetCommonForcePitch(force);
            SetCommonDragPitch(drag);
        }
        #endregion

        #region Spring Properties
        public SpringFloat VolumeSpring
        {
            get => volumeSpring;
            set => volumeSpring = value;
        }

        public SpringFloat PitchSpring
        {
            get => pitchSpring;
            set => pitchSpring = value;
        }
        #endregion

        #region Configuration API
        public void SetCommonForceAndDrag(float force, float drag)
        {
            commonForce = force;
            commonDrag = drag;

            if (useCommonSettings)
            {
                ConfigureSprings();
            }
        }

        public void EnableVolumeSpring(bool enable)
        {
            enableVolumeSpring = enable;
            InvalidateValidation();
        }

        public void EnablePitchSpring(bool enable)
        {
            enablePitchSpring = enable;
            InvalidateValidation();
        }
        #endregion

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        internal override Spring[] GetSpringsArray()
        {
            var springs = new List<Spring>();

            if (enableVolumeSpring)
            {
                springs.Add(volumeSpring);
            }

            if (enablePitchSpring)
            {
                springs.Add(pitchSpring);
            }

            return springs.ToArray();
        }
#endif
    }
}
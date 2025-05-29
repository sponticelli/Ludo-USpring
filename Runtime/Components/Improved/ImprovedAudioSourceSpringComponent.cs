using UnityEngine;
using USpring.Core;
using System.Collections.Generic;

namespace USpring.Components.Improved
{
    /// <summary>
    /// Improved AudioSource spring component that manages multiple springs for volume and pitch.
    /// Demonstrates the new architecture with reduced code duplication and better validation.
    /// </summary>
    [AddComponentMenu("Ludo/USpring/Components/Improved/Audio Source Spring")]
    [RequireComponent(typeof(AudioSource))]
    public class ImprovedAudioSourceSpringComponent : SpringComponent
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
            var errorMessages = new List<string>();
            bool isValid = SpringComponentValidator.ValidateAudioSource(audioSource, "AudioSource", errorMessages);

            if (!isValid)
            {
                foreach (var error in errorMessages)
                {
                    AddErrorReason(error);
                }
            }

            return isValid;
        }

        protected virtual void Update()
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
        public float GetVolumeTarget() => volumeSpring.GetTarget();
        public void SetVolumeTarget(float target) => volumeSpring.SetTarget(target);
        public float GetVolumeCurrentValue() => volumeSpring.GetCurrentValue();
        public void SetVolumeCurrentValue(float value) => volumeSpring.SetCurrentValue(value);
        public float GetVolumeVelocity() => volumeSpring.GetVelocity();
        public void SetVolumeVelocity(float velocity) => volumeSpring.SetVelocity(velocity);
        public void AddVolumeVelocity(float velocity) => volumeSpring.AddVelocity(velocity);
        public void ReachVolumeEquilibrium() => volumeSpring.ReachEquilibrium();
        #endregion

        #region Pitch Spring API
        public float GetPitchTarget() => pitchSpring.GetTarget();
        public void SetPitchTarget(float target) => pitchSpring.SetTarget(target);
        public float GetPitchCurrentValue() => pitchSpring.GetCurrentValue();
        public void SetPitchCurrentValue(float value) => pitchSpring.SetCurrentValue(value);
        public float GetPitchVelocity() => pitchSpring.GetVelocity();
        public void SetPitchVelocity(float velocity) => pitchSpring.SetVelocity(velocity);
        public void AddPitchVelocity(float velocity) => pitchSpring.AddVelocity(velocity);
        public void ReachPitchEquilibrium() => pitchSpring.ReachEquilibrium();
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

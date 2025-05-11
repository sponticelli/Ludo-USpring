using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Audio Source Spring")]
	public partial class AudioSourceSpringComponent : SpringComponent
	{
        [SerializeField] private AudioSource autoUpdatedAudioSource;

		[SerializeField] private SpringFloat volumeSpring = new SpringFloat();
		[SerializeField] private SpringFloat pitchSpring = new SpringFloat();

		public SpringEvents VolumeEvents => volumeSpring.springEvents;
		public float GetTargetVolume() => volumeSpring.GetTarget();
		public void SetTargetVolume(float target) => volumeSpring.SetTarget(target);
		public float GetCurrentValueVolume() => volumeSpring.GetCurrentValue();
		public void SetCurrentValueVolume(float currentValues) => volumeSpring.SetCurrentValue(currentValues);
		public float GetVelocityVolume() => volumeSpring.GetVelocity();
		public void SetVelocityVolume(float velocity) => volumeSpring.SetVelocity(velocity);
		public void AddVelocityVolume(float velocityToAdd) =>	volumeSpring.AddVelocity(velocityToAdd);
		public void ReachEquilibriumVolume() => volumeSpring.ReachEquilibrium();
		public float GetForceVolume() => volumeSpring.GetForce();
		public void SetForceVolume(float force) => volumeSpring.SetForce(force);
		public float GetDragVolume() => volumeSpring.GetDrag();
		public void SetDragVolume(float drag) => volumeSpring.SetDrag(drag);
		public void SetMinValuesVolume(float minValue) => volumeSpring.SetMinValue(minValue);
		public void SetMaxValuesVolume(float maxValue) => volumeSpring.SetMaxValue(maxValue);
		public void SetClampCurrentValuesVolume(bool clamp) => volumeSpring.SetClampCurrentValue(clamp);
		public void SetClampTargetVolume(bool clamp) => volumeSpring.SetClampTarget(clamp);
		public void StopSpringOnClampVolume(bool stop) => volumeSpring.SetStopOnClamp(stop);
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
		
		
		public SpringEvents PitchEvents => pitchSpring.springEvents;
		public float GetTargetPitch() => pitchSpring.GetTarget();
		public void SetTargetPitch(float target) => pitchSpring.SetTarget(target);
		public float GetCurrentValuePitch() => pitchSpring.GetCurrentValue();
		public void SetCurrentValuePitch(float currentValues) => pitchSpring.SetCurrentValue(currentValues);
		public float GetVelocityPitch() => pitchSpring.GetVelocity();
		public void SetVelocityPitch(float velocity) => pitchSpring.SetVelocity(velocity);
		public void AddVelocityPitch(float velocityToAdd) =>	pitchSpring.AddVelocity(velocityToAdd);
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
		
		
		protected override void RegisterSprings()
		{
			RegisterSpring(volumeSpring);
			RegisterSpring(pitchSpring);
		}

		protected override void SetCurrentValueByDefault()
		{
			SetCurrentValueVolume(autoUpdatedAudioSource.volume);
			SetCurrentValuePitch(autoUpdatedAudioSource.pitch);
		}

		protected override void SetTargetByDefault()
		{
			SetTargetVolume(autoUpdatedAudioSource.volume);
			SetTargetPitch(autoUpdatedAudioSource.pitch);
		}

		public void Update()
		{
			if (!initialized) { return; }

			UpdateAudioSource();
		}

		private void UpdateAudioSource()
		{
			autoUpdatedAudioSource.volume = volumeSpring.GetCurrentValue();
			autoUpdatedAudioSource.pitch = pitchSpring.GetCurrentValue();
		}

		public override bool IsValidSpringComponent()
		{
			bool res = true;

			if (autoUpdatedAudioSource == null)
			{
				AddErrorReason($"{gameObject.name} autoUpdatedAudioSource is null.");
				res = false;
			}

			return res;
		}
		
		#region ENABLE/DISABLE SPRING PROPERTIES
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

#if UNITY_EDITOR
		protected override void Reset()
		{
			base.Reset();

			if(autoUpdatedAudioSource == null)
			{
				autoUpdatedAudioSource = GetComponent<AudioSource>();
			}
		}

		internal override Spring[] GetSpringsArray()
		{
			Spring[] res = new Spring[]
			{
				volumeSpring, pitchSpring
			};

			return res;
		}
#endif
	}
}
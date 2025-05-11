using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Light Intensity Spring")]
	public partial class LightIntensitySpringComponent : SpringComponent
	{
		[SerializeField] private SpringFloat lightIntensitySpring = new SpringFloat();

		[SerializeField] private Light autoUpdatedLight;

		
		public SpringEvents Events => lightIntensitySpring.springEvents;
		public float GetTarget() => lightIntensitySpring.GetTarget();
		public void SetTarget(float target) => lightIntensitySpring.SetTarget(target);
		public float GetCurrentValue() => lightIntensitySpring.GetCurrentValue();
		public void SetCurrentValue(float currentValues) => lightIntensitySpring.SetCurrentValue(currentValues);
		public float GetVelocity() => lightIntensitySpring.GetVelocity();
		public void SetVelocity(float velocity) => lightIntensitySpring.SetVelocity(velocity);
		public void AddVelocity(float velocityToAdd) =>	lightIntensitySpring.AddVelocity(velocityToAdd);
		public float GetForce() => lightIntensitySpring.GetForce();
		public void SetForce(float force) => lightIntensitySpring.SetForce(force);
		public float GetDrag() => lightIntensitySpring.GetDrag();
		public void SetDrag(float drag) => lightIntensitySpring.SetDrag(drag);
		public void SetMinValues(float minValue) => lightIntensitySpring.SetMinValue(minValue);
		public void SetMaxValues(float maxValue) => lightIntensitySpring.SetMaxValue(maxValue);
		public void SetClampCurrentValues(bool clamp) => lightIntensitySpring.SetClampCurrentValue(clamp);
		public void SetClampTarget(bool clamp) => lightIntensitySpring.SetClampTarget(clamp);
		public void StopSpringOnClamp(bool stop) => lightIntensitySpring.SetStopOnClamp(stop);
		public float GetCommonForce() => lightIntensitySpring.GetCommonForce();
		public float GetCommonDrag() => lightIntensitySpring.GetCommonDrag();
		public void SetCommonForce(float force)
		{
			lightIntensitySpring.SetCommonForceAndDrag(true);
			lightIntensitySpring.SetCommonForce(force);
		}
		public void SetCommonDrag(float drag)
		{
			lightIntensitySpring.SetCommonForceAndDrag(true);
			lightIntensitySpring.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
		}
		
		protected override void RegisterSprings()
		{
			RegisterSpring(lightIntensitySpring);
		}

		protected override void SetCurrentValueByDefault()
		{
			SetCurrentValue(autoUpdatedLight.intensity);
		}

		protected override void SetTargetByDefault()
		{
			SetTarget(autoUpdatedLight.intensity);
		}

		public void Update()
		{
			if (!initialized) { return; }

			UpdateLightIntensity();
		}

		public void UpdateLightIntensity()
		{
			autoUpdatedLight.intensity = GetCurrentValue();
		}

		public override bool IsValidSpringComponent()
		{
			bool res = true;

			if (autoUpdatedLight == null)
			{
				AddErrorReason($"{gameObject.name} autoUpdatedLight is null.");
				res = false;
			}
			
			return res;
		}

#if UNITY_EDITOR
		protected override void Reset()
		{
			base.Reset();

			if(autoUpdatedLight == null)
			{
				autoUpdatedLight = GetComponent<Light>();
			}
		}

		internal override Spring[] GetSpringsArray()
		{
			Spring[] res = new Spring[]
			{
				lightIntensitySpring
			};

			return res;
		}
#endif
	}
}
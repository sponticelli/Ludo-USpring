using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
	//Broad purpose spring class that can be used to animate any float value, we can then use this spring values to feed them to Reactor components
	[AddComponentMenu("Ludo/USpring/Components/Float Spring")]
	public partial class FloatSpringComponent : SpringComponent
	{
		[SerializeField] private SpringFloat springFloat = new SpringFloat();
		
		public SpringEvents Events => springFloat.springEvents;
		public float GetTarget() => springFloat.GetTarget();
		public void SetTarget(float target) => springFloat.SetTarget(target);
		public float GetCurrentValue() => springFloat.GetCurrentValue();
		public void SetCurrentValue(float currentValues) => springFloat.SetCurrentValue(currentValues);
		public float GetVelocity() => springFloat.GetVelocity();
		public void SetVelocity(float velocity) => springFloat.SetVelocity(velocity);
		public void AddVelocity(float velocityToAdd) =>	springFloat.AddVelocity(velocityToAdd);
		public float GetForce() => springFloat.GetForce();
		public void SetForce(float force) => springFloat.SetForce(force);
		public float GetDrag() => springFloat.GetDrag();
		public void SetDrag(float drag) => springFloat.SetDrag(drag);
		public void SetMinValues(float minValue) => springFloat.SetMinValue(minValue);
		public void SetMaxValues(float maxValue) => springFloat.SetMaxValue(maxValue);
		public void SetClampCurrentValues(bool clamp) => springFloat.SetClampCurrentValue(clamp);
		public void SetClampTarget(bool clamp) => springFloat.SetClampTarget(clamp);
		public void StopSpringOnClamp(bool stop) => springFloat.SetStopOnClamp(stop);
		public float GetCommonForce() => springFloat.GetCommonForce();
		public float GetCommonDrag() => springFloat.GetCommonDrag();
		public void SetCommonForce(float force)
		{
			springFloat.SetCommonForceAndDrag(true);
			springFloat.SetCommonForce(force);
		}
		public void SetCommonDrag(float drag)
		{
			springFloat.SetCommonForceAndDrag(true);
			springFloat.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
		}

		
		protected override void RegisterSprings()
		{
			RegisterSpring(springFloat);
		}

		protected override void SetCurrentValueByDefault()
		{
			springFloat.SetCurrentValue(0f);
		}

		protected override void SetTargetByDefault()
		{
			springFloat.SetTarget(0f);
		}

		public override bool IsValidSpringComponent()
		{
			//No direct dependencies
			return true;
		}

#if UNITY_EDITOR
		internal override Spring[] GetSpringsArray()
		{
			Spring[] res = new Spring[]
			{
				springFloat
			};

			return res;
		}
#endif

	}
}
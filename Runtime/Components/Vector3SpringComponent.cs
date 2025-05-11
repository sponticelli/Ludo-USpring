using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Vector3 Spring")]
	public partial class Vector3SpringComponent : SpringComponent
	{
		[SerializeField] private SpringVector3 springVector3 = new SpringVector3();

		public SpringEvents Events => springVector3.springEvents;
		public Vector3 GetTarget() => springVector3.GetTarget();
		public void SetTarget(Vector3 target) => springVector3.SetTarget(target);
		public void SetTarget(float target) => SetTarget(Vector3.one * target);
		public Vector3 GetCurrentValue() => springVector3.GetCurrentValue();
		public void SetCurrentValue(Vector3 currentValues) => springVector3.SetCurrentValue(currentValues);
		public void SetCurrentValue(float currentValues) => SetCurrentValue(Vector3.one * currentValues);
		public Vector3 GetVelocity() => springVector3.GetVelocity();
		public void SetVelocity(Vector3 velocity) => springVector3.SetVelocity(velocity);
		public void SetVelocity(float velocity) => SetVelocity(Vector3.one * velocity);
		public void AddVelocity(Vector3 velocityToAdd) =>	springVector3.AddVelocity(velocityToAdd);
		public Vector3 GetForce() => springVector3.GetForce();
		public void SetForce(Vector3 force) => springVector3.SetForce(force);
		public void SetForce(float force) => SetForce(Vector3.one * force);
		public Vector3 GetDrag() => springVector3.GetDrag();
		public void SetDrag(Vector3 drag) => springVector3.SetDrag(drag);
		public void SetDrag(float drag) => SetDrag(Vector3.one * drag);
		public float GetCommonForce() => springVector3.GetCommonForce();
		public float GetCommonDrag() => springVector3.GetCommonDrag();
		public void SetCommonForce(float force)
		{
			springVector3.SetCommonForceAndDrag(true);
			springVector3.SetCommonForce(force);
		}
		public void SetCommonDrag(float drag)
		{
			springVector3.SetCommonForceAndDrag(true);
			springVector3.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
		}
		public void SetMinValues(Vector3 minValue) => springVector3.SetMinValues(minValue);
		public void SetMinValues(float minValue) => SetMinValues(Vector3.one * minValue);
		public void SetMaxValues(Vector3 maxValue) => springVector3.SetMaxValues(maxValue);
		public void SetMaxValues(float maxValue) => SetMaxValues(Vector3.one * maxValue);
		public void SetClampCurrentValues(bool clampTargetX, bool clampTargetY, bool clampTargetZ) => springVector3.SetClampCurrentValues(clampTargetX, clampTargetY, clampTargetZ);
		public void SetClampTarget(bool clampTargetX, bool clampTargetY, bool clampTargetZ) => springVector3.SetClampTarget(clampTargetX, clampTargetY, clampTargetZ);
		public void StopSpringOnClamp(bool stopX, bool stopY, bool stopZ) => springVector3.StopSpringOnClamp(stopX, stopY, stopZ);
		
		
		protected override void RegisterSprings()
		{
			RegisterSpring(springVector3);
		}

		protected override void SetCurrentValueByDefault()
		{
			SetCurrentValue(Vector3.zero);
		}

		protected override void SetTargetByDefault()
		{
			SetTarget(Vector3.zero);
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
				springVector3
			};

			return res;
		}
#endif
	}
}
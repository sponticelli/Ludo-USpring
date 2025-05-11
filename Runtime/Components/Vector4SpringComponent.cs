using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Vector4 Spring")]
	public partial class Vector4SpringComponent : SpringComponent
	{
		[SerializeField] private SpringVector4 springVector4 = new SpringVector4();

		public SpringEvents Events => springVector4.springEvents;
		public Vector4 GetTarget() => springVector4.GetTarget();
		public void SetTarget(Vector4 target) => springVector4.SetTarget(target);
		public void SetTarget(float target) => SetTarget(Vector4.one * target);
		public Vector4 GetCurrentValue() => springVector4.GetCurrentValue();
		public void SetCurrentValue(Vector4 currentValues) => springVector4.SetCurrentValue(currentValues);
		public void SetCurrentValue(float currentValues) => SetCurrentValue(Vector4.one * currentValues);
		public Vector4 GetVelocity() => springVector4.GetVelocity();
		public void SetVelocity(Vector4 velocity) => springVector4.SetVelocity(velocity);
		public void SetVelocity(float velocity) => SetVelocity(Vector4.one * velocity);
		public void AddVelocity(Vector4 velocityToAdd) =>	springVector4.AddVelocity(velocityToAdd);
		public Vector4 GetForce() => springVector4.GetForce();
		public void SetForce(Vector4 force) => springVector4.SetForce(force);
		public void SetForce(float force) => SetForce(Vector4.one * force);
		public Vector4 GetDrag() => springVector4.GetDrag();
		public void SetDrag(Vector4 drag) => springVector4.SetDrag(drag);
		public void SetDrag(float drag) => SetDrag(Vector4.one * drag);
		public float GetCommonForce() => springVector4.GetCommonForce();
		public float GetCommonDrag() => springVector4.GetCommonDrag();
		public void SetCommonForce(float force)
		{
			springVector4.SetCommonForceAndDrag(true);
			springVector4.SetCommonForce(force);
		}
		public void SetCommonDrag(float drag)
		{
			springVector4.SetCommonForceAndDrag(true);
			springVector4.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
		}
		public void SetMinValues(Vector4 minValue) => springVector4.SetMinValues(minValue);
		public void SetMinValues(float minValue) => SetMinValues(Vector4.one * minValue);
		public void SetMaxValues(Vector4 maxValue) => springVector4.SetMaxValues(maxValue);
		public void SetMaxValues(float maxValue) => SetMaxValues(Vector4.one * maxValue);
		public void SetClampCurrentValues(bool clampTargetX, bool clampTargetY, bool clampTargetZ, bool clampTargetW) => springVector4.SetClampCurrentValues(clampTargetX, clampTargetY, clampTargetZ, clampTargetW);
		public void SetClampTarget(bool clampTargetX, bool clampTargetY, bool clampTargetZ, bool clampTargetW) => springVector4.SetClampTarget(clampTargetX, clampTargetY, clampTargetZ, clampTargetW);
		public void StopSpringOnClamp(bool stopX, bool stopY, bool stopZ, bool stopW) => springVector4.StopSpringOnClamp(stopX, stopY, stopZ, stopW);


		protected override void RegisterSprings()
		{
			RegisterSpring(springVector4);
		}

		protected override void SetCurrentValueByDefault()
		{
			SetCurrentValue(Vector4.zero);
		}

		protected override void SetTargetByDefault()
		{
			SetTarget(Vector4.zero);
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
				springVector4
			};

			return res;
		}
#endif
	}
}
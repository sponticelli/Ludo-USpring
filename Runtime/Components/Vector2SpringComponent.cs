using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Vector2 Spring")]
	public partial class Vector2SpringComponent : SpringComponent
	{
		[SerializeField] private SpringVector2 springVector2 = new SpringVector2(); 

		public SpringEvents Events => springVector2.springEvents;
		public Vector2 GetTarget() => springVector2.GetTarget();
		public void SetTarget(Vector2 target) => springVector2.SetTarget(target);
		public void SetTarget(float target) => SetTarget(Vector2.one * target);
		public Vector2 GetCurrentValue() => springVector2.GetCurrentValue();
		public void SetCurrentValue(Vector2 currentValues) => springVector2.SetCurrentValue(currentValues);
		public void SetCurrentValue(float currentValues) => SetCurrentValue(Vector2.one * currentValues);
		public Vector2 GetVelocity() => springVector2.GetVelocity();
		public void SetVelocity(Vector2 velocity) => springVector2.SetVelocity(velocity);
		public void SetVelocity(float velocity) => SetVelocity(Vector2.one * velocity);
		public void AddVelocity(Vector2 velocityToAdd) =>	springVector2.AddVelocity(velocityToAdd);
		public Vector2 GetForce() => springVector2.GetForce();
		public void SetForce(Vector2 force) => springVector2.SetForce(force);
		public void SetForce(float force) => SetForce(Vector2.one * force);
		public Vector2 GetDrag() => springVector2.GetDrag();
		public void SetDrag(Vector2 drag) => springVector2.SetDrag(drag);
		public void SetDrag(float drag) => SetDrag(Vector2.one * drag);
		public float GetCommonForce() => springVector2.GetCommonForce();
		public float GetCommonDrag() => springVector2.GetCommonDrag();
		public void SetCommonForce(float force)
		{
			springVector2.SetCommonForceAndDrag(true);
			springVector2.SetCommonForce(force);
		}
		public void SetCommonDrag(float drag)
		{
			springVector2.SetCommonForceAndDrag(true);
			springVector2.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
		}
		public void SetMinValues(Vector2 minValue) => springVector2.SetMinValue(minValue);
		public void SetMinValues(float minValue) => SetMinValues(Vector2.one * minValue);
		public void SetMaxValues(Vector2 maxValue) => springVector2.SetMaxValues(maxValue);
		public void SetMaxValues(float maxValue) => SetMaxValues(Vector2.one * maxValue);
		public void SetClampCurrentValues(bool clampTargetX, bool clampTargetY) => springVector2.SetClampCurrentValues(clampTargetX, clampTargetY);
		public void SetClampTarget(bool clampTargetX, bool clampTargetY) => springVector2.SetClampTarget(clampTargetX, clampTargetY);
		public void StopSpringOnClamp(bool stopX, bool stopY) => springVector2.StopSpringOnClamp(stopX, stopY);
		
		
		protected override void RegisterSprings()
		{
			RegisterSpring(springVector2);
		}

		protected override void SetCurrentValueByDefault()
		{
			SetCurrentValue(Vector2.zero);
		}

		protected override void SetTargetByDefault()
		{
			SetTarget(Vector2.zero);
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
				springVector2
			};

			return res;
		}
#endif
	}
}
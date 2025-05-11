using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Rotation Spring")]
	public partial class RotationSpringComponent : SpringComponent
	{
		[SerializeField] private SpringRotation springRotation = new SpringRotation();

		public SpringEvents Events => springRotation.springEvents;
		public Quaternion GetTarget() => springRotation.GetTarget();
		public void SetTarget(Quaternion targetQuaternion)
		{
			springRotation.SetTarget(targetQuaternion);
		}
		public void SetTarget(Vector3 targetEuler)
		{
			springRotation.SetTarget(targetEuler);
		}
		public Quaternion GetCurrentValue() => springRotation.GetCurrentValue();
		public void SetCurrentValue(Quaternion currentQuaternion) => springRotation.SetCurrentValue(currentQuaternion);
		public void SetCurrentValue(Vector3 currentEuler) => springRotation.SetCurrentValue(currentEuler);
		public Vector3 GetVelocity() => springRotation.GetVelocity();
		public void SetVelocity(Vector3 velocity) => springRotation.SetVelocity(velocity);
		public void AddVelocity(Vector3 velocityToAdd) => springRotation.AddVelocity(velocityToAdd);
		public float GetCommonForce() => springRotation.GetCommonForce();
		public float GetCommonDrag() => springRotation.GetCommonDrag();
		public void SetCommonForce(float force)
		{
			springRotation.SetCommonForceAndDrag(true);
			springRotation.SetCommonForce(force);
		}
		public void SetCommonDrag(float drag)
		{
			springRotation.SetCommonForceAndDrag(true);
			springRotation.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
		}
		
		public override bool IsValidSpringComponent()
		{
			//No direct dependencies
			return true;
		}

		protected override void SetCurrentValueByDefault()
		{
			SetCurrentValue(Quaternion.identity);
		}

		protected override void SetTargetByDefault()
		{
			SetTarget(Quaternion.identity);
		}

		protected override void RegisterSprings()
		{
			RegisterSpring(springRotation);
		}

#if UNITY_EDITOR
		internal override Spring[] GetSpringsArray()
		{
			Spring[] res = new Spring[]
			{
				springRotation
			};

			return res;
		}
#endif
	}
}
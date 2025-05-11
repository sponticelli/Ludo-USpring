using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Rigidbody Spring")]
	public partial class RigidbodySpringComponent : SpringComponent
	{
		private const float VELOCITY_FACTOR = 10f;

		[SerializeField] private SpringVector3 positionSpring = new SpringVector3();
		[SerializeField] private SpringRotation rotationSpring = new SpringRotation();

		public bool useTransformAsTarget;

        [SerializeField] private Rigidbody rigidBodyFollower;
		[SerializeField] private Transform target;

		
		public SpringEvents PositionEvents => positionSpring.springEvents;
		public Vector3 GetTargetPosition() => positionSpring.GetTarget();
		public void SetTargetPosition(Vector3 target) => positionSpring.SetTarget(target);
		public void SetTargetPosition(float target) => SetTargetPosition(Vector3.one * target);
		public Vector3 GetCurrentValuePosition() => positionSpring.GetCurrentValue();
		public void SetCurrentValuePosition(Vector3 currentValues) => positionSpring.SetCurrentValue(currentValues);
		public void SetCurrentValuePosition(float currentValues) => SetCurrentValuePosition(Vector3.one * currentValues);
		public Vector3 GetVelocityPosition() => positionSpring.GetVelocity();
		public void SetVelocityPosition(Vector3 velocity) => positionSpring.SetVelocity(velocity);
		public void SetVelocityPosition(float velocity) => SetVelocityPosition(Vector3.one * velocity);
		public void AddVelocityPosition(Vector3 velocityToAdd) =>	positionSpring.AddVelocity(velocityToAdd);
		public void ReachEquilibriumPosition() => positionSpring.ReachEquilibrium();
		public Vector3 GetForcePosition() => positionSpring.GetForce();
		public void SetForcePosition(Vector3 force) => positionSpring.SetForce(force);
		public void SetForcePosition(float force) => SetForcePosition(Vector3.one * force);
		public Vector3 GetDragPosition() => positionSpring.GetDrag();
		public void SetDragPosition(Vector3 drag) => positionSpring.SetDrag(drag);
		public void SetDragPosition(float drag) => SetDragPosition(Vector3.one * drag);
		public float GetCommonForcePosition() => positionSpring.GetCommonForce();
		public float GetCommonDragPosition() => positionSpring.GetCommonDrag();
		public void SetCommonForcePosition(float force)
		{
			positionSpring.SetCommonForceAndDrag(true);
			positionSpring.SetCommonForce(force);
		}
		public void SetCommonDragPosition(float drag)
		{
			positionSpring.SetCommonForceAndDrag(true);
			positionSpring.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDragPosition(float force, float drag)
		{
			SetCommonForcePosition(force);
			SetCommonDragPosition(drag);
		}
		public void SetMinValuesPosition(Vector3 minValue) => positionSpring.SetMinValues(minValue);
		public void SetMinValuesPosition(float minValue) => SetMinValuesPosition(Vector3.one * minValue);
		public void SetMaxValuesPosition(Vector3 maxValue) => positionSpring.SetMaxValues(maxValue);
		public void SetMaxValuesPosition(float maxValue) => SetMaxValuesPosition(Vector3.one * maxValue);
		public void SetClampCurrentValuesPosition(bool clampTargetX, bool clampTargetY, bool clampTargetZ) => positionSpring.SetClampCurrentValues(clampTargetX, clampTargetY, clampTargetZ);
		public void SetClampTargetPosition(bool clampTargetX, bool clampTargetY, bool clampTargetZ) => positionSpring.SetClampTarget(clampTargetX, clampTargetY, clampTargetZ);
		public void StopSpringOnClampPosition(bool stopX, bool stopY, bool stopZ) => positionSpring.StopSpringOnClamp(stopX, stopY, stopZ);
		
		
		public SpringEvents RotationEvents => rotationSpring.springEvents;
		public Quaternion GetTargetRotation() => rotationSpring.GetTarget();
		public void SetTargetRotation(Quaternion targetQuaternion)
		{
			rotationSpring.SetTarget(targetQuaternion);
		}
		public void SetTargetRotation(Vector3 targetEuler)
		{
			rotationSpring.SetTarget(targetEuler);
		}
		public Quaternion GetCurrentValueRotation() => rotationSpring.GetCurrentValue();
		public void SetCurrentValueRotation(Quaternion currentQuaternion) => rotationSpring.SetCurrentValue(currentQuaternion);
		public void SetCurrentValueRotation(Vector3 currentEuler) => rotationSpring.SetCurrentValue(currentEuler);
		public Vector3 GetVelocityRotation() => rotationSpring.GetVelocity();
		public void SetVelocityRotation(Vector3 velocity) => rotationSpring.SetVelocity(velocity);
		public void AddVelocityRotation(Vector3 velocityToAdd) => rotationSpring.AddVelocity(velocityToAdd);
		public void ReachEquilibriumRotation() => rotationSpring.ReachEquilibrium();
		public float GetCommonForceRotation() => rotationSpring.GetCommonForce();
		public float GetCommonDragRotation() => rotationSpring.GetCommonDrag();
		public void SetCommonForceRotation(float force)
		{
			rotationSpring.SetCommonForceAndDrag(true);
			rotationSpring.SetCommonForce(force);
		}
		public void SetCommonDragRotation(float drag)
		{
			rotationSpring.SetCommonForceAndDrag(true);
			rotationSpring.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDragRotation(float force, float drag)
		{
			SetCommonForceRotation(force);
			SetCommonDragRotation(drag);
		}
		
		private Vector3 positionTarget;
		private Quaternion rotationTarget;

		protected override void SetInitialValues()
		{
			if (!hasCustomInitialValues)
			{
				SetCurrentValueByDefault();
			}
			else
			{
				if (!positionSpring.useInitialValues)
				{
					SetCurrentValuePositionByDefault();
				}
				if (!rotationSpring.useInitialValues)
				{
					SetCurrentValueRotationByDefault();
				}
			}

			if (useTransformAsTarget)
			{
				UpdateTarget();
			}
			else
			{
				if (!hasCustomTarget)
				{
					SetTargetByDefault();
				}
				else
				{
					if (!positionSpring.useCustomTarget)
					{
						SetTargetPositionByDefault();
					}
					if (!rotationSpring.useCustomTarget)
					{
						SetTargetRotationByDefault();
					}
				}
			}
		}

		protected override void SetCurrentValueByDefault()
		{
			SetCurrentValuePositionByDefault();
			SetCurrentValueRotationByDefault();
		}

		private void SetCurrentValuePositionByDefault()
		{
			SetCurrentValuePosition(rigidBodyFollower.position);
		}

		private void SetCurrentValueRotationByDefault()
		{
			SetCurrentValueRotation(rigidBodyFollower.rotation);
		}

		protected override void SetTargetByDefault()
		{
			SetTargetPositionByDefault();
			SetTargetRotationByDefault();
		}

		private void SetTargetPositionByDefault()
		{
			SetTargetPosition(rigidBodyFollower.position);
			positionTarget = GetTargetPosition();
		}

		private void SetTargetRotationByDefault()
		{
			SetTargetRotation(rigidBodyFollower.rotation);
			rotationTarget = GetTargetRotation();
		}

		protected override void RegisterSprings()
		{
			RegisterSpring(positionSpring);
			RegisterSpring(rotationSpring);
		}

		public void Update()
		{
			if (!initialized) { return; }
			UpdateTarget();
		}

        private void FixedUpdate()
        {
			if (!initialized) { return; }

			if (positionSpring.springEnabled)
			{
				Vector3 velocity = (positionSpring.GetCurrentValue() - rigidBodyFollower.position) * VELOCITY_FACTOR;
				rigidBodyFollower.linearVelocity = velocity;
			}

			if (rotationSpring.springEnabled)
			{
				Quaternion targetRotation = rotationSpring.GetCurrentValue();
				ApplyTorqueTowards(targetRotation);
			}
		}
        
        private void ApplyTorqueTowards(Quaternion targetRotation)
        {
	        Quaternion rotationDifference = targetRotation * Quaternion.Inverse(rigidBodyFollower.rotation);
	        rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);
    
	        // Ensure the angle is between -180 and 180 degrees
	        if (angle > 180f)
	        {
		        angle -= 360f;
	        }
    
	        Vector3 angularVelocity = (angle * Mathf.Deg2Rad / Time.fixedDeltaTime) * axis.normalized;
	        rigidBodyFollower.angularVelocity = angularVelocity;
        }

		private void UpdateTarget()
		{
			if (useTransformAsTarget)
			{
				positionTarget = target.position;
				rotationTarget = target.rotation;
			}

			if (positionSpring.springEnabled)
			{
				positionSpring.SetTarget(positionTarget);
			}

			if (rotationSpring.springEnabled)
			{
				rotationSpring.SetTarget(rotationTarget);
			}
		}
		
		public override bool IsValidSpringComponent()
		{
			bool res = true;

			if (useTransformAsTarget && target == null)
			{
				AddErrorReason($"{gameObject.name} useTransformAsTarget is enabled but target cannot be null");
				res = false;
			}
			if (rigidBodyFollower == null)
			{
				AddErrorReason($"{gameObject.name} rigidBodyFollower cannot be null");
				res = false;
			}

			return res;
		}

		#region ENABLE/DISABLE SPRING PROPERTIES
		public SpringVector3 PositionSpring
		{
			get => positionSpring;
			set => positionSpring = value;
		}

		public SpringRotation RotationSpring
		{
			get => rotationSpring;
			set => rotationSpring = value;
		}
		#endregion

#if UNITY_EDITOR
		private void OnValidate()
		{
			if(rigidBodyFollower == null)
			{
				rigidBodyFollower = GetComponent<Rigidbody>();
			}
		}

		internal override Spring[] GetSpringsArray()
		{
			Spring[] res = new Spring[]
			{
				positionSpring,
				rotationSpring
			};

			return res;
		}
#endif
	}
}
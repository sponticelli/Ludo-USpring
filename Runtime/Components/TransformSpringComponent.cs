using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Transform Spring")] 
	public partial class TransformSpringComponent : SpringComponent
	{
		public enum SpaceType
		{
			WorldSpace,
			LocalSpace,
		}
		
		public SpaceType spaceType;

		[SerializeField] protected SpringVector3 positionSpring = new SpringVector3();
		[SerializeField] protected SpringVector3 scaleSpring = new SpringVector3();
		[SerializeField] protected SpringRotation rotationSpring = new SpringRotation();

		[Tooltip("Follower Transform will be set to match the spring values every frame")]
		public Transform followerTransform;

		[Tooltip("When enabled, the Target position, rotation, and scale this is TargetTransform. When disabled, you must manually set targets via code")]
		public bool useTransformAsTarget;
		public Transform targetTransform;

		private Vector3 positionTarget;
		private Vector3 scaleTarget;
		private Quaternion rotationTarget;
		
		
		
		public SpringVector3 PositionSpring => positionSpring;
		public SpringRotation RotationSpring => rotationSpring;
		public SpringVector3 ScaleSpring => scaleSpring;
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
		
		public SpringEvents ScaleEvents => scaleSpring.springEvents;
		public Vector3 GetTargetScale() => scaleSpring.GetTarget();
		public void SetTargetScale(Vector3 target) => scaleSpring.SetTarget(target);
		public void SetTargetScale(float target) => SetTargetScale(Vector3.one * target);
		public Vector3 GetCurrentValueScale() => scaleSpring.GetCurrentValue();
		public void SetCurrentValueScale(Vector3 currentValues) => scaleSpring.SetCurrentValue(currentValues);
		public void SetCurrentValueScale(float currentValues) => SetCurrentValueScale(Vector3.one * currentValues);
		public Vector3 GetVelocityScale() => scaleSpring.GetVelocity();
		public void SetVelocityScale(Vector3 velocity) => scaleSpring.SetVelocity(velocity);
		public void SetVelocityScale(float velocity) => SetVelocityScale(Vector3.one * velocity);
		public void AddVelocityScale(Vector3 velocityToAdd) =>	scaleSpring.AddVelocity(velocityToAdd);
		public void ReachEquilibriumScale() => scaleSpring.ReachEquilibrium();
		public Vector3 GetForceScale() => scaleSpring.GetForce();
		public void SetForceScale(Vector3 force) => scaleSpring.SetForce(force);
		public void SetForceScale(float force) => SetForceScale(Vector3.one * force);
		public Vector3 GetDragScale() => scaleSpring.GetDrag();
		public void SetDragScale(Vector3 drag) => scaleSpring.SetDrag(drag);
		public void SetDragScale(float drag) => SetDragScale(Vector3.one * drag);
		public float GetCommonForceScale() => scaleSpring.GetCommonForce();
		public float GetCommonDragScale() => scaleSpring.GetCommonDrag();
		public void SetCommonForceScale(float force)
		{
			scaleSpring.SetCommonForceAndDrag(true);
			scaleSpring.SetCommonForce(force);
		}
		public void SetCommonDragScale(float drag)
		{
			scaleSpring.SetCommonForceAndDrag(true);
			scaleSpring.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDragScale(float force, float drag)
		{
			SetCommonForceScale(force);
			SetCommonDragScale(drag);
		}
		public void SetMinValuesScale(Vector3 minValue) => scaleSpring.SetMinValues(minValue);
		public void SetMinValuesScale(float minValue) => SetMinValuesScale(Vector3.one * minValue);
		public void SetMaxValuesScale(Vector3 maxValue) => scaleSpring.SetMaxValues(maxValue);
		public void SetMaxValuesScale(float maxValue) => SetMaxValuesScale(Vector3.one * maxValue);
		public void SetClampCurrentValuesScale(bool clampTargetX, bool clampTargetY, bool clampTargetZ) => scaleSpring.SetClampCurrentValues(clampTargetX, clampTargetY, clampTargetZ);
		public void SetClampTargetScale(bool clampTargetX, bool clampTargetY, bool clampTargetZ) => scaleSpring.SetClampTarget(clampTargetX, clampTargetY, clampTargetZ);
		public void StopSpringOnClampScale(bool stopX, bool stopY, bool stopZ) => scaleSpring.StopSpringOnClamp(stopX, stopY, stopZ);
		
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
		

		protected override void RegisterSprings()
		{
			RegisterSpring(positionSpring);
			RegisterSpring(scaleSpring);
			RegisterSpring(rotationSpring);
		}

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
				if (!scaleSpring.useInitialValues)
				{
					SetCurrentValueScaleByDefault();
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
					if (!scaleSpring.useCustomTarget)
					{
						SetTargetScaleByDefault();
					}
				}
			}
		}

		protected override void SetCurrentValueByDefault()
		{
			SetCurrentValuePositionByDefault();
			SetCurrentValueRotationByDefault();
			SetCurrentValueScaleByDefault();
		}

		private void SetCurrentValuePositionByDefault()
		{
			if (spaceType == SpaceType.LocalSpace)
			{
				positionSpring.SetCurrentValue(followerTransform.localPosition);
			}
			else
			{
				positionSpring.SetCurrentValue(followerTransform.position);
			}
		}

		private void SetCurrentValueRotationByDefault()
		{
			if (spaceType == SpaceType.LocalSpace)
			{
				rotationSpring.SetCurrentValue(followerTransform.localRotation);
			}
			else
			{
				rotationSpring.SetCurrentValue(followerTransform.rotation);
			}
		}

		private void SetCurrentValueScaleByDefault()
		{
			scaleSpring.SetCurrentValue(followerTransform.localScale);
		}

		protected override void SetTargetByDefault()
		{
			SetTargetPositionByDefault();
			SetTargetRotationByDefault();
			SetTargetScaleByDefault();
		}

		private void SetTargetPositionByDefault()
		{
			if (spaceType == SpaceType.LocalSpace)
			{
				positionSpring.SetTarget(followerTransform.localPosition);
			}
			else
			{
				positionSpring.SetTarget(followerTransform.position);
			}
		}

		private void SetTargetRotationByDefault()
		{
			if (spaceType == SpaceType.LocalSpace)
			{
				rotationSpring.SetTarget(followerTransform.localRotation);
			}
			else
			{
				rotationSpring.SetTarget(followerTransform.rotation);
			}
		}

		private void SetTargetScaleByDefault()
		{
			scaleSpring.SetTarget(followerTransform.localScale);
		}
		

		private void Start()
		{
			if (!initialized) { return; }

			UpdateTransform();
		}

		public void Update()
		{
			if (!initialized) { return; }

			UpdateTransform();

			if (useTransformAsTarget)
			{
				UpdateTarget();
			}
		}

		#region UPDATE

		public void UpdateTarget()
		{
			if (spaceType == SpaceType.WorldSpace)
			{
				GetTargetsWorldSpace();
			}
			else if (spaceType == SpaceType.LocalSpace)
			{
				GetTargetsLocalSpace();
			}

			RefreshSpringsTargets();
		}

		private void UpdateTransform()
		{
			if (spaceType == SpaceType.WorldSpace)
			{
				UpdateTransformWorldSpace();
			}
			else if (spaceType == SpaceType.LocalSpace)
			{
				UpdateTransformLocalSpace();
			}
		}

		private void UpdateTransformWorldSpace()
		{
			if (positionSpring.springEnabled)
			{
				followerTransform.position = positionSpring.GetCurrentValue();
			}

			if (rotationSpring.springEnabled)
			{
				followerTransform.rotation = rotationSpring.GetCurrentValue();
			}

			if (scaleSpring.springEnabled)
			{
				followerTransform.localScale = scaleSpring.GetCurrentValue();
			}
		}

		private void UpdateTransformLocalSpace()
		{
			if (positionSpring.springEnabled)
			{
				followerTransform.localPosition = positionSpring.GetCurrentValue();
			}

			if (rotationSpring.springEnabled)
			{
				followerTransform.localRotation = rotationSpring.GetCurrentValue();
			}

			if (scaleSpring.springEnabled)
			{
				followerTransform.localScale = scaleSpring.GetCurrentValue();
			}
		}

		private void GetTargetsWorldSpace()
		{
			if (positionSpring.springEnabled)
			{
				positionTarget = targetTransform.position;
			}

			if (rotationSpring.springEnabled)
			{
				rotationTarget = targetTransform.rotation;
			}

			if (scaleSpring.springEnabled)
			{
				scaleTarget = targetTransform.localScale;
			}
		}

		private void GetTargetsLocalSpace()
		{
			if (positionSpring.springEnabled)
			{
				positionTarget = targetTransform.localPosition;
			}

			if (rotationSpring.springEnabled)
			{
				rotationTarget = targetTransform.localRotation;
			}

			if (scaleSpring.springEnabled)
			{
				scaleTarget = targetTransform.localScale;
			}
		}

		private void RefreshSpringsTargets()
		{
			positionSpring.SetTarget(positionTarget);

			if(spaceType == SpaceType.WorldSpace)
			{
				rotationSpring.SetTarget(rotationTarget);
			}
			else
			{
				rotationSpring.SetTarget(rotationTarget);
			}

			scaleSpring.SetTarget(scaleTarget);
		}

		#endregion

		public override bool IsValidSpringComponent()
		{
			bool res = true;

			if(useTransformAsTarget && targetTransform == null)
			{
				AddErrorReason($"{gameObject.name} useTransformAsTarget is enabled but targetTransform is null");
				res = false;
			}
			if(followerTransform == null)
			{
				AddErrorReason($"{gameObject.name} followerTransform cannot be null");
				res = false;
			}
			
			return res;
		}
		
		#region ENABLE/DISABLE SPRING PROPERTIES
		public bool SpringPositionEnabled
		{
			get => positionSpring.springEnabled;
			set => positionSpring.springEnabled = value;
		}

		public bool SpringRotationEnabled
		{
			get => rotationSpring.springEnabled;
			set => rotationSpring.springEnabled = value;
		}

		public bool SpringScaleEnabled
		{
			get => scaleSpring.springEnabled;
			set => scaleSpring.springEnabled = value;
		}
		#endregion

#if UNITY_EDITOR
		protected override void Reset()
		{
			base.Reset();

			followerTransform = transform;
		}

		protected override void DrawGizmosSelected()
		{
			positionSpring.DrawGizmosSelected(transform.position);
			rotationSpring.DrawGizmosSelected(transform.position);
			scaleSpring.DrawGizmosSelected(transform.position);
		}

		internal override Spring[] GetSpringsArray()
		{
			Spring[] res = new Spring[]
			{
				positionSpring, scaleSpring, rotationSpring
			};

			return res;
		}
#endif
	}
}
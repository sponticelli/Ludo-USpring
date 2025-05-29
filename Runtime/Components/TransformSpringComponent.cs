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
		/// <summary>
		/// Sets the target position that the spring will move towards.
		/// </summary>
		/// <param name="target">The new target position.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetTargetPosition(Vector3 target)
		{
			positionSpring.SetTarget(target);
			return this;
		}

		/// <summary>
		/// Sets the target position using a single float for all axes.
		/// </summary>
		/// <param name="target">The target value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetTargetPosition(float target) => SetTargetPosition(Vector3.one * target);

		public Vector3 GetCurrentValuePosition() => positionSpring.GetCurrentValue();

		/// <summary>
		/// Sets the current position of the spring.
		/// </summary>
		/// <param name="currentValues">The new current position.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCurrentValuePosition(Vector3 currentValues)
		{
			positionSpring.SetCurrentValue(currentValues);
			return this;
		}

		/// <summary>
		/// Sets the current position using a single float for all axes.
		/// </summary>
		/// <param name="currentValues">The current value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCurrentValuePosition(float currentValues) => SetCurrentValuePosition(Vector3.one * currentValues);

		public Vector3 GetVelocityPosition() => positionSpring.GetVelocity();

		/// <summary>
		/// Sets the velocity of the position spring.
		/// </summary>
		/// <param name="velocity">The new velocity value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetVelocityPosition(Vector3 velocity)
		{
			positionSpring.SetVelocity(velocity);
			return this;
		}

		/// <summary>
		/// Sets the velocity using a single float for all axes.
		/// </summary>
		/// <param name="velocity">The velocity for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetVelocityPosition(float velocity) => SetVelocityPosition(Vector3.one * velocity);

		/// <summary>
		/// Adds velocity to the current position spring velocity.
		/// </summary>
		/// <param name="velocityToAdd">The velocity to add.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent AddVelocityPosition(Vector3 velocityToAdd)
		{
			positionSpring.AddVelocity(velocityToAdd);
			return this;
		}

		/// <summary>
		/// Immediately sets the position spring to its target value and stops all motion.
		/// </summary>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent ReachEquilibriumPosition()
		{
			positionSpring.ReachEquilibrium();
			return this;
		}
		public Vector3 GetForcePosition() => positionSpring.GetForce();

		/// <summary>
		/// Sets the force values for the position spring.
		/// </summary>
		/// <param name="force">The force vector.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetForcePosition(Vector3 force)
		{
			positionSpring.SetForce(force);
			return this;
		}

		/// <summary>
		/// Sets the force using a single float for all position axes.
		/// </summary>
		/// <param name="force">The force for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetForcePosition(float force) => SetForcePosition(Vector3.one * force);

		public Vector3 GetDragPosition() => positionSpring.GetDrag();

		/// <summary>
		/// Sets the drag values for the position spring.
		/// </summary>
		/// <param name="drag">The drag vector.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetDragPosition(Vector3 drag)
		{
			positionSpring.SetDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets the drag using a single float for all position axes.
		/// </summary>
		/// <param name="drag">The drag for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetDragPosition(float drag) => SetDragPosition(Vector3.one * drag);

		public float GetCommonForcePosition() => positionSpring.GetCommonForce();
		public float GetCommonDragPosition() => positionSpring.GetCommonDrag();

		/// <summary>
		/// Sets the common force (stiffness) value for position.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCommonForcePosition(float force)
		{
			positionSpring.SetCommonForceAndDrag(true);
			positionSpring.SetCommonForce(force);
			return this;
		}

		/// <summary>
		/// Sets the common drag (damping) value for position.
		/// </summary>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCommonDragPosition(float drag)
		{
			positionSpring.SetCommonForceAndDrag(true);
			positionSpring.SetCommonDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets both common force and drag values for position.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCommonForceAndDragPosition(float force, float drag)
		{
			SetCommonForcePosition(force);
			SetCommonDragPosition(drag);
			return this;
		}

		/// <summary>
		/// Sets the minimum values for position clamping.
		/// </summary>
		/// <param name="minValue">The minimum values.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetMinValuesPosition(Vector3 minValue)
		{
			positionSpring.SetMinValues(minValue);
			return this;
		}

		/// <summary>
		/// Sets the minimum values using a single float for all position axes.
		/// </summary>
		/// <param name="minValue">The minimum value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetMinValuesPosition(float minValue) => SetMinValuesPosition(Vector3.one * minValue);

		/// <summary>
		/// Sets the maximum values for position clamping.
		/// </summary>
		/// <param name="maxValue">The maximum values.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetMaxValuesPosition(Vector3 maxValue)
		{
			positionSpring.SetMaxValues(maxValue);
			return this;
		}

		/// <summary>
		/// Sets the maximum values using a single float for all position axes.
		/// </summary>
		/// <param name="maxValue">The maximum value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetMaxValuesPosition(float maxValue) => SetMaxValuesPosition(Vector3.one * maxValue);

		/// <summary>
		/// Sets clamping for current position values per axis.
		/// </summary>
		/// <param name="clampTargetX">Clamp X axis.</param>
		/// <param name="clampTargetY">Clamp Y axis.</param>
		/// <param name="clampTargetZ">Clamp Z axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetClampCurrentValuesPosition(bool clampTargetX, bool clampTargetY, bool clampTargetZ)
		{
			positionSpring.SetClampCurrentValues(clampTargetX, clampTargetY, clampTargetZ);
			return this;
		}

		/// <summary>
		/// Sets clamping for target position values per axis.
		/// </summary>
		/// <param name="clampTargetX">Clamp X axis.</param>
		/// <param name="clampTargetY">Clamp Y axis.</param>
		/// <param name="clampTargetZ">Clamp Z axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetClampTargetPosition(bool clampTargetX, bool clampTargetY, bool clampTargetZ)
		{
			positionSpring.SetClampTarget(clampTargetX, clampTargetY, clampTargetZ);
			return this;
		}

		/// <summary>
		/// Sets stop on clamp per position axis.
		/// </summary>
		/// <param name="stopX">Stop on clamp X axis.</param>
		/// <param name="stopY">Stop on clamp Y axis.</param>
		/// <param name="stopZ">Stop on clamp Z axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent StopSpringOnClampPosition(bool stopX, bool stopY, bool stopZ)
		{
			positionSpring.StopSpringOnClamp(stopX, stopY, stopZ);
			return this;
		}

		public SpringEvents ScaleEvents => scaleSpring.springEvents;
		public Vector3 GetTargetScale() => scaleSpring.GetTarget();

		/// <summary>
		/// Sets the target scale that the spring will move towards.
		/// </summary>
		/// <param name="target">The new target scale.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetTargetScale(Vector3 target)
		{
			scaleSpring.SetTarget(target);
			return this;
		}

		/// <summary>
		/// Sets the target scale using a single float for all axes.
		/// </summary>
		/// <param name="target">The target value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetTargetScale(float target) => SetTargetScale(Vector3.one * target);

		public Vector3 GetCurrentValueScale() => scaleSpring.GetCurrentValue();

		/// <summary>
		/// Sets the current scale of the spring.
		/// </summary>
		/// <param name="currentValues">The new current scale.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCurrentValueScale(Vector3 currentValues)
		{
			scaleSpring.SetCurrentValue(currentValues);
			return this;
		}

		/// <summary>
		/// Sets the current scale using a single float for all axes.
		/// </summary>
		/// <param name="currentValues">The current value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCurrentValueScale(float currentValues) => SetCurrentValueScale(Vector3.one * currentValues);

		public Vector3 GetVelocityScale() => scaleSpring.GetVelocity();

		/// <summary>
		/// Sets the velocity of the scale spring.
		/// </summary>
		/// <param name="velocity">The new velocity value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetVelocityScale(Vector3 velocity)
		{
			scaleSpring.SetVelocity(velocity);
			return this;
		}

		/// <summary>
		/// Sets the velocity using a single float for all scale axes.
		/// </summary>
		/// <param name="velocity">The velocity for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetVelocityScale(float velocity) => SetVelocityScale(Vector3.one * velocity);

		/// <summary>
		/// Adds velocity to the current scale spring velocity.
		/// </summary>
		/// <param name="velocityToAdd">The velocity to add.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent AddVelocityScale(Vector3 velocityToAdd)
		{
			scaleSpring.AddVelocity(velocityToAdd);
			return this;
		}

		/// <summary>
		/// Immediately sets the scale spring to its target value and stops all motion.
		/// </summary>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent ReachEquilibriumScale()
		{
			scaleSpring.ReachEquilibrium();
			return this;
		}
		public Vector3 GetForceScale() => scaleSpring.GetForce();

		/// <summary>
		/// Sets the force values for the scale spring.
		/// </summary>
		/// <param name="force">The force vector.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetForceScale(Vector3 force)
		{
			scaleSpring.SetForce(force);
			return this;
		}

		/// <summary>
		/// Sets the force using a single float for all scale axes.
		/// </summary>
		/// <param name="force">The force for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetForceScale(float force) => SetForceScale(Vector3.one * force);

		public Vector3 GetDragScale() => scaleSpring.GetDrag();

		/// <summary>
		/// Sets the drag values for the scale spring.
		/// </summary>
		/// <param name="drag">The drag vector.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetDragScale(Vector3 drag)
		{
			scaleSpring.SetDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets the drag using a single float for all scale axes.
		/// </summary>
		/// <param name="drag">The drag for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetDragScale(float drag) => SetDragScale(Vector3.one * drag);

		public float GetCommonForceScale() => scaleSpring.GetCommonForce();
		public float GetCommonDragScale() => scaleSpring.GetCommonDrag();

		/// <summary>
		/// Sets the common force (stiffness) value for scale.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCommonForceScale(float force)
		{
			scaleSpring.SetCommonForceAndDrag(true);
			scaleSpring.SetCommonForce(force);
			return this;
		}

		/// <summary>
		/// Sets the common drag (damping) value for scale.
		/// </summary>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCommonDragScale(float drag)
		{
			scaleSpring.SetCommonForceAndDrag(true);
			scaleSpring.SetCommonDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets both common force and drag values for scale.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCommonForceAndDragScale(float force, float drag)
		{
			SetCommonForceScale(force);
			SetCommonDragScale(drag);
			return this;
		}

		/// <summary>
		/// Sets the minimum values for scale clamping.
		/// </summary>
		/// <param name="minValue">The minimum values.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetMinValuesScale(Vector3 minValue)
		{
			scaleSpring.SetMinValues(minValue);
			return this;
		}

		/// <summary>
		/// Sets the minimum values using a single float for all scale axes.
		/// </summary>
		/// <param name="minValue">The minimum value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetMinValuesScale(float minValue) => SetMinValuesScale(Vector3.one * minValue);

		/// <summary>
		/// Sets the maximum values for scale clamping.
		/// </summary>
		/// <param name="maxValue">The maximum values.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetMaxValuesScale(Vector3 maxValue)
		{
			scaleSpring.SetMaxValues(maxValue);
			return this;
		}

		/// <summary>
		/// Sets the maximum values using a single float for all scale axes.
		/// </summary>
		/// <param name="maxValue">The maximum value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetMaxValuesScale(float maxValue) => SetMaxValuesScale(Vector3.one * maxValue);

		/// <summary>
		/// Sets clamping for current scale values per axis.
		/// </summary>
		/// <param name="clampTargetX">Clamp X axis.</param>
		/// <param name="clampTargetY">Clamp Y axis.</param>
		/// <param name="clampTargetZ">Clamp Z axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetClampCurrentValuesScale(bool clampTargetX, bool clampTargetY, bool clampTargetZ)
		{
			scaleSpring.SetClampCurrentValues(clampTargetX, clampTargetY, clampTargetZ);
			return this;
		}

		/// <summary>
		/// Sets clamping for target scale values per axis.
		/// </summary>
		/// <param name="clampTargetX">Clamp X axis.</param>
		/// <param name="clampTargetY">Clamp Y axis.</param>
		/// <param name="clampTargetZ">Clamp Z axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetClampTargetScale(bool clampTargetX, bool clampTargetY, bool clampTargetZ)
		{
			scaleSpring.SetClampTarget(clampTargetX, clampTargetY, clampTargetZ);
			return this;
		}

		/// <summary>
		/// Sets stop on clamp per scale axis.
		/// </summary>
		/// <param name="stopX">Stop on clamp X axis.</param>
		/// <param name="stopY">Stop on clamp Y axis.</param>
		/// <param name="stopZ">Stop on clamp Z axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent StopSpringOnClampScale(bool stopX, bool stopY, bool stopZ)
		{
			scaleSpring.StopSpringOnClamp(stopX, stopY, stopZ);
			return this;
		}

		public SpringEvents RotationEvents => rotationSpring.springEvents;
		public Quaternion GetTargetRotation() => rotationSpring.GetTarget();

		/// <summary>
		/// Sets the target rotation that the spring will move towards.
		/// </summary>
		/// <param name="targetQuaternion">The new target quaternion.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetTargetRotation(Quaternion targetQuaternion)
		{
			rotationSpring.SetTarget(targetQuaternion);
			return this;
		}

		/// <summary>
		/// Sets the target rotation that the spring will move towards.
		/// </summary>
		/// <param name="targetEuler">The new target euler angles.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetTargetRotation(Vector3 targetEuler)
		{
			rotationSpring.SetTarget(targetEuler);
			return this;
		}

		public Quaternion GetCurrentValueRotation() => rotationSpring.GetCurrentValue();

		/// <summary>
		/// Sets the current rotation of the spring.
		/// </summary>
		/// <param name="currentQuaternion">The new current quaternion.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCurrentValueRotation(Quaternion currentQuaternion)
		{
			rotationSpring.SetCurrentValue(currentQuaternion);
			return this;
		}

		/// <summary>
		/// Sets the current rotation of the spring.
		/// </summary>
		/// <param name="currentEuler">The new current euler angles.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCurrentValueRotation(Vector3 currentEuler)
		{
			rotationSpring.SetCurrentValue(currentEuler);
			return this;
		}

		public Vector3 GetVelocityRotation() => rotationSpring.GetVelocity();

		/// <summary>
		/// Sets the velocity of the rotation spring.
		/// </summary>
		/// <param name="velocity">The new velocity value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetVelocityRotation(Vector3 velocity)
		{
			rotationSpring.SetVelocity(velocity);
			return this;
		}

		/// <summary>
		/// Adds velocity to the current rotation spring velocity.
		/// </summary>
		/// <param name="velocityToAdd">The velocity to add.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent AddVelocityRotation(Vector3 velocityToAdd)
		{
			rotationSpring.AddVelocity(velocityToAdd);
			return this;
		}

		/// <summary>
		/// Immediately sets the rotation spring to its target value and stops all motion.
		/// </summary>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent ReachEquilibriumRotation()
		{
			rotationSpring.ReachEquilibrium();
			return this;
		}

		public float GetCommonForceRotation() => rotationSpring.GetCommonForce();
		public float GetCommonDragRotation() => rotationSpring.GetCommonDrag();

		/// <summary>
		/// Sets the common force (stiffness) value for rotation.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCommonForceRotation(float force)
		{
			rotationSpring.SetCommonForceAndDrag(true);
			rotationSpring.SetCommonForce(force);
			return this;
		}

		/// <summary>
		/// Sets the common drag (damping) value for rotation.
		/// </summary>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCommonDragRotation(float drag)
		{
			rotationSpring.SetCommonForceAndDrag(true);
			rotationSpring.SetCommonDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets both common force and drag values for rotation.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public TransformSpringComponent SetCommonForceAndDragRotation(float force, float drag)
		{
			SetCommonForceRotation(force);
			SetCommonDragRotation(drag);
			return this;
		}

		/// <summary>
		/// Immediately sets all springs (position, rotation, scale) to their target values and stops all motion.
		/// </summary>
		public override void ReachEquilibrium()
		{
			base.ReachEquilibrium();
			UpdateTransform();
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
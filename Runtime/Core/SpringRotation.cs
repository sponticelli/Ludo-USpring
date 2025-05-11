using UnityEditor;
using UnityEngine;
using USpring.Core.Interfaces;

namespace USpring.Core
{
	[System.Serializable]
	public class SpringRotation : Spring, ISpringRotation
	{
		public enum AxisRestriction
		{
			None = 0,
			OnlyXAxis = 1,
			OnlyYAxis = 2,
			OnlyZAxis = 3
		}

		public const int SPRING_SIZE = 9;

		private const int FORWARD_X = 0;
		private const int FORWARD_Y = 1;
		private const int FORWARD_Z = 2;

		private const int UP_X = 6;
		private const int UP_Y = 7;
		private const int UP_Z = 8;

		private const int LOCAL_AXIS_X = 3;
		private const int LOCAL_AXIS_Y = 4;
		private const int LOCAL_AXIS_Z = 5;

		[SerializeField] private AxisRestriction axisRestriction;

		public SpringRotation() : base(SPRING_SIZE)
		{

		}

		public override int GetSpringSize()
		{
			return SPRING_SIZE;
		}

		public override bool HasValidSize()
		{
			return (springValues.Length == SPRING_SIZE);
		}


		private Vector3 CurrentLocalAxis
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[LOCAL_AXIS_X].GetCurrentValue(),
					springValues[LOCAL_AXIS_Y].GetCurrentValue(),
					springValues[LOCAL_AXIS_Z].GetCurrentValue());

				return res;
			}
			set
			{
				springValues[LOCAL_AXIS_X].SetCurrentValue(value.x);
				springValues[LOCAL_AXIS_Y].SetCurrentValue(value.y);
				springValues[LOCAL_AXIS_Z].SetCurrentValue(value.z);
			}
		}

		private Vector3 CurrentForward
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[FORWARD_X].GetCurrentValue(),
					springValues[FORWARD_Y].GetCurrentValue(),
					springValues[FORWARD_Z].GetCurrentValue()).normalized;

				return res;
			}
			set
			{
				springValues[FORWARD_X].SetCurrentValue(value.x);
				springValues[FORWARD_Y].SetCurrentValue(value.y);
				springValues[FORWARD_Z].SetCurrentValue(value.z);
			}
		}

		private Vector3 CurrentUp
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[UP_X].GetCurrentValue(),
					springValues[UP_Y].GetCurrentValue(),
					springValues[UP_Z].GetCurrentValue());

				return res;
			}
			set
			{
				springValues[UP_X].SetCurrentValue(value.x);
				springValues[UP_Y].SetCurrentValue(value.y);
				springValues[UP_Z].SetCurrentValue(value.z);
			}
		}

		public Quaternion GetCurrentGlobalRotation()
		{
			Quaternion res = Quaternion.LookRotation(CurrentForward, CurrentUp);
			return res;
		}

		public Quaternion GetCurrentValue()
		{
			Quaternion globalQuat = GetCurrentGlobalRotation();

			Vector3 forward = globalQuat * Vector3.forward;
			Vector3 up = globalQuat * Vector3.up;
			Vector3 right = globalQuat * Vector3.right;

			Quaternion res =
				Quaternion.AngleAxis(CurrentLocalAxis.x, right) *
				Quaternion.AngleAxis(CurrentLocalAxis.y, up) *
				Quaternion.AngleAxis(CurrentLocalAxis.z, forward) *
				globalQuat;

			return res;
		}

		public ISpringRotation SetCurrentValue(Quaternion newCurrentQuaternion)
		{
			CurrentForward = (newCurrentQuaternion * Vector3.forward).normalized;
			CurrentUp = (newCurrentQuaternion * Vector3.up).normalized;
			return this;
		}

		public ISpringRotation SetCurrentValue(Vector3 currentEuler)
		{
			if (axisRestriction == AxisRestriction.OnlyXAxis)
			{
				currentEuler.x = CurrentLocalAxis.x;
			}
			if (axisRestriction == AxisRestriction.OnlyYAxis)
			{
				currentEuler.y = CurrentLocalAxis.y;
			}
			if (axisRestriction == AxisRestriction.OnlyZAxis)
			{
				currentEuler.z = CurrentLocalAxis.z;
			}

			CurrentLocalAxis = currentEuler;

			return this;
		}

		private Vector3 TargetLocalAxis
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[LOCAL_AXIS_X].GetTarget(),
					springValues[LOCAL_AXIS_Y].GetTarget(),
					springValues[LOCAL_AXIS_Z].GetTarget());

				return res;
			}
			set
			{
				springValues[LOCAL_AXIS_X].SetTarget(value.x);
				springValues[LOCAL_AXIS_Y].SetTarget(value.y);
				springValues[LOCAL_AXIS_Z].SetTarget(value.z);
			}
		}

		private Vector3 TargetForward
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[FORWARD_X].GetTarget(),
					springValues[FORWARD_Y].GetTarget(),
					springValues[FORWARD_Z].GetTarget());

				return res;
			}
			set
			{
				springValues[FORWARD_X].SetTarget(value.x);
				springValues[FORWARD_Y].SetTarget(value.y);
				springValues[FORWARD_Z].SetTarget(value.z);
			}
		}

		private Vector3 TargetUp
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[UP_X].GetTarget(),
					springValues[UP_Y].GetTarget(),
					springValues[UP_Z].GetTarget());

				return res;
			}
			set
			{
				springValues[UP_X].SetTarget(value.x);
				springValues[UP_Y].SetTarget(value.y);
				springValues[UP_Z].SetTarget(value.z);
			}
		}

		private Vector3 TargetRight
		{
			get
			{
				Vector3 res = Vector3.Cross(TargetUp, TargetForward).normalized;
				return res;
			}
		}

		public ISpringRotation SetTarget(Quaternion target)
		{
			Vector3 rawForward = (target * Vector3.forward).normalized;
			Vector3 rawUp = (target * Vector3.up).normalized;

			Vector3 correctedForward = rawForward;
			Vector3 correctedUp = rawUp;
			LimitTargetRotation(rawForward, rawUp, ref correctedForward, ref correctedUp);

			TargetForward = correctedForward;
			TargetUp = correctedUp;

			return this;
		}

		private void LimitTargetRotation(Vector3 rawForward, Vector3 rawUp, ref Vector3 correctedForward, ref Vector3 correctedUp)
		{
			bool isXLimited = axisRestriction == AxisRestriction.OnlyXAxis;
			bool isYLimited = axisRestriction == AxisRestriction.OnlyYAxis;
			bool isZLimited = axisRestriction == AxisRestriction.OnlyZAxis;

			int numIterations = 5;
			for (int i = 0; i < numIterations; i++)
			{
				Vector3 correctedRight = Vector3.Cross(correctedUp, correctedForward).normalized;

				if (isXLimited)
				{
					Quaternion cancelQuat = GetCancelQuat(correctedRight, Vector3.right, 0f);
					correctedForward = cancelQuat * correctedForward;
					correctedUp = cancelQuat * correctedUp;
				}
				if (isYLimited)
				{
					Quaternion cancelQuat = GetCancelQuat(correctedUp, Vector3.up, 0f);
					correctedForward = cancelQuat * correctedForward;
					correctedUp = cancelQuat * correctedUp;
				}
				if (isZLimited)
				{
					Quaternion cancelQuat = GetCancelQuat(correctedForward, Vector3.forward, 0f);
					correctedForward = cancelQuat * correctedForward;
					correctedUp = cancelQuat * correctedUp;
				}
			}
		}

		private Quaternion GetCancelQuat(Vector3 vec, Vector3 reference, float maxAngle)
		{
			Quaternion res = Quaternion.identity;

			float angle = Vector3.Angle(vec, reference);
			if (angle >= maxAngle)
			{
				float diffAngle = angle - maxAngle;
				Vector3 rotationAxis = Vector3.Cross(vec, reference).normalized;
				Vector3 rotatedVec = Quaternion.AngleAxis(diffAngle, rotationAxis) * vec;
				res = Quaternion.FromToRotation(vec, rotatedVec);
			}

			return res;
		}

		public struct DotAndAxis
		{
			public float angle;
			public Vector3 axis;

			public DotAndAxis(Vector3 vec, Vector3 axis)
			{
				angle = Vector3.Angle(axis, vec);
				this.axis = axis;
			}
		}

		public ISpringRotation SetTarget(Vector3 targetValues)
		{
			if (axisRestriction == AxisRestriction.OnlyXAxis)
			{
				targetValues.x = TargetLocalAxis.x;
			}
			if (axisRestriction == AxisRestriction.OnlyYAxis)
			{
				targetValues.y = TargetLocalAxis.y;
			}
			if (axisRestriction == AxisRestriction.OnlyZAxis)
			{
				targetValues.z = TargetLocalAxis.z;
			}

			TargetLocalAxis = targetValues;

			return this;
		}

		public Quaternion GetTargetGlobalRotation()
		{
			Quaternion res = Quaternion.LookRotation(TargetForward, TargetUp);
			return res;
		}

		public Quaternion GetTarget()
		{
			Quaternion globalQuat = GetTargetGlobalRotation();

			Vector3 forward = globalQuat * Vector3.forward;
			Vector3 up = globalQuat * Vector3.up;
			Vector3 right = globalQuat * Vector3.right;

			Quaternion res =
				Quaternion.AngleAxis(TargetLocalAxis.x, right) *
				Quaternion.AngleAxis(TargetLocalAxis.y, up) *
				Quaternion.AngleAxis(TargetLocalAxis.z, forward) *
				globalQuat;

			return res;
		}


		private Vector3 VelocityLocalAxis
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[LOCAL_AXIS_X].GetVelocity(),
					springValues[LOCAL_AXIS_Y].GetVelocity(),
					springValues[LOCAL_AXIS_Z].GetVelocity());

				return res;
			}
			set
			{
				springValues[LOCAL_AXIS_X].SetVelocity(value.x);
				springValues[LOCAL_AXIS_Y].SetVelocity(value.y);
				springValues[LOCAL_AXIS_Z].SetVelocity(value.z);
			}
		}

		private Vector3 VelocityForward
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[FORWARD_X].GetVelocity(),
					springValues[FORWARD_Y].GetVelocity(),
					springValues[FORWARD_Z].GetVelocity());

				return res;
			}
			set
			{
				springValues[FORWARD_X].SetVelocity(value.x);
				springValues[FORWARD_Y].SetVelocity(value.y);
				springValues[FORWARD_Z].SetVelocity(value.z);
			}
		}

		private Vector3 VelocityUp
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[UP_X].GetVelocity(),
					springValues[UP_Y].GetVelocity(),
					springValues[UP_Z].GetVelocity());

				return res;
			}
			set
			{
				springValues[UP_X].SetVelocity(value.x);
				springValues[UP_Y].SetVelocity(value.y);
				springValues[UP_Z].SetVelocity(value.z);
			}
		}

		public ISpringRotation AddVelocity(Vector3 eulerTarget)
		{
			const float velocityFactor = 150;
			VelocityLocalAxis += eulerTarget * velocityFactor;
			return this;
		}

		public ISpringRotation SetVelocity(Vector3 eulerTarget)
		{
			VelocityLocalAxis = eulerTarget;
			return this;
		}

		public Vector3 GetVelocity()
		{
			Vector3 res = VelocityLocalAxis;
			return res;
		}


		private Vector3 CandidateLocalAxis
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[LOCAL_AXIS_X].GetCandidateValue(),
					springValues[LOCAL_AXIS_Y].GetCandidateValue(),
					springValues[LOCAL_AXIS_Z].GetCandidateValue());

				return res;
			}
			set
			{
				springValues[LOCAL_AXIS_X].SetCandidateValue(value.x);
				springValues[LOCAL_AXIS_Y].SetCandidateValue(value.y);
				springValues[LOCAL_AXIS_Z].SetCandidateValue(value.z);
			}
		}

		private Vector3 CandidateForward
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[FORWARD_X].GetCandidateValue(),
					springValues[FORWARD_Y].GetCandidateValue(),
					springValues[FORWARD_Z].GetCandidateValue());

				return res;
			}
			set
			{
				springValues[FORWARD_X].SetCandidateValue(value.x);
				springValues[FORWARD_Y].SetCandidateValue(value.y);
				springValues[FORWARD_Z].SetCandidateValue(value.z);
			}
		}

		private Vector3 CandidateUp
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[UP_X].GetCandidateValue(),
					springValues[UP_Y].GetCandidateValue(),
					springValues[UP_Z].GetCandidateValue());

				return res;
			}
			set
			{
				springValues[UP_X].SetCandidateValue(value.x);
				springValues[UP_Y].SetCandidateValue(value.y);
				springValues[UP_Z].SetCandidateValue(value.z);
			}
		}

		private Quaternion CandidateGlobalRotation
		{
			get
			{
				Quaternion res = Quaternion.LookRotation(CandidateForward, CandidateUp);
				return res;
			}
		}

		public override void ProcessCandidateValue()
		{
			Vector3 deltaLocalAxis = CandidateLocalAxis - CurrentLocalAxis;
			const float maxRotationPerFrame = 80f;
			deltaLocalAxis = Vector3.ClampMagnitude(deltaLocalAxis, maxRotationPerFrame);
			CandidateLocalAxis = deltaLocalAxis + CurrentLocalAxis;

			Quaternion quatCandidate = CandidateGlobalRotation;
			Quaternion quatCurrent = GetCurrentGlobalRotation();


			float angle = Quaternion.Angle(quatCurrent, quatCandidate);
			if(angle > 15f)
			{
				quatCandidate = Quaternion.RotateTowards(quatCurrent, quatCandidate, 80f);
				CandidateForward = quatCandidate * Vector3.forward;
				CandidateUp = quatCandidate * Vector3.up;
			}

			base.ProcessCandidateValue();
		}

#if UNITY_EDITOR

		internal override void DrawGizmosSelected(Vector3 componentPosition)
		{
			base.DrawGizmosSelected(componentPosition);

			if (axisRestriction == AxisRestriction.None) return;

			const float lineLength = 1.5f;
			const float sphereRadius = 0.1f;

			Vector3 vec = Vector3.up;
			switch (axisRestriction)
			{
				case AxisRestriction.OnlyXAxis:
					vec = Vector3.right;
					break;
				case AxisRestriction.OnlyYAxis:
					vec = Vector3.up;
					break;
				case AxisRestriction.OnlyZAxis:
					vec = Vector3.forward;
					break;
			}

			Color vecColor = Color.cyan;
			Color sphereColor = vecColor;
			sphereColor.a = 0.5f;

			Gizmos.color = vecColor;
			Vector3 position01 = componentPosition - vec * lineLength;
			Vector3 position02 = componentPosition + vec * lineLength;
			Gizmos.DrawLine(position01, position02);

			Gizmos.color = sphereColor;
			Gizmos.DrawSphere(position01, sphereRadius);
			Gizmos.DrawSphere(position02, sphereRadius);
		}
#endif

		#region Missing Interface Implementations

		/// <summary>
		/// Gets the target rotation of the spring as Euler angles.
		/// </summary>
		/// <returns>The target rotation as Euler angles.</returns>
		public Vector3 GetTargetEuler()
		{
			return GetTarget().eulerAngles;
		}

		/// <summary>
		/// Gets the current rotation of the spring as Euler angles.
		/// </summary>
		/// <returns>The current rotation as Euler angles.</returns>
		public Vector3 GetCurrentValueEuler()
		{
			return GetCurrentValue().eulerAngles;
		}

		/// <summary>
		/// Sets the force (stiffness) of the spring.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation SetForce(Vector3 force)
		{
			for (int i = 0; i < springValues.Length; i++)
			{
				if (i < 3) // Forward axes
				{
					springValues[i].SetForce(force.x);
				}
				else if (i < 6) // Local axes
				{
					springValues[i].SetForce(force.y);
				}
				else // Up axes
				{
					springValues[i].SetForce(force.z);
				}
			}
			return this;
		}

		/// <summary>
		/// Sets the force (stiffness) of the spring using a uniform value for all components.
		/// </summary>
		/// <param name="uniformForce">The uniform force to use for all components.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation SetForce(float uniformForce)
		{
			for (int i = 0; i < springValues.Length; i++)
			{
				springValues[i].SetForce(uniformForce);
			}
			return this;
		}

		/// <summary>
		/// Gets the force (stiffness) of the spring.
		/// </summary>
		/// <returns>The force value.</returns>
		public Vector3 GetForce()
		{
			// Return average force values for each group of axes
			float forwardForce = (springValues[FORWARD_X].GetForce() + springValues[FORWARD_Y].GetForce() + springValues[FORWARD_Z].GetForce()) / 3f;
			float localForce = (springValues[LOCAL_AXIS_X].GetForce() + springValues[LOCAL_AXIS_Y].GetForce() + springValues[LOCAL_AXIS_Z].GetForce()) / 3f;
			float upForce = (springValues[UP_X].GetForce() + springValues[UP_Y].GetForce() + springValues[UP_Z].GetForce()) / 3f;

			return new Vector3(forwardForce, localForce, upForce);
		}

		/// <summary>
		/// Sets the drag (damping) of the spring.
		/// </summary>
		/// <param name="drag">The drag value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation SetDrag(Vector3 drag)
		{
			for (int i = 0; i < springValues.Length; i++)
			{
				if (i < 3) // Forward axes
				{
					springValues[i].SetDrag(drag.x);
				}
				else if (i < 6) // Local axes
				{
					springValues[i].SetDrag(drag.y);
				}
				else // Up axes
				{
					springValues[i].SetDrag(drag.z);
				}
			}
			return this;
		}

		/// <summary>
		/// Sets the drag (damping) of the spring using a uniform value for all components.
		/// </summary>
		/// <param name="uniformDrag">The uniform drag to use for all components.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation SetDrag(float uniformDrag)
		{
			for (int i = 0; i < springValues.Length; i++)
			{
				springValues[i].SetDrag(uniformDrag);
			}
			return this;
		}

		/// <summary>
		/// Gets the drag (damping) of the spring.
		/// </summary>
		/// <returns>The drag value.</returns>
		public Vector3 GetDrag()
		{
			// Return average drag values for each group of axes
			float forwardDrag = (springValues[FORWARD_X].GetDrag() + springValues[FORWARD_Y].GetDrag() + springValues[FORWARD_Z].GetDrag()) / 3f;
			float localDrag = (springValues[LOCAL_AXIS_X].GetDrag()  + springValues[LOCAL_AXIS_Y].GetDrag()  + springValues[LOCAL_AXIS_Z].GetDrag() ) / 3f;
			float upDrag = (springValues[UP_X].GetDrag()  + springValues[UP_Y].GetDrag()  + springValues[UP_Z].GetDrag() ) / 3f;

			return new Vector3(forwardDrag, localDrag, upDrag);
		}

		/// <summary>
		/// Sets the minimum values for clamping in Euler angles.
		/// </summary>
		/// <param name="minValues">The minimum values in Euler angles.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation SetMinValues(Vector3 minValues)
		{
			springValues[LOCAL_AXIS_X].SetMinValue(minValues.x);
			springValues[LOCAL_AXIS_Y].SetMinValue(minValues.y);
			springValues[LOCAL_AXIS_Z].SetMinValue(minValues.z);
			return this;
		}

		/// <summary>
		/// Gets the minimum values for clamping in Euler angles.
		/// </summary>
		/// <returns>The minimum values in Euler angles.</returns>
		public Vector3 GetMinValues()
		{
			return new Vector3(
				springValues[LOCAL_AXIS_X].GetMinValue(),
				springValues[LOCAL_AXIS_Y].GetMinValue(),
				springValues[LOCAL_AXIS_Z].GetMinValue()
			);
		}

		/// <summary>
		/// Sets the maximum values for clamping in Euler angles.
		/// </summary>
		/// <param name="maxValues">The maximum values in Euler angles.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation SetMaxValues(Vector3 maxValues)
		{
			springValues[LOCAL_AXIS_X].SetMaxValue(maxValues.x);
			springValues[LOCAL_AXIS_Y].SetMaxValue(maxValues.y);
			springValues[LOCAL_AXIS_Z].SetMaxValue(maxValues.z);
			return this;
		}

		/// <summary>
		/// Gets the maximum values for clamping in Euler angles.
		/// </summary>
		/// <returns>The maximum values in Euler angles.</returns>
		public Vector3 GetMaxValues()
		{
			return new Vector3(
				springValues[LOCAL_AXIS_X].GetMaxValue(),
				springValues[LOCAL_AXIS_Y].GetMaxValue(),
				springValues[LOCAL_AXIS_Z].GetMaxValue()
			);
		}

		/// <summary>
		/// Sets the minimum and maximum values for clamping in Euler angles in a single call.
		/// </summary>
		/// <param name="minValues">The minimum values in Euler angles.</param>
		/// <param name="maxValues">The maximum values in Euler angles.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation SetClampRange(Vector3 minValues, Vector3 maxValues)
		{
			SetMinValues(minValues);
			SetMaxValues(maxValues);
			return this;
		}

		/// <summary>
		/// Enables or disables target clamping.
		/// </summary>
		/// <param name="enabled">Whether target clamping should be enabled.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation SetClampTarget(bool enabled)
		{
			for (int i = 0; i < springValues.Length; i++)
			{
				springValues[i].SetClampTarget(enabled);
			}
			return this;
		}
		

		/// <summary>
		/// Enables or disables current value clamping.
		/// </summary>
		/// <param name="enabled">Whether current value clamping should be enabled.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation SetClampCurrentValue(bool enabled)
		{
			for (int i = 0; i < springValues.Length; i++)
			{
				springValues[i].SetClampCurrentValue(enabled);
			}
			return this;
		}

		/// <summary>
		/// Enables or disables stopping the spring when the current value is clamped.
		/// </summary>
		/// <param name="enabled">Whether to stop the spring when the current value is clamped.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation SetStopOnClamp(bool enabled)
		{
			for (int i = 0; i < springValues.Length; i++)
			{
				springValues[i].SetStopOnClamp(enabled);
			}
			return this;
		}

		/// <summary>
		/// Gets whether the spring stops when the current value is clamped.
		/// </summary>
		/// <returns>True if the spring stops when the current value is clamped, false otherwise.</returns>
		public bool GetStopOnClamp()
		{
			// Return true if all spring values stop on clamp
			for (int i = 0; i < springValues.Length; i++)
			{
				if (!springValues[i].GetStopOnClamp())
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Configures clamping settings in a single call.
		/// </summary>
		/// <param name="clampTarget">Whether to clamp the target value.</param>
		/// <param name="clampCurrentValue">Whether to clamp the current value.</param>
		/// <param name="stopOnClamp">Whether to stop the spring when the current value is clamped.</param>
		/// <param name="minValues">The minimum values in Euler angles for clamping.</param>
		/// <param name="maxValues">The maximum values in Euler angles for clamping.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector3 minValues, Vector3 maxValues)
		{
			SetClampTarget(clampTarget);
			SetClampCurrentValue(clampCurrentValue);
			SetStopOnClamp(stopOnClamp);
			SetMinValues(minValues);
			SetMaxValues(maxValues);
			return this;
		}

		/// <summary>
		/// Configures the spring in a single call.
		/// </summary>
		/// <param name="force">The force (stiffness) value.</param>
		/// <param name="drag">The drag (damping) value.</param>
		/// <param name="initialValue">The initial rotation of the spring.</param>
		/// <param name="target">The target rotation of the spring.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation Configure(Vector3 force, Vector3 drag, Quaternion initialValue, Quaternion target)
		{
			SetForce(force);
			SetDrag(drag);
			SetCurrentValue(initialValue);
			SetTarget(target);
			return this;
		}

		/// <summary>
		/// Configures the spring in a single call using uniform values for force and drag.
		/// </summary>
		/// <param name="uniformForce">The uniform force to use for all components.</param>
		/// <param name="uniformDrag">The uniform drag to use for all components.</param>
		/// <param name="initialValue">The initial rotation of the spring.</param>
		/// <param name="target">The target rotation of the spring.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpringRotation Configure(float uniformForce, float uniformDrag, Quaternion initialValue, Quaternion target)
		{
			SetForce(uniformForce);
			SetDrag(uniformDrag);
			SetCurrentValue(initialValue);
			SetTarget(target);
			return this;
		}

		/// <summary>
		/// Creates a new instance of ISpringRotation with the same configuration.
		/// </summary>
		/// <returns>A new instance of ISpringRotation with the same configuration.</returns>
		public ISpringRotation Clone()
		{
			SpringRotation clone = new SpringRotation();

			// Copy basic properties
			clone.commonForceAndDrag = this.commonForceAndDrag;
			clone.commonForce = this.commonForce;
			clone.commonDrag = this.commonDrag;
			clone.springEnabled = this.springEnabled;
			clone.clampingEnabled = this.clampingEnabled;
			clone.eventsEnabled = this.eventsEnabled;
			clone.axisRestriction = this.axisRestriction;

			// Initialize the clone
			clone.Initialize();

			// Copy spring values
			for (int i = 0; i < springValues.Length; i++)
			{
				if (i < clone.springValues.Length)
				{
					clone.springValues[i].SetForce(springValues[i].GetForce());
					clone.springValues[i].SetDrag(springValues[i].GetDrag());
					clone.springValues[i].SetMinValue(springValues[i].GetMinValue());
					clone.springValues[i].SetMaxValue(springValues[i].GetMaxValue());
					clone.springValues[i].SetClampTarget(springValues[i].GetClampTarget());
					clone.springValues[i].SetClampCurrentValue(springValues[i].GetClampCurrentValue());
					clone.springValues[i].SetStopOnClamp(springValues[i].GetStopOnClamp());
					clone.springValues[i].SetTarget(this.springValues[i].GetTarget());
					clone.springValues[i].SetCurrentValue(this.springValues[i].GetCurrentValue());
					clone.springValues[i].SetVelocity(this.springValues[i].GetVelocity());
				}
			}

			return clone;
		}

		#endregion
	}
}
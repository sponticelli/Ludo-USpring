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
		/// <summary>
		/// Sets the target value that the spring will move towards.
		/// </summary>
		/// <param name="targetQuaternion">The new target quaternion.</param>
		/// <returns>This component instance for method chaining.</returns>
		public RotationSpringComponent SetTarget(Quaternion targetQuaternion)
		{
			springRotation.SetTarget(targetQuaternion);
			return this;
		}

		/// <summary>
		/// Sets the target value that the spring will move towards.
		/// </summary>
		/// <param name="targetEuler">The new target euler angles.</param>
		/// <returns>This component instance for method chaining.</returns>
		public RotationSpringComponent SetTarget(Vector3 targetEuler)
		{
			springRotation.SetTarget(targetEuler);
			return this;
		}
		public Quaternion GetCurrentValue() => springRotation.GetCurrentValue();
		/// <summary>
		/// Sets the current value of the spring.
		/// </summary>
		/// <param name="currentQuaternion">The new current quaternion.</param>
		/// <returns>This component instance for method chaining.</returns>
		public RotationSpringComponent SetCurrentValue(Quaternion currentQuaternion)
		{
			springRotation.SetCurrentValue(currentQuaternion);
			return this;
		}

		/// <summary>
		/// Sets the current value of the spring.
		/// </summary>
		/// <param name="currentEuler">The new current euler angles.</param>
		/// <returns>This component instance for method chaining.</returns>
		public RotationSpringComponent SetCurrentValue(Vector3 currentEuler)
		{
			springRotation.SetCurrentValue(currentEuler);
			return this;
		}

		public Vector3 GetVelocity() => springRotation.GetVelocity();

		/// <summary>
		/// Sets the velocity of the spring.
		/// </summary>
		/// <param name="velocity">The new velocity value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public RotationSpringComponent SetVelocity(Vector3 velocity)
		{
			springRotation.SetVelocity(velocity);
			return this;
		}

		/// <summary>
		/// Adds velocity to the current spring velocity.
		/// </summary>
		/// <param name="velocityToAdd">The velocity to add.</param>
		/// <returns>This component instance for method chaining.</returns>
		public RotationSpringComponent AddVelocity(Vector3 velocityToAdd)
		{
			springRotation.AddVelocity(velocityToAdd);
			return this;
		}
		public float GetCommonForce() => springRotation.GetCommonForce();
		public float GetCommonDrag() => springRotation.GetCommonDrag();
		/// <summary>
		/// Sets the common force (stiffness) value.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public RotationSpringComponent SetCommonForce(float force)
		{
			springRotation.SetCommonForceAndDrag(true);
			springRotation.SetCommonForce(force);
			return this;
		}

		/// <summary>
		/// Sets the common drag (damping) value.
		/// </summary>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public RotationSpringComponent SetCommonDrag(float drag)
		{
			springRotation.SetCommonForceAndDrag(true);
			springRotation.SetCommonDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets both common force and drag values.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public RotationSpringComponent SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
			return this;
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
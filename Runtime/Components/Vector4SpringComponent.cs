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
		/// <summary>
		/// Sets the target value that the spring will move towards.
		/// </summary>
		/// <param name="target">The new target value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetTarget(Vector4 target)
		{
			springVector4.SetTarget(target);
			return this;
		}

		/// <summary>
		/// Sets the target value using a single float for all axes.
		/// </summary>
		/// <param name="target">The target value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetTarget(float target) => SetTarget(Vector4.one * target);

		public Vector4 GetCurrentValue() => springVector4.GetCurrentValue();

		/// <summary>
		/// Sets the current value of the spring.
		/// </summary>
		/// <param name="currentValues">The new current value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetCurrentValue(Vector4 currentValues)
		{
			springVector4.SetCurrentValue(currentValues);
			return this;
		}

		/// <summary>
		/// Sets the current value using a single float for all axes.
		/// </summary>
		/// <param name="currentValues">The current value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetCurrentValue(float currentValues) => SetCurrentValue(Vector4.one * currentValues);

		public Vector4 GetVelocity() => springVector4.GetVelocity();

		/// <summary>
		/// Sets the velocity of the spring.
		/// </summary>
		/// <param name="velocity">The new velocity value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetVelocity(Vector4 velocity)
		{
			springVector4.SetVelocity(velocity);
			return this;
		}

		/// <summary>
		/// Sets the velocity using a single float for all axes.
		/// </summary>
		/// <param name="velocity">The velocity for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetVelocity(float velocity) => SetVelocity(Vector4.one * velocity);

		/// <summary>
		/// Adds velocity to the current spring velocity.
		/// </summary>
		/// <param name="velocityToAdd">The velocity to add.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent AddVelocity(Vector4 velocityToAdd)
		{
			springVector4.AddVelocity(velocityToAdd);
			return this;
		}
		public Vector4 GetForce() => springVector4.GetForce();
		/// <summary>
		/// Sets the force values.
		/// </summary>
		/// <param name="force">The force vector.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetForce(Vector4 force)
		{
			springVector4.SetForce(force);
			return this;
		}

		/// <summary>
		/// Sets the force using a single float for all axes.
		/// </summary>
		/// <param name="force">The force for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetForce(float force) => SetForce(Vector4.one * force);

		public Vector4 GetDrag() => springVector4.GetDrag();

		/// <summary>
		/// Sets the drag values.
		/// </summary>
		/// <param name="drag">The drag vector.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetDrag(Vector4 drag)
		{
			springVector4.SetDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets the drag using a single float for all axes.
		/// </summary>
		/// <param name="drag">The drag for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetDrag(float drag) => SetDrag(Vector4.one * drag);
		public float GetCommonForce() => springVector4.GetCommonForce();
		public float GetCommonDrag() => springVector4.GetCommonDrag();
		/// <summary>
		/// Sets the common force (stiffness) value.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetCommonForce(float force)
		{
			springVector4.SetCommonForceAndDrag(true);
			springVector4.SetCommonForce(force);
			return this;
		}

		/// <summary>
		/// Sets the common drag (damping) value.
		/// </summary>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetCommonDrag(float drag)
		{
			springVector4.SetCommonForceAndDrag(true);
			springVector4.SetCommonDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets both common force and drag values.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets the minimum values for clamping.
		/// </summary>
		/// <param name="minValue">The minimum values.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetMinValues(Vector4 minValue)
		{
			springVector4.SetMinValues(minValue);
			return this;
		}

		/// <summary>
		/// Sets the minimum values using a single float for all axes.
		/// </summary>
		/// <param name="minValue">The minimum value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetMinValues(float minValue) => SetMinValues(Vector4.one * minValue);

		/// <summary>
		/// Sets the maximum values for clamping.
		/// </summary>
		/// <param name="maxValue">The maximum values.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetMaxValues(Vector4 maxValue)
		{
			springVector4.SetMaxValues(maxValue);
			return this;
		}

		/// <summary>
		/// Sets the maximum values using a single float for all axes.
		/// </summary>
		/// <param name="maxValue">The maximum value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetMaxValues(float maxValue) => SetMaxValues(Vector4.one * maxValue);

		/// <summary>
		/// Sets clamping for current values per axis.
		/// </summary>
		/// <param name="clampTargetX">Clamp X axis.</param>
		/// <param name="clampTargetY">Clamp Y axis.</param>
		/// <param name="clampTargetZ">Clamp Z axis.</param>
		/// <param name="clampTargetW">Clamp W axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetClampCurrentValues(bool clampTargetX, bool clampTargetY, bool clampTargetZ, bool clampTargetW)
		{
			springVector4.SetClampCurrentValues(clampTargetX, clampTargetY, clampTargetZ, clampTargetW);
			return this;
		}

		/// <summary>
		/// Sets clamping for target values per axis.
		/// </summary>
		/// <param name="clampTargetX">Clamp X axis.</param>
		/// <param name="clampTargetY">Clamp Y axis.</param>
		/// <param name="clampTargetZ">Clamp Z axis.</param>
		/// <param name="clampTargetW">Clamp W axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent SetClampTarget(bool clampTargetX, bool clampTargetY, bool clampTargetZ, bool clampTargetW)
		{
			springVector4.SetClampTarget(clampTargetX, clampTargetY, clampTargetZ, clampTargetW);
			return this;
		}

		/// <summary>
		/// Sets stop on clamp per axis.
		/// </summary>
		/// <param name="stopX">Stop on clamp X axis.</param>
		/// <param name="stopY">Stop on clamp Y axis.</param>
		/// <param name="stopZ">Stop on clamp Z axis.</param>
		/// <param name="stopW">Stop on clamp W axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector4SpringComponent StopSpringOnClamp(bool stopX, bool stopY, bool stopZ, bool stopW)
		{
			springVector4.StopSpringOnClamp(stopX, stopY, stopZ, stopW);
			return this;
		}

		public override void ReachEquilibrium()
		{
			base.ReachEquilibrium();
		}

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
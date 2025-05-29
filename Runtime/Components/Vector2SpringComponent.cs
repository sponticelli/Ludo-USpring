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
		/// <summary>
		/// Sets the target value that the spring will move towards.
		/// </summary>
		/// <param name="target">The new target value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetTarget(Vector2 target)
		{
			springVector2.SetTarget(target);
			return this;
		}

		/// <summary>
		/// Sets the target value using a single float for all axes.
		/// </summary>
		/// <param name="target">The target value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetTarget(float target) => SetTarget(Vector2.one * target);

		public Vector2 GetCurrentValue() => springVector2.GetCurrentValue();

		/// <summary>
		/// Sets the current value of the spring.
		/// </summary>
		/// <param name="currentValues">The new current value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetCurrentValue(Vector2 currentValues)
		{
			springVector2.SetCurrentValue(currentValues);
			return this;
		}

		/// <summary>
		/// Sets the current value using a single float for all axes.
		/// </summary>
		/// <param name="currentValues">The current value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetCurrentValue(float currentValues) => SetCurrentValue(Vector2.one * currentValues);

		public Vector2 GetVelocity() => springVector2.GetVelocity();

		/// <summary>
		/// Sets the velocity of the spring.
		/// </summary>
		/// <param name="velocity">The new velocity value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetVelocity(Vector2 velocity)
		{
			springVector2.SetVelocity(velocity);
			return this;
		}

		/// <summary>
		/// Sets the velocity using a single float for all axes.
		/// </summary>
		/// <param name="velocity">The velocity for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetVelocity(float velocity) => SetVelocity(Vector2.one * velocity);

		/// <summary>
		/// Adds velocity to the current spring velocity.
		/// </summary>
		/// <param name="velocityToAdd">The velocity to add.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent AddVelocity(Vector2 velocityToAdd)
		{
			springVector2.AddVelocity(velocityToAdd);
			return this;
		}
		public Vector2 GetForce() => springVector2.GetForce();
		/// <summary>
		/// Sets the force values.
		/// </summary>
		/// <param name="force">The force vector.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetForce(Vector2 force)
		{
			springVector2.SetForce(force);
			return this;
		}

		/// <summary>
		/// Sets the force using a single float for all axes.
		/// </summary>
		/// <param name="force">The force for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetForce(float force) => SetForce(Vector2.one * force);

		public Vector2 GetDrag() => springVector2.GetDrag();

		/// <summary>
		/// Sets the drag values.
		/// </summary>
		/// <param name="drag">The drag vector.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetDrag(Vector2 drag)
		{
			springVector2.SetDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets the drag using a single float for all axes.
		/// </summary>
		/// <param name="drag">The drag for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetDrag(float drag) => SetDrag(Vector2.one * drag);
		public float GetCommonForce() => springVector2.GetCommonForce();
		public float GetCommonDrag() => springVector2.GetCommonDrag();
		/// <summary>
		/// Sets the common force (stiffness) value.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetCommonForce(float force)
		{
			springVector2.SetCommonForceAndDrag(true);
			springVector2.SetCommonForce(force);
			return this;
		}

		/// <summary>
		/// Sets the common drag (damping) value.
		/// </summary>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetCommonDrag(float drag)
		{
			springVector2.SetCommonForceAndDrag(true);
			springVector2.SetCommonDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets both common force and drag values.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetCommonForceAndDrag(float force, float drag)
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
		public Vector2SpringComponent SetMinValues(Vector2 minValue)
		{
			springVector2.SetMinValue(minValue);
			return this;
		}

		/// <summary>
		/// Sets the minimum values using a single float for all axes.
		/// </summary>
		/// <param name="minValue">The minimum value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetMinValues(float minValue) => SetMinValues(Vector2.one * minValue);

		/// <summary>
		/// Sets the maximum values for clamping.
		/// </summary>
		/// <param name="maxValue">The maximum values.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetMaxValues(Vector2 maxValue)
		{
			springVector2.SetMaxValues(maxValue);
			return this;
		}

		/// <summary>
		/// Sets the maximum values using a single float for all axes.
		/// </summary>
		/// <param name="maxValue">The maximum value for all axes.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetMaxValues(float maxValue) => SetMaxValues(Vector2.one * maxValue);

		/// <summary>
		/// Sets clamping for current values per axis.
		/// </summary>
		/// <param name="clampTargetX">Clamp X axis.</param>
		/// <param name="clampTargetY">Clamp Y axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetClampCurrentValues(bool clampTargetX, bool clampTargetY)
		{
			springVector2.SetClampCurrentValues(clampTargetX, clampTargetY);
			return this;
		}

		/// <summary>
		/// Sets clamping for target values per axis.
		/// </summary>
		/// <param name="clampTargetX">Clamp X axis.</param>
		/// <param name="clampTargetY">Clamp Y axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent SetClampTarget(bool clampTargetX, bool clampTargetY)
		{
			springVector2.SetClampTarget(clampTargetX, clampTargetY);
			return this;
		}

		/// <summary>
		/// Sets stop on clamp per axis.
		/// </summary>
		/// <param name="stopX">Stop on clamp X axis.</param>
		/// <param name="stopY">Stop on clamp Y axis.</param>
		/// <returns>This component instance for method chaining.</returns>
		public Vector2SpringComponent StopSpringOnClamp(bool stopX, bool stopY)
		{
			springVector2.StopSpringOnClamp(stopX, stopY);
			return this;
		}

		public override void ReachEquilibrium()
		{
			base.ReachEquilibrium();
		}

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
using UnityEngine;
using USpring.Core.Interfaces;
using USpring.Physics;

namespace USpring.Core
{

	/// <summary>
	/// Represents an abstract base class for a spring system that simulates damped harmonic oscillation.
	/// Provides common functionality and structure for derived spring implementations like SpringFloat, SpringVector3, etc.
	/// </summary>
	/// <remarks>
	/// The Spring class is the foundation of the USpring system, implementing a physically-based
	/// spring animation model. It manages an array of SpringValues that represent individual
	/// spring-driven values (e.g., x, y, z components of a vector).
	///
	/// Springs are configured with force (stiffness) and drag (damping) parameters that control
	/// how quickly and smoothly they move toward their target values.
	/// </remarks>
	public abstract class Spring : ISpring
	{
		/// <summary>
		/// Threshold for determining when a spring's velocity is considered effectively zero.
		/// Used in IsCloseToStopping() to determine if the spring has come to rest.
		/// </summary>
		public const float SpringVelocityThreshold = 0.01f;

		/// <summary>
		/// Threshold for determining when a spring's current value is considered close enough to its target.
		/// Used in IsOnTarget() to determine if the spring has reached its destination.
		/// </summary>
		public const float SpringTargetDistanceThreshold = 0.005f;

		/// <summary>
		/// Threshold for determining when a change in the spring's value is significant enough to trigger events.
		/// Used in HasCurrentValueChanged() to detect meaningful changes.
		/// </summary>
		public const float SpringChangeThreshold = 0.01f;

		/// <summary>
		/// When enabled, uses the same Force and Drag values for all axes.
		/// When disabled, allows setting different Force and Drag values for each axis.
		/// </summary>
		[Tooltip("When enabled, uses the same Force and Drag values for all axes. When disabled, allows setting different Force and Drag values for each axis")]
		public bool commonForceAndDrag;

		/// <summary>
		/// Controls the stiffness of the spring. Higher values make the spring react more quickly to changes.
		/// Only used when commonForceAndDrag is true.
		/// </summary>
		[Tooltip("Higher values make the spring react more quickly to changes")]
		public float commonForce;

		/// <summary>
		/// Controls the damping of the spring. Higher values make the spring more stable with less bouncing.
		/// Only used when commonForceAndDrag is true.
		/// </summary>
		[Tooltip("Higher values make the spring more stable with less bouncing")]
		public float commonDrag;

		/// <summary>
		/// When true, the spring will use the initialValue from each SpringValues instead of the current value.
		/// </summary>
		public bool useInitialValues;

		/// <summary>
		/// When true, the spring will use a custom target value instead of automatically determining it.
		/// </summary>
		public bool useCustomTarget;

		/// <summary>
		/// Controls whether the spring simulation is active. When false, the spring will not update.
		/// </summary>
		public bool springEnabled;

		/// <summary>
		/// Controls whether value clamping is enabled. When true, spring values will be clamped
		/// according to the min/max values and clamping settings in each SpringValues.
		/// </summary>
		public bool clampingEnabled;

		/// <summary>
		/// Array of SpringValues that represent the individual spring-driven values.
		/// The size of this array depends on the specific spring implementation (e.g., 1 for SpringFloat, 3 for SpringVector3).
		/// </summary>
		public SpringValues[] springValues;

		/// <summary>
		/// Cached physics parameters for each spring value.
		/// This array is used to avoid creating new PhysicsParameters objects on every update.
		/// </summary>
		private PhysicsParameters[] cachedPhysicsParameters;

		/// <summary>
		/// Flag indicating whether the cached physics parameters need to be updated.
		/// This is set to true when force, drag, or clamping parameters change.
		/// </summary>
		private bool physicsParametersDirty = true;

		/// <summary>
		/// Used by the editor to control debug field visibility.
		/// </summary>
		[HideInInspector] public bool showDebugFields;

		/// <summary>
		/// Used by the editor to control foldout state.
		/// </summary>
		[HideInInspector] public bool unfolded;

		/// <summary>
		/// Controls whether spring events are enabled. When true, the spring will trigger events
		/// like OnTargetReached, OnCurrentValueChanged, and OnClampingApplied.
		/// </summary>
		public bool eventsEnabled;

		/// <summary>
		/// The SpringEvents instance that manages event handling for this spring.
		/// </summary>
		public SpringEvents springEvents;

		/// <summary>
		/// Initializes a new instance of the Spring class with the specified number of spring values.
		/// </summary>
		/// <param name="size">The number of spring values to create (e.g., 1 for float, 3 for Vector3).</param>
		public Spring(int size)
		{
			springValues = new SpringValues[size];
			cachedPhysicsParameters = new PhysicsParameters[size];

			// Set default values
			commonForceAndDrag = true;
			commonForce = 150f;
			commonDrag = 10f;

			springEnabled = true;

			// Initialize each spring value and cached physics parameters
			for (int i = 0; i < size; i++)
			{
				springValues[i] = new SpringValues();
				cachedPhysicsParameters[i] = new PhysicsParameters();
			}

			// Mark parameters as dirty to ensure they're updated on first use
			physicsParametersDirty = true;
		}

		/// <summary>
		/// Checks if target clamping is enabled for any of the spring values.
		/// </summary>
		/// <returns>True if at least one spring value has target clamping enabled, false otherwise.</returns>
		public bool IsClampTargetEnabled()
		{
			bool res = false;

			for (int i = 0; i < springValues.Length; i++)
			{
				res = res || springValues[i].GetClampTarget();
			}

			return res;
		}

		/// <summary>
		/// Checks if current value clamping is enabled for any of the spring values.
		/// </summary>
		/// <returns>True if at least one spring value has current value clamping enabled, false otherwise.</returns>
		public bool IsClampCurrentValueEnabled()
		{
			bool res = false;

			for (int i = 0; i < springValues.Length; i++)
			{
				res = res || springValues[i].GetClampCurrentValue();
			}

			return res;
		}

		/// <summary>
		/// Immediately sets all spring values to their target values and stops all motion.
		/// This effectively puts the spring in a state of equilibrium.
		/// </summary>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpring ReachEquilibrium()
		{
			for (int i = 0; i < springValues.Length; i++)
			{
				springValues[i].ReachEquilibrium();
			}

			return this;
		}

		/// <summary>
		/// Updates the spring physics for the given time step.
		/// This method handles the complete update process including:
		/// - Updating the spring physics
		/// - Processing candidate values
		/// - Checking events
		/// </summary>
		/// <param name="deltaTime">The time step to use for the update.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpring Update(float deltaTime)
		{
			return Update(deltaTime, 7500f); // Default force threshold
		}

		/// <summary>
		/// Updates the spring physics for the given time step with a custom force threshold.
		/// </summary>
		/// <param name="deltaTime">The time step to use for the update.</param>
		/// <param name="forceThreshold">The threshold for switching from semi-implicit to analytical integration.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpring Update(float deltaTime, float forceThreshold)
		{
			if (!springEnabled)
			{
				return this;
			}

			SpringMath.UpdateSpring(deltaTime, this, forceThreshold);
			ProcessCandidateValue();

			if (eventsEnabled)
			{
				CheckEvents();
			}

			return this;
		}

		/// <summary>
		/// Determines if the spring has the correct number of spring values for its type.
		/// </summary>
		/// <returns>True if the spring has the correct number of spring values, false otherwise.</returns>
		public abstract bool HasValidSize();

		/// <summary>
		/// Gets the expected number of spring values for this spring type.
		/// </summary>
		/// <returns>The expected number of spring values (e.g., 1 for SpringFloat, 3 for SpringVector3).</returns>
		public abstract int GetSpringSize();

		/// <summary>
		/// Checks if the spring has the correct number of spring values and recreates them if not.
		/// </summary>
		/// <returns>True if the spring already had the correct number of spring values, false if they were recreated.</returns>
		public bool CheckCorrectSize()
		{
			bool res = HasValidSize();
			if (!res)
			{
				// If the size is incorrect, recreate the spring values with the correct size
				int springSize = GetSpringSize();
				springValues = new SpringValues[springSize];
				cachedPhysicsParameters = new PhysicsParameters[springSize];

				for (int i = 0; i < springSize; i++)
				{
					springValues[i] = new SpringValues();
					cachedPhysicsParameters[i] = new PhysicsParameters();
				}

				// Mark parameters as dirty to ensure they're updated on next use
				physicsParametersDirty = true;
			}

			return res;
		}

		/// <summary>
		/// Initializes the spring by setting up each spring value and configuring the spring events.
		/// </summary>
		/// <returns>The spring instance for method chaining.</returns>
		public virtual ISpring Initialize()
		{
			for (int i = 0; i < springValues.Length; i++)
			{
				springValues[i].Initialize();
			}

			if (springEvents == null)
			{
				springEvents = new SpringEvents();
			}

			springEvents.SetSpring(this);

			return this;
		}

		/// <summary>
		/// Applies the candidate values calculated during the spring update to the current values.
		/// This is typically called after SpringMath.UpdateSpring() has calculated new candidate values.
		/// </summary>
		public virtual void ProcessCandidateValue()
		{
			for(int i = 0; i < springValues.Length; i++)
			{
				springValues[i].ApplyCandidateValue();
			}
		}

		/// <summary>
		/// Checks and triggers any spring events that should fire based on the current state.
		/// </summary>
		public void CheckEvents()
		{
			springEvents.CheckEvents();
		}

		/// <summary>
		/// Checks if any of the spring values are currently clamped.
		/// </summary>
		/// <returns>True if at least one spring value is clamped, false otherwise.</returns>
		public bool IsClamped()
		{
			bool res = false;

			for (int i = 0; i < springValues.Length; i++)
			{
				res = res || springValues[i].IsClamped();
			}

			return res;
		}

		/// <summary>
		/// Determines if the spring is close to stopping (velocity is below threshold).
		/// </summary>
		/// <returns>True if all enabled spring values have velocities below the threshold, false otherwise.</returns>
		public bool IsCloseToStopping()
		{
			bool res = true;

			for(int i = 0; i < springValues.Length; i++)
			{
				if (!springValues[i].IsEnabled())
				{
					continue;
				}

				float absVelocity = Mathf.Abs(springValues[i].GetVelocity());
				res = res && (absVelocity <= SpringVelocityThreshold);
			}

			return res;
		}

		/// <summary>
		/// Determines if the spring has reached its target (current value is close enough to target).
		/// </summary>
		/// <returns>True if all enabled spring values are close enough to their targets, false otherwise.</returns>
		public bool IsOnTarget()
		{
			bool res = true;

			for (int i = 0; i < springValues.Length; i++)
			{
				if (!springValues[i].IsEnabled())
				{
					continue;
				}

				float distanceToTarget = Mathf.Abs(springValues[i].GetTarget() - springValues[i].GetCurrentValue());
				res = res && (distanceToTarget <= SpringTargetDistanceThreshold);
			}

			return res;
		}

		/// <summary>
		/// Determines if any of the spring values have changed significantly since the last update.
		/// </summary>
		/// <returns>True if at least one enabled spring value has changed by more than the threshold, false otherwise.</returns>
		public bool HasCurrentValueChanged()
		{
			bool res = false;

			for (int i = 0; i < springValues.Length; i++)
			{
				if (!springValues[i].IsEnabled())
				{
					continue;
				}

				float delta = Mathf.Abs(springValues[i].GetCandidateValue() - springValues[i].GetCurrentValue());
				res = res || (delta >= SpringChangeThreshold);
			}

			return res;
		}


		/// <summary>
		/// Enables or disables the spring.
		/// </summary>
		/// <param name="enabled">Whether the spring should be enabled.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpring SetEnabled(bool enabled)
		{
			springEnabled = enabled;
			return this;
		}

		/// <summary>
		/// Gets whether the spring is enabled.
		/// </summary>
		/// <returns>True if the spring is enabled, false otherwise.</returns>
		public bool IsEnabled()
		{
			return springEnabled;
		}

		/// <summary>
		/// Enables or disables common force and drag for all axes.
		/// </summary>
		/// <param name="enabled">Whether to use common force and drag.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpring SetCommonForceAndDrag(bool enabled)
		{
			commonForceAndDrag = enabled;
			physicsParametersDirty = true;
			return this;
		}

		/// <summary>
		/// Gets the physics parameters for a specific spring value.
		/// If the parameters are dirty, they will be updated before being returned.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <param name="forceThreshold">The threshold for switching from semi-implicit to analytical integration.</param>
		/// <returns>The physics parameters for the specified spring value.</returns>
		public PhysicsParameters GetPhysicsParameters(int index, float forceThreshold)
		{
			// If the parameters are dirty or don't exist, update them
			if (physicsParametersDirty || cachedPhysicsParameters[index] == null)
			{
				UpdatePhysicsParameters(index, forceThreshold);
			}

			// Double-check that the parameters are valid
			if (cachedPhysicsParameters[index] == null || !PhysicsValidator.AreParametersValid(cachedPhysicsParameters[index]))
			{
				Debug.LogWarning($"Invalid physics parameters for spring {GetType().Name} at index {index}. Creating new default parameters.");
				cachedPhysicsParameters[index] = new PhysicsParameters();
				// Set default values based on spring values to ensure they're valid
				cachedPhysicsParameters[index].Force = Mathf.Max(0.0001f, commonForceAndDrag ? commonForce : springValues[index].GetForce());
				cachedPhysicsParameters[index].Drag = Mathf.Max(0f, commonForceAndDrag ? commonDrag : springValues[index].GetDrag());
			}

			return cachedPhysicsParameters[index];
		}

		/// <summary>
		/// Updates the cached physics parameters for a specific spring value.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <param name="forceThreshold">The threshold for switching from semi-implicit to analytical integration.</param>
		private void UpdatePhysicsParameters(int index, float forceThreshold)
		{
			// Create a new parameters object if it doesn't exist
			if (cachedPhysicsParameters[index] == null)
			{
				cachedPhysicsParameters[index] = new PhysicsParameters();
			}

			// Update force and drag
			float force = commonForceAndDrag ? commonForce : springValues[index].GetForce();
			float drag = commonForceAndDrag ? commonDrag : springValues[index].GetDrag();

			// Ensure force and drag are valid (non-negative)
			force = Mathf.Max(0.0001f, force);
			drag = Mathf.Max(0f, drag);

			cachedPhysicsParameters[index].Force = force;
			cachedPhysicsParameters[index].Drag = drag;

			// Update integration parameters
			cachedPhysicsParameters[index].IntegrationParameters.ForceThreshold = forceThreshold;
			cachedPhysicsParameters[index].IntegrationParameters.AlwaysUseAnalyticalSolution = forceThreshold < 0f;

			// Update clamping parameters
			cachedPhysicsParameters[index].ClampingParameters.ClampTarget = springValues[index].GetClampTarget();
			cachedPhysicsParameters[index].ClampingParameters.ClampCurrentValue = springValues[index].GetClampCurrentValue();
			cachedPhysicsParameters[index].ClampingParameters.StopSpringOnCurrentValueClamp = springValues[index].GetStopOnClamp();

			// Ensure min/max values are valid
			float minValue = springValues[index].GetMinValue();
			float maxValue = springValues[index].GetMaxValue();
			if (minValue > maxValue)
			{
				// Swap values if min is greater than max
				float temp = minValue;
				minValue = maxValue;
				maxValue = temp;

				// Update the spring values to maintain consistency
				springValues[index].SetMinValue(minValue);
				springValues[index].SetMaxValue(maxValue);
			}

			cachedPhysicsParameters[index].ClampingParameters.MinValue = minValue;
			cachedPhysicsParameters[index].ClampingParameters.MaxValue = maxValue;

			// Validate the parameters after updating
			if (!PhysicsValidator.AreParametersValid(cachedPhysicsParameters[index]))
			{
				Debug.LogWarning($"Invalid physics parameters for spring {GetType().Name} at index {index}. Using default values.");
				cachedPhysicsParameters[index] = new PhysicsParameters();
			}

			// Reset dirty flag for this index
			if (index == springValues.Length - 1)
			{
				physicsParametersDirty = false;
			}
		}

		/// <summary>
		/// Gets the common force (stiffness) value used when commonForceAndDrag is true.
		/// </summary>
		/// <returns>The common force value.</returns>
		public float GetCommonForce()
		{
			return commonForce;
		}

		/// <summary>
		/// Sets the common force (stiffness) value used when commonForceAndDrag is true.
		/// </summary>
		/// <param name="force">The new common force value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpring SetCommonForce(float force)
		{
			this.commonForce = force;
			physicsParametersDirty = true;
			return this;
		}

		/// <summary>
		/// Gets the common drag (damping) value used when commonForceAndDrag is true.
		/// </summary>
		/// <returns>The common drag value.</returns>
		public float GetCommonDrag()
		{
			return commonDrag;
		}

		/// <summary>
		/// Sets the common drag (damping) value used when commonForceAndDrag is true.
		/// </summary>
		/// <param name="drag">The new common drag value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpring SetCommonDrag(float drag)
		{
			this.commonDrag = drag;
			physicsParametersDirty = true;
			return this;
		}

		/// <summary>
		/// Sets both common force and drag values in a single call.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <param name="drag">The drag value.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpring SetCommonForceAndDragValues(float force, float drag)
		{
			this.commonForce = force;
			this.commonDrag = drag;
			physicsParametersDirty = true;
			return this;
		}

		/// <summary>
		/// Gets the force (stiffness) value for a specific spring value.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <returns>The force value for the specified spring value.</returns>
		public float GetForceByIndex(int index)
		{
			float res = springValues[index].GetForce();
			return res;
		}

		/// <summary>
		/// Sets the force (stiffness) value for a specific spring value.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <param name="force">The new force value.</param>
		public void SetForceByIndex(int index, float force)
		{
			springValues[index].SetForce(force);
			physicsParametersDirty = true;
		}

		/// <summary>
		/// Gets the drag (damping) value for a specific spring value.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <returns>The drag value for the specified spring value.</returns>
		public float GetDragByIndex(int index)
		{
			float res = springValues[index].GetDrag();
			return res;
		}

		/// <summary>
		/// Sets the drag (damping) value for a specific spring value.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <param name="drag">The new drag value.</param>
		public void SetDragByIndex(int index, float drag)
		{
			springValues[index].SetDrag(drag);
			physicsParametersDirty = true;
		}

		/// <summary>
		/// Enables or disables clamping for all spring values.
		/// </summary>
		/// <param name="clampingEnabled">True to enable clamping, false to disable it.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpring SetClampingEnabled(bool clampingEnabled)
		{
			this.clampingEnabled = clampingEnabled;
			physicsParametersDirty = true;
			return this;
		}

		/// <summary>
		/// Gets whether clamping is enabled for the spring.
		/// </summary>
		/// <returns>True if clamping is enabled, false otherwise.</returns>
		public bool IsClampingEnabled()
		{
			return clampingEnabled;
		}

		/// <summary>
		/// Enables or disables events for the spring.
		/// </summary>
		/// <param name="enabled">Whether events should be enabled.</param>
		/// <returns>The spring instance for method chaining.</returns>
		public ISpring SetEventsEnabled(bool enabled)
		{
			eventsEnabled = enabled;
			return this;
		}

		/// <summary>
		/// Gets whether events are enabled for the spring.
		/// </summary>
		/// <returns>True if events are enabled, false otherwise.</returns>
		public bool AreEventsEnabled()
		{
			return eventsEnabled;
		}

		/// <summary>
		/// Gets the spring events instance for this spring.
		/// </summary>
		/// <returns>The spring events instance.</returns>
		public SpringEvents GetEvents()
		{
			return springEvents;
		}

		/// <summary>
		/// Sets the minimum value for a specific spring value.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <param name="minValue">The new minimum value.</param>
		protected void SetMinValueByIndex(int index, float minValue)
		{
			springValues[index].SetMinValue(minValue);
			physicsParametersDirty = true;
		}

		/// <summary>
		/// Sets the maximum value for a specific spring value.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <param name="maxValue">The new maximum value.</param>
		protected void SetMaxValueByIndex(int index, float maxValue)
		{
			springValues[index].SetMaxValue(maxValue);
			physicsParametersDirty = true;
		}

		/// <summary>
		/// Sets whether to stop the spring when the current value is clamped for a specific spring value.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <param name="stop">True to stop the spring when clamped, false to allow it to continue.</param>
		protected void SetStopSpringOnCurrentValueClampByIndex(int index, bool stop)
		{
			springValues[index].SetStopOnClamp(stop);
			physicsParametersDirty = true;
		}

		/// <summary>
		/// Sets whether to clamp the target value for a specific spring value.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <param name="clampTarget">True to clamp the target value, false to allow any target value.</param>
		protected void SetClampTargetByIndex(int index, bool clampTarget)
		{
			springValues[index].SetClampTarget(clampTarget);
			physicsParametersDirty = true;
		}

		/// <summary>
		/// Sets whether to clamp the current value for a specific spring value.
		/// </summary>
		/// <param name="index">The index of the spring value.</param>
		/// <param name="clampCurrentValue">True to clamp the current value, false to allow any current value.</param>
		protected void SetClampCurrentValueByIndex(int index, bool clampCurrentValue)
		{
			springValues[index].SetClampCurrentValue(clampCurrentValue);
			physicsParametersDirty = true;
		}

#if UNITY_EDITOR
		/// <summary>
		/// Draws gizmos for the spring in the Unity Editor's Scene view when the object is selected.
		/// This method is only available in the Unity Editor and is not compiled into builds.
		/// </summary>
		/// <param name="componentPosition">The position of the component in world space.</param>
		internal virtual void DrawGizmosSelected(Vector3 componentPosition)
		{
			// Base implementation does nothing, derived classes can override to draw custom gizmos
		}
#endif
	}
}
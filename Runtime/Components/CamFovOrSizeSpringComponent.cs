using UnityEngine;
using System.Collections.Generic;
using USpring.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Cam Fov Or Size Spring")]
	public partial class CamFovOrSizeSpringComponent : SpringComponent
	{
		[SerializeField] private SpringFloat fovSpring = new SpringFloat();

		[SerializeField] private Camera autoUpdatedCamera;

		public SpringEvents Events => fovSpring.springEvents;
		public float GetTarget() => fovSpring.GetTarget();
		/// <summary>
		/// Sets the target value that the spring will move towards.
		/// </summary>
		/// <param name="target">The new target value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetTarget(float target)
		{
			fovSpring.SetTarget(target);
			return this;
		}

		public float GetCurrentValue() => fovSpring.GetCurrentValue();

		/// <summary>
		/// Sets the current value of the spring.
		/// </summary>
		/// <param name="currentValues">The new current value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetCurrentValue(float currentValues)
		{
			fovSpring.SetCurrentValue(currentValues);
			return this;
		}

		public float GetVelocity() => fovSpring.GetVelocity();

		/// <summary>
		/// Sets the velocity of the spring.
		/// </summary>
		/// <param name="velocity">The new velocity value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetVelocity(float velocity)
		{
			fovSpring.SetVelocity(velocity);
			return this;
		}

		/// <summary>
		/// Adds velocity to the current spring velocity.
		/// </summary>
		/// <param name="velocityToAdd">The velocity to add.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent AddVelocity(float velocityToAdd)
		{
			fovSpring.AddVelocity(velocityToAdd);
			return this;
		}

		public float GetForce() => fovSpring.GetForce();

		/// <summary>
		/// Sets the force value.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetForce(float force)
		{
			fovSpring.SetForce(force);
			return this;
		}

		public float GetDrag() => fovSpring.GetDrag();

		/// <summary>
		/// Sets the drag value.
		/// </summary>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetDrag(float drag)
		{
			fovSpring.SetDrag(drag);
			return this;
		}
		/// <summary>
		/// Sets the minimum value for clamping.
		/// </summary>
		/// <param name="minValue">The minimum value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetMinValues(float minValue)
		{
			fovSpring.SetMinValue(minValue);
			return this;
		}

		/// <summary>
		/// Sets the maximum value for clamping.
		/// </summary>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetMaxValues(float maxValue)
		{
			fovSpring.SetMaxValue(maxValue);
			return this;
		}

		/// <summary>
		/// Sets whether to clamp the current value.
		/// </summary>
		/// <param name="clamp">Whether to clamp the current value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetClampCurrentValues(bool clamp)
		{
			fovSpring.SetClampCurrentValue(clamp);
			return this;
		}

		/// <summary>
		/// Sets whether to clamp the target value.
		/// </summary>
		/// <param name="clamp">Whether to clamp the target value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetClampTarget(bool clamp)
		{
			fovSpring.SetClampTarget(clamp);
			return this;
		}

		/// <summary>
		/// Sets whether to stop the spring when clamping occurs.
		/// </summary>
		/// <param name="stop">Whether to stop on clamp.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent StopSpringOnClamp(bool stop)
		{
			fovSpring.SetStopOnClamp(stop);
			return this;
		}
		public float GetCommonForce() => fovSpring.GetCommonForce();
		public float GetCommonDrag() => fovSpring.GetCommonDrag();
		/// <summary>
		/// Sets the common force (stiffness) value.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetCommonForce(float force)
		{
			fovSpring.SetCommonForceAndDrag(true);
			fovSpring.SetCommonForce(force);
			return this;
		}

		/// <summary>
		/// Sets the common drag (damping) value.
		/// </summary>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetCommonDrag(float drag)
		{
			fovSpring.SetCommonForceAndDrag(true);
			fovSpring.SetCommonDrag(drag);
			return this;
		}

		/// <summary>
		/// Sets both common force and drag values.
		/// </summary>
		/// <param name="force">The force value.</param>
		/// <param name="drag">The drag value.</param>
		/// <returns>This component instance for method chaining.</returns>
		public CamFovOrSizeSpringComponent SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
			return this;
		}

		protected override void RegisterSprings()
		{
			RegisterSpring(fovSpring);
		}

		public override void Initialize()
		{
			FindCamera();
			base.Initialize();
		}

		public void Update()
        {
			if (!initialized) { return; }

			UpdateCamera();
		}

		private void UpdateCamera()
		{
			if (autoUpdatedCamera.orthographic)
			{
				autoUpdatedCamera.orthographicSize = fovSpring.GetCurrentValue();
			}
			else
			{
				autoUpdatedCamera.fieldOfView = fovSpring.GetCurrentValue();
			}
		}

		public override bool IsValidSpringComponent()
		{
			if(autoUpdatedCamera == null)
			{
				AddErrorReason($"No autoUpdatedCamera found from {gameObject.name}. " +
				               $"We looked for Camera.main and then for any Camera in the scene.");
				return false;
			}

			return true;
		}

		protected override void SetCurrentValueByDefault()
		{
			float cameraFov = GetCameraFovOrSize();
			SetCurrentValue(cameraFov);
		}

		protected override void SetTargetByDefault()
		{
			float cameraFov = GetCameraFovOrSize();
			SetTarget(cameraFov);
		}

		private float GetCameraFovOrSize()
		{
			float res = autoUpdatedCamera.orthographic ? autoUpdatedCamera.orthographicSize : autoUpdatedCamera.fieldOfView;
			return res;
		}

		private void FindCamera()
		{
			if (autoUpdatedCamera == null)
			{
				autoUpdatedCamera = Camera.main;

				if (autoUpdatedCamera == null)
				{
					autoUpdatedCamera = FindFirstObjectByType<Camera>();
				}
			}
		}

		public Camera GetAutoUpdatedCamera()
		{
			return autoUpdatedCamera;
		}

		public void SetAutoUpdatedCamera(Camera newAutoUpdatedCamera)
		{
			autoUpdatedCamera = newAutoUpdatedCamera;
		}

#if UNITY_EDITOR
		protected override void Reset()
		{
			base.Reset();

			if (!EditorApplication.isPlayingOrWillChangePlaymode && !PrefabUtility.IsPartOfPrefabAsset(this))
			{
				FindCamera();
			}
		}

		internal override Spring[] GetSpringsArray()
		{
			Spring[] res = new Spring[]
			{
				fovSpring
			};

			return res;
		}
#endif
	}
}
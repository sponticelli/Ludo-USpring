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
		public void SetTarget(float target) => fovSpring.SetTarget(target);
		public float GetCurrentValue() => fovSpring.GetCurrentValue();
		public void SetCurrentValue(float currentValues) => fovSpring.SetCurrentValue(currentValues);
		public float GetVelocity() => fovSpring.GetVelocity();
		public void SetVelocity(float velocity) => fovSpring.SetVelocity(velocity);
		public void AddVelocity(float velocityToAdd) =>	fovSpring.AddVelocity(velocityToAdd);
		public float GetForce() => fovSpring.GetForce();
		public void SetForce(float force) => fovSpring.SetForce(force);
		public float GetDrag() => fovSpring.GetDrag();
		public void SetDrag(float drag) => fovSpring.SetDrag(drag);
		public void SetMinValues(float minValue) => fovSpring.SetMinValue(minValue);
		public void SetMaxValues(float maxValue) => fovSpring.SetMaxValue(maxValue);
		public void SetClampCurrentValues(bool clamp) => fovSpring.SetClampCurrentValue(clamp);
		public void SetClampTarget(bool clamp) => fovSpring.SetClampTarget(clamp);
		public void StopSpringOnClamp(bool stop) => fovSpring.SetStopOnClamp(stop);
		public float GetCommonForce() => fovSpring.GetCommonForce();
		public float GetCommonDrag() => fovSpring.GetCommonDrag();
		public void SetCommonForce(float force)
		{
			fovSpring.SetCommonForceAndDrag(true);
			fovSpring.SetCommonForce(force);
		}
		public void SetCommonDrag(float drag)
		{
			fovSpring.SetCommonForceAndDrag(true);
			fovSpring.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
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
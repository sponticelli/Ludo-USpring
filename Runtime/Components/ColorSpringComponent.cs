using UnityEngine;
using UnityEngine.UI;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Color Spring")]
	public partial class ColorSpringComponent : SpringComponent
	{
		[SerializeField] private SpringColor colorSpring = new SpringColor();

		[SerializeField] private bool autoUpdate;
		[SerializeField] private bool autoUpdatedObjectIsRenderer = true;
		[SerializeField] private Renderer autoUpdatedRenderer;
		[SerializeField] private Graphic autoUpdatedUiGraphic;
		
		public SpringEvents Events => colorSpring.springEvents;
		public Color GetTarget() => colorSpring.GetTarget();
		public void SetTarget(Color target) => colorSpring.SetTarget(target);
		public Color GetCurrentValue() => colorSpring.GetCurrentValue();
		public void SetCurrentValue(Color currentValues)
		{
			colorSpring.SetCurrentValue(currentValues);
			SetCurrentValueInternal(currentValues);
		}
		public Vector4 GetVelocity() => colorSpring.GetVelocity();
		public void SetVelocity(Vector4 velocity) => colorSpring.SetVelocity(velocity);
		public void SetVelocity(float velocity) => SetVelocity(Vector4.one * velocity);
		public void AddVelocity(Vector4 velocityToAdd) => colorSpring.AddVelocity(velocityToAdd);
		public override void ReachEquilibrium()
		{
			base.ReachEquilibrium();
			ReachEquilibriumInternal();
		}
		public Vector4 GetForce() => colorSpring.GetForce();
		public void SetForce(Vector4 force) => colorSpring.SetForce(force);
		public void SetForce(float force) => SetForce(Vector4.one * force);
		public Vector4 GetDrag() => colorSpring.GetDrag();
		public void SetDrag(Vector4 drag) => colorSpring.SetDrag(drag);
		public void SetDrag(float drag) => SetDrag(Vector4.one * drag);
		public float GetCommonForce() => colorSpring.GetCommonForce();
		public float GetCommonDrag() => colorSpring.GetCommonDrag();
		public void SetCommonForce(float force)
		{
			colorSpring.SetCommonForceAndDrag(true);
			colorSpring.SetCommonForce(force);
		}
		public void SetCommonDrag(float drag)
		{
			colorSpring.SetCommonForceAndDrag(true);
			colorSpring.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
		}
		public void SetMinValues(Vector4 minValue) => colorSpring.SetMinValues(minValue);
		public void SetMaxValues(Vector4 maxValue) => colorSpring.SetMaxValues(maxValue);
		public void SetClampCurrentValues(bool clampR, bool clampG, bool clampB, bool clampA) => colorSpring.SetClampCurrentValues(clampR, clampG, clampB, clampA);
		public void SetClampTarget(bool clampR, bool clampG, bool clampB, bool clampA) => colorSpring.SetClampTarget(clampR, clampG, clampB, clampA);
		public void StopSpringOnClamp(bool stopR, bool stopG, bool stopB, bool stopA) => colorSpring.StopSpringOnClamp(stopR, stopG, stopB, stopA);
		
		
		private Color GetDefaultColor()
		{
			Color res = Color.white;

			if (autoUpdate)
			{
				if (autoUpdatedObjectIsRenderer)
				{
					res = autoUpdatedRenderer is SpriteRenderer spriteRenderer ? spriteRenderer.color : autoUpdatedRenderer.material.color;
				}
				else
				{
					res = autoUpdatedUiGraphic.color;
				}
			}

			return res;
		}

		protected override void SetCurrentValueByDefault()
		{
			Color defaultColor = GetDefaultColor();
			colorSpring.SetCurrentValue(defaultColor);
		}

		protected override void SetTargetByDefault()
		{
			Color defaultColor = GetDefaultColor();
			colorSpring.SetTarget(defaultColor);
		}

		public void Update()
		{
			if (!initialized) { return; }

			UpdateAutoUpdatedObject();
		}

		private void UpdateAutoUpdatedObject()
		{
			if (autoUpdate)
			{
				if (autoUpdatedObjectIsRenderer)
				{
					autoUpdatedRenderer.material.color = GetCurrentValue();
				}
				else
				{
					autoUpdatedUiGraphic.color = GetCurrentValue();
				}
			}
		}

		protected override void RegisterSprings()
		{
			RegisterSpring(colorSpring);
		}

		public override bool IsValidSpringComponent()
		{
			bool res = true;

			if (autoUpdate)
			{
				if (autoUpdatedObjectIsRenderer)
				{
					if (autoUpdatedRenderer == null)
					{
						AddErrorReason($"{gameObject.name} ColorSpringComponent Target Renderer is null but targetIsRenderer is enabled.");
						res = false;
					}
				}
				else if (autoUpdatedUiGraphic == null)
				{
					AddErrorReason($"{gameObject.name} ColorSpringComponent Target Image is null but autoUpdate is enabled.");
					res = false;
				}
			}

			return res;
		}

		private void ReachEquilibriumInternal()
		{
			UpdateAutoUpdatedObject();
		}

		private void SetCurrentValueInternal(Color currentValues)
		{
			UpdateAutoUpdatedObject();
		}

#if UNITY_EDITOR
		protected override void Reset()
		{
			base.Reset();

			if (autoUpdatedRenderer == null)
			{
				autoUpdatedRenderer = GetComponent<Renderer>();
			}
			if (autoUpdatedUiGraphic == null)
			{
				autoUpdatedUiGraphic = GetComponent<Graphic>();
			}
			
			colorSpring.commonForceAndDrag = true;
			colorSpring.commonForce = 50f;
			colorSpring.commonDrag = 10f;
		}

		internal override Spring[] GetSpringsArray()
		{
			Spring[] res = new Spring[]
			{
				colorSpring
			};

			return res;
		}
#endif

		#region API PUBLIC METHODS

		public void SetAutoUpdateToFalse()
		{
			autoUpdate = false;
		}
		
		public void SetAutoUpdateToTrue(bool isRenderer, GameObject autoUpdatedObject)
		{
			autoUpdate = true;
			autoUpdatedObjectIsRenderer = isRenderer;
			autoUpdatedRenderer = autoUpdatedObject.GetComponent<Renderer>();
			autoUpdatedUiGraphic = autoUpdatedObject.GetComponent<Graphic>();
		}

		#endregion
	}
}
using UnityEngine;
using UnityEngine.UI;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Ui Slider Spring")]
	public partial class UiSliderSpringComponent : SpringComponent
	{
		[SerializeField] private SpringFloat fillAmountSpring = new SpringFloat();

		[SerializeField] private Image autoUpdatedSliderImage;

		public SpringEvents Events => fillAmountSpring.springEvents;
		public float GetTarget() => fillAmountSpring.GetTarget();
		public void SetTarget(float target) => fillAmountSpring.SetTarget(target);
		public float GetCurrentValue() => fillAmountSpring.GetCurrentValue();
		public void SetCurrentValue(float currentValues)
		{
			fillAmountSpring.SetCurrentValue(currentValues);
			SetCurrentValueInternal(currentValues);
		}
		public float GetVelocity() => fillAmountSpring.GetVelocity();
		public void SetVelocity(float velocity) => fillAmountSpring.SetVelocity(velocity);
		public void AddVelocity(float velocityToAdd) =>	fillAmountSpring.AddVelocity(velocityToAdd);
		public override void ReachEquilibrium()
		{
			base.ReachEquilibrium();
			ReachEquilibriumInternal();
		}
		public float GetForce() => fillAmountSpring.GetForce();
		public void SetForce(float force) => fillAmountSpring.SetForce(force);
		public float GetDrag() => fillAmountSpring.GetDrag();
		public void SetDrag(float drag) => fillAmountSpring.SetDrag(drag);
		public void SetMinValues(float minValue) => fillAmountSpring.SetMinValue(minValue);
		public void SetMaxValues(float maxValue) => fillAmountSpring.SetMaxValue(maxValue);
		public void SetClampCurrentValues(bool clamp) => fillAmountSpring.SetClampCurrentValue(clamp);
		public void SetClampTarget(bool clamp) => fillAmountSpring.SetClampTarget(clamp);
		public void StopSpringOnClamp(bool stop) => fillAmountSpring.SetStopOnClamp(stop);
		public float GetCommonForce() => fillAmountSpring.GetCommonForce();
		public float GetCommonDrag() => fillAmountSpring.GetCommonDrag();
		public void SetCommonForce(float force)
		{
			fillAmountSpring.SetCommonForceAndDrag(true);
			fillAmountSpring.SetCommonForce(force);
		}
		public void SetCommonDrag(float drag)
		{
			fillAmountSpring.SetCommonForceAndDrag(true);
			fillAmountSpring.SetCommonDrag(drag);
		}
		public void SetCommonForceAndDrag(float force, float drag)
		{
			SetCommonForce(force);
			SetCommonDrag(drag);
		}
		
		
		protected override void RegisterSprings()
		{
			RegisterSpring(fillAmountSpring);
		}

		protected override void SetCurrentValueByDefault()
		{
			fillAmountSpring.SetCurrentValue(autoUpdatedSliderImage.fillAmount);
		}

		protected override void SetTargetByDefault()
		{
			fillAmountSpring.SetTarget(autoUpdatedSliderImage.fillAmount);
		}

		public void Update()
		{
			if (!initialized) { return; } 

			UpdateFillAmount();
		}

		private void UpdateFillAmount()
		{
			autoUpdatedSliderImage.fillAmount = fillAmountSpring.GetCurrentValue();
		}

		public override bool IsValidSpringComponent()
		{
			bool res = true;

			if(autoUpdatedSliderImage == null)
			{
				AddErrorReason($"{gameObject.name} autoUpdatedSliderImage is null.");
				res = false;
			}

			return res;
		}

		private void ReachEquilibriumInternal()
		{
			UpdateFillAmount();
		}

		private void SetCurrentValueInternal(float currentValues)
		{
			UpdateFillAmount();
		}

#if UNITY_EDITOR
		protected override void Reset()
		{
			base.Reset();

			if(autoUpdatedSliderImage == null)
			{
				autoUpdatedSliderImage = GetComponent<Image>();
			}
		}

		internal override Spring[] GetSpringsArray()
		{
			Spring[] res = new Spring[]
			{
				fillAmountSpring
			};

			return res;
		}
#endif
	}
}
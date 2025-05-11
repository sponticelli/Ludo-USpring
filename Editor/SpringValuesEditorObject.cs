using UnityEditor;

namespace USpring
{
	public class SpringValuesEditorObject
	{
		public SerializedProperty SpInitialValue;

		public SerializedProperty SpClampTarget;
		public SerializedProperty SpClampCurrentValue;
		public SerializedProperty SpStopSpringOnCurrentValueClamp;

		public SerializedProperty SpMinValue;
		public SerializedProperty SpMaxValue;

		public SerializedProperty SpUpdate;
		public SerializedProperty SpForce;
		public SerializedProperty SpDrag;

		public SerializedProperty SpTarget;

		public SerializedProperty SpCurrentValue;
		public SerializedProperty SpVelocity;

		public SerializedProperty SpCandidateValue;

		public SerializedProperty SpOperationValue;

		public SpringValuesEditorObject(
			SerializedProperty spInitialValue,
			SerializedProperty spClampTarget, SerializedProperty spClampCurrentValue, SerializedProperty spStopSpringOnCurrentValueClamp,
			SerializedProperty spMinValue, SerializedProperty spMaxValue,
			SerializedProperty spUpdate, SerializedProperty spForce, SerializedProperty spDrag,
			SerializedProperty spTarget,
			SerializedProperty spCurrentValue, SerializedProperty spVelocity,
			SerializedProperty spCandidateValue,
			SerializedProperty spOperationValue)
		{
			this.SpInitialValue = spInitialValue;

			this.SpClampTarget = spClampTarget;
			this.SpClampCurrentValue = spClampCurrentValue;
			this.SpStopSpringOnCurrentValueClamp = spStopSpringOnCurrentValueClamp;

			this.SpMinValue = spMinValue;
			this.SpMaxValue = spMaxValue;

			this.SpUpdate = spUpdate;
			this.SpForce = spForce;
			this.SpDrag = spDrag;

			this.SpTarget = spTarget;

			this.SpCurrentValue = spCurrentValue;
			this.SpVelocity = spVelocity;

			this.SpCandidateValue = spCandidateValue;

			this.SpOperationValue = spOperationValue;
		}

		public void SetInitialValue(float initialValue)
		{
			this.SpInitialValue.floatValue = initialValue;
		}

		public float GetInitialValue()
		{
			float res = SpInitialValue.floatValue;
			return res;
		}

		public bool GetUpdate()
		{
			bool res = SpUpdate.boolValue;
			return res;
		}

		public void SetUpdate(bool update)
		{
			SpUpdate.boolValue = update;
		}

		public float GetForce()
		{
			float res = SpForce.floatValue;
			return res;
		}

		public void SetForce(float force)
		{
			SpForce.floatValue = force;
		}

		public float GetDrag()
		{
			float res = SpDrag.floatValue;
			return res;
		}

		public void SetDrag(float drag)
		{
			SpDrag.floatValue = drag;
		}

		public bool GetClampTarget()
		{
			bool res = SpClampTarget.boolValue;
			return res;
		}

		public void SetClampTarget(bool value)
		{
			SpClampTarget.boolValue = value;
		}

		public bool GetClampCurrentValue()
		{
			bool res = SpClampCurrentValue.boolValue;
			return res;
		}

		public void SetClampCurrentValue(bool value)
		{
			SpClampCurrentValue.boolValue = value;
		}

		public bool GetStopSpringOnCurrentValueClamp()
		{
			bool res = SpStopSpringOnCurrentValueClamp.boolValue;
			return res;
		}

		public void SetStopSpringOnCurrentValueClamp(bool value)
		{
			SpStopSpringOnCurrentValueClamp.boolValue = value;
		}

		public float GetMinValue()
		{
			float res = SpMinValue.floatValue;
			return res;
		}

		public void SetMinValue(float minValue)
		{
			SpMinValue.floatValue = minValue;
		}

		public float GetMaxValue()
		{
			float res = SpMaxValue.floatValue;
			return res;
		}

		public void SetMaxValue(float maxValue)
		{
			SpMaxValue.floatValue = maxValue;
		}

		public float GetTarget()
		{
			float res = SpTarget.floatValue;
			return res;
		}

		public void SetTarget(float target)
		{
			SpTarget.floatValue = target;
		}

		public float GetCandidateValue()
		{
			float res = SpCandidateValue.floatValue;
			return res;
		}

		public void SetCandidateValue(float value)
		{
			SpCandidateValue.floatValue = value;
		}

		public float GetCurrentValue()
		{
			float res = SpCurrentValue.floatValue;
			return res;
		}

		public void SetCurrentValue(float currentValue)
		{
			if (SpUpdate.boolValue)
			{
				SpCurrentValue.floatValue = currentValue;
			}
		}

		public float GetVelocity()
		{
			float res = SpVelocity.floatValue;
			return res;
		}

		public void SetVelocity(float velocity)
		{
			SpVelocity.floatValue = velocity;
		}

		public float GetOperationValue()
		{
			float res = SpOperationValue.floatValue;
			return res;
		}

		public void SetOperationValue(float value)
		{
			SpOperationValue.floatValue = value;
		}
	}
}
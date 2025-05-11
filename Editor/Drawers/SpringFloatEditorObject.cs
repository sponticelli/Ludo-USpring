using UnityEditor;
using USpring.Core;

namespace USpring.Drawers
{
	public class SpringFloatEditorObject : SpringEditorObject
	{
		public SpringFloatEditorObject(SerializedProperty spParent) : base(spParent, SpringFloat.SPRING_SIZE) 
		{}
		
		public float InitialValue
		{
			get
			{
				float res = SpringValuesEditorObjects[X].GetInitialValue();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetInitialValue(value);
			}
		}

		public bool Update
		{
			get
			{
				bool res = SpringValuesEditorObjects[X].GetUpdate();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetUpdate(value);
			}
		}

		public bool ClampTarget
		{
			get
			{
				bool res = SpringValuesEditorObjects[X].GetClampTarget();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetClampTarget(value);
			}
		}

		public bool ClampCurrentValue
		{
			get
			{
				bool res = SpringValuesEditorObjects[X].GetClampCurrentValue();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetClampCurrentValue(value);
			}
		}

		public bool StopSpringOnCurrentValueClamp
		{
			get
			{
				bool res = SpringValuesEditorObjects[X].GetStopSpringOnCurrentValueClamp();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetStopSpringOnCurrentValueClamp(value);
			}
		}

		public float Force
		{
			get
			{
				float res = SpringValuesEditorObjects[X].GetForce();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetForce(value);
			}
		}

		public float Drag
		{
			get
			{
				float res = SpringValuesEditorObjects[X].GetDrag();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetDrag(value);
			}
		}

		public float MinValue
		{
			get
			{
				float res = SpringValuesEditorObjects[X].GetMinValue();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetMinValue(value);
			}
		}

		public float MaxValue
		{
			get
			{
				float res = SpringValuesEditorObjects[X].GetMaxValue();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetMaxValue(value);
			}
		}

		public float Target
		{
			get
			{
				float res = SpringValuesEditorObjects[X].GetTarget();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetTarget(value);
			}
		}

		public float CurrentValue
		{
			get
			{
				float res = SpringValuesEditorObjects[X].GetCurrentValue();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetCurrentValue(value);
			}
		}

		public float Velocity
		{
			get
			{
				float res = SpringValuesEditorObjects[X].GetVelocity();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetVelocity(value);
			}
		}

		public float OperationValue
		{
			get
			{
				float res = SpringValuesEditorObjects[X].GetOperationValue();
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetOperationValue(value);
			}
		}

		public override void AddVelocityNudge()
		{
			Velocity += OperationValue;
		}

		public override void SetCurrentValuesNudge()
		{
			CurrentValue = OperationValue;
		}

		public override void SetTargetNudge()
		{
			Target = OperationValue;
		}
	}
}
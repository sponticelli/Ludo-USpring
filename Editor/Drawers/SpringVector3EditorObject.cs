using UnityEditor;
using UnityEngine;
using USpring.Core;
using USpring.Vectors;

namespace USpring.Drawers
{
	public class SpringVector3EditorObject : SpringEditorObject
	{
		public Vector3 InitialValues
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[X].GetInitialValue(),
					SpringValuesEditorObjects[Y].GetInitialValue(),
					SpringValuesEditorObjects[Z].GetInitialValue());

				return res;
			}

			set
			{
				SpringValuesEditorObjects[X].SetInitialValue(value.x);
				SpringValuesEditorObjects[Y].SetInitialValue(value.y);
				SpringValuesEditorObjects[Z].SetInitialValue(value.z);
			}
		}

		public Vector3Bool Update
		{
			set
			{
				SetUpdate(value);
			}
			get
			{
				Vector3Bool res = GetUpdate();
				return res;
			}
		}

		public Vector3Bool ClampTarget
		{
			get
			{
				Vector3Bool res = new Vector3Bool(
					SpringValuesEditorObjects[X].GetClampTarget(),
					SpringValuesEditorObjects[Y].GetClampTarget(),
					SpringValuesEditorObjects[Z].GetClampTarget());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetClampTarget(value.X);
				SpringValuesEditorObjects[Y].SetClampTarget(value.Y);
				SpringValuesEditorObjects[Z].SetClampTarget(value.Z);
			}
		}

		public Vector3Bool ClampCurrentValue
		{
			get
			{
				Vector3Bool res = new Vector3Bool(
					SpringValuesEditorObjects[X].GetClampCurrentValue(),
					SpringValuesEditorObjects[Y].GetClampCurrentValue(),
					SpringValuesEditorObjects[Z].GetClampCurrentValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetClampCurrentValue(value.X);
				SpringValuesEditorObjects[Y].SetClampCurrentValue(value.Y);
				SpringValuesEditorObjects[Z].SetClampCurrentValue(value.Z);
			}
		}

		public Vector3Bool StopSpringOnCurrentValueClamp
		{
			get
			{
				Vector3Bool res = new Vector3Bool(
					SpringValuesEditorObjects[X].GetStopSpringOnCurrentValueClamp(),
					SpringValuesEditorObjects[Y].GetStopSpringOnCurrentValueClamp(),
					SpringValuesEditorObjects[Z].GetStopSpringOnCurrentValueClamp());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetStopSpringOnCurrentValueClamp(value.X);
				SpringValuesEditorObjects[Y].SetStopSpringOnCurrentValueClamp(value.Y);
				SpringValuesEditorObjects[Z].SetStopSpringOnCurrentValueClamp(value.Z);
			}
		}

		public Vector3 Force
		{
			set
			{
				SetForce(value);
			}
			get
			{
				Vector3 res = GetForce();
				return res;
			}
		}

		public Vector3 Drag
		{
			set
			{
				SetDrag(value);
			}
			get
			{
				Vector3 res = GetDrag();
				return res;
			}
		}

		public Vector3 MinValue
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[X].GetMinValue(),
					SpringValuesEditorObjects[Y].GetMinValue(),
					SpringValuesEditorObjects[Z].GetMinValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetMinValue(value.x);
				SpringValuesEditorObjects[Y].SetMinValue(value.y);
				SpringValuesEditorObjects[Z].SetMinValue(value.z);
			}
		}

		public Vector3 MaxValue
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[X].GetMaxValue(),
					SpringValuesEditorObjects[Y].GetMaxValue(),
					SpringValuesEditorObjects[Z].GetMaxValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetMaxValue(value.x);
				SpringValuesEditorObjects[Y].SetMaxValue(value.y);
				SpringValuesEditorObjects[Z].SetMaxValue(value.z);
			}
		}

		public Vector3 Target
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[X].GetTarget(),
					SpringValuesEditorObjects[Y].GetTarget(),
					SpringValuesEditorObjects[Z].GetTarget());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetTarget(value.x);
				SpringValuesEditorObjects[Y].SetTarget(value.y);
				SpringValuesEditorObjects[Z].SetTarget(value.z);
			}
		}

		public Vector3 CandidateValue
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[X].GetCandidateValue(),
					SpringValuesEditorObjects[Y].GetCandidateValue(),
					SpringValuesEditorObjects[Z].GetCandidateValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetCandidateValue(value.x);
				SpringValuesEditorObjects[Y].SetCandidateValue(value.y);
				SpringValuesEditorObjects[Z].SetCandidateValue(value.z);
			}
		}

		public Vector3 CurrentValue
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[X].GetCurrentValue(),
					SpringValuesEditorObjects[Y].GetCurrentValue(),
					SpringValuesEditorObjects[Z].GetCurrentValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetCurrentValue(value.x);
				SpringValuesEditorObjects[Y].SetCurrentValue(value.y);
				SpringValuesEditorObjects[Z].SetCurrentValue(value.z);
			}
		}

		public Vector3 Velocity
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[X].GetVelocity(),
					SpringValuesEditorObjects[Y].GetVelocity(),
					SpringValuesEditorObjects[Z].GetVelocity());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetVelocity(value.x);
				SpringValuesEditorObjects[Y].SetVelocity(value.y);
				SpringValuesEditorObjects[Z].SetVelocity(value.z);
			}
		}

		public Vector3 OperationValue
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[X].GetOperationValue(),
					SpringValuesEditorObjects[Y].GetOperationValue(),
					SpringValuesEditorObjects[Z].GetOperationValue());
				
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetOperationValue(value.x);
				SpringValuesEditorObjects[Y].SetOperationValue(value.y);
				SpringValuesEditorObjects[Z].SetOperationValue(value.z);
			}
		}

		public SpringVector3EditorObject(SerializedProperty spParentProperty) : base(spParentProperty, SpringVector3.SpringSize)
		{}

		public void SetUpdate(Vector3Bool update)
		{
			SpringValuesEditorObjects[X].SetUpdate(update.X);
			SpringValuesEditorObjects[Y].SetUpdate(update.Y);
			SpringValuesEditorObjects[Z].SetUpdate(update.Z);
		}

		public Vector3Bool GetUpdate()
		{
			Vector3Bool res = new Vector3Bool(
				SpringValuesEditorObjects[X].SpUpdate.boolValue, 
				SpringValuesEditorObjects[Y].SpUpdate.boolValue, 
				SpringValuesEditorObjects[Z].SpUpdate.boolValue);

			return res;
		}

		public void SetForce(Vector3 force)
		{
			SpringValuesEditorObjects[X].SetForce(force.x);
			SpringValuesEditorObjects[Y].SetForce(force.y);
			SpringValuesEditorObjects[Z].SetForce(force.z);
		}

		public Vector3 GetForce()
		{
			Vector3 res = new Vector3(SpringValuesEditorObjects[X].SpForce.floatValue, SpringValuesEditorObjects[Y].SpForce.floatValue, SpringValuesEditorObjects[Z].SpForce.floatValue);
			return res;
		}

		public void SetDrag(Vector3 drag)
		{
			SpringValuesEditorObjects[X].SetDrag(drag.x);
			SpringValuesEditorObjects[Y].SetDrag(drag.y);
			SpringValuesEditorObjects[Z].SetDrag(drag.z);
		}

		public Vector3 GetDrag()
		{
			Vector3 res = new Vector3(SpringValuesEditorObjects[X].SpDrag.floatValue, SpringValuesEditorObjects[Y].SpDrag.floatValue, SpringValuesEditorObjects[Z].SpDrag.floatValue);
			return res;
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
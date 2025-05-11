using UnityEditor;
using UnityEngine;
using USpring.Core;
using USpring.Vectors;

namespace USpring.Drawers
{
	public class SpringVector4EditorObject : SpringEditorObject
	{
		public Vector4 InitialValues
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[X].GetInitialValue(),
					SpringValuesEditorObjects[Y].GetInitialValue(),
					SpringValuesEditorObjects[Z].GetInitialValue(),
					SpringValuesEditorObjects[W].GetInitialValue());

				return res;
			}

			set
			{
				SpringValuesEditorObjects[X].SetInitialValue(value.x);
				SpringValuesEditorObjects[Y].SetInitialValue(value.y);
				SpringValuesEditorObjects[Z].SetInitialValue(value.z);
				SpringValuesEditorObjects[W].SetInitialValue(value.w);
			}
		}

		public Vector4Bool Update
		{
			get
			{
				Vector4Bool res = new Vector4Bool(
					SpringValuesEditorObjects[X].GetUpdate(),
					SpringValuesEditorObjects[Y].GetUpdate(),
					SpringValuesEditorObjects[Z].GetUpdate(),
					SpringValuesEditorObjects[W].GetUpdate()
				);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetUpdate(value.X);
				SpringValuesEditorObjects[Y].SetUpdate(value.Y);
				SpringValuesEditorObjects[Z].SetUpdate(value.Z);
				SpringValuesEditorObjects[W].SetUpdate(value.W);
			}
		}

		public Vector4Bool ClampTarget
		{
			get
			{
				Vector4Bool res = new Vector4Bool(
					SpringValuesEditorObjects[X].GetClampTarget(),
					SpringValuesEditorObjects[Y].GetClampTarget(),
					SpringValuesEditorObjects[Z].GetClampTarget(),
					SpringValuesEditorObjects[W].GetClampTarget());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetClampTarget(value.X);
				SpringValuesEditorObjects[Y].SetClampTarget(value.Y);
				SpringValuesEditorObjects[Z].SetClampTarget(value.Z);
				SpringValuesEditorObjects[W].SetClampTarget(value.W);
			}
		}

		public Vector4Bool ClampCurrentValue
		{
			get
			{
				Vector4Bool res = new Vector4Bool(
					SpringValuesEditorObjects[X].GetClampCurrentValue(),
					SpringValuesEditorObjects[Y].GetClampCurrentValue(),
					SpringValuesEditorObjects[Z].GetClampCurrentValue(),
					SpringValuesEditorObjects[W].GetClampCurrentValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetClampCurrentValue(value.X);
				SpringValuesEditorObjects[Y].SetClampCurrentValue(value.Y);
				SpringValuesEditorObjects[Z].SetClampCurrentValue(value.Z);
				SpringValuesEditorObjects[W].SetClampCurrentValue(value.W);
			}
		}

		public Vector4Bool StopSpringOnCurrentValueClamp
		{
			get
			{
				Vector4Bool res = new Vector4Bool(
					SpringValuesEditorObjects[X].GetStopSpringOnCurrentValueClamp(),
					SpringValuesEditorObjects[Y].GetStopSpringOnCurrentValueClamp(),
					SpringValuesEditorObjects[Z].GetStopSpringOnCurrentValueClamp(),
					SpringValuesEditorObjects[W].GetStopSpringOnCurrentValueClamp());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetStopSpringOnCurrentValueClamp(value.X);
				SpringValuesEditorObjects[Y].SetStopSpringOnCurrentValueClamp(value.Y);
				SpringValuesEditorObjects[Z].SetStopSpringOnCurrentValueClamp(value.Z);
				SpringValuesEditorObjects[W].SetStopSpringOnCurrentValueClamp(value.W);
			}
		}

		public Vector4 Force
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[X].GetForce(),
					SpringValuesEditorObjects[Y].GetForce(),
					SpringValuesEditorObjects[Z].GetForce(),
					SpringValuesEditorObjects[W].GetForce()
				);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetForce(value.x);
				SpringValuesEditorObjects[Y].SetForce(value.y);
				SpringValuesEditorObjects[Z].SetForce(value.z);
				SpringValuesEditorObjects[W].SetForce(value.w);
			}
		}

		public Vector4 Drag
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[X].GetDrag(),
					SpringValuesEditorObjects[Y].GetDrag(),
					SpringValuesEditorObjects[Z].GetDrag(),
					SpringValuesEditorObjects[W].GetDrag()
				);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetDrag(value.x);
				SpringValuesEditorObjects[Y].SetDrag(value.y);
				SpringValuesEditorObjects[Z].SetDrag(value.z);
				SpringValuesEditorObjects[W].SetDrag(value.w);
			}
		}

		public Vector4 MinValue
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[X].GetMinValue(),
					SpringValuesEditorObjects[Y].GetMinValue(),
					SpringValuesEditorObjects[Z].GetMinValue(),
					SpringValuesEditorObjects[W].GetMinValue()
				);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetMinValue(value.x);
				SpringValuesEditorObjects[Y].SetMinValue(value.y);
				SpringValuesEditorObjects[Z].SetMinValue(value.z);
				SpringValuesEditorObjects[W].SetMinValue(value.w);
			}
		}

		public Vector4 MaxValue
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[X].GetMaxValue(),
					SpringValuesEditorObjects[Y].GetMaxValue(),
					SpringValuesEditorObjects[Z].GetMaxValue(),
					SpringValuesEditorObjects[W].GetMaxValue()
				);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetMaxValue(value.x);
				SpringValuesEditorObjects[Y].SetMaxValue(value.y);
				SpringValuesEditorObjects[Z].SetMaxValue(value.z);
				SpringValuesEditorObjects[W].SetMaxValue(value.w);
			}
		}

		public Vector4 Target
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[X].GetTarget(),
					SpringValuesEditorObjects[Y].GetTarget(),
					SpringValuesEditorObjects[Z].GetTarget(),
					SpringValuesEditorObjects[W].GetTarget()
				);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetTarget(value.x);
				SpringValuesEditorObjects[Y].SetTarget(value.y);
				SpringValuesEditorObjects[Z].SetTarget(value.z);
				SpringValuesEditorObjects[W].SetTarget(value.w);
			}
		}

		public Vector4 CandidateValue
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[X].GetCandidateValue(),
					SpringValuesEditorObjects[Y].GetCandidateValue(),
					SpringValuesEditorObjects[Z].GetCandidateValue(),
					SpringValuesEditorObjects[W].GetCandidateValue()
				);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetCandidateValue(value.x);
				SpringValuesEditorObjects[Y].SetCandidateValue(value.y);
				SpringValuesEditorObjects[Z].SetCandidateValue(value.z);
				SpringValuesEditorObjects[W].SetCandidateValue(value.w);
			}
		}

		public Vector4 CurrentValue
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[X].GetCurrentValue(),
					SpringValuesEditorObjects[Y].GetCurrentValue(),
					SpringValuesEditorObjects[Z].GetCurrentValue(),
					SpringValuesEditorObjects[W].GetCurrentValue()
				);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetCurrentValue(value.x);
				SpringValuesEditorObjects[Y].SetCurrentValue(value.y);
				SpringValuesEditorObjects[Z].SetCurrentValue(value.z);
				SpringValuesEditorObjects[W].SetCurrentValue(value.w);
			}
		}

		public Vector4 Velocity
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[X].GetVelocity(),
					SpringValuesEditorObjects[Y].GetVelocity(),
					SpringValuesEditorObjects[Z].GetVelocity(),
					SpringValuesEditorObjects[W].GetVelocity()
				);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetVelocity(value.x);
				SpringValuesEditorObjects[Y].SetVelocity(value.y);
				SpringValuesEditorObjects[Z].SetVelocity(value.z);
				SpringValuesEditorObjects[W].SetVelocity(value.w);
			}
		}

		public Vector4 OperationValue
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[X].GetOperationValue(),
					SpringValuesEditorObjects[Y].GetOperationValue(),
					SpringValuesEditorObjects[Z].GetOperationValue(),
					SpringValuesEditorObjects[W].GetOperationValue()
				);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetOperationValue(value.x);
				SpringValuesEditorObjects[Y].SetOperationValue(value.y);
				SpringValuesEditorObjects[Z].SetOperationValue(value.z);
				SpringValuesEditorObjects[W].SetOperationValue(value.w);
			}
		}

		public SpringVector4EditorObject(SerializedProperty spParentroperty) : base(spParentroperty, SpringVector4.SpringSize)
		{
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
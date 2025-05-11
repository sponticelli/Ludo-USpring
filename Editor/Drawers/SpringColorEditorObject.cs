using UnityEditor;
using UnityEngine;
using USpring.Core;
using USpring.Vectors;

namespace USpring.Drawers
{
	public class SpringColorEditorObject : SpringEditorObject
	{
		public SpringColorEditorObject(SerializedProperty spParentProperty) : base(spParentProperty, SpringColor.SpringSize)
		{}

		public Color InitialValue
		{
			get
			{
				Color res = new Color(
					SpringValuesEditorObjects[R].GetInitialValue(),
					SpringValuesEditorObjects[G].GetInitialValue(),
					SpringValuesEditorObjects[B].GetInitialValue(),
					SpringValuesEditorObjects[A].GetInitialValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[R].SetInitialValue(value.r);
				SpringValuesEditorObjects[G].SetInitialValue(value.g);
				SpringValuesEditorObjects[B].SetInitialValue(value.b);
				SpringValuesEditorObjects[A].SetInitialValue(value.a);
			}
		}

		public Vector4 MinValue
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[R].GetMinValue(),
					SpringValuesEditorObjects[G].GetMinValue(),
					SpringValuesEditorObjects[B].GetMinValue(),
					SpringValuesEditorObjects[A].GetMinValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[R].SetMinValue(value.x);
				SpringValuesEditorObjects[G].SetMinValue(value.y);
				SpringValuesEditorObjects[B].SetMinValue(value.z);
				SpringValuesEditorObjects[A].SetMinValue(value.w);
			}
		}

		public Vector4 MaxValue
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[R].GetMaxValue(),
					SpringValuesEditorObjects[G].GetMaxValue(),
					SpringValuesEditorObjects[B].GetMaxValue(),
					SpringValuesEditorObjects[A].GetMaxValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[R].SetMaxValue(value.x);
				SpringValuesEditorObjects[G].SetMaxValue(value.y);
				SpringValuesEditorObjects[B].SetMaxValue(value.z);
				SpringValuesEditorObjects[A].SetMaxValue(value.w);
			}
		}

		public Color Target
		{
			get
			{
				Color res = new Color(
					SpringValuesEditorObjects[R].SpTarget.floatValue,
					SpringValuesEditorObjects[G].SpTarget.floatValue,
					SpringValuesEditorObjects[B].SpTarget.floatValue,
					SpringValuesEditorObjects[A].SpTarget.floatValue);

				return res;
			}
			set
			{
				SpringValuesEditorObjects[R].SpTarget.floatValue = value.r;
				SpringValuesEditorObjects[G].SpTarget.floatValue = value.g;
				SpringValuesEditorObjects[B].SpTarget.floatValue = value.b;
				SpringValuesEditorObjects[A].SpTarget.floatValue = value.a;
			}
		}

		public Vector4Bool Update
		{
			get
			{
				Vector4Bool res = new Vector4Bool(
					SpringValuesEditorObjects[R].GetUpdate(),
					SpringValuesEditorObjects[G].GetUpdate(),
					SpringValuesEditorObjects[B].GetUpdate(),
					SpringValuesEditorObjects[A].GetUpdate());

				return res;
			}

			set
			{
				SpringValuesEditorObjects[R].SetUpdate(value.X);
				SpringValuesEditorObjects[G].SetUpdate(value.Y);
				SpringValuesEditorObjects[B].SetUpdate(value.Z);
				SpringValuesEditorObjects[A].SetUpdate(value.W);
			}
		}

		public Vector4 Force
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[R].GetForce(),
					SpringValuesEditorObjects[G].GetForce(),
					SpringValuesEditorObjects[B].GetForce(),
					SpringValuesEditorObjects[A].GetForce());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[R].SetForce(value.x);
				SpringValuesEditorObjects[G].SetForce(value.y);
				SpringValuesEditorObjects[B].SetForce(value.z);
				SpringValuesEditorObjects[A].SetForce(value.w);
			}
		}

		public Vector4 Drag
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[R].GetDrag(),
					SpringValuesEditorObjects[G].GetDrag(),
					SpringValuesEditorObjects[B].GetDrag(),
					SpringValuesEditorObjects[A].GetDrag());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[R].SetDrag(value.x);
				SpringValuesEditorObjects[G].SetDrag(value.y);
				SpringValuesEditorObjects[B].SetDrag(value.z);
				SpringValuesEditorObjects[A].SetDrag(value.w);
			}
		}

		public Color CurrentValue
		{
			get
			{
				Color res = new Color(
					SpringValuesEditorObjects[R].GetCurrentValue(),
					SpringValuesEditorObjects[G].GetCurrentValue(),
					SpringValuesEditorObjects[B].GetCurrentValue(),
					SpringValuesEditorObjects[A].GetCurrentValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[R].SetCurrentValue(value.r);
				SpringValuesEditorObjects[G].SetCurrentValue(value.g);
				SpringValuesEditorObjects[B].SetCurrentValue(value.b);
				SpringValuesEditorObjects[A].SetCurrentValue(value.a);
			}
		}

		public Vector4 Velocity
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[R].GetVelocity(),
					SpringValuesEditorObjects[G].GetVelocity(),
					SpringValuesEditorObjects[B].GetVelocity(),
					SpringValuesEditorObjects[A].GetVelocity());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[R].SetVelocity(value.x);
				SpringValuesEditorObjects[G].SetVelocity(value.y);
				SpringValuesEditorObjects[B].SetVelocity(value.z);
				SpringValuesEditorObjects[A].SetVelocity(value.w);
			}
		}

		public Vector4 OperationValue
		{
			get
			{
				Vector4 res = new Vector4(
					SpringValuesEditorObjects[R].GetOperationValue(),
					SpringValuesEditorObjects[G].GetOperationValue(),
					SpringValuesEditorObjects[B].GetOperationValue(),
					SpringValuesEditorObjects[A].GetOperationValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[R].SetOperationValue(value.x);
				SpringValuesEditorObjects[G].SetOperationValue(value.y);
				SpringValuesEditorObjects[B].SetOperationValue(value.z);
				SpringValuesEditorObjects[A].SetOperationValue(value.w);
			}
		}

		public Vector4Bool ClampTarget
		{
			get
			{
				Vector4Bool res = new Vector4Bool(
					SpringValuesEditorObjects[R].GetClampTarget(),
					SpringValuesEditorObjects[G].GetClampTarget(),
					SpringValuesEditorObjects[B].GetClampTarget(),
					SpringValuesEditorObjects[A].GetClampTarget());

				return res;
			}

			set
			{
				SpringValuesEditorObjects[R].SetClampTarget(value.X);
				SpringValuesEditorObjects[G].SetClampTarget(value.Y);
				SpringValuesEditorObjects[B].SetClampTarget(value.Z);
				SpringValuesEditorObjects[A].SetClampTarget(value.W);
			}
		}

		public Vector4Bool ClampCurrentValue
		{
			get
			{
				Vector4Bool res = new Vector4Bool(
					SpringValuesEditorObjects[R].GetClampCurrentValue(),
					SpringValuesEditorObjects[G].GetClampCurrentValue(),
					SpringValuesEditorObjects[B].GetClampCurrentValue(),
					SpringValuesEditorObjects[A].GetClampCurrentValue());

				return res;
			}

			set
			{
				SpringValuesEditorObjects[R].SetClampCurrentValue(value.X);
				SpringValuesEditorObjects[G].SetClampCurrentValue(value.Y);
				SpringValuesEditorObjects[B].SetClampCurrentValue(value.Z);
				SpringValuesEditorObjects[A].SetClampCurrentValue(value.W);
			}
		}

		public Vector4Bool StopSpringOnCurrentValueClamp
		{
			get
			{
				Vector4Bool res = new Vector4Bool(
					SpringValuesEditorObjects[R].GetStopSpringOnCurrentValueClamp(),
					SpringValuesEditorObjects[G].GetStopSpringOnCurrentValueClamp(),
					SpringValuesEditorObjects[B].GetStopSpringOnCurrentValueClamp(),
					SpringValuesEditorObjects[A].GetStopSpringOnCurrentValueClamp());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[R].SetStopSpringOnCurrentValueClamp(value.X);
				SpringValuesEditorObjects[G].SetStopSpringOnCurrentValueClamp(value.Y);
				SpringValuesEditorObjects[B].SetStopSpringOnCurrentValueClamp(value.Z);
				SpringValuesEditorObjects[A].SetStopSpringOnCurrentValueClamp(value.W);
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
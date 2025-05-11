using UnityEditor;
using UnityEngine;
using USpring.Core;
using USpring.Vectors;

namespace USpring.Drawers
{
	public class SpringVector2EditorObject : SpringEditorObject
	{
		public SpringVector2EditorObject(SerializedProperty spParentProperty) : base(spParentProperty, SpringVector2.SpringSize)
		{}

		public Vector2 InitialValues
		{
			get
			{
				Vector2 res = new Vector3(
					SpringValuesEditorObjects[X].GetInitialValue(),
					SpringValuesEditorObjects[Y].GetInitialValue());

				return res;
			}

			set
			{
				SpringValuesEditorObjects[X].SetInitialValue(value.x);
				SpringValuesEditorObjects[Y].SetInitialValue(value.y);
			}
		}

		public Vector2Bool Update
		{
			get
			{
				Vector2Bool res = new Vector2Bool(
					SpringValuesEditorObjects[X].GetUpdate(),
					SpringValuesEditorObjects[Y].GetUpdate());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetUpdate(value.X);
				SpringValuesEditorObjects[Y].SetUpdate(value.Y);
			}
		}

		public Vector2Bool ClampTarget
		{
			get
			{
				Vector2Bool res = new Vector2Bool(
					SpringValuesEditorObjects[X].GetClampTarget(),
					SpringValuesEditorObjects[Y].GetClampTarget());
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetClampTarget(value.X);
				SpringValuesEditorObjects[Y].SetClampTarget(value.Y);
			}
		}

		public Vector2Bool ClampCurrentValue
		{
			get
			{
				Vector2Bool res = new Vector2Bool(
					SpringValuesEditorObjects[X].GetClampCurrentValue(),
					SpringValuesEditorObjects[Y].GetClampCurrentValue());
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetClampCurrentValue(value.X);
				SpringValuesEditorObjects[Y].SetClampCurrentValue(value.Y);
			}
		}

		public Vector2Bool StopSpringOnCurrentValueClamp
		{
			get
			{
				Vector2Bool res = new Vector2Bool(
					SpringValuesEditorObjects[X].GetStopSpringOnCurrentValueClamp(),
					SpringValuesEditorObjects[Y].GetStopSpringOnCurrentValueClamp());
				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetStopSpringOnCurrentValueClamp(value.X);
				SpringValuesEditorObjects[Y].SetStopSpringOnCurrentValueClamp(value.Y);
			}
		}

		public Vector2 Force
		{
			get
			{
				Vector2 res = new Vector2(
					SpringValuesEditorObjects[X].GetForce(),
					SpringValuesEditorObjects[Y].GetForce());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetForce(value.x);
				SpringValuesEditorObjects[Y].SetForce(value.y);
			}
		}

		public Vector2 Drag
		{
			get
			{
				Vector2 res = new Vector2(
					SpringValuesEditorObjects[X].GetDrag(),
					SpringValuesEditorObjects[Y].GetDrag());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetDrag(value.x);
				SpringValuesEditorObjects[Y].SetDrag(value.y);
			}
		}

		public Vector2 MinValue
		{
			get
			{
				Vector2 res = new Vector2(
					SpringValuesEditorObjects[X].GetMinValue(),
					SpringValuesEditorObjects[Y].GetMinValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetMinValue(value.x);
				SpringValuesEditorObjects[Y].SetMinValue(value.y);
			}
		}

		public Vector2 MaxValue
		{
			get
			{
				Vector2 res = new Vector2(
					SpringValuesEditorObjects[X].GetMaxValue(),
					SpringValuesEditorObjects[Y].GetMaxValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetMaxValue(value.x);
				SpringValuesEditorObjects[Y].SetMaxValue(value.y);
			}
		}

		public Vector2 Target
		{
			get
			{
				Vector2 res = new Vector2(
					SpringValuesEditorObjects[X].GetTarget(),
					SpringValuesEditorObjects[Y].GetTarget());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetTarget(value.x);
				SpringValuesEditorObjects[Y].SetTarget(value.y);
			}
		}

		public Vector2 CurrentValue
		{
			get
			{
				Vector2 res = new Vector2(
					SpringValuesEditorObjects[X].GetCurrentValue(),
					SpringValuesEditorObjects[Y].GetCurrentValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetCurrentValue(value.x);
				SpringValuesEditorObjects[Y].SetCurrentValue(value.y);
			}
		}

		public Vector2 Velocity
		{
			get
			{
				Vector2 res = new Vector2(
					SpringValuesEditorObjects[X].GetVelocity(),
					SpringValuesEditorObjects[Y].GetVelocity());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetVelocity(value.x);
				SpringValuesEditorObjects[Y].SetVelocity(value.y);
			}
		}

		public Vector2 OperationValue
		{
			get
			{
				Vector2 res = new Vector2(
					SpringValuesEditorObjects[X].GetOperationValue(),
					SpringValuesEditorObjects[Y].GetOperationValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[X].SetOperationValue(value.x);
				SpringValuesEditorObjects[Y].SetOperationValue(value.y);
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
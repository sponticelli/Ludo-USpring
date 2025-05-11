using UnityEditor;
using UnityEngine;
using USpring.Core;
using USpring.Vectors;

namespace USpring.Drawers
{
	public class SpringRotationEditorObject : SpringEditorObject
	{
		public const int SpringSize = 10;
		
		private const float VelocityFactor = 150;

		private const int GlobalAxisX = 0;
		private const int GlobalAxisY = 1;
		private const int GlobalAxisZ = 2;

		private const int LocalAxisX = 3;
		private const int LocalAxisY = 4;
		private const int LocalAxisZ = 5;

		private const int RotationAxisX = 6;
		private const int RotationAxisY = 7;
		private const int RotationAxisZ = 8;
		private const int Angle = 9;

		public readonly SerializedProperty SpAxisRestriction;

		public SpringRotationEditorObject(SerializedProperty spParentProperty) : base(spParentProperty, SpringRotation.SPRING_SIZE)
		{
			SpAxisRestriction = spParentProperty.FindPropertyRelative("axisRestriction");
		}

		public Vector3 InitialEuler
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[GlobalAxisX].GetInitialValue(),
					SpringValuesEditorObjects[GlobalAxisY].GetInitialValue(),
					SpringValuesEditorObjects[GlobalAxisZ].GetInitialValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[GlobalAxisX].SetInitialValue(value.x);
				SpringValuesEditorObjects[GlobalAxisY].SetInitialValue(value.y);
				SpringValuesEditorObjects[GlobalAxisZ].SetInitialValue(value.z);
			}
		}

		public Vector3 CurrentEuler
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[GlobalAxisX].GetCurrentValue(),
					SpringValuesEditorObjects[GlobalAxisY].GetCurrentValue(),
					SpringValuesEditorObjects[GlobalAxisZ].GetCurrentValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[GlobalAxisX].SetCurrentValue(value.x);
				SpringValuesEditorObjects[GlobalAxisY].SetCurrentValue(value.y);
				SpringValuesEditorObjects[GlobalAxisZ].SetCurrentValue(value.z);
			}
		}

		public Vector3 TargetEuler
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[GlobalAxisX].GetTarget(),
					SpringValuesEditorObjects[GlobalAxisY].GetTarget(),
					SpringValuesEditorObjects[GlobalAxisZ].GetTarget());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[GlobalAxisX].SetTarget(value.x);
				SpringValuesEditorObjects[GlobalAxisY].SetTarget(value.y);
				SpringValuesEditorObjects[GlobalAxisZ].SetTarget(value.z);
			}
		}

		public Vector3Bool Update
		{
			get
			{
				Vector3Bool res = new Vector3Bool(
					SpringValuesEditorObjects[GlobalAxisX].GetUpdate(),
					SpringValuesEditorObjects[GlobalAxisY].GetUpdate(),
					SpringValuesEditorObjects[GlobalAxisZ].GetUpdate());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[GlobalAxisX].SetUpdate(value.X);
				SpringValuesEditorObjects[GlobalAxisY].SetUpdate(value.Y);
				SpringValuesEditorObjects[GlobalAxisZ].SetUpdate(value.Z);
			}
		}

		public Vector3 ForceEuler
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[GlobalAxisX].GetForce(),
					SpringValuesEditorObjects[GlobalAxisY].GetForce(),
					SpringValuesEditorObjects[GlobalAxisZ].GetForce());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[GlobalAxisX].SetForce(value.x);
				SpringValuesEditorObjects[GlobalAxisY].SetForce(value.y);
				SpringValuesEditorObjects[GlobalAxisZ].SetForce(value.z);
			}
		}

		public Vector3 DragEuler
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[GlobalAxisX].GetDrag(),
					SpringValuesEditorObjects[GlobalAxisY].GetDrag(),
					SpringValuesEditorObjects[GlobalAxisZ].GetDrag());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[GlobalAxisX].SetDrag(value.x);
				SpringValuesEditorObjects[GlobalAxisY].SetDrag(value.y);
				SpringValuesEditorObjects[GlobalAxisZ].SetDrag(value.z);
			}
		}

		public Vector3 VelocityEuler
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[LocalAxisX].GetVelocity(),
					SpringValuesEditorObjects[LocalAxisY].GetVelocity(),
					SpringValuesEditorObjects[LocalAxisZ].GetVelocity());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[LocalAxisX].SetVelocity(value.x);
				SpringValuesEditorObjects[LocalAxisY].SetVelocity(value.y);
				SpringValuesEditorObjects[LocalAxisZ].SetVelocity(value.z);
			}
		}

		public Vector3 OperationValue
		{
			get
			{
				Vector3 res = new Vector3(
					SpringValuesEditorObjects[0].GetOperationValue(),
					SpringValuesEditorObjects[1].GetOperationValue(),
					SpringValuesEditorObjects[2].GetOperationValue());

				return res;
			}
			set
			{
				SpringValuesEditorObjects[0].SetOperationValue(value.x);
				SpringValuesEditorObjects[1].SetOperationValue(value.y);
				SpringValuesEditorObjects[2].SetOperationValue(value.z);
			}
		}

		public override void AddVelocityNudge()
		{
			

			VelocityEuler += OperationValue * VelocityFactor;
		}

		public override void SetCurrentValuesNudge()
		{
			CurrentEuler = OperationValue;
		}

		public override void SetTargetNudge()
		{
			TargetEuler = OperationValue;
		}

		public SpringRotation.AxisRestriction AxisRestriction
		{
			get
			{
				SpringRotation.AxisRestriction res = (SpringRotation.AxisRestriction)SpAxisRestriction.enumValueIndex;
				return res;
			}
		}

		public override float GetExtraFieldsHeight()
		{
			float res = 0f;

			return res;
		}
	}
}
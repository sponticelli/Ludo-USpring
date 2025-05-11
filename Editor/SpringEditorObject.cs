using UnityEditor;

namespace USpring
{
	public abstract class SpringEditorObject
	{
		protected const int X = 0;
		protected const int Y = 1;
		protected const int Z = 2;
		protected const int W = 3;

		protected const int R = 0;
		protected const int G = 1;
		protected const int B = 2;
		protected const int A = 3;

		public SerializedProperty SpParentroperty;

		public SerializedProperty SpCommonForceAndDrag;
		public SerializedProperty SpCommonForce;
		public SerializedProperty SpCommonDrag;

		public SerializedProperty SpSpringValues;
		public SerializedProperty SpShowDebugFields;

		public SerializedProperty SpUseInitialValues;
		public SerializedProperty SpUseCustomTarget;

		public SerializedProperty SpSpringEnabled;
		public SerializedProperty SpClampingEnabled;

		public SerializedProperty SpEventsEnabled;

		public SerializedProperty SpUnfolded;

		public SpringValuesEditorObject[] SpringValuesEditorObjects;

		public bool CommonForceAndDrag
		{
			get
			{
				return SpCommonForceAndDrag.boolValue;
			}
			set
			{
				SpCommonForceAndDrag.boolValue = value;
			}
		}

		public bool UseInitialValues
		{
			get
			{
				return SpUseInitialValues.boolValue;
			}
			set
			{
				SpUseInitialValues.boolValue = value;
			}
		}

		public bool UseCustomTarget
		{
			get
			{
				return SpUseCustomTarget.boolValue;
			}
			set
			{
				SpUseCustomTarget.boolValue = value;
			}
		}

		public bool Unfolded
		{
			get
			{
				return SpUnfolded.boolValue;
			}
			set
			{
				SpUnfolded.boolValue = value;
			}
		}

		public bool ClampingEnabled
		{
			get
			{
				return SpClampingEnabled.boolValue;
			}
			set
			{
				SpClampingEnabled.boolValue = value;
			}
		}

		public SpringEditorObject(SerializedProperty spParentroperty, int size)
		{
			this.SpParentroperty = spParentroperty;
			this.SpSpringValues = spParentroperty.FindPropertyRelative("springValues");
			SpSpringValues.arraySize = size;

			SpringValuesEditorObjects = new SpringValuesEditorObject[size];

			this.SpCommonForceAndDrag = spParentroperty.FindPropertyRelative("commonForceAndDrag");
			this.SpCommonForce = spParentroperty.FindPropertyRelative("commonForce");
			this.SpCommonDrag = spParentroperty.FindPropertyRelative("commonDrag");

			this.SpUseInitialValues = spParentroperty.FindPropertyRelative("useInitialValues");
			this.SpUseCustomTarget = spParentroperty.FindPropertyRelative("useCustomTarget");

			this.SpSpringEnabled = spParentroperty.FindPropertyRelative("springEnabled");
			this.SpClampingEnabled = spParentroperty.FindPropertyRelative("clampingEnabled");

			this.SpShowDebugFields = spParentroperty.FindPropertyRelative("showDebugFields");

			this.SpEventsEnabled = spParentroperty.FindPropertyRelative("eventsEnabled");

			this.SpUnfolded = spParentroperty.FindPropertyRelative("unfolded");

			CreateSpringValuesEditorObjects();
		}

		public int GetSize()
		{
			int res = SpringValuesEditorObjects.Length;
			return res;
		}

		public void AddSpringValuesEditorObject(SpringValuesEditorObject springValuesEditorObject, int index)
		{
			SpringValuesEditorObjects[index] = springValuesEditorObject;
		}

		public void CreateSpringValuesEditorObjects()
		{
			for (int i = 0; i < SpringValuesEditorObjects.Length; i++)
			{
				SerializedProperty sp = SpSpringValues.GetArrayElementAtIndex(i);

				SerializedProperty spInitialValue = sp.FindPropertyRelative("initialValue");

				SerializedProperty spClampTarget = sp.FindPropertyRelative("clampTarget");
				SerializedProperty spClampCurrentValue = sp.FindPropertyRelative("clampCurrentValue");
				SerializedProperty spStopSrpingOnCurrentValueClamp = sp.FindPropertyRelative("stopSpringOnCurrentValueClamp");

				SerializedProperty spMinValue = sp.FindPropertyRelative("minValue");
				SerializedProperty spMaxValue = sp.FindPropertyRelative("maxValue");

				SerializedProperty spUpdate = sp.FindPropertyRelative("update");
				SerializedProperty spForce = sp.FindPropertyRelative("force");
				SerializedProperty spDrag = sp.FindPropertyRelative("drag");

				SerializedProperty spTarget = sp.FindPropertyRelative("target");

				SerializedProperty spCurrentValue = sp.FindPropertyRelative("currentValue");
				SerializedProperty spVelocity = sp.FindPropertyRelative("velocity");

				SerializedProperty spCandidateValue = sp.FindPropertyRelative("candidateValue");

				SerializedProperty spOperationValue = sp.FindPropertyRelative("operationValue");

				SpringValuesEditorObjects[i] = new SpringValuesEditorObject(
					spInitialValue: spInitialValue,
					spClampTarget: spClampTarget, spClampCurrentValue: spClampCurrentValue, spStopSpringOnCurrentValueClamp: spStopSrpingOnCurrentValueClamp,
					spMinValue: spMinValue,
					spMaxValue: spMaxValue,
					spUpdate: spUpdate,
					spForce: spForce,
					spDrag: spDrag,
					spTarget: spTarget,
					spCurrentValue: spCurrentValue,
					spVelocity: spVelocity,
					spCandidateValue: spCandidateValue,
					spOperationValue: spOperationValue);
			}
		}

		public bool IsDebugShowEnabled()
		{
			bool res = SpShowDebugFields.boolValue;
			return res;
		}

		public bool IsClampTargetEnabled()
		{
			bool res = false;

			for (int i = 0; i < SpringValuesEditorObjects.Length; i++)
			{
				res = res || SpringValuesEditorObjects[i].GetClampTarget();
			}

			return res;
		}

		public void SetAllClampTarget(bool value)
		{
			for (int i = 0; i < SpringValuesEditorObjects.Length; i++)
			{
				SpringValuesEditorObjects[i].SetClampTarget(value);
			}
		}

		public void SetAllClampCurrentValue(bool value)
		{
			for (int i = 0; i < SpringValuesEditorObjects.Length; i++)
			{
				SpringValuesEditorObjects[i].SetClampCurrentValue(value);
			}
		}

		public bool IsClampCurrentValueEnabled()
		{
			bool res = false;

			for (int i = 0; i < SpringValuesEditorObjects.Length; i++)
			{
				res = res || SpringValuesEditorObjects[i].GetClampCurrentValue();
			}

			return res;
		}

		public virtual float GetExtraFieldsHeight()
		{
			float res = 0f;
			return res;
		}

		public abstract void AddVelocityNudge();

		public abstract void SetCurrentValuesNudge();

		public abstract void SetTargetNudge();
	}
}
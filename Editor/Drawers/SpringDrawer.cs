using UnityEditor;
using UnityEngine;
using static USpring.SpringsEditorUtility;

namespace USpring.Drawers
{
	public abstract class SpringDrawer
	{
		public static readonly string[] ComponentsLabelsXyzw =
		{
			"X",
			"Y",
			"Z",
			"W"
		};

		public static readonly string[] ComponentsLabelsRgba =
		{
			"R",
			"G",
			"B",
			"A"
		};

		public static readonly string[] ComponentsLabelsXYZ =
		{
			"X",
			"Y",
			"Z"
		};

		public static readonly string[] ComponentsLabelsXY =
{
			"X",
			"Y"
		};


		protected SerializedProperty ParentProperty;
		public SpringEditorObject SpringEditorObject;

		protected readonly bool IsFoldout;
		protected bool HasToggle;
		protected readonly bool IsDebugger;

		protected readonly bool DrawClampingArea;
		protected readonly bool DrawUpdateArea;

		protected abstract SpringEditorObject CreateSpringEditorObjectInstance(SerializedProperty parentProperty);


		public SpringDrawer(SerializedProperty parentProperty, bool isFoldout, bool drawClampingArea, bool drawUpdateArea, bool isDebugger)
		{
			if (parentProperty != null)
			{
				SetParentProperty(parentProperty);
			}

			IsFoldout = isFoldout;
			DrawClampingArea = drawClampingArea;
			DrawUpdateArea = drawUpdateArea;
			IsDebugger = isDebugger;
		}

		public void SetParentProperty(SerializedProperty parentProperty)
		{
			ParentProperty = parentProperty;
			RefreshSerializedProperties(parentProperty);
		}

		public virtual void RefreshSerializedProperties(SerializedProperty parentProperty)
		{
			SpringEditorObject = CreateSpringEditorObjectInstance(parentProperty);
		}

		public void OnGUI(Rect position)
		{
			OnGUI(position, ParentProperty, GUIContent.none);
		}

		public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			LabelWidth = EditorGUIUtility.currentViewWidth * 0.4f;
			LabelWidth = Mathf.Clamp(LabelWidth, 120f, 220f);
			
			RefreshSerializedProperties(property);
			
			if (IsFoldout)
			{
				Rect foldoutRect = new Rect(position.x, position.y, position.width, FoldoutHeight);
				property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);

				if (HasToggle)
				{
					Rect rectToggle = foldoutRect;
					rectToggle.x += EditorGUIUtility.labelWidth;
					EditorGUI.PropertyField(rectToggle, SpringEditorObject.SpSpringEnabled, GUIContent.none);
				}

				if (property.isExpanded)
				{
					Rect currentRect = foldoutRect;
					currentRect.x += 20f;
					currentRect.width -= 20f;

					UpdateCurrentRect(ref currentRect);
					DrawGUI(ref currentRect);
				}
			}
			else
			{
				Rect foldoutRect = new Rect(position.x, position.y, position.width, FoldoutHeight);
				property.isExpanded = true;
				Rect currentRect = foldoutRect;

				DrawGUI(ref currentRect);
			}

			EditorGUI.EndProperty();
		}

		private void DrawGUI(ref Rect currentRect)
		{
			if (IsDebugger)
			{
				SpringEditorObject.SpShowDebugFields.boolValue = DrawToggle(currentRect, SpringEditorObject.SpShowDebugFields.displayName, LabelWidth, SpringEditorObject.SpShowDebugFields.boolValue);
				UpdateCurrentRect(ref currentRect);

				if (SpringEditorObject.IsDebugShowEnabled())
				{
					DrawDebugGUI(ref currentRect);
					UpdateCurrentRect(ref currentRect);
				}
			}

			DrawCommonForceAndDrag(ref currentRect);
			UpdateCurrentRect(ref currentRect);

			if (DrawClampingArea)
			{
				UpdateCurrentRect(ref currentRect);
				DrawClampingEnabled(currentRect);

				if (SpringEditorObject.ClampingEnabled)
				{
					EditorGUI.BeginChangeCheck();
					UpdateCurrentRect(ref currentRect);
					DrawClampTarget(ref currentRect);
					if (EditorGUI.EndChangeCheck())
					{
						if (!SpringEditorObject.IsClampTargetEnabled())
						{
							SpringEditorObject.ClampingEnabled = false;
						}
					}

					UpdateCurrentRect(ref currentRect);
					DrawClampCurrentValue(ref currentRect);

					if (SpringEditorObject.IsClampCurrentValueEnabled())
					{
						UpdateCurrentRect(ref currentRect);
						DrawStopSpringOnCurrentValueClamp(ref currentRect);
					}

					UpdateCurrentRect(ref currentRect);
					DrawClampingRange(ref currentRect);
				}

				UpdateCurrentRect(ref currentRect);
			}

			UpdateCurrentRect(ref currentRect);
			SpringEditorObject.SpEventsEnabled.boolValue = DrawToggle(currentRect, FieldNameEventsEnabled, LabelWidth, SpringEditorObject.SpEventsEnabled.boolValue);
			UpdateCurrentRect(ref currentRect);

			if (Application.isPlaying)
			{
				UpdateCurrentRect(ref currentRect);
				EditorGUI.DropShadowLabel(currentRect, "Playmode Operations");

				Rect gampleyValuesAreaRect = currentRect;
				gampleyValuesAreaRect.height = EditorGUIUtility.singleLineHeight * 7f;
				gampleyValuesAreaRect.height += 5f;
				EditorGUI.DrawRect(gampleyValuesAreaRect, new Color(0f, 0f, 0f, 0.25f));

				EditorGUI.BeginDisabledGroup(true);

				currentRect.x += 15f;
				currentRect.width -= 50f;

				UpdateCurrentRect(ref currentRect);
				DrawCurrentValue(ref currentRect);
				UpdateCurrentRect(ref currentRect);
				DrawTarget(ref currentRect);
				UpdateCurrentRect(ref currentRect);
				DrawVelocity(ref currentRect);
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup(!Application.isPlaying);
				UpdateCurrentRect(ref currentRect);
				DrawNudgeOperationValues(ref currentRect);

				UpdateCurrentRect(ref currentRect, 2);
				DrawNudgeButtons(ref currentRect);
				EditorGUI.EndDisabledGroup();
			}
		}

		private void DrawClampingEnabled(Rect currentRect)
		{
			EditorGUI.BeginChangeCheck();
			SpringEditorObject.ClampingEnabled = DrawToggle(currentRect, "Clamping Enabled", TooltipClamping, LabelWidth, SpringEditorObject.ClampingEnabled);
			bool changes = EditorGUI.EndChangeCheck();
			if(changes)
			{
				if (SpringEditorObject.ClampingEnabled)
				{
					if (!SpringEditorObject.IsClampTargetEnabled())
					{
						SpringEditorObject.SetAllClampTarget(true);
						SpringEditorObject.SetAllClampCurrentValue(true);
					}
				}
			}
		}

		private void DrawDebugGUI(ref Rect currentRect)
		{
			EditorGUI.PropertyField(currentRect, SpringEditorObject.SpSpringValues, true);

			float squareYOffset = EditorGUIUtility.singleLineHeight;
			if (SpringEditorObject.SpSpringValues.isExpanded)
			{
				int numFieldsExpanded = GetNumDebugFieldsExpanded();
				
				squareYOffset += EditorGUIUtility.singleLineHeight * numFieldsExpanded * 15.0f;
				squareYOffset += 10.0f * numFieldsExpanded;
				squareYOffset += (EditorGUIUtility.singleLineHeight + 5f) * SpringEditorObject.SpSpringValues.arraySize;
				squareYOffset += EditorGUIUtility.singleLineHeight;
			}

			UpdateCurrentRect(ref currentRect, squareYOffset);
		}

		protected abstract void DrawInitialValues(ref Rect currentRect);

		protected abstract void DrawUpdate(ref Rect currentRect);

		protected abstract void DrawForce(ref Rect currentRect);

		protected abstract void DrawDrag(ref Rect currentRect);

		protected void DrawCurrentValue(ref Rect currentRect)
		{
			DrawCurrentValue(ref currentRect, "Current Value", LabelWidth);
		}

		public void DrawCurrentValue(ref Rect currentRect, string label)
		{
			DrawCurrentValue(ref currentRect, label, LabelWidth);
		}

		public abstract void DrawCurrentValue(ref Rect currentRect, string label, float labelWidth);

		protected abstract void DrawVelocity(ref Rect currentRect);

		protected void DrawTarget(ref Rect currentRect)
		{
			DrawTarget(ref currentRect, "Target", LabelWidth);
		}

		protected void DrawTarget(ref Rect currentRect, string label)
		{
			DrawTarget(ref currentRect, label, LabelWidth);
		}

		public abstract void DrawTarget(ref Rect currentRect, string label, float labelWidth);

		protected abstract void DrawClampingRange(ref Rect currentRect);

		protected abstract void DrawClampTarget(ref Rect currentRect);

		protected abstract void DrawClampCurrentValue(ref Rect currentRect);

		protected abstract void DrawStopSpringOnCurrentValueClamp(ref Rect currentRect);


		protected virtual void DrawNudgeButtons(ref Rect currentRect)
		{
			float padding = 20f;
			float halfPadding = padding * 0.5f;

			float widthButton = currentRect.width / 3f;
			widthButton -= padding;

			Rect buttonRect = currentRect;
			buttonRect.width = widthButton;

			buttonRect.x += padding;
			if (GUI.Button(buttonRect, "Set Current Values"))
			{
				SpringEditorObject.SetCurrentValuesNudge();
			}

			buttonRect.x += widthButton;
			buttonRect.x += padding;
			if (GUI.Button(buttonRect, "Set Target"))
			{
				SpringEditorObject.SetTargetNudge();
			}

			buttonRect.x += widthButton;
			buttonRect.x += padding;
			if (GUI.Button(buttonRect, "Add Velocity"))
			{
				SpringEditorObject.AddVelocityNudge();
			}
		}

		protected abstract void DrawNudgeOperationValues(ref Rect currentRect);

		public virtual void DrawCommonForceAndDrag(ref Rect currentRect)
		{
			SpringEditorObject.CommonForceAndDrag = DrawToggle(currentRect, "Common force & drag", 
				SpringEditorObject.SpCommonForceAndDrag.tooltip, LabelWidth, SpringEditorObject.CommonForceAndDrag);

			if (SpringEditorObject.CommonForceAndDrag)
			{
				EditorGUI.BeginDisabledGroup(!SpringEditorObject.CommonForceAndDrag);

				UpdateCurrentRect(ref currentRect);

				DrawSerializedPropertyWithRect(currentRect, LabelWidth, SpringEditorObject.SpCommonForce);
				UpdateCurrentRect(ref currentRect);

				DrawSerializedPropertyWithRect(currentRect, LabelWidth, SpringEditorObject.SpCommonDrag);

				EditorGUI.EndDisabledGroup();
			}
			else
			{
				EditorGUI.BeginDisabledGroup(SpringEditorObject.CommonForceAndDrag);
				UpdateCurrentRect(ref currentRect);
				DrawForce(ref currentRect);

				UpdateCurrentRect(ref currentRect);
				DrawDrag(ref currentRect);
				EditorGUI.EndDisabledGroup();
			}

			if (DrawUpdateArea)
			{
				UpdateCurrentRect(ref currentRect);
				DrawUpdate(ref currentRect);
			}

			DrawExtraFields(ref currentRect);
		}

		protected virtual void DrawExtraFields(ref Rect rect)
		{

		}

		public void DrawUseInitialValues(ref Rect currentRect, string label, float width)
		{
			DrawSerializedPropertyWithRect(currentRect, label, width, SpringEditorObject.SpUseInitialValues);
		}

		public void DrawInitialValues(ref Rect currentRect, string label, float width)
		{
			DrawInitialValues(ref currentRect);
		}

		public void DrawUseCustomTarget(ref Rect currentRect, string label, float width)
		{
			DrawSerializedPropertyWithRect(currentRect, label, width, SpringEditorObject.SpUseCustomTarget);
		}

		public float GetPropertyHeight()
		{
			float res = FoldoutHeight;

			if (SpringEditorObject == null)
			{
				RefreshSerializedProperties(ParentProperty);
			}

			if (ParentProperty.isExpanded)
			{
				res = EditorGUIUtility.singleLineHeight * 6f;
				if (DrawClampingArea)
				{
					res += EditorGUIUtility.singleLineHeight * 2f;
				}
				if (SpringEditorObject.ClampingEnabled)
				{
					res += EditorGUIUtility.singleLineHeight * 4f;
					if (SpringEditorObject.IsClampCurrentValueEnabled())
					{
						res += EditorGUIUtility.singleLineHeight;
					}
				}
				if (Application.isPlaying)
				{
					res += EditorGUIUtility.singleLineHeight * 9f;
				}
				if (IsDebugger)
				{
					res += EditorGUIUtility.singleLineHeight * 2f;
				}

				if (IsFoldout)
				{
					res += EditorGUIUtility.singleLineHeight * 2f;
				}

				res += SpringEditorObject.GetExtraFieldsHeight();

				if (SpringEditorObject.IsDebugShowEnabled())
				{
					float debugOffset = EditorGUIUtility.singleLineHeight;
					if (SpringEditorObject.SpSpringValues.isExpanded)
					{
						int numFieldsExpanded = GetNumDebugFieldsExpanded();

						debugOffset += EditorGUIUtility.singleLineHeight * numFieldsExpanded * 15.0f;
						debugOffset += 10.0f * numFieldsExpanded;
						debugOffset += (EditorGUIUtility.singleLineHeight + 5f) * SpringEditorObject.SpSpringValues.arraySize;
						debugOffset += EditorGUIUtility.singleLineHeight;
					}

					res += debugOffset;
				}
			}

			return res;
		}

		private int GetNumDebugFieldsExpanded()
		{
			int res = 0;

			for (int i = 0; i < SpringEditorObject.SpSpringValues.arraySize; i++)
			{
				if (SpringEditorObject.SpSpringValues.GetArrayElementAtIndex(i).isExpanded)
				{
					res++;
				}
			}

			return res;
		}
	}
}
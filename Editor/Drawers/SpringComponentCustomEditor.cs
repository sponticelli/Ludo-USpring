using UnityEditor;
using UnityEngine;
using static USpring.SpringsEditorUtility;
using System.Collections.Generic;

namespace USpring.Drawers
{
	/// <summary>
	/// Base class for all USpring component custom editors.
	/// Provides common functionality and UI layout for spring components.
	/// </summary>
	public abstract class SpringComponentCustomEditor : Editor
	{
		// Tab indices for the tabbed interface
		protected enum EditorTab
		{
			General = 0,
			Springs = 1,
			Advanced = 2,
			Help = 3
		}

		// Current selected tab
		protected EditorTab currentTab = EditorTab.General;

		// Serialized Properties
		protected SerializedProperty SpHasCustomInitialValues;
		protected SerializedProperty SpHasCustomTarget;
		protected SerializedProperty SpUseScaledTime;
		protected SerializedProperty SpAlwaysUseAnalyticalSolution;
		protected SerializedProperty SpDoesAutoInitialize;

		protected SerializedProperty SpGeneralPropertiesUnfolded;
		protected SerializedProperty SpInitialValuesUnfolded;

		// Flag to show the preview panel
		protected bool showPreview = true;

		protected virtual void RefreshSerializedProperties()
		{
			SpHasCustomInitialValues = serializedObject.FindProperty("hasCustomInitialValues");
			SpHasCustomTarget = serializedObject.FindProperty("hasCustomTarget");
			SpUseScaledTime = serializedObject.FindProperty("useScaledTime");
			SpAlwaysUseAnalyticalSolution = serializedObject.FindProperty("alwaysUseAnalyticalSolution");
			SpDoesAutoInitialize = serializedObject.FindProperty("doesAutoInitialize");

			SpGeneralPropertiesUnfolded = serializedObject.FindProperty("generalPropertiesUnfolded");
			SpInitialValuesUnfolded = serializedObject.FindProperty("initialValuesUnfolded");
		}

		// No need for RefreshStyles anymore as we're using USpringEditorStyles
		protected GUIStyle GUIStyleLabelTitle
		{
			get { return USpringEditorStyles.HeaderStyle; }
		}

		protected virtual void OnEnable()
		{
			RefreshSerializedProperties();
			CreateDrawers();
		}

		protected abstract void CreateDrawers();

		protected void DrawMainArea()
		{
			SpGeneralPropertiesUnfolded.boolValue = DrawRectangleArea("General Properties", SpGeneralPropertiesUnfolded.boolValue);

			if(SpGeneralPropertiesUnfolded.boolValue)
			{
				DrawMainAreaUnfolded();

				DrawSerializedProperty(SpUseScaledTime, LabelWidth);
				DrawSerializedProperty(SpAlwaysUseAnalyticalSolution, LabelWidth);
				DrawSerializedProperty(SpDoesAutoInitialize, LabelWidth);
			}
		}

		protected virtual void DrawMainAreaUnfolded()
		{

		}

		protected void DrawInitiaValues()
		{
			SpInitialValuesUnfolded.boolValue = DrawRectangleArea("Initial Values", SpInitialValuesUnfolded.boolValue);

			if(SpInitialValuesUnfolded.boolValue)
			{
				DrawInitialValuesSection();
			}
		}

		protected virtual void DrawInitialValuesSection()
		{
			EditorGUILayout.BeginVertical();

			SpHasCustomInitialValues.boolValue = DrawToggleLayout("Has Custom Initial Values", SpHasCustomInitialValues.tooltip, LabelWidth, SpHasCustomInitialValues.boolValue);

			if(SpHasCustomInitialValues.boolValue)
			{
				DrawCustomInitialValuesSection();
				Space();
			}

			if(!SpHasCustomInitialValues.boolValue && SpHasCustomTarget.boolValue)
			{
				Space();
			}
			DrawSerializedProperty(SpHasCustomTarget, LabelWidth);

			if(SpHasCustomTarget.boolValue)
			{
				DrawCustomInitialTarget();
			}

			EditorGUILayout.EndVertical();
		}

		protected abstract void DrawSprings();

		protected abstract void DrawCustomInitialValuesSection();

		protected abstract void DrawCustomInitialTarget();

		protected bool DrawRectangleArea(string areaName, bool foldout)
		{
			return DrawRectangleArea(height: GetTitleAreaHeight(), areaName: areaName, spToggle: null, areaEnabled: true, foldout: foldout);
		}

		protected bool DrawRectangleArea(string areaName, SerializedProperty spToggle, bool foldout)
		{
			return DrawRectangleArea(height: GetTitleAreaHeight(), areaName: areaName, spToggle: spToggle, areaEnabled: true, foldout: foldout);
		}

		protected bool DrawRectangleArea(float height, string areaName, bool foldout)
		{
			return DrawRectangleArea(height: height, areaName: areaName, spToggle: null, areaEnabled: true, foldout: foldout);
		}

		protected bool DrawRectangleArea(float height, string areaName, SerializedProperty spToggle, bool areaEnabled, bool foldout)
		{
			bool res = foldout;

			bool toggleHasChanged = false;

			EditorGUILayout.BeginHorizontal();

			Rect rect = EditorGUILayout.GetControlRect(hasLabel: false, height: height);
			EditorGUI.DrawRect(rect, USpringEditorStyles.HeaderBackgroundColor);

			Rect labelRect = new Rect(rect);
			labelRect.x = 25f;
			EditorGUIUtility.labelWidth = 100f;


			if(spToggle != null)
			{
				Rect toggleRect = new Rect(labelRect);
				toggleRect.width = 25f;

				EditorGUI.BeginChangeCheck();
				spToggle.boolValue = EditorGUI.Toggle(toggleRect, spToggle.boolValue);
				toggleHasChanged = EditorGUI.EndChangeCheck();
			}

			labelRect.width = areaName.Length * 10f;
			labelRect.x += 20f;

			EditorGUI.LabelField(labelRect, areaName, GUIStyleLabelTitle);

			EditorGUIUtility.labelWidth = 0f;

			EditorGUILayout.EndHorizontal();

			if(Event.current.type == EventType.MouseDown)
			{
				if(rect.Contains(Event.current.mousePosition))
				{
					res = !res;
					Event.current.Use();
				}
			}

			if(toggleHasChanged)
			{
				res = true;
			}

			if(spToggle != null)
			{
				res = res && spToggle.boolValue;
			}

			return res;
		}

		protected void DrawSimpleRectangleArea(float height, string areaName)
		{
			EditorGUILayout.BeginHorizontal();

			Rect rect = EditorGUILayout.GetControlRect(hasLabel: false, height: height);
			EditorGUI.DrawRect(rect, USpringEditorStyles.HeaderBackgroundColor);

			Rect labelRect = new Rect(rect);
			labelRect.x = rect.width * 0.5f;
			EditorGUIUtility.labelWidth = 100f;
			labelRect.width = EditorGUIUtility.labelWidth;
			EditorGUI.LabelField(labelRect, areaName, GUIStyleLabelTitle);

			EditorGUIUtility.labelWidth = 0f;

			EditorGUILayout.EndHorizontal();
		}

		protected float GetTitleAreaHeight()
		{
			float res = EditorGUIUtility.singleLineHeight * 1.5f;
			return res;
		}

		protected void DrawInitialValuesBySpring(string labelUseInitialValues, string labelInitialValues, float width, bool springEnabled, SpringDrawer springDrawer)
		{
			if(springEnabled)
			{
				Rect rectUseInitialValues = EditorGUILayout.GetControlRect(hasLabel: false, height: EditorGUIUtility.singleLineHeight);
				springDrawer.DrawUseInitialValues(ref rectUseInitialValues, labelUseInitialValues, width);

				if(springDrawer.SpringEditorObject.UseInitialValues)
				{
					DrawInitialValuesBySpring(labelInitialValues, width, springDrawer);

					Space();
				}
			}
		}

		protected void DrawInitialValuesBySpring(string labelInitialValues, float width, SpringDrawer springDrawer)
		{
			Rect rect = EditorGUILayout.GetControlRect(hasLabel: false, height: EditorGUIUtility.singleLineHeight);
			springDrawer.DrawInitialValues(ref rect, labelInitialValues, width);
		}

		protected void DrawCustomTargetBySpring(string labelUseCustomTarget, string labelCustomTarget, float width, bool springEnabled, SpringDrawer springDrawer)
		{
			if(springEnabled)
			{
				Rect rectUseInitialValues = EditorGUILayout.GetControlRect(hasLabel: false, height: EditorGUIUtility.singleLineHeight);
				springDrawer.DrawUseCustomTarget(ref rectUseInitialValues, labelUseCustomTarget, width);

				if(springDrawer.SpringEditorObject.UseCustomTarget)
				{
					Rect rect = EditorGUILayout.GetControlRect(hasLabel: false, height: EditorGUIUtility.singleLineHeight);
					springDrawer.DrawTarget(ref rect, labelCustomTarget, width);

					Space();
				}
			}
		}

		protected void DrawCustomTargetBySpring(string labelCustomTarget, float width, SpringDrawer springDrawer)
		{
			Rect rect = EditorGUILayout.GetControlRect(hasLabel: false, height: EditorGUIUtility.singleLineHeight);
			springDrawer.DrawTarget(ref rect, labelCustomTarget, width);
		}

		protected void DrawSpring(SpringDrawer springDrawer)
		{
			DrawSpring(springDrawer.SpringEditorObject.SpParentroperty.displayName, springDrawer, null);
		}

		protected void DrawSpringWithEnableToggle(SpringDrawer springDrawer)
		{
			DrawSpring(springDrawer.SpringEditorObject.SpParentroperty.displayName, springDrawer, springDrawer.SpringEditorObject.SpSpringEnabled);
		}

		protected void DrawSpring(string springName, SpringDrawer springDrawer, SerializedProperty spToggle)
		{
			springDrawer.SpringEditorObject.Unfolded = DrawRectangleArea(
				height: GetTitleAreaHeight(),
				areaName: springName,
				areaEnabled: true,
				spToggle: spToggle,
				foldout: springDrawer.SpringEditorObject.Unfolded);


			if(springDrawer.SpringEditorObject.Unfolded)
			{
				Rect propertyRect = EditorGUILayout.GetControlRect(hasLabel: false, height: springDrawer.GetPropertyHeight());

				springDrawer.OnGUI(propertyRect);
			}
		}

		protected virtual void DrawInfoArea()
		{

		}

		/// <summary>
		/// Draws the tabbed interface for the inspector
		/// </summary>
		protected void DrawTabbedInterface()
		{
			// Draw tab buttons
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Toggle(currentTab == EditorTab.General, "General", USpringEditorStyles.TabButtonStyle))
			{
				currentTab = EditorTab.General;
			}

			if (GUILayout.Toggle(currentTab == EditorTab.Springs, "Springs", USpringEditorStyles.TabButtonStyle))
			{
				currentTab = EditorTab.Springs;
			}

			if (GUILayout.Toggle(currentTab == EditorTab.Advanced, "Advanced", USpringEditorStyles.TabButtonStyle))
			{
				currentTab = EditorTab.Advanced;
			}

			if (GUILayout.Toggle(currentTab == EditorTab.Help, "Help", USpringEditorStyles.TabButtonStyle))
			{
				currentTab = EditorTab.Help;
			}

			EditorGUILayout.EndHorizontal();

			// Draw horizontal line
			USpringEditorStyles.DrawHorizontalLine(USpringEditorStyles.SecondaryColor, 1f, 0f);

			// Draw tab content
			EditorGUILayout.Space(4);

			switch (currentTab)
			{
				case EditorTab.General:
					DrawGeneralTab();
					break;

				case EditorTab.Springs:
					DrawSpringsTab();
					break;

				case EditorTab.Advanced:
					DrawAdvancedTab();
					break;

				case EditorTab.Help:
					DrawHelpTab();
					break;
			}
		}

		/// <summary>
		/// Draws the General tab content
		/// </summary>
		protected virtual void DrawGeneralTab()
		{
			DrawMainArea();
			DrawInitiaValues();
			DrawInfoArea();
		}

		/// <summary>
		/// Draws the Springs tab content
		/// </summary>
		protected virtual void DrawSpringsTab()
		{
			// Draw preview toggle
			EditorGUILayout.BeginHorizontal();
			showPreview = EditorGUILayout.Toggle("Show Preview", showPreview, GUILayout.Width(120));
			EditorGUILayout.EndHorizontal();

			// Draw springs
			DrawSprings();

			// Draw preview if enabled
			if (showPreview)
			{
				DrawSpringPreview();
			}
		}

		/// <summary>
		/// Draws a preview of the spring behavior
		/// </summary>
		protected virtual void DrawSpringPreview()
		{
			// Override in derived classes to draw specific spring previews
		}

		/// <summary>
		/// Draws the Advanced tab content
		/// </summary>
		protected virtual void DrawAdvancedTab()
		{
			// Draw advanced settings
			USpringEditorStyles.DrawSubHeader("Advanced Settings");
			USpringEditorStyles.BeginSection();

			DrawSerializedProperty(SpUseScaledTime, USpringEditorStyles.DefaultLabelWidth);
			DrawSerializedProperty(SpAlwaysUseAnalyticalSolution, USpringEditorStyles.DefaultLabelWidth);
			DrawSerializedProperty(SpDoesAutoInitialize, USpringEditorStyles.DefaultLabelWidth);

			USpringEditorStyles.EndSection();

			// Draw debug info
			USpringEditorStyles.DrawSubHeader("Debug Information");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("Component Type", target.GetType().Name);
			EditorGUILayout.LabelField("GameObject", target.name);

			USpringEditorStyles.EndSection();
		}

		/// <summary>
		/// Draws the Help tab content
		/// </summary>
		protected virtual void DrawHelpTab()
		{
			USpringEditorStyles.DrawSubHeader("About USpring");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("USpring is a powerful spring physics system for Unity that allows you to create smooth, physically-based animations.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("This component allows you to animate properties using spring physics.");

			USpringEditorStyles.EndSection();

			USpringEditorStyles.DrawSubHeader("Common Settings");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("Force", "Higher values make the spring stronger and faster.");
			EditorGUILayout.LabelField("Drag", "Higher values make the spring more damped and less bouncy.");
			EditorGUILayout.LabelField("Use Scaled Time", "When enabled, the spring will use Time.timeScale for updates.");
			EditorGUILayout.LabelField("Analytical Solution", "When enabled, uses a more accurate but potentially more expensive calculation.");

			USpringEditorStyles.EndSection();

			DrawComponentSpecificHelp();
		}

		/// <summary>
		/// Draws component-specific help information
		/// </summary>
		protected virtual void DrawComponentSpecificHelp()
		{
			// Override in derived classes to provide component-specific help
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Draw the tabbed interface
			DrawTabbedInterface();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
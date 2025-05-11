using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	/// <summary>
	/// Custom editor for the FloatSpringComponent.
	/// </summary>
	/// <remarks>
	/// This class is responsible for drawing the custom editor for the FloatSpringComponent in the Unity Inspector.
	/// It inherits from SpringComponentCustomEditor and provides specific implementations for drawing float springs.
	/// </remarks>
	[CustomEditor(typeof(FloatSpringComponent))]
	[CanEditMultipleObjects]
	public class FloatSpringComponentEditor : SpringComponentCustomEditor
	{
		private SpringFloatDrawer springFloatDrawer;

		protected override void CreateDrawers()
		{
			springFloatDrawer = new SpringFloatDrawer(serializedObject.FindProperty("springFloat"), false, false);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from the spring
				SerializedProperty forceProperty = springFloatDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty dragProperty = springFloatDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset
				USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the spring
			DrawSpring(springFloatDrawer);
		}

		/// <summary>
		/// Draws a preview of the float spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters
			float force = springFloatDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float drag = springFloatDrawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw the preview
			USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "Float Spring Preview");
		}

		/// <summary>
		/// Draws component-specific help information
		/// </summary>
		protected override void DrawComponentSpecificHelp()
		{
			USpringEditorStyles.DrawSubHeader("Float Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Float Spring Component allows you to animate single float values with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Common uses include animating opacity, scale factors, or any other single numeric property.");

			USpringEditorStyles.EndSection();

			// Draw example usage
			USpringEditorStyles.DrawSubHeader("Example Usage");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("1. Attach this component to any GameObject");
			EditorGUILayout.LabelField("2. Set your desired target value");
			EditorGUILayout.LabelField("3. Call SetTarget() from code to animate to a new value");
			EditorGUILayout.LabelField("4. Access CurrentValue to get the current spring value");

			USpringEditorStyles.EndSection();
		}

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring("Initial Value", LabelWidth, springFloatDrawer);
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring("Target", LabelWidth, springFloatDrawer);
		}
	}
}
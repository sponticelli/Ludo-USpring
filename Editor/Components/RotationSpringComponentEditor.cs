using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	/// <summary>
	/// Custom editor for the RotationSpringComponent.
	/// </summary>
	/// <remarks>
	/// This class is responsible for drawing the custom editor for the RotationSpringComponent.
	/// It inherits from SpringComponentCustomEditor and overrides the necessary methods to provide
	/// a custom UI for the rotation spring component.
	/// </remarks>
	[CustomEditor(typeof(RotationSpringComponent))]
	[CanEditMultipleObjects]
	public class RotationSpringComponentEditor : SpringComponentCustomEditor
	{
		private SpringRotationDrawer springRotationDrawer;


		protected override void CreateDrawers()
		{
			springRotationDrawer = new SpringRotationDrawer(serializedObject.FindProperty("springRotation"), false, false);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from the spring
				SerializedProperty forceProperty = springRotationDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty dragProperty = springRotationDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset
				USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the spring
			DrawSpring(springRotationDrawer);
		}

		/// <summary>
		/// Draws a preview of the rotation spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters
			float force = springRotationDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float drag = springRotationDrawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw the preview for each rotation component (X, Y, Z)
			float[] forces = new float[] { force, force, force };
			float[] drags = new float[] { drag, drag, drag };
			Color[] colors = new Color[] {
				USpringEditorStyles.RotationSpringColor, // X
				USpringEditorStyles.RotationSpringColor, // Y
				USpringEditorStyles.RotationSpringColor  // Z
			};
			string[] labels = new string[] { "X", "Y", "Z" };

			// Draw the multi-component preview
			USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);
		}

		/// <summary>
		/// Draws component-specific help information
		/// </summary>
		protected override void DrawComponentSpecificHelp()
		{
			USpringEditorStyles.DrawSubHeader("Rotation Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Rotation Spring Component allows you to animate rotations with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("It handles quaternion interpolation correctly to avoid gimbal lock and provide smooth rotation.");

			USpringEditorStyles.EndSection();

			// Draw example usage
			USpringEditorStyles.DrawSubHeader("Example Usage");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("1. Attach this component to any GameObject");
			EditorGUILayout.LabelField("2. Set your desired target rotation");
			EditorGUILayout.LabelField("3. Call SetTarget() from code to animate to a new rotation");
			EditorGUILayout.LabelField("4. Access CurrentValue to get the current spring rotation");

			USpringEditorStyles.EndSection();
		}

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring(
				labelInitialValues: "Initial Value",
				width: LabelWidth,
				springDrawer: springRotationDrawer);
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring(
				labelCustomTarget: "Target",
				width: LabelWidth,
				springDrawer: springRotationDrawer);
		}
	}
}
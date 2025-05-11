using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	/// <summary>
	/// Custom editor for the Vector3SpringComponent.
	/// </summary>
	/// <remarks>
	/// This class is responsible for drawing the custom editor for the Vector3SpringComponent in the Unity Inspector.
	/// It inherits from SpringComponentCustomEditor and provides specific implementations for drawing the Vector3 spring properties.
	/// </remarks>
	[CustomEditor(typeof(Vector3SpringComponent))]
	[CanEditMultipleObjects]
	public class Vector3SpringComponentEditor : SpringComponentCustomEditor
	{
		private SpringVector3Drawer springVector3Drawer;


		protected override void CreateDrawers()
		{
			springVector3Drawer = new SpringVector3Drawer(serializedObject.FindProperty("springVector3"), false, false);
		}

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring(
				labelInitialValues: "Initial Value",
				width: LabelWidth,
				springDrawer: springVector3Drawer);
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring(
				labelCustomTarget: "Target",
				width: LabelWidth,
				springDrawer: springVector3Drawer
			);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from the spring
				SerializedProperty forceProperty = springVector3Drawer.SpringEditorObject.SpCommonForce;
				SerializedProperty dragProperty = springVector3Drawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset
				USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the spring
			DrawSpring(springVector3Drawer);
		}

		/// <summary>
		/// Draws a preview of the vector3 spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters
			float force = springVector3Drawer.SpringEditorObject.SpCommonForce.floatValue;
			float drag = springVector3Drawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw the preview for each vector component (X, Y, Z)
			float[] forces = new float[] { force, force, force };
			float[] drags = new float[] { drag, drag, drag };
			Color[] colors = new Color[] {
				USpringEditorStyles.FloatSpringColor, // X
				USpringEditorStyles.FloatSpringColor, // Y
				USpringEditorStyles.FloatSpringColor  // Z
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
			USpringEditorStyles.DrawSubHeader("Vector3 Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Vector3 Spring Component allows you to animate 3D vectors with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Common uses include animating positions, scales, or any other 3D vector property.");

			USpringEditorStyles.EndSection();

			// Draw example usage
			USpringEditorStyles.DrawSubHeader("Example Usage");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("1. Attach this component to any GameObject");
			EditorGUILayout.LabelField("2. Set your desired target vector");
			EditorGUILayout.LabelField("3. Call SetTarget() from code to animate to a new vector");
			EditorGUILayout.LabelField("4. Access CurrentValue to get the current spring value");

			USpringEditorStyles.EndSection();
		}
	}
}
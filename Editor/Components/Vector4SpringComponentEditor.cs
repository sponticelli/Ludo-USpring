using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	[CustomEditor(typeof(Vector4SpringComponent))]
	[CanEditMultipleObjects]
	public class Vector4SpringComponentEditor : SpringComponentCustomEditor
	{
		private SerializedProperty spSpringVector4;
		private SpringVector4Drawer springVector4Drawer;

		protected override void RefreshSerializedProperties()
		{
			base.RefreshSerializedProperties();

			spSpringVector4 = serializedObject.FindProperty("springVector4");
		}

		protected override void CreateDrawers()
		{
			springVector4Drawer = new SpringVector4Drawer(spSpringVector4, false, false);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from the spring
				SerializedProperty forceProperty = springVector4Drawer.SpringEditorObject.SpCommonForce;
				SerializedProperty dragProperty = springVector4Drawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset
				USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the spring
			DrawSpring(springVector4Drawer);
		}

		/// <summary>
		/// Draws a preview of the vector4 spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters
			float force = springVector4Drawer.SpringEditorObject.SpCommonForce.floatValue;
			float drag = springVector4Drawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw the preview for each vector component (X, Y, Z, W)
			float[] forces = new float[] { force, force, force, force };
			float[] drags = new float[] { drag, drag, drag, drag };
			Color[] colors = new Color[] {
				USpringEditorStyles.Vector4SpringColor, // X
				USpringEditorStyles.Vector4SpringColor, // Y
				USpringEditorStyles.Vector4SpringColor, // Z
				USpringEditorStyles.Vector4SpringColor  // W
			};
			string[] labels = new string[] { "X", "Y", "Z", "W" };

			// Draw the multi-component preview
			USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);
		}

		/// <summary>
		/// Draws component-specific help information
		/// </summary>
		protected override void DrawComponentSpecificHelp()
		{
			USpringEditorStyles.DrawSubHeader("Vector4 Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Vector4 Spring Component allows you to animate 4D vectors with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Common uses include animating shader parameters, tangent vectors, or any other 4D vector property.");

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

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring(
				labelInitialValues: "Initial Value",
				width: LabelWidth,
				springDrawer: springVector4Drawer);
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring(
				labelCustomTarget: "Target",
				width: LabelWidth,
				springDrawer: springVector4Drawer);
		}
	}
}
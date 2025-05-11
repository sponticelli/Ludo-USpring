using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	/// <summary>
	/// Custom editor for the Vector2SpringComponent.
	/// </summary>
	/// <remarks>
	/// This class is responsible for drawing the custom editor for the Vector2SpringComponent in the Unity Inspector.
	/// It inherits from SpringComponentCustomEditor and provides specific implementations for drawing the Vector2 spring properties.
	/// </remarks>
	[CustomEditor(typeof(Vector2SpringComponent))]
	[CanEditMultipleObjects]
	public class Vector2SpringComponentEditor : SpringComponentCustomEditor
	{
		private SerializedProperty spSpringVector2;

		private SpringVector2Drawer springVector2Drawer;

		protected override void RefreshSerializedProperties()
		{
			base.RefreshSerializedProperties();

			spSpringVector2 = serializedObject.FindProperty("springVector2");
		}

		protected override void CreateDrawers()
		{
			springVector2Drawer = new SpringVector2Drawer(spSpringVector2, false, false);
		}

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring(
				labelInitialValues: "Initial Value",
				width: LabelWidth,
				springDrawer: springVector2Drawer);
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring(
				labelCustomTarget: "Target",
				width: LabelWidth,
				springDrawer: springVector2Drawer);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from the spring
				SerializedProperty forceProperty = springVector2Drawer.SpringEditorObject.SpCommonForce;
				SerializedProperty dragProperty = springVector2Drawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset
				USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the spring
			DrawSpring(springVector2Drawer);
		}

		/// <summary>
		/// Draws a preview of the vector2 spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters
			float force = springVector2Drawer.SpringEditorObject.SpCommonForce.floatValue;
			float drag = springVector2Drawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw the preview for each vector component (X, Y)
			float[] forces = new float[] { force, force };
			float[] drags = new float[] { drag, drag };
			Color[] colors = new Color[] {
				USpringEditorStyles.Vector2SpringColor, // X
				USpringEditorStyles.Vector2SpringColor  // Y
			};
			string[] labels = new string[] { "X", "Y" };

			// Draw the multi-component preview
			USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);
		}

		/// <summary>
		/// Draws component-specific help information
		/// </summary>
		protected override void DrawComponentSpecificHelp()
		{
			USpringEditorStyles.DrawSubHeader("Vector2 Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Vector2 Spring Component allows you to animate 2D vectors with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Common uses include animating UI positions, 2D game object positions, or any other 2D vector property.");

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
using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	/// <summary>
	/// Custom editor for the ColorSpringComponent.
	/// </summary>
	/// <remarks>
	/// This class is responsible for drawing the custom editor for the ColorSpringComponent in the Unity Inspector.
	/// It inherits from SpringComponentCustomEditor and provides additional functionality specific to color springs.
	/// </remarks>
	[CustomEditor(typeof(ColorSpringComponent))]
	[CanEditMultipleObjects]
	public class ColorSpringComponentEditor : SpringComponentCustomEditor
	{
		private SpringColorDrawer springColorDrawer;

		private SerializedProperty spAutoUpdate;
		private SerializedProperty spAutoUpdatedObjectIsRenderer;
		private SerializedProperty spAutoUpdatedRenderer;
		private SerializedProperty spAutoUpdatedUiGraphic;

		protected override void RefreshSerializedProperties()
		{
			base.RefreshSerializedProperties();

			spAutoUpdate = serializedObject.FindProperty("autoUpdate");
			spAutoUpdatedObjectIsRenderer = serializedObject.FindProperty("autoUpdatedObjectIsRenderer");
			spAutoUpdatedRenderer = serializedObject.FindProperty("autoUpdatedRenderer");
			spAutoUpdatedUiGraphic = serializedObject.FindProperty("autoUpdatedUiGraphic");
		}

		protected override void CreateDrawers()
		{
			springColorDrawer = new SpringColorDrawer(serializedObject.FindProperty("colorSpring"), false, false);
		}

		protected override void DrawMainAreaUnfolded()
		{
			DrawSerializedProperty(spAutoUpdate, LabelWidth);

			if (spAutoUpdate.boolValue)
			{
				EditorGUILayout.LabelField("Choose if the target is a Renderer(3D) or a Graphic(UI)");
				DrawSerializedProperty(spAutoUpdatedObjectIsRenderer, LabelWidth);

				if (spAutoUpdatedObjectIsRenderer.boolValue)
				{
					DrawSerializedProperty(spAutoUpdatedRenderer, LabelWidth);
				}
				else
				{
					DrawSerializedProperty(spAutoUpdatedUiGraphic, LabelWidth);
				}
			}
		}

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring(
				labelInitialValues: "Initial Value",
				width: LabelWidth,
				springDrawer: springColorDrawer
				);
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring(
				labelCustomTarget: "Target",
				width: LabelWidth,
				springDrawer: springColorDrawer
				);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from the spring
				SerializedProperty forceProperty = springColorDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty dragProperty = springColorDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset
				USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the spring
			DrawSpring(springColorDrawer);
		}

		/// <summary>
		/// Draws a preview of the color spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters
			float force = springColorDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float drag = springColorDrawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw the preview for each color component (R, G, B, A)
			float[] forces = new float[] { force, force, force, force };
			float[] drags = new float[] { drag, drag, drag, drag };
			Color[] colors = new Color[] {
				new Color(1, 0, 0, 1), // Red
				new Color(0, 1, 0, 1), // Green
				new Color(0, 0, 1, 1), // Blue
				new Color(0.5f, 0.5f, 0.5f, 1) // Alpha (shown as gray)
			};
			string[] labels = new string[] { "R", "G", "B", "A" };

			// Draw the multi-component preview
			USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);
		}

		/// <summary>
		/// Draws component-specific help information
		/// </summary>
		protected override void DrawComponentSpecificHelp()
		{
			USpringEditorStyles.DrawSubHeader("Color Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Color Spring Component allows you to animate colors with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Auto Update", "When enabled, automatically updates a Renderer or UI Graphic with the spring color.");
			EditorGUILayout.LabelField("Target Type", "Choose between a 3D Renderer or a UI Graphic as the target.");

			USpringEditorStyles.EndSection();

			// Draw example usage
			USpringEditorStyles.DrawSubHeader("Example Usage");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("1. Attach this component to a GameObject with a Renderer or UI Graphic");
			EditorGUILayout.LabelField("2. Enable Auto Update and select the target");
			EditorGUILayout.LabelField("3. Set your desired target color");
			EditorGUILayout.LabelField("4. Call SetTarget() from code to animate to a new color");

			USpringEditorStyles.EndSection();
		}
	}
}
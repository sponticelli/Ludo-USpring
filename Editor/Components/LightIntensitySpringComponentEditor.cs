using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	/// <summary>
	/// Custom editor for the LightIntensitySpringComponent.
	/// </summary>
	/// <remarks>
	/// This class is responsible for drawing the custom editor for the LightIntensitySpringComponent.
	/// It inherits from SpringComponentCustomEditor and provides additional functionality specific to light intensity springs.
	/// </remarks>
	[CustomEditor(typeof(LightIntensitySpringComponent))]
	[CanEditMultipleObjects]
	public class LightIntensitySpringComponentCustomEditor : SpringComponentCustomEditor
	{
		private SerializedProperty spAutoUpdatedLight;

		private SpringFloatDrawer lightIntensitySpringDrawer;

		protected override void RefreshSerializedProperties()
		{
			base.RefreshSerializedProperties();

			spAutoUpdatedLight = serializedObject.FindProperty("autoUpdatedLight");
		}

		protected override void CreateDrawers()
		{
			lightIntensitySpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("lightIntensitySpring"), false, false);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from the spring
				SerializedProperty forceProperty = lightIntensitySpringDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty dragProperty = lightIntensitySpringDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset
				USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the spring
			DrawSpring(lightIntensitySpringDrawer);
		}

		/// <summary>
		/// Draws a preview of the light intensity spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters
			float force = lightIntensitySpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float drag = lightIntensitySpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw the preview
			USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "Light Intensity Spring Preview");

			// Draw a simple visualization of light intensity change
			EditorGUILayout.Space(8);
			USpringEditorStyles.DrawSubHeader("Light Intensity Visualization");

			// Draw a simple light visualization
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(100));

			// Draw background
			EditorGUI.DrawRect(rect, new Color(0.1f, 0.1f, 0.1f, 1f));

			// Draw light source
			float centerX = rect.x + rect.width / 2;
			float centerY = rect.y + rect.height / 2;
			float lightSize = 20f;

			EditorGUI.DrawRect(new Rect(centerX - lightSize/2, centerY - lightSize/2, lightSize, lightSize), Color.yellow);

			// Draw light rays
			DrawLightRays(new Vector2(centerX, centerY), 0.5f, new Color(1f, 1f, 0.5f, 0.3f));
			DrawLightRays(new Vector2(centerX, centerY), 1.0f, new Color(1f, 1f, 0.5f, 0.7f));
		}

		/// <summary>
		/// Helper method to draw light rays
		/// </summary>
		private void DrawLightRays(Vector2 center, float intensity, Color color)
		{
			Handles.color = color;

			int rayCount = 16;
			float radius = 50f * intensity;

			for (int i = 0; i < rayCount; i++)
			{
				float angle = (i / (float)rayCount) * Mathf.PI * 2;
				Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
				Vector3 centerVec3 = new Vector3(center.x, center.y, 0);
				Handles.DrawLine(centerVec3, centerVec3 + direction);
			}
		}

		/// <summary>
		/// Draws component-specific help information
		/// </summary>
		protected override void DrawComponentSpecificHelp()
		{
			USpringEditorStyles.DrawSubHeader("Light Intensity Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Light Intensity Spring Component allows you to animate light intensity with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("It can create dynamic lighting effects with natural-feeling transitions.");

			USpringEditorStyles.EndSection();

			// Draw example usage
			USpringEditorStyles.DrawSubHeader("Example Usage");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("1. Attach this component to a GameObject with a Light");
			EditorGUILayout.LabelField("2. Assign the Light to the Auto Updated Light field");
			EditorGUILayout.LabelField("3. Set your desired target intensity");
			EditorGUILayout.LabelField("4. Call SetTarget() from code to animate to a new intensity");

			USpringEditorStyles.EndSection();

			// Draw tips
			USpringEditorStyles.DrawSubHeader("Tips");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("• Use with flickering effects for more natural-looking light sources");
			EditorGUILayout.LabelField("• Combine with other spring components for coordinated lighting effects");
			EditorGUILayout.LabelField("• Great for creating dynamic lighting responses to game events");

			USpringEditorStyles.EndSection();
		}

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring("Initial Value", LabelWidth, lightIntensitySpringDrawer);
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring("Target", LabelWidth, lightIntensitySpringDrawer);
		}

		protected override void DrawMainAreaUnfolded()
		{
			DrawSerializedProperty(spAutoUpdatedLight, LabelWidth);
		}

		protected override void DrawInfoArea()
		{
			EditorGUILayout.Space(2);

			if (spAutoUpdatedLight.objectReferenceValue == null)
			{
				EditorGUILayout.HelpBox("AutoUpdatedLight is not assigned!", MessageType.Error);
			}
		}
	}
}
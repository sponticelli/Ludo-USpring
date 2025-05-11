using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	/// <summary>
	/// Custom editor for the <see cref="CamFovOrSizeSpringComponent"/>.
	/// </summary>
	[CustomEditor(typeof(CamFovOrSizeSpringComponent))]
	[CanEditMultipleObjects]
	public class CamFovOrSizeSpringComponentEditor : SpringComponentCustomEditor
	{
		private SpringFloatDrawer fovSpringDrawer;

		private SerializedProperty spAutoUpdatedCamera;

		protected override void CreateDrawers()
		{
			fovSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("fovSpring"), false, false);
		}

		protected override void RefreshSerializedProperties()
		{
			base.RefreshSerializedProperties();

			spAutoUpdatedCamera = serializedObject.FindProperty("autoUpdatedCamera");
		}

		protected override void DrawMainAreaUnfolded()
		{
			DrawSerializedProperty(spAutoUpdatedCamera, LabelWidth);
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring(
				labelCustomTarget: "Target Fov",
				width: LabelWidth,
				springDrawer: fovSpringDrawer);
		}

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring(
				labelInitialValues: "Initial Fov",
				width: LabelWidth,
				springDrawer: fovSpringDrawer
			);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from the spring
				SerializedProperty forceProperty = fovSpringDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty dragProperty = fovSpringDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset
				USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the spring
			DrawSpring(fovSpringDrawer);
		}

		/// <summary>
		/// Draws a preview of the camera FOV/size spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters
			float force = fovSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float drag = fovSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw the preview
			USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "FOV/Size Spring Preview");

			// Draw a simple visualization of FOV/size change
			EditorGUILayout.Space(8);
			USpringEditorStyles.DrawSubHeader("FOV/Size Effect Visualization");

			// Draw a simple camera frustum visualization
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(100));

			// Draw camera body
			EditorGUI.DrawRect(new Rect(rect.x + rect.width / 2 - 10, rect.y + rect.height - 20, 20, 20), new Color(0.3f, 0.3f, 0.3f, 1f));

			// Draw initial frustum
			DrawCameraFrustum(rect, 0.5f, new Color(0.5f, 0.5f, 0.5f, 0.5f));

			// Draw animated frustum based on spring preview
			// This is a simplified visualization that just shows the target state
			DrawCameraFrustum(rect, 0.8f, USpringEditorStyles.FloatSpringColor);

			// Add labels
			EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, 20), "Initial FOV/Size");
			EditorGUI.LabelField(new Rect(rect.x + rect.width - 100, rect.y, 100, 20), "Target FOV/Size");
		}

		/// <summary>
		/// Helper method to draw a simple camera frustum visualization
		/// </summary>
		private void DrawCameraFrustum(Rect rect, float widthFactor, Color color)
		{
			float centerX = rect.x + rect.width / 2;
			float bottomY = rect.y + rect.height - 20;
			float frustumWidth = rect.width * widthFactor;
			float frustumHeight = rect.height * 0.7f;

			Vector3[] points = new Vector3[4];
			points[0] = new Vector3(centerX - frustumWidth / 2, rect.y, 0);
			points[1] = new Vector3(centerX + frustumWidth / 2, rect.y, 0);
			points[2] = new Vector3(centerX, bottomY, 0);
			points[3] = new Vector3(centerX, bottomY, 0);

			Handles.color = color;
			Handles.DrawLine(points[0], points[2]);
			Handles.DrawLine(points[1], points[2]);
			Handles.DrawLine(points[0], points[1]);
		}

		/// <summary>
		/// Draws component-specific help information
		/// </summary>
		protected override void DrawComponentSpecificHelp()
		{
			USpringEditorStyles.DrawSubHeader("Camera FOV/Size Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Camera FOV/Size Spring Component allows you to animate camera properties with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("It automatically detects whether the camera is perspective (FOV) or orthographic (Size) and animates the appropriate property.");

			USpringEditorStyles.EndSection();

			// Draw example usage
			USpringEditorStyles.DrawSubHeader("Example Usage");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("1. Attach this component to a GameObject with a Camera");
			EditorGUILayout.LabelField("2. Assign the Camera to the Auto Updated Camera field");
			EditorGUILayout.LabelField("3. Set your desired target FOV or Size");
			EditorGUILayout.LabelField("4. Call SetTarget() from code to animate to a new FOV/Size");

			USpringEditorStyles.EndSection();

			// Draw tips
			USpringEditorStyles.DrawSubHeader("Tips");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("• For perspective cameras, this component animates the Field of View");
			EditorGUILayout.LabelField("• For orthographic cameras, this component animates the Size");
			EditorGUILayout.LabelField("• Use lower force and higher drag for smoother camera movements");
			EditorGUILayout.LabelField("• This component is great for creating dynamic camera effects like zoom in/out");

			USpringEditorStyles.EndSection();
		}

		protected override void DrawInfoArea()
		{
			EditorGUILayout.Space(2);

			if (spAutoUpdatedCamera.objectReferenceValue == null)
			{
				EditorGUILayout.HelpBox("AutoUpdatedCamera is not assigned!", MessageType.Error);
			}
		}
	}
}
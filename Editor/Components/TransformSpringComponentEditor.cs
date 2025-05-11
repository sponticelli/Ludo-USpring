using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	[CustomEditor(typeof(TransformSpringComponent))]
	[CanEditMultipleObjects]
	public class TransformSpringComponentEditor : SpringComponentCustomEditor
	{
		private SerializedProperty spFollowerTransform;

		private SerializedProperty spUseTransformAsTarget;
		private SerializedProperty spTargetTransform;

		private SerializedProperty spPositionSpring;
		private SerializedProperty spRotationSpring;
		private SerializedProperty spScaleSpring;

		private SerializedProperty spSpaceType;

		private SpringVector3Drawer springPositionDrawer;
		private SpringRotationDrawer springRotationDrawer;
		private SpringVector3Drawer springScaleDrawer;

		protected override void RefreshSerializedProperties()
		{
			base.RefreshSerializedProperties();

			spPositionSpring = serializedObject.FindProperty("positionSpring");
			spRotationSpring = serializedObject.FindProperty("rotationSpring");
			spScaleSpring = serializedObject.FindProperty("scaleSpring");

			spFollowerTransform = serializedObject.FindProperty("followerTransform");
			spUseTransformAsTarget = serializedObject.FindProperty("useTransformAsTarget");
			spTargetTransform = serializedObject.FindProperty("targetTransform");

			spSpaceType = serializedObject.FindProperty("spaceType");
		}

		protected override void CreateDrawers()
		{
			springPositionDrawer = new SpringVector3Drawer(spPositionSpring, false, false);
			springRotationDrawer = new SpringRotationDrawer(spRotationSpring, false, false);
			springScaleDrawer = new SpringVector3Drawer(spScaleSpring, false, false);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from all springs
				SerializedProperty posForceProperty = springPositionDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty posDragProperty = springPositionDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty rotForceProperty = springRotationDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty rotDragProperty = springRotationDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty scaleForceProperty = springScaleDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty scaleDragProperty = springScaleDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset to all springs
				USpringPresets.ApplyPreset(serializedObject, preset, posForceProperty, posDragProperty, analyticalSolutionProperty);
				USpringPresets.ApplyPreset(serializedObject, preset, rotForceProperty, rotDragProperty, null);
				USpringPresets.ApplyPreset(serializedObject, preset, scaleForceProperty, scaleDragProperty, null);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the springs
			DrawSpring("Position Spring", spPositionSpring, springPositionDrawer);
			DrawSpring("Rotation Spring", spRotationSpring, springRotationDrawer);
			DrawSpring("Scale Spring", spScaleSpring, springScaleDrawer);
		}

		/// <summary>
		/// Draws a preview of the transform spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters for each component
			float posForce = springPositionDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float posDrag = springPositionDrawer.SpringEditorObject.SpCommonDrag.floatValue;
			float rotForce = springRotationDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float rotDrag = springRotationDrawer.SpringEditorObject.SpCommonDrag.floatValue;
			float scaleForce = springScaleDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float scaleDrag = springScaleDrawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw a visualization of the transform spring behavior
			EditorGUILayout.Space(8);
			USpringEditorStyles.DrawSubHeader("Transform Spring Visualization");

			// Draw a simple transform visualization
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(150));

			// Draw background
			EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1f));

			// Draw grid
			DrawGrid(rect);

			// Draw the transform visualization
			DrawTransformVisualization(rect);

			// Draw the spring previews
			EditorGUILayout.Space(8);

			// Position spring preview
			if (springPositionDrawer.SpringEditorObject.SpSpringEnabled.boolValue)
			{
				USpringEditorStyles.DrawSubHeader("Position Spring Preview");
				float[] forces = new float[] { posForce, posForce, posForce };
				float[] drags = new float[] { posDrag, posDrag, posDrag };
				Color[] colors = new Color[] {
					USpringEditorStyles.Vector3SpringColor, // X
					USpringEditorStyles.Vector3SpringColor, // Y
					USpringEditorStyles.Vector3SpringColor  // Z
				};
				string[] labels = new string[] { "X", "Y", "Z" };

				USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);
			}

			// Rotation spring preview
			if (springRotationDrawer.SpringEditorObject.SpSpringEnabled.boolValue)
			{
				USpringEditorStyles.DrawSubHeader("Rotation Spring Preview");
				float[] forces = new float[] { rotForce, rotForce, rotForce };
				float[] drags = new float[] { rotDrag, rotDrag, rotDrag };
				Color[] colors = new Color[] {
					USpringEditorStyles.RotationSpringColor, // X
					USpringEditorStyles.RotationSpringColor, // Y
					USpringEditorStyles.RotationSpringColor  // Z
				};
				string[] labels = new string[] { "X", "Y", "Z" };

				USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);
			}

			// Scale spring preview
			if (springScaleDrawer.SpringEditorObject.SpSpringEnabled.boolValue)
			{
				USpringEditorStyles.DrawSubHeader("Scale Spring Preview");
				float[] forces = new float[] { scaleForce, scaleForce, scaleForce };
				float[] drags = new float[] { scaleDrag, scaleDrag, scaleDrag };
				Color[] colors = new Color[] {
					new Color(0.4f, 0.8f, 0.4f, 1f), // X
					new Color(0.4f, 0.8f, 0.4f, 1f), // Y
					new Color(0.4f, 0.8f, 0.4f, 1f)  // Z
				};
				string[] labels = new string[] { "X", "Y", "Z" };

				USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);
			}
		}

		/// <summary>
		/// Draws a grid in the visualization area
		/// </summary>
		private void DrawGrid(Rect rect)
		{
			// Draw center lines
			float centerX = rect.x + rect.width / 2;
			float centerY = rect.y + rect.height / 2;

			Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
			Handles.DrawLine(new Vector3(centerX, rect.y, 0), new Vector3(centerX, rect.y + rect.height, 0));
			Handles.DrawLine(new Vector3(rect.x, centerY, 0), new Vector3(rect.x + rect.width, centerY, 0));

			// Draw grid lines
			Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.1f);
			float gridSize = 20f;

			// Vertical lines
			for (float x = centerX - gridSize; x >= rect.x; x -= gridSize)
			{
				Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.y + rect.height, 0));
			}

			for (float x = centerX + gridSize; x <= rect.x + rect.width; x += gridSize)
			{
				Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.y + rect.height, 0));
			}

			// Horizontal lines
			for (float y = centerY - gridSize; y >= rect.y; y -= gridSize)
			{
				Handles.DrawLine(new Vector3(rect.x, y, 0), new Vector3(rect.x + rect.width, y, 0));
			}

			for (float y = centerY + gridSize; y <= rect.y + rect.height; y += gridSize)
			{
				Handles.DrawLine(new Vector3(rect.x, y, 0), new Vector3(rect.x + rect.width, y, 0));
			}
		}

		/// <summary>
		/// Draws a visualization of the transform with initial and target states
		/// </summary>
		private void DrawTransformVisualization(Rect rect)
		{
			float centerX = rect.x + rect.width / 2;
			float centerY = rect.y + rect.height / 2;

			// Draw initial transform (gray)
			DrawCube(centerX - 40, centerY, 30, 30, new Color(0.5f, 0.5f, 0.5f, 0.5f), 0);

			// Draw target transform (colored)
			Color targetColor = new Color(0.4f, 0.8f, 1f, 0.8f);

			// Apply position spring effect
			float targetX = centerX + 40;
			float targetY = centerY;

			// Apply rotation spring effect
			float targetRotation = 30f;

			// Apply scale spring effect
			float targetWidth = 40f;
			float targetHeight = 40f;

			DrawCube(targetX, targetY, targetWidth, targetHeight, targetColor, targetRotation);

			// Draw labels
			EditorGUI.LabelField(new Rect(centerX - 70, centerY - 50, 60, 20), "Initial");
			EditorGUI.LabelField(new Rect(targetX + 10, targetY - 50, 60, 20), "Target");

			// Draw arrow
			DrawArrow(centerX, centerY, targetX, targetY, new Color(0.4f, 0.8f, 0.4f, 0.8f));
		}

		/// <summary>
		/// Draws a cube with the specified parameters
		/// </summary>
		private void DrawCube(float x, float y, float width, float height, Color color, float rotation)
		{
			// Save the current matrix
			Matrix4x4 matrix = GUI.matrix;

			// Apply rotation
			GUIUtility.RotateAroundPivot(rotation, new Vector2(x, y));

			// Draw the cube
			EditorGUI.DrawRect(new Rect(x - width/2, y - height/2, width, height), color);

			// Draw outline
			Handles.color = new Color(color.r, color.g, color.b, color.a + 0.2f);
			Vector3[] points = new Vector3[5];
			points[0] = new Vector3(x - width/2, y - height/2, 0);
			points[1] = new Vector3(x + width/2, y - height/2, 0);
			points[2] = new Vector3(x + width/2, y + height/2, 0);
			points[3] = new Vector3(x - width/2, y + height/2, 0);
			points[4] = new Vector3(x - width/2, y - height/2, 0);
			Handles.DrawPolyLine(points);

			// Draw axes
			Handles.color = Color.red;
			Handles.DrawLine(new Vector3(x, y, 0), new Vector3(x + width/2, y, 0));
			Handles.color = Color.green;
			Handles.DrawLine(new Vector3(x, y, 0), new Vector3(x, y - height/2, 0));

			// Restore the matrix
			GUI.matrix = matrix;
		}

		/// <summary>
		/// Draws an arrow between two points
		/// </summary>
		private void DrawArrow(float x1, float y1, float x2, float y2, Color color)
		{
			Handles.color = color;

			// Draw the line
			Handles.DrawLine(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0));

			// Calculate the direction
			Vector2 direction = new Vector2(x2 - x1, y2 - y1).normalized;
			Vector2 perpendicular = new Vector2(-direction.y, direction.x);

			// Draw the arrowhead
			float arrowSize = 10f;
			Vector2 arrowTip = new Vector2(x2, y2);
			Vector2 arrowBase = arrowTip - direction * arrowSize;

			Vector2 arrowLeft = arrowBase + perpendicular * arrowSize * 0.5f;
			Vector2 arrowRight = arrowBase - perpendicular * arrowSize * 0.5f;

			Handles.DrawLine(arrowTip, arrowLeft);
			Handles.DrawLine(arrowTip, arrowRight);
		}

		/// <summary>
		/// Draws component-specific help information
		/// </summary>
		protected override void DrawComponentSpecificHelp()
		{
			USpringEditorStyles.DrawSubHeader("Transform Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Transform Spring Component allows you to animate position, rotation, and scale with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("It can be used to create smooth, physically-based animations for any GameObject's transform.");

			USpringEditorStyles.EndSection();

			// Draw example usage
			USpringEditorStyles.DrawSubHeader("Example Usage");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("1. Attach this component to any GameObject");
			EditorGUILayout.LabelField("2. Assign the Follower Transform (usually this GameObject's transform)");
			EditorGUILayout.LabelField("3. Enable the springs you want to use (position, rotation, scale)");
			EditorGUILayout.LabelField("4. Set your desired target values or use a target transform");
			EditorGUILayout.LabelField("5. Call SetTarget() from code to animate to new values");

			USpringEditorStyles.EndSection();

			// Draw space type explanation
			USpringEditorStyles.DrawSubHeader("Space Types");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("World Space: The spring will operate in world coordinates");
			EditorGUILayout.LabelField("Local Space: The spring will operate relative to the parent's transform");
			EditorGUILayout.LabelField("Self Space: The spring will operate relative to the object's own transform");

			USpringEditorStyles.EndSection();

			// Draw tips
			USpringEditorStyles.DrawSubHeader("Tips");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("• You can enable/disable individual springs (position, rotation, scale)");
			EditorGUILayout.LabelField("• Use Transform As Target allows you to follow another transform");
			EditorGUILayout.LabelField("• Each spring can have different force and drag values");
			EditorGUILayout.LabelField("• Great for camera follow, UI animations, and object movement");

			USpringEditorStyles.EndSection();
		}
		protected override void DrawMainAreaUnfolded()
		{
			DrawSerializedProperty(spFollowerTransform, LabelWidth);

			DrawSerializedProperty(spUseTransformAsTarget, LabelWidth);

			if (spUseTransformAsTarget.boolValue)
			{
				DrawSerializedProperty(spTargetTransform, LabelWidth);
			}

			DrawSerializedProperty(spSpaceType, LabelWidth);
		}

		private void DrawSpring(string springName, SerializedProperty spSpring, SpringDrawer springDrawer)
		{
			springDrawer.SpringEditorObject.Unfolded = DrawRectangleArea(areaName: springName, springDrawer.SpringEditorObject.SpSpringEnabled, springDrawer.SpringEditorObject.Unfolded);

			if (springDrawer.SpringEditorObject.Unfolded)
			{
				springDrawer.SetParentProperty(spSpring);
				Rect propertyRect = EditorGUILayout.GetControlRect(hasLabel: false, height: springDrawer.GetPropertyHeight());
				springDrawer.OnGUI(propertyRect, spSpring, GUIContent.none);
			}
		}

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring(
				labelUseInitialValues: "Use Initial Position",
				labelInitialValues: "Initial Position",
				width: LabelWidth,
				springEnabled: springPositionDrawer.SpringEditorObject.SpSpringEnabled.boolValue,
				springDrawer: springPositionDrawer);

			DrawInitialValuesBySpring(
				labelUseInitialValues: "Use Initial Rotation",
				labelInitialValues: "Initial Rotation",
				width: LabelWidth,
				springEnabled: springRotationDrawer.SpringEditorObject.SpSpringEnabled.boolValue,
				springDrawer: springRotationDrawer);

			DrawInitialValuesBySpring(
				labelUseInitialValues: "Use Initial Scale",
				labelInitialValues: "Initial Scale",
				width: LabelWidth,
				springEnabled: springScaleDrawer.SpringEditorObject.SpSpringEnabled.boolValue,
				springDrawer: springScaleDrawer);
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring(
				labelUseCustomTarget: "Use Custom Target Position",
				labelCustomTarget: "Target Position",
				width: LabelWidth,
				springEnabled: springPositionDrawer.SpringEditorObject.SpSpringEnabled.boolValue,
				springDrawer: springPositionDrawer);

			DrawCustomTargetBySpring(
				labelUseCustomTarget: "Use Custom Target Rotation",
				labelCustomTarget: "Target Rotation",
				width: LabelWidth,
				springEnabled: springRotationDrawer.SpringEditorObject.SpSpringEnabled.boolValue,
				springDrawer: springRotationDrawer);

			DrawCustomTargetBySpring(
				labelUseCustomTarget: "Use Custom Target Scale",
				labelCustomTarget: "Target Scale",
				width: LabelWidth,
				springEnabled: springScaleDrawer.SpringEditorObject.SpSpringEnabled.boolValue,
				springDrawer: springScaleDrawer);

			if (spUseTransformAsTarget.boolValue)
			{
				SpringsEditorUtility.Space(2);
				EditorGUILayout.HelpBox("Custom Target won't have any effect becouse you're using a transform as a target", MessageType.Warning);
			}
		}

		protected override void DrawInfoArea()
		{
			EditorGUILayout.Space(2);

			if(spFollowerTransform.objectReferenceValue == null)
			{
				EditorGUILayout.HelpBox("Follower is not assigned!", MessageType.Error);
			}
			if(spUseTransformAsTarget.boolValue && SpHasCustomTarget.boolValue)
			{
				EditorGUILayout.HelpBox("You are using a transform as a target and custom initial target at the same time. Disable one of them", MessageType.Warning);
			}
			if (spUseTransformAsTarget.boolValue && spTargetTransform.objectReferenceValue == null)
			{
				EditorGUILayout.HelpBox("Use Transform As Target is enabled but the reference is not assigned", MessageType.Warning);
			}
		}
	}
}
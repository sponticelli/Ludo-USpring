using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	/// <summary>
	/// Custom editor for the RigidbodySpringComponent.
	/// </summary>
	/// <remarks>
	/// This class is responsible for drawing the custom editor for the RigidbodySpringComponent in the Unity Editor.
	/// It inherits from SpringComponentCustomEditor and provides additional functionality specific to Rigidbody springs.
	/// </remarks>
	[CustomEditor(typeof(RigidbodySpringComponent))]
	[CanEditMultipleObjects]
	public class RigidbodySpringComponentEditor : SpringComponentCustomEditor
	{
		private SpringVector3Drawer springPositionDrawer;
		private SpringRotationDrawer springRotationDrawer;

		private SerializedProperty spUseTransformAsTarget;
		private SerializedProperty spRigidBodyFollower;
		private SerializedProperty spTarget;

		protected override void RefreshSerializedProperties()
		{
			base.RefreshSerializedProperties();

			spUseTransformAsTarget = serializedObject.FindProperty("useTransformAsTarget");
			spRigidBodyFollower = serializedObject.FindProperty("rigidBodyFollower");
			spTarget = serializedObject.FindProperty("target");
		}

		protected override void CreateDrawers()
		{
			springPositionDrawer = new SpringVector3Drawer(serializedObject.FindProperty("positionSpring"), false, false);
			springRotationDrawer = new SpringRotationDrawer(serializedObject.FindProperty("rotationSpring"), false, false);
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
				springEnabled: springRotationDrawer.SpringEditorObject.SpSpringEnabled.boolValue,
				width: LabelWidth,
				springDrawer: springRotationDrawer);
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
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from both springs
				SerializedProperty posForceProperty = springPositionDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty posDragProperty = springPositionDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty rotForceProperty = springRotationDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty rotDragProperty = springRotationDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset to both springs
				USpringPresets.ApplyPreset(serializedObject, preset, posForceProperty, posDragProperty, analyticalSolutionProperty);
				USpringPresets.ApplyPreset(serializedObject, preset, rotForceProperty, rotDragProperty, null);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the springs
			DrawSpringWithEnableToggle(springPositionDrawer);
			DrawSpringWithEnableToggle(springRotationDrawer);
		}

		/// <summary>
		/// Draws a preview of the rigidbody spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Draw a visualization of rigidbody physics
			EditorGUILayout.Space(8);
			USpringEditorStyles.DrawSubHeader("Rigidbody Physics Visualization");

			// Draw a simple physics visualization
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(200));

			// Draw background
			EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1f));

			// Draw ground
			float groundHeight = 20f;
			Rect groundRect = new Rect(rect.x, rect.y + rect.height - groundHeight, rect.width, groundHeight);
			EditorGUI.DrawRect(groundRect, new Color(0.3f, 0.5f, 0.3f, 1f));

			// Draw grid
			DrawPhysicsGrid(rect);

			// Draw rigidbody visualization
			DrawRigidbodyVisualization(rect);

			// Draw spring previews if enabled
			EditorGUILayout.Space(8);

			// Position spring preview
			if (springPositionDrawer.SpringEditorObject.SpSpringEnabled.boolValue)
			{
				// Get the spring parameters
				float posForce = springPositionDrawer.SpringEditorObject.SpCommonForce.floatValue;
				float posDrag = springPositionDrawer.SpringEditorObject.SpCommonDrag.floatValue;

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
				// Get the spring parameters
				float rotForce = springRotationDrawer.SpringEditorObject.SpCommonForce.floatValue;
				float rotDrag = springRotationDrawer.SpringEditorObject.SpCommonDrag.floatValue;

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
		}

		/// <summary>
		/// Draws a grid in the physics visualization area
		/// </summary>
		private void DrawPhysicsGrid(Rect rect)
		{
			// Draw center lines
			float centerX = rect.x + rect.width / 2;

			Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
			Handles.DrawLine(new Vector3(centerX, rect.y, 0), new Vector3(centerX, rect.y + rect.height - 20, 0));

			// Draw grid lines
			Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.1f);
			float gridSize = 20f;

			// Vertical lines
			for (float x = centerX - gridSize; x >= rect.x; x -= gridSize)
			{
				Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.y + rect.height - 20, 0));
			}

			for (float x = centerX + gridSize; x <= rect.x + rect.width; x += gridSize)
			{
				Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.y + rect.height - 20, 0));
			}

			// Horizontal lines
			float groundY = rect.y + rect.height - 20;
			for (float y = groundY - gridSize; y >= rect.y; y -= gridSize)
			{
				Handles.DrawLine(new Vector3(rect.x, y, 0), new Vector3(rect.x + rect.width, y, 0));
			}
		}

		/// <summary>
		/// Draws a visualization of the rigidbody with initial and target states
		/// </summary>
		private void DrawRigidbodyVisualization(Rect rect)
		{
			float centerX = rect.x + rect.width / 2;
			float groundY = rect.y + rect.height - 20;

			// Draw initial rigidbody (gray)
			float initialX = centerX - 60;
			float initialY = groundY - 60;
			float initialRotation = 0;
			DrawRigidbodyCube(initialX, initialY, 40, 40, new Color(0.5f, 0.5f, 0.5f, 0.7f), initialRotation);

			// Draw target rigidbody (colored)
			float targetX = centerX + 60;
			float targetY = groundY - 80;
			float targetRotation = 30;
			DrawRigidbodyCube(targetX, targetY, 40, 40, new Color(0.4f, 0.8f, 1f, 0.7f), targetRotation);

			// Draw animated rigidbody (green)
			float animatedX = centerX;
			float animatedY = groundY - 70;
			float animatedRotation = 15;
			DrawRigidbodyCube(animatedX, animatedY, 40, 40, new Color(0.4f, 0.8f, 0.4f, 0.8f), animatedRotation);

			// Draw labels
			EditorGUI.LabelField(new Rect(initialX - 20, initialY - 50, 60, 20), "Initial");
			EditorGUI.LabelField(new Rect(targetX - 20, targetY - 50, 60, 20), "Target");
			EditorGUI.LabelField(new Rect(animatedX - 30, animatedY - 50, 80, 20), "Animated");

			// Draw force vectors
			DrawForceVector(animatedX, animatedY, targetX - animatedX, targetY - animatedY, new Color(1f, 0.5f, 0.2f, 0.8f));

			// Draw gravity vector
			DrawForceVector(animatedX, animatedY, 0, 30, new Color(1f, 0.2f, 0.2f, 0.8f));

			// Draw labels for forces
			EditorGUI.LabelField(new Rect(animatedX + 20, animatedY - 20, 80, 20), "Spring Force");
			EditorGUI.LabelField(new Rect(animatedX - 80, animatedY + 20, 80, 20), "Gravity");
		}

		/// <summary>
		/// Draws a cube representing a rigidbody
		/// </summary>
		private void DrawRigidbodyCube(float x, float y, float width, float height, Color color, float rotation)
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

			// Restore the matrix
			GUI.matrix = matrix;
		}

		/// <summary>
		/// Draws a force vector
		/// </summary>
		private void DrawForceVector(float x, float y, float dx, float dy, Color color)
		{
			Handles.color = color;

			// Scale the vector
			float scale = 0.5f;
			dx *= scale;
			dy *= scale;

			// Draw the line
			Handles.DrawLine(new Vector3(x, y, 0), new Vector3(x + dx, y + dy, 0));

			// Calculate the direction
			Vector2 direction = new Vector2(dx, dy).normalized;
			Vector2 perpendicular = new Vector2(-direction.y, direction.x);

			// Draw the arrowhead
			float arrowSize = 10f;
			Vector2 arrowTip = new Vector2(x + dx, y + dy);
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
			USpringEditorStyles.DrawSubHeader("Rigidbody Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Rigidbody Spring Component allows you to animate physics objects with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("It works with Unity's Rigidbody component to create physically-based animations that respect physics constraints.");

			USpringEditorStyles.EndSection();

			// Draw example usage
			USpringEditorStyles.DrawSubHeader("Example Usage");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("1. Attach this component to a GameObject with a Rigidbody");
			EditorGUILayout.LabelField("2. Assign the Rigidbody Follower (usually this GameObject's Rigidbody)");
			EditorGUILayout.LabelField("3. Enable the springs you want to use (position, rotation)");
			EditorGUILayout.LabelField("4. Set your desired target values or use a target transform");
			EditorGUILayout.LabelField("5. Call SetTarget() from code to animate to new values");

			USpringEditorStyles.EndSection();

			// Draw physics tips
			USpringEditorStyles.DrawSubHeader("Physics Tips");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("• This component uses AddForce and AddTorque to move the Rigidbody");
			EditorGUILayout.LabelField("• It respects collisions, gravity, and other physics constraints");
			EditorGUILayout.LabelField("• Higher force values may be needed compared to non-physics springs");
			EditorGUILayout.LabelField("• Great for physically-based animations like doors, platforms, etc.");
			EditorGUILayout.LabelField("• Works best with Rigidbodies that have appropriate mass and drag settings");

			USpringEditorStyles.EndSection();
		}

		protected override void DrawMainAreaUnfolded()
		{
			DrawSerializedProperty(spRigidBodyFollower, LabelWidth);
			DrawSerializedProperty(spUseTransformAsTarget, LabelWidth);

			if (spUseTransformAsTarget.boolValue)
			{
				DrawSerializedProperty(spTarget, LabelWidth);
			}
		}

		protected override void DrawInfoArea()
		{
			EditorGUILayout.Space(2);

			if (spRigidBodyFollower.objectReferenceValue == null)
			{
				EditorGUILayout.HelpBox("RigidBodyFollower is not assigned!", MessageType.Error);
			}
		}
	}
}
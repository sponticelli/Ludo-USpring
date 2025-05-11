using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	/// <summary>
	/// Custom editor for the AnchoredPositionSpringComponent.
	/// </summary>
	/// <remarks>
	/// This class is responsible for drawing the custom editor for the AnchoredPositionSpringComponent.
	/// It inherits from SpringComponentCustomEditor and provides additional functionality specific to the AnchoredPositionSpringComponent.
	/// </remarks>
	[CustomEditor(typeof(AnchoredPositionSpringComponent))]
	[CanEditMultipleObjects]
	public class AnchoredPositionSpringComponentEditor : SpringComponentCustomEditor
	{
		private SpringVector2Drawer anchoredPositionSpringDrawer;

		private SerializedProperty spFollowRectTransform;
		private SerializedProperty spTargetRectTransform;
		private SerializedProperty spUseTransformAsTarget;

		protected override void CreateDrawers()
		{
			anchoredPositionSpringDrawer = new SpringVector2Drawer(serializedObject.FindProperty("anchoredPositionSpring"), false, false);
		}

		protected override void RefreshSerializedProperties()
		{
			base.RefreshSerializedProperties();

			spFollowRectTransform = serializedObject.FindProperty("followRectTransform");
			spTargetRectTransform = serializedObject.FindProperty("targetRectTransform");
			spUseTransformAsTarget = serializedObject.FindProperty("useTransformAsTarget");
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring("Target", LabelWidth, anchoredPositionSpringDrawer);
		}

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring("Initial Values", LabelWidth, anchoredPositionSpringDrawer);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from the spring
				SerializedProperty forceProperty = anchoredPositionSpringDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty dragProperty = anchoredPositionSpringDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset
				USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the spring
			DrawSpring(anchoredPositionSpringDrawer);
		}

		/// <summary>
		/// Draws a preview of the anchored position spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters
			float force = anchoredPositionSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float drag = anchoredPositionSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw the preview for each component (X, Y)
			float[] forces = new float[] { force, force };
			float[] drags = new float[] { drag, drag };
			Color[] colors = new Color[] {
				USpringEditorStyles.Vector2SpringColor, // X
				USpringEditorStyles.Vector2SpringColor  // Y
			};
			string[] labels = new string[] { "X", "Y" };

			// Draw the multi-component preview
			USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);

			// Draw a visualization of UI anchored position
			EditorGUILayout.Space(8);
			USpringEditorStyles.DrawSubHeader("UI Anchored Position Visualization");

			// Draw a simple UI visualization
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(150));

			// Draw background (canvas)
			EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f, 1f));

			// Draw parent RectTransform
			float parentWidth = rect.width * 0.8f;
			float parentHeight = rect.height * 0.8f;
			Rect parentRect = new Rect(
				rect.x + (rect.width - parentWidth) / 2,
				rect.y + (rect.height - parentHeight) / 2,
				parentWidth,
				parentHeight
			);
			EditorGUI.DrawRect(parentRect, new Color(0.3f, 0.3f, 0.3f, 1f));

			// Draw initial UI element
			float elementWidth = parentWidth * 0.3f;
			float elementHeight = parentHeight * 0.3f;
			Rect initialRect = new Rect(
				parentRect.x + parentWidth * 0.2f,
				parentRect.y + parentHeight * 0.2f,
				elementWidth,
				elementHeight
			);
			EditorGUI.DrawRect(initialRect, new Color(0.5f, 0.5f, 0.5f, 0.7f));

			// Draw target UI element
			Rect targetRect = new Rect(
				parentRect.x + parentWidth * 0.5f,
				parentRect.y + parentHeight * 0.5f,
				elementWidth,
				elementHeight
			);
			EditorGUI.DrawRect(targetRect, new Color(0.4f, 0.8f, 1f, 0.7f));

			// Draw labels
			EditorGUI.LabelField(new Rect(initialRect.x, initialRect.y - 20, 100, 20), "Initial");
			EditorGUI.LabelField(new Rect(targetRect.x, targetRect.y - 20, 100, 20), "Target");

			// Draw arrow
			DrawUIArrow(
				initialRect.x + initialRect.width / 2,
				initialRect.y + initialRect.height / 2,
				targetRect.x + targetRect.width / 2,
				targetRect.y + targetRect.height / 2,
				new Color(0.4f, 0.8f, 0.4f, 0.8f)
			);
		}

		/// <summary>
		/// Draws an arrow between two points
		/// </summary>
		private void DrawUIArrow(float x1, float y1, float x2, float y2, Color color)
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
			USpringEditorStyles.DrawSubHeader("Anchored Position Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Anchored Position Spring Component allows you to animate UI element positions with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("It works specifically with RectTransforms in the Unity UI system to animate anchored positions.");

			USpringEditorStyles.EndSection();

			// Draw example usage
			USpringEditorStyles.DrawSubHeader("Example Usage");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("1. Attach this component to a UI GameObject with a RectTransform");
			EditorGUILayout.LabelField("2. Assign the Follow RectTransform (usually this GameObject's RectTransform)");
			EditorGUILayout.LabelField("3. Set your desired target anchored position or use a target RectTransform");
			EditorGUILayout.LabelField("4. Call SetTarget() from code to animate to a new position");

			USpringEditorStyles.EndSection();

			// Draw tips
			USpringEditorStyles.DrawSubHeader("Tips");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("• Great for UI animations like sliding panels, buttons, and menus");
			EditorGUILayout.LabelField("• Use Transform As Target allows you to follow another UI element");
			EditorGUILayout.LabelField("• Works well with UI layout groups and content size fitters");
			EditorGUILayout.LabelField("• Can be combined with other spring components for complex UI animations");

			USpringEditorStyles.EndSection();
		}

		protected override void DrawMainAreaUnfolded()
		{
			DrawSerializedProperty(spFollowRectTransform, LabelWidth);

			SpringsEditorUtility.Space();

			DrawSerializedProperty(spUseTransformAsTarget, LabelWidth);
			if (spUseTransformAsTarget.boolValue)
			{
				DrawSerializedProperty(spTargetRectTransform, LabelWidth);
			}

			SpringsEditorUtility.Space();
		}

		protected override void DrawInfoArea()
		{
			EditorGUILayout.Space(2);

			if (spFollowRectTransform.objectReferenceValue == null)
			{
				EditorGUILayout.HelpBox("Follower is not assigned!", MessageType.Error);
			}
			if (spUseTransformAsTarget.boolValue && SpHasCustomTarget.boolValue)
			{
				EditorGUILayout.HelpBox("You are using a rect transform as a target and custom initial target at the same time. Disable one of them", MessageType.Warning);
			}
			if (spUseTransformAsTarget.boolValue && spTargetRectTransform.objectReferenceValue == null)
			{
				EditorGUILayout.HelpBox("Use Transform As Target is enabled but the reference is not assigned", MessageType.Warning);
			}
		}
	}
}
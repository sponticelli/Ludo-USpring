using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
    [CustomEditor(typeof(CanvasGroupSpringComponent))]
    [CanEditMultipleObjects]
    public class CanvasGroupSpringComponentEditor : SpringComponentCustomEditor
    {
        private SerializedProperty spAutoUpdatedCanvasGroup;
        private SerializedProperty spUpdateBlocksRaycasts;
        private SerializedProperty spUpdateInteractable;
        private SerializedProperty spBlocksRaycastsThreshold;
        private SerializedProperty spInteractableThreshold;

        private SpringFloatDrawer alphaSpringDrawer;

        protected override void RefreshSerializedProperties()
        {
            base.RefreshSerializedProperties();

            spAutoUpdatedCanvasGroup = serializedObject.FindProperty("autoUpdatedCanvasGroup");
            spUpdateBlocksRaycasts = serializedObject.FindProperty("updateBlocksRaycasts");
            spUpdateInteractable = serializedObject.FindProperty("updateInteractable");
            spBlocksRaycastsThreshold = serializedObject.FindProperty("blocksRaycastsThreshold");
            spInteractableThreshold = serializedObject.FindProperty("interactableThreshold");
        }

        protected override void CreateDrawers()
        {
            alphaSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("alphaSpring"), false, false);
        }

        protected override void DrawSprings()
        {
            // Draw presets dropdown
            USpringPresets.DrawPresetDropdown(preset => {
                serializedObject.Update();

                // Get the force and drag properties from the spring
                SerializedProperty forceProperty = alphaSpringDrawer.SpringEditorObject.SpCommonForce;
                SerializedProperty dragProperty = alphaSpringDrawer.SpringEditorObject.SpCommonDrag;
                SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

                // Apply the preset
                USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

                serializedObject.ApplyModifiedProperties();
            });

            // Draw the spring
            DrawSpring(alphaSpringDrawer);
        }

        /// <summary>
        /// Draws a preview of the canvas group alpha spring behavior
        /// </summary>
        protected override void DrawSpringPreview()
        {
            // Get the spring parameters
            float force = alphaSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
            float drag = alphaSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

            // Draw the preview
            USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "Alpha Spring Preview");

            // Draw additional information about thresholds
            if (spUpdateBlocksRaycasts.boolValue || spUpdateInteractable.boolValue)
            {
                EditorGUILayout.Space(8);
                USpringEditorStyles.DrawSubHeader("Boolean Property Thresholds");

                // Draw a simple visualization of the thresholds
                Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(30));
                EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f, 1f));

                // Draw alpha range
                EditorGUI.LabelField(new Rect(rect.x, rect.y, 50, 20), "Alpha: 0");
                EditorGUI.LabelField(new Rect(rect.x + rect.width - 50, rect.y, 50, 20), "1");

                // Draw thresholds
                if (spUpdateBlocksRaycasts.boolValue)
                {
                    float threshold = spBlocksRaycastsThreshold.floatValue;
                    float xPos = rect.x + threshold * rect.width;
                    EditorGUI.DrawRect(new Rect(xPos - 1, rect.y + 15, 2, 15), Color.red);
                    EditorGUI.LabelField(new Rect(xPos - 40, rect.y + 15, 80, 15), "Raycasts");
                }

                if (spUpdateInteractable.boolValue)
                {
                    float threshold = spInteractableThreshold.floatValue;
                    float xPos = rect.x + threshold * rect.width;
                    EditorGUI.DrawRect(new Rect(xPos - 1, rect.y + 15, 2, 15), Color.green);
                    EditorGUI.LabelField(new Rect(xPos - 40, rect.y + 30, 80, 15), "Interactable");
                }
            }
        }

        /// <summary>
        /// Draws component-specific help information
        /// </summary>
        protected override void DrawComponentSpecificHelp()
        {
            USpringEditorStyles.DrawSubHeader("Canvas Group Spring Component");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("The Canvas Group Spring Component allows you to animate UI element opacity with spring physics.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("It can also automatically control the BlocksRaycasts and Interactable properties based on alpha thresholds.");

            USpringEditorStyles.EndSection();

            // Draw example usage
            USpringEditorStyles.DrawSubHeader("Example Usage");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("1. Attach this component to a GameObject with a CanvasGroup");
            EditorGUILayout.LabelField("2. Assign the CanvasGroup to the Auto Updated Canvas Group field");
            EditorGUILayout.LabelField("3. Configure the boolean property thresholds if needed");
            EditorGUILayout.LabelField("4. Call SetTarget() from code to animate alpha with spring physics");

            USpringEditorStyles.EndSection();

            // Draw threshold explanation
            USpringEditorStyles.DrawSubHeader("Boolean Property Thresholds");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("Thresholds determine when boolean properties change based on alpha value:");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("• When alpha rises above the threshold, the property becomes true");
            EditorGUILayout.LabelField("• When alpha falls below the threshold, the property becomes false");
            EditorGUILayout.LabelField("• This creates smooth transitions between interactive and non-interactive states");

            USpringEditorStyles.EndSection();
        }

        protected override void DrawCustomInitialValuesSection()
        {
            DrawInitialValuesBySpring("Initial Alpha", LabelWidth, alphaSpringDrawer);
        }

        protected override void DrawCustomInitialTarget()
        {
            DrawCustomTargetBySpring("Target Alpha", LabelWidth, alphaSpringDrawer);
        }

        protected override void DrawMainAreaUnfolded()
        {
            // Canvas Group reference
            DrawSerializedProperty(spAutoUpdatedCanvasGroup, LabelWidth);

            Space(1);

            // Boolean properties settings
            EditorGUILayout.LabelField("Boolean Properties", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;

            // Blocks Raycasts
            DrawSerializedProperty(spUpdateBlocksRaycasts, LabelWidth);
            if (spUpdateBlocksRaycasts.boolValue)
            {
                EditorGUI.indentLevel++;
                DrawSerializedProperty(spBlocksRaycastsThreshold, LabelWidth);
                EditorGUI.indentLevel--;
            }

            Space(1);

            // Interactable
            DrawSerializedProperty(spUpdateInteractable, LabelWidth);
            if (spUpdateInteractable.boolValue)
            {
                EditorGUI.indentLevel++;
                DrawSerializedProperty(spInteractableThreshold, LabelWidth);
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }

        protected override void DrawInfoArea()
        {
            EditorGUILayout.Space(2);

            if (spAutoUpdatedCanvasGroup.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Canvas Group is not assigned!", MessageType.Error);
            }

            // Add tooltip-like information about threshold values
            if (spUpdateBlocksRaycasts.boolValue || spUpdateInteractable.boolValue)
            {
                EditorGUILayout.HelpBox("Threshold values determine when boolean properties change based on alpha. Set between 0 and 1.", MessageType.Info);
            }

            // Flag value problems
            if (spBlocksRaycastsThreshold.floatValue < 0 || spBlocksRaycastsThreshold.floatValue > 1)
            {
                EditorGUILayout.HelpBox("Blocks Raycasts threshold should be between 0 and 1", MessageType.Warning);
            }

            if (spInteractableThreshold.floatValue < 0 || spInteractableThreshold.floatValue > 1)
            {
                EditorGUILayout.HelpBox("Interactable threshold should be between 0 and 1", MessageType.Warning);
            }
        }
    }
}
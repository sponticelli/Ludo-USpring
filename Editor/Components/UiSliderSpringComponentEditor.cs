using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
    [CustomEditor(typeof(UiSliderSpringComponent))]
    [CanEditMultipleObjects]
    public class UiSliderSpringComponentEditor : SpringComponentCustomEditor
    {
        private SerializedProperty spAutoUpdatedSliderImage;
        private SpringFloatDrawer fillAmountSpringDrawer;

        protected override void RefreshSerializedProperties()
        {
            base.RefreshSerializedProperties();

            spAutoUpdatedSliderImage = serializedObject.FindProperty("autoUpdatedSliderImage");
        }

        protected override void CreateDrawers()
        {
            fillAmountSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("fillAmountSpring"), false, false);
        }

        protected override void DrawCustomInitialValuesSection()
        {
            DrawInitialValuesBySpring("Initial Value", LabelWidth, fillAmountSpringDrawer);
        }

        protected override void DrawCustomInitialTarget()
        {
            DrawCustomTargetBySpring("Target", LabelWidth, fillAmountSpringDrawer);
        }

        protected override void DrawMainAreaUnfolded()
        {
            DrawSerializedProperty(spAutoUpdatedSliderImage, LabelWidth);
        }

        protected override void DrawSprings()
        {
            // Draw presets dropdown
            USpringPresets.DrawPresetDropdown(preset => {
                serializedObject.Update();

                // Get the force and drag properties from the spring
                SerializedProperty forceProperty = fillAmountSpringDrawer.SpringEditorObject.SpCommonForce;
                SerializedProperty dragProperty = fillAmountSpringDrawer.SpringEditorObject.SpCommonDrag;
                SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

                // Apply the preset
                USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

                serializedObject.ApplyModifiedProperties();
            });

            // Draw the spring
            DrawSpring(fillAmountSpringDrawer);
        }

        /// <summary>
        /// Draws a preview of the UI slider spring behavior
        /// </summary>
        protected override void DrawSpringPreview()
        {
            // Get the spring parameters
            float force = fillAmountSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
            float drag = fillAmountSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

            // Draw the preview
            USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "Slider Value Spring Preview");

            // Draw a visualization of slider value change
            EditorGUILayout.Space(8);
            USpringEditorStyles.DrawSubHeader("UI Slider Visualization");

            // Draw a simple slider visualization
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(150));

            // Draw background
            EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f, 1f));

            // Draw slider background
            float sliderHeight = 20f;
            float sliderWidth = rect.width * 0.8f;
            float sliderX = rect.x + (rect.width - sliderWidth) / 2;
            float sliderY = rect.y + 40;

            Rect sliderBgRect = new Rect(sliderX, sliderY, sliderWidth, sliderHeight);
            EditorGUI.DrawRect(sliderBgRect, new Color(0.3f, 0.3f, 0.3f, 1f));

            // Draw initial fill
            float initialValue = 0.3f; // Example value
            Rect initialFillRect = new Rect(sliderX, sliderY, sliderWidth * initialValue, sliderHeight);
            EditorGUI.DrawRect(initialFillRect, new Color(0.5f, 0.5f, 0.5f, 0.7f));

            // Draw target fill
            float targetValue = 0.7f; // Example value
            Rect targetFillRect = new Rect(sliderX, sliderY + 40, sliderWidth * targetValue, sliderHeight);
            EditorGUI.DrawRect(targetFillRect, new Color(0.4f, 0.8f, 1f, 0.7f));

            // Draw labels
            EditorGUI.LabelField(new Rect(sliderX - 60, sliderY, 60, sliderHeight), "Initial:");
            EditorGUI.LabelField(new Rect(sliderX + sliderWidth + 5, sliderY, 60, sliderHeight),
                initialValue.ToString("P0"));

            EditorGUI.LabelField(new Rect(sliderX - 60, sliderY + 40, 60, sliderHeight), "Target:");
            EditorGUI.LabelField(new Rect(sliderX + sliderWidth + 5, sliderY + 40, 60, sliderHeight),
                targetValue.ToString("P0"));

            // Draw animated slider
            float animatedValue = 0.5f; // Example value between initial and target
            float handleSize = 20f;

            // Draw slider track
            Rect animatedTrackRect = new Rect(sliderX, sliderY + 80, sliderWidth, sliderHeight);
            EditorGUI.DrawRect(animatedTrackRect, new Color(0.3f, 0.3f, 0.3f, 1f));

            // Draw slider fill
            Rect animatedFillRect = new Rect(sliderX, sliderY + 80, sliderWidth * animatedValue, sliderHeight);
            EditorGUI.DrawRect(animatedFillRect, new Color(0.4f, 0.8f, 0.4f, 0.8f));

            // Draw slider handle
            float handleX = sliderX + sliderWidth * animatedValue - handleSize / 2;
            float handleY = sliderY + 80 - handleSize / 4;
            Rect handleRect = new Rect(handleX, handleY, handleSize, handleSize + sliderHeight / 2);
            EditorGUI.DrawRect(handleRect, new Color(0.8f, 0.8f, 0.8f, 1f));

            // Draw animated label
            EditorGUI.LabelField(new Rect(sliderX - 80, sliderY + 80, 80, sliderHeight), "Animated:");
            EditorGUI.LabelField(new Rect(sliderX + sliderWidth + 5, sliderY + 80, 60, sliderHeight),
                animatedValue.ToString("P0"));
        }

        /// <summary>
        /// Draws component-specific help information
        /// </summary>
        protected override void DrawComponentSpecificHelp()
        {
            USpringEditorStyles.DrawSubHeader("UI Slider Spring Component");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("The UI Slider Spring Component allows you to animate slider values with spring physics.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("It works with Unity UI Slider components to create smooth, physically-based transitions.");

            USpringEditorStyles.EndSection();

            // Draw example usage
            USpringEditorStyles.DrawSubHeader("Example Usage");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("1. Attach this component to a GameObject with a UI Slider");
            EditorGUILayout.LabelField("2. Assign the Auto Updated Slider Image (usually the Fill Image of the slider)");
            EditorGUILayout.LabelField("3. Set your desired target value (0-1 range)");
            EditorGUILayout.LabelField("4. Call SetTarget() from code to animate to a new slider value");

            USpringEditorStyles.EndSection();

            // Draw tips
            USpringEditorStyles.DrawSubHeader("Tips");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("• Great for health bars, progress indicators, and loading screens");
            EditorGUILayout.LabelField("• Values are normalized (0-1 range) to match Unity's slider value range");
            EditorGUILayout.LabelField("• Can be combined with other spring components for coordinated UI animations");
            EditorGUILayout.LabelField("• Use lower force and higher drag for smoother, less bouncy transitions");

            USpringEditorStyles.EndSection();
        }

        protected override void DrawInfoArea()
        {
            EditorGUILayout.Space(2);

            if (spAutoUpdatedSliderImage.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("AutoUpdatedSliderImage is not assigned!", MessageType.Error);
            }
        }
    }
}
using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
    [CustomEditor(typeof(ShaderFloatSpringComponent))]
    [CanEditMultipleObjects]
    public class ShaderFloatSpringComponentEditor : SpringComponentCustomEditor
    {
        private SpringFloatDrawer shaderValueSpringDrawer;

        private SerializedProperty spShaderPropertyName;
        private SerializedProperty spTargetIsRenderer;
        private SerializedProperty spTargetRenderer;
        private SerializedProperty spTargetGraphic;
        private SerializedProperty spGetAutoUpdatedMaterialFromTarget;
        private SerializedProperty spAutoUpdatedMaterial;

        protected override void RefreshSerializedProperties()
        {
            base.RefreshSerializedProperties();

            spShaderPropertyName = serializedObject.FindProperty("shaderPropertyName");
            spTargetIsRenderer = serializedObject.FindProperty("targetIsRenderer");
            spTargetRenderer = serializedObject.FindProperty("targetRenderer");
            spTargetGraphic = serializedObject.FindProperty("targetGraphic");
            spGetAutoUpdatedMaterialFromTarget = serializedObject.FindProperty("getAutoUpdatedMaterialFromTarget");
            spAutoUpdatedMaterial = serializedObject.FindProperty("autoUpdatedMaterial");
        }

        protected override void CreateDrawers()
        {
            shaderValueSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("shaderValueSpring"), false, false);
        }

        protected override void DrawMainAreaUnfolded()
        {
            DrawSerializedProperty(spShaderPropertyName, LabelWidth);
            DrawSerializedProperty(spTargetIsRenderer, LabelWidth);

            if (spTargetIsRenderer.boolValue)
            {
                DrawSerializedProperty(spTargetRenderer, LabelWidth);
            }
            else
            {
                DrawSerializedProperty(spTargetGraphic, LabelWidth);
            }

            DrawSerializedProperty(spGetAutoUpdatedMaterialFromTarget, LabelWidth);

            if (!spGetAutoUpdatedMaterialFromTarget.boolValue)
            {
                DrawSerializedProperty(spAutoUpdatedMaterial, LabelWidth);
            }
        }

        protected override void DrawCustomInitialValuesSection()
        {
            DrawInitialValuesBySpring("Initial Values", LabelWidth, shaderValueSpringDrawer);
        }

        protected override void DrawCustomInitialTarget()
        {
            DrawCustomTargetBySpring("Target", LabelWidth, shaderValueSpringDrawer);
        }

        protected override void DrawSprings()
        {
            // Draw presets dropdown
            USpringPresets.DrawPresetDropdown(preset => {
                serializedObject.Update();

                // Get the force and drag properties from the spring
                SerializedProperty forceProperty = shaderValueSpringDrawer.SpringEditorObject.SpCommonForce;
                SerializedProperty dragProperty = shaderValueSpringDrawer.SpringEditorObject.SpCommonDrag;
                SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

                // Apply the preset
                USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);

                serializedObject.ApplyModifiedProperties();
            });

            // Draw the spring
            DrawSpring(shaderValueSpringDrawer);
        }

        /// <summary>
        /// Draws a preview of the shader float spring behavior
        /// </summary>
        protected override void DrawSpringPreview()
        {
            // Get the spring parameters
            float force = shaderValueSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
            float drag = shaderValueSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

            // Draw the preview
            USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "Shader Float Spring Preview");

            // Draw a visualization of shader parameter change
            EditorGUILayout.Space(8);
            USpringEditorStyles.DrawSubHeader("Shader Parameter Visualization");

            // Draw a simple material visualization
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(150));

            // Draw background
            EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1f));

            // Get property name
            string propertyName = spShaderPropertyName.stringValue;
            if (string.IsNullOrEmpty(propertyName))
            {
                propertyName = "Shader Property";
            }

            // Draw property name
            EditorGUI.LabelField(new Rect(rect.x + 10, rect.y + 10, rect.width - 20, 20),
                "Property: " + propertyName, EditorStyles.boldLabel);

            // Draw initial and target values
            float initialValue = 0.3f; // Example value
            float targetValue = 0.7f;  // Example value

            // Draw value bars
            float barHeight = 20f;
            float barWidth = rect.width - 100;
            float barY = rect.y + 50;

            // Initial value bar
            Rect initialBarRect = new Rect(rect.x + 80, barY, barWidth * initialValue, barHeight);
            EditorGUI.DrawRect(initialBarRect, new Color(0.5f, 0.5f, 0.5f, 0.7f));
            EditorGUI.LabelField(new Rect(rect.x + 10, barY, 70, barHeight), "Initial:");
            EditorGUI.LabelField(new Rect(initialBarRect.x + initialBarRect.width + 5, barY, 50, barHeight),
                initialValue.ToString("F2"));

            // Target value bar
            Rect targetBarRect = new Rect(rect.x + 80, barY + 40, barWidth * targetValue, barHeight);
            EditorGUI.DrawRect(targetBarRect, new Color(0.4f, 0.8f, 1f, 0.7f));
            EditorGUI.LabelField(new Rect(rect.x + 10, barY + 40, 70, barHeight), "Target:");
            EditorGUI.LabelField(new Rect(targetBarRect.x + targetBarRect.width + 5, barY + 40, 50, barHeight),
                targetValue.ToString("F2"));

            // Draw material preview
            float previewSize = 60f;
            float previewX = rect.x + (rect.width - previewSize) / 2;
            float previewY = rect.y + rect.height - previewSize - 20;

            // Draw initial material
            Rect initialPreviewRect = new Rect(previewX - 40, previewY, previewSize, previewSize);
            EditorGUI.DrawRect(initialPreviewRect, new Color(0.3f, 0.3f, 0.3f, 1f));
            EditorGUI.LabelField(new Rect(initialPreviewRect.x, initialPreviewRect.y - 20, previewSize, 20), "Initial");

            // Draw target material
            Rect targetPreviewRect = new Rect(previewX + 40, previewY, previewSize, previewSize);
            EditorGUI.DrawRect(targetPreviewRect, new Color(0.6f, 0.6f, 0.6f, 1f));
            EditorGUI.LabelField(new Rect(targetPreviewRect.x, targetPreviewRect.y - 20, previewSize, 20), "Target");

            // Draw arrow
            DrawShaderArrow(
                initialPreviewRect.x + initialPreviewRect.width / 2,
                initialPreviewRect.y + initialPreviewRect.height / 2,
                targetPreviewRect.x + targetPreviewRect.width / 2,
                targetPreviewRect.y + targetPreviewRect.height / 2,
                new Color(0.4f, 0.8f, 0.4f, 0.8f)
            );
        }

        /// <summary>
        /// Draws an arrow between two points
        /// </summary>
        private void DrawShaderArrow(float x1, float y1, float x2, float y2, Color color)
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
            USpringEditorStyles.DrawSubHeader("Shader Float Spring Component");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("The Shader Float Spring Component allows you to animate shader parameters with spring physics.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("It can be used to create smooth, physically-based animations for material properties.");

            USpringEditorStyles.EndSection();

            // Draw example usage
            USpringEditorStyles.DrawSubHeader("Example Usage");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("1. Attach this component to a GameObject with a Renderer or UI Graphic");
            EditorGUILayout.LabelField("2. Set the Shader Property Name to the name of the float property in your shader");
            EditorGUILayout.LabelField("3. Choose whether the target is a Renderer or UI Graphic");
            EditorGUILayout.LabelField("4. Assign the target Renderer/Graphic and material if needed");
            EditorGUILayout.LabelField("5. Call SetTarget() from code to animate to a new shader value");

            USpringEditorStyles.EndSection();

            // Draw common shader properties
            USpringEditorStyles.DrawSubHeader("Common Shader Properties");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("• _Metallic - Controls metallic appearance (0-1)");
            EditorGUILayout.LabelField("• _Glossiness/_Smoothness - Controls surface smoothness (0-1)");
            EditorGUILayout.LabelField("• _EmissionIntensity - Controls emission strength");
            EditorGUILayout.LabelField("• _Cutoff - Controls alpha cutoff threshold");
            EditorGUILayout.LabelField("• _BumpScale - Controls normal map intensity");

            USpringEditorStyles.EndSection();

            // Draw tips
            USpringEditorStyles.DrawSubHeader("Tips");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("• Great for creating dynamic material effects like glowing, pulsing, etc.");
            EditorGUILayout.LabelField("• Can be combined with other spring components for coordinated animations");
            EditorGUILayout.LabelField("• Make sure the shader property exists in your material's shader");
            EditorGUILayout.LabelField("• Property names are case-sensitive and must match exactly");

            USpringEditorStyles.EndSection();
        }

        protected override void DrawInfoArea()
        {
            EditorGUILayout.Space(2);

            if (string.IsNullOrEmpty(spShaderPropertyName.stringValue))
            {
                EditorGUILayout.HelpBox("The ShaderPropertyName isn't valid!", MessageType.Error);
            }

            if (spTargetIsRenderer.boolValue && spTargetRenderer.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("The TargetRenderer isn't assigned!", MessageType.Error);
            }

            if (!spTargetIsRenderer.boolValue && spTargetGraphic.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("The TargetGraphic isn't assigned!", MessageType.Error);
            }
        }
    }
}
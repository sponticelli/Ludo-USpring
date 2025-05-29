using UnityEditor;
using UnityEngine;
using USpring.Core;
using System.Collections.Generic;

namespace USpring.Enhanced
{
    /// <summary>
    /// Enhanced property drawer for spring types with visual feedback, presets, and real-time preview.
    /// </summary>
    [CustomPropertyDrawer(typeof(SpringFloat))]
    [CustomPropertyDrawer(typeof(SpringVector2))]
    [CustomPropertyDrawer(typeof(SpringVector3))]
    [CustomPropertyDrawer(typeof(SpringVector4))]
    [CustomPropertyDrawer(typeof(SpringColor))]
    public class EnhancedSpringPropertyDrawer : PropertyDrawer
    {
        private static readonly Dictionary<string, bool> _foldoutStates = new Dictionary<string, bool>();
        private static readonly Dictionary<string, int> _selectedPresets = new Dictionary<string, int>();
        private static readonly Dictionary<string, bool> _showPreview = new Dictionary<string, bool>();
        private static readonly Dictionary<string, float> _previewTime = new Dictionary<string, float>();

        private const float LineHeight = 18f;
        private const float Spacing = 2f;
        private const float PreviewHeight = 60f;
        private const float ButtonHeight = 20f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string propertyPath = property.propertyPath;

            EditorGUI.BeginProperty(position, label, property);

            // Get foldout state
            if (!_foldoutStates.ContainsKey(propertyPath))
                _foldoutStates[propertyPath] = false;

            // Header with spring type indicator
            Rect headerRect = new Rect(position.x, position.y, position.width, LineHeight);
            Color springColor = GetSpringTypeColor(property);

            // Draw colored background for spring type
            Rect colorRect = new Rect(headerRect.x, headerRect.y, 4f, headerRect.height);
            EditorGUI.DrawRect(colorRect, springColor);

            // Foldout with enhanced styling
            Rect foldoutRect = new Rect(headerRect.x + 6f, headerRect.y, headerRect.width - 6f, headerRect.height);
            _foldoutStates[propertyPath] = EditorGUI.Foldout(foldoutRect, _foldoutStates[propertyPath],
                GetEnhancedLabel(property, label), true, USpringEditorStyles.HeaderStyle);

            if (_foldoutStates[propertyPath])
            {
                float currentY = position.y + LineHeight + Spacing;

                // Quick preset selection
                DrawPresetSection(property, ref currentY, position.width);

                // Main spring properties
                DrawMainProperties(property, ref currentY, position.width);

                // Advanced properties (clamping, etc.)
                DrawAdvancedProperties(property, ref currentY, position.width);

                // Preview section
                DrawPreviewSection(property, propertyPath, ref currentY, position.width);

                // Quick actions
                DrawQuickActions(property, ref currentY, position.width);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            string propertyPath = property.propertyPath;

            if (!_foldoutStates.ContainsKey(propertyPath) || !_foldoutStates[propertyPath])
                return LineHeight;

            float height = LineHeight + Spacing; // Header
            height += ButtonHeight + Spacing; // Presets
            height += LineHeight * 4 + Spacing * 4; // Main properties (force, drag, target, current)
            height += LineHeight * 3 + Spacing * 3; // Advanced properties

            // Preview section
            if (_showPreview.ContainsKey(propertyPath) && _showPreview[propertyPath])
                height += PreviewHeight + Spacing;

            height += ButtonHeight + Spacing; // Quick actions

            return height;
        }

        private void DrawPresetSection(SerializedProperty property, ref float currentY, float width)
        {
            string propertyPath = property.propertyPath;

            Rect presetRect = new Rect(0, currentY, width, ButtonHeight);

            // Preset dropdown
            Rect dropdownRect = new Rect(presetRect.x, presetRect.y, width * 0.7f, presetRect.height);

            if (!_selectedPresets.ContainsKey(propertyPath))
                _selectedPresets[propertyPath] = 0;

            var presets = USpringPresets.GetAllPresets();
            string[] presetNames = new string[presets.Count + 1];
            presetNames[0] = "Select Preset...";
            for (int i = 0; i < presets.Count; i++)
                presetNames[i + 1] = presets[i].Name;

            int newSelection = EditorGUI.Popup(dropdownRect, _selectedPresets[propertyPath], presetNames);
            if (newSelection != _selectedPresets[propertyPath] && newSelection > 0)
            {
                _selectedPresets[propertyPath] = newSelection;
                ApplyPreset(property, presets[newSelection - 1]);
            }

            // Apply button
            Rect applyRect = new Rect(dropdownRect.xMax + 2f, presetRect.y, width * 0.28f, presetRect.height);
            if (GUI.Button(applyRect, "Apply", USpringEditorStyles.ButtonStyle))
            {
                if (_selectedPresets[propertyPath] > 0)
                    ApplyPreset(property, presets[_selectedPresets[propertyPath] - 1]);
            }

            currentY += ButtonHeight + Spacing;
        }

        private void DrawMainProperties(SerializedProperty property, ref float currentY, float width)
        {
            // Force
            DrawPropertyField(property, "commonForce", "Force", ref currentY, width,
                "Controls the stiffness of the spring. Higher values make it more responsive.");

            // Drag
            DrawPropertyField(property, "commonDrag", "Drag", ref currentY, width,
                "Controls the damping of the spring. Higher values reduce oscillation.");

            // Target (if available)
            var targetProp = GetTargetProperty(property);
            if (targetProp != null)
            {
                DrawPropertyField(targetProp, "Target", ref currentY, width,
                    "The target value the spring is trying to reach.");
            }

            // Current Value (if available)
            var currentProp = GetCurrentValueProperty(property);
            if (currentProp != null)
            {
                DrawPropertyField(currentProp, "Current", ref currentY, width,
                    "The current value of the spring.");
            }
        }

        private void DrawAdvancedProperties(SerializedProperty property, ref float currentY, float width)
        {
            // Clamping enabled
            DrawPropertyField(property, "clampingEnabled", "Enable Clamping", ref currentY, width,
                "Enables value clamping for this spring.");

            // Spring enabled
            DrawPropertyField(property, "springEnabled", "Spring Enabled", ref currentY, width,
                "Enables/disables spring simulation.");

            // Events enabled
            DrawPropertyField(property, "eventsEnabled", "Events Enabled", ref currentY, width,
                "Enables spring events for callbacks.");
        }

        private void DrawPreviewSection(SerializedProperty property, string propertyPath, ref float currentY, float width)
        {
            // Preview toggle
            Rect toggleRect = new Rect(0, currentY, width, LineHeight);

            if (!_showPreview.ContainsKey(propertyPath))
                _showPreview[propertyPath] = false;

            _showPreview[propertyPath] = EditorGUI.ToggleLeft(toggleRect, "Show Preview", _showPreview[propertyPath]);
            currentY += LineHeight + Spacing;

            if (_showPreview[propertyPath])
            {
                Rect previewRect = new Rect(0, currentY, width, PreviewHeight);
                DrawSpringPreview(property, previewRect, propertyPath);
                currentY += PreviewHeight + Spacing;
            }
        }

        private void DrawQuickActions(SerializedProperty property, ref float currentY, float width)
        {
            Rect buttonRect = new Rect(0, currentY, width / 3f - 1f, ButtonHeight);

            // Reset button
            if (GUI.Button(buttonRect, "Reset", USpringEditorStyles.ButtonStyle))
            {
                ResetSpring(property);
            }

            // Copy button
            buttonRect.x += width / 3f + 1f;
            if (GUI.Button(buttonRect, "Copy", USpringEditorStyles.ButtonStyle))
            {
                CopySpringSettings(property);
            }

            // Paste button
            buttonRect.x += width / 3f + 1f;
            if (GUI.Button(buttonRect, "Paste", USpringEditorStyles.ButtonStyle))
            {
                PasteSpringSettings(property);
            }

            currentY += ButtonHeight + Spacing;
        }

        private void DrawPropertyField(SerializedProperty parentProperty, string propertyName, string label,
            ref float currentY, float width, string tooltip = "")
        {
            var prop = parentProperty.FindPropertyRelative(propertyName);
            if (prop != null)
            {
                DrawPropertyField(prop, label, ref currentY, width, tooltip);
            }
        }

        private void DrawPropertyField(SerializedProperty property, string label, ref float currentY, float width, string tooltip = "")
        {
            Rect rect = new Rect(0, currentY, width, LineHeight);

            GUIContent labelContent = new GUIContent(label, tooltip);
            EditorGUI.PropertyField(rect, property, labelContent);

            currentY += LineHeight + Spacing;
        }

        private Color GetSpringTypeColor(SerializedProperty property)
        {
            string typeName = property.type;
            switch (typeName)
            {
                case "SpringFloat": return USpringEditorStyles.FloatSpringColor;
                case "SpringVector2": return USpringEditorStyles.Vector2SpringColor;
                case "SpringVector3": return USpringEditorStyles.Vector3SpringColor;
                case "SpringVector4": return USpringEditorStyles.Vector4SpringColor;
                case "SpringColor": return USpringEditorStyles.ColorSpringColor;
                case "SpringRotation": return USpringEditorStyles.RotationSpringColor;
                default: return USpringEditorStyles.PrimaryColor;
            }
        }

        private GUIContent GetEnhancedLabel(SerializedProperty property, GUIContent originalLabel)
        {
            string typeName = property.type;
            string icon = GetSpringTypeIcon(typeName);
            return new GUIContent($"{icon} {originalLabel.text}", originalLabel.tooltip);
        }

        private string GetSpringTypeIcon(string typeName)
        {
            switch (typeName)
            {
                case "SpringFloat": return "📊";
                case "SpringVector2": return "📐";
                case "SpringVector3": return "🎯";
                case "SpringVector4": return "🔷";
                case "SpringColor": return "🎨";
                case "SpringRotation": return "🔄";
                default: return "⚡";
            }
        }

        private SerializedProperty GetTargetProperty(SerializedProperty property)
        {
            // Try to find target property based on spring type
            var springValues = property.FindPropertyRelative("springValues");
            if (springValues != null && springValues.arraySize > 0)
            {
                return springValues.GetArrayElementAtIndex(0).FindPropertyRelative("target");
            }
            return null;
        }

        private SerializedProperty GetCurrentValueProperty(SerializedProperty property)
        {
            // Try to find current value property based on spring type
            var springValues = property.FindPropertyRelative("springValues");
            if (springValues != null && springValues.arraySize > 0)
            {
                return springValues.GetArrayElementAtIndex(0).FindPropertyRelative("currentValue");
            }
            return null;
        }

        private void ApplyPreset(SerializedProperty property, USpringPresets.SpringPreset preset)
        {
            property.serializedObject.Update();

            var forceProperty = property.FindPropertyRelative("commonForce");
            var dragProperty = property.FindPropertyRelative("commonDrag");

            if (forceProperty != null) forceProperty.floatValue = preset.Force;
            if (dragProperty != null) dragProperty.floatValue = preset.Drag;

            property.serializedObject.ApplyModifiedProperties();
        }

        private void DrawSpringPreview(SerializedProperty property, Rect rect, string propertyPath)
        {
            // Simple spring curve preview
            if (!_previewTime.ContainsKey(propertyPath))
                _previewTime[propertyPath] = 0f;

            _previewTime[propertyPath] += 0.016f; // Approximate 60 FPS

            // Draw background
            EditorGUI.DrawRect(rect, new Color(0.1f, 0.1f, 0.1f, 1f));

            // Draw spring curve
            var forceProperty = property.FindPropertyRelative("commonForce");
            var dragProperty = property.FindPropertyRelative("commonDrag");

            if (forceProperty != null && dragProperty != null)
            {
                float force = forceProperty.floatValue;
                float drag = dragProperty.floatValue;

                DrawSpringCurve(rect, force, drag, _previewTime[propertyPath]);
            }

            // Repaint to animate
            if (Application.isPlaying)
                EditorUtility.SetDirty(property.serializedObject.targetObject);
        }

        private void DrawSpringCurve(Rect rect, float force, float drag, float time)
        {
            // Simple spring simulation for preview
            int samples = 100;
            Vector3[] points = new Vector3[samples];

            for (int i = 0; i < samples; i++)
            {
                float t = (float)i / (samples - 1) * 2f; // 2 seconds
                float value = CalculateSpringValue(1f, 0f, 0f, force, drag, t);

                float x = rect.x + (t / 2f) * rect.width;
                float y = rect.y + rect.height * 0.5f - value * rect.height * 0.4f;

                points[i] = new Vector3(x, y, 0);
            }

            // Draw the curve
            Handles.color = GetSpringTypeColor(null);
            Handles.DrawAAPolyLine(2f, points);

            // Draw center line
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector3(rect.x, rect.y + rect.height * 0.5f, 0),
                           new Vector3(rect.xMax, rect.y + rect.height * 0.5f, 0));
        }

        private float CalculateSpringValue(float target, float current, float velocity, float force, float drag, float deltaTime)
        {
            // Simple spring physics calculation for preview
            float springForce = (target - current) * force;
            float dampingForce = velocity * drag;
            float acceleration = springForce - dampingForce;

            velocity += acceleration * deltaTime;
            current += velocity * deltaTime;

            return current;
        }

        private void ResetSpring(SerializedProperty property)
        {
            property.serializedObject.Update();

            var forceProperty = property.FindPropertyRelative("commonForce");
            var dragProperty = property.FindPropertyRelative("commonDrag");

            if (forceProperty != null) forceProperty.floatValue = 20f;
            if (dragProperty != null) dragProperty.floatValue = 10f;

            property.serializedObject.ApplyModifiedProperties();
        }

        private void CopySpringSettings(SerializedProperty property)
        {
            // Implementation for copying spring settings to clipboard
            EditorGUIUtility.systemCopyBuffer = JsonUtility.ToJson(new SpringSettings
            {
                force = property.FindPropertyRelative("commonForce")?.floatValue ?? 0f,
                drag = property.FindPropertyRelative("commonDrag")?.floatValue ?? 0f
            });
        }

        private void PasteSpringSettings(SerializedProperty property)
        {
            // Implementation for pasting spring settings from clipboard
            try
            {
                var settings = JsonUtility.FromJson<SpringSettings>(EditorGUIUtility.systemCopyBuffer);
                property.serializedObject.Update();

                var forceProperty = property.FindPropertyRelative("commonForce");
                var dragProperty = property.FindPropertyRelative("commonDrag");

                if (forceProperty != null) forceProperty.floatValue = settings.force;
                if (dragProperty != null) dragProperty.floatValue = settings.drag;

                property.serializedObject.ApplyModifiedProperties();
            }
            catch
            {
                Debug.LogWarning("Could not paste spring settings from clipboard");
            }
        }

        [System.Serializable]
        private class SpringSettings
        {
            public float force;
            public float drag;
        }
    }
}

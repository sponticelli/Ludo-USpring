using UnityEditor;
using UnityEngine;
using USpring.UI;
using USpring.Components;

namespace USpring.Drawers
{
    [CustomEditor(typeof(SpringPulse))]
    public class SpringPulseEditor : UnityEditor.Editor
    {
        // SerializedProperties
        private SerializedProperty spSpringComponent;
        private SerializedProperty spAutoStart;
        private SerializedProperty spLoop;
        private SerializedProperty spBaseScale;
        private SerializedProperty spPulseScale;
        private SerializedProperty spPulseInterval;
        private SerializedProperty spRandomVariation;
        private SerializedProperty spSpringForce;
        private SerializedProperty spSpringDrag;
        private SerializedProperty spAddVelocity;
        private SerializedProperty spVelocityScale;

        // Tab state
        private int selectedTab = 0;
        private readonly string[] tabNames = { "General", "Preview", "Help" };

        private void OnEnable()
        {
            // Initialize properties
            spSpringComponent = serializedObject.FindProperty("springComponent");
            spAutoStart = serializedObject.FindProperty("autoStart");
            spLoop = serializedObject.FindProperty("loop");
            spBaseScale = serializedObject.FindProperty("baseScale");
            spPulseScale = serializedObject.FindProperty("pulseScale");
            spPulseInterval = serializedObject.FindProperty("pulseInterval");
            spRandomVariation = serializedObject.FindProperty("randomVariation");
            spSpringForce = serializedObject.FindProperty("springForce");
            spSpringDrag = serializedObject.FindProperty("springDrag");
            spAddVelocity = serializedObject.FindProperty("addVelocity");
            spVelocityScale = serializedObject.FindProperty("velocityScale");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw tabs
            selectedTab = GUILayout.Toolbar(selectedTab, tabNames, EditorStyles.toolbarButton);

            EditorGUILayout.Space(5);

            // Draw content based on selected tab
            switch (selectedTab)
            {
                case 0: // General
                    DrawGeneralTab();
                    break;
                case 1: // Preview
                    DrawPreviewTab();
                    break;
                case 2: // Help
                    DrawHelpTab();
                    break;
            }

            // Apply modified properties
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGeneralTab()
        {
            // Draw components and references section
            DrawComponentsSection();

            // Draw pulse settings
            DrawPulseSettings();

            // Draw spring settings
            DrawSpringSettings();

            // Draw preview and test controls
            if (Application.isPlaying)
            {
                DrawTestControls();
            }
        }

        private void DrawPreviewTab()
        {
            // Draw spring preview
            DrawSpringPreview();

            // Draw pulse animation preview
            DrawPulseAnimationPreview();

            // Draw test controls in play mode
            if (Application.isPlaying)
            {
                DrawTestControls();
            }
        }

        private void DrawHelpTab()
        {
            DrawComponentHelp();
        }

        private void DrawComponentsSection()
        {
            USpringEditorStyles.DrawSubHeader("Components");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.PropertyField(spSpringComponent);

            // Show warning if spring component is missing
            if (spSpringComponent.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox(
                    "No TransformSpringComponent assigned. One will be automatically created at runtime.",
                    MessageType.Info);
            }

            USpringEditorStyles.EndSection();
        }

        private void DrawPulseSettings()
        {
            USpringEditorStyles.DrawSubHeader("Pulse Settings");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.PropertyField(spAutoStart);
            EditorGUILayout.PropertyField(spLoop);

            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(spBaseScale);
            EditorGUILayout.PropertyField(spPulseScale);

            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(spPulseInterval);
            EditorGUILayout.PropertyField(spRandomVariation);

            USpringEditorStyles.EndSection();
        }

        private void DrawSpringSettings()
        {
            USpringEditorStyles.DrawSubHeader("Spring Properties");
            USpringEditorStyles.BeginSection();

            // Draw presets dropdown
            USpringPresets.DrawPresetDropdown(preset => {
                serializedObject.Update();

                // Apply the preset to spring properties
                spSpringForce.floatValue = preset.Force;
                spSpringDrag.floatValue = preset.Drag;

                serializedObject.ApplyModifiedProperties();
            });

            EditorGUILayout.PropertyField(spSpringForce);
            EditorGUILayout.PropertyField(spSpringDrag);

            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(spAddVelocity);

            if (spAddVelocity.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(spVelocityScale);
                EditorGUI.indentLevel--;
            }

            USpringEditorStyles.EndSection();
        }

        private void DrawTestControls()
        {
            USpringEditorStyles.DrawSubHeader("Test Controls");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Trigger Single Pulse", GUILayout.Height(30)))
            {
                SpringPulse pulseComponent = (SpringPulse)target;
                pulseComponent.TriggerPulse();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Start Continuous", GUILayout.Height(24)))
            {
                SpringPulse pulseComponent = (SpringPulse)target;
                pulseComponent.StartPulsing();
            }

            if (GUILayout.Button("Stop", GUILayout.Height(24)))
            {
                SpringPulse pulseComponent = (SpringPulse)target;
                pulseComponent.StopPulsing();
            }

            EditorGUILayout.EndHorizontal();

            USpringEditorStyles.EndSection();
        }

        private void DrawSpringPreview()
        {
            USpringEditorStyles.DrawSubHeader("Spring Preview");

            // Get the spring parameters
            float force = spSpringForce.floatValue;
            float drag = spSpringDrag.floatValue;

            // Draw the spring preview
            USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.Vector3SpringColor, "Scale Spring");
        }

        private void DrawPulseAnimationPreview()
        {
            USpringEditorStyles.DrawSubHeader("Pulse Animation Preview");

            // Draw a simple pulse visualization
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(200));

            // Draw background
            EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1f));

            // Draw pulse animation
            float centerX = rect.x + rect.width / 2;
            float centerY = rect.y + rect.height / 2;

            // Get pulse parameters
            Vector3 baseScale = spBaseScale.vector3Value;
            Vector3 pulseScale = spPulseScale.vector3Value;
            float interval = spPulseInterval.floatValue;

            // Calculate animation frames
            int frameCount = 60;
            float animationDuration = interval * 1.5f; // Show one full pulse plus a bit more

            // Draw timeline
            float timelineY = rect.y + rect.height - 20;
            float timelineWidth = rect.width - 40;
            float timelineX = rect.x + 20;

            Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            Handles.DrawLine(
                new Vector3(timelineX, timelineY, 0),
                new Vector3(timelineX + timelineWidth, timelineY, 0)
            );

            // Draw interval marker
            float intervalX = timelineX + (timelineWidth * interval / animationDuration);
            Handles.color = new Color(1f, 0.5f, 0.2f, 0.8f);
            Handles.DrawLine(
                new Vector3(intervalX, timelineY - 5, 0),
                new Vector3(intervalX, timelineY + 5, 0)
            );
            EditorGUI.LabelField(
                new Rect(intervalX - 20, timelineY + 5, 40, 20),
                "Pulse"
            );

            // Draw animation frames
            for (int i = 0; i < frameCount; i++)
            {
                float t = (float)i / (frameCount - 1) * animationDuration;
                float normalizedTime = t / animationDuration;

                // Calculate scale based on time
                Vector3 currentScale;
                Color objectColor;

                if (t < interval)
                {
                    // Before pulse
                    currentScale = baseScale;
                    objectColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);
                }
                else
                {
                    // After pulse, spring animation
                    float springT = (t - interval) / (animationDuration - interval);
                    float springDrag = spSpringDrag.floatValue;
                    float springForce = spSpringForce.floatValue;
                    float springValue = Mathf.Exp(-springDrag * springT) * Mathf.Cos(Mathf.Sqrt(springForce) * springT);

                    // Interpolate between base and pulse scale based on spring value
                    currentScale = Vector3.Lerp(
                        pulseScale,
                        baseScale,
                        1f - (springValue + 1f) / 2f
                    );

                    objectColor = new Color(0.4f, 0.8f, 1f, 0.7f);
                }

                // Only draw a few key frames to avoid clutter
                if (i % 10 == 0 || i == frameCount - 1 || t >= interval && t <= interval + 0.1f)
                {
                    // Draw object at this frame
                    float frameX = timelineX + normalizedTime * timelineWidth;
                    float frameY = timelineY - 10;

                    // Draw timeline marker
                    Handles.color = new Color(0.7f, 0.7f, 0.7f, 0.5f);
                    Handles.DrawLine(
                        new Vector3(frameX, timelineY - 3, 0),
                        new Vector3(frameX, timelineY + 3, 0)
                    );

                    // Draw object
                    float objectSize = 30f;
                    float scaleFactor = (currentScale.x + currentScale.y + currentScale.z) / 3f;
                    float size = objectSize * scaleFactor;

                    Rect objectRect = new Rect(
                        centerX - size / 2,
                        centerY - size / 2,
                        size,
                        size
                    );

                    // Only draw the current frame object
                    if (i == Mathf.FloorToInt(normalizedTime * frameCount))
                    {
                        EditorGUI.DrawRect(objectRect, objectColor);

                        // Draw outline
                        Handles.color = new Color(objectColor.r, objectColor.g, objectColor.b, objectColor.a + 0.2f);
                        Vector3[] points = new Vector3[5];
                        points[0] = new Vector3(objectRect.x, objectRect.y, 0);
                        points[1] = new Vector3(objectRect.x + objectRect.width, objectRect.y, 0);
                        points[2] = new Vector3(objectRect.x + objectRect.width, objectRect.y + objectRect.height, 0);
                        points[3] = new Vector3(objectRect.x, objectRect.y + objectRect.height, 0);
                        points[4] = new Vector3(objectRect.x, objectRect.y, 0);
                        Handles.DrawPolyLine(points);

                        // Draw time label
                        EditorGUI.LabelField(
                            new Rect(centerX - 20, centerY - size / 2 - 20, 40, 20),
                            $"t = {t:F2}s"
                        );
                    }
                }
            }

            // Draw labels
            EditorGUI.LabelField(
                new Rect(timelineX, timelineY - 40, 100, 20),
                "Base Scale"
            );

            EditorGUI.LabelField(
                new Rect(timelineX + timelineWidth - 100, timelineY - 40, 100, 20),
                "Time"
            );

            // Draw explanation
            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox(
                "This preview shows how the object will pulse over time. " +
                "The object starts at base scale, then at the pulse interval, " +
                "it springs to the pulse scale and back.",
                MessageType.Info
            );
        }

        private void DrawComponentHelp()
        {
            USpringEditorStyles.DrawSubHeader("Spring Pulse Component");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("The Spring Pulse Component creates pulsing animations using spring physics.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("It's great for creating attention-grabbing effects, heartbeats, breathing animations, and more.");

            USpringEditorStyles.EndSection();

            // Draw example usage
            USpringEditorStyles.DrawSubHeader("Example Usage");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("1. Attach this component to any GameObject");
            EditorGUILayout.LabelField("2. Set the Base Scale (normal size) and Pulse Scale (expanded size)");
            EditorGUILayout.LabelField("3. Adjust the Pulse Interval to control timing");
            EditorGUILayout.LabelField("4. Enable Auto Start if you want pulsing to begin automatically");
            EditorGUILayout.LabelField("5. Enable Loop for continuous pulsing");
            EditorGUILayout.LabelField("6. Call TriggerPulse() from code to trigger a single pulse");

            USpringEditorStyles.EndSection();

            // Draw spring settings explanation
            USpringEditorStyles.DrawSubHeader("Spring Settings");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("• Spring Force: Controls the stiffness of the spring (higher = faster oscillation)");
            EditorGUILayout.LabelField("• Spring Drag: Controls the damping of the spring (higher = less bouncy)");
            EditorGUILayout.LabelField("• Add Velocity: When enabled, adds initial velocity for more dynamic pulses");
            EditorGUILayout.LabelField("• Velocity Scale: Controls the amount of initial velocity added");

            USpringEditorStyles.EndSection();

            // Draw tips
            USpringEditorStyles.DrawSubHeader("Tips");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("• For subtle breathing effects, use small differences between base and pulse scale");
            EditorGUILayout.LabelField("• For attention-grabbing effects, use higher spring force and lower drag");
            EditorGUILayout.LabelField("• Add Random Variation to make continuous pulses feel more natural");
            EditorGUILayout.LabelField("• Combine with other animations for more complex effects");
            EditorGUILayout.LabelField("• Great for UI elements, pickups, and highlighting important objects");

            USpringEditorStyles.EndSection();
        }
    }
}
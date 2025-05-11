using UnityEditor;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;
using UnityEngine;

namespace USpring.Components
{
    [CustomEditor(typeof(ParticleSystemSpringComponent))]
    [CanEditMultipleObjects]
    public class ParticleSystemSpringComponentEditor : SpringComponentCustomEditor
    {
        private SerializedProperty spAutoUpdatedParticleSystem;

        // Animation toggles
        private SerializedProperty spAnimateEmissionRate;
        private SerializedProperty spAnimateStartLifetime;
        private SerializedProperty spAnimateStartSpeed;
        private SerializedProperty spAnimateStartSize;
        private SerializedProperty spAnimateStartColor;
        private SerializedProperty spAnimateSimulationSpeed;

        // Spring drawers
        private SpringFloatDrawer emissionRateSpringDrawer;
        private SpringFloatDrawer startLifetimeSpringDrawer;
        private SpringFloatDrawer startSpeedSpringDrawer;
        private SpringFloatDrawer startSizeSpringDrawer;
        private SpringColorDrawer startColorSpringDrawer;
        private SpringFloatDrawer simulationSpeedSpringDrawer;

        // Foldout states for property sections
        private bool emissionRateFoldout = true;
        private bool startLifetimeFoldout = true;
        private bool startSpeedFoldout = true;
        private bool startSizeFoldout = true;
        private bool startColorFoldout = true;
        private bool simulationSpeedFoldout = true;

        protected override void RefreshSerializedProperties()
        {
            base.RefreshSerializedProperties();

            spAutoUpdatedParticleSystem = serializedObject.FindProperty("autoUpdatedParticleSystem");

            spAnimateEmissionRate = serializedObject.FindProperty("animateEmissionRate");
            spAnimateStartLifetime = serializedObject.FindProperty("animateStartLifetime");
            spAnimateStartSpeed = serializedObject.FindProperty("animateStartSpeed");
            spAnimateStartSize = serializedObject.FindProperty("animateStartSize");
            spAnimateStartColor = serializedObject.FindProperty("animateStartColor");
            spAnimateSimulationSpeed = serializedObject.FindProperty("animateSimulationSpeed");
        }

        protected override void CreateDrawers()
        {
            emissionRateSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("emissionRateSpring"), false, false);
            startLifetimeSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("startLifetimeSpring"), false, false);
            startSpeedSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("startSpeedSpring"), false, false);
            startSizeSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("startSizeSpring"), false, false);
            startColorSpringDrawer = new SpringColorDrawer(serializedObject.FindProperty("startColorSpring"), false, false);
            simulationSpeedSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("simulationSpeedSpring"), false, false);
        }

        protected override void DrawSprings()
        {
            // Draw presets dropdown
            USpringPresets.DrawPresetDropdown(preset => {
                serializedObject.Update();

                // Apply presets to all enabled springs
                SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

                if (spAnimateEmissionRate.boolValue)
                {
                    SerializedProperty forceProperty = emissionRateSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty dragProperty = emissionRateSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);
                    analyticalSolutionProperty = null; // Only apply to the first spring
                }

                if (spAnimateStartLifetime.boolValue)
                {
                    SerializedProperty forceProperty = startLifetimeSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty dragProperty = startLifetimeSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);
                    analyticalSolutionProperty = null;
                }

                if (spAnimateStartSpeed.boolValue)
                {
                    SerializedProperty forceProperty = startSpeedSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty dragProperty = startSpeedSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);
                    analyticalSolutionProperty = null;
                }

                if (spAnimateStartSize.boolValue)
                {
                    SerializedProperty forceProperty = startSizeSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty dragProperty = startSizeSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);
                    analyticalSolutionProperty = null;
                }

                if (spAnimateStartColor.boolValue)
                {
                    SerializedProperty forceProperty = startColorSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty dragProperty = startColorSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);
                    analyticalSolutionProperty = null;
                }

                if (spAnimateSimulationSpeed.boolValue)
                {
                    SerializedProperty forceProperty = simulationSpeedSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty dragProperty = simulationSpeedSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, forceProperty, dragProperty, analyticalSolutionProperty);
                }

                serializedObject.ApplyModifiedProperties();
            });

            // Draw each spring only if its animation toggle is enabled
            if (spAnimateEmissionRate.boolValue)
            {
                emissionRateFoldout = DrawSpringSection("Emission Rate Spring", emissionRateFoldout, emissionRateSpringDrawer);
            }

            if (spAnimateStartLifetime.boolValue)
            {
                startLifetimeFoldout = DrawSpringSection("Start Lifetime Spring", startLifetimeFoldout, startLifetimeSpringDrawer);
            }

            if (spAnimateStartSpeed.boolValue)
            {
                startSpeedFoldout = DrawSpringSection("Start Speed Spring", startSpeedFoldout, startSpeedSpringDrawer);
            }

            if (spAnimateStartSize.boolValue)
            {
                startSizeFoldout = DrawSpringSection("Start Size Spring", startSizeFoldout, startSizeSpringDrawer);
            }

            if (spAnimateStartColor.boolValue)
            {
                startColorFoldout = DrawSpringSection("Start Color Spring", startColorFoldout, startColorSpringDrawer);
            }

            if (spAnimateSimulationSpeed.boolValue)
            {
                simulationSpeedFoldout = DrawSpringSection("Simulation Speed Spring", simulationSpeedFoldout, simulationSpeedSpringDrawer);
            }
        }

        private bool DrawSpringSection(string label, bool foldout, SpringDrawer springDrawer)
        {
            bool newFoldout = DrawRectangleArea(label, foldout);

            if (newFoldout)
            {
                Rect propertyRect = EditorGUILayout.GetControlRect(false, springDrawer.GetPropertyHeight());
                springDrawer.OnGUI(propertyRect);
            }

            return newFoldout;
        }

        /// <summary>
        /// Draws a preview of the particle system spring behavior
        /// </summary>
        protected override void DrawSpringPreview()
        {
            // Draw a visualization of particle system
            EditorGUILayout.Space(8);
            USpringEditorStyles.DrawSubHeader("Particle System Visualization");

            // Draw a simple particle system visualization
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(200));

            // Draw background
            EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1f));

            // Draw emitter
            float emitterSize = 20f;
            float centerX = rect.x + rect.width / 2;
            float emitterY = rect.y + rect.height * 0.7f;

            Rect emitterRect = new Rect(centerX - emitterSize/2, emitterY - emitterSize/2, emitterSize, emitterSize);
            EditorGUI.DrawRect(emitterRect, new Color(0.4f, 0.4f, 0.4f, 1f));

            // Draw particles
            DrawParticles(rect, centerX, emitterY);

            // Draw spring previews for each enabled property
            EditorGUILayout.Space(8);

            // Draw individual spring previews
            if (spAnimateEmissionRate.boolValue)
            {
                float force = emissionRateSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float drag = emissionRateSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;
                USpringEditorStyles.DrawSubHeader("Emission Rate Spring Preview");
                USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "Emission Rate");
            }

            if (spAnimateStartLifetime.boolValue)
            {
                float force = startLifetimeSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float drag = startLifetimeSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;
                USpringEditorStyles.DrawSubHeader("Start Lifetime Spring Preview");
                USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "Lifetime");
            }

            if (spAnimateStartSpeed.boolValue)
            {
                float force = startSpeedSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float drag = startSpeedSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;
                USpringEditorStyles.DrawSubHeader("Start Speed Spring Preview");
                USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "Speed");
            }

            if (spAnimateStartSize.boolValue)
            {
                float force = startSizeSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float drag = startSizeSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;
                USpringEditorStyles.DrawSubHeader("Start Size Spring Preview");
                USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "Size");
            }

            if (spAnimateStartColor.boolValue)
            {
                float force = startColorSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float drag = startColorSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;
                USpringEditorStyles.DrawSubHeader("Start Color Spring Preview");
                USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.ColorSpringColor, "Color");
            }

            if (spAnimateSimulationSpeed.boolValue)
            {
                float force = simulationSpeedSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float drag = simulationSpeedSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;
                USpringEditorStyles.DrawSubHeader("Simulation Speed Spring Preview");
                USpringPreviewUtility.DrawSpringPreview(force, drag, USpringEditorStyles.FloatSpringColor, "Simulation Speed");
            }
        }

        /// <summary>
        /// Draws particles for the visualization
        /// </summary>
        private void DrawParticles(Rect rect, float emitterX, float emitterY)
        {
            // Draw some example particles
            int particleCount = 30;
            System.Random random = new System.Random(123); // Fixed seed for consistent visualization

            for (int i = 0; i < particleCount; i++)
            {
                // Calculate particle properties
                float normalizedLifetime = (float)i / particleCount;
                float distance = normalizedLifetime * rect.height * 0.6f;
                float angle = (float)(random.NextDouble() * Mathf.PI * 0.8f - Mathf.PI * 0.4f);

                float x = emitterX + Mathf.Sin(angle) * distance;
                float y = emitterY - distance * 0.8f;

                // Size decreases with lifetime
                float size = Mathf.Lerp(10f, 3f, normalizedLifetime);

                // Alpha decreases with lifetime
                float alpha = Mathf.Lerp(0.8f, 0.0f, normalizedLifetime);

                // Color changes with lifetime
                Color color = Color.Lerp(Color.yellow, Color.red, normalizedLifetime);
                color.a = alpha;

                // Draw the particle
                Rect particleRect = new Rect(x - size/2, y - size/2, size, size);
                EditorGUI.DrawRect(particleRect, color);
            }
        }

        /// <summary>
        /// Draws component-specific help information
        /// </summary>
        protected override void DrawComponentSpecificHelp()
        {
            USpringEditorStyles.DrawSubHeader("Particle System Spring Component");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("The Particle System Spring Component allows you to animate particle system properties with spring physics.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("It works with Unity's Particle System component to create dynamic, physically-based particle effects.");

            USpringEditorStyles.EndSection();

            // Draw example usage
            USpringEditorStyles.DrawSubHeader("Example Usage");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("1. Attach this component to a GameObject with a Particle System");
            EditorGUILayout.LabelField("2. Assign the Auto Updated Particle System (usually this GameObject's Particle System)");
            EditorGUILayout.LabelField("3. Enable the properties you want to animate with springs");
            EditorGUILayout.LabelField("4. Set your desired target values for each property");
            EditorGUILayout.LabelField("5. Call SetTarget() from code to animate to new values");

            USpringEditorStyles.EndSection();

            // Draw property explanations
            USpringEditorStyles.DrawSubHeader("Property Explanations");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("• Emission Rate: Controls how many particles are emitted per second");
            EditorGUILayout.LabelField("• Start Lifetime: Controls how long particles exist before disappearing");
            EditorGUILayout.LabelField("• Start Speed: Controls the initial velocity of particles when emitted");
            EditorGUILayout.LabelField("• Start Size: Controls the initial size of particles when emitted");
            EditorGUILayout.LabelField("• Start Color: Controls the initial color of particles when emitted");
            EditorGUILayout.LabelField("• Simulation Speed: Controls how fast the entire particle system runs");

            USpringEditorStyles.EndSection();

            // Draw tips
            USpringEditorStyles.DrawSubHeader("Tips");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("• Great for reactive particle effects that respond to game events");
            EditorGUILayout.LabelField("• Combine multiple property animations for complex effects");
            EditorGUILayout.LabelField("• Use with other spring components for coordinated animations");
            EditorGUILayout.LabelField("• For best results, use with burst or continuous emission modes");
            EditorGUILayout.LabelField("• Changes to which properties are animated require re-initialization");

            USpringEditorStyles.EndSection();
        }

        protected override void DrawCustomInitialValuesSection()
        {
            EditorGUILayout.LabelField("Initial Values", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            if (spAnimateEmissionRate.boolValue)
            {
                DrawInitialValuesBySpring("Emission Rate", LabelWidth, emissionRateSpringDrawer);
                Space(1);
            }

            if (spAnimateStartLifetime.boolValue)
            {
                DrawInitialValuesBySpring("Start Lifetime", LabelWidth, startLifetimeSpringDrawer);
                Space(1);
            }

            if (spAnimateStartSpeed.boolValue)
            {
                DrawInitialValuesBySpring("Start Speed", LabelWidth, startSpeedSpringDrawer);
                Space(1);
            }

            if (spAnimateStartSize.boolValue)
            {
                DrawInitialValuesBySpring("Start Size", LabelWidth, startSizeSpringDrawer);
                Space(1);
            }

            if (spAnimateStartColor.boolValue)
            {
                DrawInitialValuesBySpring("Start Color", LabelWidth, startColorSpringDrawer);
                Space(1);
            }

            if (spAnimateSimulationSpeed.boolValue)
            {
                DrawInitialValuesBySpring("Simulation Speed", LabelWidth, simulationSpeedSpringDrawer);
            }

            EditorGUI.indentLevel--;
        }

        protected override void DrawCustomInitialTarget()
        {
            EditorGUILayout.LabelField("Target Values", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            if (spAnimateEmissionRate.boolValue)
            {
                DrawCustomTargetBySpring("Emission Rate", LabelWidth, emissionRateSpringDrawer);
                Space(1);
            }

            if (spAnimateStartLifetime.boolValue)
            {
                DrawCustomTargetBySpring("Start Lifetime", LabelWidth, startLifetimeSpringDrawer);
                Space(1);
            }

            if (spAnimateStartSpeed.boolValue)
            {
                DrawCustomTargetBySpring("Start Speed", LabelWidth, startSpeedSpringDrawer);
                Space(1);
            }

            if (spAnimateStartSize.boolValue)
            {
                DrawCustomTargetBySpring("Start Size", LabelWidth, startSizeSpringDrawer);
                Space(1);
            }

            if (spAnimateStartColor.boolValue)
            {
                DrawCustomTargetBySpring("Start Color", LabelWidth, startColorSpringDrawer);
                Space(1);
            }

            if (spAnimateSimulationSpeed.boolValue)
            {
                DrawCustomTargetBySpring("Simulation Speed", LabelWidth, simulationSpeedSpringDrawer);
            }

            EditorGUI.indentLevel--;
        }

        protected override void DrawMainAreaUnfolded()
        {
            // Particle System reference
            DrawSerializedProperty(spAutoUpdatedParticleSystem, LabelWidth);

            Space(1);

            // Properties to animate section
            EditorGUILayout.LabelField("Properties to Animate", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;

            DrawAnimate(spAnimateEmissionRate, "Emission Rate", "Animates the particle emission rate using spring physics");
            DrawAnimate(spAnimateStartLifetime, "Start Lifetime", "Animates how long particles live using spring physics");
            DrawAnimate(spAnimateStartSpeed, "Start Speed", "Animates the initial velocity of particles using spring physics");
            DrawAnimate(spAnimateStartSize, "Start Size", "Animates the size of particles using spring physics");
            DrawAnimate(spAnimateStartColor, "Start Color", "Animates the color of particles using spring physics");
            DrawAnimate(spAnimateSimulationSpeed, "Simulation Speed", "Animates how fast the particle system runs using spring physics");

            EditorGUI.indentLevel--;
        }

        private void DrawAnimate(SerializedProperty property, string label, string tooltip)
        {
            EditorGUILayout.PropertyField(property, new GUIContent(label, tooltip));
        }

        protected override void DrawInfoArea()
        {
            EditorGUILayout.Space(2);

            if (spAutoUpdatedParticleSystem.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Particle System is not assigned!", MessageType.Error);
                return;
            }

            // Check if any property is selected for animation
            bool anyPropertyAnimated = spAnimateEmissionRate.boolValue ||
                                      spAnimateStartLifetime.boolValue ||
                                      spAnimateStartSpeed.boolValue ||
                                      spAnimateStartSize.boolValue ||
                                      spAnimateStartColor.boolValue ||
                                      spAnimateSimulationSpeed.boolValue;

            if (!anyPropertyAnimated)
            {
                EditorGUILayout.HelpBox("No properties selected for animation. Enable at least one property to animate.", MessageType.Warning);
            }

            // Check if emission module is enabled when emission rate is selected
            if (spAnimateEmissionRate.boolValue)
            {
                ParticleSystem ps = spAutoUpdatedParticleSystem.objectReferenceValue as ParticleSystem;
                if (ps != null && !ps.emission.enabled)
                {
                    EditorGUILayout.HelpBox("Emission Rate animation is enabled but the Emission module is disabled on the Particle System.", MessageType.Warning);
                }
            }

            // Message about when changes take effect
            EditorGUILayout.HelpBox("Toggling animation properties requires re-initialization to take effect.", MessageType.Info);
        }
    }
}
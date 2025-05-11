using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace USpring
{
    /// <summary>
    /// Provides preset configurations for USpring components.
    /// Allows quick application of common spring settings.
    /// </summary>
    public static class USpringPresets
    {
        /// <summary>
        /// Represents a spring preset configuration
        /// </summary>
        public class SpringPreset
        {
            public string Name;
            public string Description;
            public float Force;
            public float Drag;
            public bool UseAnalyticalSolution;
            
            public SpringPreset(string name, string description, float force, float drag, bool useAnalyticalSolution = false)
            {
                Name = name;
                Description = description;
                Force = force;
                Drag = drag;
                UseAnalyticalSolution = useAnalyticalSolution;
            }
        }
        
        // Common presets for different spring behaviors
        private static readonly List<SpringPreset> _presets = new List<SpringPreset>
        {
            new SpringPreset(
                "Bouncy", 
                "High force, low drag for a bouncy effect", 
                40f, 4f),
                
            new SpringPreset(
                "Smooth", 
                "Balanced force and drag for smooth motion", 
                20f, 10f),
                
            new SpringPreset(
                "Elastic", 
                "Medium force, low drag for elastic motion", 
                30f, 3f),
                
            new SpringPreset(
                "Tight", 
                "High force, high drag for quick, controlled motion", 
                50f, 20f),
                
            new SpringPreset(
                "Loose", 
                "Low force, low drag for slow, loose motion", 
                10f, 2f),
                
            new SpringPreset(
                "Critical", 
                "Critically damped for no overshoot", 
                20f, 9f, true),
                
            new SpringPreset(
                "Snappy", 
                "Very high force, medium drag for snappy motion", 
                80f, 12f),
                
            new SpringPreset(
                "Slow", 
                "Low force, high drag for slow, controlled motion", 
                5f, 10f)
        };
        
        /// <summary>
        /// Gets all available presets
        /// </summary>
        public static List<SpringPreset> GetAllPresets()
        {
            return _presets;
        }
        
        /// <summary>
        /// Draws preset buttons in the inspector
        /// </summary>
        /// <param name="onPresetSelected">Callback when a preset is selected</param>
        public static void DrawPresetButtons(System.Action<SpringPreset> onPresetSelected)
        {
            EditorGUILayout.Space(4);
            
            // Draw header
            USpringEditorStyles.DrawSubHeader("Presets");
            
            // Begin horizontal layout for preset buttons
            EditorGUILayout.BeginHorizontal();
            
            // Calculate how many presets to show per row
            int presetsPerRow = Mathf.Max(2, Mathf.FloorToInt(EditorGUIUtility.currentViewWidth / 100));
            int currentInRow = 0;
            
            // Draw preset buttons
            foreach (var preset in _presets)
            {
                // Create a new row if needed
                if (currentInRow >= presetsPerRow)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    currentInRow = 0;
                }
                
                // Draw preset button
                if (GUILayout.Button(preset.Name, USpringEditorStyles.PresetButtonStyle, GUILayout.Height(24)))
                {
                    onPresetSelected?.Invoke(preset);
                }
                
                currentInRow++;
            }
            
            // End horizontal layout
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(4);
        }
        
        /// <summary>
        /// Draws a dropdown menu for selecting presets
        /// </summary>
        /// <param name="onPresetSelected">Callback when a preset is selected</param>
        public static void DrawPresetDropdown(System.Action<SpringPreset> onPresetSelected)
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.PrefixLabel("Presets");
            
            if (EditorGUILayout.DropdownButton(new GUIContent("Select Preset"), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();
                
                foreach (var preset in _presets)
                {
                    menu.AddItem(new GUIContent(preset.Name + " - " + preset.Description), false, () => 
                    {
                        onPresetSelected?.Invoke(preset);
                    });
                }
                
                menu.ShowAsContext();
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        /// <summary>
        /// Applies a preset to a spring component
        /// </summary>
        /// <param name="serializedObject">The serialized object of the spring component</param>
        /// <param name="preset">The preset to apply</param>
        /// <param name="forceProperty">The force property</param>
        /// <param name="dragProperty">The drag property</param>
        /// <param name="analyticalSolutionProperty">The analytical solution property (optional)</param>
        public static void ApplyPreset(
            SerializedObject serializedObject, 
            SpringPreset preset, 
            SerializedProperty forceProperty, 
            SerializedProperty dragProperty, 
            SerializedProperty analyticalSolutionProperty = null)
        {
            serializedObject.Update();
            
            forceProperty.floatValue = preset.Force;
            dragProperty.floatValue = preset.Drag;
            
            if (analyticalSolutionProperty != null)
            {
                analyticalSolutionProperty.boolValue = preset.UseAnalyticalSolution;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        /// <summary>
        /// Applies a preset to a multi-component spring (Vector2/3/4)
        /// </summary>
        /// <param name="serializedObject">The serialized object of the spring component</param>
        /// <param name="preset">The preset to apply</param>
        /// <param name="forceProperties">Array of force properties</param>
        /// <param name="dragProperties">Array of drag properties</param>
        /// <param name="analyticalSolutionProperty">The analytical solution property (optional)</param>
        public static void ApplyMultiComponentPreset(
            SerializedObject serializedObject, 
            SpringPreset preset, 
            SerializedProperty[] forceProperties, 
            SerializedProperty[] dragProperties, 
            SerializedProperty analyticalSolutionProperty = null)
        {
            if (forceProperties.Length != dragProperties.Length)
            {
                Debug.LogError("Force and drag properties arrays must have the same length");
                return;
            }
            
            serializedObject.Update();
            
            for (int i = 0; i < forceProperties.Length; i++)
            {
                forceProperties[i].floatValue = preset.Force;
                dragProperties[i].floatValue = preset.Drag;
            }
            
            if (analyticalSolutionProperty != null)
            {
                analyticalSolutionProperty.boolValue = preset.UseAnalyticalSolution;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}

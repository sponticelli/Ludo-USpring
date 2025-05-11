using UnityEditor;
using UnityEngine;
using USpring.UI;

namespace USpring.Drawers
{
    [CustomEditor(typeof(SpringHealthBar))]
    public class SpringHealthBarEditor : UnityEditor.Editor
    {
        // SerializedProperties
        private SerializedProperty spMainBar;
        private SerializedProperty spDelayedBar;
        
        private SerializedProperty spDelayedBarForce;
        private SerializedProperty spDelayedBarDrag;
        private SerializedProperty spIncreasingMainBarForce;
        private SerializedProperty spIncreasingMainBarDrag;
        
        private SerializedProperty spUseGradient;
        private SerializedProperty spHealthGradient;
        private SerializedProperty spMainBarColor;
        private SerializedProperty spDecreasingMainBarColor;
        private SerializedProperty spDelayedBarDecreasingColor;
        private SerializedProperty spDelayedBarIncreasingColor;
        private SerializedProperty spUseSpringColorTransitions;
        
        private SerializedProperty spHealthRatio;
        private SerializedProperty spMaxHealth;
        private SerializedProperty spMinHealth;
        private SerializedProperty spAnimateHealthIncrease;
        private SerializedProperty spAnimateHealthDecrease;
        
        // UI styling properties
        private GUIStyle headerStyle;
        private readonly Color headerColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
        
        // Test variables
        private float testHealthValue = 100f;
        private float testDamageValue = 10f;
        private float testHealValue = 15f;
        
        private void OnEnable()
        {
            // Initialize properties
            spMainBar = serializedObject.FindProperty("mainBar");
            spDelayedBar = serializedObject.FindProperty("delayedBar");
            
            spDelayedBarForce = serializedObject.FindProperty("delayedBarForce");
            spDelayedBarDrag = serializedObject.FindProperty("delayedBarDrag");
            spIncreasingMainBarForce = serializedObject.FindProperty("increasingMainBarForce");
            spIncreasingMainBarDrag = serializedObject.FindProperty("increasingMainBarDrag");
            
            spUseGradient = serializedObject.FindProperty("useGradient");
            spHealthGradient = serializedObject.FindProperty("healthGradient");
            spMainBarColor = serializedObject.FindProperty("mainBarColor");
            spDecreasingMainBarColor = serializedObject.FindProperty("decreasingMainBarColor");
            spDelayedBarDecreasingColor = serializedObject.FindProperty("delayedBarDecreasingColor");
            spDelayedBarIncreasingColor = serializedObject.FindProperty("delayedBarIncreasingColor");
            spUseSpringColorTransitions = serializedObject.FindProperty("useSpringColorTransitions");
            
            spHealthRatio = serializedObject.FindProperty("healthRatio");
            spMaxHealth = serializedObject.FindProperty("maxHealth");
            spMinHealth = serializedObject.FindProperty("minHealth");
            spAnimateHealthIncrease = serializedObject.FindProperty("animateHealthIncrease");
            spAnimateHealthDecrease = serializedObject.FindProperty("animateHealthDecrease");
            
            // Initialize test values - we'll do this conditionally to avoid errors
            if (target != null)
            {
                SpringHealthBar healthBar = (SpringHealthBar)target;
                testHealthValue = healthBar.CurrentHealth;
                testDamageValue = healthBar.MaxHealth * 0.1f; // 10% of max health
                testHealValue = healthBar.MaxHealth * 0.15f;  // 15% of max health
            }
            
            // Note: We'll create GUI styles in OnInspectorGUI instead of here
            // to avoid potential null reference issues with EditorStyles
        }
        
        public override void OnInspectorGUI()
        {
            // Initialize GUI styles if needed
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel);
                headerStyle.normal.textColor = Color.white;
            }
            
            serializedObject.Update();
            
            // Draw components and references section
            DrawReferencesSection();
            
            // Draw health settings
            DrawHealthSettings();
            
            // Draw appearance settings
            DrawAppearanceSettings();
            
            // Draw spring settings
            DrawSpringSettings();
            
            // Draw preview and test controls
            if (Application.isPlaying)
            {
                DrawTestControls();
            }
            
            // Apply modified properties
            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawReferencesSection()
        {
            DrawSectionHeader("References");
            
            EditorGUILayout.PropertyField(spMainBar);
            EditorGUILayout.PropertyField(spDelayedBar);
            
            // Check for missing references
            bool isMissingReferences = spMainBar.objectReferenceValue == null || spDelayedBar.objectReferenceValue == null;
            if (isMissingReferences)
            {
                EditorGUILayout.HelpBox("Main bar and delayed bar references are required. Please assign them.", MessageType.Warning);
            }
            
            EditorGUILayout.Space();
        }
        
        private void DrawHealthSettings()
        {
            DrawSectionHeader("Health Settings");
            
            // Display and edit max/min health
            EditorGUILayout.PropertyField(spMaxHealth);
            EditorGUILayout.PropertyField(spMinHealth);
            
            // Health ratio property with a slider
            EditorGUILayout.Slider(spHealthRatio, 0f, 1f, new GUIContent("Health Ratio"));
            
            // Calculate and display current health value
            SpringHealthBar healthBar = (SpringHealthBar)target;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.FloatField("Current Health", healthBar.CurrentHealth);
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.Space();
            
            // Animation options
            EditorGUILayout.PropertyField(spAnimateHealthIncrease);
            EditorGUILayout.PropertyField(spAnimateHealthDecrease);
            
            EditorGUILayout.Space();
        }
        
        private void DrawAppearanceSettings()
        {
            DrawSectionHeader("Appearance");
            
            // Gradient mode toggle
            EditorGUILayout.PropertyField(spUseGradient);
            
            if (spUseGradient.boolValue)
            {
                // Gradient mode - show gradient field
                EditorGUILayout.PropertyField(spHealthGradient);
                
                // Show a preview of the gradient if available
                if (spHealthGradient.propertyType == SerializedPropertyType.Gradient)
                {
                    Rect rect = EditorGUILayout.GetControlRect(false, 20);
                    
                    // Create a simple gradient texture by sampling colors
                    Texture2D gradientTexture = new Texture2D(256, 1);
                    for (int i = 0; i < 256; i++)
                    {
                        float t = i / 255f;
                        // We need to display a representation of what different health values would look like
                        Color color = Color.Lerp(Color.red, Color.green, t);
                        gradientTexture.SetPixel(i, 0, color);
                    }
                    gradientTexture.Apply();
                    
                    EditorGUI.DrawTextureTransparent(rect, gradientTexture);
                    
                    // Clean up the texture to prevent memory leaks
                    if (Event.current.type == EventType.Repaint)
                    {
                        Object.DestroyImmediate(gradientTexture);
                    }
                }
            }
            else
            {
                // Fixed color mode - show main and decreasing color fields
                EditorGUILayout.PropertyField(spMainBarColor);
                EditorGUILayout.PropertyField(spDecreasingMainBarColor);
            }
            
            // Delayed bar colors - always visible
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Delayed Bar Colors", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(spDelayedBarDecreasingColor, new GUIContent("When Decreasing"));
            EditorGUILayout.PropertyField(spDelayedBarIncreasingColor, new GUIContent("When Increasing"));
            
            // Color transition spring
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(spUseSpringColorTransitions);
            
            EditorGUILayout.Space();
        }
        

        
        private void DrawSpringSettings()
        {
            DrawSectionHeader("Spring Settings");
            
            EditorGUILayout.LabelField("Delayed Bar Spring", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(spDelayedBarForce);
            EditorGUILayout.PropertyField(spDelayedBarDrag);
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Increasing Health Spring", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(spIncreasingMainBarForce);
            EditorGUILayout.PropertyField(spIncreasingMainBarDrag);
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();
        }
        
        private void DrawTestControls()
        {
            DrawSectionHeader("Test Controls");
            
            SpringHealthBar healthBar = (SpringHealthBar)target;
            
            EditorGUILayout.LabelField("Damage/Heal Amount:", EditorStyles.boldLabel);
            
            // Test damage amount editor
            testDamageValue = EditorGUILayout.FloatField("Damage Amount", testDamageValue);
            if (GUILayout.Button("Apply Damage", GUILayout.Height(24)))
            {
                healthBar.RemoveHealth(testDamageValue);
            }
            
            EditorGUILayout.Space();
            
            // Test heal amount editor
            testHealValue = EditorGUILayout.FloatField("Heal Amount", testHealValue);
            if (GUILayout.Button("Apply Healing", GUILayout.Height(24)))
            {
                healthBar.AddHealth(testHealValue);
            }
            
            EditorGUILayout.Space();
            
            // Set specific health value
            testHealthValue = EditorGUILayout.FloatField("Set Health To", testHealthValue);
            if (GUILayout.Button("Set Health", GUILayout.Height(24)))
            {
                healthBar.CurrentHealth = testHealthValue;
            }
            
            EditorGUILayout.Space();
            
            // Buttons for quick health changes
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Empty", GUILayout.Height(30)))
            {
                healthBar.SetHealthRatio(0f);
            }
            
            if (GUILayout.Button("Half", GUILayout.Height(30)))
            {
                healthBar.SetHealthRatio(0.5f);
            }
            
            if (GUILayout.Button("Full", GUILayout.Height(30)))
            {
                healthBar.SetHealthRatio(1f);
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawSectionHeader(string title)
        {
            EditorGUILayout.Space(5);
            
            // Draw header background
            Rect rect = EditorGUILayout.GetControlRect(false, 24);
            EditorGUI.DrawRect(rect, headerColor);
            
            // Draw header title
            rect.x += 10;
            rect.y += 4;
            EditorGUI.LabelField(rect, title, headerStyle);
            
            EditorGUILayout.Space(5);
        }
    }
}
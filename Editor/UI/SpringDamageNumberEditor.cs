using UnityEngine;
using UnityEditor;


namespace USpring.UI
{
    [CustomEditor(typeof(SpringUIDamageNumber))]
    public class SpringDamageNumberEditor : Editor
    {
        // SerializedProperties - References
        private SerializedProperty damageTextProp;
        private SerializedProperty springComponentProp;
        private SerializedProperty canvasGroupProp;

        // SerializedProperties - Appearance
        private SerializedProperty useColorGradientProp;
        private SerializedProperty damageColorGradientProp;
        private SerializedProperty defaultDamageColorProp;
        private SerializedProperty criticalDamageColorProp;
        private SerializedProperty showPlusSymbolProp;
        private SerializedProperty baseSizeProp;
        private SerializedProperty criticalSizeMultiplierProp;
        private SerializedProperty stackedSizeMultiplierProp;

        // SerializedProperties - Animation
        private SerializedProperty lifetimeProp;
        private SerializedProperty fadeDelayProp;
        private SerializedProperty initialVelocityMinProp;
        private SerializedProperty initialVelocityMaxProp;
        private SerializedProperty initialForceProp;
        private SerializedProperty initialDragProp;
        private SerializedProperty spreadAngleRangeProp;
        private SerializedProperty scaleCurveProp;
        private SerializedProperty applyGravityProp;
        private SerializedProperty gravityStrengthProp;
        private SerializedProperty wobbleOnCreateProp;
        private SerializedProperty wobbleStrengthProp;

        // SerializedProperties - Stacking
        private SerializedProperty enableStackingProp;
        private SerializedProperty stackRadiusProp;
        private SerializedProperty stackDurationProp;
        private SerializedProperty stackOffsetProp;

        // UI Styling
        private GUIStyle headerStyle;
        private readonly Color headerColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);

        // Foldout states
        private bool showReferencesSection = true;
        private bool showAppearanceSection = true;
        private bool showAnimationSection = true;
        private bool showStackingSection = true;
        private bool showPreviewSection = false;

        // Preview values
        private float previewDamage = 42.0f;
        private bool previewCritical = false;
        private bool previewStacked = false;
        private int previewStackCount = 3;

        private void OnEnable()
        {
            FindSerializedProperties();
        }

        private void FindSerializedProperties()
        {
            // References
            damageTextProp = serializedObject.FindProperty("damageText");
            springComponentProp = serializedObject.FindProperty("springComponent");
            canvasGroupProp = serializedObject.FindProperty("canvasGroup");

            // Appearance
            useColorGradientProp = serializedObject.FindProperty("useColorGradient");
            damageColorGradientProp = serializedObject.FindProperty("damageColorGradient");
            defaultDamageColorProp = serializedObject.FindProperty("defaultDamageColor");
            criticalDamageColorProp = serializedObject.FindProperty("criticalDamageColor");
            showPlusSymbolProp = serializedObject.FindProperty("showPlusSymbol");
            baseSizeProp = serializedObject.FindProperty("baseSize");
            criticalSizeMultiplierProp = serializedObject.FindProperty("criticalSizeMultiplier");
            stackedSizeMultiplierProp = serializedObject.FindProperty("stackedSizeMultiplier");

            // Animation
            lifetimeProp = serializedObject.FindProperty("lifetime");
            fadeDelayProp = serializedObject.FindProperty("fadeDelay");
            initialVelocityMinProp = serializedObject.FindProperty("initialVelocityMin");
            initialVelocityMaxProp = serializedObject.FindProperty("initialVelocityMax");
            initialForceProp = serializedObject.FindProperty("initialForce");
            initialDragProp = serializedObject.FindProperty("initialDrag");
            spreadAngleRangeProp = serializedObject.FindProperty("spreadAngleRange");
            scaleCurveProp = serializedObject.FindProperty("scaleCurve");
            applyGravityProp = serializedObject.FindProperty("applyGravity");
            gravityStrengthProp = serializedObject.FindProperty("gravityStrength");
            wobbleOnCreateProp = serializedObject.FindProperty("wobbleOnCreate");
            wobbleStrengthProp = serializedObject.FindProperty("wobbleStrength");

            // Stacking
            enableStackingProp = serializedObject.FindProperty("enableStacking");
            stackRadiusProp = serializedObject.FindProperty("stackRadius");
            stackDurationProp = serializedObject.FindProperty("stackDuration");
            stackOffsetProp = serializedObject.FindProperty("stackOffset");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Initialize styles
            InitializeStyles();

            // Draw sections
            DrawReferencesSection();
            DrawAppearanceSection();
            DrawAnimationSection();
            DrawStackingSection();

            // Preview section (only in play mode)
            if (Application.isPlaying)
            {
                DrawPreviewSection();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void InitializeStyles()
        {
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel);
                headerStyle.normal.textColor = Color.white;
            }
        }

        private void DrawReferencesSection()
        {
            showReferencesSection = DrawSectionHeader("References", showReferencesSection);

            if (showReferencesSection)
            {
                EditorGUILayout.PropertyField(damageTextProp);
                EditorGUILayout.PropertyField(springComponentProp);
                EditorGUILayout.PropertyField(canvasGroupProp);

                // Show warning if references are missing
                if (damageTextProp.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("Damage Text is missing! The component will try to find it at runtime.", MessageType.Warning);
                }

                if (springComponentProp.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("Spring Component is missing! The component will try to find or create it at runtime.", MessageType.Warning);
                }

                if (canvasGroupProp.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("Canvas Group is missing! The component will try to find or create it at runtime.", MessageType.Warning);
                }
            }

            EditorGUILayout.Space(5);
        }

        private void DrawAppearanceSection()
        {
            showAppearanceSection = DrawSectionHeader("Appearance", showAppearanceSection);

            if (showAppearanceSection)
            {
                EditorGUILayout.PropertyField(showPlusSymbolProp);
                EditorGUILayout.PropertyField(baseSizeProp);
                EditorGUILayout.PropertyField(criticalSizeMultiplierProp);
                EditorGUILayout.PropertyField(stackedSizeMultiplierProp);

                EditorGUILayout.Space(5);

                EditorGUILayout.PropertyField(useColorGradientProp);

                if (useColorGradientProp.boolValue)
                {
                    EditorGUILayout.PropertyField(damageColorGradientProp);
                }
                else
                {
                    EditorGUILayout.PropertyField(defaultDamageColorProp);
                    EditorGUILayout.PropertyField(criticalDamageColorProp);
                }

                // Preview of colors
                EditorGUILayout.Space(2);
                EditorGUILayout.LabelField("Color Preview:", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Normal");
                DrawColorPreview(GetNormalColor());
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Critical");
                DrawColorPreview(GetCriticalColor());
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(5);
        }

        private void DrawAnimationSection()
        {
            showAnimationSection = DrawSectionHeader("Animation", showAnimationSection);

            if (showAnimationSection)
            {
                EditorGUILayout.PropertyField(lifetimeProp);
                EditorGUILayout.PropertyField(fadeDelayProp);

                // Make sure fade delay doesn't exceed lifetime
                if (fadeDelayProp.floatValue > lifetimeProp.floatValue)
                {
                    EditorGUILayout.HelpBox("Fade delay cannot exceed lifetime.", MessageType.Warning);
                }

                EditorGUILayout.PropertyField(initialVelocityMinProp);
                EditorGUILayout.PropertyField(initialVelocityMaxProp);
                EditorGUILayout.PropertyField(initialForceProp);
                EditorGUILayout.PropertyField(initialDragProp);
                EditorGUILayout.PropertyField(spreadAngleRangeProp);
                EditorGUILayout.PropertyField(scaleCurveProp);
                EditorGUILayout.PropertyField(applyGravityProp);

                if (applyGravityProp.boolValue)
                {
                    EditorGUILayout.PropertyField(gravityStrengthProp);
                }

                EditorGUILayout.PropertyField(wobbleOnCreateProp);

                if (wobbleOnCreateProp.boolValue)
                {
                    EditorGUILayout.PropertyField(wobbleStrengthProp);
                }
            }

            EditorGUILayout.Space(5);
        }

        private void DrawStackingSection()
        {
            showStackingSection = DrawSectionHeader("Stacking", showStackingSection);

            if (showStackingSection)
            {
                EditorGUILayout.PropertyField(enableStackingProp);

                if (enableStackingProp.boolValue)
                {
                    EditorGUILayout.PropertyField(stackRadiusProp);
                    EditorGUILayout.PropertyField(stackDurationProp);
                    EditorGUILayout.PropertyField(stackOffsetProp);
                }
            }

            EditorGUILayout.Space(5);
        }

        private void DrawPreviewSection()
        {
            showPreviewSection = DrawSectionHeader("Preview", showPreviewSection);

            if (showPreviewSection)
            {
                previewDamage = EditorGUILayout.FloatField("Damage", previewDamage);
                previewCritical = EditorGUILayout.Toggle("Critical", previewCritical);
                previewStacked = EditorGUILayout.Toggle("Stacked", previewStacked);

                if (previewStacked)
                {
                    previewStackCount = EditorGUILayout.IntField("Stack Count", previewStackCount);
                }

                if (GUILayout.Button("Preview"))
                {
                    // Trigger preview logic here
                }
            }

            EditorGUILayout.Space(5);
        }

        private bool DrawSectionHeader(string title, bool foldout)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();

            foldout = EditorGUILayout.Foldout(foldout, title, true, EditorStyles.boldLabel);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            return foldout;
        }

        private Color GetNormalColor()
        {
            return defaultDamageColorProp.colorValue;
        }

        private Color GetCriticalColor()
        {
            return criticalDamageColorProp.colorValue;
        }

        private void DrawColorPreview(Color color)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 20);
            EditorGUI.DrawRect(rect, color);
        }
    }
}
using UnityEngine;
using UnityEditor;


namespace USpring.UI
{
    [CustomEditor(typeof(SpringPopup))]
    public class SpringPopupEditor : UnityEditor.Editor
    {
        // SerializedProperties
        private SerializedProperty spCurrentState;
        private SerializedProperty spAnimationType;
        private SerializedProperty spPreset;
        private SerializedProperty spShowOnEnable;
        private SerializedProperty spHideOnDisable;
        private SerializedProperty spShowDelay;
        private SerializedProperty spHideDelay;
        private SerializedProperty spAutoHideDuration;
        
        private SerializedProperty spShowForce;
        private SerializedProperty spShowDrag;
        private SerializedProperty spHideForce;
        private SerializedProperty spHideDrag;
        private SerializedProperty spAddShowVelocity;
        private SerializedProperty spAddHideVelocity;
        private SerializedProperty spShowVelocityMultiplier;
        private SerializedProperty spHideVelocityMultiplier;
        
        private SerializedProperty spUseScaleAnimation;
        private SerializedProperty spHiddenScale;
        private SerializedProperty spVisibleScale;
        private SerializedProperty spScaleOvershoot;
        
        private SerializedProperty spUsePositionAnimation;
        private SerializedProperty spHiddenPositionOffset;
        private SerializedProperty spVisiblePosition;
        
        private SerializedProperty spUseRotationAnimation;
        private SerializedProperty spHiddenRotation;
        private SerializedProperty spVisibleRotation;
        private SerializedProperty spRotationOvershoot;
        
        private SerializedProperty spOnShow;
        private SerializedProperty spOnShown;
        private SerializedProperty spOnHide;
        private SerializedProperty spOnHidden;
        
        // Foldout states
        private bool showBehaviorSettings = true;
        private bool showSpringSettings = true;
        private bool showScaleSettings = true;
        private bool showPositionSettings = true;
        private bool showRotationSettings = true;
        private bool showEvents = false;
        
        // UI Styling
        private GUIStyle headerStyle;
        private readonly Color headerColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
        
        private void OnEnable()
        {
            // Find all serialized properties
            FindSerializedProperties();
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            // Initialize styles if needed
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel);
                headerStyle.normal.textColor = Color.white;
            }
            
            // Draw inspector sections
            DrawBehaviorSettings();
            DrawSpringSettings();
            DrawAnimationSettings();
            DrawEventSettings();
            
            // Draw test buttons in play mode
            if (Application.isPlaying)
            {
                DrawTestButtons();
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void FindSerializedProperties()
        {
            // Behavior properties
            spCurrentState = serializedObject.FindProperty("currentState");
            spAnimationType = serializedObject.FindProperty("animationType");
            spPreset = serializedObject.FindProperty("preset");
            spShowOnEnable = serializedObject.FindProperty("showOnEnable");
            spHideOnDisable = serializedObject.FindProperty("hideOnDisable");
            spShowDelay = serializedObject.FindProperty("showDelay");
            spHideDelay = serializedObject.FindProperty("hideDelay");
            spAutoHideDuration = serializedObject.FindProperty("autoHideDuration");
            
            // Spring settings
            spShowForce = serializedObject.FindProperty("showForce");
            spShowDrag = serializedObject.FindProperty("showDrag");
            spHideForce = serializedObject.FindProperty("hideForce");
            spHideDrag = serializedObject.FindProperty("hideDrag");
            spAddShowVelocity = serializedObject.FindProperty("addShowVelocity");
            spAddHideVelocity = serializedObject.FindProperty("addHideVelocity");
            spShowVelocityMultiplier = serializedObject.FindProperty("showVelocityMultiplier");
            spHideVelocityMultiplier = serializedObject.FindProperty("hideVelocityMultiplier");
            
            // Scale animation properties
            spUseScaleAnimation = serializedObject.FindProperty("useScaleAnimation");
            spHiddenScale = serializedObject.FindProperty("hiddenScale");
            spVisibleScale = serializedObject.FindProperty("visibleScale");
            spScaleOvershoot = serializedObject.FindProperty("scaleOvershoot");
            
            // Position animation properties
            spUsePositionAnimation = serializedObject.FindProperty("usePositionAnimation");
            spHiddenPositionOffset = serializedObject.FindProperty("hiddenPositionOffset");
            spVisiblePosition = serializedObject.FindProperty("visiblePosition");
            
            // Rotation animation properties
            spUseRotationAnimation = serializedObject.FindProperty("useRotationAnimation");
            spHiddenRotation = serializedObject.FindProperty("hiddenRotation");
            spVisibleRotation = serializedObject.FindProperty("visibleRotation");
            spRotationOvershoot = serializedObject.FindProperty("rotationOvershoot");
            
            // Event properties
            spOnShow = serializedObject.FindProperty("onShow");
            spOnShown = serializedObject.FindProperty("onShown");
            spOnHide = serializedObject.FindProperty("onHide");
            spOnHidden = serializedObject.FindProperty("onHidden");
        }
        
        private void DrawBehaviorSettings()
        {
            showBehaviorSettings = DrawSectionHeader("Behavior Settings", showBehaviorSettings);
            
            if (showBehaviorSettings)
            {
                if (Application.isPlaying)
                {
                    // In play mode, display current state but don't allow changes
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(spCurrentState);
                    EditorGUI.EndDisabledGroup();
                }
                
                // Animation type dropdown
                EditorGUILayout.PropertyField(spAnimationType);
                
                // Update animation toggles based on animation type
                SpringPopup.AnimationType animType = (SpringPopup.AnimationType)spAnimationType.enumValueIndex;
                switch (animType)
                {
                    case SpringPopup.AnimationType.Scale:
                        spUseScaleAnimation.boolValue = true;
                        spUsePositionAnimation.boolValue = false;
                        spUseRotationAnimation.boolValue = false;
                        break;
                    case SpringPopup.AnimationType.Position:
                        spUseScaleAnimation.boolValue = false;
                        spUsePositionAnimation.boolValue = true;
                        spUseRotationAnimation.boolValue = false;
                        break;
                    case SpringPopup.AnimationType.Rotation:
                        spUseScaleAnimation.boolValue = false;
                        spUsePositionAnimation.boolValue = false;
                        spUseRotationAnimation.boolValue = true;
                        break;
                    case SpringPopup.AnimationType.Combined:
                        // Keep existing values or default to all enabled
                        if (!spUseScaleAnimation.boolValue && !spUsePositionAnimation.boolValue && !spUseRotationAnimation.boolValue)
                        {
                            spUseScaleAnimation.boolValue = true;
                            spUsePositionAnimation.boolValue = true;
                            spUseRotationAnimation.boolValue = true;
                        }
                        break;
                }
                
                // Preset selector
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(spPreset);
                if (EditorGUI.EndChangeCheck() && Application.isPlaying)
                {
                    // Apply preset immediately in play mode
                    SpringPopup popup = (SpringPopup)target;
                    popup.SetPreset((SpringPopup.PopupPreset)spPreset.enumValueIndex);
                }
                
                EditorGUILayout.Space(5);
                
                // Auto behaviors
                EditorGUILayout.PropertyField(spShowOnEnable);
                EditorGUILayout.PropertyField(spHideOnDisable);
                
                EditorGUILayout.Space(5);
                
                // Delay settings
                EditorGUILayout.PropertyField(spShowDelay);
                EditorGUILayout.PropertyField(spHideDelay);
                EditorGUILayout.PropertyField(spAutoHideDuration);
            }
            
            EditorGUILayout.Space(5);
        }
        
        private void DrawSpringSettings()
        {
            showSpringSettings = DrawSectionHeader("Spring Settings", showSpringSettings);
            
            if (showSpringSettings)
            {
                EditorGUILayout.LabelField("Show Animation", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(spShowForce);
                EditorGUILayout.PropertyField(spShowDrag);
                EditorGUILayout.PropertyField(spAddShowVelocity);
                if (spAddShowVelocity.boolValue)
                {
                    EditorGUILayout.PropertyField(spShowVelocityMultiplier);
                }
                EditorGUI.indentLevel--;
                
                EditorGUILayout.Space(5);
                
                EditorGUILayout.LabelField("Hide Animation", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(spHideForce);
                EditorGUILayout.PropertyField(spHideDrag);
                EditorGUILayout.PropertyField(spAddHideVelocity);
                if (spAddHideVelocity.boolValue)
                {
                    EditorGUILayout.PropertyField(spHideVelocityMultiplier);
                }
                EditorGUI.indentLevel--;
            }
            
            EditorGUILayout.Space(5);
        }
        
        private void DrawAnimationSettings()
        {
            // Scale Animation Settings
            if (spUseScaleAnimation.boolValue)
            {
                showScaleSettings = DrawSectionHeader("Scale Animation", showScaleSettings);
                
                if (showScaleSettings)
                {
                    EditorGUILayout.PropertyField(spHiddenScale);
                    EditorGUILayout.PropertyField(spVisibleScale);
                    EditorGUILayout.PropertyField(spScaleOvershoot);
                }
                
                EditorGUILayout.Space(5);
            }
            
            // Position Animation Settings
            if (spUsePositionAnimation.boolValue)
            {
                showPositionSettings = DrawSectionHeader("Position Animation", showPositionSettings);
                
                if (showPositionSettings)
                {
                    EditorGUILayout.PropertyField(spHiddenPositionOffset);
                    EditorGUILayout.PropertyField(spVisiblePosition);
                }
                
                EditorGUILayout.Space(5);
            }
            
            // Rotation Animation Settings
            if (spUseRotationAnimation.boolValue)
            {
                showRotationSettings = DrawSectionHeader("Rotation Animation", showRotationSettings);
                
                if (showRotationSettings)
                {
                    EditorGUILayout.PropertyField(spHiddenRotation);
                    EditorGUILayout.PropertyField(spVisibleRotation);
                    EditorGUILayout.PropertyField(spRotationOvershoot);
                }
                
                EditorGUILayout.Space(5);
            }
        }
        
        private void DrawEventSettings()
        {
            showEvents = DrawSectionHeader("Events", showEvents);
            
            if (showEvents)
            {
                EditorGUILayout.PropertyField(spOnShow);
                EditorGUILayout.PropertyField(spOnShown);
                EditorGUILayout.PropertyField(spOnHide);
                EditorGUILayout.PropertyField(spOnHidden);
            }
            
            EditorGUILayout.Space(5);
        }
        
        private void DrawTestButtons()
        {
            DrawSectionHeader("Test Controls", true, false);
            
            SpringPopup popup = (SpringPopup)target;
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Show", GUILayout.Height(30)))
            {
                popup.Show();
            }
            
            if (GUILayout.Button("Hide", GUILayout.Height(30)))
            {
                popup.Hide();
            }
            
            if (GUILayout.Button("Toggle", GUILayout.Height(30)))
            {
                popup.Toggle();
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Set Visible (No Animation)"))
            {
                popup.SetVisibleState(true);
            }
            
            if (GUILayout.Button("Set Hidden (No Animation)"))
            {
                popup.SetHiddenState(true);
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
        }
        
        private bool DrawSectionHeader(string title, bool foldout, bool showFoldout = true)
        {
            EditorGUILayout.Space(2);
            
            // Draw header background
            Rect rect = EditorGUILayout.GetControlRect(false, 24);
            EditorGUI.DrawRect(rect, headerColor);
            
            // Create a space for the foldout button/triangle
            Rect foldoutRect = rect;
            foldoutRect.width = 20;
            
            // The label rect starts after the foldout button
            Rect labelRect = rect;
            labelRect.x += showFoldout ? 20 : 10;
            
            // Handle foldout logic
            bool result = foldout;
            if (showFoldout)
            {
                result = EditorGUI.Foldout(foldoutRect, foldout, GUIContent.none);
            }
            
            // Draw title
            EditorGUI.LabelField(labelRect, title, headerStyle);
            
            return result;
        }
    }
}
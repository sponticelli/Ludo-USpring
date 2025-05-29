using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace USpring.Enhanced
{
    /// <summary>
    /// Enhanced base class for spring component editors with advanced features.
    /// </summary>
    public abstract class EnhancedSpringComponentEditor : Editor
    {
        protected Dictionary<string, bool> _foldoutStates = new Dictionary<string, bool>();
        protected bool _showPreview = false;
        protected bool _showAdvanced = false;
        protected bool _showDebugInfo = false;
        protected int _selectedPresetIndex = 0;
        
        private Vector2 _scrollPosition;
        private bool _isPlaying = false;
        private float _previewTime = 0f;
        
        // Performance monitoring
        private float _lastUpdateTime;
        private int _updateCount;
        private float _averageUpdateTime;
        
        protected virtual void OnEnable()
        {
            _isPlaying = Application.isPlaying;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        
        protected virtual void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }
        
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            _isPlaying = Application.isPlaying;
            Repaint();
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawHeader();
            DrawQuickActions();
            DrawPresetSection();
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            DrawMainProperties();
            DrawAdvancedSection();
            DrawPreviewSection();
            DrawDebugSection();
            
            EditorGUILayout.EndScrollView();
            
            DrawFooter();
            
            serializedObject.ApplyModifiedProperties();
            
            // Performance monitoring
            UpdatePerformanceMetrics();
        }
        
        protected virtual void DrawHeader()
        {
            USpringEditorStyles.BeginSection();
            
            EditorGUILayout.BeginHorizontal();
            
            // Component icon and name
            GUIContent headerContent = new GUIContent(GetComponentDisplayName(), GetComponentIcon());
            GUILayout.Label(headerContent, USpringEditorStyles.HeaderStyle);
            
            GUILayout.FlexibleSpace();
            
            // Status indicator
            DrawStatusIndicator();
            
            EditorGUILayout.EndHorizontal();
            
            // Component description
            if (!string.IsNullOrEmpty(GetComponentDescription()))
            {
                EditorGUILayout.LabelField(GetComponentDescription(), USpringEditorStyles.SubHeaderStyle);
            }
            
            USpringEditorStyles.EndSection();
        }
        
        protected virtual void DrawQuickActions()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Reset All", USpringEditorStyles.ButtonStyle, GUILayout.Width(80)))
            {
                ResetAllSprings();
            }
            
            if (GUILayout.Button("Copy Settings", USpringEditorStyles.ButtonStyle, GUILayout.Width(100)))
            {
                CopySettings();
            }
            
            if (GUILayout.Button("Paste Settings", USpringEditorStyles.ButtonStyle, GUILayout.Width(100)))
            {
                PasteSettings();
            }
            
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Open Curve Editor", USpringEditorStyles.ButtonStyle, GUILayout.Width(120)))
            {
                SpringCurveEditor.ShowWindow();
            }
            
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(4);
        }
        
        protected virtual void DrawPresetSection()
        {
            USpringEditorStyles.BeginSection();
            
            _foldoutStates["presets"] = USpringEditorStyles.DrawHeader("Presets", 
                GetFoldoutState("presets"), USpringEditorStyles.SecondaryColor);
            
            if (_foldoutStates["presets"])
            {
                EditorGUILayout.BeginHorizontal();
                
                // Preset dropdown
                var presets = USpringPresets.GetAllPresets();
                string[] presetNames = new string[presets.Count + 1];
                presetNames[0] = "Select Preset...";
                for (int i = 0; i < presets.Count; i++)
                    presetNames[i + 1] = presets[i].Name;
                
                int newSelection = EditorGUILayout.Popup("Preset", _selectedPresetIndex, presetNames);
                if (newSelection != _selectedPresetIndex && newSelection > 0)
                {
                    _selectedPresetIndex = newSelection;
                    ApplyPreset(presets[newSelection - 1]);
                }
                
                if (GUILayout.Button("Apply", USpringEditorStyles.ButtonStyle, GUILayout.Width(60)))
                {
                    if (_selectedPresetIndex > 0)
                        ApplyPreset(presets[_selectedPresetIndex - 1]);
                }
                
                EditorGUILayout.EndHorizontal();
                
                // Quick preset buttons
                USpringPresets.DrawPresetButtons(ApplyPreset);
            }
            
            USpringEditorStyles.EndSection();
        }
        
        protected virtual void DrawMainProperties()
        {
            USpringEditorStyles.BeginSection();
            
            _foldoutStates["main"] = USpringEditorStyles.DrawHeader("Spring Properties", 
                GetFoldoutState("main", true), GetSpringTypeColor());
            
            if (_foldoutStates["main"])
            {
                DrawSpringProperties();
            }
            
            USpringEditorStyles.EndSection();
        }
        
        protected virtual void DrawAdvancedSection()
        {
            USpringEditorStyles.BeginSection();
            
            _foldoutStates["advanced"] = USpringEditorStyles.DrawHeader("Advanced Settings", 
                GetFoldoutState("advanced"), USpringEditorStyles.AccentColor);
            
            if (_foldoutStates["advanced"])
            {
                DrawAdvancedProperties();
                DrawClampingProperties();
                DrawEventProperties();
            }
            
            USpringEditorStyles.EndSection();
        }
        
        protected virtual void DrawPreviewSection()
        {
            USpringEditorStyles.BeginSection();
            
            _foldoutStates["preview"] = USpringEditorStyles.DrawHeader("Preview", 
                GetFoldoutState("preview"), USpringEditorStyles.PrimaryColor);
            
            if (_foldoutStates["preview"])
            {
                DrawSpringPreview();
                DrawPreviewControls();
            }
            
            USpringEditorStyles.EndSection();
        }
        
        protected virtual void DrawDebugSection()
        {
            if (!_isPlaying) return;
            
            USpringEditorStyles.BeginSection();
            
            _foldoutStates["debug"] = USpringEditorStyles.DrawHeader("Debug Info", 
                GetFoldoutState("debug"), USpringEditorStyles.WarningColor);
            
            if (_foldoutStates["debug"])
            {
                DrawRuntimeDebugInfo();
                DrawPerformanceInfo();
            }
            
            USpringEditorStyles.EndSection();
        }
        
        protected virtual void DrawFooter()
        {
            EditorGUILayout.BeginHorizontal();
            
            // Help button
            if (GUILayout.Button("?", USpringEditorStyles.ButtonStyle, GUILayout.Width(25)))
            {
                ShowHelp();
            }
            
            GUILayout.FlexibleSpace();
            
            // Performance indicator
            if (_isPlaying)
            {
                string perfText = $"Avg Update: {_averageUpdateTime:F2}ms";
                GUILayout.Label(perfText, USpringEditorStyles.PreviewLabelStyle);
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        protected virtual void DrawStatusIndicator()
        {
            Color statusColor = GetStatusColor();
            string statusText = GetStatusText();
            
            Rect statusRect = GUILayoutUtility.GetRect(60, 16);
            EditorGUI.DrawRect(statusRect, statusColor);
            
            GUIStyle centeredStyle = new GUIStyle(EditorStyles.miniLabel);
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            centeredStyle.normal.textColor = Color.white;
            
            GUI.Label(statusRect, statusText, centeredStyle);
        }
        
        protected virtual void DrawSpringPreview()
        {
            Rect previewRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, 
                GUILayout.ExpandWidth(true), GUILayout.Height(80));
            
            // Background
            EditorGUI.DrawRect(previewRect, new Color(0.1f, 0.1f, 0.1f, 1f));
            
            // Draw spring curve
            DrawSpringCurveInRect(previewRect);
            
            // Draw current time marker if playing
            if (_isPlaying)
            {
                DrawTimeMarker(previewRect);
            }
        }
        
        protected virtual void DrawPreviewControls()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button(_isPlaying ? "⏸" : "▶", USpringEditorStyles.ButtonStyle, GUILayout.Width(30)))
            {
                TogglePreview();
            }
            
            if (GUILayout.Button("⏹", USpringEditorStyles.ButtonStyle, GUILayout.Width(30)))
            {
                StopPreview();
            }
            
            // Time slider
            _previewTime = EditorGUILayout.Slider("Time", _previewTime, 0f, 4f);
            
            EditorGUILayout.EndHorizontal();
        }
        
        protected virtual void DrawRuntimeDebugInfo()
        {
            // Override in derived classes to show runtime spring values
            EditorGUILayout.LabelField("Runtime debug info not available for this component type.");
        }
        
        protected virtual void DrawPerformanceInfo()
        {
            EditorGUILayout.LabelField($"Update Count: {_updateCount}");
            EditorGUILayout.LabelField($"Average Update Time: {_averageUpdateTime:F2}ms");
            EditorGUILayout.LabelField($"Last Update: {_lastUpdateTime:F2}ms");
        }
        
        // Abstract methods to be implemented by derived classes
        protected abstract void DrawSpringProperties();
        protected abstract void DrawAdvancedProperties();
        protected abstract void DrawClampingProperties();
        protected abstract void DrawEventProperties();
        protected abstract void DrawSpringCurveInRect(Rect rect);
        protected abstract string GetComponentDisplayName();
        protected abstract string GetComponentDescription();
        protected abstract Texture2D GetComponentIcon();
        protected abstract Color GetSpringTypeColor();
        protected abstract Color GetStatusColor();
        protected abstract string GetStatusText();
        
        // Virtual methods with default implementations
        protected virtual void ResetAllSprings()
        {
            // Default implementation
            Debug.Log("Reset all springs");
        }
        
        protected virtual void CopySettings()
        {
            // Default implementation
            Debug.Log("Copy settings");
        }
        
        protected virtual void PasteSettings()
        {
            // Default implementation
            Debug.Log("Paste settings");
        }
        
        protected virtual void ApplyPreset(USpringPresets.SpringPreset preset)
        {
            // Default implementation
            Debug.Log($"Apply preset: {preset.Name}");
        }
        
        protected virtual void TogglePreview()
        {
            _isPlaying = !_isPlaying;
            if (_isPlaying)
            {
                _previewTime = 0f;
            }
        }
        
        protected virtual void StopPreview()
        {
            _isPlaying = false;
            _previewTime = 0f;
        }
        
        protected virtual void ShowHelp()
        {
            string helpUrl = "https://github.com/sponticelli/Ludo-USpring/wiki";
            Application.OpenURL(helpUrl);
        }
        
        protected virtual void DrawTimeMarker(Rect rect)
        {
            float normalizedTime = (_previewTime % 4f) / 4f;
            float x = rect.x + normalizedTime * rect.width;
            
            Handles.color = Color.red;
            Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.yMax, 0));
        }
        
        protected bool GetFoldoutState(string key, bool defaultValue = false)
        {
            if (!_foldoutStates.ContainsKey(key))
                _foldoutStates[key] = defaultValue;
            return _foldoutStates[key];
        }
        
        private void UpdatePerformanceMetrics()
        {
            float currentTime = Time.realtimeSinceStartup * 1000f; // Convert to milliseconds
            
            if (_lastUpdateTime > 0)
            {
                float deltaTime = currentTime - _lastUpdateTime;
                _averageUpdateTime = (_averageUpdateTime * _updateCount + deltaTime) / (_updateCount + 1);
                _updateCount++;
            }
            
            _lastUpdateTime = currentTime;
        }
    }
}

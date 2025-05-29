using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace USpring.Enhanced
{
    /// <summary>
    /// Enhanced preset management system with categories, custom presets, and import/export functionality.
    /// </summary>
    public class EnhancedPresetManager : EditorWindow
    {
        private Vector2 _scrollPosition;
        private List<SpringPresetCategory> _categories = new List<SpringPresetCategory>();
        private int _selectedCategoryIndex = 0;
        private SpringPreset _editingPreset;
        private bool _isCreatingNewPreset = false;
        
        private string _newPresetName = "";
        private string _newPresetDescription = "";
        private float _newPresetForce = 20f;
        private float _newPresetDrag = 10f;
        private bool _newPresetUseAnalytical = false;
        
        private string _searchFilter = "";
        private bool _showBuiltInPresets = true;
        private bool _showCustomPresets = true;
        
        private const string CUSTOM_PRESETS_PATH = "Assets/USpring/CustomPresets";
        
        [MenuItem("Tools/USpring/Preset Manager", priority = 101)]
        public static void ShowWindow()
        {
            var window = GetWindow<EnhancedPresetManager>("Preset Manager");
            window.minSize = new Vector2(500, 400);
            window.Show();
        }
        
        private void OnEnable()
        {
            LoadPresets();
        }
        
        private void OnGUI()
        {
            DrawHeader();
            DrawToolbar();
            
            EditorGUILayout.BeginHorizontal();
            
            // Categories sidebar
            DrawCategoriesSidebar();
            
            // Main content area
            EditorGUILayout.BeginVertical();
            DrawMainContent();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
            
            // Preset editor popup
            if (_editingPreset != null || _isCreatingNewPreset)
            {
                DrawPresetEditor();
            }
        }
        
        private void DrawHeader()
        {
            USpringEditorStyles.BeginSection();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Spring Preset Manager", USpringEditorStyles.HeaderStyle);
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("New Preset", USpringEditorStyles.ButtonStyle, GUILayout.Width(100)))
            {
                StartCreatingNewPreset();
            }
            
            if (GUILayout.Button("Import", USpringEditorStyles.ButtonStyle, GUILayout.Width(80)))
            {
                ImportPresets();
            }
            
            if (GUILayout.Button("Export", USpringEditorStyles.ButtonStyle, GUILayout.Width(80)))
            {
                ExportPresets();
            }
            
            EditorGUILayout.EndHorizontal();
            
            USpringEditorStyles.EndSection();
        }
        
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            
            // Filters
            _showBuiltInPresets = GUILayout.Toggle(_showBuiltInPresets, "Built-in", EditorStyles.toolbarButton);
            _showCustomPresets = GUILayout.Toggle(_showCustomPresets, "Custom", EditorStyles.toolbarButton);
            
            GUILayout.FlexibleSpace();
            
            // Search
            GUILayout.Label("Search:", EditorStyles.miniLabel);
            _searchFilter = GUILayout.TextField(_searchFilter, EditorStyles.toolbarTextField, GUILayout.Width(150));
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawCategoriesSidebar()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(150));
            
            USpringEditorStyles.DrawSubHeader("Categories");
            
            for (int i = 0; i < _categories.Count; i++)
            {
                var category = _categories[i];
                bool isSelected = i == _selectedCategoryIndex;
                
                GUIStyle style = isSelected ? USpringEditorStyles.TabButtonStyle : GUI.skin.button;
                if (isSelected)
                {
                    GUI.backgroundColor = USpringEditorStyles.PrimaryColor;
                }
                
                if (GUILayout.Button($"{category.Name} ({category.Presets.Count})", style))
                {
                    _selectedCategoryIndex = i;
                }
                
                GUI.backgroundColor = Color.white;
            }
            
            EditorGUILayout.EndVertical();
        }
        
        private void DrawMainContent()
        {
            if (_selectedCategoryIndex >= 0 && _selectedCategoryIndex < _categories.Count)
            {
                var selectedCategory = _categories[_selectedCategoryIndex];
                
                USpringEditorStyles.DrawSubHeader($"{selectedCategory.Name} Presets");
                
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                
                var filteredPresets = GetFilteredPresets(selectedCategory);
                
                foreach (var preset in filteredPresets)
                {
                    DrawPresetItem(preset);
                }
                
                if (filteredPresets.Count == 0)
                {
                    EditorGUILayout.HelpBox("No presets found matching the current filters.", MessageType.Info);
                }
                
                EditorGUILayout.EndScrollView();
            }
        }
        
        private void DrawPresetItem(SpringPreset preset)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            EditorGUILayout.BeginHorizontal();
            
            // Preset info
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(preset.Name, EditorStyles.boldLabel);
            EditorGUILayout.LabelField(preset.Description, EditorStyles.wordWrappedLabel);
            EditorGUILayout.LabelField($"Force: {preset.Force:F1} | Drag: {preset.Drag:F1} | Analytical: {preset.UseAnalyticalSolution}");
            EditorGUILayout.EndVertical();
            
            // Actions
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            
            if (GUILayout.Button("Edit", USpringEditorStyles.ButtonStyle))
            {
                StartEditingPreset(preset);
            }
            
            if (GUILayout.Button("Duplicate", USpringEditorStyles.ButtonStyle))
            {
                DuplicatePreset(preset);
            }
            
            if (preset.IsCustom && GUILayout.Button("Delete", USpringEditorStyles.ButtonStyle))
            {
                if (EditorUtility.DisplayDialog("Delete Preset", $"Are you sure you want to delete '{preset.Name}'?", "Delete", "Cancel"))
                {
                    DeletePreset(preset);
                }
            }
            
            if (GUILayout.Button("Apply to Selection", USpringEditorStyles.ButtonStyle))
            {
                ApplyPresetToSelection(preset);
            }
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }
        
        private void DrawPresetEditor()
        {
            // Modal dialog overlay
            Rect overlayRect = new Rect(0, 0, position.width, position.height);
            EditorGUI.DrawRect(overlayRect, new Color(0, 0, 0, 0.5f));
            
            // Editor dialog
            Rect dialogRect = new Rect(position.width * 0.2f, position.height * 0.2f, 
                                     position.width * 0.6f, position.height * 0.6f);
            
            GUILayout.BeginArea(dialogRect, GUI.skin.window);
            
            GUILayout.Label(_isCreatingNewPreset ? "Create New Preset" : "Edit Preset", USpringEditorStyles.HeaderStyle);
            
            GUILayout.Space(10);
            
            // Preset properties
            _newPresetName = EditorGUILayout.TextField("Name", _newPresetName);
            _newPresetDescription = EditorGUILayout.TextField("Description", _newPresetDescription);
            _newPresetForce = EditorGUILayout.FloatField("Force", _newPresetForce);
            _newPresetDrag = EditorGUILayout.FloatField("Drag", _newPresetDrag);
            _newPresetUseAnalytical = EditorGUILayout.Toggle("Use Analytical Solution", _newPresetUseAnalytical);
            
            GUILayout.Space(10);
            
            // Preview
            USpringEditorStyles.DrawSubHeader("Preview");
            DrawPresetPreview(_newPresetForce, _newPresetDrag);
            
            GUILayout.FlexibleSpace();
            
            // Buttons
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Cancel", USpringEditorStyles.ButtonStyle))
            {
                CancelPresetEditing();
            }
            
            if (GUILayout.Button(_isCreatingNewPreset ? "Create" : "Save", USpringEditorStyles.ButtonStyle))
            {
                SavePreset();
            }
            
            EditorGUILayout.EndHorizontal();
            
            GUILayout.EndArea();
        }
        
        private void DrawPresetPreview(float force, float drag)
        {
            Rect previewRect = GUILayoutUtility.GetRect(200, 60);
            
            // Draw background
            EditorGUI.DrawRect(previewRect, new Color(0.1f, 0.1f, 0.1f, 1f));
            
            // Draw spring curve preview
            DrawSpringCurvePreview(previewRect, force, drag);
        }
        
        private void DrawSpringCurvePreview(Rect rect, float force, float drag)
        {
            // Simple spring simulation for preview
            int samples = 50;
            Vector3[] points = new Vector3[samples];
            
            float current = 0f;
            float velocity = 0f;
            float target = 1f;
            float deltaTime = 0.02f;
            
            for (int i = 0; i < samples; i++)
            {
                float t = (float)i / (samples - 1);
                
                // Simple spring physics
                float springForce = (target - current) * force;
                float dampingForce = velocity * drag;
                float acceleration = springForce - dampingForce;
                
                velocity += acceleration * deltaTime;
                current += velocity * deltaTime;
                
                float x = rect.x + t * rect.width;
                float y = rect.y + rect.height * 0.5f - current * rect.height * 0.4f;
                
                points[i] = new Vector3(x, y, 0);
            }
            
            // Draw the curve
            Handles.color = USpringEditorStyles.PrimaryColor;
            Handles.DrawAAPolyLine(2f, points);
            
            // Draw center line
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector3(rect.x, rect.y + rect.height * 0.5f, 0),
                           new Vector3(rect.xMax, rect.y + rect.height * 0.5f, 0));
        }
        
        private List<SpringPreset> GetFilteredPresets(SpringPresetCategory category)
        {
            var presets = category.Presets.AsEnumerable();
            
            if (!_showBuiltInPresets)
                presets = presets.Where(p => p.IsCustom);
            
            if (!_showCustomPresets)
                presets = presets.Where(p => !p.IsCustom);
            
            if (!string.IsNullOrEmpty(_searchFilter))
                presets = presets.Where(p => p.Name.ToLower().Contains(_searchFilter.ToLower()) ||
                                           p.Description.ToLower().Contains(_searchFilter.ToLower()));
            
            return presets.ToList();
        }
        
        private void LoadPresets()
        {
            _categories.Clear();
            
            // Load built-in presets
            var builtInCategory = new SpringPresetCategory { Name = "Built-in" };
            foreach (var preset in USpringPresets.GetAllPresets())
            {
                builtInCategory.Presets.Add(new SpringPreset
                {
                    Name = preset.Name,
                    Description = preset.Description,
                    Force = preset.Force,
                    Drag = preset.Drag,
                    UseAnalyticalSolution = preset.UseAnalyticalSolution,
                    IsCustom = false
                });
            }
            _categories.Add(builtInCategory);
            
            // Load custom presets
            var customCategory = new SpringPresetCategory { Name = "Custom" };
            LoadCustomPresets(customCategory);
            _categories.Add(customCategory);
            
            // Load categorized presets
            LoadCategorizedPresets();
        }
        
        private void LoadCustomPresets(SpringPresetCategory category)
        {
            // Implementation to load custom presets from files
            if (Directory.Exists(CUSTOM_PRESETS_PATH))
            {
                var files = Directory.GetFiles(CUSTOM_PRESETS_PATH, "*.json");
                foreach (var file in files)
                {
                    try
                    {
                        string json = File.ReadAllText(file);
                        var preset = JsonUtility.FromJson<SpringPreset>(json);
                        preset.IsCustom = true;
                        category.Presets.Add(preset);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Failed to load preset from {file}: {e.Message}");
                    }
                }
            }
        }
        
        private void LoadCategorizedPresets()
        {
            // Add more specialized categories
            var uiCategory = new SpringPresetCategory { Name = "UI Animation" };
            uiCategory.Presets.AddRange(new[]
            {
                new SpringPreset { Name = "Button Press", Description = "Quick, snappy button feedback", Force = 60f, Drag = 15f, IsCustom = false },
                new SpringPreset { Name = "Panel Slide", Description = "Smooth panel transitions", Force = 25f, Drag = 12f, IsCustom = false },
                new SpringPreset { Name = "Popup", Description = "Bouncy popup appearance", Force = 45f, Drag = 8f, IsCustom = false }
            });
            _categories.Add(uiCategory);
            
            var gameplayCategory = new SpringPresetCategory { Name = "Gameplay" };
            gameplayCategory.Presets.AddRange(new[]
            {
                new SpringPreset { Name = "Camera Follow", Description = "Smooth camera following", Force = 15f, Drag = 8f, IsCustom = false },
                new SpringPreset { Name = "Physics Object", Description = "Realistic physics response", Force = 30f, Drag = 5f, IsCustom = false },
                new SpringPreset { Name = "Character Movement", Description = "Responsive character motion", Force = 40f, Drag = 10f, IsCustom = false }
            });
            _categories.Add(gameplayCategory);
        }
        
        private void StartCreatingNewPreset()
        {
            _isCreatingNewPreset = true;
            _editingPreset = null;
            _newPresetName = "";
            _newPresetDescription = "";
            _newPresetForce = 20f;
            _newPresetDrag = 10f;
            _newPresetUseAnalytical = false;
        }
        
        private void StartEditingPreset(SpringPreset preset)
        {
            _isCreatingNewPreset = false;
            _editingPreset = preset;
            _newPresetName = preset.Name;
            _newPresetDescription = preset.Description;
            _newPresetForce = preset.Force;
            _newPresetDrag = preset.Drag;
            _newPresetUseAnalytical = preset.UseAnalyticalSolution;
        }
        
        private void CancelPresetEditing()
        {
            _isCreatingNewPreset = false;
            _editingPreset = null;
        }
        
        private void SavePreset()
        {
            if (string.IsNullOrEmpty(_newPresetName))
            {
                EditorUtility.DisplayDialog("Error", "Preset name cannot be empty.", "OK");
                return;
            }
            
            var preset = new SpringPreset
            {
                Name = _newPresetName,
                Description = _newPresetDescription,
                Force = _newPresetForce,
                Drag = _newPresetDrag,
                UseAnalyticalSolution = _newPresetUseAnalytical,
                IsCustom = true
            };
            
            if (_isCreatingNewPreset)
            {
                // Add to custom category
                var customCategory = _categories.FirstOrDefault(c => c.Name == "Custom");
                customCategory?.Presets.Add(preset);
                
                // Save to file
                SaveCustomPreset(preset);
            }
            else if (_editingPreset != null)
            {
                // Update existing preset
                _editingPreset.Name = preset.Name;
                _editingPreset.Description = preset.Description;
                _editingPreset.Force = preset.Force;
                _editingPreset.Drag = preset.Drag;
                _editingPreset.UseAnalyticalSolution = preset.UseAnalyticalSolution;
                
                if (_editingPreset.IsCustom)
                {
                    SaveCustomPreset(_editingPreset);
                }
            }
            
            CancelPresetEditing();
        }
        
        private void SaveCustomPreset(SpringPreset preset)
        {
            if (!Directory.Exists(CUSTOM_PRESETS_PATH))
            {
                Directory.CreateDirectory(CUSTOM_PRESETS_PATH);
            }
            
            string fileName = $"{preset.Name.Replace(" ", "_")}.json";
            string filePath = Path.Combine(CUSTOM_PRESETS_PATH, fileName);
            
            try
            {
                string json = JsonUtility.ToJson(preset, true);
                File.WriteAllText(filePath, json);
                AssetDatabase.Refresh();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save preset: {e.Message}");
            }
        }
        
        private void DuplicatePreset(SpringPreset preset)
        {
            var duplicate = new SpringPreset
            {
                Name = preset.Name + " Copy",
                Description = preset.Description,
                Force = preset.Force,
                Drag = preset.Drag,
                UseAnalyticalSolution = preset.UseAnalyticalSolution,
                IsCustom = true
            };
            
            var customCategory = _categories.FirstOrDefault(c => c.Name == "Custom");
            customCategory?.Presets.Add(duplicate);
            
            SaveCustomPreset(duplicate);
        }
        
        private void DeletePreset(SpringPreset preset)
        {
            // Remove from category
            foreach (var category in _categories)
            {
                category.Presets.Remove(preset);
            }
            
            // Delete file if custom
            if (preset.IsCustom)
            {
                string fileName = $"{preset.Name.Replace(" ", "_")}.json";
                string filePath = Path.Combine(CUSTOM_PRESETS_PATH, fileName);
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    AssetDatabase.Refresh();
                }
            }
        }
        
        private void ApplyPresetToSelection(SpringPreset preset)
        {
            // Apply preset to selected GameObjects with spring components
            foreach (var obj in Selection.gameObjects)
            {
                var springComponents = obj.GetComponents<MonoBehaviour>();
                foreach (var component in springComponents)
                {
                    // Apply preset to spring fields in the component
                    ApplyPresetToComponent(component, preset);
                }
            }
        }
        
        private void ApplyPresetToComponent(MonoBehaviour component, SpringPreset preset)
        {
            // Use reflection to find and apply preset to spring fields
            var fields = component.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            foreach (var field in fields)
            {
                if (IsSpringType(field.FieldType))
                {
                    var spring = field.GetValue(component);
                    if (spring != null)
                    {
                        // Apply preset values using reflection
                        ApplyPresetToSpring(spring, preset);
                    }
                }
            }
        }
        
        private bool IsSpringType(System.Type type)
        {
            return type == typeof(SpringFloat) || type == typeof(SpringVector2) || 
                   type == typeof(SpringVector3) || type == typeof(SpringVector4) || 
                   type == typeof(SpringColor);
        }
        
        private void ApplyPresetToSpring(object spring, SpringPreset preset)
        {
            // Apply preset values to spring using reflection
            var springType = spring.GetType();
            
            var forceField = springType.GetField("commonForce", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dragField = springType.GetField("commonDrag", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            forceField?.SetValue(spring, preset.Force);
            dragField?.SetValue(spring, preset.Drag);
        }
        
        private void ImportPresets()
        {
            string path = EditorUtility.OpenFilePanel("Import Spring Presets", "", "json");
            if (!string.IsNullOrEmpty(path))
            {
                // Implementation for importing presets
                Debug.Log($"Importing presets from: {path}");
            }
        }
        
        private void ExportPresets()
        {
            string path = EditorUtility.SaveFilePanel("Export Spring Presets", "", "spring_presets", "json");
            if (!string.IsNullOrEmpty(path))
            {
                // Implementation for exporting presets
                Debug.Log($"Exporting presets to: {path}");
            }
        }
        
        [System.Serializable]
        private class SpringPreset
        {
            public string Name;
            public string Description;
            public float Force;
            public float Drag;
            public bool UseAnalyticalSolution;
            public bool IsCustom;
        }
        
        private class SpringPresetCategory
        {
            public string Name;
            public List<SpringPreset> Presets = new List<SpringPreset>();
        }
    }
}

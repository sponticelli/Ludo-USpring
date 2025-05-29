using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using USpring.Core;

namespace USpring.Enhanced
{
    /// <summary>
    /// Advanced spring analyzer tool for debugging, optimization, and performance monitoring.
    /// </summary>
    public class SpringAnalyzerWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private List<SpringComponentInfo> _springComponents = new List<SpringComponentInfo>();
        private bool _autoRefresh = true;
        private float _refreshInterval = 0.5f;
        private double _lastRefreshTime;

        private int _selectedTab = 0;
        private readonly string[] _tabNames = { "Overview", "Performance", "Optimization", "Debugging" };

        private bool _showInactiveComponents = false;
        private bool _showOnlyProblematic = false;
        private string _searchFilter = "";

        [MenuItem("Tools/USpring/Spring Analyzer", priority = 100)]
        public static void ShowWindow()
        {
            var window = GetWindow<SpringAnalyzerWindow>("Spring Analyzer");
            window.minSize = new Vector2(600, 400);
            window.Show();
        }

        private void OnEnable()
        {
            RefreshSpringComponents();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            RefreshSpringComponents();
        }

        private void OnGUI()
        {
            DrawHeader();
            DrawToolbar();
            DrawTabs();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            switch (_selectedTab)
            {
                case 0: DrawOverviewTab(); break;
                case 1: DrawPerformanceTab(); break;
                case 2: DrawOptimizationTab(); break;
                case 3: DrawDebuggingTab(); break;
            }

            EditorGUILayout.EndScrollView();

            // Auto-refresh in play mode
            if (Application.isPlaying && _autoRefresh && EditorApplication.timeSinceStartup - _lastRefreshTime > _refreshInterval)
            {
                RefreshSpringComponents();
                Repaint();
            }
        }

        private void DrawHeader()
        {
            USpringEditorStyles.BeginSection();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Spring Analyzer", USpringEditorStyles.HeaderStyle);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Refresh", USpringEditorStyles.ButtonStyle, GUILayout.Width(80)))
            {
                RefreshSpringComponents();
            }

            EditorGUILayout.EndHorizontal();

            // Statistics
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"Total Springs: {_springComponents.Count}", USpringEditorStyles.SubHeaderStyle);
            GUILayout.Label($"Active: {_springComponents.Count(s => s.IsActive)}", USpringEditorStyles.SubHeaderStyle);
            GUILayout.Label($"Performance Issues: {_springComponents.Count(s => s.HasPerformanceIssues)}", USpringEditorStyles.SubHeaderStyle);
            EditorGUILayout.EndHorizontal();

            USpringEditorStyles.EndSection();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            // Auto-refresh toggle
            _autoRefresh = GUILayout.Toggle(_autoRefresh, "Auto Refresh", EditorStyles.toolbarButton);

            // Refresh interval
            GUILayout.Label("Interval:", EditorStyles.miniLabel);
            _refreshInterval = EditorGUILayout.Slider(_refreshInterval, 0.1f, 2f, GUILayout.Width(100));

            GUILayout.FlexibleSpace();

            // Filters
            _showInactiveComponents = GUILayout.Toggle(_showInactiveComponents, "Show Inactive", EditorStyles.toolbarButton);
            _showOnlyProblematic = GUILayout.Toggle(_showOnlyProblematic, "Problems Only", EditorStyles.toolbarButton);

            // Search
            GUILayout.Label("Search:", EditorStyles.miniLabel);
            _searchFilter = GUILayout.TextField(_searchFilter, EditorStyles.toolbarTextField, GUILayout.Width(150));

            EditorGUILayout.EndHorizontal();
        }

        private void DrawTabs()
        {
            _selectedTab = GUILayout.Toolbar(_selectedTab, _tabNames, USpringEditorStyles.TabButtonStyle);
        }

        private void DrawOverviewTab()
        {
            USpringEditorStyles.BeginSection();
            USpringEditorStyles.DrawSubHeader("Spring Components Overview");

            var filteredComponents = GetFilteredComponents();

            foreach (var component in filteredComponents)
            {
                DrawSpringComponentOverview(component);
            }

            if (filteredComponents.Count == 0)
            {
                EditorGUILayout.HelpBox("No spring components found matching the current filters.", MessageType.Info);
            }

            USpringEditorStyles.EndSection();
        }

        private void DrawPerformanceTab()
        {
            USpringEditorStyles.BeginSection();
            USpringEditorStyles.DrawSubHeader("Performance Analysis");

            // Performance summary
            var activeComponents = _springComponents.Where(s => s.IsActive).ToList();
            var problematicComponents = activeComponents.Where(s => s.HasPerformanceIssues).ToList();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Active Springs:", activeComponents.Count.ToString());
            EditorGUILayout.LabelField("Performance Issues:", problematicComponents.Count.ToString());
            EditorGUILayout.LabelField("Estimated CPU Impact:", GetEstimatedCPUImpact(activeComponents));
            EditorGUILayout.EndHorizontal();

            USpringEditorStyles.DrawHorizontalLine(Color.gray);

            // Performance issues
            if (problematicComponents.Any())
            {
                USpringEditorStyles.DrawSubHeader("Performance Issues");
                foreach (var component in problematicComponents)
                {
                    DrawPerformanceIssue(component);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No performance issues detected!", MessageType.Info);
            }

            USpringEditorStyles.EndSection();
        }

        private void DrawOptimizationTab()
        {
            USpringEditorStyles.BeginSection();
            USpringEditorStyles.DrawSubHeader("Optimization Suggestions");

            var suggestions = GenerateOptimizationSuggestions();

            if (suggestions.Any())
            {
                foreach (var suggestion in suggestions)
                {
                    DrawOptimizationSuggestion(suggestion);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No optimization suggestions at this time. Your springs are well configured!", MessageType.Info);
            }

            USpringEditorStyles.EndSection();
        }

        private void DrawDebuggingTab()
        {
            USpringEditorStyles.BeginSection();
            USpringEditorStyles.DrawSubHeader("Debugging Tools");

            // Global spring controls
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Pause All Springs", USpringEditorStyles.ButtonStyle))
            {
                PauseAllSprings();
            }
            if (GUILayout.Button("Resume All Springs", USpringEditorStyles.ButtonStyle))
            {
                ResumeAllSprings();
            }
            if (GUILayout.Button("Reset All Springs", USpringEditorStyles.ButtonStyle))
            {
                ResetAllSprings();
            }
            EditorGUILayout.EndHorizontal();

            USpringEditorStyles.DrawHorizontalLine(Color.gray);

            // Individual spring debugging
            var activeComponents = _springComponents.Where(s => s.IsActive).ToList();
            foreach (var component in activeComponents)
            {
                DrawSpringDebugInfo(component);
            }

            USpringEditorStyles.EndSection();
        }

        private void DrawSpringComponentOverview(SpringComponentInfo info)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);

            // Status indicator
            Color statusColor = info.IsActive ? (info.HasPerformanceIssues ? Color.yellow : Color.green) : Color.gray;
            Rect colorRect = GUILayoutUtility.GetRect(4, 20, GUILayout.ExpandHeight(false));
            EditorGUI.DrawRect(colorRect, statusColor);

            // Component info
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(info.Name, EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Type: {info.SpringType} | GameObject: {info.GameObjectName}");
            if (info.HasPerformanceIssues)
            {
                EditorGUILayout.LabelField("⚠️ Performance Issues Detected", USpringEditorStyles.SubHeaderStyle);
            }
            EditorGUILayout.EndVertical();

            // Actions
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            if (GUILayout.Button("Select", USpringEditorStyles.ButtonStyle))
            {
                Selection.activeGameObject = info.GameObject;
            }
            if (info.IsActive && GUILayout.Button("Debug", USpringEditorStyles.ButtonStyle))
            {
                // Focus on debugging tab and highlight this component
                _selectedTab = 3;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawPerformanceIssue(SpringComponentInfo info)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField($"⚠️ {info.Name}", EditorStyles.boldLabel);

            foreach (var issue in info.PerformanceIssues)
            {
                EditorGUILayout.LabelField($"• {issue}", USpringEditorStyles.SubHeaderStyle);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select Component", USpringEditorStyles.ButtonStyle))
            {
                Selection.activeGameObject = info.GameObject;
            }
            if (GUILayout.Button("Auto-Fix", USpringEditorStyles.ButtonStyle))
            {
                AutoFixPerformanceIssues(info);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void DrawOptimizationSuggestion(OptimizationSuggestion suggestion)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField($"💡 {suggestion.Title}", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(suggestion.Description, EditorStyles.wordWrappedLabel);

            if (suggestion.CanAutoApply)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Apply Suggestion", USpringEditorStyles.ButtonStyle))
                {
                    suggestion.ApplyAction?.Invoke();
                    RefreshSpringComponents();
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawSpringDebugInfo(SpringComponentInfo info)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField(info.Name, EditorStyles.boldLabel);

            // Real-time spring values would go here
            EditorGUILayout.LabelField($"Current Value: {info.CurrentValue}");
            EditorGUILayout.LabelField($"Target Value: {info.TargetValue}");
            EditorGUILayout.LabelField($"Velocity: {info.Velocity}");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", USpringEditorStyles.ButtonStyle))
            {
                // Reset this specific spring
            }
            if (GUILayout.Button("Nudge", USpringEditorStyles.ButtonStyle))
            {
                // Apply a small impulse to the spring
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private List<SpringComponentInfo> GetFilteredComponents()
        {
            var filtered = _springComponents.AsEnumerable();

            if (!_showInactiveComponents)
                filtered = filtered.Where(s => s.IsActive);

            if (_showOnlyProblematic)
                filtered = filtered.Where(s => s.HasPerformanceIssues);

            if (!string.IsNullOrEmpty(_searchFilter))
                filtered = filtered.Where(s => s.Name.ToLower().Contains(_searchFilter.ToLower()) ||
                                              s.GameObjectName.ToLower().Contains(_searchFilter.ToLower()));

            return filtered.ToList();
        }

        private void RefreshSpringComponents()
        {
            _springComponents.Clear();
            _lastRefreshTime = EditorApplication.timeSinceStartup;

            // Find all spring components in the scene
            var allComponents = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            foreach (var component in allComponents)
            {
                var springInfo = AnalyzeComponent(component);
                if (springInfo != null)
                {
                    _springComponents.Add(springInfo);
                }
            }
        }

        private SpringComponentInfo AnalyzeComponent(MonoBehaviour component)
        {
            // Analyze component for spring-related fields and properties
            var fields = component.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (IsSpringType(field.FieldType))
                {
                    return new SpringComponentInfo
                    {
                        Name = component.GetType().Name,
                        GameObject = component.gameObject,
                        GameObjectName = component.gameObject.name,
                        SpringType = field.FieldType.Name,
                        IsActive = component.gameObject.activeInHierarchy && component.enabled,
                        Component = component,
                        PerformanceIssues = AnalyzePerformanceIssues(component, field),
                        CurrentValue = "N/A", // Would need reflection to get actual values
                        TargetValue = "N/A",
                        Velocity = "N/A"
                    };
                }
            }

            return null;
        }

        private bool IsSpringType(System.Type type)
        {
            return type == typeof(SpringFloat) || type == typeof(SpringVector2) ||
                   type == typeof(SpringVector3) || type == typeof(SpringVector4) ||
                   type == typeof(SpringColor);
        }

        private List<string> AnalyzePerformanceIssues(MonoBehaviour component, System.Reflection.FieldInfo field)
        {
            var issues = new List<string>();

            // Example performance analysis
            if (component.gameObject.activeInHierarchy)
            {
                // Check for common performance issues
                if (component.GetComponents<MonoBehaviour>().Length > 10)
                {
                    issues.Add("GameObject has many components (>10)");
                }

                // Add more performance checks here
            }

            return issues;
        }

        private string GetEstimatedCPUImpact(List<SpringComponentInfo> components)
        {
            // Simple estimation based on number of active springs
            int count = components.Count;
            if (count < 10) return "Low";
            if (count < 50) return "Medium";
            return "High";
        }

        private List<OptimizationSuggestion> GenerateOptimizationSuggestions()
        {
            var suggestions = new List<OptimizationSuggestion>();

            // Example suggestions
            var activeCount = _springComponents.Count(s => s.IsActive);
            if (activeCount > 50)
            {
                suggestions.Add(new OptimizationSuggestion
                {
                    Title = "High Spring Count",
                    Description = $"You have {activeCount} active springs. Consider object pooling or disabling springs when not visible.",
                    CanAutoApply = false
                });
            }

            return suggestions;
        }

        private void PauseAllSprings()
        {
            // Implementation to pause all springs
            Debug.Log("Pausing all springs...");
        }

        private void ResumeAllSprings()
        {
            // Implementation to resume all springs
            Debug.Log("Resuming all springs...");
        }

        private void ResetAllSprings()
        {
            // Implementation to reset all springs
            Debug.Log("Resetting all springs...");
        }

        private void AutoFixPerformanceIssues(SpringComponentInfo info)
        {
            // Implementation to automatically fix common performance issues
            Debug.Log($"Auto-fixing performance issues for {info.Name}...");
        }

        private class SpringComponentInfo
        {
            public string Name;
            public GameObject GameObject;
            public string GameObjectName;
            public string SpringType;
            public bool IsActive;
            public MonoBehaviour Component;
            public List<string> PerformanceIssues = new List<string>();
            public string CurrentValue;
            public string TargetValue;
            public string Velocity;

            public bool HasPerformanceIssues => PerformanceIssues.Any();
        }

        private class OptimizationSuggestion
        {
            public string Title;
            public string Description;
            public bool CanAutoApply;
            public System.Action ApplyAction;
        }
    }
}

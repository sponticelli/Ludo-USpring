using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace USpring.Enhanced
{
    /// <summary>
    /// Visual spring curve editor for intuitive parameter adjustment with real-time preview.
    /// </summary>
    public class SpringCurveEditor : EditorWindow
    {
        private float _force = 20f;
        private float _drag = 10f;
        private bool _useAnalyticalSolution = false;
        
        private Vector2 _scrollPosition;
        private Rect _curveRect;
        private bool _isDragging = false;
        private Vector2 _dragStartPos;
        private float _dragStartForce;
        private float _dragStartDrag;
        
        private List<Vector2> _curvePoints = new List<Vector2>();
        private float _animationTime = 0f;
        private bool _isAnimating = false;
        
        private SpringSimulation _simulation = new SpringSimulation();
        
        // Visual settings
        private Color _curveColor = Color.cyan;
        private Color _targetLineColor = Color.green;
        private Color _gridColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
        private float _curveThickness = 2f;
        private bool _showGrid = true;
        private bool _showTargetLine = true;
        private bool _showVelocityCurve = false;
        
        // Curve analysis
        private float _settlingTime = 0f;
        private float _overshoot = 0f;
        private float _frequency = 0f;
        private bool _isOverdamped = false;
        private bool _isCriticallyDamped = false;
        
        [MenuItem("Tools/USpring/Curve Editor", priority = 102)]
        public static void ShowWindow()
        {
            var window = GetWindow<SpringCurveEditor>("Spring Curve Editor");
            window.minSize = new Vector2(600, 500);
            window.Show();
        }
        
        private void OnEnable()
        {
            UpdateCurve();
            EditorApplication.update += OnEditorUpdate;
        }
        
        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }
        
        private void OnEditorUpdate()
        {
            if (_isAnimating)
            {
                _animationTime += Time.unscaledDeltaTime;
                Repaint();
            }
        }
        
        private void OnGUI()
        {
            DrawHeader();
            DrawParameterControls();
            DrawCurveArea();
            DrawAnalysisPanel();
            DrawControlButtons();
            
            HandleInput();
        }
        
        private void DrawHeader()
        {
            USpringEditorStyles.BeginSection();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Spring Curve Editor", USpringEditorStyles.HeaderStyle);
            GUILayout.FlexibleSpace();
            
            // Visual options
            _showGrid = GUILayout.Toggle(_showGrid, "Grid", EditorStyles.toolbarButton);
            _showTargetLine = GUILayout.Toggle(_showTargetLine, "Target", EditorStyles.toolbarButton);
            _showVelocityCurve = GUILayout.Toggle(_showVelocityCurve, "Velocity", EditorStyles.toolbarButton);
            
            EditorGUILayout.EndHorizontal();
            
            USpringEditorStyles.EndSection();
        }
        
        private void DrawParameterControls()
        {
            USpringEditorStyles.BeginSection();
            
            EditorGUILayout.BeginHorizontal();
            
            // Force control
            EditorGUILayout.BeginVertical(GUILayout.Width(200));
            EditorGUILayout.LabelField("Force (Stiffness)", EditorStyles.boldLabel);
            float newForce = EditorGUILayout.Slider(_force, 0.1f, 100f);
            if (newForce != _force)
            {
                _force = newForce;
                UpdateCurve();
            }
            EditorGUILayout.EndVertical();
            
            GUILayout.Space(20);
            
            // Drag control
            EditorGUILayout.BeginVertical(GUILayout.Width(200));
            EditorGUILayout.LabelField("Drag (Damping)", EditorStyles.boldLabel);
            float newDrag = EditorGUILayout.Slider(_drag, 0.1f, 50f);
            if (newDrag != _drag)
            {
                _drag = newDrag;
                UpdateCurve();
            }
            EditorGUILayout.EndVertical();
            
            GUILayout.FlexibleSpace();
            
            // Advanced options
            EditorGUILayout.BeginVertical(GUILayout.Width(150));
            _useAnalyticalSolution = EditorGUILayout.Toggle("Analytical Solution", _useAnalyticalSolution);
            
            // Curve color
            _curveColor = EditorGUILayout.ColorField("Curve Color", _curveColor);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
            
            USpringEditorStyles.EndSection();
        }
        
        private void DrawCurveArea()
        {
            USpringEditorStyles.BeginSection();
            
            // Curve area
            _curveRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, 
                GUILayout.ExpandWidth(true), GUILayout.Height(300));
            
            // Background
            EditorGUI.DrawRect(_curveRect, new Color(0.1f, 0.1f, 0.1f, 1f));
            
            // Grid
            if (_showGrid)
            {
                DrawGrid(_curveRect);
            }
            
            // Target line
            if (_showTargetLine)
            {
                DrawTargetLine(_curveRect);
            }
            
            // Spring curve
            DrawSpringCurve(_curveRect);
            
            // Velocity curve
            if (_showVelocityCurve)
            {
                DrawVelocityCurve(_curveRect);
            }
            
            // Animation marker
            if (_isAnimating)
            {
                DrawAnimationMarker(_curveRect);
            }
            
            // Interactive handles
            DrawInteractiveHandles(_curveRect);
            
            USpringEditorStyles.EndSection();
        }
        
        private void DrawAnalysisPanel()
        {
            USpringEditorStyles.BeginSection();
            
            USpringEditorStyles.DrawSubHeader("Curve Analysis");
            
            EditorGUILayout.BeginHorizontal();
            
            // Left column
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField($"Settling Time: {_settlingTime:F2}s");
            EditorGUILayout.LabelField($"Overshoot: {_overshoot:F1}%");
            EditorGUILayout.LabelField($"Natural Frequency: {_frequency:F2} Hz");
            EditorGUILayout.EndVertical();
            
            // Right column
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField($"Damping: {GetDampingType()}");
            EditorGUILayout.LabelField($"Damping Ratio: {CalculateDampingRatio():F3}");
            EditorGUILayout.LabelField($"Quality Factor: {CalculateQualityFactor():F2}");
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
            
            USpringEditorStyles.EndSection();
        }
        
        private void DrawControlButtons()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button(_isAnimating ? "Stop Animation" : "Play Animation", USpringEditorStyles.ButtonStyle))
            {
                ToggleAnimation();
            }
            
            if (GUILayout.Button("Reset Parameters", USpringEditorStyles.ButtonStyle))
            {
                ResetParameters();
            }
            
            if (GUILayout.Button("Copy to Clipboard", USpringEditorStyles.ButtonStyle))
            {
                CopyParametersToClipboard();
            }
            
            if (GUILayout.Button("Apply to Selection", USpringEditorStyles.ButtonStyle))
            {
                ApplyToSelection();
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawGrid(Rect rect)
        {
            Handles.color = _gridColor;
            
            // Vertical lines
            for (int i = 0; i <= 10; i++)
            {
                float x = rect.x + (rect.width / 10f) * i;
                Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.yMax, 0));
            }
            
            // Horizontal lines
            for (int i = 0; i <= 10; i++)
            {
                float y = rect.y + (rect.height / 10f) * i;
                Handles.DrawLine(new Vector3(rect.x, y, 0), new Vector3(rect.xMax, y, 0));
            }
        }
        
        private void DrawTargetLine(Rect rect)
        {
            Handles.color = _targetLineColor;
            float targetY = rect.y + rect.height * 0.2f; // Target at 80% from bottom
            Handles.DrawLine(new Vector3(rect.x, targetY, 0), new Vector3(rect.xMax, targetY, 0));
        }
        
        private void DrawSpringCurve(Rect rect)
        {
            if (_curvePoints.Count < 2) return;
            
            Vector3[] points = new Vector3[_curvePoints.Count];
            for (int i = 0; i < _curvePoints.Count; i++)
            {
                float x = rect.x + _curvePoints[i].x * rect.width;
                float y = rect.y + rect.height - (_curvePoints[i].y * rect.height);
                points[i] = new Vector3(x, y, 0);
            }
            
            Handles.color = _curveColor;
            Handles.DrawAAPolyLine(_curveThickness, points);
        }
        
        private void DrawVelocityCurve(Rect rect)
        {
            // Draw velocity curve in different color
            Handles.color = Color.yellow;
            
            // Calculate velocity points
            var velocityPoints = _simulation.GetVelocityPoints();
            if (velocityPoints.Count < 2) return;
            
            Vector3[] points = new Vector3[velocityPoints.Count];
            for (int i = 0; i < velocityPoints.Count; i++)
            {
                float x = rect.x + velocityPoints[i].x * rect.width;
                float y = rect.y + rect.height * 0.5f - (velocityPoints[i].y * rect.height * 0.3f); // Scale velocity
                points[i] = new Vector3(x, y, 0);
            }
            
            Handles.DrawAAPolyLine(1f, points);
        }
        
        private void DrawAnimationMarker(Rect rect)
        {
            float normalizedTime = (_animationTime % 4f) / 4f; // 4 second loop
            float x = rect.x + normalizedTime * rect.width;
            
            Handles.color = Color.red;
            Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.yMax, 0));
            
            // Draw current value marker
            float currentValue = _simulation.GetValueAtTime(_animationTime);
            float y = rect.y + rect.height - (currentValue * rect.height);
            
            Handles.DrawSolidDisc(new Vector3(x, y, 0), Vector3.forward, 3f);
        }
        
        private void DrawInteractiveHandles(Rect rect)
        {
            // Draw handles for interactive parameter adjustment
            // Force handle (affects curve steepness)
            Vector2 forceHandlePos = new Vector2(rect.x + rect.width * 0.2f, rect.y + rect.height * 0.8f);
            Handles.color = Color.blue;
            Handles.DrawSolidDisc(forceHandlePos, Vector3.forward, 5f);
            
            // Drag handle (affects curve damping)
            Vector2 dragHandlePos = new Vector2(rect.x + rect.width * 0.8f, rect.y + rect.height * 0.8f);
            Handles.color = Color.red;
            Handles.DrawSolidDisc(dragHandlePos, Vector3.forward, 5f);
        }
        
        private void HandleInput()
        {
            Event e = Event.current;
            
            if (e.type == EventType.MouseDown && _curveRect.Contains(e.mousePosition))
            {
                _isDragging = true;
                _dragStartPos = e.mousePosition;
                _dragStartForce = _force;
                _dragStartDrag = _drag;
                e.Use();
            }
            else if (e.type == EventType.MouseDrag && _isDragging)
            {
                Vector2 delta = e.mousePosition - _dragStartPos;
                
                // Adjust parameters based on mouse movement
                _force = Mathf.Clamp(_dragStartForce + delta.x * 0.1f, 0.1f, 100f);
                _drag = Mathf.Clamp(_dragStartDrag - delta.y * 0.05f, 0.1f, 50f);
                
                UpdateCurve();
                Repaint();
                e.Use();
            }
            else if (e.type == EventType.MouseUp)
            {
                _isDragging = false;
                e.Use();
            }
        }
        
        private void UpdateCurve()
        {
            _simulation.UpdateParameters(_force, _drag, _useAnalyticalSolution);
            _curvePoints = _simulation.GetCurvePoints();
            AnalyzeCurve();
        }
        
        private void AnalyzeCurve()
        {
            // Calculate curve characteristics
            _settlingTime = _simulation.CalculateSettlingTime();
            _overshoot = _simulation.CalculateOvershoot();
            _frequency = _simulation.CalculateNaturalFrequency();
            
            float dampingRatio = CalculateDampingRatio();
            _isOverdamped = dampingRatio > 1f;
            _isCriticallyDamped = Mathf.Approximately(dampingRatio, 1f);
        }
        
        private float CalculateDampingRatio()
        {
            // ζ = c / (2 * sqrt(k * m)), assuming m = 1
            return _drag / (2f * Mathf.Sqrt(_force));
        }
        
        private float CalculateQualityFactor()
        {
            float dampingRatio = CalculateDampingRatio();
            return dampingRatio > 0 ? 1f / (2f * dampingRatio) : float.PositiveInfinity;
        }
        
        private string GetDampingType()
        {
            float dampingRatio = CalculateDampingRatio();
            if (dampingRatio < 1f) return "Underdamped";
            if (dampingRatio > 1f) return "Overdamped";
            return "Critically Damped";
        }
        
        private void ToggleAnimation()
        {
            _isAnimating = !_isAnimating;
            if (_isAnimating)
            {
                _animationTime = 0f;
            }
        }
        
        private void ResetParameters()
        {
            _force = 20f;
            _drag = 10f;
            _useAnalyticalSolution = false;
            UpdateCurve();
        }
        
        private void CopyParametersToClipboard()
        {
            string parameters = $"Force: {_force:F2}, Drag: {_drag:F2}, Analytical: {_useAnalyticalSolution}";
            EditorGUIUtility.systemCopyBuffer = parameters;
            Debug.Log($"Copied to clipboard: {parameters}");
        }
        
        private void ApplyToSelection()
        {
            // Apply current parameters to selected GameObjects with spring components
            foreach (var obj in Selection.gameObjects)
            {
                var springComponents = obj.GetComponents<MonoBehaviour>();
                foreach (var component in springComponents)
                {
                    ApplyParametersToComponent(component);
                }
            }
        }
        
        private void ApplyParametersToComponent(MonoBehaviour component)
        {
            // Use reflection to apply parameters to spring fields
            var fields = component.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            foreach (var field in fields)
            {
                if (IsSpringType(field.FieldType))
                {
                    var spring = field.GetValue(component);
                    if (spring != null)
                    {
                        ApplyParametersToSpring(spring);
                    }
                }
            }
        }
        
        private bool IsSpringType(System.Type type)
        {
            return type.Name.StartsWith("Spring");
        }
        
        private void ApplyParametersToSpring(object spring)
        {
            var springType = spring.GetType();
            
            var forceField = springType.GetField("commonForce", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dragField = springType.GetField("commonDrag", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            forceField?.SetValue(spring, _force);
            dragField?.SetValue(spring, _drag);
        }
        
        private class SpringSimulation
        {
            private float _force = 20f;
            private float _drag = 10f;
            private bool _useAnalytical = false;
            private List<Vector2> _curvePoints = new List<Vector2>();
            private List<Vector2> _velocityPoints = new List<Vector2>();
            
            public void UpdateParameters(float force, float drag, bool useAnalytical)
            {
                _force = force;
                _drag = drag;
                _useAnalytical = useAnalytical;
                GenerateCurve();
            }
            
            public List<Vector2> GetCurvePoints() => _curvePoints;
            public List<Vector2> GetVelocityPoints() => _velocityPoints;
            
            private void GenerateCurve()
            {
                _curvePoints.Clear();
                _velocityPoints.Clear();
                
                int samples = 200;
                float duration = 4f;
                float current = 0f;
                float velocity = 0f;
                float target = 1f;
                
                for (int i = 0; i < samples; i++)
                {
                    float t = (float)i / (samples - 1) * duration;
                    float normalizedTime = t / duration;
                    
                    if (_useAnalytical)
                    {
                        // Analytical solution (simplified)
                        float dampingRatio = _drag / (2f * Mathf.Sqrt(_force));
                        float naturalFreq = Mathf.Sqrt(_force);
                        
                        if (dampingRatio < 1f)
                        {
                            // Underdamped
                            float dampedFreq = naturalFreq * Mathf.Sqrt(1f - dampingRatio * dampingRatio);
                            current = target * (1f - Mathf.Exp(-dampingRatio * naturalFreq * t) * 
                                     (Mathf.Cos(dampedFreq * t) + (dampingRatio * naturalFreq / dampedFreq) * Mathf.Sin(dampedFreq * t)));
                        }
                        else
                        {
                            // Overdamped or critically damped
                            current = target * (1f - Mathf.Exp(-naturalFreq * t) * (1f + naturalFreq * t));
                        }
                    }
                    else
                    {
                        // Numerical integration
                        float deltaTime = duration / samples;
                        float springForce = (target - current) * _force;
                        float dampingForce = velocity * _drag;
                        float acceleration = springForce - dampingForce;
                        
                        velocity += acceleration * deltaTime;
                        current += velocity * deltaTime;
                    }
                    
                    _curvePoints.Add(new Vector2(normalizedTime, current));
                    _velocityPoints.Add(new Vector2(normalizedTime, velocity));
                }
            }
            
            public float GetValueAtTime(float time)
            {
                float normalizedTime = (time % 4f) / 4f;
                int index = Mathf.FloorToInt(normalizedTime * (_curvePoints.Count - 1));
                index = Mathf.Clamp(index, 0, _curvePoints.Count - 1);
                return _curvePoints[index].y;
            }
            
            public float CalculateSettlingTime()
            {
                // Find when curve stays within 2% of target
                float tolerance = 0.02f;
                for (int i = _curvePoints.Count - 1; i >= 0; i--)
                {
                    if (Mathf.Abs(_curvePoints[i].y - 1f) > tolerance)
                    {
                        return _curvePoints[i].x * 4f; // Convert back to actual time
                    }
                }
                return 0f;
            }
            
            public float CalculateOvershoot()
            {
                float maxValue = 0f;
                foreach (var point in _curvePoints)
                {
                    maxValue = Mathf.Max(maxValue, point.y);
                }
                return Mathf.Max(0f, (maxValue - 1f) * 100f);
            }
            
            public float CalculateNaturalFrequency()
            {
                return Mathf.Sqrt(_force) / (2f * Mathf.PI);
            }
        }
    }
}

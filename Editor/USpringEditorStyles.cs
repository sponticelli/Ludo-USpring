using UnityEditor;
using UnityEngine;

namespace USpring
{
    /// <summary>
    /// Centralized style system for USpring custom editors.
    /// Provides consistent styling, colors, and layout options across all USpring editors.
    /// </summary>
    public static class USpringEditorStyles
    {
        // Color palette
        public static readonly Color PrimaryColor = new Color(0.56f, 0.33f, 1f, 1f);
        public static readonly Color SecondaryColor = new Color(0.35f, 0.35f, 0.35f, 1f);
        public static readonly Color AccentColor = new Color(0.8f, 0.4f, 0.2f, 1f);
        public static readonly Color WarningColor = new Color(1f, 0.7f, 0.2f, 1f);
        public static readonly Color ErrorColor = new Color(1f, 0.3f, 0.3f, 1f);
        
        // Background colors
        public static readonly Color HeaderBackgroundColor = new Color(0.25f, 0.25f, 0.25f, 1f);
        public static readonly Color SectionBackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        
        // Text colors
        public static readonly Color HeaderTextColor = Color.white;
        public static readonly Color SubHeaderTextColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        
        // Spring type colors
        public static readonly Color FloatSpringColor = new Color(0.4f, 0.8f, 0.9f, 1f);
        public static readonly Color Vector2SpringColor = new Color(0.4f, 0.9f, 0.4f, 1f);
        public static readonly Color Vector3SpringColor = new Color(0.9f, 0.4f, 0.4f, 1f);
        public static readonly Color Vector4SpringColor = new Color(0.9f, 0.9f, 0.4f, 1f);
        public static readonly Color ColorSpringColor = new Color(0.9f, 0.4f, 0.9f, 1f);
        public static readonly Color RotationSpringColor = new Color(0.4f, 0.4f, 0.9f, 1f);
        
        // Dimensions
        public const float StandardPadding = 4f;
        public const float HeaderHeight = 24f;
        public const float SubHeaderHeight = 20f;
        public const float StandardElementHeight = 18f;
        public const float SpaceBetweenSections = 8f;
        public const float SpaceBetweenElements = 2f;
        public const float IndentWidth = 15f;
        
        // Default label width
        public const float DefaultLabelWidth = 140f;
        
        // Styles cache
        private static GUIStyle _headerStyle;
        private static GUIStyle _subHeaderStyle;
        private static GUIStyle _sectionStyle;
        private static GUIStyle _buttonStyle;
        private static GUIStyle _toggleStyle;
        private static GUIStyle _helpBoxStyle;
        private static GUIStyle _previewLabelStyle;
        private static GUIStyle _presetButtonStyle;
        private static GUIStyle _tabButtonStyle;
        
        /// <summary>
        /// Style for section headers
        /// </summary>
        public static GUIStyle HeaderStyle
        {
            get
            {
                if (_headerStyle == null)
                {
                    _headerStyle = new GUIStyle(EditorStyles.boldLabel);
                    _headerStyle.normal.textColor = HeaderTextColor;
                    _headerStyle.fontSize = 12;
                    _headerStyle.alignment = TextAnchor.MiddleLeft;
                    _headerStyle.padding = new RectOffset(8, 8, 4, 4);
                }
                return _headerStyle;
            }
        }
        
        /// <summary>
        /// Style for sub-headers
        /// </summary>
        public static GUIStyle SubHeaderStyle
        {
            get
            {
                if (_subHeaderStyle == null)
                {
                    _subHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
                    _subHeaderStyle.normal.textColor = SubHeaderTextColor;
                    _subHeaderStyle.fontSize = 11;
                    _subHeaderStyle.alignment = TextAnchor.MiddleLeft;
                    _subHeaderStyle.padding = new RectOffset(4, 4, 2, 2);
                }
                return _subHeaderStyle;
            }
        }
        
        /// <summary>
        /// Style for section backgrounds
        /// </summary>
        public static GUIStyle SectionStyle
        {
            get
            {
                if (_sectionStyle == null)
                {
                    _sectionStyle = new GUIStyle(GUI.skin.box);
                    _sectionStyle.padding = new RectOffset(8, 8, 8, 8);
                    _sectionStyle.margin = new RectOffset(0, 0, 4, 4);
                }
                return _sectionStyle;
            }
        }
        
        /// <summary>
        /// Style for buttons
        /// </summary>
        public static GUIStyle ButtonStyle
        {
            get
            {
                if (_buttonStyle == null)
                {
                    _buttonStyle = new GUIStyle(GUI.skin.button);
                    _buttonStyle.padding = new RectOffset(8, 8, 4, 4);
                    _buttonStyle.margin = new RectOffset(2, 2, 2, 2);
                }
                return _buttonStyle;
            }
        }
        
        /// <summary>
        /// Style for toggles
        /// </summary>
        public static GUIStyle ToggleStyle
        {
            get
            {
                if (_toggleStyle == null)
                {
                    _toggleStyle = new GUIStyle(EditorStyles.toggle);
                    _toggleStyle.margin = new RectOffset(2, 2, 2, 2);
                }
                return _toggleStyle;
            }
        }
        
        /// <summary>
        /// Style for help boxes
        /// </summary>
        public static GUIStyle HelpBoxStyle
        {
            get
            {
                if (_helpBoxStyle == null)
                {
                    _helpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                    _helpBoxStyle.padding = new RectOffset(8, 8, 8, 8);
                    _helpBoxStyle.margin = new RectOffset(0, 0, 4, 4);
                }
                return _helpBoxStyle;
            }
        }
        
        /// <summary>
        /// Style for preview labels
        /// </summary>
        public static GUIStyle PreviewLabelStyle
        {
            get
            {
                if (_previewLabelStyle == null)
                {
                    _previewLabelStyle = new GUIStyle(EditorStyles.label);
                    _previewLabelStyle.normal.textColor = SubHeaderTextColor;
                    _previewLabelStyle.fontSize = 10;
                    _previewLabelStyle.alignment = TextAnchor.MiddleCenter;
                }
                return _previewLabelStyle;
            }
        }
        
        /// <summary>
        /// Style for preset buttons
        /// </summary>
        public static GUIStyle PresetButtonStyle
        {
            get
            {
                if (_presetButtonStyle == null)
                {
                    _presetButtonStyle = new GUIStyle(GUI.skin.button);
                    _presetButtonStyle.padding = new RectOffset(4, 4, 2, 2);
                    _presetButtonStyle.margin = new RectOffset(2, 2, 2, 2);
                    _presetButtonStyle.fontSize = 10;
                }
                return _presetButtonStyle;
            }
        }
        
        /// <summary>
        /// Style for tab buttons
        /// </summary>
        public static GUIStyle TabButtonStyle
        {
            get
            {
                if (_tabButtonStyle == null)
                {
                    _tabButtonStyle = new GUIStyle(GUI.skin.button);
                    _tabButtonStyle.padding = new RectOffset(8, 8, 4, 4);
                    _tabButtonStyle.margin = new RectOffset(0, 0, 0, 0);
                    _tabButtonStyle.fixedHeight = 24;
                }
                return _tabButtonStyle;
            }
        }
        
        /// <summary>
        /// Draws a header with the specified text and color
        /// </summary>
        public static bool DrawHeader(string text, bool foldout, Color color)
        {
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, HeaderStyle, GUILayout.Height(HeaderHeight));
            
            // Draw background
            EditorGUI.DrawRect(rect, color);
            
            // Draw foldout
            Rect foldoutRect = rect;
            foldoutRect.width = 20;
            bool newFoldout = EditorGUI.Foldout(foldoutRect, foldout, GUIContent.none, true);
            
            // Draw label
            Rect labelRect = rect;
            labelRect.x += 20;
            labelRect.width -= 20;
            GUI.Label(labelRect, text, HeaderStyle);
            
            return newFoldout;
        }
        
        /// <summary>
        /// Draws a sub-header with the specified text
        /// </summary>
        public static void DrawSubHeader(string text)
        {
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, SubHeaderStyle, GUILayout.Height(SubHeaderHeight));
            GUI.Label(rect, text, SubHeaderStyle);
        }
        
        /// <summary>
        /// Begins a section with a background
        /// </summary>
        public static void BeginSection()
        {
            EditorGUILayout.BeginVertical(SectionStyle);
        }
        
        /// <summary>
        /// Ends a section
        /// </summary>
        public static void EndSection()
        {
            EditorGUILayout.EndVertical();
            GUILayout.Space(SpaceBetweenSections);
        }
        
        /// <summary>
        /// Draws a help box with the specified message and type
        /// </summary>
        public static void DrawHelpBox(string message, MessageType type)
        {
            EditorGUILayout.HelpBox(message, type);
        }
        
        /// <summary>
        /// Draws a horizontal line
        /// </summary>
        public static void DrawHorizontalLine(Color color, float height = 1f, float padding = 4f)
        {
            GUILayout.Space(padding);
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(height));
            EditorGUI.DrawRect(rect, color);
            GUILayout.Space(padding);
        }
    }
}

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace USpring
{
    /// <summary>
    /// Utility class for drawing spring previews in the inspector.
    /// Provides methods for visualizing spring motion and behavior.
    /// </summary>
    public static class USpringPreviewUtility
    {
        // Constants for preview rendering
        private const float PreviewHeight = 80f;
        private const float TimeScale = 2f;
        private const float GridLineAlpha = 0.2f;
        private const int GridLines = 4;
        private const float AnimationDuration = 3f;
        
        // Cache for preview textures
        private static Dictionary<int, Texture2D> _previewTextures = new Dictionary<int, Texture2D>();
        
        /// <summary>
        /// Draws a preview of a spring's motion over time
        /// </summary>
        /// <param name="force">Spring force</param>
        /// <param name="drag">Spring drag</param>
        /// <param name="color">Color to use for the preview curve</param>
        /// <param name="label">Optional label for the preview</param>
        public static void DrawSpringPreview(float force, float drag, Color color, string label = "")
        {
            // Calculate a unique ID for this spring configuration
            int previewId = Mathf.RoundToInt((force * 100) + (drag * 10));
            
            // Get or create the preview texture
            Texture2D previewTexture = GetPreviewTexture(previewId, force, drag, color);
            
            // Draw the preview
            EditorGUILayout.Space(4);
            
            // Draw label if provided
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, USpringEditorStyles.PreviewLabelStyle);
            }
            
            // Draw the preview texture
            Rect previewRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(PreviewHeight));
            GUI.DrawTexture(previewRect, previewTexture);
            
            // Draw time markers
            DrawTimeMarkers(previewRect);
            
            EditorGUILayout.Space(4);
        }
        
        /// <summary>
        /// Draws a preview of a spring's motion with multiple components (like Vector2/3/4)
        /// </summary>
        /// <param name="forces">Array of forces for each component</param>
        /// <param name="drags">Array of drags for each component</param>
        /// <param name="colors">Array of colors for each component</param>
        /// <param name="labels">Array of labels for each component</param>
        public static void DrawMultiComponentSpringPreview(float[] forces, float[] drags, Color[] colors, string[] labels = null)
        {
            if (forces.Length != drags.Length || forces.Length != colors.Length)
            {
                Debug.LogError("Forces, drags, and colors arrays must have the same length");
                return;
            }
            
            // Draw the preview
            EditorGUILayout.Space(4);
            
            // Create the preview rect
            Rect previewRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(PreviewHeight));
            
            // Draw background and grid
            DrawPreviewBackground(previewRect);
            
            // Draw each component
            for (int i = 0; i < forces.Length; i++)
            {
                DrawSpringCurve(previewRect, forces[i], drags[i], colors[i]);
            }
            
            // Draw time markers
            DrawTimeMarkers(previewRect);
            
            // Draw legend
            if (labels != null && labels.Length > 0)
            {
                DrawLegend(previewRect, colors, labels);
            }
            
            EditorGUILayout.Space(4);
        }
        
        /// <summary>
        /// Gets or creates a preview texture for the given spring configuration
        /// </summary>
        private static Texture2D GetPreviewTexture(int previewId, float force, float drag, Color color)
        {
            // Check if we already have a texture for this configuration
            if (_previewTextures.TryGetValue(previewId, out Texture2D texture))
            {
                return texture;
            }
            
            // Create a new texture
            int width = 512;
            int height = 128;
            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            
            // Clear the texture
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color(0.15f, 0.15f, 0.15f, 1f);
            }
            texture.SetPixels(pixels);
            
            // Draw grid lines
            DrawGridLines(texture, width, height);
            
            // Draw the spring curve
            DrawSpringCurveToTexture(texture, width, height, force, drag, color);
            
            // Apply changes
            texture.Apply();
            
            // Cache the texture
            _previewTextures[previewId] = texture;
            
            return texture;
        }
        
        /// <summary>
        /// Draws grid lines on the texture
        /// </summary>
        private static void DrawGridLines(Texture2D texture, int width, int height)
        {
            // Draw horizontal center line
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, height / 2, new Color(1f, 1f, 1f, GridLineAlpha));
            }
            
            // Draw vertical grid lines
            for (int i = 1; i < GridLines; i++)
            {
                int x = (width * i) / GridLines;
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, GridLineAlpha * 0.5f));
                }
            }
        }
        
        /// <summary>
        /// Draws a spring curve on the texture
        /// </summary>
        private static void DrawSpringCurveToTexture(Texture2D texture, int width, int height, float force, float drag, Color color)
        {
            float value = 0f;
            float velocity = 0f;
            float target = 1f;
            float halfHeight = height / 2f;
            
            // Draw the curve
            int prevX = 0;
            int prevY = Mathf.RoundToInt(halfHeight);
            
            for (int x = 1; x < width; x++)
            {
                float t = (float)x / width * AnimationDuration;
                float deltaTime = t - ((float)(x - 1) / width * AnimationDuration);
                
                // Update spring physics
                float deltaValue = velocity * deltaTime;
                float acceleration = (target - value) * force - velocity * drag;
                float deltaVelocity = acceleration * deltaTime;
                
                value += deltaValue;
                velocity += deltaVelocity;
                
                // Calculate pixel position
                int y = Mathf.RoundToInt(halfHeight - (value * halfHeight * 0.8f));
                y = Mathf.Clamp(y, 0, height - 1);
                
                // Draw line from previous point
                DrawLine(texture, prevX, prevY, x, y, color);
                
                prevX = x;
                prevY = y;
            }
        }
        
        /// <summary>
        /// Draws a line between two points on the texture
        /// </summary>
        private static void DrawLine(Texture2D texture, int x0, int y0, int x1, int y1, Color color)
        {
            int dx = Mathf.Abs(x1 - x0);
            int dy = Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;
            
            while (true)
            {
                texture.SetPixel(x0, y0, color);
                
                if (x0 == x1 && y0 == y1) break;
                
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }
        
        /// <summary>
        /// Draws the preview background and grid
        /// </summary>
        private static void DrawPreviewBackground(Rect rect)
        {
            // Draw background
            EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1f));
            
            // Draw center line
            Rect centerLineRect = new Rect(rect.x, rect.y + rect.height / 2, rect.width, 1);
            EditorGUI.DrawRect(centerLineRect, new Color(1f, 1f, 1f, GridLineAlpha));
            
            // Draw vertical grid lines
            for (int i = 1; i < GridLines; i++)
            {
                float x = rect.x + (rect.width * i) / GridLines;
                Rect gridLineRect = new Rect(x, rect.y, 1, rect.height);
                EditorGUI.DrawRect(gridLineRect, new Color(1f, 1f, 1f, GridLineAlpha * 0.5f));
            }
        }
        
        /// <summary>
        /// Draws a spring curve directly on the GUI
        /// </summary>
        private static void DrawSpringCurve(Rect rect, float force, float drag, Color color)
        {
            float value = 0f;
            float velocity = 0f;
            float target = 1f;
            
            // Calculate points
            Vector3[] points = new Vector3[60];
            for (int i = 0; i < points.Length; i++)
            {
                float t = (float)i / (points.Length - 1) * AnimationDuration;
                float deltaTime = i > 0 ? t - ((float)(i - 1) / (points.Length - 1) * AnimationDuration) : 0;
                
                // Update spring physics
                float deltaValue = velocity * deltaTime;
                float acceleration = (target - value) * force - velocity * drag;
                float deltaVelocity = acceleration * deltaTime;
                
                value += deltaValue;
                velocity += deltaVelocity;
                
                // Calculate point position
                float x = rect.x + (t / AnimationDuration) * rect.width;
                float y = rect.y + rect.height / 2 - (value * rect.height * 0.4f);
                
                points[i] = new Vector3(x, y, 0);
            }
            
            // Draw the curve
            Handles.color = color;
            Handles.DrawAAPolyLine(2f, points);
        }
        
        /// <summary>
        /// Draws time markers on the preview
        /// </summary>
        private static void DrawTimeMarkers(Rect rect)
        {
            // Draw time markers
            for (int i = 0; i <= GridLines; i++)
            {
                float x = rect.x + (rect.width * i) / GridLines;
                float time = (float)i / GridLines * AnimationDuration;
                
                // Draw time label
                Rect labelRect = new Rect(x - 15, rect.y + rect.height - 15, 30, 15);
                GUI.Label(labelRect, time.ToString("F1") + "s", USpringEditorStyles.PreviewLabelStyle);
            }
        }
        
        /// <summary>
        /// Draws a legend for the preview
        /// </summary>
        private static void DrawLegend(Rect rect, Color[] colors, string[] labels)
        {
            if (colors.Length != labels.Length) return;
            
            float legendWidth = 60;
            float legendHeight = 15;
            float spacing = 5;
            float totalWidth = 0;
            
            // Calculate total width
            for (int i = 0; i < labels.Length; i++)
            {
                totalWidth += legendWidth + spacing;
            }
            
            // Draw legend items
            float x = rect.x + (rect.width - totalWidth) / 2;
            float y = rect.y + 5;
            
            for (int i = 0; i < labels.Length; i++)
            {
                // Draw color swatch
                Rect swatchRect = new Rect(x, y, 10, 10);
                EditorGUI.DrawRect(swatchRect, colors[i]);
                
                // Draw label
                Rect labelRect = new Rect(x + 15, y - 2, legendWidth - 15, legendHeight);
                GUI.Label(labelRect, labels[i], USpringEditorStyles.PreviewLabelStyle);
                
                x += legendWidth + spacing;
            }
        }
    }
}

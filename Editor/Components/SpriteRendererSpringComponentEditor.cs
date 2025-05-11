using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
    [CustomEditor(typeof(SpriteRendererSpringComponent))]
    [CanEditMultipleObjects]
    public class SpriteRendererSpringComponentEditor : SpringComponentCustomEditor
    {
        private SerializedProperty spAutoUpdatedSpriteRenderer;
        private SerializedProperty spUpdateSize;
        private SerializedProperty spAnimateFlipX;
        private SerializedProperty spAnimateFlipY;
        private SerializedProperty spFlipThreshold;

        private SpringColorDrawer colorSpringDrawer;
        private SpringVector2Drawer sizeSpringDrawer;

        protected override void RefreshSerializedProperties()
        {
            base.RefreshSerializedProperties();

            spAutoUpdatedSpriteRenderer = serializedObject.FindProperty("autoUpdatedSpriteRenderer");
            spUpdateSize = serializedObject.FindProperty("updateSize");
            spAnimateFlipX = serializedObject.FindProperty("animateFlipX");
            spAnimateFlipY = serializedObject.FindProperty("animateFlipY");
            spFlipThreshold = serializedObject.FindProperty("flipThreshold");
        }

        protected override void CreateDrawers()
        {
            colorSpringDrawer = new SpringColorDrawer(serializedObject.FindProperty("colorSpring"), false, false);
            sizeSpringDrawer = new SpringVector2Drawer(serializedObject.FindProperty("sizeSpring"), false, false);
        }

        protected override void DrawSprings()
        {
            // Draw presets dropdown
            USpringPresets.DrawPresetDropdown(preset => {
                serializedObject.Update();

                // Get the force and drag properties from the springs
                SerializedProperty colorForceProperty = colorSpringDrawer.SpringEditorObject.SpCommonForce;
                SerializedProperty colorDragProperty = colorSpringDrawer.SpringEditorObject.SpCommonDrag;
                SerializedProperty sizeForceProperty = sizeSpringDrawer.SpringEditorObject.SpCommonForce;
                SerializedProperty sizeDragProperty = sizeSpringDrawer.SpringEditorObject.SpCommonDrag;
                SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

                // Apply the preset to the color spring
                USpringPresets.ApplyPreset(serializedObject, preset, colorForceProperty, colorDragProperty, analyticalSolutionProperty);

                // Apply the preset to the size spring if enabled
                if (spUpdateSize.boolValue)
                {
                    USpringPresets.ApplyPreset(serializedObject, preset, sizeForceProperty, sizeDragProperty, null);
                }

                serializedObject.ApplyModifiedProperties();
            });

            // Draw the springs
            DrawSpring("Color Spring", colorSpringDrawer);

            if (spUpdateSize.boolValue)
            {
                DrawSpring("Size Spring", sizeSpringDrawer);
            }
        }

        private void DrawSpring(string label, SpringDrawer springDrawer)
        {
            bool unfolded = springDrawer.SpringEditorObject.Unfolded;
            springDrawer.SpringEditorObject.Unfolded = DrawRectangleArea(label, unfolded);

            if (!springDrawer.SpringEditorObject.Unfolded) return;
            Rect propertyRect = EditorGUILayout.GetControlRect(false, springDrawer.GetPropertyHeight());
            springDrawer.OnGUI(propertyRect);
        }

        /// <summary>
        /// Draws a preview of the sprite renderer spring behavior
        /// </summary>
        protected override void DrawSpringPreview()
        {
            // Get the spring parameters
            float colorForce = colorSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
            float colorDrag = colorSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

            // Draw the color spring preview
            USpringEditorStyles.DrawSubHeader("Color Spring Preview");
            USpringPreviewUtility.DrawSpringPreview(colorForce, colorDrag, USpringEditorStyles.ColorSpringColor, "Color");

            // Draw the size spring preview if enabled
            if (spUpdateSize.boolValue)
            {
                float sizeForce = sizeSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float sizeDrag = sizeSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

                EditorGUILayout.Space(8);
                USpringEditorStyles.DrawSubHeader("Size Spring Preview");
                float[] forces = new float[] { sizeForce, sizeForce };
                float[] drags = new float[] { sizeDrag, sizeDrag };
                Color[] colors = new Color[] {
                    USpringEditorStyles.Vector2SpringColor, // X
                    USpringEditorStyles.Vector2SpringColor  // Y
                };
                string[] labels = new string[] { "Width", "Height" };

                USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);
            }

            // Draw a visualization of sprite renderer
            EditorGUILayout.Space(8);
            USpringEditorStyles.DrawSubHeader("Sprite Visualization");

            // Draw a simple sprite visualization
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(200));

            // Draw background
            EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1f));

            // Draw initial sprite
            float spriteSize = 80f;
            float initialX = rect.x + rect.width * 0.25f;
            float initialY = rect.y + rect.height * 0.5f;
            Color initialColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);
            DrawSprite(initialX, initialY, spriteSize, spriteSize, initialColor, false, false);

            // Draw target sprite
            float targetX = rect.x + rect.width * 0.75f;
            float targetY = rect.y + rect.height * 0.5f;
            Color targetColor = new Color(0.4f, 0.8f, 1f, 0.7f);
            bool flipX = spAnimateFlipX.boolValue;
            bool flipY = spAnimateFlipY.boolValue;
            DrawSprite(targetX, targetY, spriteSize * 1.2f, spriteSize * 1.2f, targetColor, flipX, flipY);

            // Draw labels
            EditorGUI.LabelField(new Rect(initialX - 40, initialY - spriteSize/2 - 20, 80, 20), "Initial");
            EditorGUI.LabelField(new Rect(targetX - 40, targetY - spriteSize/2 - 20, 80, 20), "Target");

            // Draw arrow
            DrawSpriteArrow(initialX, initialY, targetX, targetY, new Color(0.4f, 0.8f, 0.4f, 0.8f));
        }

        /// <summary>
        /// Draws a sprite representation
        /// </summary>
        private void DrawSprite(float x, float y, float width, float height, Color color, bool flipX, bool flipY)
        {
            // Calculate the actual width and height based on flips
            float actualWidth = flipX ? -width : width;
            float actualHeight = flipY ? -height : height;

            // Draw the sprite
            Rect spriteRect = new Rect(
                x - Mathf.Abs(actualWidth) / 2,
                y - Mathf.Abs(actualHeight) / 2,
                actualWidth,
                actualHeight
            );

            // Draw the sprite background
            EditorGUI.DrawRect(spriteRect, color);

            // Draw outline
            Handles.color = new Color(color.r, color.g, color.b, color.a + 0.2f);
            Vector3[] points = new Vector3[5];
            points[0] = new Vector3(spriteRect.x, spriteRect.y, 0);
            points[1] = new Vector3(spriteRect.x + spriteRect.width, spriteRect.y, 0);
            points[2] = new Vector3(spriteRect.x + spriteRect.width, spriteRect.y + spriteRect.height, 0);
            points[3] = new Vector3(spriteRect.x, spriteRect.y + spriteRect.height, 0);
            points[4] = new Vector3(spriteRect.x, spriteRect.y, 0);
            Handles.DrawPolyLine(points);

            // Draw a simple face to represent the sprite
            float eyeSize = Mathf.Abs(width) * 0.1f;
            float mouthWidth = Mathf.Abs(width) * 0.4f;
            float mouthHeight = Mathf.Abs(height) * 0.1f;

            // Draw eyes
            float eyeY = y - Mathf.Abs(height) * 0.1f;
            float leftEyeX = x - Mathf.Abs(width) * 0.15f;
            float rightEyeX = x + Mathf.Abs(width) * 0.15f;

            if (flipX)
            {
                float temp = leftEyeX;
                leftEyeX = rightEyeX;
                rightEyeX = temp;
            }

            if (flipY)
            {
                eyeY = y + Mathf.Abs(height) * 0.1f;
            }

            EditorGUI.DrawRect(new Rect(leftEyeX - eyeSize/2, eyeY - eyeSize/2, eyeSize, eyeSize), Color.black);
            EditorGUI.DrawRect(new Rect(rightEyeX - eyeSize/2, eyeY - eyeSize/2, eyeSize, eyeSize), Color.black);

            // Draw mouth
            float mouthY = y + Mathf.Abs(height) * 0.1f;
            if (flipY)
            {
                mouthY = y - Mathf.Abs(height) * 0.1f;
            }

            EditorGUI.DrawRect(new Rect(x - mouthWidth/2, mouthY - mouthHeight/2, mouthWidth, mouthHeight), Color.black);
        }

        /// <summary>
        /// Draws an arrow between two points
        /// </summary>
        private void DrawSpriteArrow(float x1, float y1, float x2, float y2, Color color)
        {
            Handles.color = color;

            // Draw the line
            Handles.DrawLine(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0));

            // Calculate the direction
            Vector2 direction = new Vector2(x2 - x1, y2 - y1).normalized;
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);

            // Draw the arrowhead
            float arrowSize = 10f;
            Vector2 arrowTip = new Vector2(x2, y2);
            Vector2 arrowBase = arrowTip - direction * arrowSize;

            Vector2 arrowLeft = arrowBase + perpendicular * arrowSize * 0.5f;
            Vector2 arrowRight = arrowBase - perpendicular * arrowSize * 0.5f;

            Handles.DrawLine(arrowTip, arrowLeft);
            Handles.DrawLine(arrowTip, arrowRight);
        }

        /// <summary>
        /// Draws component-specific help information
        /// </summary>
        protected override void DrawComponentSpecificHelp()
        {
            USpringEditorStyles.DrawSubHeader("Sprite Renderer Spring Component");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("The Sprite Renderer Spring Component allows you to animate sprite color and size with spring physics.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("It works with Unity's SpriteRenderer component to create smooth, physically-based animations for 2D sprites.");

            USpringEditorStyles.EndSection();

            // Draw example usage
            USpringEditorStyles.DrawSubHeader("Example Usage");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("1. Attach this component to a GameObject with a SpriteRenderer");
            EditorGUILayout.LabelField("2. Assign the Auto Updated Sprite Renderer (usually this GameObject's SpriteRenderer)");
            EditorGUILayout.LabelField("3. Enable 'Update Size' if you want to animate the sprite's size");
            EditorGUILayout.LabelField("4. Set your desired target color and/or size");
            EditorGUILayout.LabelField("5. Call SetTarget() from code to animate to new values");

            USpringEditorStyles.EndSection();

            // Draw flip animation section
            USpringEditorStyles.DrawSubHeader("Flip Animation");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("The component can automatically flip the sprite based on size changes:");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("• Animate Flip X: Flips the sprite horizontally when X size becomes negative");
            EditorGUILayout.LabelField("• Animate Flip Y: Flips the sprite vertically when Y size becomes negative");
            EditorGUILayout.LabelField("• Flip Threshold: Controls when the flip occurs (0-1 range)");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Note: 'Update Size' must be enabled for flip animation to work");

            USpringEditorStyles.EndSection();

            // Draw tips
            USpringEditorStyles.DrawSubHeader("Tips");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("• Great for character reactions, UI elements, and visual feedback");
            EditorGUILayout.LabelField("• Use color animation for flashing, fading, or highlighting effects");
            EditorGUILayout.LabelField("• Size animation works well for squash and stretch effects");
            EditorGUILayout.LabelField("• Combine with other spring components for complex animations");
            EditorGUILayout.LabelField("• For best results with size animation, set the sprite pivot appropriately");

            USpringEditorStyles.EndSection();
        }

        protected override void DrawCustomInitialValuesSection()
        {
            DrawInitialValuesBySpring("Initial Color", LabelWidth, colorSpringDrawer);

            if (!spUpdateSize.boolValue) return;
            Space(1);
            DrawInitialValuesBySpring("Initial Size", LabelWidth, sizeSpringDrawer);
        }

        protected override void DrawCustomInitialTarget()
        {
            DrawCustomTargetBySpring("Target Color", LabelWidth, colorSpringDrawer);

            if (!spUpdateSize.boolValue) return;
            Space(1);
            DrawCustomTargetBySpring("Target Size", LabelWidth, sizeSpringDrawer);
        }

        protected override void DrawMainAreaUnfolded()
        {
            DrawSerializedProperty(spAutoUpdatedSpriteRenderer, LabelWidth);
            DrawSerializedProperty(spUpdateSize, LabelWidth);

            Space(1);

            EditorGUILayout.LabelField("Flip Animation", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            DrawSerializedProperty(spAnimateFlipX, LabelWidth);
            DrawSerializedProperty(spAnimateFlipY, LabelWidth);

            if (spAnimateFlipX.boolValue || spAnimateFlipY.boolValue)
            {
                DrawSerializedProperty(spFlipThreshold, LabelWidth);
            }

            EditorGUI.indentLevel--;
        }

        protected override void DrawInfoArea()
        {
            EditorGUILayout.Space(2);

            if (spAutoUpdatedSpriteRenderer.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("SpriteRenderer is not assigned!", MessageType.Error);
            }

            if ((spAnimateFlipX.boolValue || spAnimateFlipY.boolValue) && !spUpdateSize.boolValue)
            {
                EditorGUILayout.HelpBox("Flip animation requires 'Update Size' to be enabled", MessageType.Warning);
            }

            if (spAnimateFlipX.boolValue || spAnimateFlipY.boolValue)
            {
                if (spFlipThreshold.floatValue <= 0 || spFlipThreshold.floatValue >= 1)
                {
                    EditorGUILayout.HelpBox("Flip threshold should be between 0 and 1", MessageType.Warning);
                }
            }
        }
    }
}
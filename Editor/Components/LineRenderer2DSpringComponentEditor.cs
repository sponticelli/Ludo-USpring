using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;
using System.Collections.Generic;

namespace USpring.Components
{
    [CustomEditor(typeof(LineRenderer2DSpringComponent))]
    [CanEditMultipleObjects]
    public class LineRenderer2DSpringComponentEditor : SpringComponentCustomEditor
    {
        private SerializedProperty spAutoUpdatedLineRenderer;

        // Animation toggles
        private SerializedProperty spAnimateWidth;
        private SerializedProperty spAnimateColor;
        private SerializedProperty spAnimateTexture;
        private SerializedProperty spAnimatePoints;

        // Point animation settings
        private SerializedProperty spUseCommonPointForce;
        private SerializedProperty spUseCommonPointDrag;
        private SerializedProperty spCommonPointForce;
        private SerializedProperty spCommonPointDrag;

        // Follow target settings
        private SerializedProperty spUseFollowTargets;
        private SerializedProperty spFollowTargets;
        private SerializedProperty spFollowTargetUpdateSpeed;

        // Spring drawers
        private SpringFloatDrawer startWidthSpringDrawer;
        private SpringFloatDrawer endWidthSpringDrawer;
        private SpringColorDrawer colorSpringDrawer;
        private SpringVector2Drawer textureOffsetSpringDrawer;
        private SpringVector2Drawer textureScaleSpringDrawer;

        // Foldout states
        private bool widthFoldout = true;
        private bool colorFoldout = true;
        private bool textureFoldout = true;
        private bool pointsFoldout = true;
        private bool followTargetsFoldout = true;

        // Reorderable list for points
        private SerializedProperty spPointSprings;
        private List<SpringVector2Drawer> pointSpringDrawers = new List<SpringVector2Drawer>();

        protected override void RefreshSerializedProperties()
        {
            base.RefreshSerializedProperties();

            spAutoUpdatedLineRenderer = serializedObject.FindProperty("autoUpdatedLineRenderer");

            spAnimateWidth = serializedObject.FindProperty("animateWidth");
            spAnimateColor = serializedObject.FindProperty("animateColor");
            spAnimateTexture = serializedObject.FindProperty("animateTexture");
            spAnimatePoints = serializedObject.FindProperty("animatePoints");

            spUseCommonPointForce = serializedObject.FindProperty("useCommonPointForce");
            spUseCommonPointDrag = serializedObject.FindProperty("useCommonPointDrag");
            spCommonPointForce = serializedObject.FindProperty("commonPointForce");
            spCommonPointDrag = serializedObject.FindProperty("commonPointDrag");

            spUseFollowTargets = serializedObject.FindProperty("useFollowTargets");
            spFollowTargets = serializedObject.FindProperty("followTargets");
            spFollowTargetUpdateSpeed = serializedObject.FindProperty("followTargetUpdateSpeed");

            spPointSprings = serializedObject.FindProperty("pointSprings");
        }

        protected override void CreateDrawers()
        {
            startWidthSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("startWidthSpring"), false, false);
            endWidthSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("endWidthSpring"), false, false);
            colorSpringDrawer = new SpringColorDrawer(serializedObject.FindProperty("colorSpring"), false, false);
            textureOffsetSpringDrawer = new SpringVector2Drawer(serializedObject.FindProperty("textureOffsetSpring"), false, false);
            textureScaleSpringDrawer = new SpringVector2Drawer(serializedObject.FindProperty("textureScaleSpring"), false, false);

            // Create point spring drawers
            RefreshPointSpringDrawers();
        }

        private void RefreshPointSpringDrawers()
        {
            pointSpringDrawers.Clear();

            if (spPointSprings == null || !spPointSprings.isArray) return;

            for (int i = 0; i < spPointSprings.arraySize; i++)
            {
                SerializedProperty pointSpring = spPointSprings.GetArrayElementAtIndex(i);
                SpringVector2Drawer drawer = new SpringVector2Drawer(pointSpring, false, false);
                pointSpringDrawers.Add(drawer);
            }
        }

        protected override void DrawSprings()
        {
            // Draw presets dropdown
            USpringPresets.DrawPresetDropdown(preset => {
                serializedObject.Update();

                // Apply presets to all enabled springs
                SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

                if (spAnimateWidth.boolValue)
                {
                    SerializedProperty startWidthForceProperty = startWidthSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty startWidthDragProperty = startWidthSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, startWidthForceProperty, startWidthDragProperty, analyticalSolutionProperty);
                    analyticalSolutionProperty = null; // Only apply to the first spring

                    SerializedProperty endWidthForceProperty = endWidthSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty endWidthDragProperty = endWidthSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, endWidthForceProperty, endWidthDragProperty, null);
                }

                if (spAnimateColor.boolValue)
                {
                    SerializedProperty colorForceProperty = colorSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty colorDragProperty = colorSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, colorForceProperty, colorDragProperty, analyticalSolutionProperty);
                    analyticalSolutionProperty = null;
                }

                if (spAnimateTexture.boolValue)
                {
                    SerializedProperty offsetForceProperty = textureOffsetSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty offsetDragProperty = textureOffsetSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, offsetForceProperty, offsetDragProperty, analyticalSolutionProperty);
                    analyticalSolutionProperty = null;

                    SerializedProperty scaleForceProperty = textureScaleSpringDrawer.SpringEditorObject.SpCommonForce;
                    SerializedProperty scaleDragProperty = textureScaleSpringDrawer.SpringEditorObject.SpCommonDrag;
                    USpringPresets.ApplyPreset(serializedObject, preset, scaleForceProperty, scaleDragProperty, null);
                }

                // Apply to point springs if enabled
                if (spAnimatePoints.boolValue && spUseCommonPointForce.boolValue && spUseCommonPointDrag.boolValue)
                {
                    // Apply to common point force and drag
                    spCommonPointForce.floatValue = preset.Force;
                    spCommonPointDrag.floatValue = preset.Drag;
                }

                serializedObject.ApplyModifiedProperties();
            });

            // Draw each spring section with foldout based on animation toggles
            if (spAnimateWidth.boolValue)
            {
                DrawWidthSprings();
            }

            if (spAnimateColor.boolValue)
            {
                DrawColorSpring();
            }

            if (spAnimateTexture.boolValue)
            {
                DrawTextureSprings();
            }

            if (spAnimatePoints.boolValue)
            {
                DrawPointSettings();
            }
        }

        /// <summary>
        /// Draws a preview of the line renderer spring behavior
        /// </summary>
        protected override void DrawSpringPreview()
        {
            // Draw a visualization of line renderer
            EditorGUILayout.Space(8);
            USpringEditorStyles.DrawSubHeader("Line Renderer Visualization");

            // Draw a simple line visualization
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(200));

            // Draw background
            EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1f));

            // Draw initial line
            DrawLine(rect, false);

            // Draw target line
            DrawLine(rect, true);

            // Draw spring previews for each enabled property
            EditorGUILayout.Space(8);

            // Draw individual spring previews
            if (spAnimateWidth.boolValue)
            {
                float startWidthForce = startWidthSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float startWidthDrag = startWidthSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;
                float endWidthForce = endWidthSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float endWidthDrag = endWidthSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

                USpringEditorStyles.DrawSubHeader("Width Spring Preview");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Start Width", EditorStyles.boldLabel);
                USpringPreviewUtility.DrawSpringPreview(startWidthForce, startWidthDrag, USpringEditorStyles.FloatSpringColor, "");
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("End Width", EditorStyles.boldLabel);
                USpringPreviewUtility.DrawSpringPreview(endWidthForce, endWidthDrag, USpringEditorStyles.FloatSpringColor, "");
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            if (spAnimateColor.boolValue)
            {
                float colorForce = colorSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float colorDrag = colorSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

                USpringEditorStyles.DrawSubHeader("Color Spring Preview");
                USpringPreviewUtility.DrawSpringPreview(colorForce, colorDrag, USpringEditorStyles.ColorSpringColor, "Color");
            }

            if (spAnimateTexture.boolValue)
            {
                float offsetForce = textureOffsetSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float offsetDrag = textureOffsetSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;
                float scaleForce = textureScaleSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
                float scaleDrag = textureScaleSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

                USpringEditorStyles.DrawSubHeader("Texture Spring Preview");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Texture Offset", EditorStyles.boldLabel);
                float[] offsetForces = new float[] { offsetForce, offsetForce };
                float[] offsetDrags = new float[] { offsetDrag, offsetDrag };
                Color[] offsetColors = new Color[] {
                    USpringEditorStyles.Vector2SpringColor, // X
                    USpringEditorStyles.Vector2SpringColor  // Y
                };
                string[] offsetLabels = new string[] { "X", "Y" };
                USpringPreviewUtility.DrawMultiComponentSpringPreview(offsetForces, offsetDrags, offsetColors, offsetLabels);
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Texture Scale", EditorStyles.boldLabel);
                float[] scaleForces = new float[] { scaleForce, scaleForce };
                float[] scaleDrags = new float[] { scaleDrag, scaleDrag };
                Color[] scaleColors = new Color[] {
                    USpringEditorStyles.Vector2SpringColor, // X
                    USpringEditorStyles.Vector2SpringColor  // Y
                };
                string[] scaleLabels = new string[] { "X", "Y" };
                USpringPreviewUtility.DrawMultiComponentSpringPreview(scaleForces, scaleDrags, scaleColors, scaleLabels);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            if (spAnimatePoints.boolValue)
            {
                USpringEditorStyles.DrawSubHeader("Point Springs Preview");

                if (spUseCommonPointForce.boolValue && spUseCommonPointDrag.boolValue)
                {
                    float pointForce = spCommonPointForce.floatValue;
                    float pointDrag = spCommonPointDrag.floatValue;

                    float[] forces = new float[] { pointForce, pointForce };
                    float[] drags = new float[] { pointDrag, pointDrag };
                    Color[] colors = new Color[] {
                        USpringEditorStyles.Vector2SpringColor, // X
                        USpringEditorStyles.Vector2SpringColor  // Y
                    };
                    string[] labels = new string[] { "X", "Y" };

                    USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);
                }
                else
                {
                    EditorGUILayout.HelpBox("Point springs use individual force and drag values. Enable 'Use Common Force' and 'Use Common Drag' to see a preview.", MessageType.Info);
                }
            }
        }

        /// <summary>
        /// Draws a line visualization
        /// </summary>
        private void DrawLine(Rect rect, bool isTarget)
        {
            float padding = 20f;
            float width = rect.width - padding * 2;
            float height = rect.height - padding * 2;

            // Get line points
            Vector2[] points = new Vector2[5];

            if (isTarget)
            {
                // Target line (curved)
                points[0] = new Vector2(rect.x + padding, rect.y + height * 0.5f + padding);
                points[1] = new Vector2(rect.x + width * 0.25f + padding, rect.y + height * 0.25f + padding);
                points[2] = new Vector2(rect.x + width * 0.5f + padding, rect.y + height * 0.75f + padding);
                points[3] = new Vector2(rect.x + width * 0.75f + padding, rect.y + height * 0.25f + padding);
                points[4] = new Vector2(rect.x + width + padding, rect.y + height * 0.5f + padding);
            }
            else
            {
                // Initial line (straight)
                points[0] = new Vector2(rect.x + padding, rect.y + height * 0.5f + padding);
                points[1] = new Vector2(rect.x + width * 0.25f + padding, rect.y + height * 0.5f + padding);
                points[2] = new Vector2(rect.x + width * 0.5f + padding, rect.y + height * 0.5f + padding);
                points[3] = new Vector2(rect.x + width * 0.75f + padding, rect.y + height * 0.5f + padding);
                points[4] = new Vector2(rect.x + width + padding, rect.y + height * 0.5f + padding);
            }

            // Draw the line
            Color lineColor = isTarget ? new Color(0.4f, 0.8f, 1f, 0.8f) : new Color(0.5f, 0.5f, 0.5f, 0.5f);
            float lineWidth = isTarget ? 4f : 2f;

            Handles.color = lineColor;
            for (int i = 0; i < points.Length - 1; i++)
            {
                Handles.DrawAAPolyLine(lineWidth, points[i], points[i + 1]);
            }

            // Draw points
            for (int i = 0; i < points.Length; i++)
            {
                float pointSize = isTarget ? 8f : 6f;
                Rect pointRect = new Rect(points[i].x - pointSize/2, points[i].y - pointSize/2, pointSize, pointSize);
                EditorGUI.DrawRect(pointRect, lineColor);
            }

            // Draw label
            string label = isTarget ? "Target" : "Initial";
            EditorGUI.LabelField(new Rect(points[0].x - 50, points[0].y - 20, 100, 20), label);
        }

        /// <summary>
        /// Draws component-specific help information
        /// </summary>
        protected override void DrawComponentSpecificHelp()
        {
            USpringEditorStyles.DrawSubHeader("Line Renderer 2D Spring Component");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("The Line Renderer 2D Spring Component allows you to animate various properties of a LineRenderer with spring physics.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("It works with Unity's LineRenderer component to create smooth, physically-based line animations in 2D space.");

            USpringEditorStyles.EndSection();

            // Draw example usage
            USpringEditorStyles.DrawSubHeader("Example Usage");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("1. Attach this component to a GameObject with a LineRenderer");
            EditorGUILayout.LabelField("2. Assign the Auto Updated Line Renderer (usually this GameObject's LineRenderer)");
            EditorGUILayout.LabelField("3. Enable the properties you want to animate with springs (Width, Color, Texture, Points)");
            EditorGUILayout.LabelField("4. Set your desired target values for each property");
            EditorGUILayout.LabelField("5. Call SetTarget() from code to animate to new values");

            USpringEditorStyles.EndSection();

            // Draw property explanations
            USpringEditorStyles.DrawSubHeader("Property Explanations");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("• Width: Animates the start and end width of the line");
            EditorGUILayout.LabelField("• Color: Animates the color of the line");
            EditorGUILayout.LabelField("• Texture: Animates the texture offset and scale");
            EditorGUILayout.LabelField("• Points: Animates the position of line points");

            USpringEditorStyles.EndSection();

            // Draw point animation section
            USpringEditorStyles.DrawSubHeader("Point Animation");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("Point animation has two modes:");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("1. Target-based: Set target positions directly");
            EditorGUILayout.LabelField("2. Follow Targets: Each point follows a Transform");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Common Point Settings allow you to use the same force and drag values for all points.");

            USpringEditorStyles.EndSection();

            // Draw tips
            USpringEditorStyles.DrawSubHeader("Tips");
            USpringEditorStyles.BeginSection();

            EditorGUILayout.LabelField("• Great for creating dynamic cables, ropes, laser effects, etc.");
            EditorGUILayout.LabelField("• Use Follow Targets to create lines that connect moving objects");
            EditorGUILayout.LabelField("• Combine width and color animation for pulsing effects");
            EditorGUILayout.LabelField("• For best results with point animation, use fewer points (5-10)");
            EditorGUILayout.LabelField("• Point animation works best in 2D space (Z values are preserved but not animated)");

            USpringEditorStyles.EndSection();
        }

        private void DrawWidthSprings()
        {
            widthFoldout = DrawRectangleArea("Width Springs", widthFoldout);

            if (widthFoldout)
            {
                EditorGUILayout.Space(1);

                EditorGUILayout.LabelField("Start Width", EditorStyles.boldLabel);
                Rect propertyRect = EditorGUILayout.GetControlRect(false, startWidthSpringDrawer.GetPropertyHeight());
                startWidthSpringDrawer.OnGUI(propertyRect);

                EditorGUILayout.Space(5);

                EditorGUILayout.LabelField("End Width", EditorStyles.boldLabel);
                propertyRect = EditorGUILayout.GetControlRect(false, endWidthSpringDrawer.GetPropertyHeight());
                endWidthSpringDrawer.OnGUI(propertyRect);
            }
        }

        private void DrawColorSpring()
        {
            colorFoldout = DrawRectangleArea("Color Spring", colorFoldout);

            if (colorFoldout)
            {
                EditorGUILayout.Space(1);

                Rect propertyRect = EditorGUILayout.GetControlRect(false, colorSpringDrawer.GetPropertyHeight());
                colorSpringDrawer.OnGUI(propertyRect);
            }
        }

        private void DrawTextureSprings()
        {
            textureFoldout = DrawRectangleArea("Texture Springs", textureFoldout);

            if (textureFoldout)
            {
                EditorGUILayout.Space(1);

                EditorGUILayout.LabelField("Texture Offset", EditorStyles.boldLabel);
                Rect propertyRect = EditorGUILayout.GetControlRect(false, textureOffsetSpringDrawer.GetPropertyHeight());
                textureOffsetSpringDrawer.OnGUI(propertyRect);

                EditorGUILayout.Space(5);

                EditorGUILayout.LabelField("Texture Scale", EditorStyles.boldLabel);
                propertyRect = EditorGUILayout.GetControlRect(false, textureScaleSpringDrawer.GetPropertyHeight());
                textureScaleSpringDrawer.OnGUI(propertyRect);
            }
        }

        private void DrawPointSettings()
        {
            pointsFoldout = DrawRectangleArea("Point Springs", pointsFoldout);

            if (pointsFoldout)
            {
                EditorGUILayout.Space(1);

                // Point spring settings
                EditorGUILayout.LabelField("Common Point Settings", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(spUseCommonPointForce, new GUIContent("Use Common Force"));
                if (spUseCommonPointForce.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(spCommonPointForce, new GUIContent("Force"));
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.PropertyField(spUseCommonPointDrag, new GUIContent("Use Common Drag"));
                if (spUseCommonPointDrag.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(spCommonPointDrag, new GUIContent("Drag"));
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;

                // Draw individual point springs if not too many
                LineRenderer2DSpringComponent component = (LineRenderer2DSpringComponent)target;
                LineRenderer lineRenderer = component.GetComponent<LineRenderer>();

                if (lineRenderer && lineRenderer.positionCount <= 8)
                {
                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("Individual Point Settings", EditorStyles.boldLabel);

                    if (GUILayout.Button("Refresh Point Springs"))
                    {
                        RefreshPointSpringDrawers();
                    }

                    if (pointSpringDrawers.Count > 0)
                    {
                        EditorGUI.indentLevel++;

                        for (int i = 0; i < pointSpringDrawers.Count; i++)
                        {
                            EditorGUILayout.LabelField($"Point {i}", EditorStyles.boldLabel);
                            Rect propertyRect = EditorGUILayout.GetControlRect(false, pointSpringDrawers[i].GetPropertyHeight());
                            pointSpringDrawers[i].OnGUI(propertyRect);

                            if (i < pointSpringDrawers.Count - 1)
                            {
                                EditorGUILayout.Space(5);
                            }
                        }

                        EditorGUI.indentLevel--;
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No point springs available. Initialize the component first.", MessageType.Info);
                    }
                }
                else if (lineRenderer && lineRenderer.positionCount > 8)
                {
                    EditorGUILayout.HelpBox($"Line has {lineRenderer.positionCount} points. Individual point editing is disabled for performance reasons.", MessageType.Info);
                }

                // Follow targets section
                EditorGUILayout.Space(10);
                followTargetsFoldout = EditorGUILayout.Foldout(followTargetsFoldout, "Follow Targets", true);

                if (followTargetsFoldout)
                {
                    EditorGUILayout.PropertyField(spUseFollowTargets, new GUIContent("Use Follow Targets"));

                    if (spUseFollowTargets.boolValue)
                    {
                        EditorGUILayout.PropertyField(spFollowTargetUpdateSpeed, new GUIContent("Update Speed"));

                        if (spFollowTargets.isArray && lineRenderer && lineRenderer.positionCount <= 8)
                        {
                            EditorGUILayout.LabelField("Target Transforms", EditorStyles.boldLabel);

                            // Make sure the array has enough elements
                            while (spFollowTargets.arraySize < lineRenderer.positionCount)
                            {
                                spFollowTargets.arraySize++;
                            }

                            while (spFollowTargets.arraySize > lineRenderer.positionCount)
                            {
                                spFollowTargets.arraySize--;
                            }

                            // Display the target slots
                            for (int i = 0; i < spFollowTargets.arraySize; i++)
                            {
                                EditorGUILayout.PropertyField(
                                    spFollowTargets.GetArrayElementAtIndex(i),
                                    new GUIContent($"Point {i} Target")
                                );
                            }
                        }
                        else if (lineRenderer && lineRenderer.positionCount > 8)
                        {
                            EditorGUILayout.HelpBox($"Line has {lineRenderer.positionCount} points. Expand the Follow Targets list during runtime or via script.", MessageType.Info);

                            if (GUILayout.Button("Clear All Follow Targets"))
                            {
                                spFollowTargets.ClearArray();
                            }
                        }
                    }
                }
            }
        }

        protected override void DrawCustomInitialValuesSection()
        {
            EditorGUILayout.LabelField("Initial Values", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            if (spAnimateWidth.boolValue)
            {
                DrawInitialValuesBySpring("Start Width", LabelWidth, startWidthSpringDrawer);
                DrawInitialValuesBySpring("End Width", LabelWidth, endWidthSpringDrawer);
                Space(1);
            }

            if (spAnimateColor.boolValue)
            {
                DrawInitialValuesBySpring("Color", LabelWidth, colorSpringDrawer);
                Space(1);
            }

            if (spAnimateTexture.boolValue)
            {
                DrawInitialValuesBySpring("Texture Offset", LabelWidth, textureOffsetSpringDrawer);
                DrawInitialValuesBySpring("Texture Scale", LabelWidth, textureScaleSpringDrawer);
                Space(1);
            }

            // We don't show individual point initial values here as there can be many
            if (spAnimatePoints.boolValue)
            {
                EditorGUILayout.HelpBox("Point initial values will be taken from the LineRenderer's current positions.", MessageType.Info);
            }

            EditorGUI.indentLevel--;
        }

        protected override void DrawCustomInitialTarget()
        {
            EditorGUILayout.LabelField("Target Values", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            if (spAnimateWidth.boolValue)
            {
                DrawCustomTargetBySpring("Start Width", LabelWidth, startWidthSpringDrawer);
                DrawCustomTargetBySpring("End Width", LabelWidth, endWidthSpringDrawer);
                Space(1);
            }

            if (spAnimateColor.boolValue)
            {
                DrawCustomTargetBySpring("Color", LabelWidth, colorSpringDrawer);
                Space(1);
            }

            if (spAnimateTexture.boolValue)
            {
                DrawCustomTargetBySpring("Texture Offset", LabelWidth, textureOffsetSpringDrawer);
                DrawCustomTargetBySpring("Texture Scale", LabelWidth, textureScaleSpringDrawer);
                Space(1);
            }

            // We don't show individual point target values here as there can be many
            if (spAnimatePoints.boolValue)
            {
                EditorGUILayout.HelpBox("Point target values will be taken from the LineRenderer's current positions or follow targets.", MessageType.Info);
            }

            EditorGUI.indentLevel--;
        }

        protected override void DrawMainAreaUnfolded()
        {
            // LineRenderer reference
            DrawSerializedProperty(spAutoUpdatedLineRenderer, LabelWidth);

            Space(1);

            // Properties to animate section
            EditorGUILayout.LabelField("Properties to Animate", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;

            DrawAnimate(spAnimateWidth, "Width", "Animates the start and end width of the line using spring physics");
            DrawAnimate(spAnimateColor, "Color", "Animates the color of the line using spring physics");
            DrawAnimate(spAnimateTexture, "Texture", "Animates the texture offset and scale using spring physics");
            DrawAnimate(spAnimatePoints, "Points", "Animates the position of line points using spring physics");

            EditorGUI.indentLevel--;

            LineRenderer2DSpringComponent component = target as LineRenderer2DSpringComponent;
            LineRenderer lineRenderer = component?.GetComponent<LineRenderer>();

            if (lineRenderer && Application.isPlaying)
            {
                Space(1);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Update Point Count"))
                {
                    component.UpdatePointCount(lineRenderer.positionCount);
                    RefreshPointSpringDrawers();
                }

                if (GUILayout.Button("Refresh From LineRenderer"))
                {
                    component.Initialize();
                    RefreshPointSpringDrawers();
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawAnimate(SerializedProperty property, string label, string tooltip)
        {
            EditorGUILayout.PropertyField(property, new GUIContent(label, tooltip));
        }

        protected override void DrawInfoArea()
        {
            EditorGUILayout.Space(2);

            if (spAutoUpdatedLineRenderer.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("LineRenderer is not assigned!", MessageType.Error);
                return;
            }

            // Check if any property is selected for animation
            bool anyPropertyAnimated = spAnimateWidth.boolValue ||
                                      spAnimateColor.boolValue ||
                                      spAnimateTexture.boolValue ||
                                      spAnimatePoints.boolValue;

            if (!anyPropertyAnimated)
            {
                EditorGUILayout.HelpBox("No properties selected for animation. Enable at least one property to animate.", MessageType.Warning);
            }

            // Check if material is properly set for texture animation
            if (spAnimateTexture.boolValue)
            {
                LineRenderer lineRenderer = spAutoUpdatedLineRenderer.objectReferenceValue as LineRenderer;
                if (lineRenderer != null && lineRenderer.sharedMaterial == null)
                {
                    EditorGUILayout.HelpBox("Texture animation is enabled but the LineRenderer has no material assigned.", MessageType.Warning);
                }
            }

            // Show tip about point animation
            if (spAnimatePoints.boolValue)
            {
                EditorGUILayout.HelpBox("Point position animation works best in 2D. The Z coordinate will be preserved but not animated.", MessageType.Info);

                if (spUseFollowTargets.boolValue)
                {
                    bool anyTargetAssigned = false;
                    for (int i = 0; i < spFollowTargets.arraySize; i++)
                    {
                        if (spFollowTargets.GetArrayElementAtIndex(i).objectReferenceValue != null)
                        {
                            anyTargetAssigned = true;
                            break;
                        }
                    }

                    if (!anyTargetAssigned)
                    {
                        EditorGUILayout.HelpBox("Follow Targets is enabled but no targets are assigned.", MessageType.Info);
                    }
                }
            }
        }
    }
}
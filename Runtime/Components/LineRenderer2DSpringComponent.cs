using UnityEngine;
using USpring.Core;
using System.Collections.Generic;

namespace USpring.Components
{
    [AddComponentMenu("Ludo/USpring/Components/Line Renderer 2D Spring")]
    public partial class LineRenderer2DSpringComponent : SpringComponent
    {
        [SerializeField] private LineRenderer autoUpdatedLineRenderer;
        
        // Springs for various line renderer properties
        [SerializeField] private SpringFloat startWidthSpring = new SpringFloat();
        [SerializeField] private SpringFloat endWidthSpring = new SpringFloat();
        [SerializeField] private SpringColor colorSpring = new SpringColor();
        [SerializeField] private SpringVector2 textureOffsetSpring = new SpringVector2();
        [SerializeField] private SpringVector2 textureScaleSpring = new SpringVector2();
        
        // Points position springs - we use a list since the number of points can vary
        [SerializeField] private List<SpringVector2> pointSprings = new List<SpringVector2>();
        
        // Toggle which properties to animate
        [SerializeField] private bool animateWidth = true;
        [SerializeField] private bool animateColor = false;
        [SerializeField] private bool animateTexture = false;
        [SerializeField] private bool animatePoints = false;
        
        // Point animation settings
        [SerializeField] private bool useCommonPointForce = true;
        [SerializeField] private bool useCommonPointDrag = true;
        [SerializeField] private float commonPointForce = 80f;
        [SerializeField] private float commonPointDrag = 8f;

        // Follow targets
        [SerializeField] private bool useFollowTargets = false;
        [SerializeField] private List<Transform> followTargets = new List<Transform>();
        [SerializeField] private float followTargetUpdateSpeed = 10f;
        
        // Cached current positions
        private Vector3[] currentPositions;
        private Vector3[] targetPositions;
        private int currentPointCount = 0;
        
        #region Width Spring Methods
        public SpringEvents StartWidthEvents => startWidthSpring.springEvents;
        public float GetTargetStartWidth() => startWidthSpring.GetTarget();
        public void SetTargetStartWidth(float target) => startWidthSpring.SetTarget(target);
        public float GetCurrentStartWidth() => startWidthSpring.GetCurrentValue();
        public void SetCurrentStartWidth(float value) => startWidthSpring.SetCurrentValue(value);
        public float GetVelocityStartWidth() => startWidthSpring.GetVelocity();
        public void SetVelocityStartWidth(float velocity) => startWidthSpring.SetVelocity(velocity);
        public void AddVelocityStartWidth(float velocityToAdd) => startWidthSpring.AddVelocity(velocityToAdd);
        
        public SpringEvents EndWidthEvents => endWidthSpring.springEvents;
        public float GetTargetEndWidth() => endWidthSpring.GetTarget();
        public void SetTargetEndWidth(float target) => endWidthSpring.SetTarget(target);
        public float GetCurrentEndWidth() => endWidthSpring.GetCurrentValue();
        public void SetCurrentEndWidth(float value) => endWidthSpring.SetCurrentValue(value);
        public float GetVelocityEndWidth() => endWidthSpring.GetVelocity();
        public void SetVelocityEndWidth(float velocity) => endWidthSpring.SetVelocity(velocity);
        public void AddVelocityEndWidth(float velocityToAdd) => endWidthSpring.AddVelocity(velocityToAdd);
        
        // Common methods for both width springs
        public void SetTargetWidth(float start, float end)
        {
            SetTargetStartWidth(start);
            SetTargetEndWidth(end);
        }
        
        public void SetTargetWidth(float width)
        {
            SetTargetWidth(width, width);
        }
        
        public void AddVelocityWidth(float velocity)
        {
            AddVelocityStartWidth(velocity);
            AddVelocityEndWidth(velocity);
        }
        
        public void SetCommonForceWidth(float force)
        {
            startWidthSpring.SetCommonForceAndDrag(true);
            startWidthSpring.SetCommonForce(force);
            endWidthSpring.SetCommonForceAndDrag(true);
            endWidthSpring.SetCommonForce(force);
        }
        
        public void SetCommonDragWidth(float drag)
        {
            startWidthSpring.SetCommonForceAndDrag(true);
            startWidthSpring.SetCommonDrag(drag);
            endWidthSpring.SetCommonForceAndDrag(true);
            endWidthSpring.SetCommonDrag(drag);
        }
        #endregion
        
        #region Color Spring Methods
        public SpringEvents ColorEvents => colorSpring.springEvents;
        public Color GetTargetColor() => colorSpring.GetTargetColor();
        public void SetTargetColor(Color target) => colorSpring.SetTarget(target);
        public Color GetCurrentColor() => colorSpring.GetCurrentColor();
        public void SetCurrentColor(Color value) => colorSpring.SetCurrentValue(value);
        public Vector4 GetVelocityColor() => colorSpring.GetVelocity();
        public void SetVelocityColor(Vector4 velocity) => colorSpring.SetVelocity(velocity);
        public void AddVelocityColor(Vector4 velocityToAdd) => colorSpring.AddVelocity(velocityToAdd);
        
        public void SetCommonForceColor(float force)
        {
            colorSpring.SetCommonForceAndDrag(true);
            colorSpring.SetCommonForce(force);
        }
        
        public void SetCommonDragColor(float drag)
        {
            colorSpring.SetCommonForceAndDrag(true);
            colorSpring.SetCommonDrag(drag);
        }
        #endregion
        
        #region Texture Spring Methods
        public SpringEvents TextureOffsetEvents => textureOffsetSpring.springEvents;
        public Vector2 GetTargetTextureOffset() => textureOffsetSpring.GetTarget();
        public void SetTargetTextureOffset(Vector2 target) => textureOffsetSpring.SetTarget(target);
        public Vector2 GetCurrentTextureOffset() => textureOffsetSpring.GetCurrentValue();
        public void SetCurrentTextureOffset(Vector2 value) => textureOffsetSpring.SetCurrentValue(value);
        public Vector2 GetVelocityTextureOffset() => textureOffsetSpring.GetVelocity();
        public void SetVelocityTextureOffset(Vector2 velocity) => textureOffsetSpring.SetVelocity(velocity);
        public void AddVelocityTextureOffset(Vector2 velocityToAdd) => textureOffsetSpring.AddVelocity(velocityToAdd);
        
        public SpringEvents TextureScaleEvents => textureScaleSpring.springEvents;
        public Vector2 GetTargetTextureScale() => textureScaleSpring.GetTarget();
        public void SetTargetTextureScale(Vector2 target) => textureScaleSpring.SetTarget(target);
        public Vector2 GetCurrentTextureScale() => textureScaleSpring.GetCurrentValue();
        public void SetCurrentTextureScale(Vector2 value) => textureScaleSpring.SetCurrentValue(value);
        public Vector2 GetVelocityTextureScale() => textureScaleSpring.GetVelocity();
        public void SetVelocityTextureScale(Vector2 velocity) => textureScaleSpring.SetVelocity(velocity);
        public void AddVelocityTextureScale(Vector2 velocityToAdd) => textureScaleSpring.AddVelocity(velocityToAdd);
        
        public void SetCommonForceTexture(float force)
        {
            textureOffsetSpring.SetCommonForceAndDrag(true);
            textureOffsetSpring.SetCommonForce(force);
            textureScaleSpring.SetCommonForceAndDrag(true);
            textureScaleSpring.SetCommonForce(force);
        }
        
        public void SetCommonDragTexture(float drag)
        {
            textureOffsetSpring.SetCommonForceAndDrag(true);
            textureOffsetSpring.SetCommonDrag(drag);
            textureScaleSpring.SetCommonForceAndDrag(true);
            textureScaleSpring.SetCommonDrag(drag);
        }
        #endregion
        
        #region Point Position Methods
        // Get point position (2D - X,Y only)
        public Vector2 GetPointPosition(int index)
        {
            if (index < 0 || index >= pointSprings.Count)
            {
                Debug.LogError($"Index {index} out of range for point springs");
                return Vector2.zero;
            }
            
            Vector2 pos = pointSprings[index].GetCurrentValue();
            return pos;
        }
        
        // Set target position for a specific point
        public void SetTargetPointPosition(int index, Vector2 position)
        {
            if (index < 0 || index >= pointSprings.Count)
            {
                Debug.LogError($"Index {index} out of range for point springs");
                return;
            }
            
            pointSprings[index].SetTarget(position);
            targetPositions[index] = new Vector3(position.x, position.y, targetPositions[index].z);
        }
        
        // Add velocity to a specific point
        public void AddPointVelocity(int index, Vector2 velocity)
        {
            if (index < 0 || index >= pointSprings.Count)
            {
                Debug.LogError($"Index {index} out of range for point springs");
                return;
            }
            
            pointSprings[index].AddVelocity(velocity);
        }
        
        // Set all target positions at once
        public void SetTargetPointPositions(Vector2[] positions)
        {
            if (positions.Length != pointSprings.Count)
            {
                Debug.LogError("Position array length doesn't match point count");
                return;
            }
            
            for (int i = 0; i < positions.Length; i++)
            {
                SetTargetPointPosition(i, positions[i]);
            }
        }
        
        // Update points count (recreate springs if needed)
        public void UpdatePointCount(int newCount)
        {
            if (newCount <= 0)
            {
                Debug.LogError("Point count must be greater than 0");
                return;
            }
            
            // Update the LineRenderer first
            autoUpdatedLineRenderer.positionCount = newCount;
            
            // Cache the current positions
            Vector3[] positions = new Vector3[newCount];
            autoUpdatedLineRenderer.GetPositions(positions);
            
            // Update our arrays
            currentPositions = new Vector3[newCount];
            targetPositions = new Vector3[newCount];
            System.Array.Copy(positions, currentPositions, newCount);
            System.Array.Copy(positions, targetPositions, newCount);
            
            // Update our springs list
            if (pointSprings.Count != newCount)
            {
                // Save existing points
                for (int i = 0; i < Mathf.Min(pointSprings.Count, newCount); i++)
                {
                    // Keep the existing springs
                }
                
                // Add or remove springs as needed
                if (newCount > pointSprings.Count)
                {
                    // Add new springs
                    for (int i = pointSprings.Count; i < newCount; i++)
                    {
                        SpringVector2 newSpring = new SpringVector2();
                        SetupPointSpring(newSpring);
                        
                        Vector2 pos = new Vector2(positions[i].x, positions[i].y);
                        newSpring.SetCurrentValue(pos);
                        newSpring.SetTarget(pos);
                        
                        pointSprings.Add(newSpring);
                        
                        if (initialized)
                        {
                            RegisterSpring(newSpring);
                            newSpring.Initialize();
                        }
                    }
                }
                else if (newCount < pointSprings.Count)
                {
                    // Remove excess springs
                    for (int i = pointSprings.Count - 1; i >= newCount; i--)
                    {
                        // No need to unregister as we'll reinitialize
                        pointSprings.RemoveAt(i);
                    }
                }
                
                currentPointCount = newCount;
            }
        }
        
        private void SetupPointSpring(SpringVector2 spring)
        {
            spring.SetCommonForceAndDrag(true);
            
            if (useCommonPointForce)
            {
                spring.SetCommonForce(commonPointForce);
            }
            
            if (useCommonPointDrag)
            {
                spring.SetCommonDrag(commonPointDrag);
            }
        }
        
        // Set the springs' force and drag
        public void SetPointSpringForceAndDrag(float force, float drag)
        {
            useCommonPointForce = true;
            useCommonPointDrag = true;
            commonPointForce = force;
            commonPointDrag = drag;
            
            foreach (var spring in pointSprings)
            {
                spring.SetCommonForceAndDrag(true);
                spring.SetCommonForce(force);
                spring.SetCommonDrag(drag);
            }
        }
        #endregion
        
        #region Follow Target Methods
        public void SetFollowTarget(int pointIndex, Transform target)
        {
            if (pointIndex < 0 || pointIndex >= followTargets.Count)
            {
                Debug.LogError($"Point index {pointIndex} out of range for follow targets");
                return;
            }
            
            followTargets[pointIndex] = target;
        }
        
        public void UpdateFollowTargets()
        {
            if (!useFollowTargets || followTargets.Count == 0)
                return;
                
            // Make sure we have the right number of targets
            while (followTargets.Count < currentPointCount)
            {
                followTargets.Add(null);
            }
            
            while (followTargets.Count > currentPointCount)
            {
                followTargets.RemoveAt(followTargets.Count - 1);
            }
            
            // Update targets for each point that has a follow target
            for (int i = 0; i < followTargets.Count; i++)
            {
                if (followTargets[i] != null)
                {
                    Vector2 targetPos = followTargets[i].position;
                    SetTargetPointPosition(i, targetPos);
                }
            }
        }
        #endregion
        
        public override void Initialize()
        {
            if (autoUpdatedLineRenderer == null)
            {
                Debug.LogError($"LineRenderer2DSpringComponent on {gameObject.name} has no LineRenderer assigned!");
                return;
            }
            
            // Initialize position arrays
            currentPointCount = autoUpdatedLineRenderer.positionCount;
            currentPositions = new Vector3[currentPointCount];
            targetPositions = new Vector3[currentPointCount];
            autoUpdatedLineRenderer.GetPositions(currentPositions);
            System.Array.Copy(currentPositions, targetPositions, currentPointCount);
            
            // Set up point springs if we need to animate points
            if (animatePoints)
            {
                // Make sure we have the right number of point springs
                if (pointSprings.Count != currentPointCount)
                {
                    pointSprings.Clear();
                    
                    for (int i = 0; i < currentPointCount; i++)
                    {
                        SpringVector2 spring = new SpringVector2();
                        SetupPointSpring(spring);
                        
                        Vector2 pos = new Vector2(currentPositions[i].x, currentPositions[i].y);
                        spring.SetCurrentValue(pos);
                        spring.SetTarget(pos);
                        
                        pointSprings.Add(spring);
                    }
                }
                
                // Initialize follow targets array
                if (useFollowTargets)
                {
                    while (followTargets.Count < currentPointCount)
                    {
                        followTargets.Add(null);
                    }
                    
                    while (followTargets.Count > currentPointCount)
                    {
                        followTargets.RemoveAt(followTargets.Count - 1);
                    }
                }
            }
            
            base.Initialize();
        }
        
        protected override void RegisterSprings()
        {
            if (animateWidth)
            {
                RegisterSpring(startWidthSpring);
                RegisterSpring(endWidthSpring);
            }
            
            if (animateColor)
            {
                RegisterSpring(colorSpring);
            }
            
            if (animateTexture)
            {
                RegisterSpring(textureOffsetSpring);
                RegisterSpring(textureScaleSpring);
            }
            
            if (animatePoints)
            {
                foreach (var spring in pointSprings)
                {
                    RegisterSpring(spring);
                }
            }
        }

        protected override void SetCurrentValueByDefault()
        {
            if (animateWidth)
            {
                startWidthSpring.SetCurrentValue(autoUpdatedLineRenderer.startWidth);
                endWidthSpring.SetCurrentValue(autoUpdatedLineRenderer.endWidth);
            }
            
            if (animateColor)
            {
                // Get color from start color if using gradient, otherwise use the color property
                if (autoUpdatedLineRenderer.useWorldSpace)
                {
                    colorSpring.SetCurrentValue(autoUpdatedLineRenderer.startColor);
                }
                else
                {
                    colorSpring.SetCurrentValue(autoUpdatedLineRenderer.startColor);
                }
            }
            
            if (animateTexture)
            {
                if (autoUpdatedLineRenderer.sharedMaterial != null)
                {
                    textureOffsetSpring.SetCurrentValue(autoUpdatedLineRenderer.sharedMaterial.mainTextureOffset);
                    textureScaleSpring.SetCurrentValue(autoUpdatedLineRenderer.sharedMaterial.mainTextureScale);
                }
            }
            
            if (animatePoints)
            {
                autoUpdatedLineRenderer.GetPositions(currentPositions);
                
                for (int i = 0; i < Mathf.Min(pointSprings.Count, currentPositions.Length); i++)
                {
                    Vector2 pos = new Vector2(currentPositions[i].x, currentPositions[i].y);
                    pointSprings[i].SetCurrentValue(pos);
                }
            }
        }

        protected override void SetTargetByDefault()
        {
            if (animateWidth)
            {
                startWidthSpring.SetTarget(autoUpdatedLineRenderer.startWidth);
                endWidthSpring.SetTarget(autoUpdatedLineRenderer.endWidth);
            }
            
            if (animateColor)
            {
                // Set target color same as current
                if (autoUpdatedLineRenderer.useWorldSpace)
                {
                    colorSpring.SetTarget(autoUpdatedLineRenderer.startColor);
                }
                else
                {
                    colorSpring.SetTarget(autoUpdatedLineRenderer.startColor);
                }
            }
            
            if (animateTexture)
            {
                if (autoUpdatedLineRenderer.sharedMaterial != null)
                {
                    textureOffsetSpring.SetTarget(autoUpdatedLineRenderer.sharedMaterial.mainTextureOffset);
                    textureScaleSpring.SetTarget(autoUpdatedLineRenderer.sharedMaterial.mainTextureScale);
                }
            }
            
            if (animatePoints)
            {
                autoUpdatedLineRenderer.GetPositions(targetPositions);
                
                for (int i = 0; i < Mathf.Min(pointSprings.Count, targetPositions.Length); i++)
                {
                    Vector2 pos = new Vector2(targetPositions[i].x, targetPositions[i].y);
                    pointSprings[i].SetTarget(pos);
                }
            }
        }
        
        public void Update()
        {
            if (!initialized) return;
            
            // Update based on follow targets
            if (animatePoints && useFollowTargets)
            {
                UpdateFollowTargets();
            }
            
            // Update LineRenderer properties based on spring values
            if (animateWidth)
            {
                autoUpdatedLineRenderer.startWidth = startWidthSpring.GetCurrentValue();
                autoUpdatedLineRenderer.endWidth = endWidthSpring.GetCurrentValue();
            }
            
            if (animateColor)
            {
                Color currentColor = colorSpring.GetCurrentColor();
                autoUpdatedLineRenderer.startColor = currentColor;
                autoUpdatedLineRenderer.endColor = currentColor;
            }
            
            if (animateTexture && autoUpdatedLineRenderer.sharedMaterial != null)
            {
                // We need to create a copy of the material to avoid affecting other objects
                if (Application.isPlaying && autoUpdatedLineRenderer.sharedMaterial == autoUpdatedLineRenderer.material)
                {
                    autoUpdatedLineRenderer.material = new Material(autoUpdatedLineRenderer.sharedMaterial);
                }
                
                if (Application.isPlaying)
                {
                    autoUpdatedLineRenderer.material.mainTextureOffset = textureOffsetSpring.GetCurrentValue();
                    autoUpdatedLineRenderer.material.mainTextureScale = textureScaleSpring.GetCurrentValue();
                }
            }
            
            if (animatePoints)
            {
                for (int i = 0; i < Mathf.Min(pointSprings.Count, currentPositions.Length); i++)
                {
                    Vector2 pos = pointSprings[i].GetCurrentValue();
                    currentPositions[i] = new Vector3(pos.x, pos.y, currentPositions[i].z);
                }
                
                autoUpdatedLineRenderer.SetPositions(currentPositions);
            }
        }

        public override bool IsValidSpringComponent()
        {
            bool res = true;

            if (autoUpdatedLineRenderer == null)
            {
                AddErrorReason($"{gameObject.name} autoUpdatedLineRenderer is null.");
                res = false;
            }
            
            // Check if at least one property is selected for animation
            bool anyPropertyAnimated = animateWidth || animateColor || animateTexture || animatePoints;
            
            if (!anyPropertyAnimated)
            {
                AddErrorReason($"{gameObject.name} has no LineRenderer properties selected to animate.");
                res = false;
            }

            return res;
        }
        
        // Animation toggle methods
        public void SetAnimateWidth(bool animate)
        {
            animateWidth = animate;
            if (initialized)
            {
                Initialize();
            }
        }
        
        public void SetAnimateColor(bool animate)
        {
            animateColor = animate;
            if (initialized)
            {
                Initialize();
            }
        }
        
        public void SetAnimateTexture(bool animate)
        {
            animateTexture = animate;
            if (initialized)
            {
                Initialize();
            }
        }
        
        public void SetAnimatePoints(bool animate)
        {
            animatePoints = animate;
            if (initialized)
            {
                Initialize();
            }
        }
        
        public void SetUseFollowTargets(bool use)
        {
            useFollowTargets = use;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            if (autoUpdatedLineRenderer == null)
            {
                autoUpdatedLineRenderer = GetComponent<LineRenderer>();
            }
            
            // Setup springs with reasonable defaults
            SetupDefaultSpringValues();
        }
        
        private void SetupDefaultSpringValues()
        {
            // Default values for width springs
            startWidthSpring.SetCommonForceAndDrag(true);
            startWidthSpring.SetCommonForce(80f);
            startWidthSpring.SetCommonDrag(8f);
            startWidthSpring.SetClampCurrentValue(true);
            startWidthSpring.SetMinValue(0f);
            
            endWidthSpring.SetCommonForceAndDrag(true);
            endWidthSpring.SetCommonForce(80f);
            endWidthSpring.SetCommonDrag(8f);
            endWidthSpring.SetClampCurrentValue(true);
            endWidthSpring.SetMinValue(0f);
            
            // Default values for color spring
            colorSpring.SetCommonForceAndDrag(true);
            colorSpring.SetCommonForce(50f);
            colorSpring.SetCommonDrag(10f);
            colorSpring.SetClampCurrentValues(true, true, true, true);
            colorSpring.SetMinValues(new Vector4(0, 0, 0, 0));
            colorSpring.SetMaxValues(new Vector4(1, 1, 1, 1));
            
            // Default values for texture springs
            textureOffsetSpring.SetCommonForceAndDrag(true);
            textureOffsetSpring.SetCommonForce(80f);
            textureOffsetSpring.SetCommonDrag(8f);
            
            textureScaleSpring.SetCommonForceAndDrag(true);
            textureScaleSpring.SetCommonForce(80f);
            textureScaleSpring.SetCommonDrag(8f);
            textureScaleSpring.SetClampCurrentValues(true, true);
            textureScaleSpring.SetMinValue(new Vector2(0.01f, 0.01f)); // Avoid zero scale
        }

        internal override Spring[] GetSpringsArray()
        {
            // Only return the springs that are being animated
            List<Spring> springs = new List<Spring>();
            
            if (animateWidth)
            {
                springs.Add(startWidthSpring);
                springs.Add(endWidthSpring);
            }
            
            if (animateColor)
            {
                springs.Add(colorSpring);
            }
            
            if (animateTexture)
            {
                springs.Add(textureOffsetSpring);
                springs.Add(textureScaleSpring);
            }
            
            if (animatePoints)
            {
                springs.AddRange(pointSprings);
            }
            
            return springs.ToArray();
        }
#endif
    }
}
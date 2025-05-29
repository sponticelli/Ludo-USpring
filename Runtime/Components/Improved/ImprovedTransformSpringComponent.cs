using UnityEngine;
using USpring.Core;
using System.Collections.Generic;

namespace USpring.Components.Improved
{
    /// <summary>
    /// Space type for transform operations.
    /// </summary>
    public enum SpaceType
    {
        LocalSpace,
        WorldSpace
    }

    /// <summary>
    /// Improved Transform spring component that manages position, rotation, and scale springs.
    /// Uses the new architecture for better organization and reduced code duplication.
    /// </summary>
    [AddComponentMenu("Ludo/USpring/Components/Improved/Transform Spring")]
    public class ImprovedTransformSpringComponent : SpringComponent
    {
        [Header("Target Transform")]
        [SerializeField] private Transform followerTransform;
        [SerializeField] private SpaceType spaceType = SpaceType.LocalSpace;

        [Header("Target Configuration")]
        [SerializeField] private bool useTransformAsTarget = false;
        [SerializeField] private Transform targetTransform;

        [Header("Spring Configuration")]
        [SerializeField] private SpringVector3 positionSpring = new SpringVector3();
        [SerializeField] private SpringVector3 scaleSpring = new SpringVector3();
        [SerializeField] private SpringRotation rotationSpring = new SpringRotation();

        [Header("Enable Springs")]
        [SerializeField] private bool enablePositionSpring = true;
        [SerializeField] private bool enableRotationSpring = true;
        [SerializeField] private bool enableScaleSpring = true;

        [Header("Common Settings")]
        [SerializeField] private bool useCommonSettings = true;
        [SerializeField] private float commonForce = 150f;
        [SerializeField] private float commonDrag = 10f;

        // Cached target values
        private Vector3 positionTarget;
        private Vector3 scaleTarget;
        private Quaternion rotationTarget;

        /// <summary>
        /// Gets or sets the follower transform.
        /// </summary>
        public Transform FollowerTransform
        {
            get => followerTransform;
            set
            {
                followerTransform = value;
                InvalidateValidation();
            }
        }

        /// <summary>
        /// Gets or sets the target transform.
        /// </summary>
        public Transform TargetTransform
        {
            get => targetTransform;
            set => targetTransform = value;
        }

        /// <summary>
        /// Gets the position spring events.
        /// </summary>
        public SpringEvents PositionEvents => positionSpring.springEvents;

        /// <summary>
        /// Gets the rotation spring events.
        /// </summary>
        public SpringEvents RotationEvents => rotationSpring.springEvents;

        /// <summary>
        /// Gets the scale spring events.
        /// </summary>
        public SpringEvents ScaleEvents => scaleSpring.springEvents;

        private void Awake()
        {
            // Auto-assign follower transform if not set
            if (followerTransform == null)
            {
                followerTransform = transform;
            }
        }

        public override void Initialize()
        {
            ConfigureSprings();
            base.Initialize();
        }

        private void ConfigureSprings()
        {
            if (useCommonSettings)
            {
                ConfigureCommonSettings();
            }
        }

        private void ConfigureCommonSettings()
        {
            if (enablePositionSpring)
            {
                positionSpring.SetCommonForceAndDrag(true);
                positionSpring.SetCommonForce(commonForce);
                positionSpring.SetCommonDrag(commonDrag);
            }

            if (enableRotationSpring)
            {
                rotationSpring.SetCommonForceAndDrag(true);
                rotationSpring.SetCommonForce(commonForce);
                rotationSpring.SetCommonDrag(commonDrag);
            }

            if (enableScaleSpring)
            {
                scaleSpring.SetCommonForceAndDrag(true);
                scaleSpring.SetCommonForce(commonForce);
                scaleSpring.SetCommonDrag(commonDrag);
            }
        }

        protected override void RegisterSprings()
        {
            if (enablePositionSpring)
            {
                RegisterSpring(positionSpring);
            }

            if (enableRotationSpring)
            {
                RegisterSpring(rotationSpring);
            }

            if (enableScaleSpring)
            {
                RegisterSpring(scaleSpring);
            }
        }

        protected override void SetCurrentValueByDefault()
        {
            if (followerTransform == null) return;

            if (enablePositionSpring)
            {
                Vector3 position = spaceType == SpaceType.LocalSpace
                    ? followerTransform.localPosition
                    : followerTransform.position;
                positionSpring.SetCurrentValue(position);
            }

            if (enableRotationSpring)
            {
                Quaternion rotation = spaceType == SpaceType.LocalSpace
                    ? followerTransform.localRotation
                    : followerTransform.rotation;
                rotationSpring.SetCurrentValue(rotation);
            }

            if (enableScaleSpring)
            {
                scaleSpring.SetCurrentValue(followerTransform.localScale);
            }
        }

        protected override void SetTargetByDefault()
        {
            if (useTransformAsTarget && targetTransform != null)
            {
                SetTargetsFromTransform();
            }
            else
            {
                SetTargetsFromCurrentValues();
            }
        }

        private void SetTargetsFromTransform()
        {
            if (enablePositionSpring)
            {
                Vector3 position = spaceType == SpaceType.LocalSpace
                    ? targetTransform.localPosition
                    : targetTransform.position;
                positionSpring.SetTarget(position);
                positionTarget = position;
            }

            if (enableRotationSpring)
            {
                Quaternion rotation = spaceType == SpaceType.LocalSpace
                    ? targetTransform.localRotation
                    : targetTransform.rotation;
                rotationSpring.SetTarget(rotation);
                rotationTarget = rotation;
            }

            if (enableScaleSpring)
            {
                scaleSpring.SetTarget(targetTransform.localScale);
                scaleTarget = targetTransform.localScale;
            }
        }

        private void SetTargetsFromCurrentValues()
        {
            if (followerTransform == null) return;

            if (enablePositionSpring)
            {
                Vector3 position = spaceType == SpaceType.LocalSpace
                    ? followerTransform.localPosition
                    : followerTransform.position;
                positionSpring.SetTarget(position);
                positionTarget = position;
            }

            if (enableRotationSpring)
            {
                Quaternion rotation = spaceType == SpaceType.LocalSpace
                    ? followerTransform.localRotation
                    : followerTransform.rotation;
                rotationSpring.SetTarget(rotation);
                rotationTarget = rotation;
            }

            if (enableScaleSpring)
            {
                scaleSpring.SetTarget(followerTransform.localScale);
                scaleTarget = followerTransform.localScale;
            }
        }

        public override bool IsValidSpringComponent()
        {
            var errorMessages = new List<string>();
            bool isValid = SpringComponentValidator.ValidateTransform(followerTransform, "FollowerTransform", errorMessages);

            if (useTransformAsTarget)
            {
                isValid &= SpringComponentValidator.ValidateTransform(targetTransform, "TargetTransform", errorMessages);
            }

            if (!isValid)
            {
                foreach (var error in errorMessages)
                {
                    AddErrorReason(error);
                }
            }

            return isValid;
        }

        protected virtual void Update()
        {
            if (!initialized || followerTransform == null)
            {
                return;
            }

            UpdateTargets();
            UpdateTransform();
        }

        private void UpdateTargets()
        {
            if (useTransformAsTarget && targetTransform != null)
            {
                SetTargetsFromTransform();
            }
        }

        private void UpdateTransform()
        {
            if (spaceType == SpaceType.LocalSpace)
            {
                UpdateTransformLocalSpace();
            }
            else
            {
                UpdateTransformWorldSpace();
            }
        }

        private void UpdateTransformLocalSpace()
        {
            if (enablePositionSpring)
            {
                followerTransform.localPosition = positionSpring.GetCurrentValue();
            }

            if (enableRotationSpring)
            {
                followerTransform.localRotation = rotationSpring.GetCurrentValue();
            }

            if (enableScaleSpring)
            {
                followerTransform.localScale = scaleSpring.GetCurrentValue();
            }
        }

        private void UpdateTransformWorldSpace()
        {
            if (enablePositionSpring)
            {
                followerTransform.position = positionSpring.GetCurrentValue();
            }

            if (enableRotationSpring)
            {
                followerTransform.rotation = rotationSpring.GetCurrentValue();
            }

            if (enableScaleSpring)
            {
                followerTransform.localScale = scaleSpring.GetCurrentValue();
            }
        }

        #region Public API
        public void SetPositionTarget(Vector3 target)
        {
            positionTarget = target;
            if (enablePositionSpring)
            {
                positionSpring.SetTarget(target);
            }
        }

        public void SetRotationTarget(Quaternion target)
        {
            rotationTarget = target;
            if (enableRotationSpring)
            {
                rotationSpring.SetTarget(target);
            }
        }

        public void SetScaleTarget(Vector3 target)
        {
            scaleTarget = target;
            if (enableScaleSpring)
            {
                scaleSpring.SetTarget(target);
            }
        }

        public Vector3 GetPositionTarget() => positionSpring.GetTarget();
        public Quaternion GetRotationTarget() => rotationSpring.GetTarget();
        public Vector3 GetScaleTarget() => scaleSpring.GetTarget();

        public Vector3 GetCurrentPosition() => positionSpring.GetCurrentValue();
        public Quaternion GetCurrentRotation() => rotationSpring.GetCurrentValue();
        public Vector3 GetCurrentScale() => scaleSpring.GetCurrentValue();
        #endregion

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            if (followerTransform == null)
            {
                followerTransform = transform;
            }
        }

        internal override Spring[] GetSpringsArray()
        {
            var springs = new List<Spring>();

            if (enablePositionSpring)
            {
                springs.Add(positionSpring);
            }

            if (enableRotationSpring)
            {
                springs.Add(rotationSpring);
            }

            if (enableScaleSpring)
            {
                springs.Add(scaleSpring);
            }

            return springs.ToArray();
        }
#endif
    }
}

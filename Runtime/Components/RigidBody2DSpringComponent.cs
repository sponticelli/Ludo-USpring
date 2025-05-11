using UnityEngine;
using USpring.Core;

namespace USpring.Components
{
    [AddComponentMenu("Ludo/USpring/Components/Rigidbody2D Spring")]
    public partial class RigidBody2DSpringComponent : SpringComponent
    {
        private const float VelocityFactor = 10f;

        [SerializeField] private SpringVector2 positionSpring = new SpringVector2();
        [SerializeField] private SpringFloat rotationSpring = new SpringFloat();

        public bool useTransformAsTarget;

        [SerializeField] private Rigidbody2D rigidBodyFollower;
        [SerializeField] private Transform target;

        // Position Spring methods
        public SpringEvents PositionEvents => positionSpring.springEvents;
        public Vector2 GetTargetPosition() => positionSpring.GetTarget();
        public void SetTargetPosition(Vector2 target) => positionSpring.SetTarget(target);
        public void SetTargetPosition(float target) => SetTargetPosition(Vector2.one * target);
        public Vector2 GetCurrentValuePosition() => positionSpring.GetCurrentValue();
        public void SetCurrentValuePosition(Vector2 currentValues) => positionSpring.SetCurrentValue(currentValues);
        public void SetCurrentValuePosition(float currentValues) => SetCurrentValuePosition(Vector2.one * currentValues);
        public Vector2 GetVelocityPosition() => positionSpring.GetVelocity();
        public void SetVelocityPosition(Vector2 velocity) => positionSpring.SetVelocity(velocity);
        public void SetVelocityPosition(float velocity) => SetVelocityPosition(Vector2.one * velocity);
        public void AddVelocityPosition(Vector2 velocityToAdd) => positionSpring.AddVelocity(velocityToAdd);
        public void ReachEquilibriumPosition() => positionSpring.ReachEquilibrium();
        public Vector2 GetForcePosition() => positionSpring.GetForce();
        public void SetForcePosition(Vector2 force) => positionSpring.SetForce(force);
        public void SetForcePosition(float force) => SetForcePosition(Vector2.one * force);
        public Vector2 GetDragPosition() => positionSpring.GetDrag();
        public void SetDragPosition(Vector2 drag) => positionSpring.SetDrag(drag);
        public void SetDragPosition(float drag) => SetDragPosition(Vector2.one * drag);
        public float GetCommonForcePosition() => positionSpring.GetCommonForce();
        public float GetCommonDragPosition() => positionSpring.GetCommonDrag();
        public void SetCommonForcePosition(float force)
        {
            positionSpring.SetCommonForceAndDrag(true);
            positionSpring.SetCommonForce(force);
        }
        public void SetCommonDragPosition(float drag)
        {
            positionSpring.SetCommonForceAndDrag(true);
            positionSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragPosition(float force, float drag)
        {
            SetCommonForcePosition(force);
            SetCommonDragPosition(drag);
        }
        public void SetMinValuesPosition(Vector2 minValue) => positionSpring.SetMinValue(minValue);
        public void SetMinValuesPosition(float minValue) => SetMinValuesPosition(Vector2.one * minValue);
        public void SetMaxValuesPosition(Vector2 maxValue) => positionSpring.SetMaxValues(maxValue);
        public void SetMaxValuesPosition(float maxValue) => SetMaxValuesPosition(Vector2.one * maxValue);
        public void SetClampCurrentValuesPosition(bool clampTargetX, bool clampTargetY) => 
            positionSpring.SetClampCurrentValues(clampTargetX, clampTargetY);
        public void SetClampTargetPosition(bool clampTargetX, bool clampTargetY) => 
            positionSpring.SetClampTarget(clampTargetX, clampTargetY);
        public void StopSpringOnClampPosition(bool stopX, bool stopY) => 
            positionSpring.StopSpringOnClamp(stopX, stopY);

        // Rotation Spring methods
        public SpringEvents RotationEvents => rotationSpring.springEvents;
        public float GetTargetRotation() => rotationSpring.GetTarget();
        public void SetTargetRotation(float targetAngle) => rotationSpring.SetTarget(targetAngle);
        public float GetCurrentValueRotation() => rotationSpring.GetCurrentValue();
        public void SetCurrentValueRotation(float currentAngle) => rotationSpring.SetCurrentValue(currentAngle);
        public float GetVelocityRotation() => rotationSpring.GetVelocity();
        public void SetVelocityRotation(float velocity) => rotationSpring.SetVelocity(velocity);
        public void AddVelocityRotation(float velocityToAdd) => rotationSpring.AddVelocity(velocityToAdd);
        public void ReachEquilibriumRotation() => rotationSpring.ReachEquilibrium();
        public float GetForceRotation() => rotationSpring.GetForce();
        public void SetForceRotation(float force) => rotationSpring.SetForce(force);
        public float GetDragRotation() => rotationSpring.GetDrag();
        public void SetDragRotation(float drag) => rotationSpring.SetDrag(drag);
        public float GetCommonForceRotation() => rotationSpring.GetCommonForce();
        public float GetCommonDragRotation() => rotationSpring.GetCommonDrag();
        public void SetCommonForceRotation(float force)
        {
            rotationSpring.SetCommonForceAndDrag(true);
            rotationSpring.SetCommonForce(force);
        }
        public void SetCommonDragRotation(float drag)
        {
            rotationSpring.SetCommonForceAndDrag(true);
            rotationSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDragRotation(float force, float drag)
        {
            SetCommonForceRotation(force);
            SetCommonDragRotation(drag);
        }
        public void SetMinValuesRotation(float minValue) => rotationSpring.SetMinValue(minValue);
        public void SetMaxValuesRotation(float maxValue) => rotationSpring.SetMaxValue(maxValue);
        public void SetClampCurrentValuesRotation(bool clamp) => rotationSpring.SetClampCurrentValue(clamp);
        public void SetClampTargetRotation(bool clamp) => rotationSpring.SetClampTarget(clamp);
        public void StopSpringOnClampRotation(bool stop) => rotationSpring.SetStopOnClamp(stop);

        private Vector2 positionTarget;
        private float rotationTarget;

        protected override void SetInitialValues()
        {
            if (!hasCustomInitialValues)
            {
                SetCurrentValueByDefault();
            }
            else
            {
                if (!positionSpring.useInitialValues)
                {
                    SetCurrentValuePositionByDefault();
                }
                if (!rotationSpring.useInitialValues)
                {
                    SetCurrentValueRotationByDefault();
                }
            }

            if (useTransformAsTarget)
            {
                UpdateTarget();
            }
            else
            {
                if (!hasCustomTarget)
                {
                    SetTargetByDefault();
                }
                else
                {
                    if (!positionSpring.useCustomTarget)
                    {
                        SetTargetPositionByDefault();
                    }
                    if (!rotationSpring.useCustomTarget)
                    {
                        SetTargetRotationByDefault();
                    }
                }
            }
        }

        protected override void SetCurrentValueByDefault()
        {
            SetCurrentValuePositionByDefault();
            SetCurrentValueRotationByDefault();
        }

        private void SetCurrentValuePositionByDefault()
        {
            SetCurrentValuePosition(rigidBodyFollower.position);
        }

        private void SetCurrentValueRotationByDefault()
        {
            SetCurrentValueRotation(rigidBodyFollower.rotation);
        }

        protected override void SetTargetByDefault()
        {
            SetTargetPositionByDefault();
            SetTargetRotationByDefault();
        }

        private void SetTargetPositionByDefault()
        {
            SetTargetPosition(rigidBodyFollower.position);
            positionTarget = GetTargetPosition();
        }

        private void SetTargetRotationByDefault()
        {
            SetTargetRotation(rigidBodyFollower.rotation);
            rotationTarget = GetTargetRotation();
        }

        protected override void RegisterSprings()
        {
            RegisterSpring(positionSpring);
            RegisterSpring(rotationSpring);
        }

        public void Update()
        {
            if (!initialized) { return; }
            UpdateTarget();
        }

        private void FixedUpdate()
        {
            if (!initialized) { return; }

            if (positionSpring.springEnabled)
            {
                Vector2 velocity = (positionSpring.GetCurrentValue() - rigidBodyFollower.position) * VelocityFactor;
                rigidBodyFollower.linearVelocity = velocity;
            }

            if (rotationSpring.springEnabled)
            {
                float targetRotation = rotationSpring.GetCurrentValue();
                ApplyAngularVelocityTowards(targetRotation);
            }
        }
        
        private void ApplyAngularVelocityTowards(float targetRotation)
        {
            float rotationDifference = Mathf.DeltaAngle(rigidBodyFollower.rotation, targetRotation);
            
            // Calculate angular velocity needed to reach target rotation
            float angularVelocity = rotationDifference / Time.fixedDeltaTime;
            
            // Assign to rigidbody
            rigidBodyFollower.angularVelocity = angularVelocity;
        }

        private void UpdateTarget()
        {
            if (useTransformAsTarget)
            {
                positionTarget = target.position;
                rotationTarget = target.rotation.eulerAngles.z; // For 2D we only care about Z rotation
            }

            if (positionSpring.springEnabled)
            {
                positionSpring.SetTarget(positionTarget);
            }

            if (rotationSpring.springEnabled)
            {
                rotationSpring.SetTarget(rotationTarget);
            }
        }
        
        public override bool IsValidSpringComponent()
        {
            bool res = true;

            if (useTransformAsTarget && target == null)
            {
                AddErrorReason($"{gameObject.name} useTransformAsTarget is enabled but target cannot be null");
                res = false;
            }
            if (rigidBodyFollower == null)
            {
                AddErrorReason($"{gameObject.name} rigidBodyFollower cannot be null");
                res = false;
            }

            return res;
        }

        #region ENABLE/DISABLE SPRING PROPERTIES
        public SpringVector2 PositionSpring
        {
            get => positionSpring;
            set => positionSpring = value;
        }

        public SpringFloat RotationSpring
        {
            get => rotationSpring;
            set => rotationSpring = value;
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(rigidBodyFollower == null)
            {
                rigidBodyFollower = GetComponent<Rigidbody2D>();
            }
        }

        internal override Spring[] GetSpringsArray()
        {
            Spring[] res = new Spring[]
            {
                positionSpring,
                rotationSpring
            };

            return res;
        }
#endif
    }
}
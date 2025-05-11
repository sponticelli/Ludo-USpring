using UnityEngine;
using USpring.Core.Interfaces;

namespace USpring.Core
{
    [System.Serializable]
    public class SpringColor : Spring, ISpringColor
    {
        public const int SpringSize = 4;

        private const int R = 0;
        private const int G = 1;
        private const int B = 2;
        private const int A = 3;

        public SpringColor() : base(SpringSize)
        {
            SetDefaultValues();
        }

        public override bool HasValidSize()
        {
            bool res = springValues.Length == SpringSize;
            return res;
        }

        public override int GetSpringSize()
        {
            return SpringSize;
        }

        private void SetDefaultValues()
        {
            SetCurrentValue(Color.white);
            SetTarget(Color.white);
        }

        // Implementation of ISpringColor interface methods
        public Color GetTarget()
        {
            return new Color(
                springValues[R].GetTarget(),
                springValues[G].GetTarget(),
                springValues[B].GetTarget(),
                springValues[A].GetTarget());
        }
        

        public Color GetTargetColor()
        {
            return new Color(
                springValues[R].GetTarget(),
                springValues[G].GetTarget(),
                springValues[B].GetTarget(),
                springValues[A].GetTarget());
        }

        public ISpringColor SetTarget(Color color)
        {
            springValues[R].SetTarget(color.r);
            springValues[G].SetTarget(color.g);
            springValues[B].SetTarget(color.b);
            springValues[A].SetTarget(color.a);
            return this;
        }


        Color ISpringColor.GetCurrentValue()
        {
            return new Color(
                springValues[R].GetCurrentValue(),
                springValues[G].GetCurrentValue(),
                springValues[B].GetCurrentValue(),
                springValues[A].GetCurrentValue());
        }

        // Keep the original method for backward compatibility
        public Vector4 GetCurrentValue()
        {
            return new Vector4(
                springValues[R].GetCurrentValue(),
                springValues[G].GetCurrentValue(),
                springValues[B].GetCurrentValue(),
                springValues[A].GetCurrentValue());
        }

        public Color GetCurrentColor()
        {
            return new Color(
                springValues[R].GetCurrentValue(),
                springValues[G].GetCurrentValue(),
                springValues[B].GetCurrentValue(),
                springValues[A].GetCurrentValue());
        }

        public ISpringColor SetCurrentValue(Color color)
        {
            springValues[R].SetCurrentValue(color.r);
            springValues[G].SetCurrentValue(color.g);
            springValues[B].SetCurrentValue(color.b);
            springValues[A].SetCurrentValue(color.a);
            return this;
        }
        

        public Vector4 GetVelocity()
        {
            return new Vector4(
                springValues[R].GetVelocity(),
                springValues[G].GetVelocity(),
                springValues[B].GetVelocity(),
                springValues[A].GetVelocity());
        }

        ISpringColor ISpringColor.AddVelocity(Vector4 velocity)
        {
            springValues[R].AddVelocity(velocity.x);
            springValues[G].AddVelocity(velocity.y);
            springValues[B].AddVelocity(velocity.z);
            springValues[A].AddVelocity(velocity.w);
            return this;
        }

        // Keep the original method for backward compatibility
        public void AddVelocity(Vector4 velocity)
        {
            springValues[R].AddVelocity(velocity.x);
            springValues[G].AddVelocity(velocity.y);
            springValues[B].AddVelocity(velocity.z);
            springValues[A].AddVelocity(velocity.w);
        }

        ISpringColor ISpringColor.SetVelocity(Vector4 velocity)
        {
            springValues[R].SetVelocity(velocity.x);
            springValues[G].SetVelocity(velocity.y);
            springValues[B].SetVelocity(velocity.z);
            springValues[A].SetVelocity(velocity.w);
            return this;
        }

        // Keep the original method for backward compatibility
        public void SetVelocity(Vector4 velocity)
        {
            springValues[R].SetVelocity(velocity.x);
            springValues[G].SetVelocity(velocity.y);
            springValues[B].SetVelocity(velocity.z);
            springValues[A].SetVelocity(velocity.w);
        }

        public Vector4 GetForce()
        {
            return new Vector4(
                GetForceByIndex(R),
                GetForceByIndex(G),
                GetForceByIndex(B),
                GetForceByIndex(A));
        }

        ISpringColor ISpringColor.SetForce(Vector4 force)
        {
            SetForceByIndex(R, force.x);
            SetForceByIndex(G, force.y);
            SetForceByIndex(B, force.z);
            SetForceByIndex(A, force.w);
            return this;
        }

        // Keep the original method for backward compatibility
        public void SetForce(Vector4 force)
        {
            SetForceByIndex(R, force.x);
            SetForceByIndex(G, force.y);
            SetForceByIndex(B, force.z);
            SetForceByIndex(A, force.w);
        }

        ISpringColor ISpringColor.SetForce(float force)
        {
            SetForce(Vector4.one * force);
            return this;
        }

        // Keep the original method for backward compatibility
        public void SetForce(float force)
        {
            SetForce(Vector4.one * force);
        }

        public Vector4 GetDrag()
        {
            return new Vector4(
                GetDragByIndex(R),
                GetDragByIndex(G),
                GetDragByIndex(B),
                GetDragByIndex(A));
        }

        ISpringColor ISpringColor.SetDrag(Vector4 drag)
        {
            SetDragByIndex(R, drag.x);
            SetDragByIndex(G, drag.y);
            SetDragByIndex(B, drag.z);
            SetDragByIndex(A, drag.w);
            return this;
        }

        // Keep the original method for backward compatibility
        public void SetDrag(Vector4 drag)
        {
            SetDragByIndex(R, drag.x);
            SetDragByIndex(G, drag.y);
            SetDragByIndex(B, drag.z);
            SetDragByIndex(A, drag.w);
        }

        ISpringColor ISpringColor.SetDrag(float drag)
        {
            SetDrag(Vector4.one * drag);
            return this;
        }

        // Keep the original method for backward compatibility
        public void SetDrag(float drag)
        {
            SetDrag(Vector4.one * drag);
        }

        ISpringColor ISpringColor.SetMinValues(Vector4 minValues)
        {
            SetMinValueByIndex(R, minValues.x);
            SetMinValueByIndex(G, minValues.y);
            SetMinValueByIndex(B, minValues.z);
            SetMinValueByIndex(A, minValues.w);
            return this;
        }

        // Keep the original method for backward compatibility
        public void SetMinValues(Vector4 minValues)
        {
            SetMinValueByIndex(R, minValues.x);
            SetMinValueByIndex(G, minValues.y);
            SetMinValueByIndex(B, minValues.z);
            SetMinValueByIndex(A, minValues.w);
        }

        ISpringColor ISpringColor.SetMinValues(float uniformMinValue)
        {
            SetMinValues(Vector4.one * uniformMinValue);
            return this;
        }

        public Vector4 GetMinValues()
        {
            return new Vector4(
                springValues[R].GetMinValue(),
                springValues[G].GetMinValue(),
                springValues[B].GetMinValue(),
                springValues[A].GetMinValue());
        }

        public void SetMinValueR(float minValue)
        {
            SetMinValueByIndex(R, minValue);
        }

        public void SetMinValueG(float minValue)
        {
            SetMinValueByIndex(G, minValue);
        }

        public void SetMinValueB(float minValue)
        {
            SetMinValueByIndex(B, minValue);
        }

        public void SetMinValueA(float minValue)
        {
            SetMinValueByIndex(A, minValue);
        }

        ISpringColor ISpringColor.SetMaxValues(Vector4 maxValues)
        {
            SetMaxValueByIndex(R, maxValues.x);
            SetMaxValueByIndex(G, maxValues.y);
            SetMaxValueByIndex(B, maxValues.z);
            SetMaxValueByIndex(A, maxValues.w);
            return this;
        }

        // Keep the original method for backward compatibility
        public void SetMaxValues(Vector4 maxValues)
        {
            SetMaxValueByIndex(R, maxValues.x);
            SetMaxValueByIndex(G, maxValues.y);
            SetMaxValueByIndex(B, maxValues.z);
            SetMaxValueByIndex(A, maxValues.w);
        }

        ISpringColor ISpringColor.SetMaxValues(float uniformMaxValue)
        {
            SetMaxValues(Vector4.one * uniformMaxValue);
            return this;
        }

        public Vector4 GetMaxValues()
        {
            return new Vector4(
                springValues[R].GetMaxValue(),
                springValues[G].GetMaxValue(),
                springValues[B].GetMaxValue(),
                springValues[A].GetMaxValue());
        }

        public void SetMaxValueR(float maxValue)
        {
            SetMaxValueByIndex(R, maxValue);
        }

        public void SetMaxValueG(float maxValue)
        {
            SetMaxValueByIndex(G, maxValue);
        }

        public void SetMaxValueB(float maxValue)
        {
            SetMaxValueByIndex(B, maxValue);
        }

        public void SetMaxValueA(float maxValue)
        {
            SetMaxValueByIndex(A, maxValue);
        }

        ISpringColor ISpringColor.SetClampRange(Vector4 minValues, Vector4 maxValues)
        {
            SetMinValues(minValues);
            SetMaxValues(maxValues);
            return this;
        }

        ISpringColor ISpringColor.SetClampRange(float uniformMinValue, float uniformMaxValue)
        {
            SetMinValues(Vector4.one * uniformMinValue);
            SetMaxValues(Vector4.one * uniformMaxValue);
            return this;
        }

        ISpringColor ISpringColor.SetClampTarget(bool enabled)
        {
            SetClampTarget(enabled, enabled, enabled, enabled);
            return this;
        }

        // Keep the original method for backward compatibility
        public void SetClampCurrentValues(bool clampR, bool clampG, bool clampB, bool clampA)
        {
            SetClampCurrentValueByIndex(R, clampR);
            SetClampCurrentValueByIndex(G, clampG);
            SetClampCurrentValueByIndex(B, clampB);
            SetClampCurrentValueByIndex(A, clampA);
        }

        ISpringColor ISpringColor.SetClampCurrentValue(bool enabled)
        {
            SetClampCurrentValues(enabled, enabled, enabled, enabled);
            return this;
        }

        // Keep the original method for backward compatibility
        public void SetClampTarget(bool clampR, bool clampG, bool clampB, bool clampA)
        {
            SetClampTargetByIndex(R, clampR);
            SetClampTargetByIndex(G, clampG);
            SetClampTargetByIndex(B, clampB);
            SetClampTargetByIndex(A, clampA);
        }

        ISpringColor ISpringColor.SetStopOnClamp(bool enabled)
        {
            StopSpringOnClamp(enabled, enabled, enabled, enabled);
            return this;
        }

        public bool DoesStopOnClamp()
        {
            // Return true if any component is set to stop on clamp
            return springValues[R].GetStopOnClamp() ||
                   springValues[G].GetStopOnClamp() ||
                   springValues[B].GetStopOnClamp() ||
                   springValues[A].GetStopOnClamp();
        }

        // Keep the original method for backward compatibility
        public void StopSpringOnClamp(bool stopR, bool stopG, bool stopB, bool stopA)
        {
            SetStopSpringOnCurrentValueClampByIndex(R, stopR);
            SetStopSpringOnCurrentValueClampByIndex(G, stopG);
            SetStopSpringOnCurrentValueClampByIndex(B, stopB);
            SetStopSpringOnCurrentValueClampByIndex(A, stopA);
        }

        public void StopSpringOnClampR(bool stop)
        {
            SetStopSpringOnCurrentValueClampByIndex(R, stop);
        }

        public void StopSpringOnClampG(bool stop)
        {
            SetStopSpringOnCurrentValueClampByIndex(G, stop);
        }

        public void StopSpringOnClampB(bool stop)
        {
            SetStopSpringOnCurrentValueClampByIndex(B, stop);
        }

        public void StopSpringOnClampA(bool stop)
        {
            SetStopSpringOnCurrentValueClampByIndex(A, stop);
        }

        public ISpringColor ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector4 minValues, Vector4 maxValues)
        {
            SetClampTarget(clampTarget, clampTarget, clampTarget, clampTarget);
            SetClampCurrentValues(clampCurrentValue, clampCurrentValue, clampCurrentValue, clampCurrentValue);
            StopSpringOnClamp(stopOnClamp, stopOnClamp, stopOnClamp, stopOnClamp);
            SetMinValues(minValues);
            SetMaxValues(maxValues);
            return this;
        }

        public ISpringColor Configure(Vector4 force, Vector4 drag, Color initialValue, Color target)
        {
            SetForce(force);
            SetDrag(drag);
            SetCurrentValue(initialValue);
            SetTarget(target);
            return this;
        }

        public ISpringColor Configure(float uniformForce, float uniformDrag, Color initialValue, Color target)
        {
            SetForce(uniformForce);
            SetDrag(uniformDrag);
            SetCurrentValue(initialValue);
            SetTarget(target);
            return this;
        }

        public ISpringColor Clone()
        {
            SpringColor clone = new SpringColor();

            // Copy base spring properties
            clone.commonForceAndDrag = this.commonForceAndDrag;
            clone.commonForce = this.commonForce;
            clone.commonDrag = this.commonDrag;
            clone.springEnabled = this.springEnabled;
            clone.clampingEnabled = this.clampingEnabled;

            // Clone spring values
            for (int i = 0; i < SpringSize; i++)
            {
                clone.springValues[i].SetTarget(this.springValues[i].GetTarget());
                clone.springValues[i].SetCurrentValue(this.springValues[i].GetCurrentValue());
                clone.springValues[i].SetVelocity(this.springValues[i].GetVelocity());
                clone.springValues[i].SetForce(this.springValues[i].GetForce());
                clone.springValues[i].SetDrag(this.springValues[i].GetDrag());
                clone.springValues[i].SetMinValue(this.springValues[i].GetMinValue());
                clone.springValues[i].SetMaxValue(this.springValues[i].GetMaxValue());
                clone.springValues[i].SetClampTarget(this.springValues[i].GetClampTarget());
                clone.springValues[i].SetClampCurrentValue(this.springValues[i].GetClampCurrentValue());
                clone.springValues[i].SetStopOnClamp(this.springValues[i].GetStopOnClamp());
            }

            return clone;
        }
    }
}
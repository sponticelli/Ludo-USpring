using UnityEngine;
using USpring.Core.Interfaces;

namespace USpring.Core
{
	[System.Serializable]
	public class SpringVector4 : Spring, ISpringVector4
	{
		public const int SpringSize = 4;

		private const int X = 0;
		private const int Y = 1;
		private const int Z = 2;
		private const int W = 3;

		public SpringVector4() : base(SpringSize)
		{

		}

		public override int GetSpringSize()
		{
			return SpringSize;
		}

		public override bool HasValidSize()
		{
			bool res = springValues.Length == SpringSize;
			return res;
		}

		public Vector4 GetTarget()
		{
			Vector4 res = new Vector4(
				springValues[X].GetTarget(),
				springValues[Y].GetTarget(),
				springValues[Z].GetTarget(),
				springValues[W].GetTarget());

			return res;
		}

		public ISpringVector4 SetTarget(Vector4 target)
		{
			springValues[X].SetTarget(target.x);
			springValues[Y].SetTarget(target.y);
			springValues[Z].SetTarget(target.z);
			springValues[W].SetTarget(target.w);
			return this;
		}

		public ISpringVector4 SetTarget(float value)
		{
			SetTarget(Vector4.one * value);
			return this;
		}

		public Vector4 GetCurrentValue()
		{
			Vector4 res = new Vector4(springValues[X].GetCurrentValue(), springValues[Y].GetCurrentValue(), springValues[Z].GetCurrentValue(), springValues[W].GetCurrentValue());
			return res;
		}

		public ISpringVector4 SetCurrentValue(Vector4 value)
		{
			springValues[X].SetCurrentValue(value.x);
			springValues[Y].SetCurrentValue(value.y);
			springValues[Z].SetCurrentValue(value.z);
			springValues[W].SetCurrentValue(value.w);
			return this;
		}

		public ISpringVector4 SetCurrentValue(float value)
		{
			SetCurrentValue(Vector4.one * value);
			return this;
		}

		public Vector4 GetVelocity()
		{
			Vector4 res = new Vector4(springValues[X].GetVelocity(), springValues[Y].GetVelocity(), springValues[Z].GetVelocity(), springValues[W].GetVelocity());
			return res;
		}

		public ISpringVector4 AddVelocity(Vector4 velocity)
		{
			springValues[X].AddVelocity(velocity.x);
			springValues[Y].AddVelocity(velocity.y);
			springValues[Z].AddVelocity(velocity.z);
			springValues[W].AddVelocity(velocity.w);
			return this;
		}

		public ISpringVector4 AddVelocity(float value)
		{
			AddVelocity(Vector4.one * value);
			return this;
		}

		public ISpringVector4 SetVelocity(Vector4 velocity)
		{
			springValues[X].SetVelocity(velocity.x);
			springValues[Y].SetVelocity(velocity.y);
			springValues[Z].SetVelocity(velocity.z);
			springValues[W].SetVelocity(velocity.w);
			return this;
		}

		public ISpringVector4 SetVelocity(float value)
		{
			SetVelocity(Vector4.one * value);
			return this;
		}

		public Vector4 GetForce()
		{
			Vector4 res = new Vector4(
				GetForceByIndex(X),
				GetForceByIndex(Y),
				GetForceByIndex(Z),
				GetForceByIndex(W)
			);

			return res;
		}

		public ISpringVector4 SetForce(Vector4 force)
		{
			SetForceByIndex(X, force.x);
			SetForceByIndex(Y, force.y);
			SetForceByIndex(Z, force.z);
			SetForceByIndex(W, force.w);
			return this;
		}

		public ISpringVector4 SetForce(float value)
		{
			SetForce(Vector4.one * value);
			return this;
		}

		public Vector4 GetDrag()
		{
			Vector4 res = new Vector4(
				GetDragByIndex(X),
				GetDragByIndex(Y),
				GetDragByIndex(Z),
				GetDragByIndex(W)
			);

			return res;
		}

		public ISpringVector4 SetDrag(Vector4 drag)
		{
			SetDragByIndex(X, drag.x);
			SetDragByIndex(Y, drag.y);
			SetDragByIndex(Z, drag.z);
			SetDragByIndex(W, drag.w);
			return this;
		}

		public ISpringVector4 SetDrag(float value)
		{
			SetDrag(Vector4.one * value);
			return this;
		}
		public ISpringVector4 SetMinValues(Vector4 minValues)
		{
			SetMinValueByIndex(X, minValues.x);
			SetMinValueByIndex(Y, minValues.y);
			SetMinValueByIndex(Z, minValues.z);
			SetMinValueByIndex(W, minValues.w);
			return this;
		}

		public ISpringVector4 SetMinValues(float value)
		{
			SetMinValues(Vector4.one * value);
			return this;
		}

		public Vector4 GetMinValues()
		{
			return new Vector4(
				springValues[X].GetMinValue(),
				springValues[Y].GetMinValue(),
				springValues[Z].GetMinValue(),
				springValues[W].GetMinValue());
		}

		public void SetMinValueX(float minValue)
		{
			SetMinValueByIndex(X, minValue);
		}

		public void SetMinValueY(float minValue)
		{
			SetMinValueByIndex(Y, minValue);
		}

		public void SetMinValueZ(float minValue)
		{
			SetMinValueByIndex(Z, minValue);
		}

		public void SetMinValueW(float minValue)
		{
			SetMinValueByIndex(W, minValue);
		}

		public ISpringVector4 SetMaxValues(Vector4 maxValues)
		{
			SetMaxValueByIndex(X, maxValues.x);
			SetMaxValueByIndex(Y, maxValues.y);
			SetMaxValueByIndex(Z, maxValues.z);
			SetMaxValueByIndex(W, maxValues.w);
			return this;
		}

		public ISpringVector4 SetMaxValues(float value)
		{
			SetMaxValues(Vector4.one * value);
			return this;
		}

		public Vector4 GetMaxValues()
		{
			return new Vector4(
				springValues[X].GetMaxValue(),
				springValues[Y].GetMaxValue(),
				springValues[Z].GetMaxValue(),
				springValues[W].GetMaxValue());
		}

		public void SetMaxValueX(float maxValue)
		{
			SetMaxValueByIndex(X, maxValue);
		}

		public void SetMaxValueY(float maxValue)
		{
			SetMaxValueByIndex(Y, maxValue);
		}

		public void SetMaxValueZ(float maxValue)
		{
			SetMaxValueByIndex(Z, maxValue);
		}

		public void SetMaxValueW(float maxValue)
		{
			SetMaxValueByIndex(W, maxValue);
		}

		public void SetClampCurrentValues(bool clampX, bool clampY, bool clampZ, bool clampW)
		{
			SetClampCurrentValueByIndex(X, clampX);
			SetClampCurrentValueByIndex(Y, clampY);
			SetClampCurrentValueByIndex(Z, clampZ);
			SetClampCurrentValueByIndex(W, clampW);
		}

		public void SetClampTarget(bool clampTargetX, bool clampTargetY, bool clampTargetZ, bool clampTargetW)
		{
			SetStopSpringOnCurrentValueClampByIndex(X, clampTargetX);
			SetStopSpringOnCurrentValueClampByIndex(Y, clampTargetY);
			SetStopSpringOnCurrentValueClampByIndex(Z, clampTargetZ);
			SetStopSpringOnCurrentValueClampByIndex(W, clampTargetW);
		}

		public void StopSpringOnClamp(bool stopX, bool stopY, bool stopZ, bool stopW)
		{
			SetStopSpringOnCurrentValueClampByIndex(X, stopX);
			SetStopSpringOnCurrentValueClampByIndex(Y, stopY);
			SetStopSpringOnCurrentValueClampByIndex(Z, stopZ);
			SetStopSpringOnCurrentValueClampByIndex(W, stopW);
		}

		public void StopSpringOnClampX(bool stop)
		{
			SetStopSpringOnCurrentValueClampByIndex(X, stop);
		}

		public void StopSpringOnClampY(bool stop)
		{
			SetStopSpringOnCurrentValueClampByIndex(Y, stop);
		}

		public void StopSpringOnClampZ(bool stop)
		{
			SetStopSpringOnCurrentValueClampByIndex(Z, stop);
		}

		public void StopSpringOnClampW(bool stop)
		{
			SetStopSpringOnCurrentValueClampByIndex(W, stop);
		}

		// Interface implementations for clamping
		public ISpringVector4 SetClampRange(Vector4 minValues, Vector4 maxValues)
		{
			SetMinValues(minValues);
			SetMaxValues(maxValues);
			return this;
		}

		public ISpringVector4 SetClampRange(float uniformMinValue, float uniformMaxValue)
		{
			SetMinValues(Vector4.one * uniformMinValue);
			SetMaxValues(Vector4.one * uniformMaxValue);
			return this;
		}

		public ISpringVector4 SetClampTarget(bool enabled)
		{
			SetClampTarget(enabled, enabled, enabled, enabled);
			return this;
		}

		public new bool IsClampTargetEnabled()
		{
			return springValues[X].GetClampTarget() || springValues[Y].GetClampTarget() ||
			       springValues[Z].GetClampTarget() || springValues[W].GetClampTarget();
		}

		public ISpringVector4 SetClampCurrentValue(bool enabled)
		{
			SetClampCurrentValues(enabled, enabled, enabled, enabled);
			return this;
		}

		public new bool IsClampCurrentValueEnabled()
		{
			return springValues[X].GetClampCurrentValue() || springValues[Y].GetClampCurrentValue() ||
			       springValues[Z].GetClampCurrentValue() || springValues[W].GetClampCurrentValue();
		}

		public ISpringVector4 SetStopOnClamp(bool enabled)
		{
			StopSpringOnClamp(enabled, enabled, enabled, enabled);
			return this;
		}

		public bool DoesStopOnClamp()
		{
			return springValues[X].GetStopOnClamp() || springValues[Y].GetStopOnClamp() ||
			       springValues[Z].GetStopOnClamp() || springValues[W].GetStopOnClamp();
		}

		public ISpringVector4 ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector4 minValues, Vector4 maxValues)
		{
			SetClampTarget(clampTarget);
			SetClampCurrentValue(clampCurrentValue);
			SetStopOnClamp(stopOnClamp);
			SetMinValues(minValues);
			SetMaxValues(maxValues);
			return this;
		}

		public ISpringVector4 Configure(Vector4 force, Vector4 drag, Vector4 initialValue, Vector4 target)
		{
			SetForce(force);
			SetDrag(drag);
			SetCurrentValue(initialValue);
			SetTarget(target);
			return this;
		}

		public ISpringVector4 Configure(float uniformForce, float uniformDrag, Vector4 initialValue, Vector4 target)
		{
			SetForce(uniformForce);
			SetDrag(uniformDrag);
			SetCurrentValue(initialValue);
			SetTarget(target);
			return this;
		}

		public ISpringVector4 Clone()
		{
			SpringVector4 clone = new SpringVector4();

			// Copy basic properties
			clone.commonForceAndDrag = this.commonForceAndDrag;
			clone.commonForce = this.commonForce;
			clone.commonDrag = this.commonDrag;
			clone.springEnabled = this.springEnabled;
			clone.clampingEnabled = this.clampingEnabled;
			clone.eventsEnabled = this.eventsEnabled;

			// Initialize the clone
			clone.Initialize();

			// Copy spring values
			for (int i = 0; i < springValues.Length; i++)
			{
				if (i < clone.springValues.Length)
				{
					clone.springValues[i].SetForce(springValues[i].GetForce());
					clone.springValues[i].SetDrag(springValues[i].GetDrag());
					clone.springValues[i].SetMinValue(springValues[i].GetMinValue());
					clone.springValues[i].SetMaxValue(springValues[i].GetMaxValue());
					clone.springValues[i].SetClampTarget(springValues[i].GetClampTarget());
					clone.springValues[i].SetClampCurrentValue(springValues[i].GetClampCurrentValue());
					clone.springValues[i].SetStopOnClamp(springValues[i].GetStopOnClamp());
					clone.springValues[i].SetTarget(this.springValues[i].GetTarget());
					clone.springValues[i].SetCurrentValue(this.springValues[i].GetCurrentValue());
					clone.springValues[i].SetVelocity(this.springValues[i].GetVelocity());
				}
			}

			return clone;
		}
	}
}
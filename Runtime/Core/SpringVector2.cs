using UnityEngine;
using USpring.Core.Interfaces;

namespace USpring.Core
{
	[System.Serializable]
	public class SpringVector2 : Spring, ISpringVector2
	{
		public const int SpringSize = 2;

		private const int X = 0;
		private const int Y = 1;

		public SpringVector2() : base(SpringSize)
		{

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

		public Vector2 GetTarget()
		{
			Vector2 res = new Vector2(
				springValues[X].GetTarget(),
				springValues[Y].GetTarget());

			return res;
		}

		public ISpringVector2 SetTarget(Vector2 target)
		{
			springValues[X].SetTarget(target.x);
			springValues[Y].SetTarget(target.y);
			return this;
		}

		public ISpringVector2 SetTarget(float target)
		{
			SetTarget(Vector2.one * target);
			return this;
		}

		public Vector2 GetCurrentValue()
		{
			Vector2 res = new Vector2(springValues[X].GetCurrentValue(), springValues[Y].GetCurrentValue());
			return res;
		}

		public ISpringVector2 SetCurrentValue(Vector2 value)
		{
			springValues[X].SetCurrentValue(value.x);
			springValues[Y].SetCurrentValue(value.y);
			return this;
		}

		public ISpringVector2 SetCurrentValue(float value)
		{
			SetCurrentValue(Vector2.one * value);
			return this;
		}

		public Vector2 GetVelocity()
		{
			Vector2 res = new Vector2(springValues[X].GetVelocity(), springValues[Y].GetVelocity());
			return res;
		}

		public ISpringVector2 AddVelocity(Vector2 velocity)
		{
			springValues[X].AddVelocity(velocity.x);
			springValues[Y].AddVelocity(velocity.y);
			return this;
		}

		public ISpringVector2 AddVelocity(float value)
		{
			AddVelocity(Vector2.one * value);
			return this;
		}

		public ISpringVector2 SetVelocity(Vector2 velocity)
		{
			springValues[X].SetVelocity(velocity.x);
			springValues[Y].SetVelocity(velocity.y);
			return this;
		}

		public ISpringVector2 SetVelocity(float value)
		{
			SetVelocity(Vector2.one * value);
			return this;
		}

		public Vector2 GetForce()
		{
			Vector2 res = new Vector2(GetForceByIndex(X), GetForceByIndex(Y));
			return res;
		}

		public ISpringVector2 SetForce(Vector2 force)
		{
			SetForceByIndex(X, force.x);
			SetForceByIndex(Y, force.y);
			return this;
		}

		public ISpringVector2 SetForce(float value)
		{
			SetForce(Vector2.one * value);
			return this;
		}

		public Vector2 GetDrag()
		{
			Vector2 res = new Vector2(GetDragByIndex(X), GetDragByIndex(Y));
			return res;
		}

		public ISpringVector2 SetDrag(Vector2 drag)
		{
			SetDragByIndex(X, drag.x);
			SetDragByIndex(Y, drag.y);
			return this;
		}

		public ISpringVector2 SetDrag(float value)
		{
			SetDrag(Vector2.one * value);
			return this;
		}

		public ISpringVector2 SetMinValues(Vector2 minValues)
		{
			SetMinValueByIndex(X, minValues.x);
			SetMinValueByIndex(Y, minValues.y);
			return this;
		}

		public ISpringVector2 SetMinValues(float value)
		{
			SetMinValues(Vector2.one * value);
			return this;
		}

		public Vector2 GetMinValues()
		{
			return new Vector2(
				springValues[X].GetMinValue(),
				springValues[Y].GetMinValue());
		}

		// Backward compatibility method
		public void SetMinValue(Vector2 minValues)
		{
			SetMinValues(minValues);
		}

		public void SetMinValueX(float minValue)
		{
			SetMinValueByIndex(X, minValue);
		}

		public void SetMinValueY(float minValue)
		{
			SetMinValueByIndex(Y, minValue);
		}

		public ISpringVector2 SetMaxValues(Vector2 maxValues)
		{
			SetMaxValueByIndex(X, maxValues.x);
			SetMaxValueByIndex(Y, maxValues.y);
			return this;
		}

		public ISpringVector2 SetMaxValues(float value)
		{
			SetMaxValues(Vector2.one * value);
			return this;
		}

		public Vector2 GetMaxValues()
		{
			return new Vector2(
				springValues[X].GetMaxValue(),
				springValues[Y].GetMaxValue());
		}

		public void SetMaxValueX(float maxValue)
		{
			SetMaxValueByIndex(X, maxValue);
		}

		public void SetMaxValueY(float maxValue)
		{
			SetMaxValueByIndex(Y, maxValue);
		}

		public void StopSpringOnClamp(bool stopX, bool stopY)
		{
			SetStopSpringOnCurrentValueClampByIndex(X, stopX);
			SetStopSpringOnCurrentValueClampByIndex(Y, stopY);
		}

		public void StopSpringOnClampX(bool stop)
		{
			SetStopSpringOnCurrentValueClampByIndex(X, stop);
		}

		public void StopSpringOnClampY(bool stop)
		{
			SetStopSpringOnCurrentValueClampByIndex(Y, stop);
		}

		public void SetClampTarget(bool clampTargetX, bool clampTargetY)
		{
			SetClampTargetByIndex(X, clampTargetX);
			SetClampTargetByIndex(Y, clampTargetY);
		}

		public void SetClampCurrentValues(bool clampCurrentValueX, bool clampCurrentValueY)
		{
			SetClampCurrentValueByIndex(X, clampCurrentValueX);
			SetClampCurrentValueByIndex(Y, clampCurrentValueY);
		}

		// Interface implementations for clamping
		public ISpringVector2 SetClampRange(Vector2 minValues, Vector2 maxValues)
		{
			SetMinValues(minValues);
			SetMaxValues(maxValues);
			return this;
		}

		public ISpringVector2 SetClampRange(float uniformMinValue, float uniformMaxValue)
		{
			SetMinValues(Vector2.one * uniformMinValue);
			SetMaxValues(Vector2.one * uniformMaxValue);
			return this;
		}

		public ISpringVector2 SetClampTarget(bool enabled)
		{
			SetClampTarget(enabled, enabled);
			return this;
		}

		public new bool IsClampTargetEnabled()
		{
			return springValues[X].GetClampTarget() || springValues[Y].GetClampTarget();
		}

		public ISpringVector2 SetClampCurrentValue(bool enabled)
		{
			SetClampCurrentValues(enabled, enabled);
			return this;
		}

		public new bool IsClampCurrentValueEnabled()
		{
			return springValues[X].GetClampCurrentValue() || springValues[Y].GetClampCurrentValue();
		}

		public ISpringVector2 SetStopOnClamp(bool enabled)
		{
			StopSpringOnClamp(enabled, enabled);
			return this;
		}

		public bool DoesStopOnClamp()
		{
			return springValues[X].GetStopOnClamp() || springValues[Y].GetStopOnClamp();
		}

		public ISpringVector2 ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector2 minValues, Vector2 maxValues)
		{
			SetClampTarget(clampTarget);
			SetClampCurrentValue(clampCurrentValue);
			SetStopOnClamp(stopOnClamp);
			SetMinValues(minValues);
			SetMaxValues(maxValues);
			return this;
		}

		public ISpringVector2 Configure(Vector2 force, Vector2 drag, Vector2 initialValue, Vector2 target)
		{
			SetForce(force);
			SetDrag(drag);
			SetCurrentValue(initialValue);
			SetTarget(target);
			return this;
		}

		public ISpringVector2 Configure(float uniformForce, float uniformDrag, Vector2 initialValue, Vector2 target)
		{
			SetForce(uniformForce);
			SetDrag(uniformDrag);
			SetCurrentValue(initialValue);
			SetTarget(target);
			return this;
		}

		public ISpringVector2 Clone()
		{
			SpringVector2 clone = new SpringVector2();

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
using UnityEngine;
using USpring.Core.Interfaces;

namespace USpring.Core
{
	[System.Serializable]
	public class SpringVector3 : Spring, ISpringVector3
	{
		public const int SpringSize = 3;

		private const int X = 0;
		private const int Y = 1;
		private const int Z = 2;

		public SpringVector3() : base(SpringSize)
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

		// Implementation of ISpringVector3 interface methods
		public Vector3 GetTarget()
		{
			Vector3 res = new Vector3(
				springValues[X].GetTarget(),
				springValues[Y].GetTarget(),
				springValues[Z].GetTarget());

			return res;
		}

		// Explicit interface implementation
		public ISpringVector3 SetTarget(Vector3 target)
		{
			springValues[X].SetTarget(target.x);
			springValues[Y].SetTarget(target.y);
			springValues[Z].SetTarget(target.z);
			return this;
		}


		// Explicit interface implementation
		public ISpringVector3 SetTarget(float value)
		{
			SetTarget(Vector3.one * value);
			return this;
		}


		public Vector3 GetCurrentValue()
		{
			Vector3 res = new Vector3(springValues[X].GetCurrentValue(), springValues[Y].GetCurrentValue(), springValues[Z].GetCurrentValue());
			return res;
		}

		// Explicit interface implementation
		public ISpringVector3 SetCurrentValue(Vector3 value)
		{
			springValues[X].SetCurrentValue(value.x);
			springValues[Y].SetCurrentValue(value.y);
			springValues[Z].SetCurrentValue(value.z);
			return this;
		}


		public ISpringVector3 SetCurrentValue(float value)
		{
			var vector = Vector3.one * value;
			springValues[X].SetCurrentValue(vector.x);
			springValues[Y].SetCurrentValue(vector.y);
			springValues[Z].SetCurrentValue(vector.z);
			return this;
		}


		public Vector3 GetVelocity()
		{
			Vector3 res = new Vector3(springValues[X].GetVelocity(), springValues[Y].GetVelocity(), springValues[Z].GetVelocity());
			return res;
		}

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.AddVelocity(Vector3 velocity)
		{
			springValues[X].AddVelocity(velocity.x);
			springValues[Y].AddVelocity(velocity.y);
			springValues[Z].AddVelocity(velocity.z);
			return this;
		}

		// Keep the original method for backward compatibility
		public void AddVelocity(Vector3 velocity)
		{
			springValues[X].AddVelocity(velocity.x);
			springValues[Y].AddVelocity(velocity.y);
			springValues[Z].AddVelocity(velocity.z);
		}

		// Implement the missing method
		ISpringVector3 ISpringVector3.AddVelocity(float value)
		{
			AddVelocity(Vector3.one * value);
			return this;
		}

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.SetVelocity(Vector3 velocity)
		{
			springValues[X].SetVelocity(velocity.x);
			springValues[Y].SetVelocity(velocity.y);
			springValues[Z].SetVelocity(velocity.z);
			return this;
		}

		// Keep the original method for backward compatibility
		public void SetVelocity(Vector3 velocity)
		{
			springValues[X].SetVelocity(velocity.x);
			springValues[Y].SetVelocity(velocity.y);
			springValues[Z].SetVelocity(velocity.z);
		}

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.SetVelocity(float value)
		{
			SetVelocity(Vector3.one * value);
			return this;
		}

		// Keep the original method for backward compatibility
		public void SetVelocity(float value)
		{
			SetVelocity(Vector3.one * value);
		}

		public Vector3 GetForce()
		{
			Vector3 res = new Vector3(GetForceByIndex(X), GetForceByIndex(Y), GetForceByIndex(Z));
			return res;
		}

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.SetForce(Vector3 force)
		{
			SetForceByIndex(X, force.x);
			SetForceByIndex(Y, force.y);
			SetForceByIndex(Z, force.z);
			return this;
		}

		// Keep the original method for backward compatibility
		public void SetForce(Vector3 force)
		{
			SetForceByIndex(X, force.x);
			SetForceByIndex(Y, force.y);
			SetForceByIndex(Z, force.z);
		}

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.SetForce(float value)
		{
			SetForce(Vector3.one * value);
			return this;
		}

		// Keep the original method for backward compatibility
		public void SetForce(float value)
		{
			SetForce(Vector3.one * value);
		}

		public Vector3 GetDrag()
		{
			Vector3 res = new Vector3(GetDragByIndex(X), GetDragByIndex(Y), GetDragByIndex(Z));
			return res;
		}

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.SetDrag(Vector3 drag)
		{
			SetDragByIndex(X, drag.x);
			SetDragByIndex(Y, drag.y);
			SetDragByIndex(Z, drag.z);
			return this;
		}

		// Keep the original method for backward compatibility
		public void SetDrag(Vector3 drag)
		{
			SetDragByIndex(X, drag.x);
			SetDragByIndex(Y, drag.y);
			SetDragByIndex(Z, drag.z);
		}

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.SetDrag(float value)
		{
			SetDrag(Vector3.one * value);
			return this;
		}

		// Keep the original method for backward compatibility
		public void SetDrag(float value)
		{
			SetDrag(Vector3.one * value);
		}

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.SetMinValues(Vector3 minValues)
		{
			SetMinValueByIndex(X, minValues.x);
			SetMinValueByIndex(Y, minValues.y);
			SetMinValueByIndex(Z, minValues.z);
			return this;
		}

		// Keep the original method for backward compatibility
		public void SetMinValues(Vector3 minValues)
		{
			SetMinValueByIndex(X, minValues.x);
			SetMinValueByIndex(Y, minValues.y);
			SetMinValueByIndex(Z, minValues.z);
		}

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.SetMinValues(float value)
		{
			SetMinValues(Vector3.one * value);
			return this;
		}

		// Keep the original method for backward compatibility
		public void SetMinValues(float value)
		{
			SetMinValues(Vector3.one * value);
		}

		public Vector3 GetMinValues()
		{
			return new Vector3(
				springValues[X].GetMinValue(),
				springValues[Y].GetMinValue(),
				springValues[Z].GetMinValue());
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

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.SetMaxValues(Vector3 maxValues)
		{
			SetMaxValueByIndex(X, maxValues.x);
			SetMaxValueByIndex(Y, maxValues.y);
			SetMaxValueByIndex(Z, maxValues.z);
			return this;
		}

		// Keep the original method for backward compatibility
		public void SetMaxValues(Vector3 maxValues)
		{
			SetMaxValueByIndex(X, maxValues.x);
			SetMaxValueByIndex(Y, maxValues.y);
			SetMaxValueByIndex(Z, maxValues.z);
		}

		// Explicit interface implementation
		ISpringVector3 ISpringVector3.SetMaxValues(float value)
		{
			SetMaxValues(Vector3.one * value);
			return this;
		}

		// Keep the original method for backward compatibility
		public void SetMaxValues(float value)
		{
			SetMaxValues(Vector3.one * value);
		}

		public Vector3 GetMaxValues()
		{
			return new Vector3(
				springValues[X].GetMaxValue(),
				springValues[Y].GetMaxValue(),
				springValues[Z].GetMaxValue());
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

		// Implement the missing methods
		ISpringVector3 ISpringVector3.SetClampRange(Vector3 minValues, Vector3 maxValues)
		{
			SetMinValues(minValues);
			SetMaxValues(maxValues);
			return this;
		}

		ISpringVector3 ISpringVector3.SetClampRange(float minValue, float maxValue)
		{
			SetMinValues(Vector3.one * minValue);
			SetMaxValues(Vector3.one * maxValue);
			return this;
		}

		ISpringVector3 ISpringVector3.SetClampTarget(bool enabled)
		{
			SetClampTarget(enabled, enabled, enabled);
			return this;
		}

		public void SetClampCurrentValues(bool clampX, bool clampY, bool clampZ)
		{
			SetClampCurrentValueByIndex(X, clampX);
			SetClampCurrentValueByIndex(Y, clampY);
			SetClampCurrentValueByIndex(Z, clampZ);
		}

		ISpringVector3 ISpringVector3.SetClampCurrentValue(bool enabled)
		{
			SetClampCurrentValues(enabled, enabled, enabled);
			return this;
		}

		public void SetClampTarget(bool clampX, bool clampY, bool clampZ)
		{
			SetClampTargetByIndex(X, clampX);
			SetClampTargetByIndex(Y, clampY);
			SetClampTargetByIndex(Z, clampZ);
		}

		ISpringVector3 ISpringVector3.SetStopOnClamp(bool enabled)
		{
			StopSpringOnClamp(enabled, enabled, enabled);
			return this;
		}

		public bool DoesStopOnClamp()
		{
			// Return true if any component is set to stop on clamp
			return springValues[X].GetStopOnClamp() ||
				   springValues[Y].GetStopOnClamp() ||
				   springValues[Z].GetStopOnClamp();
		}

		public void StopSpringOnClamp(bool stopX, bool stopY, bool stopZ)
		{
			SetStopSpringOnCurrentValueClampByIndex(X, stopX);
			SetStopSpringOnCurrentValueClampByIndex(Y, stopY);
			SetStopSpringOnCurrentValueClampByIndex(Z, stopZ);
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

		public ISpringVector3 ConfigureClamping(bool clampTarget, bool clampCurrentValue, bool stopOnClamp, Vector3 minValues, Vector3 maxValues)
		{
			SetClampTarget(clampTarget, clampTarget, clampTarget);
			SetClampCurrentValues(clampCurrentValue, clampCurrentValue, clampCurrentValue);
			StopSpringOnClamp(stopOnClamp, stopOnClamp, stopOnClamp);
			SetMinValues(minValues);
			SetMaxValues(maxValues);
			return this;
		}

		public ISpringVector3 Configure(Vector3 force, Vector3 drag, Vector3 initialValue, Vector3 target)
		{
			SetForce(force);
			SetDrag(drag);
			SetCurrentValue(initialValue);
			SetTarget(target);
			return this;
		}

		public ISpringVector3 Configure(float uniformForce, float uniformDrag, Vector3 initialValue, Vector3 target)
		{
			SetForce(uniformForce);
			SetDrag(uniformDrag);
			SetCurrentValue(initialValue);
			SetTarget(target);
			return this;
		}

		public bool IsClampTargetEnabled()
		{
			return springValues[X].GetClampTarget() || springValues[Y].GetClampTarget() || springValues[Z].GetClampTarget();
		}

		public bool IsClampCurrentValueEnabled()
		{
			return springValues[X].GetClampCurrentValue() || springValues[Y].GetClampCurrentValue() || springValues[Z].GetClampCurrentValue();
		}

		public ISpringVector3 Clone()
		{
			SpringVector3 clone = new SpringVector3();

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
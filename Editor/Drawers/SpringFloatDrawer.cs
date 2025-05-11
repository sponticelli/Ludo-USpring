using UnityEditor;
using UnityEngine;
using static USpring.SpringsEditorUtility;

namespace USpring.Drawers
{
	public class SpringFloatDrawer : SpringDrawer
	{
		private SpringFloatEditorObject springFloatEditorObject;

		public SpringFloatDrawer(SerializedProperty property, bool isFoldout, bool isDebugger) : 
			base(parentProperty: property, isFoldout: isFoldout, drawClampingArea: true, drawUpdateArea: true, isDebugger) 
		{}

		public SpringFloatDrawer(bool isFoldout) : 
			base(parentProperty: null, isFoldout: isFoldout, drawClampingArea: true, drawUpdateArea: true, isDebugger: false)
		{}

		public SpringFloatDrawer(bool isFoldout, bool isDebugger) :
			base(parentProperty: null, isFoldout: isFoldout, drawClampingArea: true, drawUpdateArea: true, isDebugger: isDebugger)
		{}

		protected override SpringEditorObject CreateSpringEditorObjectInstance(SerializedProperty parentProperty)
		{
			SpringEditorObject res = new SpringFloatEditorObject(parentProperty);
			return res;
		}

		public override void RefreshSerializedProperties(SerializedProperty parentProperty)
		{
			base.RefreshSerializedProperties(parentProperty);

			springFloatEditorObject = (SpringFloatEditorObject)SpringEditorObject;
		}

		protected override void DrawInitialValues(ref Rect currentRect)
		{
			springFloatEditorObject.InitialValue = DrawCustomFloatLogic(currentRect, FieldNameInitialValue, LabelWidth, springFloatEditorObject.InitialValue, threeDecimalsOnly: true);
		}

		protected override void DrawDrag(ref Rect currentRect)
		{
			springFloatEditorObject.Drag = DrawCustomFloat(currentRect, FieldNameDrag, springFloatEditorObject.Drag);
		}

		protected override void DrawForce(ref Rect currentRect)
		{
			springFloatEditorObject.Force = DrawCustomFloat(currentRect, FieldNameForce, springFloatEditorObject.Force);
		}

		protected override void DrawUpdate(ref Rect currentRect)
		{
			springFloatEditorObject.Update = DrawToggle(currentRect, FieldNameUpdate, LabelWidth, springFloatEditorObject.Update);
		}

		public override void DrawCurrentValue(ref Rect currentRect, string label, float labelWidth)
		{
			DrawCustomFloatLogic(currentRect, label, labelWidth, springFloatEditorObject.CurrentValue, threeDecimalsOnly: true);
		}

		protected override void DrawVelocity(ref Rect currentRect)
		{
			DrawCustomFloat(currentRect, FieldNameVelocity, springFloatEditorObject.Velocity, threeDecimalsOnly: true);
		}

		public override void DrawTarget(ref Rect currentRect, string label, float labelWidth)
		{
			springFloatEditorObject.Target = DrawCustomFloatLogic(currentRect, label, labelWidth, springFloatEditorObject.Target);
		}

		protected override void DrawClampTarget(ref Rect currentRect)
		{
			springFloatEditorObject.ClampTarget = DrawToggle(currentRect, FieldNameClampTarget, TooltipClampTarget, springFloatEditorObject.ClampTarget);
		}

		protected override void DrawClampCurrentValue(ref Rect currentRect)
		{
			springFloatEditorObject.ClampCurrentValue = DrawToggle(currentRect, FieldNameClampCurrentValue, TooltipClampCurrentValue, springFloatEditorObject.ClampCurrentValue);
		}

		protected override void DrawStopSpringOnCurrentValueClamp(ref Rect currentRect)
		{
			springFloatEditorObject.StopSpringOnCurrentValueClamp = DrawToggle(currentRect, FieldNameStopSpringOnCurrentValueClamp, TooltipStopSpringOnCurrentValueClamp, springFloatEditorObject.StopSpringOnCurrentValueClamp);
		}

		protected override void DrawClampingRange(ref Rect currentRect)
		{
			springFloatEditorObject.MinValue = DrawCustomFloat(currentRect, FieldNameMinValues, springFloatEditorObject.MinValue);

			UpdateCurrentRect(ref currentRect);
			springFloatEditorObject.MaxValue = DrawCustomFloat(currentRect, FieldNameMaxValues, springFloatEditorObject.MaxValue);
		}

		protected override void DrawNudgeOperationValues(ref Rect currentRect)
		{
			springFloatEditorObject.OperationValue = DrawCustomFloat(currentRect, FieldNameOperationValue, springFloatEditorObject.OperationValue);
		}
	}
}
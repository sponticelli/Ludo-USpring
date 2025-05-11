using UnityEditor;
using UnityEngine;
using static USpring.SpringsEditorUtility;

namespace USpring.Drawers
{
	public class SpringColorDrawer : SpringDrawer
	{
		private SpringColorEditorObject springColorEditorObject;

		public SpringColorDrawer(SerializedProperty property, bool isFoldout, bool isDebugger) : base(property, isFoldout, drawClampingArea: true, drawUpdateArea: true, isDebugger: isDebugger) 
		{}

		public SpringColorDrawer(bool isFoldout) : base(parentProperty: null, isFoldout: isFoldout, drawClampingArea: true, drawUpdateArea: true, isDebugger: false)
		{}

		public SpringColorDrawer(bool isFoldout, bool isDebugger) : base(parentProperty: null, isFoldout: isFoldout, drawClampingArea: true, drawUpdateArea: true, isDebugger: isDebugger)
		{}

		public override void RefreshSerializedProperties(SerializedProperty parentProperty)
		{
			base.RefreshSerializedProperties(parentProperty);

			springColorEditorObject = (SpringColorEditorObject)SpringEditorObject;
		}

		protected override SpringEditorObject CreateSpringEditorObjectInstance(SerializedProperty parentProperty)
		{
			SpringEditorObject res = new SpringColorEditorObject(parentProperty);
			return res;
		}

		protected override void DrawInitialValues(ref Rect currentRect)
		{
			springColorEditorObject.InitialValue = DrawColorField(currentRect, FieldNameInitialValue, LabelWidth, springColorEditorObject.InitialValue);
		}

		protected override void DrawDrag(ref Rect currentRect)
		{
			springColorEditorObject.Drag = DrawCustomVector4(currentRect, FieldNameDrag, LabelWidth, ComponentsLabelsRgba, springColorEditorObject.Drag);
		}

		protected override void DrawForce(ref Rect currentRect)
		{
			springColorEditorObject.Force = DrawCustomVector4(currentRect, FieldNameForce, LabelWidth, ComponentsLabelsRgba, springColorEditorObject.Force);
		}

		public override void DrawTarget(ref Rect currentRect, string label, float labelWidth)
		{
			springColorEditorObject.Target = DrawColorField(currentRect, label, LabelWidth, springColorEditorObject.Target);
		}

		protected override void DrawUpdate(ref Rect currentRect)
		{
			springColorEditorObject.Update = DrawVector4Bool(currentRect, FieldNameUpdateChannel, ComponentsLabelsRgba, LabelWidth, springColorEditorObject.Update);
		}

		public override void DrawCurrentValue(ref Rect currentRect, string label, float labelWidth)
		{
			DrawColorField(currentRect, label, LabelWidth, springColorEditorObject.CurrentValue);
		}

		protected override void DrawVelocity(ref Rect currentRect)
		{
			DrawCustomVector4(currentRect, FieldNameVelocity, LabelWidth, ComponentsLabelsXyzw, springColorEditorObject.Velocity, threeDecimalsOnly: true);
		}

		protected override void DrawNudgeOperationValues(ref Rect currentRect)
		{
			springColorEditorObject.OperationValue = DrawCustomVector4(currentRect, FieldNameOperationValue, LabelWidth, ComponentsLabelsRgba, springColorEditorObject.OperationValue);
		}

		protected override void DrawClampingRange(ref Rect currentRect)
		{
			springColorEditorObject.MinValue = DrawCustomVector4(currentRect, FieldNameMinValues, LabelWidth, ComponentsLabelsRgba, springColorEditorObject.MinValue);

			UpdateCurrentRect(ref currentRect);
			springColorEditorObject.MaxValue = DrawCustomVector4(currentRect, FieldNameMaxValues, LabelWidth, ComponentsLabelsRgba, springColorEditorObject.MaxValue);
		}

		protected override void DrawClampTarget(ref Rect currentRect)
		{
			springColorEditorObject.ClampTarget = DrawVector4Bool(currentRect, FieldNameClampTarget, TooltipClampTarget, ComponentsLabelsRgba, LabelWidth, springColorEditorObject.ClampTarget);
		}

		protected override void DrawClampCurrentValue(ref Rect currentRect)
		{
			springColorEditorObject.ClampCurrentValue = DrawVector4Bool(currentRect, FieldNameClampCurrentValue, TooltipClampCurrentValue, ComponentsLabelsRgba, LabelWidth, springColorEditorObject.ClampCurrentValue);
		}

		protected override void DrawStopSpringOnCurrentValueClamp(ref Rect currentRect)
		{
			springColorEditorObject.StopSpringOnCurrentValueClamp = DrawVector4Bool(currentRect, FieldNameStopSpringOnCurrentValueClamp, TooltipStopSpringOnCurrentValueClamp, ComponentsLabelsRgba, LabelWidth, springColorEditorObject.StopSpringOnCurrentValueClamp);
		}
	}
}
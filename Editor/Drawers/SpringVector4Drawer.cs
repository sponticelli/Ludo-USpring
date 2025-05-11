
using UnityEditor;
using UnityEngine;
using static USpring.SpringsEditorUtility;

namespace USpring.Drawers
{
	public class SpringVector4Drawer : SpringDrawer
	{
		private SpringVector4EditorObject springVector4EditorObject;

		public SpringVector4Drawer(SerializedProperty property, bool isFoldout, bool isDebugger) :
			base(parentProperty: property, isFoldout: isFoldout, drawClampingArea: true, drawUpdateArea: true, isDebugger: isDebugger)
		{}

		public override void RefreshSerializedProperties(SerializedProperty parentProperty)
		{
			base.RefreshSerializedProperties(parentProperty);

			springVector4EditorObject = (SpringVector4EditorObject)SpringEditorObject;
		}

		protected override SpringEditorObject CreateSpringEditorObjectInstance(SerializedProperty parentProperty)
		{
			SpringEditorObject res = new SpringVector4EditorObject(parentProperty);
			return res;
		}

		protected override void DrawInitialValues(ref Rect currentRect)
		{
			springVector4EditorObject.InitialValues = DrawCustomVector4(currentRect, FieldNameInitialValue, LabelWidth, ComponentsLabelsXyzw, springVector4EditorObject.InitialValues, threeDecimalsOnly: true);
		}

		public override void DrawCurrentValue(ref Rect currentRect, string label, float labelWidth)
		{
			DrawCustomVector4(currentRect, label, labelWidth, ComponentsLabelsXyzw, springVector4EditorObject.CurrentValue, threeDecimalsOnly: true);
		}

		public override void DrawTarget(ref Rect currentRect, string label, float labelWidth)
		{
			springVector4EditorObject.Target = DrawCustomVector4(currentRect, label, labelWidth, ComponentsLabelsXyzw, springVector4EditorObject.Target);
		}

		protected override void DrawClampingRange(ref Rect currentRect)
		{
			springVector4EditorObject.MinValue = DrawCustomVector4(currentRect, FieldNameMinValues, LabelWidth, ComponentsLabelsXyzw, springVector4EditorObject.MinValue);

			UpdateCurrentRect(ref currentRect);
			springVector4EditorObject.MaxValue = DrawCustomVector4(currentRect, FieldNameMaxValues, LabelWidth, ComponentsLabelsXyzw, springVector4EditorObject.MaxValue);
		}

		protected override void DrawClampTarget(ref Rect currentRect)
		{
			springVector4EditorObject.ClampTarget = DrawVector4Bool(currentRect, FieldNameClampTarget, TooltipClampTarget , ComponentsLabelsXyzw, LabelWidth, springVector4EditorObject.ClampTarget);
		}

		protected override void DrawClampCurrentValue(ref Rect currentRect)
		{
			springVector4EditorObject.ClampCurrentValue = DrawVector4Bool(currentRect, FieldNameClampCurrentValue, TooltipClampCurrentValue ,ComponentsLabelsXyzw, LabelWidth, springVector4EditorObject.ClampCurrentValue);
		}

		protected override void DrawStopSpringOnCurrentValueClamp(ref Rect currentRect)
		{
			springVector4EditorObject.StopSpringOnCurrentValueClamp = DrawVector4Bool(currentRect, FieldNameStopSpringOnCurrentValueClamp, TooltipStopSpringOnCurrentValueClamp, ComponentsLabelsXyzw, LabelWidth, springVector4EditorObject.StopSpringOnCurrentValueClamp);
		}

		protected override void DrawDrag(ref Rect currentRect)
		{
			springVector4EditorObject.Drag = DrawCustomVector4(currentRect, FieldNameDrag, LabelWidth, ComponentsLabelsXyzw, springVector4EditorObject.Drag);
		}

		protected override void DrawForce(ref Rect currentRect)
		{
			springVector4EditorObject.Force = DrawCustomVector4(currentRect, FieldNameForce, LabelWidth, ComponentsLabelsXyzw, springVector4EditorObject.Force);
		}

		protected override void DrawNudgeOperationValues(ref Rect currentRect)
		{
			springVector4EditorObject.OperationValue = DrawCustomVector4(currentRect, FieldNameOperationValue, LabelWidth, ComponentsLabelsXyzw, springVector4EditorObject.OperationValue);
		}

		protected override void DrawUpdate(ref Rect currentRect)
		{
			springVector4EditorObject.Update = DrawVector4Bool(currentRect, FieldNameUpdateAxis, ComponentsLabelsXyzw, LabelWidth, springVector4EditorObject.Update);
		}

		protected override void DrawVelocity(ref Rect currentRect)
		{
			DrawCustomVector4(currentRect, FieldNameVelocity, LabelWidth, ComponentsLabelsXyzw, springVector4EditorObject.Velocity, threeDecimalsOnly: true);
		}
	}
}
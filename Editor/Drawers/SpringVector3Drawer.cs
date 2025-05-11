using UnityEditor;
using UnityEngine;
using static USpring.SpringsEditorUtility;

namespace USpring.Drawers
{
	public class SpringVector3Drawer : SpringDrawer
	{
		private SpringVector3EditorObject springVector3EditorObject;

		public SpringVector3Drawer(SerializedProperty property, bool isFoldout, bool isDebugger) : 
			base(parentProperty: property, isFoldout: isFoldout, drawClampingArea: true, drawUpdateArea: true, isDebugger: isDebugger)	{}

		public SpringVector3Drawer(bool isFoldout, bool isDebugger) :
			base(parentProperty: null, isFoldout: isFoldout, drawClampingArea: true, drawUpdateArea: true, isDebugger: isDebugger)	{}

		public override void RefreshSerializedProperties(SerializedProperty parentProperty)
		{
			base.RefreshSerializedProperties(parentProperty);

			springVector3EditorObject = (SpringVector3EditorObject)SpringEditorObject;
		}

		protected override SpringEditorObject CreateSpringEditorObjectInstance(SerializedProperty parentProperty)
		{
			SpringEditorObject res = new SpringVector3EditorObject(parentProperty);
			return res;
		}

		protected override void DrawInitialValues(ref Rect currentRect)
		{
			springVector3EditorObject.InitialValues = DrawCustomVector3(currentRect, FieldNameInitialValue, LabelWidth, ComponentsLabelsXYZ, springVector3EditorObject.InitialValues, threeDecimalsOnly: true);
		}

		protected override void DrawClampingRange(ref Rect currentRect)
		{
			springVector3EditorObject.MinValue = DrawCustomVector3(currentRect, FieldNameMinValues, LabelWidth, ComponentsLabelsXYZ, springVector3EditorObject.MinValue);

			UpdateCurrentRect(ref currentRect);
			springVector3EditorObject.MaxValue = DrawCustomVector3(currentRect, FieldNameMaxValues, LabelWidth, ComponentsLabelsXYZ, springVector3EditorObject.MaxValue);
		}

		protected override void DrawClampTarget(ref Rect currentRect)
		{
			springVector3EditorObject.ClampTarget = DrawVector3Bool(currentRect, FieldNameClampTarget, TooltipClampTarget, ComponentsLabelsXYZ, LabelWidth, springVector3EditorObject.ClampTarget);
		}

		protected override void DrawClampCurrentValue(ref Rect currentRect)
		{
			springVector3EditorObject.ClampCurrentValue = DrawVector3Bool(currentRect, FieldNameClampCurrentValue,  TooltipClampCurrentValue, ComponentsLabelsXYZ, LabelWidth, springVector3EditorObject.ClampCurrentValue);
		}

		protected override void DrawStopSpringOnCurrentValueClamp(ref Rect currentRect)
		{
			springVector3EditorObject.StopSpringOnCurrentValueClamp = DrawVector3Bool(currentRect, FieldNameStopSpringOnCurrentValueClamp, TooltipStopSpringOnCurrentValueClamp, ComponentsLabelsXYZ,
				LabelWidth, springVector3EditorObject.StopSpringOnCurrentValueClamp);
		}

		protected override void DrawUpdate(ref Rect currentRect)
		{
			int indexChanged = -1;
			springVector3EditorObject.Update = DrawVector3Bool(currentRect, FieldNameUpdateAxis, ComponentsLabelsXYZ, LabelWidth, springVector3EditorObject.Update, ref indexChanged);
		}

		protected override void DrawForce(ref Rect currentRect)
		{
			springVector3EditorObject.Force = DrawCustomVector3(currentRect, FieldNameForce, LabelWidth, ComponentsLabelsXYZ, springVector3EditorObject.Force);
		}

		protected override void DrawDrag(ref Rect currentRect)
		{
			springVector3EditorObject.Drag = DrawCustomVector3(currentRect, FieldNameDrag, LabelWidth, ComponentsLabelsXYZ, springVector3EditorObject.Drag);
		}

		public override void DrawCurrentValue(ref Rect currentRect, string label, float labelWidth)
		{
			DrawCustomVector3(currentRect, label, labelWidth, ComponentsLabelsXYZ, springVector3EditorObject.CurrentValue, threeDecimalsOnly: true);
		}

		public override void DrawTarget(ref Rect currentRect, string label, float labelWidth)
		{
			springVector3EditorObject.Target = DrawCustomVector3(currentRect, label, labelWidth, ComponentsLabelsXYZ, springVector3EditorObject.Target);
		}

		protected override void DrawNudgeOperationValues(ref Rect currentRect)
		{
			springVector3EditorObject.OperationValue = DrawCustomVector3(currentRect, FieldNameOperationValue, LabelWidth, ComponentsLabelsXYZ, springVector3EditorObject.OperationValue);
		}

		protected override void DrawVelocity(ref Rect currentRect)
		{
			DrawCustomVector3(currentRect, FieldNameVelocity, LabelWidth, ComponentsLabelsXYZ, springVector3EditorObject.Velocity, threeDecimalsOnly: true);
		}
	}
}
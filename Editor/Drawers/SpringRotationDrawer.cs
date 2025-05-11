using UnityEditor;
using UnityEngine;
using USpring;
using USpring.Core;
using USpring.Drawers;
using USpring.Vectors;
using static USpring.SpringsEditorUtility;

namespace USpring.Drawers
{
	public class SpringRotationDrawer : SpringDrawer
	{
		private SpringRotationEditorObject springRotationEditorObject;

		public SpringRotationDrawer(SerializedProperty property, bool isFoldout, bool isDebugger) : 
			base(parentProperty: property, isFoldout: isFoldout, drawClampingArea: false, drawUpdateArea: false, isDebugger: isDebugger)
		{}

		public SpringRotationDrawer(bool isFoldout) : 
			base(parentProperty: null, isFoldout: isFoldout, drawClampingArea: false, drawUpdateArea: false, isDebugger: false)
		{}

		public SpringRotationDrawer(bool isFoldout, bool isDebugger) : 
			base(parentProperty: null, isFoldout: isFoldout, drawClampingArea: false, drawUpdateArea: false, isDebugger: isDebugger)
		{}

		public override void RefreshSerializedProperties(SerializedProperty parentProperty)
		{
			base.RefreshSerializedProperties(parentProperty);

			springRotationEditorObject = (SpringRotationEditorObject)SpringEditorObject;
		}

		protected override SpringEditorObject CreateSpringEditorObjectInstance(SerializedProperty parentProperty)
		{
			SpringEditorObject res = new SpringRotationEditorObject(parentProperty);
			return res;
		}

		protected override void DrawInitialValues(ref Rect currentRect)
		{
			springRotationEditorObject.InitialEuler = DrawCustomVector3(currentRect, FieldNameInitialValue, LabelWidth, ComponentsLabelsXYZ, springRotationEditorObject.InitialEuler, threeDecimalsOnly: true);
		}

		protected override void DrawUpdate(ref Rect currentRect)
		{
			int indexChanged = -1;
			springRotationEditorObject.Update = DrawVector3Bool(
				position: currentRect, 
				label: FieldNameUpdateAxis, 
				componentsLabels: ComponentsLabelsXYZ, 
				labelWidth: LabelWidth, 
				vector3Bool: springRotationEditorObject.Update,
				ref indexChanged);
		}

		protected override void DrawForce(ref Rect currentRect)
		{
			springRotationEditorObject.ForceEuler = DrawCustomVector3(position: currentRect, label: FieldNameForce, LabelWidth, componentsLabels: ComponentsLabelsXYZ, vector3: springRotationEditorObject.ForceEuler);
		}

		protected override void DrawDrag(ref Rect currentRect)
		{
			springRotationEditorObject.DragEuler = DrawCustomVector3(position: currentRect, label: FieldNameDrag, LabelWidth, componentsLabels: ComponentsLabelsXYZ, vector3: springRotationEditorObject.DragEuler);
		}

		public override void DrawCurrentValue(ref Rect currentRect, string label, float labelWidth)
		{
			DrawCustomVector3(currentRect, label, labelWidth, ComponentsLabelsXYZ, springRotationEditorObject.CurrentEuler, threeDecimalsOnly: true);
		}

		protected override void DrawVelocity(ref Rect currentRect)
		{
			DrawCustomVector3(currentRect, FieldNameVelocity, LabelWidth, ComponentsLabelsXYZ, springRotationEditorObject.VelocityEuler, threeDecimalsOnly: true);
		}

		public override void DrawTarget(ref Rect currentRect, string label, float labelWidth)
		{
			springRotationEditorObject.TargetEuler = DrawCustomVector3(currentRect, label, labelWidth, ComponentsLabelsXYZ, springRotationEditorObject.TargetEuler);
		}

		protected override void DrawNudgeOperationValues(ref Rect currentRect)
		{
			springRotationEditorObject.OperationValue = DrawCustomVector3(currentRect, FieldNameOperationValue, LabelWidth, ComponentsLabelsXYZ, springRotationEditorObject.OperationValue);
		}

		protected override void DrawClampTarget(ref Rect currentRect)
		{}

		protected override void DrawClampCurrentValue(ref Rect currentRect)
		{
		
		}

		protected override void DrawStopSpringOnCurrentValueClamp(ref Rect currentRect)
		{
		
		}

		protected override void DrawExtraFields(ref Rect rect)
		{
			UpdateCurrentRect(ref rect);
			DrawLockAxes(rect);
		}

		private void DrawLockAxes(Rect currentRect)
		{
			DrawSerializedPropertyWithRect(currentRect, LabelWidth, springRotationEditorObject.SpAxisRestriction);
		}

		private void DrawClampAxes(Rect currentRect)
		{
	
		}

		protected override void DrawClampingRange(ref Rect currentRect)
		{
			
		}

		private Vector3Bool GetEnableFieldsByAxisRestriction(SpringRotation.AxisRestriction axisRestriction)
		{
			Vector3Bool res = Vector3Bool.AllTrue;

			switch (axisRestriction)
			{
				case SpringRotation.AxisRestriction.None:
					res = Vector3Bool.AllTrue;
					break;
				case SpringRotation.AxisRestriction.OnlyXAxis:
					res.X = false;
					break;
				case SpringRotation.AxisRestriction.OnlyYAxis:
					res.Y = false;
					break;
				case SpringRotation.AxisRestriction.OnlyZAxis:
					res.Z = false;
					break;
				default:
					res = Vector3Bool.AllTrue;
					break;
			}

			return res;
		}
	}
}
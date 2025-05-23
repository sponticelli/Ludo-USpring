using UnityEditor;
using USpring.UI;
using static USpring.SpringsEditorUtility;

namespace USpring.UI
{
	[CustomEditor(typeof(SimpleSpringButton))]
	[CanEditMultipleObjects]
	public class SimpleSpringButtonCustomEditor : UnityEditor.Editor
	{
		SerializedProperty _mColorsProperty;
		SerializedProperty onClickEventProperty;
		SerializedProperty navigationProperty;
		SerializedProperty transformSpringComponentProperty;
		SerializedProperty doScaleSpringProperty;
		SerializedProperty highlightedScaleProperty;
		SerializedProperty pressedScaleVelocityAddProperty;
		SerializedProperty doRotationSpringProperty;
		SerializedProperty highlightedRotationProperty;
		SerializedProperty pressedRotationVelocityAddProperty;

		private void OnEnable()
		{
			_mColorsProperty = serializedObject.FindProperty("m_Colors");
			onClickEventProperty = serializedObject.FindProperty("onClickEvent");
			navigationProperty = serializedObject.FindProperty("m_Navigation");
			transformSpringComponentProperty = serializedObject.FindProperty("transformSpringComponent");
			doScaleSpringProperty = serializedObject.FindProperty("doScaleSpring");
			highlightedScaleProperty = serializedObject.FindProperty("highlightedScale");
			pressedScaleVelocityAddProperty = serializedObject.FindProperty("pressedScaleVelocityAdd");
			doRotationSpringProperty = serializedObject.FindProperty("doRotationSpring");
			highlightedRotationProperty = serializedObject.FindProperty("highlightedRotation");
			pressedRotationVelocityAddProperty = serializedObject.FindProperty("pressedRotationVelocityAdd");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(_mColorsProperty);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(onClickEventProperty);
			EditorGUILayout.PropertyField(transformSpringComponentProperty);
			
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(navigationProperty);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(doScaleSpringProperty);
			EditorGUILayout.PropertyField(highlightedScaleProperty);
			EditorGUILayout.PropertyField(pressedScaleVelocityAddProperty);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(doRotationSpringProperty);
			EditorGUILayout.PropertyField(highlightedRotationProperty);
			EditorGUILayout.PropertyField(pressedRotationVelocityAddProperty);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
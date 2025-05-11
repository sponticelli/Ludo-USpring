using UnityEditor;

namespace USpring.Skeletal
{
	[CustomEditor(typeof(SpringBoneAttachable))]
	public class SpringBoneAttachableCustomEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
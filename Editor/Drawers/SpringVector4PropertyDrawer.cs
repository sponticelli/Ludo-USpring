using UnityEditor;
using USpring.Core;

namespace USpring.Drawers
{
	[CustomPropertyDrawer(typeof(SpringVector4))]
	public class SpringVector4PropertyDrawer : SpringPropertyDrawer
	{
		public override SpringDrawer GetSpringDrawer(SerializedProperty property, bool isFoldout, bool isDebugger)
		{
			SpringDrawer res = new SpringVector4Drawer(property, isFoldout, isDebugger);
			return res;
		}
	}
}
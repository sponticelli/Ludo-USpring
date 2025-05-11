using UnityEditor;
using USpring.Core;

namespace USpring.Drawers
{
	[CustomPropertyDrawer(typeof(SpringVector2))]
	public class SpringVector2PropertyDrawer : SpringPropertyDrawer
	{
		public override SpringDrawer GetSpringDrawer(SerializedProperty property, bool isFoldout, bool isDebugger)
		{
			SpringDrawer res = new SpringVector2Drawer(property, isFoldout, isDebugger);
			return res;
		}
	}
}
using UnityEditor;
using USpring.Core;

namespace USpring.Drawers
{
	[CustomPropertyDrawer(typeof(SpringVector3))]
	public class SpringVector3PropertyDrawer : SpringPropertyDrawer
	{
		public override SpringDrawer GetSpringDrawer(SerializedProperty property, bool isFoldout, bool isDebugger)
		{
			SpringDrawer res = new SpringVector3Drawer(property, isFoldout, isDebugger);
			return res;
		}
	}
}
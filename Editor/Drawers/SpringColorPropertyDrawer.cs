using UnityEditor;
using USpring.Core;

namespace USpring.Drawers
{
	[CustomPropertyDrawer(typeof(SpringColor))]
	public class SpringColorPropertyDrawer : SpringPropertyDrawer
	{
		public override SpringDrawer GetSpringDrawer(SerializedProperty property, bool isFoldout, bool isDebugger)
		{
			SpringDrawer res = new SpringColorDrawer(property, isFoldout, isDebugger);
			return res;
		}
	}
}
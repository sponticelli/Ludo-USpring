using UnityEditor;
using USpring.Core;

namespace USpring.Drawers
{
    [CustomPropertyDrawer(typeof(SpringRotation))]
    public class SpringRotationPropertyDrawer : SpringPropertyDrawer
    {
        public override SpringDrawer GetSpringDrawer(SerializedProperty property, bool isFoldout, bool isDebugger)
        {
            SpringDrawer res = new SpringRotationDrawer(property, isFoldout, isDebugger);
            return res;
        }
    }
}
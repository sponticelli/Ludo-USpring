using UnityEngine;

namespace USpring
{
    [CreateAssetMenu(fileName = "SpringSettingsData", menuName = "Ludo/USpring/Settings")]
    public class SpringSettingsData : ScriptableObject
    {
        public bool doFixedUpdateRate = true;
        public float springFixedTimeStep = 0.02f;
        public float maxForceBeforeAnalyticalIntegration = 7500f;
    }
}
using UnityEngine;

namespace USpring
{
    [AddComponentMenu("Ludo/USpring/Spring Settings Provider")]
    public class SpringSettingsProvider : MonoBehaviour, ISpringSettingsProvider
    {
        [SerializeField] private SpringSettingsData settings;
        
        public bool DoFixedUpdateRate
        {
            get => settings.doFixedUpdateRate;
            set => settings.doFixedUpdateRate = value;
        }
        
        public float SpringFixedTimeStep
        {
            get => settings.springFixedTimeStep;
            set => settings.springFixedTimeStep = value;
        }
        
        public float MaxForceBeforeAnalyticalIntegration
        {
            get => settings.maxForceBeforeAnalyticalIntegration;
            set => settings.maxForceBeforeAnalyticalIntegration = value;
        }
    }
}
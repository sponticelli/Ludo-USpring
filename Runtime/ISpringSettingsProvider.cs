namespace USpring
{
    public interface ISpringSettingsProvider
    {
        public bool DoFixedUpdateRate { get; set;  }
        public float SpringFixedTimeStep { get; set; }
        public float MaxForceBeforeAnalyticalIntegration { get; set; }
    }
}
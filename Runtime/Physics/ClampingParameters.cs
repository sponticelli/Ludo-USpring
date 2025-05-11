using System;
using UnityEngine;

namespace USpring.Physics
{
    /// <summary>
    /// Parameters for configuring spring value clamping.
    /// </summary>
    /// <remarks>
    /// This class encapsulates parameters related to clamping spring values
    /// to ensure they stay within specified bounds.
    /// </remarks>
    [Serializable]
    public class ClampingParameters
    {
        /// <summary>
        /// Whether to clamp the target value.
        /// </summary>
        [SerializeField] private bool clampTarget = true;
        
        /// <summary>
        /// Whether to clamp the current value.
        /// </summary>
        [SerializeField] private bool clampCurrentValue = true;
        
        /// <summary>
        /// Whether to stop the spring when the current value is clamped.
        /// </summary>
        [SerializeField] private bool stopSpringOnCurrentValueClamp = false;
        
        /// <summary>
        /// The minimum value for clamping.
        /// </summary>
        [SerializeField] private float minValue = 0f;
        
        /// <summary>
        /// The maximum value for clamping.
        /// </summary>
        [SerializeField] private float maxValue = 100f;
        
        /// <summary>
        /// Gets or sets whether to clamp the target value.
        /// </summary>
        public bool ClampTarget
        {
            get => clampTarget;
            set => clampTarget = value;
        }
        
        /// <summary>
        /// Gets or sets whether to clamp the current value.
        /// </summary>
        public bool ClampCurrentValue
        {
            get => clampCurrentValue;
            set => clampCurrentValue = value;
        }
        
        /// <summary>
        /// Gets or sets whether to stop the spring when the current value is clamped.
        /// </summary>
        public bool StopSpringOnCurrentValueClamp
        {
            get => stopSpringOnCurrentValueClamp;
            set => stopSpringOnCurrentValueClamp = value;
        }
        
        /// <summary>
        /// Gets or sets the minimum value for clamping.
        /// </summary>
        public float MinValue
        {
            get => minValue;
            set => minValue = value;
        }
        
        /// <summary>
        /// Gets or sets the maximum value for clamping.
        /// </summary>
        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }
        
        /// <summary>
        /// Creates a copy of the clamping parameters.
        /// </summary>
        /// <returns>A new instance of ClampingParameters with the same values.</returns>
        public ClampingParameters Clone()
        {
            return new ClampingParameters
            {
                clampTarget = clampTarget,
                clampCurrentValue = clampCurrentValue,
                stopSpringOnCurrentValueClamp = stopSpringOnCurrentValueClamp,
                minValue = minValue,
                maxValue = maxValue
            };
        }
    }
}

using System;

namespace USpring.Physics
{
    /// <summary>
    /// Exception thrown when a physics-related error occurs.
    /// </summary>
    /// <remarks>
    /// This exception is thrown when a physics-related error occurs,
    /// such as invalid parameters or numerical instability.
    /// </remarks>
    public class PhysicsException : Exception
    {
        /// <summary>
        /// Creates a new instance of PhysicsException with the specified message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public PhysicsException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Creates a new instance of PhysicsException with the specified message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PhysicsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

using System;

namespace OpenExtensions.UWP.Services
{
    /// <summary>
    /// Interface for GestureService
    /// </summary>
    public interface IGestureService
    {
        /// <summary>
        /// Raised when Backwards navigation occurs.
        /// </summary>
        event EventHandler<GestureEventArgs> GoBackRequested;

        /// <summary>
        /// Raised when Forwards navigation occurs.
        /// </summary>
        event EventHandler<GestureEventArgs> GoForwardRequested;
    }
}

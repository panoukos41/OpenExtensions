using System;

namespace OpenExtensions.Core.Services
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
        /// Method to Raise the <see cref="GoBackRequested"/>
        /// </summary>
        /// <returns>True if event was handled false otherwise</returns>
        bool RaiseGoBackRequested();

        /// <summary>
        /// Raised when Forwards navigation occurs.
        /// </summary>
        event EventHandler<GestureEventArgs> GoForwardRequested;

        /// <summary>
        /// Method to Raise the <see cref="GoForwardRequested"/>
        /// </summary>
        /// <returns>True if event was handled false otherwise</returns>
        bool RaiseGoForwardRequested();
    }
}

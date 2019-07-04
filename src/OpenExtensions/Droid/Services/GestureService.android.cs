using OpenExtensions.Core.Services;
using System;

namespace OpenExtensions.Droid.Services
{
    /// <summary>
    /// Create a service that will handle events for back and forward navigation
    /// for now the wire up has to be done manually in the activity
    /// </summary>
    public class GestureService : IGestureService
    {
        /// <summary>
        /// 
        /// </summary>
        public GestureService() { }

        /// <summary>
        /// Event raised when Back navigation is requested.
        /// </summary>
        public event EventHandler<GestureEventArgs> GoBackRequested;

        /// <summary>
        /// Event raised when Forward navigation is requested.
        /// </summary>
        public event EventHandler<GestureEventArgs> GoForwardRequested;

        /// <summary>
        /// Raise <see cref="GoBackRequested"/>
        /// </summary>
        /// <returns>True if handled false otherwise</returns>
        public bool RaiseGoBackRequested()
        {
            var args = new GestureEventArgs();
            Core.Extensions.RaiseCancelableEvent(GoBackRequested, this, args);
            return args.Handled;
        }

        /// <summary>
        /// Raise <see cref="GoForwardRequested"/>
        /// </summary>
        /// <returns>True if handled false otherwise</returns>
        public bool RaiseGoForwardRequested()
        {
            var args = new GestureEventArgs();
            Core.Extensions.RaiseCancelableEvent(GoForwardRequested, this, args);
            return args.Handled;
        }
    }
}

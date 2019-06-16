using System.ComponentModel;

namespace OpenExtensions.UWP.Services
{
    /// <summary>
    /// Event arguments for the <see cref="GestureService"/>
    /// </summary>
    public class GestureEventArgs : CancelEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public GestureEventArgs() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handled"></param>
        public GestureEventArgs(bool handled)
        {
            Handled = handled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handled"></param>
        public GestureEventArgs(bool handled, bool isHardwareButton)
        {
            Handled = handled;
            IsHardwareButton = isHardwareButton;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsHardwareButton { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Handled { get; set; }
    }
}

using System;

namespace OpenExtensions.MVVM.Views
{
    /// <summary>
    /// Event arguments for the a NavigationService. for example its used in the 
    /// FramgmentNavigation service that misses navigated event.
    /// </summary>
    public class NavigationEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="parameter"></param>
        public NavigationEventArgs(Type pageType, object parameter)
        {
            PageType = pageType;
            Parameter = parameter;
        }

        /// <summary>
        /// Type of the page.
        /// </summary>
        public Type PageType { get; set; }

        /// <summary>
        /// The parameter passed to the page
        /// </summary>
        public object Parameter { get; set; }
    }
}
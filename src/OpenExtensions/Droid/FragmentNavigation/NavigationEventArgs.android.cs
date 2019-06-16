using System;

namespace OpenExtensions.Droid.FragmentNavigation
{
    /// <summary>
    /// Event arguments for the <see cref="FragmentNavigationService"/>
    /// </summary>
    public class NavigationEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="parameter"></param>
        public NavigationEventArgs(Type pageType, object parameter) => (PageType, Parameter) = (pageType, parameter);

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
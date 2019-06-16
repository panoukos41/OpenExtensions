using GalaSoft.MvvmLight.Views;
using System;
using Windows.UI.Xaml.Controls;

namespace OpenExtensions.UWP.Navigation
{
    /// <summary>
    /// A <see cref="INavigationService"/> that extends MvvmLight navigation service.
    /// </summary>
    public class NavigationServiceEx : NavigationService, INavigationService
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NavigationServiceEx"/>
        /// </summary>
        /// <param name="rootFrame"></param>
        public NavigationServiceEx(Frame rootFrame)
        {
            CurrentFrame = rootFrame;
        }

        /// <summary>
        /// CurrentParameter passed to <see cref="NavigateTo(string, object)"/>
        /// </summary>
        public object CurrentParameter { get; private set; }

        /// <summary>
        /// Override of <see cref="NavigateTo(string, object)"/> that makes sure
        /// you don't navigate to the same page twice.
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="parameter"></param>
        public override void NavigateTo(string pageKey, object parameter)
        {
            Type Page = CurrentFrame.CurrentSourcePageType;
            string key;
            if (Page != null)
                key = GetKeyForPage(Page);
            else
            {
                CurrentParameter = parameter;
                base.NavigateTo(pageKey, parameter);
                return;
            }

            if (Equals(key, pageKey) && Equals(CurrentParameter, parameter))
                return;

            CurrentParameter = parameter;
            base.NavigateTo(pageKey, parameter);
            return;
        }

        /// <summary>
        /// An ovveride to <see cref="Configure(string, Type)"/> that allows configuration in a fluent way.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageType"></param>
        public new NavigationServiceEx Configure(string key, Type pageType)
        {
            base.Configure(key, pageType);
            return this;
        }
    }
}

namespace OpenExtensions.MVVM.Views
{
    /// <summary>
    /// 
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// The key corresponding to the currently displayed page.
        /// </summary>
        string CurrentPageKey
        {
            get;
        }

        /// <summary>
        /// If possible, discards the current page and display's 
        /// the previous page on the navigation stack.
        /// </summary>
        void GoBack();

        /// <summary>
        /// A bool indicatting if the service can go back.
        /// </summary>
        /// <returns>True if the service can go back.</returns>
        bool CanGoBack();

        /// <summary>
        /// Display a new page corresponding to the given key. Depending on the platforms,
        /// the navigation service might have to be configured with a key/page list.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page that should be displayed.</param>
        void NavigateTo(string pageKey);

        /// <summary>
        /// Display a new page corresponding to the given key. Depending on the platforms,
        /// the navigation service might have to be configured with a key/page list.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page that should be displayed.</param>
        /// <param name="parameter">The parameter that should be passed to the new page.</param>
        void NavigateTo(string pageKey, object parameter);
    }
}

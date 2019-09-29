using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OpenExtensions.Uwp.UI.Controls
{
    public partial class SearchToolbar
    {
        /// <summary>SearchBox TextChanged event.</summary>
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs> TextChanged;

        /// <summary>SearchBox SuggestionChoosen event.</summary>
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs> SuggestionChoosen;

        /// <summary>SearchBox QuerySubmitted event.</summary>
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs> QuerySubmitted;

        /// <summary>SearchToolbar SearchBox visibility event.</summary>
        public event TypedEventHandler<SearchToolbar, SearchVisibilityEventArgs> SearchVisibilityChanged;

        /// <summary>
        /// Event arguments for when the SearchToolbar SearchBox changes visibility state.
        /// </summary>
        public class SearchVisibilityEventArgs : EventArgs
        {
            /// <summary>
            /// True if the control is visible.
            /// </summary>
            public bool IsVisible => Visibility == Visibility.Visible;

            /// <summary>
            /// The new visibility state.
            /// </summary>
            public Visibility Visibility { get; set; }

            /// <summary></summary>
            public SearchVisibilityEventArgs(bool isVisible)
            {
                Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            }

            /// <summary></summary>
            public SearchVisibilityEventArgs(Visibility newVisibility)
            {
                Visibility = newVisibility;
            }
        }
    }
}

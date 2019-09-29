using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace OpenExtensions.Uwp.UI.Controls
{

    public partial class SearchToolbar
    {
        /// <summary></summary>
        public object Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        /// <summary></summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty
            .Register(nameof(Title), typeof(object), typeof(SearchToolbar), new PropertyMetadata(""));

        /// <summary>
        /// Set if the commands button should be visible.
        /// </summary>
        public Visibility MenuButtonVisibility
        {
            get => (Visibility)GetValue(MenuButtonVisibilityProperty);
            set => SetValue(MenuButtonVisibilityProperty, value);
        }
        /// <summary></summary>
        public static readonly DependencyProperty MenuButtonVisibilityProperty = DependencyProperty
            .Register(nameof(MenuButtonVisibility), typeof(Visibility), typeof(SearchToolbar), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Set if the search button should be visible.
        /// </summary>
        public Visibility SearchButtonVisibility
        {
            get => (Visibility)GetValue(SearchButtonVisibilityProperty);
            set => SetValue(SearchButtonVisibilityProperty, value);
        }
        /// <summary></summary>
        public static readonly DependencyProperty SearchButtonVisibilityProperty = DependencyProperty
            .Register(nameof(SearchButtonVisibility), typeof(Visibility), typeof(SearchToolbar), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Set if the search layout should be visible.
        /// </summary>
        public bool IsSearchVisible
        {
            get => (bool)GetValue(IsSearchVisibleProperty);
            set => SetValue(IsSearchVisibleProperty, value);
        }
        /// <summary></summary>
        public static readonly DependencyProperty IsSearchVisibleProperty = DependencyProperty
            .Register(nameof(IsSearchVisible), typeof(bool), typeof(SearchToolbar), new PropertyMetadata(false, (d, e) => (d as SearchToolbar).Update()));

        /// <summary>
        /// The Item template the search view should use.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }
        /// <summary></summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty
            .Register(nameof(ItemTemplate), typeof(int), typeof(SearchToolbar), new PropertyMetadata(null));

        /// <summary>
        /// Transitions to play when the <see cref="IsSearchVisible"/> changes state.
        /// </summary>
        public TransitionCollection SearchTransitions
        {
            get => (TransitionCollection)GetValue(SearchTransitionsProperty);
            set => SetValue(SearchTransitionsProperty, value);
        }
        /// <summary></summary>
        public static readonly DependencyProperty SearchTransitionsProperty = DependencyProperty
            .Register(nameof(SearchTransitions), typeof(TransitionCollection), typeof(SearchToolbar), new PropertyMetadata(null));

        /// <summary></summary>
        public Brush SearchBackground
        {
            get => (Brush)GetValue(SearchBackgroundProperty);
            set => SetValue(SearchBackgroundProperty, value);
        }
        /// <summary></summary>
        public static readonly DependencyProperty SearchBackgroundProperty = DependencyProperty
            .Register(nameof(SearchBackground), typeof(Brush), typeof(SearchToolbar), new PropertyMetadata(Colors.Transparent));

        /// <summary></summary>
        public object MenuButtonContent
        {
            get => (object)GetValue(MenuButtonContentProperty);
            set => SetValue(MenuButtonContentProperty, value);
        }
        /// <summary></summary>
        public static readonly DependencyProperty MenuButtonContentProperty = DependencyProperty
            .Register(nameof(MenuButtonContent), typeof(object), typeof(SearchToolbar), new PropertyMetadata(null));

        /// <summary></summary>
        public object SearchButtonContent
        {
            get => (object)GetValue(SearchButtonContentProperty);
            set => SetValue(SearchButtonContentProperty, value);
        }
        /// <summary></summary>
        public static readonly DependencyProperty SearchButtonContentProperty = DependencyProperty
            .Register(nameof(SearchButtonContent), typeof(object), typeof(SearchToolbar), new PropertyMetadata(null));
    }
}

namespace OpenExtensions.Uwp.UI.Controls
{
    /// <summary></summary>
    public partial class SearchToolbar
    {
        /// <summary>
        /// Key of the VisualStateGroup that shows/dismisses the search view.
        /// </summary>
        private const string GROUP_CommonStates = "CommonStates";

        /// <summary>
        /// Key of the VisualState when search is visible
        /// </summary>
        private const string STATE_SearchVisible = "Visible";

        /// <summary>
        /// Key of the VisualState when search is collapsed
        /// </summary>
        private const string STATE_SearchCollapsed = "Collapsed";

        /// <summary>
        /// SHowMenuButton button name
        /// </summary>
        private const string PART_ShowSearchButton = "ShowSearchButton";

        /// <summary>
        /// HideSearchButton button name
        /// </summary>
        private const string PART_HideSearchButton = "HideSearchButton";

        /// <summary>
        /// CommandsFlyout name
        /// </summary>
        private const string PART_CommandsFlyout = "CommandsFlyout";

        /// <summary>
        /// AutoSuggestBox
        /// </summary>
        private const string PART_SearchBox = "SearchBox";

        /// <summary>
        /// Relative layout of search
        /// </summary>
        private const string PART_SearchLayout = "SearchLayout";

        /// <summary>
        /// MenuButton
        /// </summary>
        private const string PART_MenuButton = "MenuButton";
    }
}

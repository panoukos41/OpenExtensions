using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#pragma warning disable 1591
namespace OpenExtensions.Uwp.UI.Controls
{
    [TemplateVisualState(Name = STATE_SearchVisible, GroupName = GROUP_CommonStates)]
    [TemplateVisualState(Name = STATE_SearchCollapsed, GroupName = GROUP_CommonStates)]
    [TemplatePart(Name = PART_MenuButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_SearchLayout, Type = typeof(RelativePanel))]
    [TemplatePart(Name = PART_CommandsFlyout, Type = typeof(MenuFlyout))]
    public partial class SearchToolbar : ControlEx
    {
        Button _hideSearchButton;
        Button _showSearchButton;
        MenuFlyout _commandsFlyout;
        VisualStateGroup _groupContent;
        AutoSuggestBox _autoSuggestBox;

        public SearchToolbar()
        {
            DefaultStyleKey = typeof(SearchToolbar);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_showSearchButton != null)
                _showSearchButton.Click -= SearchButton_Click;

            if (_hideSearchButton != null)
                _hideSearchButton.Click -= SearchButton_Click;

            if (TryGetTemplatedChild(PART_ShowSearchButton, out _showSearchButton))
            {
                _showSearchButton.Click += SearchButton_Click;
            }

            if (TryGetTemplatedChild(PART_HideSearchButton, out _hideSearchButton))
            {
                _hideSearchButton.Click += SearchButton_Click;
            }

            _commandsFlyout = GetTemplatedChild<MenuFlyout>(PART_CommandsFlyout);
            _groupContent = GetTemplatedChild<VisualStateGroup>(GROUP_CommonStates);
            _autoSuggestBox = GetTemplatedChild<AutoSuggestBox>(PART_SearchBox);

            _autoSuggestBox.TextChanged += (s, e) => TextChanged?.Invoke(s, e);
            _autoSuggestBox.SuggestionChosen += (s, e) => SuggestionChoosen?.Invoke(s, e);
            _autoSuggestBox.QuerySubmitted += (s, e) => QuerySubmitted?.Invoke(s, e);

            Update();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == PART_ShowSearchButton)
            {
                ShowSearch();
                _autoSuggestBox.Focus(FocusState.Programmatic);
            }
            else
                HideSearch();
        }

        private void Update()
        {
            if (_groupContent == null)
                return;

            string state = IsSearchVisible ? STATE_SearchVisible : STATE_SearchCollapsed;
            VisualStateManager.GoToState(this, state, true);
            SearchVisibilityChanged?.Invoke(this, new SearchVisibilityEventArgs(IsSearchVisible));

            // this switch ensures ModalTransitions plays every time.
            //if (!IsSearchVisible)
            //{
            //    var content = ModalContent;
            //    ModalContent = null;
            //    ModalContent = content;
            //}
        }

        /// <summary>
        /// Show the searchbox.
        /// </summary>
        public void ShowSearch() => IsSearchVisible = true;

        /// <summary>
        /// Hide the searchbox.
        /// </summary>
        public void HideSearch() => IsSearchVisible = false;

        /// <summary>
        /// Add commands to the overflow menu.
        /// </summary>
        /// <param name="commands">String should contain the display name of the command.</param>
        public void AddCommands(IEnumerable<KeyValuePair<string, Action>> commands)
        {
            List<MenuFlyoutItem> items = new List<MenuFlyoutItem>();
            foreach (var command in commands)
            {
                MenuFlyoutItem flyoutItem = new MenuFlyoutItem
                {
                    Text = command.Key
                };
                flyoutItem.Click += (s, e) => command.Value?.Invoke();

                items.Add(flyoutItem);
            }
            foreach (var item in items)
            {
                _commandsFlyout.Items.Add(item);
            }
            MenuButtonVisibility = Visibility.Visible;
        }

        /// <summary>
        /// It clears the added commands of the overflow menu.
        /// </summary>
        public void ClearCommands()
        {
            _commandsFlyout.Items.Clear();
            MenuButtonVisibility = Visibility.Collapsed;
        }
    }
}

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using System;
using System.Collections.Generic;
using SearchView = Android.Support.V7.Widget.SearchView;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace OpenExtensions.Droid.UI.Controls
{
    /// <summary>
    /// A toolbar with search. Use <see cref="OnCreateOptionsMenu(MenuInflater, IMenu)"/>
    /// to have the search button initiated and enable the Add and Clear commands methods.
    /// </summary>
    [Register("openextensions.droid.ui.controls.SearchToolbar")]
    public class SearchToolbar : Toolbar
    {
        #region Properties

        /// <summary>
        /// The search view element that is used.
        /// </summary>
        public SearchView SearchView { get; set; }

        /// <summary>
        /// True if the search button is visible.
        /// seting this will hide or show the search button.
        /// </summary>
        public bool IsSearchButtonVisible
        {
            get => menu.FindItem(Resource.Id.action_search).IsVisible;
            set => menu.FindItem(Resource.Id.action_search).SetVisible(value);
        }

        /// <summary>
        /// True if search is visible.
        /// seting this will hide or show the search view.
        /// </summary>
        public bool IsSearchVisible
        {
            get => menu.FindItem(Resource.Id.action_search).IsActionViewExpanded;
            set
            {
                if (value)
                    ShowSearch();
                else
                    CloseSearch();
            }
        }
        #endregion

        #region Constructors
        /// <summary></summary>
        public SearchToolbar(Context context) : base(context) { }
        /// <summary></summary>
        public SearchToolbar(Context context, IAttributeSet attrs) : base(context, attrs) { }
        /// <summary></summary>
        public SearchToolbar(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        /// <summary></summary>
        public SearchToolbar(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the toolbar menu and adds the search button.
        /// </summary>
        public void OnCreateOptionsMenu(MenuInflater menuInflater, IMenu menu)
        {
            (MenuInflater, this.menu) = (menuInflater, menu);
            MenuInflater.Inflate(Resource.Menu.searchToolbar_menu, this.menu);

            SearchView = this.menu.FindItem(Resource.Id.action_search).ActionView.JavaCast<SearchView>();
        }

        private MenuInflater MenuInflater;
        private IMenu menu;
        private readonly List<int> CommandIds = new List<int>();

        /// <summary>
        /// Expand the search action view.
        /// </summary>
        public void ShowSearch()
        {
            menu.FindItem(Resource.Id.action_search).ExpandActionView();
        }

        /// <summary>
        /// Collapse the search action view.
        /// </summary>
        public void CloseSearch()
        {
            SearchView.ClearFocus();
            menu.FindItem(Resource.Id.action_search).CollapseActionView();
        }

        /// <summary>
        /// Add new commands to the overflow menu.
        /// </summary>
        /// <param name="commands"></param>
        public void AddCommands(IEnumerable<KeyValuePair<string, Action>> commands)
        {
            foreach (var command in commands)
            {
                var menuItem = menu
                    .Add(command.Key)
                    .SetOnMenuItemClickListener(new MenuItemClickListener(command.Value));

                CommandIds.Add(menuItem.ItemId);
            }
        }

        /// <summary>
        /// It wont remove the search.
        /// </summary>
        public void ClearCommands()
        {
            foreach (var id in CommandIds)
            {
                menu.RemoveItem(id);
            }
        }

        internal class MenuItemClickListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
        {
            readonly Action Action;
            public MenuItemClickListener(Action action)
            {
                Action = action;
            }

            public bool OnMenuItemClick(IMenuItem item)
            {
                if (Action == null)
                    return false;

                Action?.Invoke();
                return true;
            }
        }
        #endregion
    }
}
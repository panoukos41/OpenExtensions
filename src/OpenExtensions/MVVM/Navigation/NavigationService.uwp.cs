﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OpenExtensions.MVVM.Views
{
    /// <summary>
    /// A navigation service to use with UWP implements <see cref="INavigationService"/>.
    /// </summary>
    public class NavigationService : INavigationService
    {
        /// <summary>
        /// The key that is returned by the <see cref="CurrentPageKey"/> property
        /// when the current Page is the root page.
        /// </summary>
        public const string RootPageKey = "-- ROOT --";

        /// <summary>
        /// The key that is returned by the <see cref="CurrentPageKey"/> property
        /// when the current Page is not found.
        /// This can be the case when the navigation wasn't managed by this NavigationService,
        /// for example when it is directly triggered in the code behind, and the
        /// NavigationService was not configured for this page type.
        /// </summary>
        public const string UnknownPageKey = "-- UNKNOWN --";

        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private Frame _currentFrame;

        /// <summary>
        /// Gets or sets the Frame that should be use for the navigation.
        /// If this is not set explicitly, then (Frame)Window.Current.Content is used.
        /// </summary>
        public Frame CurrentFrame
        {
            get
            {
                return _currentFrame ?? (_currentFrame = ((Frame)Window.Current.Content));
            }

            set
            {
                _currentFrame = value;
            }
        }

        /// <summary>
        /// Indicates if the CurrentFrame can navigate backwards.
        /// </summary>
        public bool CanGoBack() => CurrentFrame.CanGoBack;

        /// <summary>
        /// Indicates if the CurrentFrame can navigate forward.
        /// </summary>
        public bool CanGoForward() => CurrentFrame.CanGoForward;

        /// <summary>
        /// Initialize a new navigationservice.
        /// </summary>
        public NavigationService() { }

        /// <summary>
        /// Create a new navigation service providing a rootframe to use for navigation.
        /// </summary>
        /// <param name="rootframe"></param>
        public NavigationService(Frame rootframe) => CurrentFrame = rootframe;

        /// <summary>
        /// If possible, discards the current page and displays the previous page
        /// on the navigation stack.
        /// </summary>
        public void GoBack()
        {
            if (CurrentFrame.CanGoBack)
            {
                CurrentFrame.GoBack();
            }
        }

        /// <summary>
        /// Check if the CurrentFrame can navigate forward, and if yes, performs
        /// a forward navigation.
        /// </summary>
        public void GoForward()
        {
            if (CurrentFrame.CanGoForward)
            {
                CurrentFrame.GoForward();
            }
        }

        /// <summary>
        /// The key corresponding to the currently displayed page.
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (CurrentFrame.BackStackDepth == 0)
                    {
                        return RootPageKey;
                    }

                    if (CurrentFrame.Content == null)
                    {
                        return UnknownPageKey;
                    }

                    var currentType = CurrentFrame.Content.GetType();

                    if (_pagesByKey.All(p => p.Value != currentType))
                    {
                        return UnknownPageKey;
                    }

                    var item = _pagesByKey.FirstOrDefault(
                        i => i.Value == currentType);

                    return item.Key;
                }
            }
        }

        private object CurrentParameter { get; set; }

        /// <summary>
        /// Displays a new page corresponding to the given key. 
        /// Make sure to call the <see cref="Configure"/> method first.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page that should be displayed.</param>
        /// <exception cref="ArgumentException">When this method is called for a key that has not been configured earlier.</exception>
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        /// <summary>
        /// Displays a new page corresponding to the given key,
        /// and passes a parameter to the new page.
        /// Make sure to call the <see cref="Configure"/>
        /// method first.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        /// that should be displayed.</param>
        /// <param name="parameter">The parameter that should be passed
        /// to the new page.</param>
        /// <exception cref="ArgumentException">When this method is called for a key that has not been configured earlier.</exception>
        public virtual void NavigateTo(string pageKey, object parameter)
        {
            Type Page = CurrentFrame.CurrentSourcePageType;
            string key;
            if (Page != null)
                key = GetKeyForPage(Page);
            else
            {
                CurrentParameter = parameter;
                _navigateTo(pageKey, parameter);
                return;
            }

            if (Equals(key, pageKey) && Equals(CurrentParameter, parameter))
                return;

            CurrentParameter = parameter;
            _navigateTo(pageKey, parameter);
        }

        private void _navigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(string.Format("No such page: {0}. Did you forget to call NavigationService.Configure?", pageKey), "pageKey");
                }

                CurrentFrame.Navigate(_pagesByKey[pageKey], parameter);
            }
        }

        /// <summary>
        /// Adds a key/page pair to the navigation service.
        /// </summary>
        /// <param name="key">The key that will be used later
        /// in the <see cref="NavigateTo(string)"/> or <see cref="NavigateTo(string, object)"/> methods.</param>
        /// <param name="pageType">The type of the page corresponding to the key.</param>
        public NavigationService Configure(string key, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(key))
                {
                    throw new ArgumentException("This key is already used: " + key);
                }

                if (_pagesByKey.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException(
                        "This type is already configured with key " + _pagesByKey.First(p => p.Value == pageType).Key);
                }

                _pagesByKey.Add(key, pageType);
            }
            return this;
        }

        /// <summary>
        /// Gets the key corresponding to a given page type.
        /// </summary>
        /// <param name="page">The type of the page for which the key must be returned.</param>
        /// <returns>The key corresponding to the page type.</returns>
        public string GetKeyForPage(Type page)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsValue(page))
                {
                    return _pagesByKey.FirstOrDefault(p => p.Value == page).Key;
                }
                else
                {
                    throw new ArgumentException($"The page '{page.Name}' is unknown by the NavigationService");
                }
            }
        }
    }
}
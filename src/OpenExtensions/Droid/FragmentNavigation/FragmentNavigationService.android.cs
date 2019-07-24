using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using OpenExtensions.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenExtensions.Droid.FragmentNavigation
{
    /// <summary>
    /// A navigation service placed in the UI as a fragment, 
    /// call fragmentmanager to place this service in the UI.
    /// </summary>
    public class FragmentNavigationService : Fragment, IFragmentNavigationService
    {
        /// <summary>
        /// True if the OnCreated method is called.
        /// </summary>
        public bool IsCreated { get; private set; } = false;

        private const string FRAGMENT_STACK_KEY = "the_fragment_stack";
        private readonly int FRAGMENT_CONTAINER_ID = View.GenerateViewId();
        private readonly Dictionary<string, Type> fragmentsByKey = new Dictionary<string, Type>();
        private Stack<NavigationFragment> backStack = new Stack<NavigationFragment>();

        private int? EnterAnimation { get; set; }
        private int? ExitAnimation { get; set; }
        private int? PopEnterAnimation { get; set; }
        private int? PopExitAnimation { get; set; }

        /// <summary>
        /// [0] is the root and the pages are in the order they were placed.
        /// </summary>
        public IEnumerable<NavigationFragment> Fragments => backStack.Reverse();

        /// <summary>
        /// Called when the fragment is created. Restores backstack if any.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public sealed override void OnCreate(Bundle savedInstanceState)
        {            
            base.OnCreate(savedInstanceState);
            if (savedInstanceState != null)
            {
                var stack = savedInstanceState?.GetString(FRAGMENT_STACK_KEY, null);
                JsonService.TryDeserialize(stack, out Stack<NavigationFragment> newstack);
                if (newstack != null)
                    backStack = newstack;
            }
        }

        /// <summary>
        /// Called when the view needs to be created.
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public sealed override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var Container = new FrameLayout(Context)
            {
                Id = FRAGMENT_CONTAINER_ID,
                LayoutParameters = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent)
            };
            if (backStack.Count > 0)
            {
                var frag = backStack.Peek();
                DisplayFragment(frag);
            }
            IsCreated = true;
            return Container;
        }

        /// <summary>
        /// Called to save the instance (backstack)
        /// </summary>
        /// <param name="outState"></param>
        public sealed override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutString(FRAGMENT_STACK_KEY, JsonService.Serialize(backStack));
        }

        /// <summary>
        /// Clear the backstack and set <see cref="IsCreated"/> to false.
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
            IsCreated = false;
            backStack.Clear();
        }

        /// <summary>
        /// The current parameter passed to the <see cref="NavigateTo(string, object)"/> method for the current fragment.
        /// </summary>
        public object CurrentPageParameter { get => CurrentPage.Nav_parameter; }

        /// <summary>
        /// The key corresponding to the currently displayed fragment.
        /// </summary>
        public string CurrentPageKey { get { return GetKeyForFragment(CurrentPage.GetType()); } }

        /// <summary>
        /// The currently displayed fragment.
        /// </summary>
        public NavigationFragment CurrentPage { get => backStack.Count == 0 ? null : backStack.Peek(); }

        /// <summary>
        /// Adds a key/fragment pair to the navigation service.
        /// </summary>
        /// <param name="key">The key that will be used later
        /// in the <see cref="NavigateTo(string)"/> or <see cref="NavigateTo(string, object)"/> methods.</param>
        /// <param name="fragmentType">The type of the fragment (page) corresponding to the key.</param>
        public FragmentNavigationService Configure(string key, Type fragmentType)
        {
            lock (fragmentsByKey)
            {
                if (fragmentsByKey.ContainsKey(key))
                {
                    fragmentsByKey[key] = fragmentType;
                }
                else
                {
                    fragmentsByKey.Add(key, fragmentType);
                }
            }
            return this as FragmentNavigationService;
        }

        /// <summary>
        /// If possible, discards the current fragment and displays the previous fragment on the navigation stack.
        /// </summary>
        public void GoBack()
        {
            if (!CanGoback())
                return;

            backStack.Pop();

            var frag = backStack.Peek();
            RaiseNavigated(new NavigationEventArgs(frag.GetType(), frag.Nav_parameter));
            DisplayFragment(frag, true);            
        }

        /// <summary>
        /// Returns true if it's possible to go back, 
        /// the root fragment can't be removed and wont't be counted.
        /// </summary>
        public bool CanGoback()
        {
            return backStack.Count > 1;
        }

        /// <summary>
        /// Displays a new fragment corresponding to the given key. 
        /// Make sure to call the <see cref="Configure"/> method first.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the <see cref="NavigationFragment"/> that should be displayed.</param>
        /// <exception cref="ArgumentException">When this method is called for a key that has not been configured earlier.</exception>
        public void NavigateTo(string pageKey) => NavigateTo(pageKey, null);

        /// <summary>
        /// Displays a new fragment corresponding to the given key, and passes a parameter to the new <see cref="NavigationFragment"/>.
        /// Make sure to call the <see cref="Configure"/> method first.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the fragment that should be displayed.</param>
        /// <param name="parameter">The parameter that should be passed to the new fragment.</param>
        /// <exception cref="ArgumentException">When this method is called for a key that has not been configured earlier.</exception>
        public void NavigateTo(string pageKey, object parameter)
        {
            if (!fragmentsByKey.ContainsKey(pageKey))
                throw new ArgumentException($"The page '{pageKey}' is unknown by the NavigationService");

            Type fragType = fragmentsByKey[pageKey];
            Type currentfragType = CurrentPage?.GetType() ?? null;

            //never navigate to the same page.
            if (Equals(fragType, currentfragType) && Equals(parameter, CurrentPageParameter))
                return;

            NavigationFragment frag = (NavigationFragment)Activator.CreateInstance(fragmentsByKey[pageKey]);
            frag.Nav_parameter = parameter;

            //if we are not created and added yet just add to the backstack.
            if (IsCreated)
                DisplayFragment(frag);

            backStack.Push(frag);
            RaiseNavigated(new NavigationEventArgs(fragType, parameter));
        }

        /// <summary>
        /// Set the Fragment manager's animations.
        /// </summary>
        /// <param name="enter">Animation of new fragment.</param>
        /// <param name="exit">Animation of old fragment.</param>
        /// <param name="popEnter">Same as enter but for the old fragment that enters.</param>
        /// <param name="popExit">Same as exit but for the new fragment that's leaving.</param>
        public void SetAnimations(int enter, int exit, int popEnter, int popExit)
        {
            (EnterAnimation, ExitAnimation, PopEnterAnimation, PopExitAnimation) = (enter, exit, popEnter, popExit);
        }

        /// <summary>
        /// Gets the key corresponding to a given fragment type.
        /// </summary>
        /// <param name="fragment">The type of the fragment for which the key must be returned.</param>
        /// <returns>The key corresponding to the page type.</returns>
        public string GetKeyForFragment(Type fragment)
        {
            lock (fragmentsByKey)
            {
                if (fragmentsByKey.ContainsValue(fragment))
                {
                    return fragmentsByKey.FirstOrDefault(p => p.Value == fragment).Key;
                }
                else
                {
                    throw new ArgumentException($"The page '{fragment.Name}' is unknown by the NavigationService");
                }
            }
        }

        /// <summary>
        /// Event raised when fragments are navigated to and back.
        /// </summary>
        public event EventHandler<NavigationEventArgs> Navigated;
        private void RaiseNavigated(NavigationEventArgs args) => Navigated?.Invoke(this, args);

        private void DisplayFragment(Fragment fragment, bool backNavigation = false)
        {
            var transaction = ChildFragmentManager
                .BeginTransaction()
                .DisallowAddToBackStack();

            if (!backNavigation && EnterAnimation.HasValue && ExitAnimation.HasValue)
                transaction.SetCustomAnimations(EnterAnimation.Value, ExitAnimation.Value);
            else if (backNavigation && PopEnterAnimation.HasValue && PopExitAnimation.HasValue)
                transaction.SetCustomAnimations(PopEnterAnimation.Value, PopExitAnimation.Value);

            if (fragment.IsDetached)
                transaction.Attach(fragment);
            transaction
                .Replace(FRAGMENT_CONTAINER_ID, fragment)
                .CommitNow();
        }
    }
}
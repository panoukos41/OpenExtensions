using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using GalaSoft.MvvmLight.Views;
using OpenExtensions.Core.Services;
using OpenExtensions.Droid.FragmentNavigation;
using OpenExtensions.Droid.Services;
using System.Threading.Tasks;
using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace OpenExtensions.Droid.App
{
    /// <summary>
    /// Set [Activity(ConfigurationChanges = ConfigChanges.Orientation)] to allow the activity 
    /// to handle the configuration changes for the navigation service"/>
    /// </summary>
    [Activity(ConfigurationChanges = ConfigChanges.Orientation)]
    public abstract class FragmentNavigationActivity : AppCompatActivity
    {
        /// <summary>
        /// Your Toolbar
        /// </summary>
        public virtual Toolbar Toolbar { get; }

        /// <summary>
        /// Your DrawerLayout
        /// </summary>
        public virtual DrawerLayout Drawer { get; }

        /// <summary>
        /// Your layout
        /// </summary>
        public abstract int ShellLayout { get; }

        /// <summary>
        /// The framelayout to host the <see cref="NavigationService"/>
        /// </summary>
        public virtual int NavigationServiceFrame { get; }

        /// <summary>
        /// The navigationservice created in <see cref="OnCreateNavigationService"/>
        /// default is the <see cref="FragmentNavigationService"/>
        /// </summary>
        public IFragmentNavigationService NavigationService => fragment as IFragmentNavigationService;

        /// <summary>
        /// Default GestureService. Use <see cref="OnCreateGestureService"/> to inject your own.
        /// </summary>
        public IGestureService GestureService { get; private set; }

        private Fragment fragment;
        private const string NAVIGATIONSERVICE_TAG = "fragment navigation service";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected sealed override async void OnCreate(Bundle savedInstanceState)
        {
            SetTheme();
            base.OnCreate(savedInstanceState);
            SetContentView(ShellLayout);

            await OnInitializeAsync();            
            CreateNavigationService();
            CreateGestureService();
            OnCreateToolbar(Toolbar);
            OnCreateDrawerForToolbar(Toolbar, Drawer);

            await OnLaunchAsync(savedInstanceState);
            SetNavigationService();
        }

        /// <summary>
        /// Last call before the activity begins.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public abstract Task OnLaunchAsync(Bundle savedInstanceState);

        /// <summary>
        /// Runs after the content view has been set
        /// this is the first method that runs code, initialization should go here.
        /// </summary>
        public virtual Task OnInitializeAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Restarts the Activity
        /// </summary>
        public async Task RestartAsync()
        {
            Finish();
            await Task.Delay(400);
            StartActivity(Intent);
        }

        /// <summary>
        /// Called before <see cref="OnCreate(Bundle)"/> to set the app theme.
        /// </summary>
        public virtual void SetTheme()
        {

        }

        /// <summary>
        /// Creates and sets the toolbar
        /// </summary>
        /// <param name="toolbar">The provided toolbar</param>
        public virtual void OnCreateToolbar(Toolbar toolbar)
        {
            if (toolbar != null)
                SetSupportActionBar(toolbar);
        }

        /// <summary>
        /// Connects the provided <see cref="Drawer"/> to the provided <see cref="Toolbar"/>
        /// </summary>
        /// <param name="toolbar"></param>
        /// <param name="drawerLayout"></param>
        public virtual void OnCreateDrawerForToolbar(Toolbar toolbar, DrawerLayout drawerLayout)
        {
            if (toolbar == null || drawerLayout == null)
                return;

            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this,
                drawerLayout,
                toolbar,
                Resource.String.navigation_drawer_open,
                Resource.String.navigation_drawer_close);

            drawerLayout.AddDrawerListener(toggle);
            toggle.SyncState();
        }

        /// <summary>
        /// Create the navigation service that is a <see cref="Fragment"/> 
        /// and implements the <see cref="IFragmentNavigationService"/>
        /// </summary>
        public virtual Fragment OnCreateNavigationService()
        {
            return null;
        }

        /// <summary>
        /// Creates the GestureService, ovveride to provide your own.
        /// </summary>
        /// <returns></returns>
        public virtual IGestureService OnCreateGestureService()
        {
            return new GestureService();
        }

        /// <summary>
        /// Called when back navigation is requested, if the <see cref="Drawer"/> is open it will be closed,
        /// if the <see cref="NavigationService"/> can go back it will navigate back
        /// else the app will be finished through the Finish() method.
        /// </summary>
        public override void OnBackPressed()
        {
            if (GestureService.RaiseGoBackRequested())
                return;
            Finish();
        }

        /// <summary>
        /// Call the base to handle the orientation change for the navigation service.
        /// </summary>
        /// <param name="newConfig">The new configuration.</param>
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (NavigationService != null)
            {
                SupportFragmentManager
                .BeginTransaction()
                .Detach(fragment)
                .Attach(fragment)
                .CommitNow();
            }
        }

        private void CreateGestureService()
        {
            GestureService = OnCreateGestureService();
            GestureService.GoBackRequested += OnGoBackRequested;
            GestureService.GoForwardRequested += OnGoForwardRequested;
        }

        /// <summary>
        /// Ovveride with logic as to what happens when <see cref="IGestureService.GoForwardRequested"/> is fired.
        /// </summary>
        protected virtual void OnGoForwardRequested(object sender, GestureEventArgs e)
        {
            
        }

        /// <summary>
        /// Ovveride with logic as to what happens when <see cref="IGestureService.GoBackRequested"/> is fired,
        /// by default it will try to close all drawers if they are closed then it will
        /// call <see cref="INavigationService.GoBack()"/> if its possible.
        /// </summary>
        protected virtual void OnGoBackRequested(object sender, GestureEventArgs e)
        {
            if (e.Handled)
                return;

            if (Drawer != null && (Drawer.IsDrawerOpen(GravityCompat.Start) || Drawer.IsDrawerOpen(GravityCompat.End)))
            {
                Drawer.CloseDrawers();
                e.Handled = true;
            }
            else if (NavigationService != null && NavigationService.CanGoback())
            {
                NavigationService.GoBack();
                e.Handled = true;
            }
            else
                e.Handled = false;
        }

        private void CreateNavigationService()
        {
            fragment = OnCreateNavigationService();
        }

        private void SetNavigationService()
        {
            if (NavigationService == null)
            {
                return;
            }
            var Transaction = SupportFragmentManager.BeginTransaction();
            if (fragment.IsDetached)
                Transaction.Attach(fragment);
            Transaction
                .DisallowAddToBackStack()
                .Replace(NavigationServiceFrame, fragment, NAVIGATIONSERVICE_TAG)
                .CommitNow();
        }
    }
}
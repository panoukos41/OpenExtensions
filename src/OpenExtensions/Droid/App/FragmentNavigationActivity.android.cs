using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using OpenExtensions.Droid.FragmentNavigation;
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
        #region Properties/Controls

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

        private Fragment fragment;
        private const string NAVIGATIONSERVICE_TAG = "fragment navigation service";
        #endregion

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
            OnCreateToolbar(Toolbar);
            OnCreateDrawerForToolbar(Toolbar, Drawer);

            await OnLaunchAsync(savedInstanceState);
            SetNavigationService();
        }

        #region virtual Methods

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
        #endregion

        #region ovverides

        /// <summary>
        /// Called when back navigation is requested, if the <see cref="Drawer"/> is open it will be closed,
        /// if the <see cref="NavigationService"/> can go back it will navigate back
        /// else the app will be finished through the Finish() method.
        /// </summary>
        public override void OnBackPressed()
        {
            if (Drawer != null && (Drawer.IsDrawerOpen(GravityCompat.Start) || Drawer.IsDrawerOpen(GravityCompat.End)))
            {
                Drawer.CloseDrawers();
            }
            else if (NavigationService != null && NavigationService.CanGoback())
            {
                NavigationService.GoBack();
            }
            else
            {
                Finish();
            }
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
        #endregion

        #region Private Methods

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
        #endregion
    }
}
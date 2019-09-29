using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using OpenExtensions.Core.Services;
using OpenExtensions.Droid.Services;
using System.Threading.Tasks;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace OpenExtensions.Droid.App
{
    /// <summary>
    /// A base activity that provides overrides for toolbar and drawerlayout
    /// and will configure them accordingly.
    /// </summary>
    public abstract class ActivityBase : AppCompatActivity
    {
        /// <summary>
        /// Your Toolbar
        /// </summary>
        public virtual Toolbar Toolbar { get; }

        /// <summary>
        /// Your Drawer
        /// </summary>
        public virtual DrawerLayout Drawer { get; }

        /// <summary>
        /// Your shell layout (default layout).
        /// </summary>
        public abstract int ShellLayout { get; }

        /// <summary>
        /// Default GestureService. Used to provide Backwards and Forwards navigation callbacks.
        /// Use <see cref="OnCreateGestureService"/> to inject your own.
        /// </summary>
        public IGestureService GestureService { get; private set; }

        /// <summary>
        /// Called before SetContentView() to set the app theme.
        /// </summary>
        protected virtual void SetTheme() { }

        /// <summary>
        /// Last call before the activity begins.
        /// Called at the end of <see cref="OnCreate(Bundle)"/>
        /// <see cref="OnCreate(Bundle)"/> logic should go here.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected abstract Task OnLaunchAsync(Bundle savedInstanceState);

        /// <summary>
        /// Runs after the content view has been set
        /// this is the first method that runs code, initialization should go here.
        /// </summary>
        protected virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected sealed override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetTheme();
            SetContentView(ShellLayout);

            InitializeAsync();
            CreateGestureService();
            OnCreateToolbar(Toolbar);
            OnCreateDrawerForToolbar(Toolbar, Drawer);
            OnLaunchAsync(savedInstanceState);
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
        /// Creates and sets the toolbar, override to set the toolbar your own way.
        /// </summary>
        /// <param name="toolbar">The provided toolbar</param>
        protected virtual void OnCreateToolbar(Toolbar toolbar)
        {
            if (toolbar != null)
                SetSupportActionBar(toolbar);
        }

        /// <summary>
        /// Connects the provided <see cref="Drawer"/> to the provided <see cref="Toolbar"/>
        /// override 
        /// </summary>
        /// <param name="toolbar"></param>
        /// <param name="drawerLayout"></param>
        protected virtual void OnCreateDrawerForToolbar(Toolbar toolbar, DrawerLayout drawerLayout)
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
        /// Creates the <see cref="GestureService"/>, override to provide your own.
        /// </summary>
        /// <returns></returns>
        protected virtual IGestureService OnCreateGestureService()
        {
            return new GestureService();
        }

        private void CreateGestureService()
        {
            GestureService = OnCreateGestureService();
            GestureService.GoBackRequested += OnGoBackRequested;
            GestureService.GoForwardRequested += OnGoForwardRequested;
        }

        /// <summary>
        /// Called when back navigation is requested, it will execute <see cref="GestureService.RaiseGoBackRequested()"/>
        /// if the request is not handled aka you are at the root page Finish() will be executed./>
        /// it's recommended to override <see cref="OnGoBackRequested(object, GestureEventArgs)"/> to handle back navigation.
        /// </summary>
        public override void OnBackPressed()
        {
            if (GestureService.RaiseGoBackRequested())
                return;
            Finish();
        }



        /// <summary>
        /// Override with logic as to what happens when <see cref="IGestureService.GoForwardRequested"/> is fired
        /// you can fire <see cref="IGestureService.GoForwardRequested"/> with <see cref="GestureService.RaiseGoForwardRequested"/>.
        /// </summary>
        protected virtual void OnGoForwardRequested(object sender, GestureEventArgs e)
        {

        }

        /// <summary>
        /// Override with logic as to what happens when <see cref="IGestureService.GoBackRequested"/> is fired,
        /// by default it will try to close all drawers if <see cref="GestureEventArgs.Handled"/> remains false
        /// the <see cref="OnBackPressed"/> will call finish();
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
        }
    }
}

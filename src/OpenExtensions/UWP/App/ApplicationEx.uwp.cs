using GalaSoft.MvvmLight.Views;
using OpenExtensions.UWP.Navigation;
using OpenExtensions.UWP.Services;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OpenExtensions.UWP.App
{
    /// <summary>
    /// Provides the base class for your Universal Windows Platform application.
    /// Takes care of the creation and initialization when the application starts.
    /// </summary>
    public abstract class ApplicationEx : Application
    {
        private bool _handledOnResume;
        private bool _isRestoringFromTermination;

        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        protected ApplicationEx()
        {
            Suspending += OnSuspending;
            Resuming += OnResuming;
        }

        /// <summary>
        /// Gets or sets whether the application triggers navigation restore on app resume.
        /// Default = true
        /// </summary>
        protected bool RestoreNavigationStateOnResume { get; set; } = true;

        /// <summary>
        /// Gets the shell user interface
        /// </summary>
        protected UIElement Shell { get; private set; }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        protected INavigationService NavigationService { get; private set; }

        /// <summary>
        /// Get the device <see cref="GestureService"/>.
        /// Use this instead of <see cref="SystemNavigationManager"/> 
        /// for forward and backwards navigation events and handling.
        /// </summary>
        protected IGestureService GestureService { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the application is suspending.
        /// </summary>
        /// <value>
        /// <c>true</c> if the application is suspending; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuspending { get; private set; }

        /// <summary>
        /// Ovveride with logic as to what happens when <see cref="GestureService.GoForwardRequested"/> is fired,
        /// by default it will call <see cref="Frame.GoForward()"/> if its possible.
        /// </summary>
        protected virtual void OnGoForwardRequested(object sender, GestureEventArgs e)
        {
            if (!e.Handled && RootFrame.CanGoForward)
            {
                RootFrame.GoForward();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Ovveride with logic as to what happens when <see cref="GestureService.GoBackRequested"/> is fired,
        /// by default it will call <see cref="Frame.GoBack()"/> if its possible.
        /// </summary>
        protected virtual void OnGoBackRequested(object sender, GestureEventArgs e)
        {
            if (!e.Handled && RootFrame.CanGoBack)
            {
                RootFrame.GoBack();
                e.Handled = true;
            }
        }

        /// <summary>
        /// This is called whenever the app is launched normally, navigate to the application's home page.
        /// </summary>
        /// <param name="args">The <see cref="LaunchActivatedEventArgs"/> instance containing the event data.</param>
        protected abstract Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args);

        /// <summary>
        /// This is called when application is activated through other means than a normal launch 
        /// (i.e. Voice Commands, URI activation, being used as a share target from another app).
        /// </summary>
        /// <param name="args">The <see cref="IActivatedEventArgs"/> instance containing the event data.</param>
        protected virtual Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Logic of your application. Here you can initialize services etc.
        /// </summary>
        /// <param name="args">The <see cref="IActivatedEventArgs"/> instance containing the event data.</param>
        protected virtual Task OnInitializeAsync(IActivatedEventArgs args)
        {
            return Task.CompletedTask;
        }

        /// <summary> 
        /// Invoked when the application is activated via ShareTarget.
        /// OnShareTargetActivated is the entry point for an application when it is activated through sharing.
        /// Call <see cref="OnActivated"/> to bootstrap the application.
        /// </summary>
        /// <param name="args">Event data for the event.</param>
        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            base.OnShareTargetActivated(args);
            OnActivated(args);
        }

        /// <summary>
        /// OnFileActivated is the entry point for an application when it is launched and the ActivationKind is File.
        /// Call <see cref="OnActivated"/> to bootstrap the prism application.
        /// </summary>
        /// <param name="args">Event data for the event.</param>
        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            base.OnFileActivated(args);
            OnActivated(args);
        }

        /// <summary>
        /// Creates the navigation service, use this to create your own <see cref="INavigationService"/> implementation.
        /// Default is <see cref="NavigationServiceEx"/>
        /// </summary>
        /// <param name="rootFrame">The root frame.</param>
        protected virtual INavigationService OnCreateNavigationService(Frame rootFrame) => new NavigationServiceEx(rootFrame);

        /// <summary>
        /// Creates the device gesture service. Use this to inject your own IDeviceGestureService implementation.
        /// </summary>
        /// <returns>The initialized device gesture service.</returns>
        protected virtual IGestureService OnCreateGestureService() => null;

        /// <summary>
        /// Override this method to provide custom logic that determines whether the app should restore state from a previous session.
        /// By default, the app will only restore state when args.PreviousExecutionState is <see cref="ApplicationExecutionState"/>.Terminated.
        /// Note: restoring from state will prevent OnLaunchApplicationAsync() from getting called, 
        /// as that is only called during a fresh launch. Restoring will trigger OnResumeApplicationAsync() instead.
        /// </summary>
        /// <returns>True if the app should restore state. False if the app should perform a fresh launch.</returns>
        protected virtual bool ShouldRestoreState(IActivatedEventArgs args) => args.PreviousExecutionState == ApplicationExecutionState.Terminated;

        /// <summary>
        /// Creates the shell of the app.
        /// </summary>
        protected virtual UIElement CreateShell(Frame rootFrame)
        {
            return rootFrame;
        }

        #region Lifecycle

        /// <summary>
        /// Invoked when the application is launched normally by the end user and the application is not resuming.
        /// Other entry points will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            await InitializeShell(args);

            // If the app is launched via the app's primary tile, the args.TileId property
            // will have the same value as the AppUserModelId, which is set in the Package.appxmanifest.
            // See http://go.microsoft.com/fwlink/?LinkID=288842
            string tileId = ManifestHelper.GetApplicationId();

            if (Window.Current.Content != null && (!_isRestoringFromTermination || (args != null && args.TileId != tileId)))
            {
                await OnLaunchApplicationAsync(args);
            }
            else if (Window.Current.Content != null && _isRestoringFromTermination)
            {
                await OnResumeApplicationAsync(args);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// OnActivated is the entry point for an application when it is launched via
        /// means other normal user interaction. This includes Voice Commands, URI activation,
        /// being used as a share target from another app, etc.
        /// </summary>
        /// <param name="args">Details about the activation method, including the activation
        /// phrase (for voice commands) and the semantic interpretation, parameters, etc.</param>
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            await InitializeShell(args);

            if (Window.Current.Content != null && (!_isRestoringFromTermination || args != null))
            {
                await OnActivateApplicationAsync(args);
            }
            else if (Window.Current.Content != null && _isRestoringFromTermination && !_handledOnResume)
            {
                await OnResumeApplicationAsync(args);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            IsSuspending = true;
            try
            {
                var deferral = e.SuspendingOperation.GetDeferral();

                //Custom calls before suspending.
                await OnSuspendingApplicationAsync();

                // Save application state
                await SaveNavigationStateAsync();

                deferral.Complete();
            }
            finally
            {
                IsSuspending = false;
            }
        }

        /// <summary>
        /// Invoked when the application is suspending, but before the general suspension calls.
        /// </summary>
        protected virtual Task OnSuspendingApplicationAsync() => Task.CompletedTask;

        private async void OnResuming(object sender, object e)
        {
            if (RestoreNavigationStateOnResume)
                await RestoreNavigationStateAsync();

            _handledOnResume = true;
            await OnResumeApplicationAsync(null).ConfigureAwait(false); // explicit fire and forget, would lock the app if we await
        }

        /// <summary>
        /// This method is called after the navigation state of <see cref="RootFrame"/> is restored.
        /// Note: This is called whenever the app is resuming from suspend and terminate, but not during a fresh launch and 
        /// not when reactivating the app if it hasn't been suspended.
        /// </summary>
        /// <param name="args">
        /// - The <see cref="IActivatedEventArgs"/> instance containing the event data if the app is activated.
        /// - Null if the app is only suspended and resumed without reactivation 
        /// </param>
        protected virtual Task OnResumeApplicationAsync(IActivatedEventArgs args)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region Frame

        private const string NavigationStateFileName = "_NavigationState.xml";
        private const string NavigationDataProtectionProvider = "LOCAL=user";
        /// <summary>
        /// The main frame the application will use.
        /// </summary>
        protected Frame RootFrame { get; private set; }

        /// <summary>
        /// Creates the root frame. Use this to inject your own Frame implementation.
        /// </summary>
        protected virtual Frame OnCreateRootFrame() => null;

        private async Task<bool> CanRestoreNavigationStateAsync()
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(NavigationStateFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task SaveNavigationStateAsync()
        {
            MemoryStream sessionData = new MemoryStream();
            var session = RootFrame.GetNavigationState();
            new DataContractSerializer(typeof(string)).WriteObject(sessionData, session);

            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(NavigationStateFileName, CreationCollisionOption.ReplaceExisting);
            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var provider = new DataProtectionProvider(NavigationDataProtectionProvider);
                // Encrypt the session data and write it to disk.
                await provider.ProtectStreamAsync(sessionData.AsInputStream(), fileStream);
                await fileStream.FlushAsync();
            }
        }

        private async Task RestoreNavigationStateAsync()
        {
            try
            {
                // Get the input stream for the SessionState file
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(NavigationStateFileName);
                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    var memoryStream = new MemoryStream();
                    var provider = new DataProtectionProvider(NavigationDataProtectionProvider);

                    // Decrypt the prevously saved session data.
                    await provider.UnprotectStreamAsync(inStream, memoryStream.AsOutputStream());
                    // Deserialize the Session State
                    var data = new DataContractSerializer(typeof(string)).ReadObject(memoryStream);
                    RootFrame.SetNavigationState((string)data);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Couldn't Restore the state.", e);
            }
        }
        #endregion

        private async Task InitializeShell(IActivatedEventArgs args)
        {
            if (Window.Current.Content == null)
            {
                Frame rootFrame = RootFrame = OnCreateRootFrame() ?? new Frame();

                GestureService = OnCreateGestureService() ?? new GestureService();
                GestureService.GoBackRequested += OnGoBackRequested;
                GestureService.GoForwardRequested += OnGoForwardRequested;

                NavigationService = OnCreateNavigationService(rootFrame);

                bool canRestore = await CanRestoreNavigationStateAsync();
                bool shouldRestore = canRestore && ShouldRestoreState(args);

                await OnInitializeAsync(args);

                if (shouldRestore)
                {
                    // Restore the saved session state and navigate to the last page visited
                    try
                    {
                        await RestoreNavigationStateAsync();
                        _isRestoringFromTermination = true;
                    }
                    catch
                    {
                        // Something went wrong restoring state.
                        // Assume there is no state and continue
                    }
                }

                Shell = CreateShell(rootFrame);

                Window.Current.Content = Shell ?? rootFrame;
            }
        }
    }
}

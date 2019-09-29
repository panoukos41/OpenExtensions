using OpenExtensions.Core.Services;
using System;
using Windows.Devices.Input;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace OpenExtensions.Uwp.Services
{
    /// <summary>
    /// The DeviceGestureService class is used for handling mouse, 
    /// keyboard and other gesture events.
    /// </summary>
    public class GestureService : IGestureService, IDisposable
    {
        /// <summary></summary>
        public bool IsHardwareBackButtonPresent { get; private set; }
        /// <summary></summary>
        public bool IsHardwareCameraButtonPresent { get; private set; }
        /// <summary></summary>
        public bool IsKeyboardPresent { get; private set; }
        /// <summary></summary>
        public bool IsMousePresent { get; private set; }
        /// <summary></summary>
        public bool IsTouchPresent { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="GestureService"/>
        /// </summary>
        public GestureService()
        {
            IsHardwareBackButtonPresent = ApiInformation.IsEventPresent("Windows.Phone.UI.Input.HardwareButtons", "BackPressed");

            IsKeyboardPresent = new KeyboardCapabilities().KeyboardPresent != 0;
            IsMousePresent = new MouseCapabilities().MousePresent != 0;
            IsTouchPresent = new TouchCapabilities().TouchPresent != 0;

#if WINDOWS_PHONE_APP
            if (IsHardwareBackButtonPresent)
                HardwareButtons.BackPressed += OnHardwareButtonsBackPressed;
#endif // WINDOWS_PHONE_APP

            if (IsMousePresent)
                MouseDevice.GetForCurrentView().MouseMoved += OnMouseMoved;

            SystemNavigationManager.GetForCurrentView().BackRequested += OnSystemNavigationManagerBackRequested;
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += OnAcceleratorKeyActivated;
            Window.Current.CoreWindow.PointerPressed += OnPointerPressed;
        }

        /// <summary>
        /// The handlers attached to GoBackRequested are invoked in reverse order
        /// so that handlers added by the users are invoked before handlers in the system.
        /// </summary>
        public event EventHandler<GestureEventArgs> GoBackRequested;

        /// <summary>
        /// The handlers attached to GoForwardRequested are invoked in reverse order
        /// so that handlers added by the users are invoked before handlers in the system.
        /// </summary>
        public event EventHandler<GestureEventArgs> GoForwardRequested;
        /// <summary></summary>
        public event EventHandler<MouseEventArgs> MouseMoved;

        /// <summary>
        /// Dispose implementation - unsubscribes from nav state changes
        /// </summary>
        public void Dispose()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnSystemNavigationManagerBackRequested;
            Window.Current.CoreWindow.PointerPressed -= OnPointerPressed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnMouseMoved(MouseDevice sender, MouseEventArgs args)
        {
            Core.Extensions.RaiseEvent(MouseMoved, this, args);
        }

        /// <summary>
        /// Raised when backwards navigation is requested.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSystemNavigationManagerBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = RaiseGoBackRequested();
        }

        /// <summary>
        /// Raise <see cref="GoBackRequested"/>
        /// </summary>
        /// <returns>true if handled false otherwise</returns>
        public bool RaiseGoBackRequested()
        {
            var args = new GestureEventArgs();
            Core.Extensions.RaiseCancelableEvent(GoBackRequested, this, args);
            return args.Handled;
        }

        /// <summary>
        /// Raise <see cref="GoForwardRequested"/>
        /// </summary>
        /// <returns>true if handled false otherwise</returns>
        public bool RaiseGoForwardRequested()
        {
            var args = new GestureEventArgs();
            Core.Extensions.RaiseCancelableEvent(GoForwardRequested, this, args);
            return args.Handled;
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnHardwareButtonsBackPressed(object sender, BackPressedEventArgs e)
        {
            var args = new GestureEventArgs(false, true);
            RaiseCancelableEvent(GoBackRequested, this, args);
            e.Handled = args.Handled;
        }
#endif // WINDOWS_PHONE_APP

        /// <summary>
        /// Invoked on every keystroke, including system keys such as Alt key combinations.
        /// Used to detect keyboard navigation between pages.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="args">Event data describing the conditions that led to the event.</param>
        protected virtual void OnAcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {

        }

        /// <summary>
        /// Invoked on every mouse click, touch screen tap, or equivalent interaction.
        /// Used to detect browser-style next and previous mouse button clicks to navigate between pages.
        /// By defualt the 2 mouse keys work like the broswer, ovveride to implement your own logic.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="args">Event data describing the conditions that led to the event.</param>
        protected virtual void OnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            var properties = args.CurrentPoint.Properties;

            // Ignore button chords with the left, right, and middle buttons
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed || properties.IsMiddleButtonPressed)
                return;

            // If back or foward are pressed (but not both) navigate appropriately
            bool backPressed = properties.IsXButton1Pressed;
            bool forwardPressed = properties.IsXButton2Pressed;

            if (backPressed ^ forwardPressed)
            {
                args.Handled = true;

                if (backPressed)
                    RaiseGoBackRequested();

                if (forwardPressed)
                    RaiseGoForwardRequested();
            }
        }
    }
}

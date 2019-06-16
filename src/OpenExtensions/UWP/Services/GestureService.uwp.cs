using System;
using System.ComponentModel;
using Windows.Devices.Input;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace OpenExtensions.UWP.Services
{
    /// <summary>
    /// The DeviceGestureService class is used for handling mouse, 
    /// keyboard, hardware button and other gesture events.
    /// </summary>
    public class GestureService : IGestureService, IDisposable
    {
        public bool IsHardwareBackButtonPresent { get; private set; }
        public bool IsHardwareCameraButtonPresent { get; private set; }

        public bool IsKeyboardPresent { get; private set; }
        public bool IsMousePresent { get; private set; }
        public bool IsTouchPresent { get; private set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="GestureService"/>
        /// </summary>
        public GestureService()
        {           
            IsHardwareBackButtonPresent = ApiInformation.IsEventPresent("Windows.Phone.UI.Input.HardwareButtons", "BackPressed");
            IsHardwareCameraButtonPresent = ApiInformation.IsEventPresent("Windows.Phone.UI.Input.HardwareButtons", "CameraPressed");

            IsKeyboardPresent = new KeyboardCapabilities().KeyboardPresent != 0;
            IsMousePresent = new MouseCapabilities().MousePresent != 0;
            IsTouchPresent = new TouchCapabilities().TouchPresent != 0;

#if WINDOWS_PHONE_APP
            if (IsHardwareBackButtonPresent)
                HardwareButtons.BackPressed += OnHardwareButtonsBackPressed;
#endif // WINDOWS_PHONE_APP

            if (IsHardwareCameraButtonPresent)
            {
                HardwareButtons.CameraHalfPressed += OnHardwareButtonCameraHalfPressed;
                HardwareButtons.CameraPressed += OnHardwareButtonCameraPressed;
                HardwareButtons.CameraReleased += OnHardwareButtonCameraReleased;
            }

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
        public event EventHandler<GestureEventArgs> CameraButtonHalfPressed;
        public event EventHandler<GestureEventArgs> CameraButtonPressed;
        public event EventHandler<GestureEventArgs> CameraButtonReleased;
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
        /// Invokes the handlers attached to an eventhandler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventHandler">The EventHandler</param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void RaiseEvent<T>(EventHandler<T> eventHandler, object sender, T args)
        {
            EventHandler<T> handler = eventHandler;

            if (handler != null)
                foreach (EventHandler<T> del in handler.GetInvocationList())
                {
                    try
                    {
                        del(sender, args);
                    }
                    catch { } // Events should be fire and forget, subscriber fail should not affect publishing process
                }
        }

        /// <summary>
        /// Invokes the handlers attached to an eventhandler in reverse order and stops if a handler has canceled the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventHandler">The EventHandler</param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void RaiseCancelableEvent<T>(EventHandler<T> eventHandler, object sender, T args) where T : CancelEventArgs
        {
            EventHandler<T> handler = eventHandler;

            if (handler != null)
            {
                Delegate[] invocationList = handler.GetInvocationList();

                for (int i = invocationList.Length - 1; i >= 0; i--)
                {
                    EventHandler<T> del = (EventHandler<T>)invocationList[i];

                    try
                    {
                        del(sender, args);

                        if (args.Cancel)
                            break;
                    }
                    catch { } // Events should be fire and forget, subscriber fail should not affect publishing process
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnMouseMoved(MouseDevice sender, MouseEventArgs args)
        {
            RaiseEvent(MouseMoved, this, args);
        }

        /// <summary>
        /// Raised when backwards navigation is requested.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSystemNavigationManagerBackRequested(object sender, BackRequestedEventArgs e)
        {
            var args = new GestureEventArgs();
            RaiseCancelableEvent(GoBackRequested, this, args);
            e.Handled = args.Handled;
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
                    RaiseCancelableEvent(GoBackRequested, this, new GestureEventArgs());

                if (forwardPressed)
                    RaiseCancelableEvent(GoForwardRequested, this, new GestureEventArgs());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnHardwareButtonCameraHalfPressed(object sender, CameraEventArgs e)
        {
            RaiseEvent(CameraButtonHalfPressed, this, new GestureEventArgs(false, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnHardwareButtonCameraPressed(object sender, CameraEventArgs e)
        {
            RaiseEvent(CameraButtonPressed, this, new GestureEventArgs(false, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnHardwareButtonCameraReleased(object sender, CameraEventArgs e)
        {
            RaiseEvent(CameraButtonReleased, this, new GestureEventArgs(false, true));
        }
    }
}

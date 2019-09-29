using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

//
// This is the original ModalDialog of template 10 since the project seems dead i ported this usefull control here.
// https://github.com/Windows-XAML/Template10
//

#pragma warning disable 1591
namespace OpenExtensions.Uwp.UI.Controls
{
    /// <summary>
    /// A modal dialog is a control that can display a modal control when requested above the currently showed content.
    /// </summary>
    public sealed class ModalDialog : ContentControl
    {
        VisualStateGroup CommonStates;

        /// <summary></summary>
        public ModalDialog()
        {
            DefaultStyleKey = typeof(ModalDialog);
        }

        protected override void OnApplyTemplate()
        {
            CommonStates = GetTemplatedChild<VisualStateGroup>(nameof(CommonStates));
            Update();
        }

        private T GetTemplatedChild<T>(string name) where T : DependencyObject
        {
            return (GetTemplateChild(name) as T) ?? throw new NullReferenceException(name);
        }

        private void Update()
        {
            if (CommonStates == null)
                return;

            var state = (IsModal) ? "Modal" : "Normal";
            VisualStateManager.GoToState(this, state, true);

            // this switch ensures ModalTransitions plays every time.
            if (!IsModal)
            {
                var content = ModalContent;
                ModalContent = null;
                ModalContent = content;
            }
        }

        #region properties

        public bool IsModal
        {
            get { return (bool)GetValue(IsModalProperty); }
            set { SetValue(IsModalProperty, value); }
        }
        public static readonly DependencyProperty IsModalProperty = DependencyProperty
            .Register(nameof(IsModal), typeof(bool), typeof(ModalDialog), new PropertyMetadata(false, (d, e) => (d as ModalDialog).Update()));

        public Brush ModalBackground
        {
            get { return (Brush)GetValue(ModalBackgroundProperty); }
            set { SetValue(ModalBackgroundProperty, value); }
        }
        public static readonly DependencyProperty ModalBackgroundProperty = DependencyProperty
            .Register(nameof(ModalBackground), typeof(Brush), typeof(ModalDialog), new PropertyMetadata(null));

        public UIElement ModalContent
        {
            get { return (UIElement)GetValue(ModalContentProperty); }
            set { SetValue(ModalContentProperty, value); }
        }
        public static readonly DependencyProperty ModalContentProperty = DependencyProperty
            .Register(nameof(ModalContent), typeof(UIElement), typeof(ModalDialog), new PropertyMetadata(null));

        public TransitionCollection ModalTransitions
        {
            get { return (TransitionCollection)GetValue(ModalTransitionsProperty); }
            set { SetValue(ModalTransitionsProperty, value); }
        }
        public static readonly DependencyProperty ModalTransitionsProperty = DependencyProperty
            .Register(nameof(ModalTransitions), typeof(TransitionCollection), typeof(ModalDialog), new PropertyMetadata(null));

        #endregion
    }
}
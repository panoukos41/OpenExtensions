using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace OpenExtensions.Droid.UI.Controls
{
    /// <summary>
    /// A <see cref="ProgressBar"/> with Indeterminate equal to true and a method that shows or hides the loading,
    /// it will count how many times loading is set to true (+1) or false (-1) and will disapear only when loading count reaches 0.
    /// </summary>
    [Register("openextensions.droid.ui.controls.LoadingBar")]
    public class LoadingBar : ProgressBar
    {
        private readonly object loadingLock = new object();
        private int loadingCounter = 0;

        /// <summary>
        /// Get or Set if loading should start, the control keeps a counter when you set true and will disable loading only when the counter is 0.
        /// </summary>
        public bool IsLoading
        {
            get => loadingCounter == 0;
            set => SetLoading(value);
        }

        #region Constructors
        /// <summary></summary>
        public LoadingBar(Context context) : this(context, null) { }

        /// <summary></summary>
        public LoadingBar(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Indeterminate = true;
        }
        #endregion

        /// <summary>
        /// Set if loading should start, the control keeps a counter when you set true and will disable loading only when the counter is 0.
        /// </summary>
        public void SetLoading(bool state)
        {
            lock (loadingLock)
            {
                if (state)
                {
                    loadingCounter++;
                    if (Visibility == ViewStates.Invisible)
                        Visibility = ViewStates.Visible;
                }
                else
                {
                    if (--loadingCounter < 0)
                        loadingCounter = 0;
                    if (loadingCounter == 0 && Visibility == ViewStates.Visible)
                        Visibility = ViewStates.Invisible;
                }
            }
        }
    }
}

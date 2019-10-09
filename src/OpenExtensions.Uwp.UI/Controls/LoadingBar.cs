using Windows.UI.Xaml.Controls;

namespace OpenExtensions.Uwp.UI.Controls
{
    /// <summary>
    /// A <see cref="ProgressBar"/> with a method that shows or hides the an indeterminate loading,
    /// it will count how many times loading is set to true (+1) or false (-1) and will disapear only when loading count reaches 0.
    /// </summary>
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

        /// <summary></summary>
        public LoadingBar() : base() { }

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
                    if (!IsIndeterminate)
                        IsIndeterminate = true;
                }
                else
                {
                    if (--loadingCounter < 0)
                        loadingCounter = 0;
                    if (loadingCounter == 0 && IsIndeterminate)
                        IsIndeterminate = false;
                }
            }
        }
    }
}

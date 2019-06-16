using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace OpenExtensions.UWP
{
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Method to await on a storyboard till it finishes.
        /// </summary>
        /// <param name="storyboard"></param>
        /// <returns></returns>
        public static async Task PlayAsync(this Storyboard storyboard)
        {
            var tcs = new TaskCompletionSource<object>();
            void lambda(object s, object e) => tcs.TrySetResult(null);

            try
            {
                storyboard.Completed += lambda;
                storyboard.Begin();
                await tcs.Task;
            }
            finally
            {
                storyboard.Completed -= lambda;
            }
        }
    }
}

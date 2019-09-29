using Android.Animation;
using System.Threading.Tasks;

namespace OpenExtensions.Droid
{
    public static partial class Extensions
    {
        /// <summary>
        /// A way to await for an animation to finish before continuing execution.
        /// </summary>
        public static async Task StartAsync(this Animator animator)
        {
            var tsk = new TaskCompletionSource<object>();
            void lambda(object s, object e) => tsk.TrySetResult(null);

            try
            {
                animator.AnimationEnd += lambda;
                animator.Start();
                await tsk.Task;
            }
            finally
            {
                animator.AnimationEnd -= lambda;
            }
        }
    }
}

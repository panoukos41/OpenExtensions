using OpenExtensions.Droid.App;
using System.Threading.Tasks;

namespace OpenExtensions.Droid.FragmentNavigation
{
    /// <summary>
    /// A fragment based on <see cref="FragmentBase"/> meant to be used with
    /// <see cref="FragmentNavigationService"/> and provides a 
    /// PARAMETER property to store navigation parameters.
    /// </summary>
    public class NavigationFragment : FragmentBase
    {
        /// <summary>
        /// Navigation parameter.
        /// </summary>
        public object Nav_parameter { get; set; }

        private bool started = false;

        /// <summary>
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            if (!started)
            {
                started = true;
                OnNavigatingTo();
            }
        }

        /// <summary>
        /// </summary>
        public override void OnDestroyView()
        {
            base.OnDestroyView();
            started = false;
            OnNavigatingFrom();
        }

        /// <summary>
        /// Called at the start of <see cref="OnStart()"/> only once.
        /// No need to run base.OnNavigatingTo()
        /// </summary>
        public virtual Task OnNavigatingTo()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called at the start of <see cref="OnDestroyView()"/>.
        /// No need to run base.OnNavigationgFrom()
        /// </summary>
        public virtual Task OnNavigatingFrom()
        {
            return Task.CompletedTask;
        }
    }
}
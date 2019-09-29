using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OpenExtensions.Uwp.UI.Controls
{
    /// <summary>
    /// Class that derives from <see cref="Control"/> with usefull methods.
    /// </summary>
    public abstract class ControlEx : Control
    {
        /// <summary>
        /// Get a child control casted as T.
        /// </summary>
        protected T GetTemplatedChild<T>(string name) where T : DependencyObject
        {
            return (GetTemplateChild(name) as T) ?? throw new NullReferenceException(name);
        }

        /// <summary>
        /// Try getting a child control casted as T. If it fails returns false.
        /// </summary>
        protected bool TryGetTemplatedChild<T>(string name, out T child) where T : DependencyObject
        {
            child = default;
            bool result = true;
            try
            {
                child = GetTemplatedChild<T>(name);
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}

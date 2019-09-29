using Android.Views;
using GalaSoft.MvvmLight.Helpers;
using System;
using System.Linq.Expressions;

namespace OpenExtensions.Droid
{
    public static partial class Extensions
    {
        /// <summary>
        /// Create an MVVMLight binding from the view.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target property.</typeparam>
        /// <param name="target">The view the binding will associate with.</param>
        /// <param name="source">The object the binding will use to bind to the view.</param>
        /// <param name="sourceProperty">The property of the source object to observe.</param>
        /// <param name="targetProperty">The property of the view to change.</param>
        /// <param name="mode"><see cref="BindingMode"/></param>
        /// <param name="fallbackValue">A value to fallback to if anything goes wrong.</param>
        /// <param name="targetNullValue">A value to fallback to if the source value is null.</param>
        /// <returns></returns>
        public static Binding CreateBinding<TSource, TTarget>(
            this View target,
            object source,
            Expression<Func<TSource>> sourceProperty,
            Expression<Func<TTarget>> targetProperty = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default)
        {
            return new Binding<TSource, TTarget>(source, sourceProperty, target, targetProperty, mode, fallbackValue, targetNullValue);
        }
    }
}

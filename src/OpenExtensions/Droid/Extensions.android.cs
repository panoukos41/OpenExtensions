using Android.Animation;
using Android.Content;
using Android.Database;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenExtensions.Droid
{
    /// <summary>
    /// Extension methods for anything that has to do with android and mvvmlight.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns a SimpleCursorAdapter with a MatrixCursor as the cursor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="layout">the layout to use to display the properties.</param>
        /// <param name="to">id of the view for the displayed property.</param>
        public static SimpleCursorAdapter CreateSimpleCursorAdapter(Context context, int layout, int[] to)
        {
            return new SimpleCursorAdapter(context, layout, null, new string[] { "text" }, to, CursorAdapterFlags.None);
        }

        /// <summary>
        /// Returns a cursor adapter, use <see cref="CreateSimpleCursorAdapter(Context, int, int[])"/> to create an adapter.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="text">The text to display</param>
        /// <param name="serialize">A way to serialize the object and retrieve it from the third column => [2] as a string.</param>
        public static MatrixCursor ToMatrixCursor<T>(this IEnumerable<T> collection, Func<T, string> text, Func<T, string> serialize = null)
        {
            MatrixCursor cursor = new MatrixCursor(new string[] { "_id", "text", "item" });

            int id = 0;
            foreach (var item in collection)
            {
                cursor.AddRow(new Java.Lang.Object[] { id.ToString(), text?.Invoke(item), serialize?.Invoke(item) });
                id++;
            }
            return cursor;
        }

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

        /// <summary>
        /// An extension to turn a string collection into an <see cref="ArrayAdapter{T}"/> adapter of string.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="context"></param>
        /// <param name="textViewResourceId">Leave 0 to use the default <see cref="Resource.Layout.support_simple_spinner_dropdown_item"/></param>
        /// <returns></returns>
        public static ArrayAdapter<string> ToArrayAdapter(
            this IEnumerable<string> collection, 
            Context context, 
            int textViewResourceId = 0)
        {
            if (textViewResourceId == 0)
            {
                return new ArrayAdapter<string>(context, Resource.Layout.support_simple_spinner_dropdown_item, collection.ToList());
            }
            return new ArrayAdapter<string>(context, textViewResourceId, collection.ToList());
        }

        /// <summary>
        /// An extension to turn a collection of T into an <see cref="ArrayAdapter{T}"/> adapter of string.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="context"></param>
        /// <param name="selector">A fucn to choose the string title of the item.</param>
        /// <param name="textViewResourceId">Leave 0 to use the default <see cref="Resource.Layout.support_simple_spinner_dropdown_item"/></param>
        /// <returns></returns>
        public static ArrayAdapter<string> ToArrayAdapter<T>(
            this IEnumerable<T> collection, 
            Context context, 
            Func<T, string> selector,
            int textViewResourceId = 0)
        {
            return ToArrayAdapter(collection.Select(selector), context, textViewResourceId);
        }
    }
}
using Android.Content;
using Android.Database;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace OpenExtensions.Droid
{
    /// <summary>
    /// Extension methods for anything that has to do with android and mvvmlight.
    /// </summary>
    public static partial class Extensions
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
        /// Returns a SimpleCursorAdapter with a MatrixCursor as the cursor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="layout">the layout to use to display the properties.</param>
        /// <param name="to">id of the view for the displayed property.</param>
        public static Android.Support.V4.Widget.SimpleCursorAdapter CreateSimpleCursorAdapterSupport(Context context, int layout, int[] to)
        {
            return new Android.Support.V4.Widget.SimpleCursorAdapter(context, layout, null, new string[] { "text" }, to);
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
    }
}
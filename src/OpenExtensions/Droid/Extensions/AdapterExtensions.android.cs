using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenExtensions.Droid
{
    public static partial class Extensions
    {
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

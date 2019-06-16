using OpenExtensions.Collections;
using System;
using System.Collections.Generic;

namespace OpenExtensions
{
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Return any <see cref="IEnumerable{T}"/> to an <see cref="OCollection{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static OCollection<T> ToOcollection<T>(this IEnumerable<T> collection)
        {
            return new OCollection<T>(collection);
        }

        /// <summary>
        /// A for each extension tha can be called in a fluent manner.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> FluentForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
            return collection;
        }
    }
}

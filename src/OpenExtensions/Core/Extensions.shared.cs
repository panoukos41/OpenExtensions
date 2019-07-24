using OpenExtensions.Core.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OpenExtensions.Core
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

        /// <summary>
        /// Invokes the handlers attached to an eventhandler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventHandler">The EventHandler</param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void RaiseEvent<T>(EventHandler<T> eventHandler, object sender, T args)
        {
            EventHandler<T> handler = eventHandler;

            if (handler != null)
                foreach (EventHandler<T> del in handler.GetInvocationList())
                {
                    try
                    {
                        del(sender, args);
                    }
                    catch { } // Events should be fire and forget, subscriber fail should not affect publishing process
                }
        }

        /// <summary>
        /// Invokes the handlers attached to an eventhandler in reverse order and stops if a handler has canceled the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventHandler">The EventHandler</param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void RaiseCancelableEvent<T>(EventHandler<T> eventHandler, object sender, T args) where T : CancelEventArgs
        {
            EventHandler<T> handler = eventHandler;

            if (handler != null)
            {
                Delegate[] invocationList = handler.GetInvocationList();

                for (int i = invocationList.Length - 1; i >= 0; i--)
                {
                    EventHandler<T> del = (EventHandler<T>)invocationList[i];

                    try
                    {
                        del(sender, args);

                        if (args.Cancel)
                            break;
                    }
                    catch { } // Events should be fire and forget, subscriber fail should not affect publishing process
                }
            }
        }
    }
}

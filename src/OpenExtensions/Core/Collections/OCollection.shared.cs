using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace OpenExtensions.Core.Collections
{
    /// <summary>
    /// An <see cref="ObservableCollection{T}"/> with some extra methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Initialize a new instance.
        /// </summary>
        public OCollection() : base() { }

        /// <summary>
        /// Initialize a new instance with the elements from the provided collection.
        /// </summary>
        public OCollection(IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// Sorts the collection based on the provided selector,
        /// it performs a remove and insert.
        /// </summary>
        /// <typeparam name="Tresult"></typeparam>
        /// <param name="selector"></param>
        public void Sort<Tresult>(Func<T, Tresult> selector)
        {
            var sorted = Items.OrderBy(selector).ToList();

            for (int i = 0; i < sorted.Count; ++i)
            {
                var actualItemIndex = Items.IndexOf(sorted[i]);

                if (actualItemIndex != i)
                {
                    RemoveAt(actualItemIndex);
                    Insert(i, sorted[i]);
                }
            }
        }

        /// <summary>
        /// Add a range to the collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="notificationMode"></param>
        public void AddRange(IEnumerable<T> collection, NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                foreach (var i in collection)
                    Items.Add(i);

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

                return;
            }
            else
            {
                int startIndex = Count;
                var changedItems = collection is List<T> ? (List<T>)collection : new List<T>(collection);
                foreach (var i in changedItems)
                    Items.Add(i);

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startIndex));

                return;
            }
        }

        /// <summary>
        /// Replace the collection with a new one.
        /// </summary>
        /// <param name="collection"></param>
        public void ReplaceRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            Items.Clear();
            AddRange(collection, NotifyCollectionChangedAction.Reset);
        }
    }
}

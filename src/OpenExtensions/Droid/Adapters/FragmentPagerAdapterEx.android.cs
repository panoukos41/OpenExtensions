using Android.Support.V4.App;
using Java.Lang;
using System;

namespace OpenExtensions.Droid.Adapters
{
    /// <summary>
    /// An adapter that provides the 2 important methods out of the box.
    /// </summary>
    public class FragmentPagerAdapterEx : FragmentPagerAdapter, IFragmentPagerAdapterEx
    {
        /// <summary>
        /// item count.
        /// </summary>
        public override int Count => _Count;
        private readonly int _Count;
        private Func<int,Fragment> getItem { get; set; }
        private Func<int, ICharSequence> getPageTitle { get; set; }        

        /// <summary>
        /// Create a pager adapter that hosts a fixed number of fragments.
        /// </summary>
        /// <param name="fm">The <see cref="FragmentManager"/>.</param>
        /// <param name="count">The number of fragments that the adapter will host.</param>
        /// <param name="GetItem">The method that returns a fragment for a given position.</param>
        /// <param name="GetTitle">The method that returns a title for a given position.</param>
        public FragmentPagerAdapterEx(FragmentManager fm, int count, Func<int,Fragment> GetItem, Func<int, ICharSequence> GetTitle) : base(fm)
        {
            _Count = count;
            getItem = GetItem;
            getPageTitle = GetTitle;
        }

        /// <summary>
        /// Create a pager adapter that hosts a fixed number of fragments and uses the methods of the 
        /// provided <see cref="IFragmentPagerAdapterEx"/> <paramref name="obj"/>.
        /// </summary>
        /// <param name="fm">The <see cref="FragmentManager"/>.</param>
        /// <param name="count">The number of fragments that the adapter will host</param>
        /// <param name="obj">The object that implements the <see cref="IFragmentPagerAdapterEx"/> interface.</param>
        public FragmentPagerAdapterEx(FragmentManager fm, int count, IFragmentPagerAdapterEx obj) : base(fm)
        {
            _Count = count;
            getItem = obj.GetPage;
            getPageTitle = obj.GetPageTitle;
        }        

        /// <summary>
        /// </summary>
        public override Fragment GetItem(int position) => getItem(position);

        /// <summary>
        /// </summary>
        public override ICharSequence GetPageTitleFormatted(int position) => getPageTitle(position);

        Fragment IFragmentPagerAdapterEx.GetPage(int index) => GetItem(index);

        ICharSequence IFragmentPagerAdapterEx.GetPageTitle(int index) => GetPageTitleFormatted(index);
    }
}
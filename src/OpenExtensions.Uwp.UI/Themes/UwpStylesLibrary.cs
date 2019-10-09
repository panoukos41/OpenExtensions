using System;
using Windows.UI.Xaml;

namespace OpenExtensions.Uwp.UI.Themes
{
    /// <summary>
    /// ResourceDictionary with the source set to the xaml dictionary that contains the resources.
    /// </summary>
    public class UwpStylesLibrary : ResourceDictionary
    {
        /// <summary>
        /// Initialize the UwpStylesLibrary ResourceDictionary
        /// </summary>
        public UwpStylesLibrary()
        {
            Source = new Uri("ms-appx:///OpenExtensions.Uwp.UI/Themes/UwpStylesLibrary.xaml");
        }
    }
}

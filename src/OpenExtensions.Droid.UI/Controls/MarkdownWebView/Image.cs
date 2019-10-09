using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    public partial class MarkdownWebView
    {
        public double? ImageMaxHeight { get; set; }
        public double? ImageMaxWidth { get; set; }
    }
}
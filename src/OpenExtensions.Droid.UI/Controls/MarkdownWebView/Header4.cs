using Android.Graphics;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    public partial class MarkdownWebView
    {
        public double? Header4FontSize { get; set; }
        public FontWeight? Header4FontWeight { get; set; }
        public Color? Header4Foreground { get; set; }
        public string Header4Margin { get; set; }

        private string Header4Css() => ResolveCommonCss("h4", Header4FontSize, Header4FontWeight, Header4Foreground, Header4Margin);
    }
}
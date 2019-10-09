using Android.Graphics;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    public partial class MarkdownWebView
    {
        public double? Header1FontSize { get; set; }
        public FontWeight? Header1FontWeight { get; set; }
        public Color? Header1Foreground { get; set; }
        public string Header1Margin { get; set; }

        private string Header1Css() => ResolveCommonCss("h1", Header1FontSize, Header1FontWeight, Header1Foreground, Header1Margin);
    }
}
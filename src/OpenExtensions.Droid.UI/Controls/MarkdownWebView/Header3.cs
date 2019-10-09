using Android.Graphics;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    public partial class MarkdownWebView
    {
        public double? Header3FontSize { get; set; }
        public FontWeight? Header3FontWeight { get; set; }
        public Color? Header3Foreground { get; set; }
        public string Header3Margin { get; set; }

        private string Header3Css() => ResolveCommonCss("h3", Header3FontSize, Header3FontWeight, Header3Foreground, Header3Margin);
    }
}
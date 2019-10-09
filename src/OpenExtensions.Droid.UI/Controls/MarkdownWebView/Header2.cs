using Android.Graphics;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    public partial class MarkdownWebView
    {
        public double? Header2FontSize { get; set; }
        public FontWeight? Header2FontWeight { get; set; }
        public Color? Header2Foreground { get; set; }
        public string Header2Margin { get; set; }

        private string Header2Css() => ResolveCommonCss("h2", Header2FontSize, Header2FontWeight, Header2Foreground, Header2Margin);
    }
}
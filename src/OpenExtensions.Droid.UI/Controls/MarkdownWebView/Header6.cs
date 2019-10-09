using Android.Graphics;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    public partial class MarkdownWebView
    {
        public double? Header6FontSize { get; set; }
        public FontWeight? Header6FontWeight { get; set; }
        public Color? Header6Foreground { get; set; }
        public string Header6Margin { get; set; }

        private string Header6Css() => ResolveCommonCss("h6", Header6FontSize, Header6FontWeight, Header6Foreground, Header6Margin);
    }
}
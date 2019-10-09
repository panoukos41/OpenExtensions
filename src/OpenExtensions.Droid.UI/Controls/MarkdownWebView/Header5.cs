using Android.Graphics;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    public partial class MarkdownWebView
    {
        public double? Header5FontSize { get; set; }
        public FontWeight? Header5FontWeight { get; set; }
        public Color? Header5Foreground { get; set; }
        public string Header5Margin { get; set; }

        private string Header5Css() => ResolveCommonCss("h5", Header5FontSize, Header5FontWeight, Header5Foreground, Header5Margin);
    }
}
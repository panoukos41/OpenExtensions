using Android.Graphics;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    public partial class MarkdownWebView
    {
        public double? ParagraphFontSize { get; set; }
        public FontWeight? ParagraphFontWeight { get; set; }
        public Color? ParagraphForeground { get; set; }
        public string ParagraphMargin { get; set; }

        private string ParagraphCss() => ResolveCommonCss("p", ParagraphFontSize, ParagraphFontWeight, ParagraphForeground, ParagraphMargin);
    }
}
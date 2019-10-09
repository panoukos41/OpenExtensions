using Android.Graphics;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    public partial class MarkdownWebView
    {
        public Color? LinkForegroundColor { get; set; }

        private string LinkCss()
        {
            string css =
                $"a{{\n" +
                $"{ResolveForeground(LinkForegroundColor)}\n" +
                $"}}\n";

            if (css.Length > 6)
                return css;
            return "";
        }
    }
}
using Android.Graphics;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    public partial class MarkdownWebView
    {
        public Color? BackgroundColor { get; set; }
        public Color? ForegroundColor { get; set; }

        private string BodyCss()
        {
            string css = 
                $"body{{\n" +
                $"{ResolveBackground(BackgroundColor)}\n" +
                $"{ResolveForeground(ForegroundColor)}\n" +
                $"}}\n";

            if (css.Length > 10)
                return css;
            return "";
        }
    }
}
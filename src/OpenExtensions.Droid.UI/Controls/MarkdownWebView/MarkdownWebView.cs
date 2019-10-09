using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Webkit;
using Markdig;
using System;
using Android.Graphics;

#pragma warning disable 1591
namespace OpenExtensions.Droid.UI.Controls
{
    /// <summary></summary>
    [Register("openextensions.droid.ui.controls.MarkdownWebView")]
    public partial class MarkdownWebView : WebView
    {
        #region Constructors

        public MarkdownWebView(Context context) : base(context)
        {
            Init();
        }

        public MarkdownWebView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public MarkdownWebView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public MarkdownWebView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public MarkdownWebView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }
        #endregion

        private void Init()
        {
            VerticalScrollBarEnabled = false;
            HorizontalScrollBarEnabled = false;
            SetScrollContainer(false);
        }

        /// <summary>
        /// Set the markdown for this view.
        /// </summary>
        /// <param name="markdown"></param>
        public void SetMarkdown(string markdown)
        {
            markdown = Markdown.ToHtml(markdown);
            markdown = CompleteHtmlWithCss(markdown);
            LoadDataWithBaseURL("", markdown, "text/html", "UTF-8", "");
        }

        private string CompleteHtmlWithCss(string htmlBody)
        {
            return
                $"<html>\n" +
                $"<head>\n" +
                $"<style>\n" +
                $"{BodyCss()}" +
                $"{Header1Css()}" +
                $"{Header2Css()}" +
                $"{Header3Css()}" +
                $"{Header4Css()}" +
                $"{Header5Css()}" +
                $"{Header6Css()}" +
                $"{LinkCss()}" +
                $"{ParagraphCss()}" +
                $"{CodeCss()}" +
                $"</style>\n" +
                $"</head>\n" +
                $"<body>\n" +
                $"{htmlBody}" +
                $"</body>\n" +
                $"</html>\n";
        }

        private string ColorToHex(Color color) => $"#{color.R.ToString("X2")}{color.G.ToString("X2")}{color.B.ToString("X2")}";

        private string ResolveCommonCss(string elementName, double? fontsize, FontWeight? fontweight, Color? foregroundColor, string margin)
        {
            string css = "";

            var resFontSize = ResolveFontSize(fontsize);
            var resFontWeight = ResolveFontWeight(fontweight);
            var resForeground = ResolveForeground(foregroundColor);
            var resMargin = ResolveMargin(margin);

            if (resFontSize != "" ) css += resFontSize + "\n";
            if (resFontWeight != "") css += resFontWeight + "\n";
            if (resForeground != "") css += resForeground + "\n";
            if (resMargin != "") css += resMargin + "\n";

            if(css != "")
            {
                css =
                    $"{elementName}{{\n" +
                    $"{css}" +
                    $"}}\n";
            }
            return css;
        }

        private string ResolveFontSize(double? fontsize)
        {
            if (fontsize.HasValue)
                return $"font-size: {fontsize.Value}px;";
            return "";
        }

        private string ResolveFontWeight(FontWeight? fontweight)
        {
            if (fontweight.HasValue)
                return $"font-weight: {fontweight.Value.ToString()};";
            return "";
        }

        private string ResolveForeground(Color? color)
        {
            if (color.HasValue)
                return $"color: {ColorToHex(color.Value)};";
            return "";
        }

        private string ResolveBackground(Color? color)
        {
            if (color.HasValue)
                return $"background-color: {ColorToHex(color.Value)};";
            return "";
        }

        private string ResolveMargin(string margin)
        {
            if (margin != null)
                return $"margin: {margin};";
            return "";
        }
    }
}
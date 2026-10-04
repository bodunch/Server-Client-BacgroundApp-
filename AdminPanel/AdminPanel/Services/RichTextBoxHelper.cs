using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AdminPanel.Services
{
    public static class RichTextBoxHelper
    {
        public static void AppendBlackText(RichTextBox rtb, string text)
        {
            if (rtb == null) return;

            rtb.Dispatcher.Invoke(() =>
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Margin = new Thickness(0);
                paragraph.Foreground = Brushes.Black;

                paragraph.Inlines.Add(new Run(text.TrimEnd('\r', '\n')));

                rtb.Document.Blocks.Add(paragraph);
                rtb.ScrollToEnd();
            });
        }

        public static void AppendYellowText(RichTextBox rtb, string text)
        {
            if (rtb == null) return;

            rtb.Dispatcher.Invoke(() =>
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Margin = new Thickness(0);
                paragraph.Foreground = Brushes.Yellow;

                paragraph.Inlines.Add(new Run(text.TrimEnd('\r', '\n')));

                rtb.Document.Blocks.Add(paragraph);
                rtb.ScrollToEnd();
            });
        }

        public static void AppendRedText(RichTextBox rtb, string text)
        {
            if (rtb == null) return;

            rtb.Dispatcher.Invoke(() =>
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Margin = new Thickness(0);
                paragraph.Foreground = Brushes.Red;

                paragraph.Inlines.Add(new Run(text.TrimEnd('\r', '\n')));

                rtb.Document.Blocks.Add(paragraph);
                rtb.ScrollToEnd();
            });
        }
    }
}

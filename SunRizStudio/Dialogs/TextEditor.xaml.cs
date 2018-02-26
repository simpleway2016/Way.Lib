using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SunRizStudio.Dialogs
{
    /// <summary>
    /// TextEditor.xaml 的交互逻辑
    /// </summary>
    public partial class TextEditor : Window
    {
        List<TextRange> _findedRanges = new List<TextRange>();
        object _findedRange_Foreground;
        object _findedRange_Background;
        int _windowId;
        public TextEditor(string content,int windowid)
        {
            InitializeComponent();
            _windowId = windowid;
            txtEditor.AcceptsReturn = false;
            LoadText(txtEditor, content);

          
        }
        private void LoadText(RichTextBox richTextBox, string txtContent)
        {
            richTextBox.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(txtContent));
            richTextBox.Document.Blocks.Add(paragraph);
        }

        List<TextRange> FindWordListFromPosition(TextPointer position, string word)
        {
            List<TextRange> matchingText = new List<TextRange>();
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //带有内容的文本
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    //查找关键字在这文本中的位置
                    int indexInRun = textRun.IndexOf(word);
                    int indexHistory = 0;
                    while (indexInRun >= 0)
                    {
                        TextPointer start = position.GetPositionAtOffset(indexInRun + indexHistory);
                        TextPointer end = start.GetPositionAtOffset(word.Length);
                        matchingText.Add(new TextRange(start, end));

                        indexHistory = indexHistory + indexInRun + word.Length;
                        textRun = textRun.Substring(indexInRun + word.Length);//去掉已经采集过的内容
                        indexInRun = textRun.IndexOf(word);//重新判断新的字符串是否还有关键字
                    }
                }

                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            return matchingText;
        }
        TextRange FindWordFromPosition(TextPointer position, string word)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //带有内容的文本
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    //查找关键字在这文本中的位置
                    int indexInRun = textRun.IndexOf(word);
                    int indexHistory = 0;
                    while (indexInRun >= 0)
                    {
                        TextPointer start = position.GetPositionAtOffset(indexInRun + indexHistory);
                        TextPointer end = start.GetPositionAtOffset(word.Length);
                        return new TextRange(start, end);
                    }
                }

                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            return null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            _findedRanges = FindWordListFromPosition(txtEditor.Document.ContentStart, txtSearch.Text);
            if(_findedRanges.Count > 0 )
            {
              
                _findedRange_Foreground = _findedRanges[0].GetPropertyValue(TextElement.ForegroundProperty);
                _findedRange_Background = _findedRanges[0].GetPropertyValue(TextElement.BackgroundProperty);

                foreach (TextRange range in _findedRanges)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.White));
                    range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Blue));
                }
            }
        }

        private void txtEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(_findedRanges.Count > 0)
            {
                foreach (TextRange range in _findedRanges)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, _findedRange_Foreground);
                    range.ApplyPropertyValue(TextElement.BackgroundProperty, _findedRange_Background);
                }

                _findedRanges.Clear();
            }
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            if (_findedRanges.Count == 0)
            {
                if (txtSearch.Text.Length > 0)
                {
                    btnSearch_Click(null, null);
                }
            }
            if (_findedRanges.Count > 0)
            {
                {
                    foreach (TextRange range in _findedRanges)
                    {
                        range.ApplyPropertyValue(TextElement.ForegroundProperty, _findedRange_Foreground);
                        range.ApplyPropertyValue(TextElement.BackgroundProperty, _findedRange_Background);
                    }

                    var range2 = _findedRanges[0];
                    range2.Text = txtReplace.Text;

                    btnSearch_Click(null, null);

                }
            }
        }

        private void btnReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            if(_findedRanges.Count == 0 )
            {
                if(txtSearch.Text.Length > 0)
                {
                    btnSearch_Click(null, null);
                }
            }
            if (_findedRanges.Count > 0)
            {
                foreach (TextRange range in _findedRanges)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, _findedRange_Foreground);
                    range.ApplyPropertyValue(TextElement.BackgroundProperty, _findedRange_Background);
                }
                foreach (TextRange range in _findedRanges)
                {
                    range.Text = txtReplace.Text;
                }
                _findedRanges.Clear();
            }
        }

        private void txtEditor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                txtEditor.Focus();
                txtEditor.CaretPosition = txtEditor.CaretPosition.InsertLineBreak();
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(txtEditor.Document.ContentStart, txtEditor.Document.ContentEnd);

            Helper.Remote.Invoke<int>("WriteWindowScript", (ret, err) => {
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    MessageBox.Show(this, "保存成功！");
                }
            }, _windowId, textRange.Text);
        }
    }
}

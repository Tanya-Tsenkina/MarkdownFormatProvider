using System.IO;
using System.Windows;
using Telerik.Windows.Controls;
using System.Windows.Forms;
using System;

namespace MarkdownFormatProvider_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string outPath;

        public MainWindow()
        {
            //RadRichTextBox.DefaultTextRenderingMode = Telerik.Windows.Documents.UI.TextBlocks.TextBlockRenderingMode.TextBlockWithPropertyCaching;
            InitializeComponent();

            var provider = new MarkDownFormatProvider();
            this.radRichTextBox.Document = provider.Import(new FileStream("../../SampleData/README.md", FileMode.Open));
        }

        private void ToPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                outPath = dialog.SelectedPath;
                this.toPathTextBox.Text = outPath;
            }
        }

        private void FromPathButton_Click(object sender, RoutedEventArgs e)
        {
            radRichTextBox.Commands.OpenDocumentCommand.Execute();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.toPathTextBox.Text))
            {
                System.Windows.MessageBox.Show("To path is not specified.");
                return;
            }
            var provider = new MarkDownFormatProvider();

            try
            {
                FileStream writer = new FileStream(outPath + "\\hide.html", FileMode.Create);
                using (writer)
                {
                    provider.OutPath = outPath;
                    provider.Export(radRichTextBox.Document, writer);
                }
                File.Delete(outPath + "\\hide.html");
                System.Windows.MessageBox.Show("Export completed successfully.");
            }
            catch (IOException ex)
            {
                System.Windows.MessageBox.Show(LocalizationManager.GetString("Documents_OpenDocumentCommand_TheFileIsLocked"));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(LocalizationManager.GetString("Documents_OpenDocumentCommand_TheFileCannotBeOpened"));
            }

        }
    }
}


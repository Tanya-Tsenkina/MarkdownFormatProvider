using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.FormatProviders;
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.Model;
using CommonMark;
using System;
using Aspose.Html.Saving;
using Aspose.Html;

namespace MarkdownFormatProvider_WPF
{
    [CustomDocumentFormatProvider]
    class MarkDownFormatProvider : DocumentFormatProviderBase
    {
        private static readonly string name = "MarkDownFormatProvider";
        private static readonly IEnumerable<string> supportedExtensions = new string[] { ".md", ".markdown" };
        private readonly HtmlFormatProvider htmlProvider;
        private static string outPath;

        public MarkDownFormatProvider()
        {
            this.htmlProvider = new HtmlFormatProvider();
        }

        public override string Name
        {
            get
            {
                return MarkDownFormatProvider.name;
            }
        }

        public override string FilesDescription
        {
            get
            {
                return "MarkDown Document";
            }
        }

        public string OutPath
        {
            get
            {
                return outPath;
            }
            set
            {
                outPath = value;
            }
        }

        public override IEnumerable<string> SupportedExtensions
        {
            get
            {
                return MarkDownFormatProvider.supportedExtensions;
            }
        }
        public override bool CanImport
        {
            get { return true; }
        }

        public override bool CanExport
        {
            get
            {
                return true;
            }
        }

        public override RadDocument Import(Stream input)
        {
            using (StreamReader reader = new StreamReader(input))
            {
                // Performs the first stage of the conversion - parses block elements from the source
                // and created the syntax tree.
                var blockDoc = CommonMarkConverter.ProcessStage1(reader);

                //Performs the second stage of the conversion - parses block element contents into
                //inline elements.
                CommonMarkConverter.ProcessStage2(blockDoc);

                string tempFileName = System.IO.Path.GetTempFileName();
                StreamWriter writer = new StreamWriter(tempFileName);
                using (writer)
                {
                    // Performs the last stage of the conversion - converts the syntax tree to HTML
                    // representation.
                    CommonMarkConverter.ProcessStage3(blockDoc, writer);
                }

                RadDocument document;
                using (FileStream stream = File.OpenRead(tempFileName))
                {
                    document = htmlProvider.Import(stream);
                }
                File.Delete(tempFileName);

                return document;
            }
        }

        public override void Export(RadDocument document, Stream output)
        {

            //export as HTML
            using (output)
            {
                htmlProvider.Export(document, output);
            }
            //using Aspose.HTML to convert html to markdown
            var documentap = new HTMLDocument(outPath + "\\hide.html");
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            documentap.Save(Path.GetFullPath(outPath + "\\MarkDownFile.md"), HTMLSaveFormat.Markdown);

        }
    }
}


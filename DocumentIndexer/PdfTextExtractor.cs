using BitMiracle.Docotic.Pdf;

namespace PdfTextExtractor
{
    public class PdfTextExtractor : ITextExtractor
    {
        private static string GetTextFromPdf(string filePath)
        {
            using (var pdf = new PdfDocument(filePath))
            {
                var result = pdf.GetText();
                return result;
            }
        }
        
        private static string GetPdfFileText(string pdfFilePath)
        {
            var fileContentText = GetTextFromPdf(pdfFilePath);
            return fileContentText;
        }

        public string GetFileContents(string filePath)
        {
            return GetPdfFileText(filePath);
        }

        public FileType FileType { get; set; } = FileType.Pdf;
    }
}
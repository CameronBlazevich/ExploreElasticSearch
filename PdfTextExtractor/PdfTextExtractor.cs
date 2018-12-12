using BitMiracle.Docotic.Pdf;

namespace PdfTextExtractor
{
    public static class PdfTextExtractor
    {
        private static string GetTextFromPdf(string filePath)
        {
            using (var pdf = new PdfDocument(filePath))
            {
                var result = pdf.GetText();
                return result;
            }
        }
        
        public static string GetPdfFileText(string pdfFilePath)
        {
            var fileContentText = GetTextFromPdf(pdfFilePath);
            return fileContentText;
        }
    }
}
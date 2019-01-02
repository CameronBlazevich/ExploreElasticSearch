namespace PdfTextExtractor
{
    public interface ITextExtractorFactory
    {
        ITextExtractor GetTextExtractor(string filePath);
    }
}
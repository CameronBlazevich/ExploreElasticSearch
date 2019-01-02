namespace PdfTextExtractor
{
    public interface ITextExtractor
    {
        string GetFileContents(string filePath);
        FileType FileType { get; set; }
    }
}
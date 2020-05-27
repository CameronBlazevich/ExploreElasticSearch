
using System.IO;

namespace PdfTextExtractor
{
    public class TextFileTextExtractor : ITextExtractor
    {
        public string GetFileContents(string filePath)
        {
            var contents = File.ReadAllText(filePath);
            return contents;
        }

        public FileType FileType { get; set; } = FileType.Text;
    }
}
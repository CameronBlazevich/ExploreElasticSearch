using System.IO;

namespace PdfTextExtractor
{
    public static class FileTypeResolver
    {
        public static FileType GetFileType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();

            switch (extension)
            {
                case ".pdf":
                    return FileType.Pdf;
                case ".txt": 
                    return FileType.Text;
                default:
                    return FileType.Unknown;
            }

        }
    }
}
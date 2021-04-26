using System.Collections.Generic;
using System.IO;

namespace PdfTextExtractor
{
    public class FileHelper
    {
        public IEnumerable<string> GetFilePaths(string directoryPath)
        {
            var filePaths = Directory.GetFiles(directoryPath);

            return filePaths;
        }

        public IEnumerable<string> GetChildDirectories(string parentDirectoryPath)
        {
            var directoryPaths = Directory.GetDirectories(parentDirectoryPath);
            return directoryPaths;
        }

        public string GetFileName(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            return fileName;
        }
    }
}
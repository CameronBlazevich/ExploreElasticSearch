using System.Collections.Generic;
using System.IO;

namespace PdfTextExtractor
{
    public class DocumentIndexerHelper
    {
        public FileInfo[] GetFiles()
        {
            var directoryInfo =
                new DirectoryInfo(@"C:\Users\cblazevich\RiderProjects\ExploreElasticSearch\Transcripts");
            return directoryInfo.GetFiles("*.pdf");
        }

        public IEnumerable<string> GetFilePaths(string directoryPath)
        {
            var filePaths = Directory.GetFiles(directoryPath,
                "*.pdf", SearchOption.TopDirectoryOnly);

            return filePaths;
        }
    }
}
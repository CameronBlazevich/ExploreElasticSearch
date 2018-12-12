using System;
using System.Collections.Generic;
using System.IO;
using ExploreElasticSearch.Core.Models;

namespace PdfTextExtractor
{
    public class TimFerrissPodcastIndexer: IDocumentIndexer
    {
        //const string timFerrissDirectoryPath = @"C:\Users\cblazevich\RiderProjects\ExploreElasticSearch\Transcripts\";
        
        public Document GetDocumentToIndex(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var fileNameParts = fileName.Split('-');
            var podcastNumber = fileNameParts[0];

            var documentToIndex = CreateDocumentToIndex(filePath);
            documentToIndex.Id = int.Parse(podcastNumber);
            return documentToIndex;
        }

        private static Document CreateDocumentToIndex(string filePath)
        {
            var fileContents = GetFileContents(filePath);
            var sanitizedFileContents = SanitizeFileContents(fileContents);
            var title = GetTitle(sanitizedFileContents);
            
            var documentToIndex = new Document
            {
                Contents = fileContents,
                PostDate = DateTime.Now,
                Title = title,
                Author = "Tim Ferriss",
            };

            return documentToIndex;
        }

        private static string GetFileContents(string filePath)
        {
            var fileContents = PdfTextExtractor.GetPdfFileText(filePath);
            return fileContents;
        }

        private static string SanitizeFileContents(string fileContents)
        {
            var sanitizedFileContents = RemoveNewLines(fileContents);
            return sanitizedFileContents;
        }
        
        private static string RemoveNewLines(string fileContents)
        {
            fileContents = fileContents.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
            return fileContents;
        }
        
        private static string GetTitle(string fileContents)
        {
            var title = fileContents.Substring(
                fileContents.IndexOf(":", StringComparison.InvariantCultureIgnoreCase) + 1,
                fileContents.IndexOf("show notes", StringComparison.InvariantCultureIgnoreCase) -
                (fileContents.IndexOf(":", StringComparison.InvariantCultureIgnoreCase) + 1)).Trim();
            return title;
        }
         
        private static bool IsTimFerriss(IReadOnlyList<string> fileNameParts, string fileContents)
        {
            var numberOfFileNameParts = fileNameParts.Count;
            for (var i = 0; i < numberOfFileNameParts; i++)
            {
                if (int.TryParse(fileNameParts[i], out _))
                {
                    i++;
                    continue;
                }

                if (fileNameParts[i].Equals("tim", StringComparison.InvariantCultureIgnoreCase) &&
                    i < numberOfFileNameParts - 1 &&
                    fileNameParts[i + 1].Equals("ferriss", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
using System;
using System.Linq;
using ElasticSearchClient;
using ExploreElasticSearch.Core.Models;


namespace PdfTextExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            var documentIndexer = new TimFerrissPodcastIndexer();
            var documentIndexerHelper = new DocumentIndexerHelper();

            const string timFerrissDirectoryPath =
                @"C:\Users\cblazevich\RiderProjects\ExploreElasticSearch\Transcripts\";

            IndexDocuments(timFerrissDirectoryPath, documentIndexer, documentIndexerHelper);

            Console.ReadLine();
        }


        private static void IndexDocuments(string directoryPath, IDocumentIndexer documentIndexer,
            DocumentIndexerHelper documentIndexerHelper)
        {
            var filePaths = documentIndexerHelper.GetFilePaths(directoryPath);
            foreach (var filePath in filePaths)
            {
                var documentToIndex = documentIndexer.GetDocumentToIndex(filePath);

                PrintDocumentInfo(documentToIndex);
                IndexDocument(documentToIndex);
            }
        }

        private static void PrintDocumentInfo(Document documentToIndex)
        {
            Console.WriteLine($"Id: {documentToIndex.Id}");
            Console.WriteLine($"Title: {documentToIndex.Title}");
            if (documentToIndex.Participants.Any())
            {
                Console.WriteLine($"Participants: {string.Join(", ", documentToIndex.Participants)}");
            }
        }

        private static void IndexDocument(Document documentToIndex)
        {
            var elasticClient = new Client();
            elasticClient.IndexDocument(documentToIndex);
        }
    }
}
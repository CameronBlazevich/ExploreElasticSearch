using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElasticSearchClient;
using ExploreElasticSearch.Core.Models;


namespace PdfTextExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteIndex();
//            const string timFerrissDirectoryPath =
//                @"C:\Users\cblazevich\RiderProjects\ExploreElasticSearch\Transcripts\TimFerriss";
//
//            IndexDocuments(timFerrissDirectoryPath);

            const string dirPath =
                @"C:\Users\cblazevich\RiderProjects\ExploreElasticSearch\Transcripts\";

            IndexDocumentsMultiLevelDirectory(dirPath);

            Console.ReadLine();
        }

        private static List<string> FoldersToSkip = new List<string> { "TimFerriss", "RhondaPatrick"};

        private static void IndexDocumentsMultiLevelDirectory(string directoryPath)
        {
            var documentIndexerFactory = new DocumentIndexerFactory();
            var fileHelper = new FileHelper();
            var childDirectoryPaths = fileHelper.GetChildDirectories(directoryPath);
            foreach (var childDirectoryPath in childDirectoryPaths)
            {
                
                //delete this
                var dirName = new DirectoryInfo(childDirectoryPath).Name;
                if (FoldersToSkip.Contains(dirName))
                {
                    continue;
                }
                
                
                var documentIndexer = documentIndexerFactory.GetDocumentIndexer(childDirectoryPath);
                IndexDocuments(childDirectoryPath, documentIndexer, fileHelper);
            }
        }

        private static void IndexDocuments(string directoryPath, IDocumentIndexer documentIndexer,
            FileHelper fileHelper)
        {
            var filePaths = fileHelper.GetFilePaths(directoryPath);
            foreach (var filePath in filePaths)
            {
                var documentToIndex = documentIndexer.GetDocumentToIndex(filePath);

                PrintDocumentInfo(documentToIndex);
                IndexDocument(documentToIndex);
            }
        }


        private static void IndexDocuments(string directoryPath)
        {
            var documentIndexerFactory = new DocumentIndexerFactory();
            var documentIndexer = documentIndexerFactory.GetDocumentIndexer(directoryPath);
            var documentIndexerHelper = new FileHelper();

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

        private static void DeleteIndex()
        {
            var elasticClient = new Client();
            elasticClient.DeleteIndex();
        }
    }
}

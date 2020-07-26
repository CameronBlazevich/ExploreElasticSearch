using System;
using System.Linq;
using ElasticSearchClient;
using ExploreElasticSearch.Core.Models;
using Microsoft.Extensions.Configuration;


namespace PdfTextExtractor
{
    class Program
    {
        private static IConfiguration _configuration;

        static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                // .AddCommandLine(args) //install Microsoft.Extensions.Configuration.CommandLine
                .Build();

            // const string dirPath =
            //     @"C:\Users\cblazevich\RiderProjects\ExploreElasticSearch\Transcripts\";
            const string dirPath =
                @"C:\Users\Cameron\Documents\GitHub\exploratory\ExploreElasticSearch\Transcripts";

            IndexDocumentsMultiLevelDirectory(dirPath);
            //
            // Console.ReadLine();
        }

        private static void IndexDocumentsMultiLevelDirectory(string directoryPath)
        {
            var documentIndexerFactory = new DocumentIndexerFactory();
            var fileHelper = new FileHelper();
            var childDirectoryPaths = fileHelper.GetChildDirectories(directoryPath);
            foreach (var childDirectoryPath in childDirectoryPaths)
            {
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
            var elasticClient = new Client(
                new ClientConstructorArgs(
                    _configuration["ElasticSearch:Url"],
                    _configuration["ElasticSearch:Username"],
                    _configuration["ElasticSearch:Password"]));
            elasticClient.IndexDocument(documentToIndex);
        }

        private static void DeleteIndex()
        {
            var elasticClient = new Client(
                new ClientConstructorArgs(
                    _configuration["ElasticSearch:Url"],
                    _configuration["ElasticSearch:Username"],
                    _configuration["ElasticSearch:Password"]));
            elasticClient.DeleteIndex();
        }
    }
}
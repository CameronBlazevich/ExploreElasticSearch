using System;
using System.Globalization;
using ExploreElasticSearch.Core.Common;
using ExploreElasticSearch.Core.Models;

namespace PdfTextExtractor.Indexers
{
    public class RhondaPatrickPodcastIndexer : IDocumentIndexer
    {
        private readonly ITextExtractorFactory _textExtractorFactory;
        private readonly FileHelper _fileHelper;

        public RhondaPatrickPodcastIndexer(ITextExtractorFactory textExtractorFactory)
        {
            _textExtractorFactory = textExtractorFactory;
            _fileHelper = new FileHelper();
        }

        public Document GetDocumentToIndex(string filePath)
        {
            var documentToIndex = CreateDocumentToIndex(filePath);
            return documentToIndex;
        }

        private Document CreateDocumentToIndex(string filePath)
        {
            var fileName = _fileHelper.GetFileName(filePath);
            var fileNameParts = fileName.Split('-');
            var documentId = fileNameParts[0];

            var fileContents = GetFileContents(filePath);
            //var sanitizedFileContents = SanitizeFileContents(fileContents);
            var title = GetTitle(filePath);

            var documentToIndex = new Document
            {
                Contents = fileContents,
                PostDate = DateTime.Now,
                Title = title,
                Author = "Rhonda Patrick",
                Id = $"patrick-{documentId}",
                MetaTitle = "Found My Fitness",
            };
              
            var rhondaPatrick = new Participant(Honorific.Dr.ToString(), "Rhonda", "Patrick");
            documentToIndex.Participants.Add(rhondaPatrick);
            
            var participant = GetParticipant(filePath);
            documentToIndex.Participants.Add(participant);

            return documentToIndex;
        }

        private string GetTitle(string filePath)
        {
            var fileName = _fileHelper.GetFileName(filePath);

            var rawTitle = GetRawTitleWithParticipant(fileName);

            var titleWithSpaces = rawTitle.Replace("-", " ");

            var textInfo = new CultureInfo("en-US", false).TextInfo;
            var title = textInfo.ToTitleCase(titleWithSpaces);

            return title;
        }

        private Participant GetParticipant(string filePath)
        {
            var fileName = _fileHelper.GetFileName(filePath);
            var guestsName = fileName.Substring(
                fileName.IndexOf("-", StringComparison.InvariantCultureIgnoreCase) + 1,
                fileName.IndexOf("-on-", StringComparison.InvariantCultureIgnoreCase) -
                (fileName.IndexOf("-", StringComparison.InvariantCultureIgnoreCase) + 1));

            var charToSplitOn = '-';
            return NameParser.ParseParticipantName(guestsName, charToSplitOn);
        }

        

        private string GetRawTitleWithParticipant(string fileName)
        {
            var rawTitle = fileName.Substring(
                fileName.IndexOf("-", StringComparison.InvariantCultureIgnoreCase) + 1,
                fileName.IndexOf(".", StringComparison.InvariantCultureIgnoreCase) -
                (fileName.IndexOf("-", StringComparison.InvariantCultureIgnoreCase) + 1));

            return rawTitle;
        }

        private string GetRawTitleWithoutParticipant(string fileName)
        {
            var rawTitle = fileName.Substring(
                fileName.IndexOf("-on-", StringComparison.InvariantCultureIgnoreCase) + 4,
                fileName.IndexOf(".", StringComparison.InvariantCultureIgnoreCase) -
                (fileName.IndexOf("-on-", StringComparison.InvariantCultureIgnoreCase) + 4));

            return rawTitle;
        }

        private string GetFileContents(string filePath)
        {
            var textExtractor = _textExtractorFactory.GetTextExtractor(filePath);
            var fileContents = textExtractor.GetFileContents(filePath);
            return fileContents;
        }
    }
}

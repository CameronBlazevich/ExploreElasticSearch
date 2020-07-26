using System;
using System.Collections.Generic;
using ExploreElasticSearch.Core.Models;

namespace PdfTextExtractor.Indexers
{
    public class TimFerrissPodcastIndexer : IDocumentIndexer
    {
        private readonly ITextExtractorFactory _textExtractorFactory;
        private readonly FileHelper _fileHelper;

        public TimFerrissPodcastIndexer(ITextExtractorFactory textExtractorFactory)
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
            var podcastNumber = fileNameParts[0];

            var fileContents = GetFileContents(filePath);
            var sanitizedFileContents = SanitizeFileContents(fileContents);
            var title = GetTitle(sanitizedFileContents);

            var participants = GetParticipants(filePath, title);
            foreach (var participant in participants)
            {
                fileContents = fileContents.Replace($"{participant.FullName}:",
                    $"</br></br><strong>{participant.FullName}:</strong>", StringComparison.OrdinalIgnoreCase);
            }
            
            var documentToIndex = new Document
            {
                Contents = fileContents,
                PostDate = DateTime.Now,
                Title = title,
                Author = "Tim Ferriss",
                Id = $"ferriss-{podcastNumber}",
                MetaTitle = $"The Tim Ferriss Show: Episode {podcastNumber}",
                Participants = participants,
            };

            return documentToIndex;
        }

        private string GetFileContents(string filePath)
        {
            var textExtractor = _textExtractorFactory.GetTextExtractor(filePath);
            var fileContents = textExtractor.GetFileContents(filePath);
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

        private List<Participant> GetParticipants(string filePath, string title)
        {
            var participants = new List<Participant>();
            var fileName = _fileHelper.GetFileName(filePath);
            var fileNameParts = fileName.Split('-');
            if (!IsTimFerriss(fileNameParts))
            {
                var guests = title.Split(',');
                foreach (var guest in guests)
                {
                    participants.Add(new Participant(guest.Trim()));
                }
            }

            participants.Add(new Participant(string.Empty, "Tim", "Ferriss"));

            return participants;
        }

        private static string GetTitle(string fileContents)
        {
            var title = fileContents.Substring(
                fileContents.IndexOf(":", StringComparison.InvariantCultureIgnoreCase) + 1,
                fileContents.IndexOf("show notes", StringComparison.InvariantCultureIgnoreCase) -
                (fileContents.IndexOf(":", StringComparison.InvariantCultureIgnoreCase) + 1)).Trim();
            return title;
        }

        private static bool IsTimFerriss(IReadOnlyList<string> fileNameParts)
        {
            var numberOfFileNameParts = fileNameParts.Count;
            for (var i = 0; i < numberOfFileNameParts; i++)
            {
                if (!int.TryParse(fileNameParts[i], out _))
                {
                    return fileNameParts[i].Equals("tim", StringComparison.InvariantCultureIgnoreCase) &&
                           i < numberOfFileNameParts - 1 &&
                           fileNameParts[i + 1].Equals("ferriss", StringComparison.InvariantCultureIgnoreCase);
                }
            }

            return false;
        }
    }
}
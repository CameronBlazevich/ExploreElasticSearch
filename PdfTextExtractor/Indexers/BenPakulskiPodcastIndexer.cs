using System;
using System.Collections.Generic;
using System.Linq;
using ExploreElasticSearch.Core.Models;

namespace PdfTextExtractor.Indexers
{
    public class BenPakulskiPodcastIndexer : IDocumentIndexer
    {
        private FileHelper _fileHelper;
        private ITextExtractorFactory _textExtractorFactory;
        
        public BenPakulskiPodcastIndexer(ITextExtractorFactory textExtractorFactory)
        {
            _fileHelper = new FileHelper();
            _textExtractorFactory = textExtractorFactory;
        }

        public Document GetDocumentToIndex(string filePath)
        {
            var fileName = _fileHelper.GetFileName(filePath);
            var fileNameParts = fileName.Split('-');
            var podcastNumber = fileNameParts.First();

            var fileNameWithoutExtension = fileNameParts.Last().Split('.').First();
            var title = fileNameWithoutExtension;

            var fileContents = GetFileContents(filePath);

            var participants = GetParticipants(fileContents);
            foreach (var speaker in participants)
            {
                fileContents = fileContents.Replace($"{speaker.FullName}:", $"</br></br><strong>{speaker.FullName}:</strong>", StringComparison.OrdinalIgnoreCase);
            }
            var documentToIndex = new Document
            {
                Contents = fileContents, 
                Author = "Ben Pakulski",
                PostDate = DateTime.Now,
                Title = title,
                Id = $"pakulski-{podcastNumber}",
                MetaTitle = "Muscle Intelligence",
                Participants = participants,
            };

            return documentToIndex;
        }

        private List<Participant> GetParticipants(string fileContents)
        {
            var participants = new List<Participant>();
            var lines = fileContents.Split("\n");

            foreach (var line in lines)
            {
                var splitLine = line.Split(':');
                if (!splitLine.Any() || splitLine.Length == 1)
                {
                    continue;
                }

                var speaker = splitLine.FirstOrDefault();
                if (string.IsNullOrEmpty(speaker))
                {
                    continue;
                }
                
                var guest = NameParser.ParseParticipantName(speaker, ' ');
                if (!participants.Contains(guest))
                {
                    participants.Add(guest);
                }
            }

            return participants;
        }

        private string GetFileContents(string filePath)
        {
            var textExtractor = _textExtractorFactory.GetTextExtractor(filePath);
            var fileContents = textExtractor.GetFileContents(filePath);
            return fileContents;
        }
    }
}
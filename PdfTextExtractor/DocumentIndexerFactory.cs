using System;
using System.Collections.Generic;
using System.IO;
using PdfTextExtractor.Indexers;

namespace PdfTextExtractor
{
    public class DocumentIndexerFactory
    {
        private Dictionary<DocumentIndexerType, IDocumentIndexer> _documentIndexers = new Dictionary<DocumentIndexerType, IDocumentIndexer>
        {
            {DocumentIndexerType.TimFerriss, new TimFerrissPodcastIndexer(new TextExtractorFactory())},
            {DocumentIndexerType.RhondaPatrick, new RhondaPatrickPodcastIndexer(new TextExtractorFactory())},
            {DocumentIndexerType.BenPakulski, new BenPakulskiPodcastIndexer(new TextExtractorFactory())},
        };
        
        public IDocumentIndexer GetDocumentIndexer(string filePath)
        {
            var dirName = new DirectoryInfo(filePath).Name;

            if (Enum.TryParse(dirName, true, out DocumentIndexerType documentIndexerType))
            {
                if (_documentIndexers.TryGetValue(documentIndexerType, out var indexer))
                {
                    return indexer;
                }
            }

            throw new Exception("No matching indexer found.");
        }
    }
}
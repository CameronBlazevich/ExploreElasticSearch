using ExploreElasticSearch.Core.Models;

namespace PdfTextExtractor
{
    public interface IDocumentIndexer
    {
        Document GetDocumentToIndex(string filePath);
    }
}
using ExploreElasticSearch.Documents.Models;

namespace ExploreElasticSearch.Documents
{
    public interface IDocumentsService
    {
        SearchDocumentsResponse Search(string searchPhrase);
    }
}
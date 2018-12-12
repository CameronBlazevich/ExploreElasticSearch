using ExploreElasticSearch.Core.Models;
using Nest;

namespace ElasticSearchClient
{
    public interface IClient
    {
        ISearchResponse<Document> Search(string searchPhrase);
    }
}
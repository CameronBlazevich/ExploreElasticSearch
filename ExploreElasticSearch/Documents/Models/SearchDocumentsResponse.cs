using System.Collections.Generic;
using ExploreElasticSearch.Core.Models;
using Nest;

namespace ExploreElasticSearch.Documents.Models
{
    public class SearchDocumentsResponse
    {
        public SearchDocumentsResponse()
        {
            SearchResults = new List<SearchResult>();
        }

        public List<SearchResult> SearchResults { get; set; }
    }
}
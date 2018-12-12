
using System.Collections.Generic;
using System.Linq;
using ElasticSearchClient;
using ExploreElasticSearch.Core.Models;
using ExploreElasticSearch.Documents.Models;
using Nest;
using Highlight = ExploreElasticSearch.Documents.Models.Highlight;


namespace ExploreElasticSearch.Documents
{
    public class DocumentsService : IDocumentsService
    {
        private IClient _client;

        public DocumentsService(IClient client)
        {
            _client = client;
        }
        
        public SearchDocumentsResponse Search(string searchPhrase)
        {
            var result =  _client.Search(searchPhrase);

            var searchResponse = new SearchDocumentsResponse();

            foreach (var hit in result.Hits)
            {
                var searchResult = new SearchResult();
                searchResult.ParentArticle.RelevancyScore = hit.Score;
                
                searchResult.ParentArticle.Title = $"Episode {hit.Source.Id}: {hit.Source.Title}";
                searchResult.ParentArticle.Author = hit.Source.Author;
               
                var highlights = hit.Highlights.Values;

                foreach (var highlight in highlights)
                {
                    var snippets = highlight.Highlights.ToList();
                    foreach (var snippet in snippets)
                    {
                        searchResult.Highlights.Add(new Highlight{Snippet = snippet});
                    }
                }
                
                searchResponse.SearchResults.Add(searchResult);
            }

            return searchResponse;
        }
    }
}
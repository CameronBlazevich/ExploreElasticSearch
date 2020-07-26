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
            var result = _client.Search(searchPhrase);

            var searchResponse = new SearchDocumentsResponse();

            foreach (var hit in result.Hits)
            {
                var searchResult = new SearchResult
                {
                    ParentArticle =
                    {
                        RelevancyScore = hit.Score,
                        Title = hit.Source.Title,
                        Author = hit.Source.Author,
                        MetaTitle = hit.Source.MetaTitle,
                        Participants = hit.Source.Participants
                    }
                };


                var highlights = hit.Highlight;

                foreach (var highlight in highlights)
                {
                    var snippets = highlight.Value.ToList();
                    foreach (var snippet in snippets)
                    {
                        searchResult.Highlights.Add(new Highlight {Snippet = snippet});
                    }
                }

                searchResponse.SearchResults.Add(searchResult);
            }

            return searchResponse;
        }
    }
}
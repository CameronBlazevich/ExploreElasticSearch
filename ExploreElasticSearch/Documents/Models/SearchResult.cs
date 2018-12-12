using System.Collections.Generic;

namespace ExploreElasticSearch.Documents.Models
{
    public class SearchResult
    {
        public SearchResult()
        {
            Highlights = new List<Highlight>();
            ParentArticle = new Article();
        }

        public List<Highlight> Highlights { get; set; }
        public Article ParentArticle { get; set; }
    }
}
using System.Collections.Generic;
using ExploreElasticSearch.Core.Models;

namespace ExploreElasticSearch.Documents.Models
{
    public class Article
    {
        public Article()
        {
            Participants = new List<Participant>();
        }
        public string Title { get; set; }
        public string Text { get; set; }
        public double? RelevancyScore { get; set; }
        public List<Participant> Participants { get; set; }
        public string Author { get; set; }
    }
}
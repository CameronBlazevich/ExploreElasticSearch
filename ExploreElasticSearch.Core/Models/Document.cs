using System;
using System.Collections.Generic;

namespace ExploreElasticSearch.Core.Models
{
    public class Document
    {
        public Document()
        {
            Participants = new List<Participant>();
        }
        public int Id { get; set; }
        public string Author { get; set; }
        public DateTime PostDate { get; set; }
        public string Contents { get; set; }
        public List<Participant> Participants { get; set; }
        public string Title { get; set; }
    }
}

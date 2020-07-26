using System;
using System.Collections.Generic;
using System.Linq;
using ExploreElasticSearch.Core.Models;
using Nest;

namespace ElasticSearchClient
{
    public class Client : IClient
    {
        private readonly IElasticClient _elasticClient;

        public Client(ClientConstructorArgs constructorArgs)
        {
            var node = new Uri(constructorArgs.Url);
            var settings = new ConnectionSettings(node)
                .BasicAuthentication(constructorArgs.Username, constructorArgs.Password)
                .DefaultIndex(ExploreElasticSearch.Core.Common.IndexNames.PodcastsAndInterviews);
            _elasticClient = new ElasticClient(settings);
        }
        

        public ISearchResponse<Document> Search(string searchPhrase)
        {
            var initialResponse = _elasticClient.Search<Document>(s => s
                .From(0)
                .Size(100)
                .MinScore(0.5)
                .Query(q => q
                    .MatchPhrase(mq => mq
                        .Field(f => f.Contents)
                        .Query(searchPhrase)

                    )
                ));

            
            
                
            var searchResponse = _elasticClient.Search<Document>(s => s
                .From(0)
                .Size(200)
                .MinScore(0.5)
                .Query(q => q
                    .MatchPhrase(mq => mq
                        .Field(f => f.Contents)
                        .Query(searchPhrase)
                    )
                )
                .Highlight(h => h
                    .Fields(fs => fs
                            .Field(f => f.Contents)
                            .PreTags("<span>")
                            .PostTags("</span>")
                            .HighlightQuery(hq => hq
                                .Match(mp => mp
                                    .Field(f => f.Contents)
                                    .Query(searchPhrase)
                                )
                            )
                    )
                    
                    .FragmentSize(1000)
                    .Order(HighlighterOrder.Score)
                )
            );

            return searchResponse;
        }
        
        public void IndexDocument(Document documentToIndex)
        {
            var response = _elasticClient.Index(documentToIndex, idx => idx.Index(ExploreElasticSearch.Core.Common.IndexNames.PodcastsAndInterviews));
        }

        public void DeleteIndex()
        {
            // var response = _elasticClient.DeleteIndex(ExploreElasticSearch.Core.Common.IndexNames.PodcastsAndInterviews);
        }
    }
}


//var searchResponse = _elasticClient.Search<Document>(s => s
//.From(0)
//.Size(200)
//.MinScore(0.5)
//.Query(q => q
//.Fuzzy(f => f.Field(p => p.Message)
//.Fuzziness(Fuzziness.Auto)
//.Value(searchPhrase)
//.Boost(1.1)
//.MaxExpansions(100)
//.PrefixLength(3)
//.Transpositions())
//)
//.Highlight(h => h
//.PreTags("<span>")
//.PostTags("</span>")
//.Fields(fs =>
//fs.Field(f => f.Message).HighlightQuery(hq =>
//hq.Fuzzy(f => f.Field(p => p.Message)
//.Fuzziness(Fuzziness.Auto)
//.Value(searchPhrase)
//.Boost(1.1)
//.MaxExpansions(100)
//.PrefixLength(3)
//.Transpositions())
//                                    
////.Type(HighlighterType.Unified)
//)
//.FragmentSize(1500)
//.Order(HighlighterOrder.Score)
//)
//));
//
//return searchResponse;
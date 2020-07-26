using System;
using ExploreElasticSearch.Core.Common;
using ExploreElasticSearch.Core.Models;

namespace PdfTextExtractor.Indexers
{
    public static class NameParser
    {
        public static Participant ParseParticipantName(string guestsName, char charToSplitOn)
        {
            var guestsNameParts = guestsName.Split(charToSplitOn);
            var numberOfNameParts = guestsNameParts.Length;

            Participant participant;
            if (Enum.TryParse(guestsNameParts[0], true, out Honorific honorific))
            {
                if (numberOfNameParts == 4)
                {
                    participant = new Participant(honorific.ToString(), guestsNameParts[1], guestsNameParts[2],
                        guestsNameParts[3]);
                }
                else
                {
                    participant = new Participant(honorific.ToString(), guestsNameParts[1], guestsNameParts[2]);
                }
            }
            else
            {
                switch (numberOfNameParts)
                {
                    case 3:
                        participant = new Participant(null, guestsNameParts[0], guestsNameParts[1], guestsNameParts[2]);
                        break;
                    case 2:
                        participant = new Participant(null, guestsNameParts[0], guestsNameParts[1]);
                        break;
                    default:
                        participant = new Participant(null, guestsNameParts[0], null);
                        break;
                }
            }

            return participant;
        }
    }
}
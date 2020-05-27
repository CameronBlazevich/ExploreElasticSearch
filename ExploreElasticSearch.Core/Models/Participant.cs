using System;
using System.Globalization;

namespace ExploreElasticSearch.Core.Models
{
    public class Participant
    {
        public Participant(string honorific, string firstName, string lastName, string qualification = "")
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            
            Honorific = honorific;
            FirstName = string.IsNullOrWhiteSpace(firstName) ? string.Empty : textInfo.ToTitleCase(firstName);
            LastName = string.IsNullOrWhiteSpace(lastName) ? string.Empty : textInfo.ToTitleCase(lastName);
            Qualification = string.IsNullOrWhiteSpace(qualification) ? string.Empty : textInfo.ToTitleCase(qualification);
            FullName = string.IsNullOrWhiteSpace(Honorific)
                ? $"{FirstName} {LastName} {Qualification}".Trim()
                : $"{Honorific}. {FirstName} {LastName} {Qualification}".Trim();
        }

        public Participant(string fullName)
        {
            FullName = fullName;
        }

        public Participant()
        {
            
        }

        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Honorific { get; set; }
        private string Qualification { get; set; }
        public string FullName { get; set; }

        public override string ToString()
        {
            return FullName;
        }

        public override bool Equals(object obj)
        {
            var participant1 = obj as Participant;

            if (participant1 == null)
            {
                return false;
            }

            var honorificsMatch = Honorific == participant1.Honorific;
            var firstNamesMatch = FirstName == participant1.FirstName;
            var lastNamesMatch = LastName == participant1.LastName;
            var qualificationsMatch = Qualification == participant1.Qualification;
            var fullNamesMatch = FullName == participant1.FullName;

            return honorificsMatch && firstNamesMatch && lastNamesMatch && qualificationsMatch && fullNamesMatch;
        }
    }
}

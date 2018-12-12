namespace ExploreElasticSearch.Core.Models
{
    public class Participant
    {
        public Participant(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => FirstName + LastName;

        public override string ToString()
        {
            return FullName;
        }
    }
}
namespace Library.LearningManagement.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Dictionary<int, double> Grades { get; set; } //A list of assignments, each have ID (key into dictionary, double is grade)

        public PersonClassification Classification { get; set; }

        public Person()
        {
            Name = string.Empty;    //set empty string instead of null
            Grades = new Dictionary<int, double>();
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Classification}";
        }

    }

    public enum PersonClassification
    {
        Freshman, Sophomore, Junior, Senior
    }
}
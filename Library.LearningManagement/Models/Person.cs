namespace Library.LearningManagement.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Dictionary<int, double> Grades { get; set; } //A list of assignments, each have ID (key into dictionary, double is grade)

        public char Classification { get; set; }

        public Person()
        {
            Name = string.Empty;    //set empty string instead of null
            Grades = new Dictionary<int, double>();
        }

    }
}
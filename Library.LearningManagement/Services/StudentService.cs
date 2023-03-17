using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Services
{
    public class StudentService
    {
        private List<Student> studentList = new List<Student>();

        public void Add(Student student)
        {
            studentList.Add(student);
        }

        public List<Student> Students
        {
            get
            {
                return studentList;
            }
        }

        public IEnumerable<Student> Search(string query)
        {
            return studentList.Where(s => s.Name.ToUpper().Contains(query.ToUpper()));
        }
    }
}

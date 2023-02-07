﻿using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.LearningMangement.Helpers
{
    internal class StudentHelper
    {
        private StudentService studentService = new StudentService();
        public void CreateStudentRecord()
        {
            Console.WriteLine("What is the ID of the student?");
            var id = Console.ReadLine();
            Console.WriteLine("What is the name of the student?");
            var name = Console.ReadLine();
            Console.WriteLine("What is the classification of the student? [(F)reshman, S(O)phomore, (J)unior, (S)enior]");
            var classification = Console.ReadLine() ?? string.Empty;
            PersonClassification classEnum = PersonClassification.Freshman;

            if (classification.Equals("O", StringComparison.InvariantCultureIgnoreCase))
            {
                classEnum = PersonClassification.Sophomore;
            }
            else if (classification.Equals("J", StringComparison.InvariantCultureIgnoreCase))
            {
                classEnum = PersonClassification.Junior;
            }
            else if (classification.Equals("S", StringComparison.InvariantCultureIgnoreCase))
            {
                classEnum = PersonClassification.Senior;
            }

            var student = new Person
            {
                Id = int.Parse(id ?? "0"),  //If id is empty, make 0
                Name = name ?? string.Empty,
                Classification = classEnum  //If the classification variable is Null or Empty
                                            //make it a 'F' for freshman.

            };

            studentService.Add(student);

            studentService.studentList.ForEach(Console.WriteLine);
        }
    }
}

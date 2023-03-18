﻿using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace App.LearningMangement.Helpers
{
    internal class StudentHelper
    {
        private StudentService studentService;
        private CourseService courseService;

        public StudentHelper(StudentService service)
        {
            studentService = service;
            courseService = CourseService.Current;
        }

        public void CreateStudentRecord(Person? selectedStudent = null)
        {
            bool isCreate = false;
            if (selectedStudent == null)
            {
                isCreate = true;
                Console.WriteLine("What type of person would you like to add?");
                Console.WriteLine("(S)tudent");
                Console.WriteLine("(T)eaching Assistant");
                Console.WriteLine("(I)nstructor");
                var choice = Console.ReadLine() ?? string.Empty;
                if(string.IsNullOrEmpty(choice)) 
                {
                    return;
                }
                if (choice.Equals("S", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectedStudent = new Student();
                }
                else if (choice.Equals("T", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectedStudent = new TeachingAssistant();
                }
                else if (choice.Equals("I", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectedStudent = new Instructor();
                }
                
            }


            Console.WriteLine("What is the ID of the student?");
            var id = Console.ReadLine();
            Console.WriteLine("What is the name of the student?");
            var name = Console.ReadLine();
            if (selectedStudent is Student)
            {
                Console.WriteLine("What is the classification of the student?");
                Console.WriteLine("[F] Freshman " +
                    "[O] Sophomore " +
                    "[J] Junior " +
                    "[S] Senior ");

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
                var studentRecord = selectedStudent as Student;
                if (studentRecord != null)
                {
                    studentRecord.Classification = classEnum;
                    studentRecord.Id = int.Parse(id ?? "0");
                    studentRecord.Name = name ?? string.Empty;


                    if (isCreate)
                    {
                        studentService.Add(selectedStudent);
                    }
                }

            }
            else
            {
                if (selectedStudent != null)
                {
                    selectedStudent.Id = int.Parse(id ?? "0");
                    selectedStudent.Name = name ?? string.Empty;

                    if (isCreate)
                    {
                        studentService.Add(selectedStudent);
                    }
                }
            }
        }

        public void UpdateStudentRecord()
        {
            Console.WriteLine("Select a person to update");
            ListStudents();

            var selectionStr = Console.ReadLine();

            if (int.TryParse(selectionStr, out int selectionInt))
            {
                var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectionInt);
                if (selectionStr != null)
                {
                    CreateStudentRecord(selectedStudent);
                }
            }
        }

        public void ListStudents()
        {
            studentService.Students.ForEach(Console.WriteLine);
        }

        public void ListAndInfo()
        {
            studentService.Students.ForEach(Console.WriteLine);

            Console.WriteLine("Which student would you like to select?");
            var selectionStr = Console.ReadLine();
            var selectionInt = int.Parse(selectionStr ?? "0");

            Console.WriteLine("Courses Student is Enrolled in:");
            courseService.Courses.Where(c => c.Roster.Any(s => s.Id == selectionInt)).ToList().ForEach(Console.WriteLine);
        }

        public void SearchStudents()
        {
            Console.WriteLine("Enter a query: ");
            var query = Console.ReadLine() ?? string.Empty;

            studentService.Search(query).ToList().ForEach(Console.WriteLine);
            var selectionStr = Console.ReadLine();
            var selectionInt = int.Parse(selectionStr ?? "0");

            Console.WriteLine("Courses Student is Enrolled in:");

            courseService.Courses.Where(c => c.Roster.Any(s => s.Id == selectionInt)).ToList().ForEach(Console.WriteLine);
        }
    }
}

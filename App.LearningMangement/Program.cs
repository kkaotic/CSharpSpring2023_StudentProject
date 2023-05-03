﻿using System;
using App.LearningMangement.Helpers;
using Library.LearningManagement.Models;
using Library.LearningManagement.Services;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var studentService = new StudentService();
            var studentHelper = new StudentHelper(studentService);
            var courseHelper = new CourseHelper(studentService);

            bool cont = true;

            Console.WriteLine("Welcome to the Learning Management System v0.1!");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            while (cont)
            {
                Console.WriteLine("[1] Maintain People");
                Console.WriteLine("[2] Maintain Courses");
                Console.WriteLine("[3] Exit");                              //sys
                var input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                {
                    if(result == 1)
                    {
                        ShowStudentMenu(studentHelper);
                    }else if(result == 2)
                    {
                        ShowCourseMenu(courseHelper);
                    }
                    else if (result == 3)
                    {
                        Console.WriteLine("Thank you for using the Learning Management System v0.1!");
                        cont = false;
                    }

                }
            }

        }

        static void ShowStudentMenu(StudentHelper studentHelper)
        {
            Console.WriteLine("Choose an action:");
            Console.WriteLine("[1] Add a new Person");                  //Student
            Console.WriteLine("[2] Update a Person in Registry");       //Student
            Console.WriteLine("[3] List all People");                   //Student
            Console.WriteLine("[4] Search for a Person");               //Student

            var input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                if (result == 1)
                {
                    studentHelper.CreateStudentRecord();
                }
                else if (result == 2)
                {
                    studentHelper.UpdateStudentRecord();
                }
                else if (result == 3)
                {
                    studentHelper.ListAndInfo();
                }
                else if (result == 4)
                {
                    studentHelper.SearchStudents();
                }
            }
        }

        static void ShowCourseMenu(CourseHelper courseHelper)
        {
            Console.WriteLine("[1] Create a new Course");               //Course
            Console.WriteLine("[2] Update a Course");                   //Course
            Console.WriteLine("[3] Add a student to a Course");         //Course
            Console.WriteLine("[4] Add an Assignment");
            Console.WriteLine("[5] Remove an Assignment");
            Console.WriteLine("[6] Update an Assignment");
            Console.WriteLine("[7] Remove a student from a Course");    //Course
            Console.WriteLine("[8] Add a Module to a Course");          //Course
            Console.WriteLine("[9] Remove a Module from a Course");
            Console.WriteLine("[10] Update a Module");
            Console.WriteLine("[11] List all Courses");                  //Course
            Console.WriteLine("[12] Search for a Course");               //Course

            var input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                if (result == 1)
                {
                    courseHelper.CreateCourseRecord();
                }
                else if (result == 2)
                {
                    courseHelper.UpdateCourse();
                }
                else if (result == 3)
                {
                    courseHelper.AddStudent();
                }
                else if (result == 4)
                {
                    courseHelper.AddAssignment();
                }
                else if (result == 5)
                {
                    courseHelper.RemoveAssignment();
                }
                else if (result == 6)
                {
                    courseHelper.UpdateAssignment();
                }
                else if (result == 7)
                {
                    courseHelper.RemoveStudent();
                }
                else if (result == 8)
                {
                    courseHelper.AddModule();
                }
                else if (result == 9)
                {
                    courseHelper.RemoveModule();
                }
                else if (result == 10)
                {
                    courseHelper.UpdateModule();
                }
                else if (result == 11)
                {
                    courseHelper.ListAndSelect();
                }
                else if (result == 12)
                {
                    courseHelper.SearchCourses();
                }
            }

        }
    }
}
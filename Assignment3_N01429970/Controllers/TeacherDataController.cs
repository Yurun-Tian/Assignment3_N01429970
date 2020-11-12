using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment3_N01429970.Models;
using MySql.Data.MySqlClient;

namespace Assignment3_N01429970.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();
        //This Controller Will access the teachers table of our school database.
        /// <summary>
        /// Returns a list of teachers in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>
        /// A list of teachers (first names and last names)
        /// </returns>
        [HttpGet]
        public IEnumerable<Teacher> ListTeachers()
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from `teachers`";

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;

                //Add the teacher name to the List
                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teacher names
            return Teachers;
        }
        /// <summary>
        /// Returns the info about specific teacher and courses taught by the teacher
        /// </summary>
        /// <example>GET api/TeacherData/FindTeacher/{id}</example>
        /// <param name="id">teacherid</param>
        /// <returns>two object in type Tuple(Teacher, Course)</returns>
        [HttpGet]
        public (Teacher, Course) FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();
            Course NewCourse = new Course();
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "SELECT teachers.teacherid, teacherfname, teacherlname, employeenumber, hiredate, salary, classcode, classname, startdate, finishdate FROM `teachers` INNER JOIN `classes` ON teachers.teacherid = classes.teacherid WHERE teachers.teacherid = " + id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                double Salary = Convert.ToDouble(ResultSet["salary"]);
                string CourseCode = ResultSet["classcode"].ToString();
                string CourseName = ResultSet["classname"].ToString();
                DateTime StartDate = (DateTime)ResultSet["startdate"];
                DateTime FinishDate = (DateTime)ResultSet["finishdate"];


                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
                NewCourse.CourseCode = CourseCode;
                NewCourse.CourseName = CourseName;
                NewCourse.StartDate = StartDate;
                NewCourse.FinishDate = FinishDate;
            }

            //return the type Tuple(introduced in C#7)
            return (NewTeacher, NewCourse);
        }


    }
}

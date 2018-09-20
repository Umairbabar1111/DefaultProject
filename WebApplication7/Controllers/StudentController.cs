using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class StudentController : Controller
    {
        TasksContext mydb = null;
        public StudentController(TasksContext abc)
        {
            mydb = abc;
        }


        [HttpGet]
        public IActionResult Student()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Student(Student S)
        {
            mydb.Student.Add(S);
            mydb.SaveChanges();
            return View();
        }
        public IActionResult AllStudents()
        {
            IList<Student> S = new List<Student>();
            S = mydb.Student.ToList<Student>();
            return View(S);
        }




    }
}
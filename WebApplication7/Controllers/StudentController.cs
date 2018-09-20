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
        [HttpGet]
        public IActionResult EditStudent(int Id)
        {
            Student S = mydb.Student.Where(a => a.Id == Id).SingleOrDefault<Student>();
            return View(S);
        }
        [HttpPost]
        public IActionResult EditStudent(Student S)
        {
            mydb.Student.Update(S);
            mydb.SaveChanges();
            return RedirectToAction("AllStudents");
        }



    }
}
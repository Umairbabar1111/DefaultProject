using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class TeacherController : Controller
    {
        TasksContext mydb = null;
        public TeacherController(TasksContext abc)
        {
            mydb = abc;
        }

        [HttpGet]
        public IActionResult Teacher()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Teacher(Teacher T)
        {
            mydb.Teacher.Add(T);
            mydb.SaveChanges();
            return View();
        }


        public IActionResult AllTeachers()
        {
            IList<Teacher> T = new List<Teacher>();
            T = mydb.Teacher.ToList<Teacher>();
            return View(T);
        }

        [HttpGet]
        public IActionResult EditTeacher(int Id)
        {
            Teacher T = mydb.Teacher.Where(a => a.Id == Id).SingleOrDefault<Teacher>();
            return View(T);
        }
        [HttpPost]
        public IActionResult EditTeacher(Teacher T)
        { mydb.Teacher.Update(T);
            mydb.SaveChanges();
            return RedirectToAction("AllTeachers");
        }
        public IActionResult DetailsTeacher(int Id)
        {
            Teacher T = mydb.Teacher.Where(a => a.Id == Id).SingleOrDefault<Teacher>();
            return View(T);
        }
        
        public IActionResult DeleteTeacher(Teacher T)
        {
            mydb.Teacher.Remove(T);
            mydb.SaveChanges();
            return RedirectToAction("AllTeachers");

        }
    }
}
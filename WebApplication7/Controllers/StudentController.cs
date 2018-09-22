using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class StudentController : Controller
    {
        TasksContext mydb = null;
        IHostingEnvironment _env = null;
        public StudentController(TasksContext abc, IHostingEnvironment xyz)
        {
            this._env = xyz;
            mydb = abc;
        }


        [HttpGet]
        public IActionResult Student()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Student(Student S, IFormFile Image, IFormFile Cv)
        {
            bool x = mydb.Student.Where(a => a.Email == S.Email).Any();
          if(x)  {
                ViewBag.Message = "Email Already Exists";
                return View();
                
            }
            string path = _env.WebRootPath + "/Data/StudentsImages/";
            var E = Path.GetExtension(Image.FileName);
            var Name = DateTime.Now.ToString("yymmddhhmmss") + E;
            var f = new FileStream(path + E, FileMode.Create);
            Image.CopyTo(f);
            f.Close();
            S.Image = "/Data/StudentsImages/" + Name;

            string C = _env.WebRootPath + "/Data/StudentsDocuments/";
            var O = Path.GetExtension(Cv.FileName);
            var N = DateTime.Now.ToString("yymmhhddmmss") + O;
            var fs = new FileStream(C + O, FileMode.Create);
            Cv.CopyTo(fs);
            fs.Close();
            S.Cv = "/Data/StudentsDocuments/" + N;
            try
            {
                var msg = new MailMessage();
                msg.From = new MailAddress("Registration Mail");
                msg.To.Add(new MailAddress(S.Email));
                msg.CC.Add(new MailAddress("farhan@gmail.com"));
                msg.Subject = "Registration Mail";
                msg.Body = "Dear" + @S.Name + "Thanks" + "For" + "Registration";
                msg.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(@S.Image))
                    msg.Attachments.Add(new Attachment(path + Name));
                if (!string.IsNullOrEmpty(@S.Cv))
                    msg.Attachments.Add(new Attachment(C + N));


                SmtpClient sc = new SmtpClient();
                sc.Credentials = new System.Net.NetworkCredential();
                sc.Host = "smtp.gmail.com";
                sc.Port = 587;
                sc.EnableSsl = true;
                sc.Send(msg);
            }
            catch (Exception w)
            { }
            
                mydb.Student.Add(S);
                mydb.SaveChanges();
                return View();
            }
        [HttpGet]
        public IActionResult AllStudents()
        {
            IList<Student> S = mydb.Student.ToList<Student>();
            return View(S);
        }[HttpPost]
        public IActionResult AllStudents(string SearchByName, string SearchByEmail)
        {
            IList<Student> p = mydb.Student.Where(a => a.Name.Contains(SearchByName) || a.Email.Contains(SearchByEmail)).ToList<Student>();
            return View(p);

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

        public IActionResult DetailsStudent(int Id)
        {
            Student S = mydb.Student.Where(a => a.Id == Id).SingleOrDefault<Student>(); 
            return View(S);
        }
        public IActionResult DeleteStudent(Student S)
        {
            mydb.Student.Remove(S);
            mydb.SaveChanges();
            return RedirectToAction("AllStudents");
        }

        public FileResult DownloadCv(string path)
        {
            
                if (path == null)
                {
                    ViewBag.Message = "Invalid Path";
                    return null;
                }
                return File(path, new MimeSharp.Mime().Lookup(path), DateTime.Now.ToString("yymmddhhmmss") + System.IO.Path.GetExtension(path));
            }        
        
    }
}
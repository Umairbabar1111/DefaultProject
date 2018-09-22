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
    public class TeacherController : Controller
    {
        TasksContext mydb = null;
        IHostingEnvironment _env = null;
        public TeacherController(TasksContext abc,IHostingEnvironment xyz)
        {
            this._env = xyz;
            mydb = abc;
        }

        [HttpGet]
        public IActionResult Teacher()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Teacher(Teacher T,IFormFile Image,IFormFile Cv)
        {
            bool Email = mydb.Teacher.Where(p => p.Email == T.Email).Any();
            if (Email){
                ViewBag.Message = "Email Already Exists";
                return View();

            }
            string path = _env.WebRootPath + "/Data/TeachersImages/";
          string FileExt=  Path.GetExtension(Image.FileName);
            string Name = DateTime.Now.ToString("yymmddhhmmss") + FileExt;
            FileStream fs = new FileStream(path + FileExt, FileMode.Create);
            Image.CopyTo(fs);
            fs.Close();
            T.Image = "/Data/TeachersImages/" + Name;

            string CVPath = _env.WebRootPath+"/Data/TeachersDocuments/";
            string Ext = Path.GetExtension(Cv.FileName);
            string NameCV = DateTime.Now.ToString("yymmddhhmmss")+Ext;
            FileStream cs = new FileStream(CVPath + Ext, FileMode.Create);
            Cv.CopyTo(cs);
            cs.Close();
            T.Cv = "/Data/TeacherDocuments/" + NameCV;

            mydb.Teacher.Add(T);
            mydb.SaveChanges();




            var msg = new MailMessage();
            msg.From = new MailAddress("umairbabr1996@gmail.com", "Registration Mail");
            msg.To.Add(new MailAddress(T.Email));
            msg.CC.Add(new MailAddress("Farhan1996@gmail.com"));
            msg.Subject = "ragistration Mail";
            msg.Body = "Dear" + T.Name + "Thanks For Registration";
            msg.IsBodyHtml = true;
            if(!string.IsNullOrEmpty(@T.Image))
            msg.Attachments.Add(new Attachment(path + T.Image));
            SmtpClient sc = new SmtpClient();
            sc.Credentials =  new System.Net.NetworkCredential("umairbabar1996@gmail.com","88669966@@");
            sc.Host = "smpt.gmail.com";
            sc.Port = 587;
            sc.EnableSsl = true;
            sc.Send(msg);
            return View();
        }

        [HttpGet]
        public IActionResult AllTeachers()
        {
            IList<Teacher> T = mydb.Teacher.ToList<Teacher>();
            return View(T);
        }[HttpPost]
        public IActionResult AllTeachers(string SearchByName,string SearchByEmail)
        {
            IList<Teacher> p = mydb.Teacher.Where(a => a.Name.Contains(SearchByName) || a.Email.Contains(SearchByEmail)).ToList<Teacher>();
            return View(p);
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
        public FileResult DownloadCv(string path)
        {if (path == null) {
                ViewBag.Message = "Invalid Path";
                return null;           }
            return File(path,new MimeSharp.Mime().Lookup(path),DateTime.Now.ToString("yymmddhhmmss")+System.IO.Path.GetExtension(path));
        }
        public IActionResult AllTeacher()
        {
            return View(mydb.Teacher.ToList<Teacher>());
        }

    }
}
using Contoso.Data;
using Contoso.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Contoso.API.Controllers
{
    public class StudentController : ApiController
    {
        private ContosoContext _context;
        [HttpGet]
        public List<Student> GetStudents()
        {
            _context = new ContosoContext();
            return _context.Students.ToList();
        }

        [HttpGet]
        public Student GetStudentById(int id)
        {
            _context = new ContosoContext();
            return _context.Students.Find(id);
        }

        [HttpPost]
        public IHttpActionResult PostStudent(Student student)
        {
            _context = new ContosoContext();
            _context.Students.Add(student);
            _context.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = student.Id }, student);
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(Student student)
        {
            _context = new ContosoContext();
            _context.Entry(student).State = EntityState.Modified;
            _context.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(int id)
        {
            _context = new ContosoContext();
            var student = _context.Students.Find(id);
            _context.Students.Remove(student);
            _context.SaveChanges();
            return Ok(student);
        }

        [HttpPost]
        [Route("api/upload-student-photo/{studentId}")]
        public IHttpActionResult UploadStudentPhoto(int studentId)
        {
            _context = new ContosoContext();
            var student = _context.Students.Find(studentId);

            string destinationPath = @"E:\Personal\Personal Projects\AngularJs\ContosoApp\Contoso.Web\Documents";
            HttpFileCollection files = HttpContext.Current.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];
                if(file.ContentLength > 0)
                {
                    string fileName = new FileInfo(file.FileName).Name;
                    string fileExtension = new FileInfo(file.FileName).Extension;

                    if(fileExtension == ".jpg" || fileExtension == ".png")
                    {
                        Guid id = Guid.NewGuid();
                        fileName = id.ToString() + "_" + fileName;
                        file.SaveAs(destinationPath + Path.GetFileName(fileName));
                        student.Photo = "Documents/" + fileName;
                        _context.Entry(student).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Ok("Photo uploaded successfully.");
                    }
                }
            }
            return Ok("Photo upload unsuccessful.");
        }
    }
}

using System.Collections.Generic;
using ASPNetCoreDemo1.Core.Interfaces;
using ASPNetCoreDemo1.Core.Models;
using System.Linq;

namespace ASPNetCoreDemo1.Core.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbContext _context;

        public StudentRepository(StudentDbContext context)
        {
            _context = context;
        }
        public Student Add(Student s)
        {
            _context.Students.Add(s);
            _context.SaveChanges();
            return s;
        }

        public List<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public Student Get(string jmbag)
        {
            return _context.Students.Where(s => s.Jmbag == jmbag).FirstOrDefault();
        }
    }


}

using DataAccess.Interface;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Practical_19.Data;

namespace Practical_19.Repository
{
    public class StudentRepository : IStudentRepo
    {
        private readonly AppDbContext context;

        public StudentRepository(AppDbContext context)
        {
            this.context = context;
        }
        int IStudentRepo.Add(Students student)
        {
            try
            {
                context.Students.Add(student);
                context.SaveChanges();
                int id = context.Students.Max(s => s.Id);
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        bool IStudentRepo.Delete(int id)
        {
            var st = context.Students.Where(a => a.Id == id).FirstOrDefault();
            if (st != null)
            {
                context.Remove(st);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        bool IStudentRepo.Edit(Students studentChanges)
        {
            var st = context.Students.FirstOrDefault(x => x.Id == studentChanges.Id);
            if (st != null)
            {
                st.StudentName = studentChanges.StudentName;
                st.MobileNumber = studentChanges.MobileNumber;
                st.Email = studentChanges.Email;
                context.Students.Update(st);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        async Task<IEnumerable<Students>> IStudentRepo.GetAllStudents()
        {
            var data = await context.Students.ToListAsync();
            return data;
        }

        Students IStudentRepo.GetStudent(int id)
        {
            Students? data = context.Students.FirstOrDefault(s => s.Id == id);
            return data;
        }
    }
}

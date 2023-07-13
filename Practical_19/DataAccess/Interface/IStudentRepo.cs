using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface IStudentRepo
    {
        Students GetStudent(int id);
        Task<IEnumerable<Students>> GetAllStudents();
        int Add(Students student);
        bool Edit(Students studentChanges);
        bool Delete(int id);
    }
}

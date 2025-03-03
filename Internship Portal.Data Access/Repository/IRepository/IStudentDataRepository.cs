using Internship_Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IStudentDataRepository : IRepository<Student>
    {
        public void Update(Student student);
    }
}

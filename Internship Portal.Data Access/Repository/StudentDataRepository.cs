using Internship_Portal.Data_Access.Data;
using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using static System.Collections.Specialized.BitVector32;

namespace Internship_Portal.Data_Access.Repository
{
    public class StudentDataRepository : Repository<Student>, IStudentDataRepository
    {
        private readonly ApplicationDbContext _db;

        public StudentDataRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Student studentData)
        {
            _db.StudentsData.Update(studentData);
        }


    }
}

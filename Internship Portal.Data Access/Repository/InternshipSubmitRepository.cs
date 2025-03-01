using Internship_Portal.Data_Access.Data;
using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;
using Internship_Portal.Utility;

namespace Internship_Portal.Data_Access.Repository
{
    public class InternshipSubmitRepository : Repository<InternshipSubmit>, IInternshipSubmitRepository
    {
        private readonly ApplicationDbContext _db;

        public InternshipSubmitRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(InternshipSubmit entity)
        {
            _db.Update(entity);
        }

        public void UpdateStatus(int internshipId, string internshipStatus)
        {
        }

    }
}

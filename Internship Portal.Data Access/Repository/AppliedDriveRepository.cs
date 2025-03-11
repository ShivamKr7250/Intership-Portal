using Internship_Portal.Data_Access.Data;
using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;

namespace Internship_Portal.Data_Access.Repository
{
    public class AppliedDriveRepository : Repository<AppliedDrive> , IAppliedDriveRepository
    {
        private readonly ApplicationDbContext _db;
        public AppliedDriveRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(AppliedDrive entity)
        {
            _db.Update(entity);
        }
    }
}

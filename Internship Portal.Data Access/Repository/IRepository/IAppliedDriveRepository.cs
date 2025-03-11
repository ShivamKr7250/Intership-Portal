using Internship_Portal.Model;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IAppliedDriveRepository : IRepository<AppliedDrive>
    {
        void Update(AppliedDrive entity);
    }
}

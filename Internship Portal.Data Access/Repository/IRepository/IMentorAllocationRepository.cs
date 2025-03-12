using Internship_Portal.Model;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IMentorAllocationRepository : IRepository<MentorAllocation>
    {
        void Update(MentorAllocation entity);
    }
}

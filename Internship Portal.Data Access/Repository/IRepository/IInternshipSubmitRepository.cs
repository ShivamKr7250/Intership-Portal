using Internship_Portal.Model;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IInternshipSubmitRepository : IRepository<InternshipSubmit>
    {
        void Update(InternshipSubmit entity);

        void UpdateStatus(int internshipId, string internshipStatus);
    }
}

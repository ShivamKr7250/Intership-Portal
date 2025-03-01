using Internship_Portal.Model;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IInteractionRepository : IRepository<Interaction>
    {
        void Update(Interaction entity);
    }
}

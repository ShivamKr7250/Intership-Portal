using Internship_Portal.Model;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IContactRepository : IRepository<Contact>
    {
        void Update(Contact entity);
    }
}

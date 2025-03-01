using Internship_Portal.Model;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IBlogCategoryRepository : IRepository<BlogCategory>
    {
        void Update(BlogCategory entity);
    }
}

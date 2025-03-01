using Internship_Portal.Model;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IBlogCommentRepository : IRepository<BlogComment>
    {
        void Update(BlogComment entity);
    }
}

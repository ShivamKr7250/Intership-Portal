using Internship_Portal.Model;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        void Update(BlogPost entity);

        IEnumerable<BlogPost> GetPostsByDescendingPublicationDate();

        IEnumerable<BlogPost> SearchBlogPosts(string searchTerm);
    }
}

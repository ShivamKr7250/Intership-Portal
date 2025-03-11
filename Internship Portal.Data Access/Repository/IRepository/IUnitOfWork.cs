using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRegistrationFormRepository RegistrationForm { get; }
        IApplicationUserRepository User { get; }
        IContactRepository Contact { get; }
        IInternshipSubmitRepository InternshipSubmit { get; }
        IBlogPostRepository BlogPost { get; }
        IBlogCommentRepository BlogComment { get; }
        IInteractionRepository Interaction { get; }
        IBlogCategoryRepository BlogCategory { get; }
        IStudentDataRepository StudentData { get; }
        IAppliedDriveRepository AppliedDrive { get; }
        void Save();
    }
}

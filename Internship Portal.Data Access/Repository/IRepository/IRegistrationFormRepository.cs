using Internship_Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_Portal.Data_Access.Repository.IRepository
{
    public interface IRegistrationFormRepository : IRepository<RegistrationForm>
    {
        void Update(RegistrationForm entity);
        void UpdateStatus(int registrationId, string registrationStatus);
    }
}

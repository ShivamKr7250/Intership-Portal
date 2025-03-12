using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Internship_Portal.Data_Access.Data;
using Internship_Portal.Data_Access.Repository.IRepository;
using Internship_Portal.Model;

namespace Internship_Portal.Data_Access.Repository
{
    public class MentorAllocationRepository : Repository<MentorAllocation> , IMentorAllocationRepository
    {
        private readonly ApplicationDbContext _db;
        public MentorAllocationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MentorAllocation entity)
        {
            _db.Update(entity);
        }
    }
}

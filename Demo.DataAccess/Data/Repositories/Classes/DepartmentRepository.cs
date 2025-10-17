using Demo.DataAccess.Data.Contexts;
using Demo.DataAccess.Data.Repositories.Interfaces;
using Demo.DataAccess.Models.DepartmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Data.Repositories.Classes
{
    public class DepartmentRepository(ApplicationDbContext _dbContext) : GenericRepository<Department>(_dbContext), IDepartmentRepository
    {
         
    }
}

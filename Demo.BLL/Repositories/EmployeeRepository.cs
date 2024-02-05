using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        //private readonly AppDbContext _dbContext;
        // انا خليته يورثه من جينرك ريبوزيتورى و عملته هناك بروتكتد و كده مش محتاج اعرفه تحت فى الا اوبجكت

        public EmployeeRepository(AppDbContext dbContext)  // Ask CLR for creating an Obj from DbContext 
            : base(dbContext)
        {

        }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
        {
            return _dbContext.Employees.Where(E=>E.Address.ToLower().Contains(address.ToLower()));
        }

        public IQueryable<Employee> SearchByName(string name)
            => _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name));
    }
}

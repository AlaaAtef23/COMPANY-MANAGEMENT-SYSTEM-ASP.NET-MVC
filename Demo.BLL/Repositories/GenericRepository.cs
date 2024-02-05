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
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly AppDbContext _dbContext; //Prtected علشان اعرف اورثه

        public GenericRepository(AppDbContext dbContext)
        {
            //_dbContext = /*new AppDbContext();*/

            _dbContext = dbContext;
        }

        public void Add(T entity)
           => _dbContext.Add(entity); //EF Core 3.1 Feature
        //_dbContext.Set<T>().Add(entity);


        public void Delete(T entity)
            => _dbContext.Remove(entity);
        //_dbContext.Set<T>().Remove(entity);

        public void Update(T entity)
            =>_dbContext.Update(entity);
        //_dbContext.Set<T>().Update(entity);

        public T Get(int id)
        {
            ///var department = _dbContext.Departments.Local.Where(D => D.Id == id).FirstOrDefault();
            ///if(department == null)
            ///    department = _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();
            ///return department;
            //return _dbContext.Departments.Find(id);

            return _dbContext.Find<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            if(typeof(T)== typeof(Employee))
                return (IEnumerable<T>) _dbContext.Employees.Include(E =>E.Department).AsNoTracking().ToList();
            else
                return _dbContext.Set<T>().AsNoTracking().ToList();
        }
    }
}

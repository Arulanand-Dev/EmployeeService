using EmployeeService.DataAccess.DBContext;
using EmployeeService.DataAccess.Repository.Interface;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DataAccess.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context; 
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsTracking();
        }

        public IQueryable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> predicate =null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var dbEntity=_dbSet.AsTracking();

            if (includeProperties.Any()){
                dbEntity = includeProperties.Aggregate
                    (dbEntity, (current, includeProperties) => (current.Include(includeProperties)));
            }

            return dbEntity.Where(predicate);
        }

        public async Task<int> UpdatePatch(int id, JsonPatchDocument TEntity)
        {
            var dbEntity = await _dbSet.FindAsync(id);

            if (dbEntity != null)
            {
                TEntity.ApplyTo(dbEntity);
                return await _context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }
    }
}

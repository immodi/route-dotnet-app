using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using SqlServerWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SqlServerWebApi.Data
{
    public class Repository<T> : IRepository<T> where T : class, IEntity 
    {
        private readonly OrderManagementDbContext _context;


        public Repository(OrderManagementDbContext context)
        {
            _context = context;
        }
       
        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    return entity;
                }
                return null;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var items = new List<T>();

            items = await _context.Set<T>().ToListAsync();

            return items;
        }

        public async Task<T?> AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<T>?> AddRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                await _context.Set<T>().AddRangeAsync(entities);
                await _context.SaveChangesAsync();
                
                return entities;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                return null;
            }
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                return null;
            }
        }

        

        //     // Additional method for lazy loading related entities
        // public async Task<IEnumerable<T>?> GetAllIncludeAsync(params Expression<Func<T, object>>[] includes)
        // {
        //     // var items = new List<T>();

        //     var query = _context.Set<T>().AsQueryable();
            
        //     foreach (var include in includes)
        //     {
        //         query.Include(include);
        //     }

        //     var items = await query.ToListAsync();
        //     return items;
        // }
    }
}

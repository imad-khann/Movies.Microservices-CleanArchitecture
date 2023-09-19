using Microsoft.EntityFrameworkCore;
using Movies.Core.Entities.Base;
using Movies.Core.Repositories.Base;
using Movies.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Infrastructure.Repositories.Base
{
    /// <summary>
    /// Designed by Imad Khan
    /// high level CRUD logic will go here
    /// </summary>
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly MovieContext _movieContext;

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _movieContext.Set<T>().ToListAsync();
        }
        public Repository(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _movieContext.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _movieContext.Set<T>().AddAsync(entity);
            await _movieContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _movieContext.Entry(entity).State = EntityState.Modified;
            await _movieContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _movieContext.Set<T>().Remove(entity);
            await _movieContext.SaveChangesAsync();
        }
    }
}

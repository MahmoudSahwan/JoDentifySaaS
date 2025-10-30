using JoDentify.Core;
using JoDentify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JoDentify.Infrastructure.Repositories
{
    // 1. ده التنفيذ الفعلي للـ Interface اللي عملناه في الـ Core
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); // _context.Set<T>() دي معناها "هاتلي الجدول بتاع النوع T"
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            // إحنا هنا بنعمل remove عادي
            // والـ DbContext (في SaveChangesAsync) هو اللي هيحولها لـ Soft Delete
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            // الـ Global Query Filter (بتاع IsDeleted) هيتطبق هنا أوتوماتيك
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // الـ Global Query Filter (بتاع IsDeleted) هيتطبق هنا أوتوماتيك
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            // الـ Global Query Filter (بتاع IsDeleted) هيتطبق هنا أوتوماتيك
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            // دي عشان نحفظ التغييرات كلها مرة واحدة
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            // الـ DbContext (في SaveChangesAsync) هيظبط الـ UpdatedAt أوتوماتيك
            _dbSet.Update(entity);
        }
    }
}

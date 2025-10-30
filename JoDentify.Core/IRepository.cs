using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JoDentify.Core
{
    // ده الـ Generic Interface للـ Repository Pattern
    // هيوفر لينا العمليات الأساسية (CRUD) لأي Entity
    // "where T : BaseEntity" معناها إن الـ Interface ده مينفعش يشتغل غير مع الكلاسات اللي بترث من BaseEntity
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        // ده عشان نعمل عمليات بحث مخصصة (زي البحث بالاسم مثلاً)
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        void Update(T entity); // Update و Delete مش Async
        void Delete(T entity);

        // هنحتاج دي عشان نعمل SaveChangesAsync مرة واحدة في الـ Unit of Work (أو في الـ Service)
        Task<int> SaveChangesAsync();
    }
}

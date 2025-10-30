using JoDentify.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Expressions; // <<<--- ده السطر الأهم

namespace JoDentify.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // هنا هنضيف الـ DbSets للجداول الجديدة (زي Clinic, Patient)
        // public DbSet<Clinic> Clinics { get; set; }
        // public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // السطر ده بيخلي الـ DbContext يدور على أي configuration
            // زي الـ Soft Delete Global Filter
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // --- 2. فلتر الـ Soft Delete العالمي ---
            // الكود ده بيضيف (WHERE IsDeleted = 0) على أي (SELECT)
            // لأي جدول بيرث من BaseEntity
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var condition = Expression.Equal(property, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);

                    builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        // --- 3. عمل Override لـ SaveChangesAsync (عشان الـ Soft Delete) ---
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; // حول الـ Delete لـ Modify
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

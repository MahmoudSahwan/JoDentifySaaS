using JoDentify.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JoDentify.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ClinicService> ClinicServices { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Clinic>(e =>
            {
                e.HasOne(c => c.Owner)
                 .WithOne()
                 .HasForeignKey<Clinic>(c => c.OwnerId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(c => c.Users)
                 .WithOne(u => u.Clinic)
                 .HasForeignKey(u => u.ClinicId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<Patient>(e =>
            {
                e.HasOne(p => p.Clinic)
                 .WithMany(c => c.Patients)
                 .HasForeignKey(p => p.ClinicId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(p => p.Gender)
                 .HasConversion<string>()
                 .HasMaxLength(10);
            });

            builder.Entity<Appointment>(e =>
            {
                e.Property(a => a.Status)
                 .HasConversion<string>()
                 .HasMaxLength(20);

                e.HasOne(a => a.Clinic)
                 .WithMany(c => c.Appointments)
                 .HasForeignKey(a => a.ClinicId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(a => a.Patient)
                 .WithMany(p => p.Appointments)
                 .HasForeignKey(a => a.PatientId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(a => a.Doctor)
                 .WithMany(u => u.AppointmentsAsDoctor)
                 .HasForeignKey(a => a.DoctorId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ClinicService>(e =>
            {
                e.Property(s => s.Price)
                 .HasColumnType("decimal(18, 2)");

                e.Property(s => s.Status)
                 .HasConversion<string>()
                 .HasMaxLength(20);

                e.HasOne(s => s.Clinic)
                 .WithMany(c => c.ClinicServices)
                 .HasForeignKey(s => s.ClinicId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Invoice>(e =>
            {
                e.Property(i => i.TotalAmount).HasColumnType("decimal(18, 2)");
                e.Property(i => i.AmountPaid).HasColumnType("decimal(18, 2)");
                e.Property(i => i.Status).HasConversion<string>().HasMaxLength(20);

                e.HasOne(i => i.Clinic)
                 .WithMany(c => c.Invoices)
                 .HasForeignKey(i => i.ClinicId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(i => i.Patient)
                 .WithMany(p => p.Invoices)
                 .HasForeignKey(i => i.PatientId)
                 .IsRequired()
                 // ---!!! ده السطر اللي اتصلح !!!---
                 // (غيرنا Cascade لـ Restrict عشان نكسر الدايرة)
                 .OnDelete(DeleteBehavior.Restrict);
                // ---!!! نهاية التعديل !!!---

                e.HasOne(i => i.Appointment)
                 .WithMany(a => a.Invoices)
                 .HasForeignKey(i => i.AppointmentId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<InvoiceItem>(e =>
            {
                e.Property(ii => ii.UnitPrice).HasColumnType("decimal(18, 2)");
                e.Property(ii => ii.TotalPrice).HasColumnType("decimal(18, 2)");

                e.HasOne(ii => ii.Invoice)
                 .WithMany(i => i.InvoiceItems)
                 .HasForeignKey(ii => ii.InvoiceId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ii => ii.ClinicService)
                 .WithMany(s => s.InvoiceItems)
                 .HasForeignKey(ii => ii.ClinicServiceId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ii => ii.Clinic)
                 .WithMany(c => c.InvoiceItems)
                 .HasForeignKey(ii => ii.ClinicId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PaymentTransaction>(e =>
            {
                e.Property(pt => pt.Amount).HasColumnType("decimal(18, 2)");
                e.Property(pt => pt.PaymentMethod).HasConversion<string>().HasMaxLength(20);

                e.HasOne(pt => pt.Invoice)
                 .WithMany(i => i.PaymentTransactions)
                 .HasForeignKey(pt => pt.InvoiceId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(pt => pt.Clinic)
                 .WithMany(c => c.PaymentTransactions)
                 .HasForeignKey(pt => pt.ClinicId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);
            });


            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "IsDeleted");
                    var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);
                    entityType.SetQueryFilter(filter);
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
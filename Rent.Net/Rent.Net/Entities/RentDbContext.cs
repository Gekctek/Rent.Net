using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;

namespace Rent.Net.Entities
{
    public class RentDbContext : DbContext
    {
        public RentDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<RentDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Request
            modelBuilder.Entity<Request>().HasKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasRequired(r => r.Payee)
                .WithMany(u => u.RequestsFrom)
                .HasForeignKey(r => r.PayeeId)
                .WillCascadeOnDelete(false);
            
           modelBuilder.Entity<Request>()
                .HasRequired(r => r.Payer)
                .WithMany(r => r.RequestsTo)
                .HasForeignKey(r => r.PayerId)
                .WillCascadeOnDelete(false);

            //Payment
            modelBuilder.Entity<Payment>().HasKey(p => p.PaymentId);

            modelBuilder.Entity<Payment>()
                .HasOptional(p => p.Request)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.RequestId);

            modelBuilder.Entity<Payment>()
                .HasRequired(p => p.Payee)
                .WithMany(u => u.PaymentsTo)
                .HasForeignKey(p => p.PayeeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .HasRequired(p => p.Payer)
                .WithMany(u => u.PaymentsFrom)
                .HasForeignKey(p => p.PayerId)
                .WillCascadeOnDelete(false);

            //User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
        }

        internal static RentDbContext Create()
        {
            return new RentDbContext();
        }

        public DbSet<Request> Requests { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }

    }

   
}
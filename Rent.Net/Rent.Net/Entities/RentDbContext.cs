using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;

namespace Rent.Net.Entities
{
    public class RentDbContext : IdentityDbContext<ApplicationUser>
    {
        public RentDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer<RentDbContext>(new CreateDatabaseIfNotExists<RentDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Request>().HasKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasRequired(r => r.Payee)
                .WithMany(r => r.RequestsTo)
                .HasForeignKey(r => r.PayeeId)
                .WillCascadeOnDelete(false);
            
           modelBuilder.Entity<Request>()
                .HasRequired(r => r.Payer)
                .WithMany(r => r.RequestsFrom)
                .HasForeignKey(r => r.PayerId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(ul => ul.UserId);
            modelBuilder.Entity<IdentityUserRole>().HasKey<string>(ur => ur.RoleId);
        }

        internal static RentDbContext Create()
        {
            return new RentDbContext();
        }

        public DbSet<Request> Requests { get; set; }

    }

   
}
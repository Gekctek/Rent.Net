using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Rent.Net.Entities
{
    public class RentDbContext : IdentityDbContext<ApplicationUser>
    {
        public RentDbContext() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Request>()
                .HasRequired(r => r.Payee)
                .WithMany(r => r.Requests)
                .HasForeignKey(r => r.PayeeId);

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(ul => ul.UserId);
            modelBuilder.Entity<IdentityUserRole>().HasKey<string>(ur => ur.RoleId);
        }

        public DbSet<Request> Requests { get; set; }

        public static RentDbContext Create()
        {
            return new RentDbContext();
        }
    }

   
}
using Microsoft.EntityFrameworkCore;
using Charity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Charity.DAL
{
    public class CharityDbContext : IdentityDbContext<IdentityUser>
    {

        public DbContextOptions _options;

        public CharityDbContext(DbContextOptions<CharityDbContext> options) : base(options)
        {
            _options = options;
        }
        // TODO: Add tables here
        // public DbSet<Entity> Models { get; set; } = null;
        public DbSet<AdminEntity> Admins { get; set; } = null;
        public DbSet<DonationEntity> Donations { get; set; } = null;
        public DbSet<ShelterAdminEntity> ShelterAdmins { get; set; } = null;
        public DbSet<ShelterEntity> Shelters { get; set; } = null;
        public DbSet<VolunteerEntity> Volunteers { get; set; } = null;
        public DbSet<VolunteeringEntity> Volunteerings { get; set; } = null;
        public DbSet<TransactionEntity> Transactions { get; set; } = null;
        public DbSet<EnrollmentEntity> Enrollments { get; set; } = null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionEntity>()
                .HasOne(t => t.Donation)
                .WithMany(d => d.Transactions)
                .HasForeignKey(tt => tt.DonationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransactionEntity>()
                .HasOne(t => t.Volunteer)
                .WithMany(d => d.Transactions)
                .HasForeignKey(tt => tt.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EnrollmentEntity>()
                .HasOne(e => e.Volunteer)
                .WithMany(v => v.Enrollments)
                .HasForeignKey(ee => ee.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EnrollmentEntity>()
                .HasOne(e => e.Volunteering)
                .WithMany(v => v.Enrollments)
                .HasForeignKey(ee => ee.VolunteeringId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VolunteeringEntity>()
                .HasOne(v => v.Shelter)
                .WithMany(s => s.Volunteerings)
                .HasForeignKey(vv => vv.ShelterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DonationEntity>()
                .HasOne(d => d.Shelter)
                .WithMany(s => s.Donations)
                .HasForeignKey(dd => dd.ShelterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShelterAdminEntity>()
                .HasOne(a => a.Shelter)
                .WithOne(s => s.Admin)
                .HasForeignKey<ShelterEntity>(aa => aa.AdminId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog = Charity;MultipleActiveResultSets = True;Integrated Security = True;");
        }
    }
}

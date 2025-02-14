using Microsoft.EntityFrameworkCore;

namespace MiniClinic.Entities
{
    public class MiniClinicDbContext(DbContextOptions<MiniClinicDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminPassword = AuthService.HashPassword("adminpass");

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 101,
                Email = "admin@example.ba",
                Name = "Super Admin",
                Password = "IdDhTeM1sRWA2FD6wDmFPx9ZGJVBiDPdYiWFbftu5d/mXQmr0OMN7HL/ZEOZWcuB",
                Role = UserRole.Admin
            });
        }
    }
}

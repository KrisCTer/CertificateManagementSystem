using CertificateManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<EducationalInstitution> EducationalInstitutions { get; set; }
        public DbSet<CertificateType> CertificateTypes { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<CertificateHistory> CertificateHistories { get; set; }
        public DbSet<CertificateVerification> CertificateVerifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<CertificateRequestz> CertificateRequests { get; set; }
        public DbSet<Attachmentz> Attachments { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure relationships
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Citizen)
                .WithMany(c => c.Certificates)
                .HasForeignKey(c => c.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.CertificateType)
                .WithMany(ct => ct.Certificates)
                .HasForeignKey(c => c.CertificateTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.IssuingInstitution)
                .WithMany(i => i.IssuedCertificates)
                .HasForeignKey(c => c.IssuingInstitutionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CertificateHistory>()
                .HasOne(ch => ch.Certificate)
                .WithMany(c => c.CertificateHistories)
                .HasForeignKey(ch => ch.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CertificateVerification>()
                .HasOne(cv => cv.Certificate)
                .WithMany(c => c.CertificateVerifications)
                .HasForeignKey(cv => cv.CertificateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CertificateRequestz>()
                .HasOne(cr => cr.Citizen)
                .WithMany(c => c.CertificateRequests)
                .HasForeignKey(cr => cr.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CertificateRequestz>()
                .HasOne(cr => cr.CertificateType)
                .WithMany(ct => ct.CertificateRequests)
                .HasForeignKey(cr => cr.CertificateTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CertificateRequestz>()
                .HasOne(cr => cr.Processor)
                .WithMany(u => u.ProcessedRequests)
                .HasForeignKey(cr => cr.ProcessedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Attachmentz>()
                .HasOne(a => a.CertificateRequest)
                .WithMany(cr => cr.Attachments)
                .HasForeignKey(a => a.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attachmentz>()
                .HasOne(a => a.Uploader)
                .WithMany(u => u.UploadedAttachments)
                .HasForeignKey(a => a.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<SystemLog>()
                .HasOne(sl => sl.User)
                .WithMany(u => u.SystemLogs)
                .HasForeignKey(sl => sl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SystemConfiguration>()
                .HasOne(sc => sc.UpdatedByUser)
                .WithMany(u => u.SystemConfigurations)
                .HasForeignKey(sc => sc.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}

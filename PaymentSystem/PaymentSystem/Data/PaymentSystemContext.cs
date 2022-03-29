using Microsoft.EntityFrameworkCore;

namespace PaymentSystem.Data
{
    public class PaymentSystemContext : DbContext
    {
        public PaymentSystemContext(DbContextOptions<PaymentSystemContext> options)
            :base(options)
        {
        }
        
        public DbSet<BalanceRecord> Balances { get; set; }
        
        public DbSet<VerificationTransferRecord> VerificationTransfers { get; set; }

        public DbSet<FundTransferRecord> FundTransfers { get; set; }
        public DbSet<RoleRecord> Roles { get; set; }
        public DbSet<UserRecord> Users { get; set; }
        public DbSet<UserRoleRecord> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BalanceRecord>()
                .HasOne(b => b.UserRecord)
                .WithOne(u => u.Balance)
                .HasForeignKey<BalanceRecord>(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRoleRecord>()
                .HasOne(ur => ur.RoleRecord)
                .WithMany(r => r.UserRoleRecords)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<UserRoleRecord>()
                .HasOne(ur => ur.UserRecord)
                .WithMany(u => u.UserRoleRecords)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FundTransferRecord>()
                .HasOne(f => f.CreatedByUserRecord)
                .WithMany(u => u.FundTransferCreatedBy)
                .HasForeignKey(u => u.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FundTransferRecord>()
                .HasOne(f => f.UserRecord)
                .WithMany(u => u.FundTransfers)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FundTransferRecord>()
                .HasOne(f => f.ConfirmedByUserRecord)
                .WithMany(u => u.FundTransferConfirmedBy)
                .HasForeignKey(f => f.ConfirmedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VerificationTransferRecord>()
                .HasOne(v => v.User)
                .WithOne(u => u.VerificationTransfer)
                .HasForeignKey<VerificationTransferRecord>(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VerificationTransferRecord>()
                .HasOne(v => v.ConfirmedByUser)
                .WithMany(u => u.VerificationTransfersConfirmedBy)
                .HasForeignKey(v => v.ConfirmedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoleRecord>().HasData(
                new { Id = 1, Name = "User" },
                new { Id = 2, Name = "Admin" },
                new { Id = 3, Name = "KYC-Manager" },
                new { Id = 4, Name = "Funds-Manager" }
            );

            modelBuilder.Entity<UserRecord>().HasData(
                new
                {
                    Id = 2,
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin@gmail.com",
                    Password = "admin1234",
                    RegisteredAt = DateTime.UtcNow,
                    IsBlocked = false,
                    IsVerified = false
                }
            );

            modelBuilder.Entity<BalanceRecord>().HasData
            (
                new
                {
                    Id = 1,
                    UserId = 2,
                    Amount = 1000m
                }
            );

            modelBuilder.Entity<UserRoleRecord>().HasData
            (
                new 
                {
                    Id = 1,
                    UserId = 2,
                    RoleId = 2
                }
            );
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace PaymentSystem.Data
{
    public partial class PaymentSystemContext : DbContext
    {
        public PaymentSystemContext()
        {
        }

        public PaymentSystemContext(DbContextOptions<PaymentSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Balance> Balances { get; set; } = null!;
        public virtual DbSet<FundTransfer> FundTransfers { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost:5438;Database=payment_system;Username=user;Password=1234");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Balance>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("balances");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("balances_user_id_fkey");
            });

            modelBuilder.Entity<FundTransfer>(entity =>
            {
                entity.ToTable("fund_transfers");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CardCvc)
                    .HasMaxLength(3)
                    .HasColumnName("card_cvc");

                entity.Property(e => e.CardDate)
                    .HasMaxLength(10)
                    .HasColumnName("card_date");

                entity.Property(e => e.CardNumber)
                    .HasMaxLength(50)
                    .HasColumnName("card_number");

                entity.Property(e => e.ConfirmedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("confirmed_at");

                entity.Property(e => e.ConfirmedBy).HasColumnName("confirmed_by");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.ConfirmedByNavigation)
                    .WithMany(p => p.FundTransferConfirmedByNavigations)
                    .HasForeignKey(d => d.ConfirmedBy)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fund_transfers_confirmed_by_fkey");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FundTransferCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("fund_transfers_created_by_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FundTransferUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fund_transfers_user_id_fkey");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsVerified)
                    .HasColumnName("is_verified")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                entity.Property(e => e.PassportData)
                    .HasMaxLength(30)
                    .HasColumnName("passport_data");

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .HasColumnName("password");

                entity.Property(e => e.RegisteredAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("registered_at");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_roles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("user_roles_role_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("user_roles_user_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

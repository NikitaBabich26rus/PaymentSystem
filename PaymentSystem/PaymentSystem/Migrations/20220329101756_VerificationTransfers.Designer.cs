﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PaymentSystem.Data;

#nullable disable

namespace PaymentSystem.Migrations
{
    [DbContext(typeof(PaymentSystemContext))]
    [Migration("20220329101756_VerificationTransfers")]
    partial class VerificationTransfers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PaymentSystem.Data.BalanceRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("balances");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 1000m,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("PaymentSystem.Data.FundTransferRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CardCvc")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("card_cvc");

                    b.Property<string>("CardDate")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_date");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("card_number");

                    b.Property<DateTime?>("ConfirmedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("confirmed_at");

                    b.Property<int?>("ConfirmedBy")
                        .HasColumnType("integer")
                        .HasColumnName("confirmed_by");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("created_by");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("ConfirmedBy");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("UserId");

                    b.ToTable("fund_transfers");
                });

            modelBuilder.Entity("PaymentSystem.Data.RoleRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "User"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 3,
                            Name = "KYC-Manager"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Funds-Manager"
                        });
                });

            modelBuilder.Entity("PaymentSystem.Data.UserRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean")
                        .HasColumnName("is_blocked");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean")
                        .HasColumnName("is_verified");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("password");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("registered_at");

                    b.HasKey("Id");

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Email = "admin@gmail.com",
                            FirstName = "admin",
                            IsBlocked = false,
                            IsVerified = false,
                            LastName = "admin",
                            Password = "admin1234",
                            RegisteredAt = new DateTime(2022, 3, 29, 10, 17, 56, 249, DateTimeKind.Utc).AddTicks(9794)
                        });
                });

            modelBuilder.Entity("PaymentSystem.Data.UserRoleRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("RoleId")
                        .HasColumnType("integer")
                        .HasColumnName("role_id");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("user_roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            RoleId = 2,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("PaymentSystem.Data.VerificationTransferRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ConfirmedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("confirmed_at");

                    b.Property<int?>("ConfirmedBy")
                        .HasColumnType("integer")
                        .HasColumnName("confirmed_by");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("PassportData")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Passport_data");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("ConfirmedBy");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("verification_transfers");
                });

            modelBuilder.Entity("PaymentSystem.Data.BalanceRecord", b =>
                {
                    b.HasOne("PaymentSystem.Data.UserRecord", "UserRecord")
                        .WithOne("Balance")
                        .HasForeignKey("PaymentSystem.Data.BalanceRecord", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRecord");
                });

            modelBuilder.Entity("PaymentSystem.Data.FundTransferRecord", b =>
                {
                    b.HasOne("PaymentSystem.Data.UserRecord", "ConfirmedByUserRecord")
                        .WithMany("FundTransferConfirmedBy")
                        .HasForeignKey("ConfirmedBy")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PaymentSystem.Data.UserRecord", "CreatedByUserRecord")
                        .WithMany("FundTransferCreatedBy")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PaymentSystem.Data.UserRecord", "UserRecord")
                        .WithMany("FundTransfers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ConfirmedByUserRecord");

                    b.Navigation("CreatedByUserRecord");

                    b.Navigation("UserRecord");
                });

            modelBuilder.Entity("PaymentSystem.Data.UserRoleRecord", b =>
                {
                    b.HasOne("PaymentSystem.Data.RoleRecord", "RoleRecord")
                        .WithMany("UserRoleRecords")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PaymentSystem.Data.UserRecord", "UserRecord")
                        .WithMany("UserRoleRecords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleRecord");

                    b.Navigation("UserRecord");
                });

            modelBuilder.Entity("PaymentSystem.Data.VerificationTransferRecord", b =>
                {
                    b.HasOne("PaymentSystem.Data.UserRecord", "ConfirmedByUser")
                        .WithMany("VerificationTransfersConfirmedBy")
                        .HasForeignKey("ConfirmedBy")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PaymentSystem.Data.UserRecord", "User")
                        .WithOne("VerificationTransfer")
                        .HasForeignKey("PaymentSystem.Data.VerificationTransferRecord", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ConfirmedByUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PaymentSystem.Data.RoleRecord", b =>
                {
                    b.Navigation("UserRoleRecords");
                });

            modelBuilder.Entity("PaymentSystem.Data.UserRecord", b =>
                {
                    b.Navigation("Balance")
                        .IsRequired();

                    b.Navigation("FundTransferConfirmedBy");

                    b.Navigation("FundTransferCreatedBy");

                    b.Navigation("FundTransfers");

                    b.Navigation("UserRoleRecords");

                    b.Navigation("VerificationTransfer")
                        .IsRequired();

                    b.Navigation("VerificationTransfersConfirmedBy");
                });
#pragma warning restore 612, 618
        }
    }
}
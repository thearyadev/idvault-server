﻿// <auto-generated />
using System;
using IdVaultServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace idvault_server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241007150926_DocumentImage")]
    partial class DocumentImage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IdVaultServer.Models.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DocumentId"));

                    b.Property<string>("DocumentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("DocumentId");

                    b.HasIndex("UserId");

                    b.ToTable("Documents", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("IdVaultServer.Models.SharedDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DocumentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ReceiverUserId")
                        .HasColumnType("integer");

                    b.Property<int>("SenderUserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SharedDocuments", (string)null);
                });

            modelBuilder.Entity("IdVaultServer.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Email = "test@testuser.com",
                            Name = "User 1",
                            Password = "user1",
                            PhoneNumber = "4169973041",
                            PublicKey = "",
                            Username = "user1"
                        },
                        new
                        {
                            UserId = 2,
                            Email = "user2@user2.com",
                            Name = "User 2",
                            Password = "user2",
                            PhoneNumber = "6473310099",
                            PublicKey = "",
                            Username = "user2"
                        });
                });

            modelBuilder.Entity("IdVaultServer.Models.BirthCertificate", b =>
                {
                    b.HasBaseType("IdVaultServer.Models.Document");

                    b.Property<string>("Birthplace")
                        .HasColumnType("text");

                    b.Property<string>("CertificateNumber")
                        .HasColumnType("text");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("text");

                    b.Property<string>("DateOfRegistration")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("text");

                    b.Property<string>("Sex")
                        .HasColumnType("text");

                    b.ToTable("BirthCertificates", (string)null);
                });

            modelBuilder.Entity("IdVaultServer.Models.DriversLicense", b =>
                {
                    b.HasBaseType("IdVaultServer.Models.Document");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Class")
                        .HasColumnType("text");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("text");

                    b.Property<string>("DriversLicenseNumber")
                        .HasColumnType("text");

                    b.Property<string>("Height")
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .HasColumnType("text");

                    b.Property<string>("Province")
                        .HasColumnType("text");

                    b.Property<string>("Sex")
                        .HasColumnType("text");

                    b.ToTable("DriversLicenses", (string)null);
                });

            modelBuilder.Entity("IdVaultServer.Models.Passport", b =>
                {
                    b.HasBaseType("IdVaultServer.Models.Document");

                    b.Property<string>("Authority")
                        .HasColumnType("text");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Nationality")
                        .HasColumnType("text");

                    b.Property<string>("PlaceOfBirth")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.ToTable("Passports", (string)null);
                });

            modelBuilder.Entity("IdVaultServer.Models.Document", b =>
                {
                    b.HasOne("IdVaultServer.Models.User", "User")
                        .WithMany("Documents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("IdVaultServer.Models.BirthCertificate", b =>
                {
                    b.HasOne("IdVaultServer.Models.Document", null)
                        .WithOne()
                        .HasForeignKey("IdVaultServer.Models.BirthCertificate", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IdVaultServer.Models.DriversLicense", b =>
                {
                    b.HasOne("IdVaultServer.Models.Document", null)
                        .WithOne()
                        .HasForeignKey("IdVaultServer.Models.DriversLicense", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IdVaultServer.Models.Passport", b =>
                {
                    b.HasOne("IdVaultServer.Models.Document", null)
                        .WithOne()
                        .HasForeignKey("IdVaultServer.Models.Passport", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IdVaultServer.Models.User", b =>
                {
                    b.Navigation("Documents");
                });
#pragma warning restore 612, 618
        }
    }
}
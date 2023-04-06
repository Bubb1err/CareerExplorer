﻿// <auto-generated />
using System;
using CareerExplorer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CareerExplorer.Core.Entities.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VacancyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("VacancyId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.JobSeeker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("DesiredPositionId")
                        .HasColumnType("int");

                    b.Property<string>("Experience")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GitHub")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFilled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Views")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DesiredPositionId");

                    b.ToTable("JobSeekers");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.JobSeekerVacancy", b =>
                {
                    b.Property<int>("VacancyId")
                        .HasColumnType("int");

                    b.Property<int>("JobSeekerId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Cv")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsApplied")
                        .HasColumnType("bit");

                    b.HasKey("VacancyId", "JobSeekerId");

                    b.HasIndex("JobSeekerId");

                    b.ToTable("JobSeekerVacancies");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AdminId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Recruiter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFilled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Recruiters");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.SkillsTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AdminId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("SkillsTags");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Vacancy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<int>("PositionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CreatorId");

                    b.HasIndex("PositionId");

                    b.ToTable("Vacancies");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.WorkType", b =>
                {
                    b.Property<int>("WorkTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WorkTypeId"));

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<int>("VacancyId")
                        .HasColumnType("int");

                    b.Property<string>("WorkTypeTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WorkTypeId");

                    b.HasIndex("AdminId");

                    b.HasIndex("VacancyId");

                    b.ToTable("WorkTypes");
                });

            modelBuilder.Entity("JobSeekerSkillsTag", b =>
                {
                    b.Property<int>("JobSeekersId")
                        .HasColumnType("int");

                    b.Property<int>("SkillsId")
                        .HasColumnType("int");

                    b.HasKey("JobSeekersId", "SkillsId");

                    b.HasIndex("SkillsId");

                    b.ToTable("JobSeekerSkillsTag");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SkillsTagVacancy", b =>
                {
                    b.Property<int>("RequirementsId")
                        .HasColumnType("int");

                    b.Property<int>("VacanciesId")
                        .HasColumnType("int");

                    b.HasKey("RequirementsId", "VacanciesId");

                    b.HasIndex("VacanciesId");

                    b.ToTable("SkillsTagVacancy");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.AppUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<int?>("AdminProfileId")
                        .HasColumnType("int");

                    b.Property<int?>("JobSeekerProfileId")
                        .HasColumnType("int");

                    b.Property<int?>("RecruiterProfileId")
                        .HasColumnType("int");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasIndex("AdminProfileId")
                        .IsUnique()
                        .HasFilter("[AdminProfileId] IS NOT NULL");

                    b.HasIndex("JobSeekerProfileId")
                        .IsUnique()
                        .HasFilter("[JobSeekerProfileId] IS NOT NULL");

                    b.HasIndex("RecruiterProfileId")
                        .IsUnique()
                        .HasFilter("[RecruiterProfileId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("AppUser");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Country", b =>
                {
                    b.HasOne("CareerExplorer.Core.Entities.Admin", "Admin")
                        .WithMany("Countries")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CareerExplorer.Core.Entities.Vacancy", null)
                        .WithMany("Countries")
                        .HasForeignKey("VacancyId");

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.JobSeeker", b =>
                {
                    b.HasOne("CareerExplorer.Core.Entities.Position", "DesiredPosition")
                        .WithMany()
                        .HasForeignKey("DesiredPositionId");

                    b.Navigation("DesiredPosition");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.JobSeekerVacancy", b =>
                {
                    b.HasOne("CareerExplorer.Core.Entities.JobSeeker", "JobSeeker")
                        .WithMany("VacanciesApplied")
                        .HasForeignKey("JobSeekerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CareerExplorer.Core.Entities.Vacancy", "Vacancy")
                        .WithMany("Applicants")
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobSeeker");

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Position", b =>
                {
                    b.HasOne("CareerExplorer.Core.Entities.Admin", "Admin")
                        .WithMany("Positions")
                        .HasForeignKey("AdminId");

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.SkillsTag", b =>
                {
                    b.HasOne("CareerExplorer.Core.Entities.Admin", "Admin")
                        .WithMany("Tags")
                        .HasForeignKey("AdminId");

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Vacancy", b =>
                {
                    b.HasOne("CareerExplorer.Core.Entities.Company", null)
                        .WithMany("Vacancies")
                        .HasForeignKey("CompanyId");

                    b.HasOne("CareerExplorer.Core.Entities.Recruiter", "Creator")
                        .WithMany("Vacancies")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("CareerExplorer.Core.Entities.Position", "Position")
                        .WithMany("Vacancies")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.WorkType", b =>
                {
                    b.HasOne("CareerExplorer.Core.Entities.Admin", "Admin")
                        .WithMany("WorkTypes")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CareerExplorer.Core.Entities.Vacancy", "Vacancy")
                        .WithMany()
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("JobSeekerSkillsTag", b =>
                {
                    b.HasOne("CareerExplorer.Core.Entities.JobSeeker", null)
                        .WithMany()
                        .HasForeignKey("JobSeekersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CareerExplorer.Core.Entities.SkillsTag", null)
                        .WithMany()
                        .HasForeignKey("SkillsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SkillsTagVacancy", b =>
                {
                    b.HasOne("CareerExplorer.Core.Entities.SkillsTag", null)
                        .WithMany()
                        .HasForeignKey("RequirementsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CareerExplorer.Core.Entities.Vacancy", null)
                        .WithMany()
                        .HasForeignKey("VacanciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.AppUser", b =>
                {
                    b.HasOne("CareerExplorer.Core.Entities.Admin", "AdminProfile")
                        .WithOne("AppUser")
                        .HasForeignKey("CareerExplorer.Core.Entities.AppUser", "AdminProfileId");

                    b.HasOne("CareerExplorer.Core.Entities.JobSeeker", "JobSeekerProfile")
                        .WithOne("AppUser")
                        .HasForeignKey("CareerExplorer.Core.Entities.AppUser", "JobSeekerProfileId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("CareerExplorer.Core.Entities.Recruiter", "RecruiterProfile")
                        .WithOne("AppUser")
                        .HasForeignKey("CareerExplorer.Core.Entities.AppUser", "RecruiterProfileId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("AdminProfile");

                    b.Navigation("JobSeekerProfile");

                    b.Navigation("RecruiterProfile");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Admin", b =>
                {
                    b.Navigation("AppUser")
                        .IsRequired();

                    b.Navigation("Countries");

                    b.Navigation("Positions");

                    b.Navigation("Tags");

                    b.Navigation("WorkTypes");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Company", b =>
                {
                    b.Navigation("Vacancies");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.JobSeeker", b =>
                {
                    b.Navigation("AppUser")
                        .IsRequired();

                    b.Navigation("VacanciesApplied");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Position", b =>
                {
                    b.Navigation("Vacancies");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Recruiter", b =>
                {
                    b.Navigation("AppUser")
                        .IsRequired();

                    b.Navigation("Vacancies");
                });

            modelBuilder.Entity("CareerExplorer.Core.Entities.Vacancy", b =>
                {
                    b.Navigation("Applicants");

                    b.Navigation("Countries");
                });
#pragma warning restore 612, 618
        }
    }
}

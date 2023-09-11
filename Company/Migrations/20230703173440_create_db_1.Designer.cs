﻿// <auto-generated />
using System;
using Company.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Company.Migrations
{
    [DbContext(typeof(CompanyContext))]
    [Migration("20230703173440_create_db_1")]
    partial class create_db_1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Company.Models.Department.DepartmentModel", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DepartmentImageLink")
                        .HasColumnType("longtext");

                    b.Property<string>("DepartmentName")
                        .HasColumnType("longtext");

                    b.Property<int?>("ParentDepartmentID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            DepartmentImageLink = "/images/DepartmentImg/customer service department.jpg",
                            DepartmentName = "Отдел по обслуживанию клиентов"
                        },
                        new
                        {
                            ID = 2,
                            DepartmentImageLink = "/images/DepartmentImg/production department.jpg",
                            DepartmentName = "Производственный отдел"
                        },
                        new
                        {
                            ID = 3,
                            DepartmentImageLink = "/images/DepartmentImg/bookkeeping.jpg",
                            DepartmentName = "Бухгалтерия"
                        },
                        new
                        {
                            ID = 4,
                            DepartmentImageLink = "/images/DepartmentImg/sales department.jpg",
                            DepartmentName = "Отдел продаж",
                            ParentDepartmentID = 1
                        },
                        new
                        {
                            ID = 5,
                            DepartmentImageLink = "/images/DepartmentImg/wholesale department.jpg",
                            DepartmentName = "Отдел оптовых продаж",
                            ParentDepartmentID = 4
                        },
                        new
                        {
                            ID = 6,
                            DepartmentImageLink = "/images/DepartmentImg/retail sales department.jpg",
                            DepartmentName = "Отдел розничных продаж",
                            ParentDepartmentID = 4
                        },
                        new
                        {
                            ID = 7,
                            DepartmentImageLink = "/images/DepartmentImg/logistics department.jpg",
                            DepartmentName = "Отдел логистики",
                            ParentDepartmentID = 1
                        },
                        new
                        {
                            ID = 8,
                            DepartmentImageLink = "/images/DepartmentImg/stock.jpg",
                            DepartmentName = "Склад",
                            ParentDepartmentID = 7
                        },
                        new
                        {
                            ID = 9,
                            DepartmentImageLink = "/images/DepartmentImg/bookkeeping.jpg",
                            DepartmentName = "Отдел доставки",
                            ParentDepartmentID = 7
                        },
                        new
                        {
                            ID = 10,
                            DepartmentImageLink = "/images/DepartmentImg/engineering department.jpg",
                            DepartmentName = "Инженерный отдел",
                            ParentDepartmentID = 2
                        },
                        new
                        {
                            ID = 11,
                            DepartmentImageLink = "/images/DepartmentImg/quality control department.jpg",
                            DepartmentName = "Отдел контроля качества",
                            ParentDepartmentID = 2
                        },
                        new
                        {
                            ID = 12,
                            DepartmentImageLink = "/images/DepartmentImg/purchasing department.jpg",
                            DepartmentName = "Отдел закупок",
                            ParentDepartmentID = 2
                        });
                });

            modelBuilder.Entity("Company.Models.Department.NumberOfEmployee", b =>
                {
                    b.Property<int?>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EmployeeCount")
                        .HasColumnType("int");

                    b.HasKey("DepartmentID");

                    b.ToTable("NumberOfEmployees");
                });

            modelBuilder.Entity("Company.Models.Employee.EmployeeModel", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Age")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("DepartmentID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Age = "28",
                            DepartmentID = 3,
                            Name = "Алексей",
                            Number = "+79123456789",
                            Surname = "Иванов"
                        },
                        new
                        {
                            ID = 2,
                            Age = "32",
                            DepartmentID = 3,
                            Name = "Екатерина",
                            Number = "+79123456780",
                            Surname = "Смирнова"
                        },
                        new
                        {
                            ID = 3,
                            Age = "21",
                            DepartmentID = 5,
                            Name = "Дмитрий",
                            Number = "+79123456781",
                            Surname = "Козлов"
                        },
                        new
                        {
                            ID = 4,
                            Age = "35",
                            DepartmentID = 5,
                            Name = "Анна",
                            Number = "+79123456782",
                            Surname = "Петрова"
                        },
                        new
                        {
                            ID = 5,
                            Age = "43",
                            DepartmentID = 6,
                            Name = "Сергей",
                            Number = "+79123456783",
                            Surname = "Михайлов"
                        },
                        new
                        {
                            ID = 6,
                            Age = "26",
                            DepartmentID = 6,
                            Name = "Ольга",
                            Number = "+79123456784",
                            Surname = "Соколова"
                        },
                        new
                        {
                            ID = 7,
                            Age = "29",
                            DepartmentID = 8,
                            Name = "Иван",
                            Number = "+79123456785",
                            Surname = "Новиков"
                        },
                        new
                        {
                            ID = 8,
                            Age = "31",
                            DepartmentID = 8,
                            Name = "Анастасия",
                            Number = "+79123456786",
                            Surname = "Федорова"
                        },
                        new
                        {
                            ID = 9,
                            Age = "40",
                            DepartmentID = 9,
                            Name = "Александр",
                            Number = "+79123456787",
                            Surname = "Морозов"
                        },
                        new
                        {
                            ID = 10,
                            Age = "27",
                            DepartmentID = 9,
                            Name = "Юлия",
                            Number = "+79123456788",
                            Surname = "Волкова"
                        },
                        new
                        {
                            ID = 11,
                            Age = "33",
                            DepartmentID = 9,
                            Name = "Михаил",
                            Number = "+79123456777",
                            Surname = "Алексеев"
                        },
                        new
                        {
                            ID = 12,
                            Age = "45",
                            DepartmentID = 9,
                            Name = "Елена",
                            Number = "+79123456776",
                            Surname = "Лебедева"
                        },
                        new
                        {
                            ID = 13,
                            Age = "39",
                            DepartmentID = 9,
                            Name = "Андрей",
                            Number = "+79123456775",
                            Surname = "Семенов"
                        },
                        new
                        {
                            ID = 14,
                            Age = "23",
                            DepartmentID = 10,
                            Name = "Мария",
                            Number = "+79123456774",
                            Surname = "Егорова"
                        },
                        new
                        {
                            ID = 15,
                            Age = "41",
                            DepartmentID = 10,
                            Name = "Владимир",
                            Number = "+79123456773",
                            Surname = "Павлов"
                        },
                        new
                        {
                            ID = 16,
                            Age = "30",
                            DepartmentID = 10,
                            Name = "Евгения",
                            Number = "+79123456772",
                            Surname = "Ковалева"
                        },
                        new
                        {
                            ID = 17,
                            Age = "37",
                            DepartmentID = 10,
                            Name = "Николай",
                            Number = "+79123456771",
                            Surname = "Орлов"
                        },
                        new
                        {
                            ID = 18,
                            Age = "34",
                            DepartmentID = 11,
                            Name = "Татьяна",
                            Number = "+79123456770",
                            Surname = "Андреева"
                        },
                        new
                        {
                            ID = 19,
                            Age = "42",
                            DepartmentID = 11,
                            Name = "Павел",
                            Number = "+79123456769",
                            Surname = "Макаров"
                        },
                        new
                        {
                            ID = 20,
                            Age = "22",
                            DepartmentID = 12,
                            Name = "Алиса",
                            Number = "+79123456768",
                            Surname = "Николаева"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<string>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("IdentityRole<string>");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole<string>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser<string>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("IdentityUser<string>");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser<string>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRole<string>");

                    b.HasDiscriminator().HasValue("IdentityRole");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            ConcurrencyStamp = "39004f6a-07db-4125-a41a-c0b255bc19af",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "2",
                            ConcurrencyStamp = "ae254dfe-d00c-4bbb-aeb7-3d8745953bc4",
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "3",
                            ConcurrencyStamp = "b8eeceb9-e2da-4aae-ae75-efaec7986796",
                            Name = "Manager",
                            NormalizedName = "MANAGER"
                        });
                });

            modelBuilder.Entity("Company.Models.ApplicationUserModel", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser<string>");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("ApplicationUserModel");
                });
#pragma warning restore 612, 618
        }
    }
}

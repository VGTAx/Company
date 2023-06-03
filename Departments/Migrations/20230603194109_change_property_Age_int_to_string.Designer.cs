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
    [DbContext(typeof(DepartmentContext))]
    [Migration("20230603194109_change_property_Age_int_to_string")]
    partial class change_property_Age_int_to_string
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Company.Models.Department", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

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
                            DepartmentName = "Отдел по обслуживанию клиентов"
                        },
                        new
                        {
                            ID = 2,
                            DepartmentName = "Производственный отдел"
                        },
                        new
                        {
                            ID = 3,
                            DepartmentName = "Бухгалтерия"
                        },
                        new
                        {
                            ID = 4,
                            DepartmentName = "Отдел продаж",
                            ParentDepartmentID = 1
                        },
                        new
                        {
                            ID = 5,
                            DepartmentName = "Отдел оптовых продаж",
                            ParentDepartmentID = 4
                        },
                        new
                        {
                            ID = 6,
                            DepartmentName = "Отдел розничных продаж",
                            ParentDepartmentID = 4
                        },
                        new
                        {
                            ID = 7,
                            DepartmentName = "Отдел логистики",
                            ParentDepartmentID = 1
                        },
                        new
                        {
                            ID = 8,
                            DepartmentName = "Склад",
                            ParentDepartmentID = 7
                        },
                        new
                        {
                            ID = 9,
                            DepartmentName = "Отдел доставки",
                            ParentDepartmentID = 7
                        },
                        new
                        {
                            ID = 10,
                            DepartmentName = "Инженерный отдел",
                            ParentDepartmentID = 2
                        },
                        new
                        {
                            ID = 11,
                            DepartmentName = "Отдел проверки качества",
                            ParentDepartmentID = 2
                        },
                        new
                        {
                            ID = 12,
                            DepartmentName = "Отдел закупок",
                            ParentDepartmentID = 2
                        });
                });

            modelBuilder.Entity("Company.Models.Employee", b =>
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

            modelBuilder.Entity("Company.Models.NumberOfEmployee", b =>
                {
                    b.Property<int?>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EmployeeCount")
                        .HasColumnType("int");

                    b.HasKey("DepartmentID");

                    b.ToTable("NumberOfEmployees");
                });
#pragma warning restore 612, 618
        }
    }
}

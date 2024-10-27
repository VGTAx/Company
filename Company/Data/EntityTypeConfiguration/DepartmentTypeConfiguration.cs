using Company.Models.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Data.EntityTypeConfiguration
{
  public sealed class DepartmentTypeConfiguration : IEntityTypeConfiguration<DepartmentModel>
  {
    public void Configure(EntityTypeBuilder<DepartmentModel> builder)
    {
      builder.HasKey(d => d.ID);
      builder.HasData(
           new DepartmentModel(Department.CustomerService, "Отдел по обслуживанию клиентов", null),
           new DepartmentModel(Department.Production, "Производственный отдел", null),
           new DepartmentModel(Department.Accounting, "Бухгалтерия", null),
           new DepartmentModel(Department.Sales, "Отдел продаж", Department.CustomerService),
           new DepartmentModel(Department.Wholesales, "Отдел оптовых продаж", Department.Wholesales),
           new DepartmentModel(Department.RetailSales, "Отдел розничных продаж", Department.Wholesales),
           new DepartmentModel(Department.Logistic, "Отдел логистики", Department.CustomerService),
           new DepartmentModel(Department.Warehouse, "Склад", Department.Logistic),
           new DepartmentModel(Department.Delivering, "Отдел доставки", Department.Logistic),
           new DepartmentModel(Department.Engineering, "Инженерный отдел", Department.Production),
           new DepartmentModel(Department.QualityControl, "Отдел контроля качества", Department.Production),
           new DepartmentModel(Department.Purchasing, "Отдел закупок", Department.Production)
          );
    }
  }
}
using Company.Models.Department;
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
           new DepartmentModel(1, "Отдел по обслуживанию клиентов", null),
           new DepartmentModel(2, "Производственный отдел", null),
           new DepartmentModel(3, "Бухгалтерия", null),
           new DepartmentModel(4, "Отдел продаж", 1),
           new DepartmentModel(5, "Отдел оптовых продаж", 4),
           new DepartmentModel(6, "Отдел розничных продаж", 4),
           new DepartmentModel(7, "Отдел логистики", 1),
           new DepartmentModel(8, "Склад", 7),
           new DepartmentModel(9, "Отдел доставки", 7),
           new DepartmentModel(10, "Инженерный отдел", 2),
           new DepartmentModel(11, "Отдел контроля качества", 2),
           new DepartmentModel(12, "Отдел закупок", 2)
          );
    }
  }
}
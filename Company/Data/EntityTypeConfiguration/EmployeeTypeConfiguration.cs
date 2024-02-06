using Company.Models.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Data.EntityTypeConfiguration
{
  public sealed class EmployeeTypeConfiguration : IEntityTypeConfiguration<EmployeeModel>
  {
    public void Configure(EntityTypeBuilder<EmployeeModel> builder)
    {
      builder.HasData(
          new EmployeeModel(1, "Алексей", "Иванов", "28", "+79123456789", 3),
          new EmployeeModel(2, "Екатерина", "Смирнова", "32", "+79123456780", 3),
          new EmployeeModel(3, "Дмитрий", "Козлов", "21", "+79123456781", 5),
          new EmployeeModel(4, "Анна", "Петрова", "35", "+79123456782", 5),
          new EmployeeModel(5, "Сергей", "Михайлов", "43", "+79123456783", 6),
          new EmployeeModel(6, "Ольга", "Соколова", "26", "+79123456784", 6),
          new EmployeeModel(7, "Иван", "Новиков", "29", "+79123456785", 8),
          new EmployeeModel(8, "Анастасия", "Федорова", "31", "+79123456786", 8),
          new EmployeeModel(9, "Александр", "Морозов", "40", "+79123456787", 9),
          new EmployeeModel(10, "Юлия", "Волкова", "27", "+79123456788", 9),
          new EmployeeModel(11, "Михаил", "Алексеев", "33", "+79123456777", 9),
          new EmployeeModel(12, "Елена", "Лебедева", "45", "+79123456776", 9),
          new EmployeeModel(13, "Андрей", "Семенов", "39", "+79123456775", 9),
          new EmployeeModel(14, "Мария", "Егорова", "23", "+79123456774", 10),
          new EmployeeModel(15, "Владимир", "Павлов", "41", "+79123456773", 10),
          new EmployeeModel(16, "Евгения", "Ковалева", "30", "+79123456772", 10),
          new EmployeeModel(17, "Николай", "Орлов", "37", "+79123456771", 10),
          new EmployeeModel(18, "Татьяна", "Андреева", "34", "+79123456770", 11),
          new EmployeeModel(19, "Павел", "Макаров", "42", "+79123456769", 11),
          new EmployeeModel(20, "Алиса", "Николаева", "22", "+79123456768", 12)
          );

      builder.Property(e => e.ID)
             .ValueGeneratedOnAdd()
             .UseMySqlIdentityColumn();
    }
  }
}

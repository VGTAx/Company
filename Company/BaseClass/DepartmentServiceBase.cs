using Microsoft.AspNetCore.Mvc.Rendering;

namespace Company.BaseClass
{
  public abstract class DepartmentServiceBase<T> : ErrorMessageBase
  {
    public abstract Task<T> GetDepartmentAsync(int? id);
    public abstract IEnumerable<SelectListItem> GetDepartmentsListItem();
    public abstract Task<IEnumerable<T>> GetDepartmentsAsync();
  }
}

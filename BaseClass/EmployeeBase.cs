namespace Company.BaseClass
{
  public abstract class EmployeeBase<TEmployee> : ErrorMessageBase
  {
    public abstract Task<List<TEmployee>> GetEmployeesAsync();
    public abstract Task<TEmployee> GetEmployeeAsync(int? id);
    public abstract Task<bool> IsEmployeeExist(int? id);
  }
}

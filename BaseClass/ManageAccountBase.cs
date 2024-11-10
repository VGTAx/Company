namespace Company.BaseClass
{
  public abstract class ManageAccountBase<T> : ErrorMessageBase
  {
    public abstract Task<string> GenerateChangeEmailTokenAsync(T user, string email);
  }
}

namespace Company.Interfaces
{
  /// <summary>
  /// 
  /// </summary>
  public interface ICheckEmailService
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<bool> CheckIsEmailExistAsync(string email);
  }
}

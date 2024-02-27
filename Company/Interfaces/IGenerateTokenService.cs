namespace Company.Interfaces
{
  public interface IGenerateTokenService<T>
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<string> GenereatePasswordResetTokenAsync(T user);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<string> GenereateEmailConfirmationTokenAsync(T user);
  }
}

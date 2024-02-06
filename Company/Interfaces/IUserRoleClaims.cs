namespace Company.Interfaces
{
  /// <summary>
  /// Интерфейс для работы с role claims пользователей.
  /// </summary>
  /// <typeparam name="T">Тип объекта, представляющего пользователя.</typeparam>
  public interface IUserRoleClaims<T>
  {
    /// <summary>
    /// Возвращает список role claims пользователя
    /// </summary>
    /// <param name="user">Текущий пользователь</param>
    /// <returns>Список role claims пользователя</returns>
    Task<List<string>> GetUserRoleClaimsAsync(T user);
    /// <summary>
    /// Меняет role claims у пользователя
    /// </summary>
    /// <param name="user">Пользователь у которого role claims</param>
    /// <param name="userRoles">Текущий список role claims пользователя</param>
    /// <param name="newUserRoles">Новый список role claims пользователя</param>
    /// <returns></returns>
    Task ChangeUserRoleClaimsAsync(T user, List<string> userRoles, List<string> newUserRoles);
  }
}

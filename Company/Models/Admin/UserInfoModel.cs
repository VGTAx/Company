namespace Company.Models.Admin
{
  /// <summary>
  /// Модель информации о пользователе.
  /// </summary>
  public class UserInfoModel
  {
    /// <summary>
    /// Список выбранных ролей пользователя.
    /// </summary>
    public List<string>? SelectedRoles { get; set; }
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public string? Id { get; set; }
  }
}

namespace Company.Models
{
  /// <summary>
  /// Модель данных для настройки SMTP
  /// </summary>
  public class SmtpSettings
  {
    /// <summary>
    /// Имя хоста
    /// </summary>
    public string? Host { get; set; }
    /// <summary>
    /// Порт хоста
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    /// Электронная почта отправителя
    /// </summary>
    public string? Email { get; set; }
    /// <summary>
    /// Пароль электронной почты
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    /// Имя отправителя
    /// </summary>
    public string? SenderName { get; set; }
  }
}

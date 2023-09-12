namespace Company.IServices
{
  /// <summary>
  /// Интерфейс для сервиса уведомлений.
  /// </summary>
  public interface INotificationService
  {
    /// <summary>
    /// Отправляет уведомление по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор уведомления.</param>
    public void SendNotification(string id);
    /// <summary>
    /// Удаляет уведомление по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор уведомления.</param>
    public void RemoveNotification(string id);
    /// <summary>
    /// Проверяет наличие уведомления по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор уведомления.</param>
    /// <returns>True, если уведомление существует, иначе False.</returns>
    public bool HasNotification(string id);
  }
}

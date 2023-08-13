namespace Company_.IServices
{
  public interface INotificationService
  {
    public void SendNotification(string id);
    public void RemoveNotification(string id);
    public bool HasNotification(string id);
  }
}

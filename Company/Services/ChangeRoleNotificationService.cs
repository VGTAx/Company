using Company.IServices;

namespace Company.Services
{
  public class ChangeRoleNotificationService : INotificationService
  {
    private HashSet<string> _usersId = new HashSet<string>();

    public ChangeRoleNotificationService(HashSet<string> usersId)
    {
      _usersId = usersId;
    }
    public ChangeRoleNotificationService() { }

    public bool HasNotification(string id)
    {
      return _usersId.Contains(id);
    }

    public void RemoveNotification(string id)
    {
      _usersId.Remove(id);
    }

    public void SendNotification(string id)
    {
      _usersId.Add(id);
    }
  }
}

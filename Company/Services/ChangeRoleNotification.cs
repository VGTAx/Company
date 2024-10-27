using Company.IServices;

namespace Company.Services
{
  public class ChangeRoleNotification : INotification
  {
    private readonly HashSet<string> _usersId = new HashSet<string>();

    public ChangeRoleNotification(HashSet<string> usersId)
    {
      _usersId = usersId;
    }
    public ChangeRoleNotification() { }

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

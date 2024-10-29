using System.Text;

namespace Company.Services;

public class StringParser
{
  public StringParser() { }
  public string CollectionToString(IEnumerable<string> errors)
  {
    var sb = new StringBuilder();

    foreach(var error in errors)
    {
      sb.Append(error);
      sb.Append("; ");
    }

    return sb.ToString();
  }
}

using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Company.TagHelpers
{
  /// <summary>
  /// Класс EmailTagHelper представляет собой Tag Helper для создания HTML-ссылки на электронную почту
  /// </summary>
  public class EmailTagHelper : TagHelper
  {
    /// <summary>
    /// Представляет аттрибут для хранения адреса email
    /// </summary>
    public string? Adress { get; set; }
    /// <summary>
    /// Представляет аттрибут Value
    /// </summary>
    public string? Value { get; set; }
    /// <summary>
    /// Представляет аттрибут Class
    /// </summary>
    public string? Class { get; set; }
    /// <summary>
    /// Представляет аттрибут id
    /// </summary>
    public string? Id { get; set; }
    /// <summary>
    /// Метод генерирует html элемент <![CDATA[<a>]]> на основе тега <![CDATA[<email>]]>
    /// </summary>
    /// <param name="context">Контекст тега (его содержимое, атрибуты)</param>
    /// <param name="output">Генерирует html элемент на основе тега</param>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      output.TagName = "a";
      output.Attributes.SetAttribute("href", "mailto:" + Adress);
      output.Attributes.SetAttribute("class", Class);
      output.Attributes.SetAttribute("id", Id);
      output.Content.SetContent(Value);
    }
  }
}

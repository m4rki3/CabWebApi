using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CabWebApi.Content.Swagger;
public class EnumXmlSchemaFilter : ISchemaFilter
{
	private const string path = "wwwroot/xml/Enums.xml";
	public EnumXmlSchemaFilter()
	{
		using FileStream stream = File.Open(
			path,
			FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite
		);
		try
		{
			XDocument.Load(stream);
			return;
		}
		catch (Exception) { }
		XDocument doc = new();
		XElement root = new("enums");
		doc.Add(root);

		Assembly core = Assembly.Load("CabWebApi.Domain.Core");
		foreach (var type in core.GetTypes())
		{
			if (!type.IsEnum) continue;
			string enumName = type.Name;
			XElement @enum = new(
				"enum", new XAttribute("name", enumName)
			);
			foreach (var member in Enum.GetValues(type))
			{
				int memberConst = Convert.ToInt32(member);
				@enum.Add(
					new XElement("member",
						new XElement("const", memberConst),
						new XElement("string", member)
					)
				);
			}
			root.Add(@enum);
		}
		doc.Save(stream);
	}
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (!context.Type.IsEnum) return;

		string? enumName = context.Type.Name;
		XDocument doc = XDocument.Load(path);
		StringBuilder builder = new("<p>Contains enum values</p>");
		XElement? enumElement = doc.Root
								   ?.XPathSelectElement(
									   $"enum[@name='{enumName}']"
								   );
		if (enumElement is null) return;
		builder.Append("<ul>");
		IEnumerable<XElement> members = enumElement.Elements();
		foreach (var member in members)
		{
			string? constant = member.Descendants("const")
									 .SingleOrDefault()
									 ?.Value;
			string? str = member.Descendants("string")
								.SingleOrDefault()
								?.Value;
			builder.Append($"<li>{constant} - {str}</li>");
		}
		builder.Append("</ul>");
		schema.Description += builder.ToString();
	}
}
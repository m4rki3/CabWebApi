using CabWebApi.Domain.Core;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CabWebApi.Content.Swagger;
public class EnumSchemaFilter : ISchemaFilter
{
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (context.Type.IsEnum)
		{
			StringBuilder builder = new("<p>Contains enum values</p><ul>");
			foreach (var enumMember in Enum.GetValues(context.Type))
			{
				string? memberName = Enum.GetName(context.Type, enumMember);
				int memberValue = Convert.ToInt32(enumMember);
				builder.Append($"<li>{memberValue} - {memberName}</li>");
			}
			builder.Append("</ul>");
			schema.Description += builder.ToString();
		}
	}
}

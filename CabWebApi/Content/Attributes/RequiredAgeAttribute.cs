using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Content.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class RequiredAgeAttribute : ValidationAttribute
{
	private double bottomDaysBorder;
	private double topDaysBorder;
	public RequiredAgeAttribute(byte bottomAgeBorder, byte topAgeBorder)
	{
		bottomDaysBorder = bottomAgeBorder * 365.2425;
		topDaysBorder = topAgeBorder * 365.2425;
	}
	public override bool IsValid(object? value)
	{
		if (value != null && value is DateTime)
		{
			DateTime date = (DateTime)value;
			TimeSpan age = DateTime.Now - date;
			if (age > TimeSpan.FromDays(bottomDaysBorder) &&
				age < TimeSpan.FromDays(topDaysBorder))
				return true;
		}
		return false;
	}
}

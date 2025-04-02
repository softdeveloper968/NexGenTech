

using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Extensions
{
	public static class TypeExtensions
	{
		public static List<Type> OrderByImportOrder(this List<Type> types)
		{
			return types.OrderBy(type => GetImportOrder(type)).ToList();
		}

		private static int GetImportOrder(Type type)
		{
			var attribute = (ImportOrderAttribute?)Attribute.GetCustomAttribute(type, typeof(ImportOrderAttribute));
			return attribute?.ImportOrder ?? int.MaxValue; // Default to int.MaxValue if the attribute is not present
		}
	}
}

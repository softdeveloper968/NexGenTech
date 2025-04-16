
namespace MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class ImportOrderAttribute : Attribute
	{
		public int ImportOrder { get; set; }

		public ImportOrderAttribute(int importOrder = 999)
		{
			ImportOrder = importOrder;
		}	
	}
}

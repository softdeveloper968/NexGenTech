namespace MedHelpAuthorizations.Infrastructure.DataPipe.Entities;

public partial class TblMapping 
{
	public TblMapping() { }
	public TblMapping(string tableSchema, string tableName, string mapping, string tableId, string source, string sourceFileName) 
	{
		TableSchema = tableSchema;
		TableName = tableName;
		Mapping = mapping;
		TableId = tableId;
		Source = source;
		SourceFileName = sourceFileName;
	}
	public string? TableSchema { get; set; }

	public string? TableName { get; set; }

	public string? Mapping { get; set; }

	public string? TableId { get; set; }

	public string? Source { get; set; }

	public string? SourceFileName { get; set; }

	//public string? TenantClientString { get; set; }
}

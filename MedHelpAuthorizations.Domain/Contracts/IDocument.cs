using System;

namespace MedHelpAuthorizations.Domain.Contracts
{
    public interface IDocument : ISoftDelete
    {
        public string Title { get; set; }
        public string Description { get; set; } 
        public bool IsPublic { get; set; }
        public string URL { get; set; }
        public DateTime? DocumentDate { get; set; }
    }
}

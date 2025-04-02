using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

#nullable disable

namespace MedHelpAuthorizations.Domain.Entities
{
    public partial class Authorization : AuditableEntity<int>//, ITenant
    {
        public Authorization()
        {
            Notes = new HashSet<Note>();
            Documents = new HashSet<Document>();
            InitialAuthorizations = new HashSet<ConcurrentAuthorization>();
            SucceededAuthorizations = new HashSet<ConcurrentAuthorization>();
            AuthorizationClientCptCodes = new HashSet<AuthorizationClientCptCode>();
        }

        public int AuthTypeId { get; set; }
        
        public int PatientId { get; set; }
        
        public DateTime? CompleteDate { get; set; }
        
        public string Completeby { get; set; }
        
        public string AuthNumber { get; set; }
        
        public int Units { get; set; } = 1;
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public DateTime? DischargedOn { get; set; }
        
        public string DischargedBy { get; set; }
        
        public DateTime? CallbackDate { get; set; }
        
        public string CareManagerName { get; set; }
        
        public int ClientId { get; set; }
        
        public AuthorizationStatusEnum AuthorizationStatusId { get; set; }
        
        public int? ClientLocationId { get; set; } 
        
        public int? ClientPlaceOfServiceId { get; set; }

        //public string TenantId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        

        [ForeignKey("ClientLocationId")]
        public virtual ClientLocation ClientLocation { get; set; }
        

        [ForeignKey("ClientPlaceOfServiceId")]
        public virtual ClientPlaceOfService ClientPlaceOfService { get; set; }

        [ForeignKey("AuthTypeId")]
        public virtual AuthType AuthType { get; set; }
        

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        
        public virtual ICollection<Note> Notes { get; set; }
        
        public virtual ICollection<AuthorizationClientCptCode> AuthorizationClientCptCodes { get; set; }
        

        [ForeignKey("AuthorizationStatusId")]
        public virtual AuthorizationStatus AuthorizationStatus { get; set; }
        
        public virtual ICollection<Document> Documents { get; set; }
        

        [InverseProperty(nameof(ConcurrentAuthorization.InitialAuthorization))]
        public virtual ICollection<ConcurrentAuthorization> InitialAuthorizations { get; set; }
        
        
        [InverseProperty(nameof(ConcurrentAuthorization.SucceededAuthorization))]
        public virtual ICollection<ConcurrentAuthorization> SucceededAuthorizations { get; set; }
    }
}

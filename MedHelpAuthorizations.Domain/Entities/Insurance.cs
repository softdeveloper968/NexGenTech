using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Insurance : AuditableEntity
    {
        [StringLength(12)]
        public string LookupName { get; set; }

        [StringLength(25)]
        public string Name { get; set; }
        public int? InsuranceCategoryId { get; set; }
        public long? PhoneNumber { get; set; }
        public long? FaxNumber { get; set; }

        [StringLength(12)]
        public string ExternalId { get; set; }

        public string EcsId { get; set; }
        
        public int? AddressId { get; set; }


        #region Navigation Objects


        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }

        #endregion
    }
}

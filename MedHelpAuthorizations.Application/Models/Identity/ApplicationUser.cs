using MedHelpAuthorizations.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Application.Models.Identity
{
    public class ApplicationUser : IdentityUser, IAuditableEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string CreatedBy { get; set; }

        [Column(TypeName = "text")]
        public string ProfilePictureDataUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }

        public string Otp { get; set; }


        [Required]
		[StringLength(12)]
		public string Pin { get; set; }    //EN-87 
        public DateTime RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<UsedPassword> UsedPasswords { get; set; }
        public virtual ICollection<UserLoginHistory> UserLoginHistory { get; set; }

        //public virtual ICollection<ChatHistory> ChatHistoryFromUsers { get; set; }
        //public virtual ICollection<ChatHistory> ChatHistoryToUsers { get; set; }

        public ApplicationUser()
        {
            UsedPasswords = new HashSet<UsedPassword>();
            UserLoginHistory = new HashSet<UserLoginHistory>();
            //ChatHistoryFromUsers = new HashSet<ChatHistory>();    
            //ChatHistoryToUsers = new HashSet<ChatHistory>();
        }        
    }
}
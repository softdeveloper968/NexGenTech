using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Represents a user's customized dashboard item with additional metadata.
    /// </summary>
    public class UserDashboardItem : AuditableEntity<int>
    {
        /// <summary>
        /// Gets or sets the identifier of the associated dashboard item.
        /// </summary>
        public int DashboardItemId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier associated with this dashboard item.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the order in which this dashboard item should be displayed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this dashboard item is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the related dashboard item.
        /// </summary>
        [ForeignKey(nameof(DashboardItemId))]
        public virtual DashboardItem DashboardItem { get; set; }
    }

}

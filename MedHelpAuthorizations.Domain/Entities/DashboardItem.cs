using MedHelpAuthorizations.Domain.Contracts;
using System;
using static MedHelpAuthorizations.Shared.Enums.CustomDashboardEnums;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Represents a dashboard item with customizable properties.
    /// </summary>
    public class DashboardItem : AuditableEntity<int>
    {
        /// <summary>
        /// Gets or sets the name of the dashboard item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the selector for the dashboard item.
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// Gets or sets the order in which this dashboard item should be displayed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this dashboard item can be dragged.
        /// </summary>
        public bool CanDrag { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether this dashboard item needs a layout filter.
        /// </summary>
        public bool NeedsLayoutFilter { get; set; } = false;

        /// <summary>
        /// Gets or sets the icon associated with this dashboard item.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the title of the component related to this dashboard item.
        /// </summary>
        public string ComponentTitle { get; set; }

        /// <summary>
        /// Gets or sets the category of this dashboard item.
        /// </summary>
        public ItemCategoryEnum Category { get; set; }

        /// <summary>
        /// Gets or sets the category of the dashboard to which this item belongs.
        /// </summary>
        public DashboardCategoryEnum Dashboard { get; set; }

        /// <summary>
        /// Gets or sets the layout category associated with this dashboard item.
        /// </summary>
        public LayoutCategoryEnum Layout { get; set; }
    }

}

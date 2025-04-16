using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes;
using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.AddEdit
{
    public class AddEditFeeScheduleEntryViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ClientCptCodeId { get; set; }
        public int ClientFeeScheduleId { get; set; }
        public decimal? Fee { get; set; }

		public decimal? _allowedAmount { get; set; }
		public decimal? AllowedAmount
		{
			get { return _allowedAmount; }
			set
			{
				if (_allowedAmount != value)
				{
					_allowedAmount = value;

					// Check if _reimbursablePercentage is greater than 0.00
					if (_allowedAmount.HasValue && _allowedAmount.Value > 0.00m)
					{
						ReimbursablePercentage = null;
					}
				}
			}
		}

		public decimal? _reimbursablePercentage;
        public decimal? ReimbursablePercentage
        {
            get { return _reimbursablePercentage; }
            set
            {
                if (_reimbursablePercentage != value)
                {
                    _reimbursablePercentage = value;

                    // Check if _reimbursablePercentage is greater than 0.00
                    if (_reimbursablePercentage.HasValue && _reimbursablePercentage.Value > 0.00m)
                    {
                        AllowedAmount = null;
                    }
                }
            }
        }

        public string ReimbursableType { get; set; } //EN-70
		public bool IsReimbursable { get; set; } = true;
        public ClientCptCodeDto ClientCptCode { get; set; } = new ClientCptCodeDto();
        public List<ClientCptCodeDto> SelectedCptCode { get; set;}
    }
}

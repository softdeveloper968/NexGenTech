using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Base
{
	public class GetAllCaimStatusBatchClaimResponse
	{
		/// <summary>
		/// Gets or sets the list of ClaimStatusBatchClaim entities.
		/// </summary>
		public List<ClaimStatusBatchClaim> ClaimStatusBatchClaims { get; set; } //AA-231
	}
}

using MedHelpAuthorizations.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ClaimNumberNormalizationServices : IClaimNumberNormalizationService
    {
        // EN-35
        /// <summary>
        /// Extracts the normalized claim number from a given claim number.
        /// </summary>
        /// <param name="claimNumber">The claim number to extract the normalized claim number from.</param>
        /// <returns>The normalized claim number if it exists, or the original claim number if not.</returns>
        public async Task<string> GetNormalizedClaimNumber(string claimNumber)
        {
            try
            {
                //TODO : need to add cases to handle SourceSystem formatting

                // Check if the claim number contains a dot and has a length of two characters after the dot
                if (claimNumber.Contains(".") && Decimal.TryParse(claimNumber, out decimal parsedClaimNumber))
                {
                    // Extract and return the part of the claim number before the dot as the normalized claim number
                    return claimNumber.Split(".")[0];
                }

                // Return the original claim number if it does not meet the criteria for a normalized claim number
                return claimNumber;
            }
            catch (Exception ex)
            {
                // Handle any exceptions and potentially log them
                throw;
            }
        }

    }
}

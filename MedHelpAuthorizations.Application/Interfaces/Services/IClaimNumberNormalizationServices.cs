using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IClaimNumberNormalizationService
    {
        /// <summary>
        /// Extracts the normalized claim number from a given claim number.
        /// </summary>
        /// <param name="claimNumber">The claim number to extract the normalized claim number from.</param>
        /// <returns>The normalized claim number if it exists, or the original claim number if not.</returns>

        Task<string> GetNormalizedClaimNumber(string claimNumber); //EN-35
    }
}

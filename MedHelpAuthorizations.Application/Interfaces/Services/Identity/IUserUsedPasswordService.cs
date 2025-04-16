using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Interfaces.Services.Identity
{
    public interface IUserUsedPasswordService : IService
    {
        Task<IResult> AddUsedPasswordAsync(string userId, string hashedPassword); //EN-178
        Task<bool> RecordUserPasswordAsync(string userId, string newPassword); //EN-178
    }
}

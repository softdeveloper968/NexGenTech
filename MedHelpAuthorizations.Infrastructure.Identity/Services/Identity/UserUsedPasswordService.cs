using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Infrastructure.Identity.Services.Identity
{
    public class UserUsedPasswordService : IUserUsedPasswordService
    {
        private readonly AdminDbContext _adminDbContext;

        public UserUsedPasswordService(AdminDbContext adminDbContext)
        {
            _adminDbContext = adminDbContext;
        }

        public async Task<IResult> AddUsedPasswordAsync(string userId, string hashedPassword)
        {
            try
            {

                _adminDbContext.Add(new UsedPassword
                {
                    UserId = userId,
                    HashedPassword = hashedPassword,
                    CreatedOn = DateTime.UtcNow,
                });
                await _adminDbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return await Result.FailAsync(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new password to the list of used passwords for a user and enforces a limit on the number of stored passwords.
        /// </summary>
        /// <param name="userId">The user ID for which the password is being added.</param>
        /// <param name="newPassword">The new password to be added.</param>
        /// <returns>Returns true if the password is added successfully; otherwise, throws an exception.</returns>
        public async Task<bool> RecordUserPasswordAsync(string userId, string newPassword)
        {
            try
            {
                // Retrieve all passwords for the user ordered by CreatedOn in descending order
                var userPasswords = await _adminDbContext.UsedPasswords
                                                             .Where(x => x.UserId == userId)
                                                             .OrderByDescending(x => x.CreatedOn)
                                                             .ToListAsync();

                // If the count exceeds the limit, delete the oldest password
                if (userPasswords.Count() >= 10)
                {
                    var oldestPassword = userPasswords.Last();
                    _adminDbContext.UsedPasswords.Remove(oldestPassword);
                    await _adminDbContext.SaveChangesAsync();
                }

                _adminDbContext.Add(new UsedPassword
                {
                    UserId = userId,
                    HashedPassword = newPassword,
                    CreatedOn = DateTime.UtcNow,
                });
                await _adminDbContext.SaveChangesAsync();

                // Return the new password
                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception($"Error getting/adding user password: {ex.Message}");
            }
        }
    }
}

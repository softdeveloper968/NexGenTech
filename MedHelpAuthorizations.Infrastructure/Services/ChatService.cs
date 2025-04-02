using AutoMapper;
using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Models.Chat;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Shared.Models;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<ChatService> _localizer;

        public ChatService(
            ApplicationContext context,
            IMapper mapper,
            IUserService userService,
            IStringLocalizer<ChatService> localizer)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _localizer = localizer;
        }

        public async Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId)
        {
            var response = await _userService.GetAsync(userId);
            //if (response.Succeeded)
            //{
            //    var user = response.Data;
            //    var query = await _context.ChatHistories
            //        .Where(h => (h.FromUserId == userId && h.ToUserId == contactId) || (h.FromUserId == contactId && h.ToUserId == userId))
            //        .OrderBy(a => a.CreatedDate)
            //        .Include(a => a.FromUser)
            //        .Include(a => a.ToUser)
            //        .Select(x => new ChatHistoryResponse
            //        {
            //            FromUserId = x.FromUserId,
            //            FromUserFullName = $"{x.FromUser.FirstName} {x.FromUser.LastName}",
            //            Message = x.Message,
            //            CreatedDate = x.CreatedDate,
            //            Id = x.Id,
            //            ToUserId = x.ToUserId,
            //            ToUserFullName = $"{x.ToUser.FirstName} {x.ToUser.LastName}",
            //            ToUserImageURL = x.ToUser.ProfilePictureDataUrl,
            //            FromUserImageURL = x.FromUser.ProfilePictureDataUrl
            //        }).ToListAsync();
            //    return await Result<IEnumerable<ChatHistoryResponse>>.SuccessAsync(query);
            //}
            //else
            //{
            //    throw new ApiException(_localizer["User Not Found!"]);
            //}

            return null;
        }

        public async Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId)
        {
            var allUsers = new List<User>(); //await _context.Users.Where(user => user.Id != userId).ToListAsync();
            var chatUsers = _mapper.Map<IEnumerable<ChatUserResponse>>(allUsers);
            return await Result<IEnumerable<ChatUserResponse>>.SuccessAsync(chatUsers);
        }

        //public async Task<IResult> SaveMessageAsync(ChatHistory message)
        //{
        //    message.ToUser = await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
        //    await _context.ChatHistories.AddAsync(message);
        //    await _context.SaveChangesAsync();
        //    return await Result.SuccessAsync();
        //}
    }
}
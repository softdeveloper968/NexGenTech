using AutoMapper.Internal;
using MedHelpAuthorizations.Application.Common.Models;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Domain.IdentityEntities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Queries
{
    public class GetTenantAssociatedUsersQuery : IRequest<PaginatedResult<TenantAssociatedUserResponse>>
    {
        public int TenantId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public GetTenantAssociatedUsersQuery(int tenantId, int pageNumber, int pageSize, string search)
        {
            TenantId = tenantId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Search = search;
        }
    }

    public class GetTenantAssociatedUsersQueryHandler : IRequestHandler<GetTenantAssociatedUsersQuery, PaginatedResult<TenantAssociatedUserResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IAdminUnitOfWork _adminUnitOfWork;

        public GetTenantAssociatedUsersQueryHandler(UserManager<ApplicationUser> userManager, IUserService userService, IAdminUnitOfWork adminUnitOfWork)
        {
            _userManager = userManager;
            _userService = userService;
            _adminUnitOfWork = adminUnitOfWork;
        }
        public async Task<PaginatedResult<TenantAssociatedUserResponse>> Handle(GetTenantAssociatedUsersQuery request, CancellationToken cancellationToken)
        {
            var tenantUserIds = await _adminUnitOfWork.Repository<TenantUser, int>()
                                                    .Entities
                                                    .Where(x => x.TenantId == request.TenantId)
                                                    .Select(x => x.UserId)
                                                    .ToListAsync();

            var tenantName = (await _adminUnitOfWork.Repository<MedHelpAuthorizations.Domain.IdentityEntities.Tenant, int>()
                                                    .Entities
                                                    .FirstAsync(x => x.Id == request.TenantId)
                                                    ).TenantName;


            Expression<Func<ApplicationUser, TenantAssociatedUserResponse>> expression = e => new TenantAssociatedUserResponse
            {
                TenantName = tenantName,
                Id = e.Id,
                UserName = e.UserName,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                EmailConfirmed = e.EmailConfirmed,
                IsActive = e.IsActive,
                PhoneNumber = string.IsNullOrEmpty(e.PhoneNumber) ? null : long.Parse(e.PhoneNumber),
                CreatedByName = e.CreatedBy,
                CreatedOn = e.CreatedOn,
                LastModifiedByName = e.LastModifiedBy,
                LastModifiedOn = e.LastModifiedOn
            };

            var query = _userManager.Users;

            if (tenantUserIds != null)
            {
                query = query.Where(x => tenantUserIds.Contains(x.Id));
            }
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => (!string.IsNullOrEmpty(x.UserName) && x.UserName.Contains(request.Search)) ||
                                x.FirstName.Contains(request.Search) || x.LastName.Contains(request.Search) ||
                                (!string.IsNullOrEmpty(x.Email) && x.Email.Contains(request.Search)));
            }

            PaginatedResult<TenantAssociatedUserResponse> users = await query
                                                            .OrderByDescending(x => x.LastModifiedOn)
                                                            .Select(expression)
                                                            .ToPaginatedListAsync(request.PageNumber, request.PageSize);


            var userIds = users.Data.Where(x => !string.IsNullOrEmpty(x.CreatedByName)).Select(x => x.CreatedByName).ToList();

            userIds.TryAdd(users.Data.Where(x => !string.IsNullOrEmpty(x.LastModifiedByName)).Select(x => x.LastModifiedByName).ToList());

            var names = await _userService.GetNamesAsync(userIds.ToArray());

            foreach (var user in users.Data)
            {
                if (!string.IsNullOrWhiteSpace(user.CreatedByName))
                {
                    user.CreatedByName = names[user.CreatedByName];
                }
                if (!string.IsNullOrWhiteSpace(user.LastModifiedByName))
                {
                    user.LastModifiedByName = names[user.LastModifiedByName];
                }
            }

            return users;
        }
    }
}

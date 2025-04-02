using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.UserClients.Commands.AddEdit
{
    public class AddEditUserClientCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }

        public ICollection<int> ClientIds { get; set; }
        public bool CreateEmployee { get; set; } //AA-233
        public string EmployeeNumber { get; set; } //AA-233
        public EmployeeRoleEnum? DefaultEmployeeRoleId { get; set; } //AA-233
    }

    public class AddEditUserClientCommandHandler : IRequestHandler<AddEditUserClientCommand, Result<string>>
    {
        private readonly IMapper _mapper;
        private readonly IUserClientRepository _userClientRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IUserService _userService;
        private ITenantRepositoryFactory _tenantRepositoryFactory;
        public AddEditUserClientCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserClientRepository userClientRepository, IMediator mediator, IUserService userService, ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userClientRepository = userClientRepository;
            _mediator = mediator;
            _userService = userService;
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }

        public async Task<Result<string>> Handle(AddEditUserClientCommand request, CancellationToken cancellationToken) //updated AA-233
        {
            try
            {
                // Update UserClient details for the user
                if (await _userClientRepository.UpdateClientsForUserAsync(request.UserId, request.ClientIds))
                    return await Result<string>.SuccessAsync(request.UserId, "UserClient Details updated");

                return await Result<string>.FailAsync("Failed to update UserClient");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

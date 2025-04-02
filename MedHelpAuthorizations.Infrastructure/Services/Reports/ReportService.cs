using AutoMapper;
using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace MedHelpAuthorizations.Infrastructure.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ReportsEnum> _unitOfWork;
        private int ClientId => _currentUserService.ClientId;

        public ReportService(IMapper mapper, ICurrentUserService currentUserService, IUnitOfWork<ReportsEnum> unitOfWork)
        {
            _mapper = mapper;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<GetAllReportsResponse>> GetAllReportsByCategory()
        {
            return await _unitOfWork.Repository<Report>().Entities
                   .OrderBy(r => r.ReportCategoryId)
                   .Select(z => new GetAllReportsResponse
                   {
                       Name = z.Name,
                       ReportCategory = z.ReportCategoryId
                   })
                   .ToListAsync();
        }
        public async Task<List<GetAllReportsResponse>> GetAllReportsByCategoryId(ReportCategoryEnum reportCategoryId)
        {
            return await _unitOfWork.Repository<Report>().Entities
                  .Where(r => r.ReportCategoryId == reportCategoryId)
                  .OrderBy(r => r.ReportCategoryId)
                  .Select(z => new GetAllReportsResponse
                  {
                      Name = z.Name,
                      ReportCategory = z.ReportCategoryId
                  })
                 .ToListAsync();
        }

    }
}

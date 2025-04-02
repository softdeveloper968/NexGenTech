using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Models.Audit;
using MedHelpAuthorizations.Application.Responses.Audit;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<AuditService> _localizer;

        public AuditService(
            IMapper mapper,
            ApplicationContext context,
            IExcelService excelService,
            IStringLocalizer<AuditService> localizer)
        {
            _mapper = mapper;
            _context = context;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync(string userId)
        {            
            try
            {

                var trails = await _context.AuditTrails.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
                var mappedLogs = _mapper.Map<List<AuditResponse>>(trails);
                return await Result<IEnumerable<AuditResponse>>.SuccessAsync(mappedLogs);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<string> ExportToExcelAsync(string userId)
        {
            var trails = await _context.AuditTrails.Where(x => x.UserId == userId)
                .OrderByDescending(a => a.DateTime)
                .Select(audit => new ExportQueryResponse
                {
                    AuditId = audit.Id,
                    TableName = audit.TableName,
                    Type = audit.Type,
                    DateTime = audit.DateTime,
                    PrimaryKey = audit.PrimaryKey,
                    OldValues = audit.OldValues,
                    NewValues = audit.NewValues,
                })
                .ToListAsync();
            var result = await _excelService.ExportAsync(trails, sheetName: _localizer["Audit trails"], workSheetName: _localizer["Audit trails Worksheet"],
                mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
                {
                    { _localizer["Table Name"], item => item.TableName },
                    { _localizer["Type"], item => item.Type },
                    { _localizer["Date Time (Local)"], item => DateTime.SpecifyKind(item.DateTime, DateTimeKind.Utc).ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss") },
                    { _localizer["Date Time (UTC)"], item => item.DateTime.ToString("dd/MM/yyyy HH:mm:ss") },
                    { _localizer["Primary Key"], item => item.PrimaryKey },
                    { _localizer["Old Values"], item => item.OldValues },
                    { _localizer["New Values"], item => item.NewValues },
                });

            return result;
        }
    }
}
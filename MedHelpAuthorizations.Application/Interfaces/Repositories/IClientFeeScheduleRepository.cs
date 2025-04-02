using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientFeeScheduleRepository
    {
        Task<Domain.Entities.ClientFeeSchedule> GetByIdAsync(int Id); //EN-214
    }
}

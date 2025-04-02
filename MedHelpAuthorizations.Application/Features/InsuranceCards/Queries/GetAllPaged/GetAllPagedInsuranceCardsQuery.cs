using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetAllPaged
{
    public class GetAllPagedInsuranceCardsQuery
    {
        private int pageNumber;
        private int pageSize;
        private string searchString;

        public GetAllPagedInsuranceCardsQuery(int pageNumber, int pageSize, string searchString)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            this.searchString = searchString;
        }
    }
}

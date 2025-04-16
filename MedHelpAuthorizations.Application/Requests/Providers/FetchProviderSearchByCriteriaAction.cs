using MedHelpAuthorizations.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Requests.Providers
{
    public class FetchProviderSearchByCriteriaAction
    {
        public SearchCriteria SearchCriteria { get; set; }

        public string SearchValue { get; set; }

        public FetchProviderSearchByCriteriaAction(SearchCriteria criteria, string searchvalue)
        {
            SearchCriteria = criteria;
            SearchValue = searchvalue;
        }
    }
}

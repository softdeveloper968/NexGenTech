using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.SortDefinitions
{
    public static partial class SortDefinitions
    {
        public static Dictionary<string, string> _fieldMappings = null;
        public static Dictionary<string, string> SortFieldMappings
        {
            get
            {
                if (_fieldMappings == null)
                {
                    _fieldMappings = GetMappingFields();
                }
                return _fieldMappings;
            }
        }
        private static string GetWithPrefix(string prefix, string key)
        {
            return $"{prefix}_{key}";
        }
        private static Dictionary<string, string> GetMappingFields()
        {
            var retVal = new Dictionary<string, string>();

            foreach (var item in ClaimStatusBatchesDefinitions)
            {
                retVal.TryAdd(GetWithPrefix(ClaimStatusBatchesPrefix, item.Key), item.Value);
            }

            foreach (var item in RPAInsurancesDefinitions)
            {
                retVal.TryAdd(GetWithPrefix(RPAInsurancesDefinitionsPrefix, item.Key), item.Value);
            }            
            
            foreach (var item in ClaimStatusBatchHistoriesDefinitions)
            {
                retVal.TryAdd(GetWithPrefix(ClaimStatusBatchHistoriesPrefix, item.Key), item.Value);
            }

            return retVal;
        }
    }
}

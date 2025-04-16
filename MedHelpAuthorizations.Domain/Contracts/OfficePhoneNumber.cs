using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.Contracts
{
    [NotMapped]
    public class OfficePhoneNumber
    {
        public OfficePhoneNumber()
        {

        }

        public string FullNumber { get; set; }
        public string Number
        {
            get
            {
                return PhoneNumberWithoutExtension + Extension;
            }

        }
        public string AreaCode { get; set; }
        public string PhoneNumberWithoutExtension { get; set; }
        public string Extension { get; set; }

        public OfficePhoneNumber(string fullNumber)
        {
            FullNumber = fullNumber ?? string.Empty;
            var digits = Regex.Replace(FullNumber, "[^0-9]", x => "");
            var number = "";
            if (digits.Length != 0 && digits.Length >= 11 && digits[0] == '1')
                digits = digits.Substring(1);
            {
                number = digits.Length >= 10 ? digits : "0000000000";
                AreaCode = number.Substring(0, 3);
                if (number.Length > 10)
                    Extension = number.Substring(10);
                number = number.Substring(3, 7);
            }
            PhoneNumberWithoutExtension = AreaCode + number;
        }

        public override string ToString()
        {
            return $"({AreaCode}) {Number.Substring(3, 3)}-{Number.Substring(6, 4)}  ext: {Number.Substring(10)}";
        }
    }
}

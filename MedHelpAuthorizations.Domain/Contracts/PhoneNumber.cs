using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace MedHelpAuthorizations.Domain.Contracts
{
    [NotMapped]
    public class PhoneNumber
    {


        public string FullNumber { get; set; }
        public string Number { get; set; }
        public string AreaCode { get; set; }

        public PhoneNumber()
        {

        }
        public PhoneNumber(string fullNumber)
        {
            FullNumber = fullNumber ?? string.Empty;
            var digits = Regex.Replace(FullNumber, "[^0-9]", x => "");
            if (digits.Length != 0 && digits.Length >= 11 && digits[0] == '1')
                digits = digits.Substring(1);
            Number = digits.Length == 10 ? digits : "0000000000";
            AreaCode = Number.Substring(0, 3);
        }

        public override string ToString()
        {
            return $"({AreaCode}) {Number.Substring(3, 3)}-{Number.Substring(6, 4)}";
        }
    }
}
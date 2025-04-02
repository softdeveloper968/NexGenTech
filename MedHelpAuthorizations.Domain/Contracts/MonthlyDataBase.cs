namespace MedHelpAuthorizations.Domain.Contracts
{
    public abstract class MonthlyDataBase : AuditableEntity<int>
    {
        public string Date { get; set; }
        public decimal Change { get; set; }
        public string Status { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        protected MonthlyDataBase(string date, decimal change, string status)
        {
            Date = date;
            Change = change;
            Status = status;
        }
    }
}

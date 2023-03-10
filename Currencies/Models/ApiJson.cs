namespace Currencies.Models
{
    public class ApiJson
    {
        public string Date { get; set; }

        public string PreviousDate { get; set; }

        public string PreviousURL { get; set; }

        public string Timestamp { get; set; }

        public Dictionary<string, Currency>? Valute { get; set; }
    }
}

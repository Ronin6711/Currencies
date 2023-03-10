using System.ComponentModel.DataAnnotations;

namespace Currencies.Models
{
    public class Currency
    {
        [Key]
        public string ID { get; set; }

        public string NumCode { get; set; }

        public string CharCode { get; set; }

        public int Nominal { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal Previous { get; set; }

    }
}

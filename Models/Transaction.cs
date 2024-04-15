using System.ComponentModel.DataAnnotations;

namespace WealthTrackr.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public string FkAccountId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string TransactionType { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        //public Category Category { get; set; }
    }
}

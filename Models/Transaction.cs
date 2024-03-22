using System.ComponentModel.DataAnnotations;

namespace WealthTrackr.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int FkAccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public int FkCategoryId { get; set; }
    }
}

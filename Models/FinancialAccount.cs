using System.ComponentModel.DataAnnotations;

namespace WealthTrackr.Models
{
    public class FinancialAccount
    {
        [Key]
        public int FinancialAccountId { get; set; }
        public string FkUserId { get; set; }
        public string AccountName { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Balance { get; set; } 
    }
}

using System.ComponentModel.DataAnnotations;
using WealthTrackr.Models;

namespace WealthTrackr.ViewModels
{
    public class TransactionModel
    {
        
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The Transaction Type field is required.")]
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "The Amount field is required.")]
        public double Amount { get; set; }
        public string? Description { get; set; }
    }
}

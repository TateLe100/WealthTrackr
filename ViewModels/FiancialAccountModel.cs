using System.ComponentModel.DataAnnotations;

namespace WealthTrackr.ViewModels
{
    public class FiancialAccountModel
    {
        
        public int FinancialAccountId { get; set; }
        
        [Required(ErrorMessage = "The Account name is required.")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Please enter your current account balance.")]
        public double Balance { get; set; }
    }
}

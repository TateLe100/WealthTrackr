using System.ComponentModel.DataAnnotations;

namespace WealthTrackr.ViewModels
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter category type.")]
        public string CategoryName { get; set; }
        public string Type { get; set; } = "Expense";
    }
}

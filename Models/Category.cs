﻿namespace WealthTrackr.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Type { get; set; } = "Expense";
        public string FkAccountId { get; set; }
    }
}

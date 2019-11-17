using System;
using System.ComponentModel.DataAnnotations;

namespace InvoiceAppDemo.ViewModels.InvoiceItems
{
    public class InvoiceItemCreateViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Količina")]
        public decimal Amount { get; set; }
        [Required]
        [Display(Name = "Pojedinačna cijena bez PDV-a")]
        public decimal PriceWithoutTax { get; set; }
        public Guid InvoiceId { get; set; }
    }
}
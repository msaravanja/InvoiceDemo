using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvoiceAppDemo.Models
{
    public class InvoiceItem
    {
        public Guid Id { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Količina")]
        public decimal Amount { get; set; }
        [Display(Name = "Pojedinačna cijena bez PDV-a")]
        public decimal PriceWithoutTax { get; set; }
        [Required] 
        public Guid InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
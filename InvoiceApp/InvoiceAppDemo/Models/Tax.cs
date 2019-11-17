using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvoiceAppDemo.Models
{
    public class Tax
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Zemlja")]
        public string Country { get; set; }
        [Required]
        [Display(Name = "Porez u %")]
        public decimal TaxValue { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}
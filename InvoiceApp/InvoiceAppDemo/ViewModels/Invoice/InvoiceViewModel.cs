using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using InvoiceAppDemo.Models;

namespace InvoiceAppDemo.ViewModels.Invoice
{
    public class InvoiceViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Broj fakture")]
        public string InvoiceNumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Datum fakture")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime InvoiceDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Datum dospijeća fakture")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime InvoicePaymentDate { get; set; }
        [Required]
        [Display(Name = "Porezna stopa")]
        public decimal TaxPercentage { get; set; }
        [Display(Name = "Primatelj računa")]
        public string InvoiceReceiver { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        [Required]
        public string CreatedByUserId { get; set; }

        public Guid TaxId { get; set; }

        public IEnumerable<SelectListItem> Taxes { get; set; }
    }
}
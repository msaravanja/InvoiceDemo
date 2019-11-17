using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InvoiceAppDemo.Models;

namespace InvoiceAppDemo.ViewModels.Invoice
{
    public class InvoiceForEditViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Broj fakture")]
        public string InvoiceNumber { get; set; }
        [Display(Name = "Datum fakture")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime InvoiceDate { get; set; }
        [Display(Name = "Datum dospijeća")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime InvoicePaymentDate { get; set; }
        [Required]
        [Display(Name = "Porezna stopa")]
        public decimal TaxPercentage { get; set; }
        [Display(Name = "Primatelj računa")]
        public string InvoiceReceiver { get; set; }
        public string CreatedByUserId { get; set; }
        public Guid TaxId { get; set; }

        public IEnumerable<SelectListItem> Taxes { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InvoiceAppDemo.ViewModels.InvoiceItems;

namespace InvoiceAppDemo.ViewModels.Invoice
{
    public class InvoiceForDetailedViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Broj fakture")]
        public string InvoiceNumber { get; set; }
        [Display(Name = "Datum fakture")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime InvoiceDate { get; set; }
        [Display(Name = "Datum dospijeća")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime InvoicePaymentDate { get; set; }
        [Display(Name = "Porezna stopa")]
        public decimal TaxPercentage { get; set; }
        [Display(Name = "Primatelj računa")]
        public string InvoiceReceiver { get; set; }
        public List<InvoiceItemsForIndex> InvoiceItems { get; set; }
        [Display(Name = "Fakturu kreirao")]
        public string CreatedByUser { get; set; }
        [Display(Name = "Ukupno s porezom")]
        public decimal TotalWithTax { get; set; }
        [Display(Name = "Ukupno bez poreza")]
        public decimal TotalWithoutTax { get; set; }
    }
}
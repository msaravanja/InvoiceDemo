using System;
using System.ComponentModel.DataAnnotations;

namespace InvoiceAppDemo.ViewModels.Invoice
{
    public class InvoiceWithPricesViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Broj fakture")]
        public string InvoiceNumber { get; set; }
        [Display(Name = "Datum fakture")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime InvoiceDate { get; set; }
        [Display(Name = "Datum dospijeća fakture")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime InvoicePaymentDate { get; set; }
        [Display(Name = "Porezna stopa")]
        public decimal TaxPercentage { get; set; }
        [Display(Name = "Primatelj računa")]
        public string InvoiceReceiver { get; set; }
        [Display(Name = "Ukupno sa porezom")]
        public decimal TotalWithTax { get; set; }
        [Display(Name = "Ukupno bez poreza")]
        public decimal TotalWithoutTax { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InvoiceAppDemo.Models
{
    public class Invoice
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoicePaymentDate { get; set; }
        [Required]
        public decimal TaxPercentage { get; set; }
        public string InvoiceReceiver { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        [Required]
        public string CreatedByUserId { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }
        [Required]
        public Guid TaxId { get; set; }
        public virtual Tax Tax { get; set; }

    }
}
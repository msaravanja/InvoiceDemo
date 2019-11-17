using System;

namespace InvoiceAppDemo.ViewModels.InvoiceItems
{
    public class InvoiceItemsForIndex
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal PriceWithoutTax { get; set; }
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal ItemTotalCost { get; set; }
    }
}
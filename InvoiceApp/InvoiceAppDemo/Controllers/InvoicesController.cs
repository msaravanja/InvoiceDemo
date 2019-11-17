using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using InvoiceAppDemo.Models;
using InvoiceAppDemo.ViewModels.Invoice;
using InvoiceAppDemo.ViewModels.InvoiceItems;
using MEFDemo;
using Microsoft.AspNet.Identity;

namespace InvoiceAppDemo.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class InvoicesController : Controller
    {

        [Import] 
        private ITaxCalculator _taxCalculator;

        //private readonly ITaxCalculator _taxCalculator;
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //[ImportingConstructor]
        //public InvoicesController(ITaxCalculator taxCalculator)
        //{
        //    this._taxCalculator = taxCalculator;
        //}

        // GET: Invoices
        public ActionResult Index()
        {
            List<InvoiceWithPricesViewModel> invoicesWithTotals = new List<InvoiceWithPricesViewModel>();

            //get all invoices
            var invoices = db.Invoices.ToList();

            foreach (var invoice in invoices)
            {
                var invoiceWithTotal = new InvoiceWithPricesViewModel
                {
                    Id = invoice.Id,
                    InvoiceDate = invoice.InvoiceDate,
                    InvoiceNumber = invoice.InvoiceNumber,
                    InvoicePaymentDate = invoice.InvoicePaymentDate,
                    InvoiceReceiver = invoice.InvoiceReceiver,
                    TaxPercentage = invoice.TaxPercentage
                };
                // if invoice has items associated to it, calculate tax
                var totals = CalculateTotals(invoice.Id);
                invoiceWithTotal.TotalWithoutTax = totals[0];
                invoiceWithTotal.TotalWithTax = totals[1];


                invoicesWithTotals.Add(invoiceWithTotal);
            }

            return View(invoicesWithTotals);
        }

        // GET: Invoices/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Invoice invoice = db.Invoices.Find(id);

            if (invoice == null)
            {
                return HttpNotFound();
            }

            var userCreatedInvoice = db.Users.FirstOrDefault(x => x.Id == invoice.CreatedByUserId);

            var invoiceDetailed = new InvoiceForDetailedViewModel
            {
                Id = invoice.Id,
                CreatedByUser = userCreatedInvoice?.UserName,
                InvoiceDate = invoice.InvoiceDate,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoicePaymentDate = invoice.InvoicePaymentDate,
                TaxPercentage = invoice.TaxPercentage,
                InvoiceReceiver = invoice.InvoiceReceiver,
                InvoiceItems = new List<InvoiceItemsForIndex>()
            };

            var totals = CalculateTotals((Guid) id);
            invoiceDetailed.TotalWithoutTax = totals[0];
            invoiceDetailed.TotalWithTax = totals[1];

            var invoiceItems = db.InvoiceItems.Where(x => x.InvoiceId == id).ToList();

            foreach (var invoiceItem in invoiceItems)
            {
                var invoiceItemWithPrices = new InvoiceItemsForIndex
                {
                    Id = invoiceItem.Id,
                    Amount = invoiceItem.Amount,
                    Description = invoiceItem.Description,
                    InvoiceId = invoiceItem.InvoiceId,
                    PriceWithoutTax = invoiceItem.PriceWithoutTax,
                    ItemTotalCost = invoiceItem.Amount * invoiceItem.PriceWithoutTax
                };

                invoiceDetailed.InvoiceItems.Add(invoiceItemWithPrices);
            }

            return View(invoiceDetailed);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            var taxes = GetTaxes();

            var newInvoice = new InvoiceViewModel
            {
                Taxes = new SelectList(taxes, "Value", "Text"),
                CreatedByUserId = User.Identity.GetUserId()
            };

            return View(newInvoice);
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvoiceNumber,InvoiceDate,InvoicePaymentDate,TaxPercentage,InvoiceReceiver,CreatedByUserId,TaxId")] InvoiceViewModel invoice)
        {
            if (ModelState.IsValid)
            {
                var taxValue = db.Taxes.FirstOrDefault(x => x.Id == invoice.TaxId);
                var newInvoiceModel = new Invoice
                {
                    Id = Guid.NewGuid(),
                    CreatedByUserId = User.Identity.GetUserId(),
                    InvoiceDate = invoice.InvoiceDate,
                    InvoiceNumber = invoice.InvoiceNumber,
                    InvoicePaymentDate = invoice.InvoicePaymentDate,
                    TaxPercentage = taxValue.TaxValue,
                    InvoiceReceiver = invoice.InvoiceReceiver,
                    TaxId = invoice.TaxId
                };

                db.Invoices.Add(newInvoiceModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            var taxes = db.Taxes.Where(x => x.Id != invoice.TaxId).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Country + " - " + x.TaxValue + "%"
            }).ToList();

            var taxesTip = new SelectListItem()
            {
                Value = invoice.TaxId.ToString(),
                Text = "-- Odaberite porez --"
            };

            taxes.Insert(0, taxesTip);

            var invoiceForEditViewModel = new InvoiceForEditViewModel
            {
                Id = invoice.Id,
                CreatedByUserId = invoice.CreatedByUserId,
                InvoiceDate = invoice.InvoiceDate,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoicePaymentDate = invoice.InvoicePaymentDate,
                InvoiceReceiver = invoice.InvoiceReceiver,
                TaxPercentage = invoice.TaxPercentage,
                Taxes = new SelectList(taxes, "Value", "Text")
            };

            return View(invoiceForEditViewModel);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvoiceNumber,InvoiceDate,InvoicePaymentDate,TaxPercentage,InvoiceReceiver,CreatedByUserId,TaxId")] InvoiceForEditViewModel invoice)
        {
            if (ModelState.IsValid)
            {
                var taxValue = db.Taxes.FirstOrDefault(x => x.Id == invoice.TaxId);
                var invoiceForEdit = new Invoice
                {
                    Id = invoice.Id,
                    CreatedByUserId = invoice.CreatedByUserId,
                    InvoiceDate = invoice.InvoiceDate,
                    InvoiceNumber = invoice.InvoiceNumber,
                    InvoicePaymentDate = invoice.InvoicePaymentDate,
                    InvoiceReceiver = invoice.InvoiceReceiver,
                    TaxPercentage = taxValue.TaxValue,
                    TaxId = invoice.TaxId
                };

                db.Entry(invoiceForEdit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            var userCreatedInvoice = db.Users.FirstOrDefault(x => x.Id == invoice.CreatedByUserId);

            var invoiceToDelete = new InvoiceForDetailedViewModel
            {
                Id = invoice.Id,
                CreatedByUser = userCreatedInvoice?.UserName,
                InvoiceDate = invoice.InvoiceDate,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoicePaymentDate = invoice.InvoicePaymentDate,
                InvoiceReceiver = invoice.InvoiceReceiver
            };
            return View(invoiceToDelete);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<SelectListItem> GetTaxes()
        {
            var taxes = db.Taxes.ToArray().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $@"{x.Country} - {x.TaxValue}%"
            }).ToList();

            var taxesTip = new SelectListItem()
            {
                Value = null,
                Text = @"-- Odaberite porez --"
            };

            taxes.Insert(0, taxesTip);

            return taxes;
        }

        public decimal[] CalculateTotals(Guid invoiceId)
        {
            var result = new decimal[] {0, 0};
            var currentInvoice = db.Invoices.FirstOrDefault(x => x.Id == invoiceId);

            if (db.InvoiceItems.Any(x => x.InvoiceId == invoiceId) && currentInvoice != null)
            {
                var totalWithoutTaxItem = db.InvoiceItems.Where(x => x.InvoiceId == invoiceId)
                    .Sum(x => (x.Amount * x.PriceWithoutTax));

                result[0] = totalWithoutTaxItem;
                result[1] = _taxCalculator.CalculateTax(totalWithoutTaxItem, currentInvoice.TaxPercentage);
            }

            return result;
        }
    }
}

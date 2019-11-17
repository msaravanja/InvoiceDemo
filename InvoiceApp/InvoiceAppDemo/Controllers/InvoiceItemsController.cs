using System;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using InvoiceAppDemo.Models;
using InvoiceAppDemo.ViewModels.InvoiceItems;

namespace InvoiceAppDemo.Controllers
{
    public class InvoiceItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: InvoiceItems
        //public ActionResult Index()
        //{
        //    return View(db.InvoiceItems.ToList());
        //}

        // GET: InvoiceItems/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceItem invoiceItem = db.InvoiceItems.Find(id);
            if (invoiceItem == null)
            {
                return HttpNotFound();
            }
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Create
        public ActionResult Create(Guid id)
        {
            var newInvoiceItem = new InvoiceItemCreateViewModel
            {
                InvoiceId = id
            };
            return View(newInvoiceItem);
        }

        // POST: InvoiceItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Description,Amount,PriceWithoutTax,InvoiceId")] InvoiceItemCreateViewModel invoiceItem)
        {
            if (ModelState.IsValid)
            {
                var newInvoiceItem = new InvoiceItem()
                {
                    Id = Guid.NewGuid(),
                    Amount = invoiceItem.Amount,
                    Description = invoiceItem.Description,
                    InvoiceId = invoiceItem.InvoiceId,
                    PriceWithoutTax = invoiceItem.PriceWithoutTax
                };

                db.InvoiceItems.Add(newInvoiceItem);
                db.SaveChanges();
                return RedirectToAction("Details", "Invoices", new { id = invoiceItem.InvoiceId });
            }

            return View(invoiceItem);
        }

        // GET: InvoiceItems/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceItem invoiceItem = db.InvoiceItems.Find(id);
            if (invoiceItem == null)
            {
                return HttpNotFound();
            }
            return View(invoiceItem);
        }

        // POST: InvoiceItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Amount,PriceWithoutTax,InvoiceId")] InvoiceItem invoiceItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoiceItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Invoices", new { id = invoiceItem.InvoiceId });
            }
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceItem invoiceItem = db.InvoiceItems.Find(id);
            if (invoiceItem == null)
            {
                return HttpNotFound();
            }
            return View(invoiceItem);
        }

        // POST: InvoiceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            InvoiceItem invoiceItem = db.InvoiceItems.Find(id);
            db.InvoiceItems.Remove(invoiceItem);
            db.SaveChanges();
            return RedirectToAction("Details", "Invoices", new { id = invoiceItem.InvoiceId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

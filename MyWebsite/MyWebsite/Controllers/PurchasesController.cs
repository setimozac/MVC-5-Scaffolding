using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyWebsite.Models;
using MyWebsite.ViewModel;
using PagedList;

namespace MyWebsite.Controllers
{
    public class PurchasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Purchases
        public ActionResult Index(string sortDir, string searchString, string currentFilter, int? page, string sortOrder = "", string purchaseId="")
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortDir = sortDir;




            if (User.IsInRole(RoleName.CanManage))
            {
                var uShippings = db.Shippings.AsQueryable();
                if (!string.IsNullOrEmpty(searchString))
                {
                    uShippings = uShippings.Where(r => r.Purchase.Product.Name.Contains(searchString));
                }

                int PageSize = 6;
                int PageNumber = page ?? 1;

                uShippings = uShippings.OrderBy(s => s.ShippingDate);

                var data = uShippings.ToPagedList(PageNumber, PageSize);

                return View(data);
            }
            else if(User.Identity.IsAuthenticated)
            {

                var uShippings = db.Shippings.AsQueryable();
                string id = User.Identity.GetUserId();
                uShippings = uShippings.Where(x => x.Purchase.User.Id == id);

                if (!string.IsNullOrEmpty(searchString))
                {
                    uShippings = uShippings.Where(r => r.Purchase.Product.Name.Contains(searchString));
                }

                int PageSize = 6;
                int PageNumber = page ?? 1;

                uShippings = uShippings.OrderBy(s => s.Purchase.Product.Name);

                var data = uShippings.ToPagedList(PageNumber, PageSize);


                return View("Orders", data);
            }

            var purchase = new Purchase();
            var shipping = new Shipping();
            var purchaseVM = new PurchaseViewModel();
            if (!string.IsNullOrEmpty(purchaseId))
            {
                purchase = db.Purchases.Where(x => x.PurchaseId == purchaseId).FirstOrDefault();
                shipping = db.Shippings.Where(x => x.Purchase.PurchaseId == purchaseId).FirstOrDefault();
            }

            purchaseVM.Purchase = purchase;
            purchaseVM.Shipping = shipping;

            return View("Tracking", purchaseVM);
        }

        // GET: Purchases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // GET: Purchases/Create
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Create([Bind(Include = "Id,PurchaseId,PurchaseDate,Payment")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Purchases.Add(purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(purchase);
        }

        // GET: Purchases/Edit/5
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit([Bind(Include = "Id,PurchaseId,PurchaseDate,Payment")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [Authorize(Roles = RoleName.CanManage)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Purchase purchase = db.Purchases.Find(id);
            var shipping = db.Shippings.Where(x => x.Purchase.Id == id).FirstOrDefault();
            db.Shippings.Remove(shipping);
            //db.Purchases.Remove(purchase);
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
    }
}

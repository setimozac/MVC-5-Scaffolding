using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MyWebsite.Helper;
using MyWebsite.Models;
using MyWebsite.ViewModel;
using Microsoft.AspNet.Identity;
using PagedList;

namespace MyWebsite.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<ProductImage> dbImages = new List<ProductImage>();

        // GET: Products
        public ActionResult Index(string sortDir, string searchString, string currentFilter, int? page, string sortOrder = "")
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

            var product = db.Products.AsQueryable();
            product = product.Where(x => x.Quantity > 0);

            if (!string.IsNullOrEmpty(searchString))
            {
                product = product.Where(r => r.Name.Contains(searchString));
            }

            int PageSize = 6;
            int PageNumber = page ?? 1;

            product = product.OrderBy(s => s.Name);


            if (sortOrder != null)
            {
                switch (sortOrder.ToLower())
                {
                    case "name":
                        if (sortDir.ToLower() == "desc")
                        {
                            product = product.OrderByDescending(s => s.Name);
                        }
                        else
                        {
                            product = product.OrderBy(s => s.Name);
                        }
                        break;
                    case "price":
                        if (sortDir.ToLower() == "desc")
                        {
                            product = product.OrderByDescending(s => s.Price);
                        }
                        else
                        {
                            product = product.OrderBy(s => s.Price);
                        }
                        break;
                    default:
                        product = product.OrderBy(s => s.Name);
                        break;
                }
            }



            var data = product.ToPagedList(PageNumber, PageSize);


            if (User.Identity.IsAuthenticated && User.IsInRole(RoleName.CanManage))
                return View(data);
            return View("AllProducts", data);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Create(ProductAddModelView productVM)
        {
            if (ModelState.IsValid)
            {

                var product = new Product();
                product.Name = productVM.Name;
                product.Price = productVM.Price;
                product.Description = productVM.Description;
                product.Quantity = productVM.Quantity;

                //var image = new ProductImage();
                //image.Image = ImageConvertor.ByteArrayFromPostedFile(productVM.Image);
                //image.IsMain = true;

                
                //image.Product = product;
                //product.ProductImages.Add(image);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = product.ProductId });
            }

            return View(productVM);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<Product, ProductAddModelView>().ForMember(x => x.Image0, opt => opt.Ignore()).ForMember(x => x.Image1, opt => opt.Ignore()).
                ForMember(x => x.Image2, opt => opt.Ignore()).ForMember(x => x.ImageDb, opt => opt.Ignore()).
                ForMember(x => x.Images, opt => opt.Ignore()).ForMember(x => x.ImagesDb, opt => opt.Ignore()).ForMember(x => x.IsMain, opt => opt.Ignore());

            var productVM = Mapper.Map<ProductAddModelView>(product);
            if(product.ProductImages.Count > 0)
            {
                productVM.PImagesDb = new List<ProductImage>();
                foreach (var image in product.ProductImages.ToList())
                {
                    
                    productVM.PImagesDb.Add(image);
                }
                


            }
            

            return View(productVM);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit(ProductAddModelView productVM)
        {
            if (ModelState.IsValid)
            {

                Product product = db.Products.Find(productVM.ProductId);

                product.Name = productVM.Name;
                product.Price = productVM.Price;
                product.Description = productVM.Description;
                product.Quantity = productVM.Quantity;

                foreach (var img in product.ProductImages.ToList())
                    dbImages.Add(img);




                //if (productVM.PImagesDb == null)
                if (product.ProductImages == null || product.ProductImages.Count() == 0)
                {
                    if (productVM.Image0 != null)
                        product.ProductImages.Add(new ProductImage() { Image = ImageConvertor.ByteArrayFromPostedFile(productVM.Image0) });
                    if (productVM.Image1 != null)
                        product.ProductImages.Add(new ProductImage() { Image = ImageConvertor.ByteArrayFromPostedFile(productVM.Image1) });
                    if (productVM.Image2 != null)
                        product.ProductImages.Add(new ProductImage() { Image = ImageConvertor.ByteArrayFromPostedFile(productVM.Image2) });
                }
                else
                {
                    if (productVM.Images == null)
                        productVM.Images = new List<HttpPostedFileBase>();


                    if (productVM.Image0 != null)
                    {
                        dbImages[0].Image = ImageConvertor.ByteArrayFromPostedFile(productVM.Image0); // in khat jadide
                    }
                    if (productVM.Image1 != null)
                    {
                        if (dbImages.Count > 1)                                                              // in khat jadide
                            dbImages[1].Image = ImageConvertor.ByteArrayFromPostedFile(productVM.Image1); // in khat jadide
                        else
                            product.ProductImages.Add(new ProductImage() { Image = ImageConvertor.ByteArrayFromPostedFile(productVM.Image1) });
                    }
                    if (productVM.Image2 != null)
                    {
                        if (dbImages.Count > 2)                                                              // in khat jadide
                            dbImages[2].Image = ImageConvertor.ByteArrayFromPostedFile(productVM.Image2); // in khat jadide
                        else
                            product.ProductImages.Add(new ProductImage() { Image = ImageConvertor.ByteArrayFromPostedFile(productVM.Image2) });
                    }
                   
                }
                
                
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productVM);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            var purchase = db.Purchases.Where(x => x.Product.ProductId == id).FirstOrDefault();
            product.Quantity = 0;
            db.Entry(product).State = EntityState.Modified;
            //db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Buy(int? id)
        {
            ViewBag.ErrMessage = TempData["ErrMessage"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<Product, BuyViewModel>().ForMember(x => x.Image0, opt => opt.Ignore()).ForMember(x => x.Image1, opt => opt.Ignore()).
                ForMember(x => x.Image2, opt => opt.Ignore()).ForMember(x => x.ImageDb, opt => opt.Ignore()).
                ForMember(x => x.Images, opt => opt.Ignore()).ForMember(x => x.ImagesDb, opt => opt.Ignore()).ForMember(x => x.IsMain, opt => opt.Ignore());

            var BuyVM = Mapper.Map<BuyViewModel>(product);
            if (product.ProductImages.Count > 0)
            {
                BuyVM.PImagesDb = new List<ProductImage>();
                foreach (var image in product.ProductImages.ToList())
                {

                    BuyVM.PImagesDb.Add(image);
                }
                BuyVM.Payment = new Payments();


            }



            return View("Buy", BuyVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buy(BuyViewModel BuyVM)
        {
            ViewBag.ErrMessage = "";

            string pId = Colors.RandomString(18);

            // find the product that being sold
            var product = db.Products.Find(BuyVM.ProductId);

            // create an object of Purchase nad set the fields
            var purchase = new Purchase();
            purchase.Payment = BuyVM.Payment;
            purchase.Product = product;
            purchase.PurchaseDate = DateTime.Now;
            purchase.PurchaseId = pId;

            // check if purchase is from a user
            ApplicationUser currentUser = null;
            if (User.Identity.IsAuthenticated)
            {
                string id = User.Identity.GetUserId();
                currentUser = db.Users.FirstOrDefault(x => x.Id == id);
            }           
            purchase.User = currentUser ;

            // start the shipping process for the purchase 
            Shipping shipping = new Shipping();
            shipping.address = BuyVM.address;
            shipping.Purchase = purchase;
            shipping.ShippingDate = DateTime.Now.AddDays(3);

            // check for quantity and inventory
            if (BuyVM.PurchaseQuantity < 0)
            {
                TempData["ErrMessage"] = "Quantity must be between 1 to 10 units";
                return RedirectToAction("Buy", "Products", new { id = product.ProductId });
            }
                
            

            if ((product.Quantity - BuyVM.PurchaseQuantity) < 0)
            {
                TempData["ErrMessage"] = "Short in Inventory! Maximun quantity: " + product.Quantity;
                return RedirectToAction("Buy", "Products", new { id = product.ProductId });
            }
            product.Quantity -= BuyVM.PurchaseQuantity;

            db.Purchases.Add(purchase);
            db.Shippings.Add(shipping);
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            //return View("AllProducts", db.Products.ToList());
            TempData["SuccessMessage"] = "your Purchase Number is: " + purchase.PurchaseId;
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

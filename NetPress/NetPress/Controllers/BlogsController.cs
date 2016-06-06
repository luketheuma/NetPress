using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetPress.Models;

namespace NetPress.Controllers
{
    public class BlogsController : Controller
    {
        private NetPressDbModel db = new NetPressDbModel();

        // GET: Blogs
        public ActionResult Index(string search, int? categoryId)
        {
            if (search == null)
                ViewBag.search = "";
            else
                ViewBag.search = search;
            ViewBag.categoryId = categoryId;
            ViewBag.categoryList = db.Categories.ToList();
            return View(db.Blogs.ToList());
        }

        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Blogs");
            List<Category> categories = db.Categories.ToList();
            List<Status> statuses = db.Statuses.ToList();
            List<SelectListItem> dropdownCategories = new List<SelectListItem>();
            List<SelectListItem> dropdownStatuses = new List<SelectListItem>();
            foreach (Category c in categories)
            {
                dropdownCategories.Add(new SelectListItem() { Text = c.CategoryName, Value = "" + c.CategoryID });
            }

            foreach (Status s in statuses)
            {
                dropdownStatuses.Add(new SelectListItem() { Text = s.StatusName, Value = "" + s.StatusID });
            }

            ViewData.Add("DropCategoryItems", dropdownCategories);
            ViewData.Add("DropStatusItems", dropdownStatuses);

            ViewBag.userid = db.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;

            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogId,Title,UserID,DateCreated,LastModified,Category,Status,Content")] Blog blog)
        {
            List<Category> categories = db.Categories.ToList();
            List<Status> statuses = db.Statuses.ToList();
            List<SelectListItem> dropdownCategories = new List<SelectListItem>();
            List<SelectListItem> dropdownStatuses = new List<SelectListItem>();
            foreach (Category c in categories)
            {
                dropdownCategories.Add(new SelectListItem() { Text = c.CategoryName, Value = "" + c.CategoryID });
            }

            foreach (Status s in statuses)
            {
                dropdownStatuses.Add(new SelectListItem() { Text = s.StatusName, Value = "" + s.StatusID });
            }

            ViewData.Add("DropCategoryItems", dropdownCategories);
            ViewData.Add("DropStatusItems", dropdownStatuses);
            var dateCreated = DateTime.Now.AddHours(2);
            blog.DateCreated = dateCreated;
            blog.LastModified = dateCreated;
            blog.UserID = db.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
               
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Blogs");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            List<Category> categories = db.Categories.ToList();
            List<Status> statuses = db.Statuses.ToList();
            List<SelectListItem> dropdownCategories = new List<SelectListItem>();
            List<SelectListItem> dropdownStatuses = new List<SelectListItem>();
            foreach (Category c in categories)
            {
                dropdownCategories.Add(new SelectListItem() { Text = c.CategoryName, Value = "" + c.CategoryID });
            }

            foreach (Status s in statuses)
            {
                dropdownStatuses.Add(new SelectListItem() { Text = s.StatusName, Value = "" + s.StatusID });
            }

            ViewData.Add("DropCategoryItems", dropdownCategories);
            ViewData.Add("DropStatusItems", dropdownStatuses);
            ViewBag.userid = blog.AspNetUser.Id;
            ViewBag.dateCreated = blog.DateCreated;
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogId,Title,UserID,DateCreated,LastModified,Category,Status,Content")] Blog blog)
        {
            List<Category> categories = db.Categories.ToList();
            List<Status> statuses = db.Statuses.ToList();
            List<SelectListItem> dropdownCategories = new List<SelectListItem>();
            List<SelectListItem> dropdownStatuses = new List<SelectListItem>();
            foreach (Category c in categories)
            {
                dropdownCategories.Add(new SelectListItem() { Text = c.CategoryName, Value = "" + c.CategoryID });
            }

            foreach (Status s in statuses)
            {
                dropdownStatuses.Add(new SelectListItem() { Text = s.StatusName, Value = "" + s.StatusID });
            }

            ViewData.Add("DropCategoryItems", dropdownCategories);
            ViewData.Add("DropStatusItems", dropdownStatuses);
            blog.LastModified = DateTime.Now.AddHours(2);
            //blog.UserID = db.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Blogs");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Blogs.Remove(blog);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
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

        public ActionResult homepage(string search, int? categoryId)
        {
            List<Blog> blogList = (categoryId == null) ? db.Blogs.ToList() : db.Blogs.Where(x => x.Category == categoryId).ToList();
            if (search != null && search != "null")
            {
                blogList = blogList.Where(x => x.Content.ToString().ToUpper().Contains(search.ToUpper())
                                            || x.Title.ToString().ToUpper().Contains(search.ToUpper())
                                            || x.CategoryObject.CategoryName.ToString().ToUpper().Contains(search.ToUpper())
                                            || x.AspNetUser.UserName.ToString().ToUpper().Contains(search.ToUpper())).ToList();
            }
            blogList = blogList.Where(x => x.StatusObject.StatusID == 1).ToList();
            blogList = blogList.OrderByDescending(x => x.LastModified).ToList();
            ViewBag.blogList = blogList;
            return PartialView("_item", blogList );
        }
        
        public ActionResult Userblogs(string search)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Blogs");
            
            List<Blog> b = db.Blogs.ToList();
            b = b.Where(x => x.AspNetUser.UserName == User.Identity.Name).ToList();
            if (search != null)
            {
                b = b.Where(x => x.Content.ToString().ToUpper().Contains(search.ToUpper())
                            || x.Title.ToString().ToUpper().Contains(search.ToUpper())
                            || x.CategoryObject.CategoryName.ToString().ToUpper().Contains(search.ToUpper())).ToList();
            }
            b = b.OrderByDescending(x => x.LastModified).ToList();
  
            ViewBag.blogList = b;
              
            return View();
        }

    }
}

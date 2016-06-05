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
        public ActionResult Index(string search)
        {
            ViewBag.search = search;
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
        public ActionResult Create([Bind(Include = "BlogId,Title,UserID,DateCreated,DateModified,Category,Status,Content")] Blog blog)
        {
            var dateCreated = DateTime.Now;
            blog.DateCreated = dateCreated;
            blog.DateModified = dateCreated;
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
        public ActionResult Edit([Bind(Include = "BlogId,Title,UserID,DateCreated,DateModified,Category,Status,Content")] Blog blog)
        {
            var dateCreated = DateTime.Now;
            blog.DateModified = dateCreated;
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

        public ActionResult homepage(string search)
        {
            List<Blog> blogList = db.Blogs.ToList();
            blogList = blogList.OrderByDescending(x => x.DateCreated).ToList();
            if (search != null)
            {
                blogList = blogList.Where(x => x.Content.ToString().ToUpper().Contains(search.ToUpper())
                                            || x.Title.ToString().ToUpper().Contains(search.ToUpper())
                                            || x.CategoryObject.CategoryName.ToString().ToUpper().Contains(search.ToUpper())
                                            || x.AspNetUser.UserName.ToString().ToUpper().Contains(search.ToUpper())).ToList();
            }
            ViewBag.blogList = blogList;
            return PartialView("_item", blogList );
        }
        

    }
}

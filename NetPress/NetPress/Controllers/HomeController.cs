using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetPress.Models;

namespace NetPress.Controllers
{
    public class HomeController : Controller
    {
        private NetPressDbModel db = new NetPressDbModel();

        public ActionResult Index(string search)
        {
            if (search == null)
                search = "";

            List<Blog> blogList = new List<Blog>();
            blogList = blogList.OrderByDescending(x => x.DateCreated).ToList();
            if (search != "")
            {
                blogList = blogList.Where(x => x.Content.ToString().ToUpper().Contains(search.ToUpper())).ToList();
            }
            ViewBag.blogList = blogList;
            return View(blogList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
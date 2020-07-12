using Microsoft.Ajax.Utilities;
using NewsWebApp.DAL;
using NewsWebApp.DAL.Tables;
using NewsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<NewsItem> model = new List<NewsItem>();
            using (var table = new Table_NewsItem())
            {
                model = table.GetNews();
            }

            return View("Index", model);
        }

        public ActionResult DeleteNews(int newsId)
        {
            using (var table = new Table_NewsItem())
            {
                table.DeleteNews(newsId);
            }

            return Index();
        }
    }
}
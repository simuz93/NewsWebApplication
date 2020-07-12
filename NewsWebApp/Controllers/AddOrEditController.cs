using log4net;
using NewsWebApp.DAL.Tables;
using NewsWebApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebApp.Controllers
{
    public class AddOrEditController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger("fileLogger");
        private readonly string[] fileAcceptedExtension = { ".jpg", ".jpeg", ".png"};

        public ActionResult Index()
        {
            return View("AddOrEdit");
        }

        public ActionResult Edit(int newsId)
        {
            NewsItem news;
            using (var table = new Table_NewsItem())
            {
                news = table.GetNews(n => n.ID == newsId).FirstOrDefault();
            }
            if (news != null)
            {
                ViewBag.ID = newsId;
                ViewBag.Error = String.Empty;
                return View("AddOrEdit", news);
            }
            else
            {
                ViewBag.Error = "News not found";
                return View("AddOrEdit");
            }
        }

        [HttpPost]
        public ActionResult AddNews(NewsItem news)
        {
            if (!CheckFields(news, out string errorMessage))
            {
                ViewBag.Error = errorMessage;
                return View("AddOrEdit", news);
            }
            else
            {
                int id = -1;
                using (var table = new Table_NewsItem())
                {
                    id = table.AddNews(news);
                }

                if (id > 0 && news.ImageFile != null)
                {
                    string relativePath = SaveFile(news.ImageFile, id);

                    using (var table = new Table_NewsItem())
                    {
                        news.ImagePath = relativePath;
                        table.UpdateNews(news);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditNews(NewsItem news)
        {
            if (!CheckFields(news, out string errorMessage))
            {
                ViewBag.Error = errorMessage;
                return View("AddOrEdit", news);
            }
            else
            {
                using (var table = new Table_NewsItem())
                {
                    table.UpdateNews(news);
                }

                if (news.ImageFile != null)
                {
                    SaveFile(news.ImageFile, news.ID);
                }
                
                return RedirectToAction("Index", "Home");
            }
        }

        private bool CheckFields(NewsItem news, out string errorMessage)
        {
            if (String.IsNullOrWhiteSpace(news.Title) ||
                String.IsNullOrWhiteSpace(news.Description) ||
                news.PublishingDatetime == default ||
                (news.ImageFile == null && String.IsNullOrEmpty(news.ImagePath)))
            {
                errorMessage = "Please fill every required field (*)";
                return false;
            }
            else if (!String.IsNullOrWhiteSpace(news.ExternalLink) && !news.ExternalLink.StartsWith("http"))
            {
                errorMessage = $"Please use a valid http or https link";
                return false;
            }
            else if (String.IsNullOrEmpty(news.ImagePath) && !fileAcceptedExtension.Contains(Path.GetExtension(news.ImageFile?.FileName)?.ToLower()))
            {
                errorMessage = $"Only {String.Join(", ", fileAcceptedExtension)} are accepted.";
                return false;
            }
            else
            {
                errorMessage = String.Empty;
                return true;
            }
        }

        private string SaveFile(HttpPostedFileBase file, int newsId) 
        {
            string result = null;
            try
            {
                string ext = Path.GetExtension(file.FileName).ToLower();
                string name = $"{newsId}_img{ext}";
                string dir = Server.MapPath("~/images");
                string fullName = Path.Combine(dir, name);
                string relativeName = Path.Combine("~/images", name);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (System.IO.File.Exists(fullName))
                {
                    System.IO.File.Delete(fullName);
                }

                file.SaveAs(fullName);

                result = relativeName;
            }
            catch(Exception ex)
            {
                log.Error($"Error saving image file for news with ID {newsId}", ex);
            }

            return result;

        }
    }
}
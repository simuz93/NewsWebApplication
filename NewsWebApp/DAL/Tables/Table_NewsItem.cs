using log4net;
using NewsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NewsWebApp.DAL.Tables
{
    public class Table_NewsItem : IDisposable
    {
        private DbNewsContext _context;
        private static readonly ILog log = LogManager.GetLogger("fileLogger");

        public Table_NewsItem()
        {
            this._context = new DbNewsContext();
        }

        public List<NewsItem> GetNews()
        {
            try
            {
                return _context.News.ToList();
            }
            catch (Exception ex)
            {
                log.Error("Error reading news from DB", ex);
                return null;
            }
        }

        public List<NewsItem> GetNews(Func<NewsItem, bool> filter)
        {
            try
            {
                return _context.News.Where(filter).ToList();
            }
            catch (Exception ex)
            {
                log.Error("Error reading news from DB", ex);
                return null;
            }
        }

        public NewsItem GetNews(int id)
        {
            try
            {
                return _context.News.FirstOrDefault(item => item.ID == id);
            }
            catch (Exception ex)
            {
                log.Error($"Error reading news from DB with ID {id}", ex);
                return null;
            }
        }

        public int AddNews(NewsItem item)
        {
            try
            {
                NewsItem added = _context.News.Add(item);
                _context.SaveChanges();
                return added.ID;
            }
            catch (Exception ex)
            {
                log.Error($"Error saving news to DB with Title {item.Title}", ex);
                return -1;
            }
        }

        public void UpdateNews(NewsItem item)
        {
            try
            {
                NewsItem news = _context.News.AsNoTracking().FirstOrDefault(x => x.ID == item.ID);
                if (news != null)
                {
                    news = item;
                    _context.Entry(item).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error updating news in DB with ID {item.ID}", ex);
            }
        }

        public void DeleteNews(int itemId)
        {
            try
            {
                NewsItem news = _context.News.FirstOrDefault(x => x.ID == itemId);
                if (news != null)
                {
                    _context.News.Remove(news);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error deleting news from DB with ID {itemId}", ex);
            }
        }

        public void Truncate()
        {
            try
            {
                _context.Database.ExecuteSqlCommand("TRUNCATE TABLE NewsItem");
            }
            catch (Exception ex)
            {
                log.Error($"Error truncating table NewsItem", ex);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
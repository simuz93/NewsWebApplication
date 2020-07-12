using log4net;
using NewsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace NewsWebApp.DAL
{
    public class DbNewsContext : DbContext
    {
        public DbSet<NewsItem> News { get; set; }

        public DbNewsContext() : base("DbNewsContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
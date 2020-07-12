using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NewsWebApp.Models
{

    public class NewsItem
    {
        [Key]
        [DatabaseGenerat‌ed(DatabaseGeneratedOp‌​tion.Identity)]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime PublishingDatetime { get; set; }
        public DateTime? ExpiringDatetime { get; set; }
        public string ExternalLink { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
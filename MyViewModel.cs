using James_Thew.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace James_Thew
{
    public class MyViewModel
    {
        public Subscription Subscription { get; set; }
        public List<Anouncement> Announcements { get; set; }
        public IEnumerable<Racipies_james> Recipes { get; set; }
        public IEnumerable<Tips> Tips { get; set; }
        public IEnumerable<FAQs> FAQs { get; set; }
        public IEnumerable<Profile_Image> img { get; set; }
        public James_ThewEntities1 DbObj { get; set; }
        public bool HasActiveSubscription { get; set; }
    }
}
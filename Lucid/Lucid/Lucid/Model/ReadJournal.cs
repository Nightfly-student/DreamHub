using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Model
{
    class ReadJournal
    {
        private int Id;

        public int id
        {
            get { return Id; }
            set { Id = value; }
        }
        public string title { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public bool isLucid { get; set; }
        public DateTime date { get; set; }
        public bool isPublic { get; set; }


        public ReadJournal(int id, string title, string description, string category, bool isLucid, DateTime date, bool isPublic)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.category = category;
            this.isLucid = isLucid;
            this.date = date;
            this.isPublic = isPublic;
        }
    }
}

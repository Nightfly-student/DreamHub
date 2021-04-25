using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Lucid.Model
{
    public class journalTable
    {
        public string dreamTitle { get; set; }
        public string dreamDesc { get; set; }
        public string dreamDate { get; set; }
        public int dreamLucid { get; set; }
        public int dreamPublic { get; set; }
        public string dreamNotes { get; set; }
        public string dreamVivid { get; set; }
        public string dreamAwareness { get; set; }
        public string dreamCategory { get; set; }
        public string userSession { get; set; }
        public List<string> tagList { get; set; }
        public int dreamLikes { get; set; }

        public journalTable()
        {

        }

        public journalTable(string dreamTitle, string dreamDesc, string dreamDate, string userSession)
        {
            this.dreamTitle = dreamTitle;
            this.dreamDesc = dreamDesc;
            this.dreamDate = dreamDate;
            this.userSession = userSession;
        }
    }
    
}

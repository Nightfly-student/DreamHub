using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Lucid.Model
{
    public class journalTableModel
    {
        public string journalTitle { get; set; }
        public string journalDesc { get; set; }
        public string journalDate { get; set; }
        public string userName { get; set; }
        public string journalNotes { get; set; }
        public string journalCategory { get; set; }
        public int journalLucid { get; set; }
        public string journalAwareness { get; set; }
        public string journalVivid { get; set; }
        public List<string> tagList { get; set; }
        public int dreamLikes { get; set; }


        public journalTableModel()
        {

        }
        public journalTableModel(string journalTitle, string journalDesc, string journalDate)
        {
            this.journalTitle = journalTitle;
            this.journalDesc = journalDesc;
            this.journalDate = journalDate;
        }
    }

    public class GroupedClientJournalModel : ObservableCollection<journalTableModel>
    {
        public string dreamDate { get; set; }
        public string userSession { get; set; }
    }
}

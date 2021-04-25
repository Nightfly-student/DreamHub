using Firebase.Database;
using Lucid.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Lucid.ViewModel
{
    class JournalPersonalModel : BindableObject
    {
        private ObservableCollection<GroupedClientJournalModel> journalItem;
        public string tag;
        public ObservableCollection<GroupedClientJournalModel> JournalItem
        {
            get { return journalItem; }
            set
            {
                journalItem = value;
                OnPropertyChanged();
            }
        }
        public ICommand RefreshList { get; set; }
        
        public JournalPersonalModel()
        {
            JournalItem = new ObservableCollection<GroupedClientJournalModel>();
            GetJournalInformation();
        }
        public JournalPersonalModel(string e)
        {
            JournalItem = new ObservableCollection<GroupedClientJournalModel>();
            OnTextChanged(e);
        }
        public async void GetJournalInformation()
        {
            JournalItem = new ObservableCollection<GroupedClientJournalModel>();
            AllView();
        }
        public async void OnTextChanged(string e)
        {
            JournalItem.Clear();

            JournalItem = new ObservableCollection<GroupedClientJournalModel>();
            FirebaseClient fc = new FirebaseClient("https://dreamhub-90140-default-rtdb.firebaseio.com/");
            var GetJournals = (await fc
                .Child("journalTable")
                .OnceAsync<journalTable>()).Select(item => new journalTable
                {
                    dreamDate = item.Object.dreamDate,
                    dreamTitle = item.Object.dreamTitle,
                    dreamDesc = item.Object.dreamDesc,
                    dreamLucid = item.Object.dreamLucid,
                    dreamPublic = item.Object.dreamPublic,
                    userSession = item.Object.userSession,
                    dreamAwareness = item.Object.dreamAwareness,
                    dreamCategory = item.Object.dreamCategory,
                    dreamNotes = item.Object.dreamNotes,
                    dreamVivid = item.Object.dreamVivid,
                    tagList = item.Object.tagList
                }).ToList().OrderByDescending(x => DateTime.ParseExact(x.dreamDate, "dd/MM/yyyy", null)).Where(i => i.dreamTitle.Contains(e.ToString()) || i.userSession.Contains(e.ToString()) || i.dreamDate.Contains(e.ToString()));

            var headergroup = GetJournals.Where(i => i.userSession == Preferences.Get("username", "")).Select(x => x.dreamDate).Distinct().ToList();


            foreach (var item in headergroup)
            {
                if (headergroup.Count != 0)
                {
                    var journalGroup = new GroupedClientJournalModel() { dreamDate = item };
                    var contents = GetJournals.Where(i => i.dreamDate == item).ToList();
                    if (contents.Count != 0)
                    {
                        foreach (var groupitems in contents)
                        {
                            if (groupitems.userSession == Preferences.Get("username", ""))
                            {
                                journalGroup.Add(new journalTableModel()
                                {
                                    journalTitle = groupitems.dreamTitle,
                                    journalDesc = groupitems.dreamDesc,
                                    journalDate = groupitems.dreamDate,
                                    journalNotes = groupitems.dreamNotes,
                                    journalAwareness = groupitems.dreamAwareness,
                                    journalVivid = groupitems.dreamVivid,
                                    journalCategory = groupitems.dreamCategory,
                                    journalLucid = groupitems.dreamLucid,
                                    tagList = groupitems.tagList
                                });
                            }
                        }
                        JournalItem.Add(journalGroup);
                    }
                }
            }
        }
        private async void AllView()
        {

            FirebaseClient fc = new FirebaseClient("https://dreamhub-90140-default-rtdb.firebaseio.com/");
            var GetJournals = (await fc
                .Child("journalTable")
                .OnceAsync<journalTable>()).Select(item => new journalTable
                {
                    dreamDate = item.Object.dreamDate,
                    dreamTitle = item.Object.dreamTitle,
                    dreamDesc = item.Object.dreamDesc,
                    dreamLucid = item.Object.dreamLucid,
                    dreamPublic = item.Object.dreamPublic,
                    userSession = item.Object.userSession,
                    dreamAwareness = item.Object.dreamAwareness,
                    dreamCategory = item.Object.dreamCategory,
                    dreamNotes = item.Object.dreamNotes,
                    dreamVivid = item.Object.dreamVivid,
                    tagList = item.Object.tagList
                }).ToList().OrderByDescending(x => DateTime.ParseExact(x.dreamDate, "dd/MM/yyyy", null));

            var headergroup = GetJournals.Where(i => i.userSession == Preferences.Get("username", "")).Select(x => x.dreamDate).Distinct().ToList();


            foreach (var item in headergroup)
            {
                if (headergroup.Count != 0)
                {
                    var journalGroup = new GroupedClientJournalModel() { dreamDate = item };
                    var contents = GetJournals.Where(i => i.dreamDate == item).ToList();
                    if (contents.Count != 0)
                    {
                        foreach (var groupitems in contents)
                        {
                            if (groupitems.userSession == Preferences.Get("username", ""))
                            {
                                journalGroup.Add(new journalTableModel()
                                {
                                    journalTitle = groupitems.dreamTitle,
                                    journalDesc = groupitems.dreamDesc,
                                    journalDate = groupitems.dreamDate,
                                    journalNotes = groupitems.dreamNotes,
                                    journalAwareness = groupitems.dreamAwareness,
                                    journalVivid = groupitems.dreamVivid,
                                    journalCategory = groupitems.dreamCategory,
                                    journalLucid = groupitems.dreamLucid,
                                    tagList = groupitems.tagList
                                });
                            }
                        }
                        JournalItem.Add(journalGroup);
                    }
                }
            }
        }
    }
}

using Firebase.Database;
using Lucid.Model;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Lucid.ViewModel
{
    class JournalViewModel : BindableObject
    {
        private ObservableCollection<GroupedClientJournalModel> journalItem;
        public ObservableCollection<GroupedClientJournalModel> JournalItem
        {
            get { return journalItem; }
            set
            {
                journalItem = value;
                OnPropertyChanged();
            }
        }

        public JournalViewModel()
        {
            JournalItem = new ObservableCollection<GroupedClientJournalModel>();
            GetJournalInformation();
        }
        public JournalViewModel(string e)
        {
            JournalItem = new ObservableCollection<GroupedClientJournalModel>();
            OnTextChanged(e);
        }
        public JournalViewModel(string e, DateTime date)
        {
            JournalItem = new ObservableCollection<GroupedClientJournalModel>();
            filterView(e);
        }
        public JournalViewModel(string e, string es)
        {
            JournalItem = new ObservableCollection<GroupedClientJournalModel>();
            OnTextChanged(e, es);
        }
        public JournalViewModel(string userSession, int i)
        {
            JournalItem = new ObservableCollection<GroupedClientJournalModel>();
            ProfileView(userSession);
        }
        public void PerformRefresh() 
        {
            GetJournalInformation();
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
                    tagList = item.Object.tagList,
                    dreamLikes = item.Object.dreamLikes
                }).ToList().OrderByDescending(x => DateTime.ParseExact(x.dreamDate, "dd/MM/yyyy", null)).Where(i => i.dreamTitle.Contains(e.ToString()) || i.userSession.Contains(e.ToString()) || i.dreamDate.Contains(e.ToString()));

            var headergroup = GetJournals.Where(i => i.dreamPublic == 1).Select(x => x.dreamDate).Distinct().ToList();

            foreach (var item in headergroup)
            {
                var journalGroup = new GroupedClientJournalModel() { dreamDate = item };
                var contents = GetJournals.Where(i => i.dreamDate == item).OrderByDescending(x => x.dreamLikes).ToList();

                if (contents.Count != 0)
                {
                    foreach (var groupitems in contents)
                    {
                        if (groupitems.dreamPublic == 1)
                        {
                            journalGroup.Add(new journalTableModel() { journalTitle = groupitems.dreamTitle, journalDesc = groupitems.dreamDesc, journalDate = groupitems.dreamDate, userName = groupitems.userSession, journalNotes = groupitems.dreamNotes, journalAwareness = groupitems.dreamAwareness, journalCategory = groupitems.dreamCategory, journalLucid = groupitems.dreamLucid, journalVivid = groupitems.dreamVivid, tagList = groupitems.tagList, dreamLikes = groupitems.dreamLikes });
                        }
                    }
                    JournalItem.Add(journalGroup);
                }
            }
        }
        public async void OnTextChanged(string e, string es)
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
                    tagList = item.Object.tagList,
                    dreamLikes = item.Object.dreamLikes
                }).ToList().OrderByDescending(x => DateTime.ParseExact(x.dreamDate, "dd/MM/yyyy", null)).Where(i => i.dreamTitle.Contains(e.ToString()) || i.userSession.Contains(e.ToString()) || i.dreamDate.Contains(e.ToString()) || i.userSession == es);

            var headergroup = GetJournals.Where(i => i.dreamPublic == 1).Select(x => x.dreamDate).Distinct().ToList();

            foreach (var item in headergroup)
            {
                var journalGroup = new GroupedClientJournalModel() { dreamDate = item };
                var contents = GetJournals.Where(i => i.dreamDate == item).OrderByDescending(x => x.dreamLikes).ToList();

                if (contents.Count != 0)
                {
                    foreach (var groupitems in contents)
                    {
                        if (groupitems.dreamPublic == 1)
                        {
                            journalGroup.Add(new journalTableModel() { journalTitle = groupitems.dreamTitle, journalDesc = groupitems.dreamDesc, journalDate = groupitems.dreamDate, userName = groupitems.userSession, journalNotes = groupitems.dreamNotes, journalAwareness = groupitems.dreamAwareness, journalCategory = groupitems.dreamCategory, journalLucid = groupitems.dreamLucid, journalVivid = groupitems.dreamVivid, tagList = groupitems.tagList, dreamLikes = groupitems.dreamLikes });
                        }
                    }
                    JournalItem.Add(journalGroup);
                }
            }
        }
        private async void filterView(string category)
        {
            JournalItem.Clear();

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
                    tagList = item.Object.tagList,
                    dreamLikes = item.Object.dreamLikes
                }).ToList().OrderByDescending(x => DateTime.ParseExact(x.dreamDate, "dd/MM/yyyy", null)).Where(i => i.dreamCategory == category);

            var headergroup = GetJournals.Where(i => i.dreamPublic == 1).Select(x => x.dreamDate).Distinct().ToList();

            foreach (var item in headergroup)
            {
                var journalGroup = new GroupedClientJournalModel() { dreamDate = item };
                var contents = GetJournals.Where(i => i.dreamDate == item).OrderByDescending(x => x.dreamLikes).ToList();

                if (contents.Count != 0)
                {
                    foreach (var groupitems in contents)
                    {

                        if (groupitems.dreamPublic == 1)
                        {
                            journalGroup.Add(new journalTableModel() { journalTitle = groupitems.dreamTitle, journalDesc = groupitems.dreamDesc, journalDate = groupitems.dreamDate, userName = groupitems.userSession, journalNotes = groupitems.dreamNotes, journalAwareness = groupitems.dreamAwareness, journalCategory = groupitems.dreamCategory, journalLucid = groupitems.dreamLucid, journalVivid = groupitems.dreamVivid, tagList = groupitems.tagList, dreamLikes = groupitems.dreamLikes });
                        }
                    }
                    JournalItem.Add(journalGroup);
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
                    tagList = item.Object.tagList,
                    dreamLikes = item.Object.dreamLikes
                }).ToList().OrderByDescending(x => DateTime.ParseExact(x.dreamDate, "dd/MM/yyyy", null));

            var headergroup = GetJournals.Where(i => i.dreamPublic == 1).Select(x => x.dreamDate).Distinct().ToList();

            foreach (var item in headergroup)
            {
                var journalGroup = new GroupedClientJournalModel() { dreamDate = item };
                var contents = GetJournals.Where(i => i.dreamDate == item).OrderByDescending(x => x.dreamLikes).ToList();

                if (contents.Count != 0)
                {
                    foreach (var groupitems in contents)
                    {

                        if (groupitems.dreamPublic == 1)
                        {
                            journalGroup.Add(new journalTableModel() { journalTitle = groupitems.dreamTitle, journalDesc = groupitems.dreamDesc, journalDate = groupitems.dreamDate, userName = groupitems.userSession, journalNotes = groupitems.dreamNotes, journalAwareness = groupitems.dreamAwareness, journalCategory = groupitems.dreamCategory, journalLucid = groupitems.dreamLucid, journalVivid = groupitems.dreamVivid, tagList = groupitems.tagList, dreamLikes = groupitems.dreamLikes});
                        }
                    }
                    JournalItem.Add(journalGroup);
                }
            }
        }
        private async void ProfileView(string userSession)
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
                    tagList = item.Object.tagList,
                    dreamLikes = item.Object.dreamLikes
                }).ToList().OrderByDescending(x => DateTime.ParseExact(x.dreamDate, "dd/MM/yyyy", null)).Where(i => i.userSession == userSession);

            var headergroup = GetJournals.Where(i => i.dreamPublic == 1).Select(x => x.dreamDate).Distinct().ToList();

            foreach (var item in headergroup)
            {
                var journalGroup = new GroupedClientJournalModel() { dreamDate = item };
                var contents = GetJournals.Where(i => i.dreamDate == item).OrderByDescending(x => x.dreamLikes).ToList();

                if (contents.Count != 0)
                {
                    foreach (var groupitems in contents)
                    {

                        if (groupitems.dreamPublic == 1)
                        {
                            journalGroup.Add(new journalTableModel() { journalTitle = groupitems.dreamTitle, journalDesc = groupitems.dreamDesc, journalDate = groupitems.dreamDate, userName = groupitems.userSession, journalNotes = groupitems.dreamNotes, journalAwareness = groupitems.dreamAwareness, journalCategory = groupitems.dreamCategory, journalLucid = groupitems.dreamLucid, journalVivid = groupitems.dreamVivid, tagList = groupitems.tagList, dreamLikes = groupitems.dreamLikes });
                        }
                    }
                    JournalItem.Add(journalGroup);
                }
            }
        }
    }
}

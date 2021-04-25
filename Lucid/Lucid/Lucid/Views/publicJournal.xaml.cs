using FireSharp;
using Firebase.Database;
using Firebase.Database.Query;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Lucid.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Lucid.ViewModel;

namespace Lucid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class publicJournal : ContentPage
    {

        private const string BasePath = "https://dreamhub-90140-default-rtdb.firebaseio.com/";
        private const string FirebaseSecret = "3w5A2pqhjMiEl05pow8Kw13e0ku6yTmY7sTxR3nw";
        private static FireSharp.FirebaseClient client;

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = FirebaseSecret,
            BasePath = BasePath
        };

        private string dreamTitles, dreamDescs, dreamDates, dreamCategorys, dreamNotess, dreamJournalUser, dreamAwarenesss, dreamVivids;
        private int DreamLucids, DreamLikes;
        private List<string> tagReport;
        private CommentViewModel c;
        public publicJournal(string title, string desc, string date, string awareness, string vivid, string category, int isLucid, List<string> tags)
        {
            try
            {
                client = new FireSharp.FirebaseClient(ifc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
            InitializeComponent();

            journalName.Text = title;
            journalDesc.Text = desc;
            journalDate.Text = date;
            journalVivid.Text = "" + vivid;

            if (isLucid == 1)
            {
                journalLucid.Text = "Lucid Dream";
            }
            else
            {
                journalLucid.Text = "Not Lucid";
            }
            journalCategory.Text = category;
            journalAwareness.Text = "" + awareness;

            if (tags != null)
            {
                foreach (var t in tags)
                {
                    tagLabel.Text += t + "   ";
                }
            }
            else
            {
                noTagClear.IsVisible = false;
            }
            likeLabel.TextColor = Color.White;
            retrieveLikes();          

            BindingContext = new CommentViewModel();
           
        }
        private void retrieveData()
        {
            FirebaseResponse res = client.Get(@"journalTable");
            Dictionary<string, journalTable> data = JsonConvert.DeserializeObject<Dictionary<string, journalTable>>(res.Body.ToString());
            PopulateRTB(data);
        }
        private void PopulateRTB(Dictionary<string, journalTable> record)
        {

            foreach (var item in record)
            {
                if (item.Value.dreamDesc == journalDesc.Text && item.Value.dreamTitle == journalName.Text)
                {
                    Preferences.Set("reportKey", item.Key);
                    Preferences.Set("reportUser", item.Value.userSession);
                    Preferences.Set("dreamId", item.Key);
                    Preferences.Set("DreamLikeKey", item.Key);
                    DreamLikes = item.Value.dreamLikes;
                    dreamTitles = item.Value.dreamTitle;
                    dreamDescs = item.Value.dreamDesc;
                    dreamAwarenesss = item.Value.dreamAwareness;
                    dreamCategorys = item.Value.dreamCategory;
                    dreamDates = item.Value.dreamDate;
                    dreamVivids = item.Value.dreamVivid;
                    DreamLucids = item.Value.dreamLucid;
                    dreamNotess = item.Value.dreamNotes;
                    tagReport = item.Value.tagList;
                    dreamJournalUser = item.Value.userSession;

                }
            }
        }
        async void OnTappReportDream(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert("Report Dream Entry", "Are you sure?", "Yes", "No");
            if (answer)
            {
                ReportLabel.IsEnabled = false;
                retrieveData();
                retrieveReports();
                await DisplayAlert("Report Dream Entry", "Succesfully Reported Dream Entry, Thanks for helping!", "OK");
            }
        }
        private async void inDatabaseAsync()
        {
            var dream = Preferences.Get("reportKey", "");
            var user = Preferences.Get("reportUser", "");
            Firebase.Database.FirebaseClient fc = new Firebase.Database.FirebaseClient("https://dreamhub-90140-default-rtdb.firebaseio.com/");
            var result = await fc
                .Child("reportedDreams")
                .PostAsync(new reportDream() { dreamId = dream, userSession = user, reports = 1 });
        }
        private void retrieveReports()
        {
            FirebaseResponse res = client.Get(@"reportedDreams");
            Dictionary<string, reportDream> data = JsonConvert.DeserializeObject<Dictionary<string, reportDream>>(res.Body.ToString());
            PopulateReports(data);

        }
        private void PopulateReports(Dictionary<string, reportDream> record)
        {
            bool exists = false;
            foreach (var item in record)
            {
                if (item.Value.dreamId == Preferences.Get("reportKey", "") && item.Value.userSession == Preferences.Get("reportUser", ""))
                {
                    Preferences.Set("entryKeyReport", item.Key);
                    exists = true;
                    if (item.Value.reports < 10)
                    {
                        int reported = item.Value.reports + 1;

                        reportDream rep = new reportDream()
                        {
                            reports = reported,
                            userSession = Preferences.Get("reportUser", ""),
                            dreamId = Preferences.Get("reportKey", "")
                        };

                        var res = client.Update(@"reportedDreams/" + Preferences.Get("entryKeyReport", ""), rep);
                    }

                    if (item.Value.reports == 10)
                    {
                        setData();
                    }
                }
            }
            if (!exists)
            {
                inDatabaseAsync();
            }
        }
        private void setData()
        {
            var pub = 0;
            journalTable js = new journalTable()
            {
                dreamPublic = pub,
                dreamLucid = DreamLucids,
                dreamAwareness = dreamAwarenesss,
                dreamDate = dreamDates,
                dreamCategory = dreamCategorys,
                dreamNotes = dreamNotess,
                dreamVivid = dreamVivids,
                dreamDesc = dreamDescs,
                dreamTitle = dreamTitles,
                tagList = tagReport,
                dreamLikes = DreamLikes,
                userSession = dreamJournalUser
            };
            var res = client.Update(@"journalTable/" + Preferences.Get("reportKey", ""), js);
        }
        private void retrieveLikes()
        {
            FirebaseResponse res = client.Get(@"entryLikes");
            Dictionary<string, likeDream> data = JsonConvert.DeserializeObject<Dictionary<string, likeDream>>(res.Body.ToString());
            retrieveData();
            PopulateLikes(data);
        }
        private void PopulateLikes(Dictionary<string, likeDream> record)
        {
            bool exists = false;
            foreach (var item in record) 
            {
                if (item.Value.dreamId == Preferences.Get("DreamLikeKey", "") && item.Value.userId == Preferences.Get("username", ""))
                {
                    likeLabel.TextColor = Color.Orange;
                    exists = true;
                }
            }
            if(!exists)
            {
                likeLabel.TextColor = Color.White;
            }
               
        }
        private async void inLikeAsync()
        {
            var dream = Preferences.Get("DreamLikeKey", "");
            var user = Preferences.Get("username", "");
            Firebase.Database.FirebaseClient fc = new Firebase.Database.FirebaseClient("https://dreamhub-90140-default-rtdb.firebaseio.com/");
            var result = await fc
                .Child("entryLikes")
                .PostAsync(new likeDream() { dreamId = dream, userId = user, hasLiked = 1 });
        }
        private void setLikeOnDream()
        {
            int like = DreamLikes +1;
            DreamLikes = like;
            journalTable js = new journalTable()
            {
                dreamPublic = 1,
                dreamLucid = DreamLucids,
                dreamAwareness = dreamAwarenesss,
                dreamDate = dreamDates,
                dreamCategory = dreamCategorys,
                dreamNotes = dreamNotess,
                dreamVivid = dreamVivids,
                dreamDesc = dreamDescs,
                dreamTitle = dreamTitles,
                tagList = tagReport,
                dreamLikes = like,
                userSession = dreamJournalUser


            };
            var res = client.Update(@"journalTable/" + Preferences.Get("DreamLikeKey", ""), js);
        }
        private void deleteLikeKey()
        {
            int like = DreamLikes - 1;
            DreamLikes = like;
            journalTable js = new journalTable()
            {
                dreamPublic = 1,
                dreamLucid = DreamLucids,
                dreamAwareness = dreamAwarenesss,
                dreamDate = dreamDates,
                dreamCategory = dreamCategorys,
                dreamNotes = dreamNotess,
                dreamVivid = dreamVivids,
                dreamDesc = dreamDescs,
                dreamTitle = dreamTitles,
                tagList = tagReport,
                dreamLikes = like,
                userSession = dreamJournalUser


            };
            var res = client.Update(@"journalTable/" + Preferences.Get("DreamLikeKey", ""), js);
            var del = client.Delete(@"entryLikes/" + Preferences.Get("DeleteKey", ""));
        }
        private void InsertLike()
        {
            FirebaseResponse res = client.Get(@"entryLikes");
            Dictionary<string, likeDream> data = JsonConvert.DeserializeObject<Dictionary<string, likeDream>>(res.Body.ToString());
            InsertedLike(data);
        }
        private void InsertedLike(Dictionary<string, likeDream> record)
        {
            bool exist = false;
            foreach (var item in record)
            {
                if (item.Value.dreamId == Preferences.Get("DreamLikeKey", "") && item.Value.userId == Preferences.Get("username", ""))
                {
                    Preferences.Set("DeleteKey", item.Key);
                    exist = true;
                    deleteLikeKey();
                    likeLabel.TextColor = Color.White;
                }
            }
            if(!exist)
            {
                inLikeAsync();
                setLikeOnDream();
                likeLabel.TextColor = Color.Orange;
            }

        }
        async void OnTapLikeDream(object sender, EventArgs args)
        {
            retrieveData();
            InsertLike();
        }
        async void commentDream(object sender, EventArgs args)
        {
            string result = await DisplayPromptAsync(dreamTitles, "Comment");

            if (!string.IsNullOrEmpty(result))
            {
                InsertComment(result);
            }
        }
        private async void InsertComment(string commentMessage)
        {
            var username = Preferences.Get("username", "");
            var dreanIdenitiy = Preferences.Get("dreamId", "");
            Firebase.Database.FirebaseClient fc = new Firebase.Database.FirebaseClient("https://dreamhub-90140-default-rtdb.firebaseio.com/");
        var result = await fc
            .Child("Comment")
            .PostAsync(new Comment() {CommentDate = DateTime.Now.ToString("dd/MM/yyyy"), CommentUser = username, dreamId = dreanIdenitiy, CommentText = commentMessage });
            BindingContext = new CommentViewModel();
        }
        private async void onCommentTap(object sender, ItemTappedEventArgs e)
        {
            var dataItem = e.Item as Comment;
            string action = await DisplayActionSheet("Go-To?", "Cancel", null, "Profile");

            if (dataItem.CommentUser.Contains(" 👑 "))
            {

                if (action == "Profile")
                {
                 await Navigation.PushAsync(new ProfileJournal(dataItem.CommentUser.Replace(" 👑 ", "")));
                }
            } else if (action == "Profile")
            {
                await Navigation.PushAsync(new ProfileJournal(dataItem.CommentUser));
            }
        }

    }
}
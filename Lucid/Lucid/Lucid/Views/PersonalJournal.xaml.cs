using Firebase.Database.Query;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Lucid.Model;
using Lucid.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lucid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonalJournal : ContentPage
    {
        private const string BasePath = "https://dreamhub-90140-default-rtdb.firebaseio.com/";
        private const string FirebaseSecret = "3w5A2pqhjMiEl05pow8Kw13e0ku6yTmY7sTxR3nw";
        private static FireSharp.FirebaseClient client;

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = FirebaseSecret,
            BasePath = BasePath
        };
        string dreamTitleForComment;
        public PersonalJournal(string title, string desc, string date, string notes, string awareness, string vivid, string category, int isLucid, List<string> tags)
        {
            try
            {
                client = new FirebaseClient(ifc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
            InitializeComponent();

            journalName.Text = title;
            journalDesc.Text = desc;
            journalDate.Text = date;
            dreamTitleForComment = title;
            if (notes == null)
            {
                journalNotes.Text = "No notes added";
            }
            else
            {
                journalNotes.Text = notes;
            }
            journalVivid.Text = "" + vivid;

            if (isLucid == 1)
            {
                journalLucid.Text = "Lucid Dream";
            } else
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
            } else
            {
                noTagClear.IsVisible = false;
            }
            retrieveData();
            ToolbarItems.Add(new ToolbarItem("edit", "edit", () =>
            {
                try
                {
                    client = new FirebaseClient(ifc);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error " + ex.Message);
                }
                retrieveData();
                navigationEditJournal();

            }));
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
                    Preferences.Set("dreamId", item.Key);
                    Preferences.Set("journalKey", item.Key);
                    Preferences.Set("journalTitle", item.Value.dreamTitle);
                    Preferences.Set("journalDesc", item.Value.dreamDesc);
                }
            }
        }
        private async void navigationEditJournal()
        {
            await Navigation.PushAsync(new EditJournal());
        }
        private void deleteData()
        {
            FirebaseResponse res = client.Get(@"journalTable");
            Dictionary<string, journalTable> data = JsonConvert.DeserializeObject<Dictionary<string, journalTable>>(res.Body.ToString());
            deleteRecord(data);
        }
        void deleteEntryComments(Dictionary<string, Comment> record)
        {
                foreach (var item in record)
                {
                    if (item.Value.dreamId == Preferences.Get("dreamId", ""))
                    {
                        var result = client.Delete(@"Comment/" + item.Key);
                    }
                }
        }
        private void deleteComment()
        {
            FirebaseResponse res = client.Get(@"Comment");
            Dictionary<string, Comment> data = JsonConvert.DeserializeObject<Dictionary<string, Comment>>(res.Body.ToString());
            deleteComments(data);
        }
        void deleteComments(Dictionary<string, Comment> record)
        {
            foreach (var item in record)
            {
                if (item.Value.CommentText == Preferences.Get("deleteComment", "") && item.Value.CommentUser == Preferences.Get("deleteCommentUser", ""))
                {
                    var result = client.Delete(@"Comment/" + item.Key);
                }
            }
        }
        private async void onCommentTap(object sender, ItemTappedEventArgs e)
        {
            var dataItem = e.Item as Comment;
            string action = await DisplayActionSheet("Go-To?", "Cancel", null, "Profile", "Delete");

            if (action == "Delete")
            {
                bool answer = await DisplayAlert("Delete Comment", "Are you sure?", "Yes", "No");
                if (answer)
                {
                    Preferences.Set("deleteComment", dataItem.CommentText);
                    Preferences.Set("deleteCommentUser", dataItem.CommentUser);
                    deleteComment();
                    BindingContext = new CommentViewModel();
                }
            }
            if (action == "Profile")
            {
                if (dataItem.CommentUser != Preferences.Get("username", "") || dataItem.CommentUser.Replace(" 👑 ", "") != Preferences.Get("username", ""))
                {
                    await DisplayAlert("Error", "You cannot look up yourself", "OK");
                } else
                {
                    await Navigation.PushAsync(new ProfileJournal(dataItem.CommentUser));
                }
            }
        }
        void deleteRecord(Dictionary<string, journalTable> record)
        {
            foreach (var item in record)
            {
                if (item.Value.dreamDesc == journalDesc.Text && item.Value.dreamTitle == journalName.Text)
                {
                   var result = client.Delete(@"journalTable/" + item.Key);
                }
            }

            FirebaseResponse res = client.Get(@"Comment");
            Dictionary<string, Comment> data = JsonConvert.DeserializeObject<Dictionary<string, Comment>>(res.Body.ToString());
            deleteEntryComments(data);

        }
        private async void deleteTap(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert("Alert", "Are you sure?", "Yes", "No");
            if (answer)
            {
                var vUpdatedPage = new Page2();
                Navigation.InsertPageBefore(vUpdatedPage, this);
                await Navigation.PopAsync();
                await Navigation.PushAsync(new Page2());
                deleteData();
            }
        }
        async void commentDream(object sender, EventArgs args)
        {
            string result = await DisplayPromptAsync(dreamTitleForComment, "Comment");

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
                .PostAsync(new Comment() { CommentDate = DateTime.Now.ToString("dd/MM/yyyy"), CommentUser = username + " 👑 ", dreamId = dreanIdenitiy, CommentText = commentMessage });
            BindingContext = new CommentViewModel();
        }
    }
}
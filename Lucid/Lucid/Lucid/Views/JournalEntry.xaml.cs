using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Lucid.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace Lucid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JournalEntry : ContentPage
    {
        Connection.Connection con = new Connection.Connection();
        private SqlConnection dbConnection;
        private int isLucid;
        private int isPublic;
        private string cateGory;
        private int counter = 0;
        private List<string> result = new List<string>();
        public string title;
        public string desc;
        public string notes;

        private const string BasePath = "https://dreamhub-90140-default-rtdb.firebaseio.com/";
        private const string FirebaseSecret = "3w5A2pqhjMiEl05pow8Kw13e0ku6yTmY7sTxR3nw";
        private static FireSharp.FirebaseClient client;

        IFirebaseConfig ifc = new FireSharp.Config.FirebaseConfig()
        {
            AuthSecret = FirebaseSecret,
            BasePath = BasePath
        };

        public JournalEntry()
        {
            InitializeComponent();

            try
            {
                client = new FireSharp.FirebaseClient(ifc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }

            NavigationPage.SetHasBackButton(this, false);
        }
        private async void SafeDream(object sender, EventArgs e)
        {
            PickedCategory(); 
            if (String.IsNullOrEmpty(DreamDesc.Text))
            {
                await DisplayAlert("Alert", "Please fill in a description", "OK");
            } 
            else if (String.IsNullOrEmpty(DreamTitle.Text))
            {
                await DisplayAlert("Alert", "Please fill in a title", "OK");
            }
            else
            {
                SendData();
            }

        }
        async void OnCheckBoxLucid(object sender, CheckedChangedEventArgs e)
        {
            if (DreamIsLucid.IsChecked)
            {
                bool answer = await DisplayAlert("Alert", "Are you sure you want your dream to be public?", "Yes", "No");
                if (answer)
                {
                    isLucid = 1;
                }
            }
            else
            {              
                isLucid = 0;
            }

        }
        void OnCheckBoxPublic(object sender, CheckedChangedEventArgs e)
        {
            if (DreamIsPublic.IsChecked)
            {
                isPublic = 1;
            }
            else
            {
                isPublic = 0;
            }
        }
        private void VividSliderOnValueChanged(object sender, ValueChangedEventArgs e)
        {
            double StepValue = 1.0;

            var newStep = Math.Round(e.NewValue / StepValue);
            VividSlider.Value = newStep * StepValue;

            switch(VividSlider.Value)
            {
                case 0:
                    VividLevelCaller.Text = "None";
                    break;
                case 1:
                    VividLevelCaller.Text = "Bad";
                    break;
                case 2:
                    VividLevelCaller.Text = "Poor";
                    break;
                case 3:
                    VividLevelCaller.Text = "Normal";
                    break;
                case 4:
                    VividLevelCaller.Text = "Good";
                    break;
                case 5:
                    VividLevelCaller.Text = "Perfect";
                    break;
            }
        }
        private void AwarenessSliderOnValueChanged(object sender, ValueChangedEventArgs e)
        {
            double StepValue = 1.0;

            var newStep = Math.Round(e.NewValue / StepValue);
            AwarenessSlider.Value = newStep * StepValue;

            switch (AwarenessSlider.Value)
            {
                case 0:
                    AwarenessLevelCaller.Text = "None";
                    break;
                case 1:
                    AwarenessLevelCaller.Text = "Bad";
                    break;
                case 2:
                    AwarenessLevelCaller.Text = "Poor";
                    break;
                case 3:
                    AwarenessLevelCaller.Text = "Normal";
                    break;
                case 4:
                    AwarenessLevelCaller.Text = "Good";
                    break;
                case 5:
                    AwarenessLevelCaller.Text = "Perfect";
                    break;
            }
        }
        private void PickedCategory()
        {
            if (DreamCategory.SelectedIndex == -1)
            {
                cateGory = DreamCategory.Items[0];
            }
            else
            {
                cateGory = DreamCategory.Items[DreamCategory.SelectedIndex];
            }
        }
        private async void SendData()
        {
            Page2 page2 = new Page2();
            title = DreamTitle.Text;
            notes = DreamNotes.Text;
            desc = DreamDesc.Text;

            var username = Preferences.Get("username", "");
            Firebase.Database.FirebaseClient fc = new Firebase.Database.FirebaseClient("https://dreamhub-90140-default-rtdb.firebaseio.com/");
            var result = await fc
                .Child("journalTable")
                .PostAsync(new journalTable() { dreamTitle = DreamTitle.Text, dreamDesc = DreamDesc.Text, dreamDate = DateTime.Now.ToString("dd/MM/yyyy"), dreamPublic = isPublic, dreamLucid = isLucid, dreamNotes = DreamNotes.Text, dreamAwareness = AwarenessLevelCaller.Text, dreamVivid = VividLevelCaller.Text,dreamCategory = cateGory,userSession = username, tagList = this.result, dreamLikes = 0 });
            updateUser();
            var vUpdatedPage = new Page2();
            Navigation.InsertPageBefore(vUpdatedPage, this);
            await Navigation.PopAsync();
            await Navigation.PushAsync((new Page2()));
        }
        private void updateUser()
        {
            FirebaseResponse res = client.Get(@"Users");
            Dictionary<string, UserNameCreation> data = JsonConvert.DeserializeObject<Dictionary<string, UserNameCreation>>(res.Body.ToString());
            PopulateRTB(data);
        }
        private void PopulateRTB(Dictionary<string, UserNameCreation> record)
        {
            foreach (var item in record)
            {
                if (item.Value.username == Preferences.Get("username", ""))
                {
                    UserNameCreation user = new UserNameCreation()
                    {
                        username = Preferences.Get("username", ""),
                        userMail = item.Value.userMail,
                        lucidCount = item.Value.lucidCount + isLucid,
                        LucidTokens = item.Value.LucidTokens + 50
                    };
                    var set = client.Set("Users/" + item.Key, user);
                }
            }
        }
        private async void AddDreamTag(object sender, EventArgs e)
        {        
            string answer = await DisplayPromptAsync("Dream Tag", "Type your dream tag" );

            if(answer != null)
            {
                result.Add(answer);
                lblTag.Text += result[counter] + ", ";
            }
            counter++;
        }
        private async void deleteDreamTag(object sender, EventArgs e)
        {
            if (counter == 0)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "No Tag Deleted", "Ok");
            } else
            {
                result.RemoveAt(counter - 1);
                lblTag.Text = "";
                foreach (var r in result)
                {
                    lblTag.Text += r + ", ";
                }
                counter--;
            }

        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                if (result) await this.Navigation.PopAsync();
            });
            return true;
        }

    }
}
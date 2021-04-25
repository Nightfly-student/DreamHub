using Firebase.Database;
using Firebase.Database.Query;
using Lucid.Model;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
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
    public partial class ChooseYourUsername : ContentPage
    {
        private const string BasePath = "https://dreamhub-90140-default-rtdb.firebaseio.com/";
        private const string FirebaseSecret = "3w5A2pqhjMiEl05pow8Kw13e0ku6yTmY7sTxR3nw";
        private static FireSharp.FirebaseClient client;

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = FirebaseSecret,
            BasePath = BasePath
        };
        public bool exist = true;
        public ChooseYourUsername()
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
        }
        private async void userSession()
        {
                var userSession = Preferences.Get("userSession", "");
                Firebase.Database.FirebaseClient fc = new Firebase.Database.FirebaseClient("https://dreamhub-90140-default-rtdb.firebaseio.com/");
                var result = await fc
                    .Child("Users")
                    .PostAsync(new UserNameCreation() { username = UsernameEntry.Text, userMail = userSession, lucidCount = 0, LucidTokens = 0 });

                Preferences.Set("username", UsernameEntry.Text);
            await Navigation.PushModalAsync(new Main());
        }
        private void retrieveData()
        {
                FirebaseResponse res = client.Get(@"Users");
                Dictionary<string, UserNameCreation> data = JsonConvert.DeserializeObject<Dictionary<string, UserNameCreation>>(res.Body.ToString());
                PopulateRTB(data);
        }
        async void PopulateRTB(Dictionary<string, UserNameCreation> record)
        {
            bool doubled = false;

            if (!exist)
            {
                foreach (var item in record)
                {
                    if (item.Value.username == UsernameEntry.Text)
                    {
                        doubled = true;
                    }
                }

                if (doubled)
                {
                    exist = true;
                    await App.Current.MainPage.DisplayAlert("Alert", "Username already exists", "Ok");
                }
            }
        }
        async void ChooseUsername(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(UsernameEntry.Text))
            {
                exist = true;
                await App.Current.MainPage.DisplayAlert("Alert", "Fill in Username", "Ok");
            }
            else if (UsernameEntry.Text.Length < 4)
            {
                exist = true;
                await App.Current.MainPage.DisplayAlert("Alert", "Username too short", "Ok");
            }
            else
            {
                exist = false;
                retrieveData();
            }
            if (!exist)
            {
                userSession();
            }
        }

    }
}
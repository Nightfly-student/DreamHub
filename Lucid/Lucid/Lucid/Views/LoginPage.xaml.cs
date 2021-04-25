using Firebase.Auth;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Lucid.Connection;
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
using Xamarin.Forms.Xaml;

namespace Lucid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public string WebApi = "AIzaSyD7-zCbfANaFV7p2YgxUGNNkW-3JElGGVI";

        private const string BasePath = "https://dreamhub-90140-default-rtdb.firebaseio.com/";
        private const string FirebaseSecret = "3w5A2pqhjMiEl05pow8Kw13e0ku6yTmY7sTxR3nw";
        private static FirebaseClient client;

        IFirebaseConfig ifc = new FireSharp.Config.FirebaseConfig()
        {
            AuthSecret = FirebaseSecret,
            BasePath = BasePath
        };

        public string username;
        Hash hc = new Hash();
        Connection.Connection con = new Connection.Connection();
        private SqlConnection dbConnection;
        public LoginPage()
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


            NavigationPage.SetHasNavigationBar(this, false);
        }
        [Obsolete]
        private void OnLabelClick(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SignUpPage());
        }

        [Obsolete]
        async void LoginLog(object sender, EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(WebApi));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(UsernameEntry.Text, PasswordEntry.Text);
                var content = await auth.GetFreshAuthAsync();
                var serializedcontent = JsonConvert.SerializeObject(content);
                Preferences.Set("userSession", content.User.Email);
                Preferences.Set("MyFirebaseRefreshToken", serializedcontent);

                if (string.IsNullOrEmpty(Preferences.Get("username", "")))
                {
                    retrieveData();
                    if (string.IsNullOrEmpty(Preferences.Get("username", "")))
                    {
                        await Navigation.PushModalAsync(new ChooseYourUsername());
                    } else
                    {
                        await Navigation.PushModalAsync(new Main());
                    }
                } else
                {
                    await Navigation.PushModalAsync(new Main());
                }
            }
            catch (FirebaseAuthException ex)
            {              
                await App.Current.MainPage.DisplayAlert("Alert", ex.Reason.ToString(), "Ok");
            }
        }
        private void retrieveData()
        {
            FirebaseResponse res = client.Get(@"Users");
            Dictionary<string, UserNameCreation> data = JsonConvert.DeserializeObject<Dictionary<string, UserNameCreation>>(res.Body.ToString());
            PopulateRTB(data);
        }
        private void PopulateRTB(Dictionary<string, UserNameCreation> record)
        {
            foreach(var item in record)
            {
                if (item.Value.userMail == Preferences.Get("userSession", "")) 
                {
                    Preferences.Set("username", item.Value.username);
                }
            }
        }
    }
}
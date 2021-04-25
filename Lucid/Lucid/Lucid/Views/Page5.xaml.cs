using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using Lucid.Model;
using Lucid.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lucid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page5 : ContentPage
    {
        private const string BasePath = "https://dreamhub-90140-default-rtdb.firebaseio.com/";
        private const string FirebaseSecret = "3w5A2pqhjMiEl05pow8Kw13e0ku6yTmY7sTxR3nw";
        private static FirebaseClient client;
        private int Tokens;

        IFirebaseConfig ifc = new FireSharp.Config.FirebaseConfig()
        {
            AuthSecret = FirebaseSecret,
            BasePath = BasePath
        };

        public Page5()
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
            retrieveTokens();
            ToolbarItems.Add(new ToolbarItem("settings", "settings.png", () =>
            {

            }));
        }
        private async void Logout(object sender, EventArgs args)
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            Preferences.Remove("username");
            Preferences.Remove("userSession");
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }
        private void retrieveTokens()
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
                    Tokens = item.Value.LucidTokens;
                }
            }
            TokenString.Text = "       LT: " + Tokens;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Lucid.Connection;
using System.Data.SqlClient;
using Firebase.Auth;

namespace Lucid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public string WebApi = "AIzaSyD7-zCbfANaFV7p2YgxUGNNkW-3JElGGVI";


        Hash hc = new Hash();
        Connection.Connection con = new Connection.Connection();
        private SqlConnection dbConnection;

        [Obsolete]
        public SignUpPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        [Obsolete]
        private void OnLabelClick(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new LoginPage());
        }
        async void RegisterUser(object sender, EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApi));
            try
            {
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(UsernameEntry.Text, PasswordEntry.Text);
                string gettoken = auth.FirebaseToken;
                // await App.Current.MainPage.DisplayAlert("Alert", gettoken, "Ok");
                await Navigation.PushModalAsync(new LoginPage());
            }
            catch (FirebaseAuthException ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Reason.ToString(), "Ok");
            }
        }
    }
}
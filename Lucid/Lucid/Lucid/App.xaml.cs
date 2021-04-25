using Lucid.Connection;
using Lucid.ViewModel;
using Lucid.Views;
using Plugin.Connectivity;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Lucid
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();

            if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")))
            {
                if (string.IsNullOrEmpty(Preferences.Get("username", "")))
                {
                    MainPage = new NavigationPage(new ChooseYourUsername());
                }
                else
                {
                    MainPage = new NavigationPage(new SplashPage());
                }
            } else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {

        }

        protected override async void OnResume()
        {
            if(!CrossConnectivity.Current.IsConnected)
            {
                do
                {
                    bool answer = await Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please connect to Internet", "Retry", "Cancel");

                    if (!answer)
                    {
                        CloseApplication c = new CloseApplication();
                        c.closeApplication();
                    }

                } while (!CrossConnectivity.Current.IsConnected);
            }
        }
    }
}

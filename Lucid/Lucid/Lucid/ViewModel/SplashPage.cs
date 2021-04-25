using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Lucid.ViewModel
{
    public class SplashPage : ContentPage
    {
        Image splashImage;
        Label splashText;
        
        public SplashPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var sub = new AbsoluteLayout();
            splashImage = new Image()
            {
                Source = "IconNated.png",
                WidthRequest = 200,
                HeightRequest = 200
            };
            splashText = new Label()
            {
                Text = "DreamHub",
                TextColor = Color.White,
                FontSize = 35
            };
            AbsoluteLayout.SetLayoutFlags(splashImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(splashImage, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(splashText, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(splashText, new Rectangle(0.5, 0.63, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            sub.Children.Add(splashImage);
            sub.Children.Add(splashText);

            this.BackgroundColor = Color.Black;
            this.Content = sub;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await splashImage.ScaleTo(1, 2000);
            await splashImage.ScaleTo(0.9, 1200);

            if (!CrossConnectivity.Current.IsConnected)
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

            Application.Current.MainPage = new Lucid.Main();
           await Navigation.PopAsync();

        }
    }
}

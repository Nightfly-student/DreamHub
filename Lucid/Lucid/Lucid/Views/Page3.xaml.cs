using CodeHollow.FeedReader;
using Lucid.Model;
using Lucid.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lucid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page3 : ContentPage
    {
        private string action { get; set; }
        public Page3()
        {
            InitializeComponent();
            BindingContext = new RssViewModel("Lucid Dreaming");
        }
        private async void onArticleTapped(object sender, ItemTappedEventArgs e)
        {
            var dateItem = e.Item as RssFeed;
            Uri openUri = new Uri(dateItem.link, UriKind.Absolute);
            await OpenBrowser(openUri);
            
        }
        private async Task OpenBrowser(Uri uri)
        {
            try
            {
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch(Exception ex)
            {
                await DisplayAlert("Link Does Not Exist", "Please try again later", "OK");
            }
        }
        private async void OnFilterClicked(object sender, EventArgs args)
        {
            action = await DisplayActionSheet("Filter to?", "Cancel", null,"Lucid Dreaming", "Dreaming", "Sleep", "Video's");
            BindingContext = new RssViewModel(action);

        }
    }
}
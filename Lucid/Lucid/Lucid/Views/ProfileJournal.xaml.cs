using Lucid.Model;
using Lucid.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lucid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileJournal : ContentPage
    {
        private string userSession { get; set; }
        public ProfileJournal(string userSession)
        {
            InitializeComponent();
            this.userSession = userSession;
            UserProfile.Text = "User: " + userSession;
            BindingContext = new JournalViewModel(userSession, 1);
        }
        private async void onTapped(object sender, ItemTappedEventArgs e)
        {
            var details = e.Item as journalTableModel;
            await Navigation.PushAsync(new publicJournal(details.journalTitle, details.journalDesc, details.journalDate, details.journalAwareness, details.journalVivid, details.journalCategory, details.journalLucid, details.tagList));
        }
    }
}
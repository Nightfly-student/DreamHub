using Lucid.Connection;
using Lucid.DAL;
using Lucid.Model;
using Lucid.ViewModel;
using Lucid.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lucid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        JournalPersonalModel j = new JournalPersonalModel();
        public Page2()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            personalFresh.Refreshing += PublicRefresh_Refreshing;
            BindingContext = new JournalPersonalModel();

        }
        private async void onTapped(object sender, ItemTappedEventArgs e)
        {
            var details = e.Item as journalTableModel;
            await Navigation.PushAsync(new PersonalJournal(details.journalTitle, details.journalDesc, details.journalDate, details.journalNotes, details.journalAwareness, details.journalVivid, details.journalCategory, details.journalLucid, details.tagList));
        }
        private async void navigationJournalEntry()
        {
            await Navigation.PushAsync(new JournalEntry());
        }
        private async void addDream(object sender, EventArgs args)
        {
            navigationJournalEntry();
        }
        public async void PublicRefresh_Refreshing(object sender, EventArgs args)
        {
            refreshEverywhere();
        }
        public async void refreshEverywhere()
        {
            personalFresh.IsRefreshing = true;
            await Task.Delay(2000);
            j.GetJournalInformation();
            BindingContext = new JournalPersonalModel();
            personalFresh.IsRefreshing = false;
        }
        private void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            BindingContext = new JournalPersonalModel(searchBar.Text);

            Console.WriteLine(sender.ToString());

            if (String.IsNullOrEmpty(sender.ToString()))
            {
                BindingContext = new JournalPersonalModel();
            }
        }
    }
}
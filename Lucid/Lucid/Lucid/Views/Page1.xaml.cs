using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Lucid.DAL;
using Lucid.Model;
using Lucid.ViewModel;
using Lucid.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lucid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        JournalViewModel j = new JournalViewModel();

        public Page1()
        {
            InitializeComponent();

            publicRefresh.Refreshing += PublicRefresh_Refreshing;

            BindingContext = new JournalViewModel();

            ToolbarItem item = new ToolbarItem
            {
                Text = "Filter",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0,            
            };
            this.ToolbarItems.Add(item);
            item.Clicked += OnItemClicked;
        }
        private async void onTapped(object sender, ItemTappedEventArgs e)
        {
            var details = e.Item as journalTableModel;
            await Navigation.PushAsync(new publicJournal(details.journalTitle, details.journalDesc, details.journalDate, details.journalAwareness, details.journalVivid, details.journalCategory, details.journalLucid, details.tagList));
        }
        private async void PublicRefresh_Refreshing(object sender, EventArgs args)
        {
            publicRefresh.IsRefreshing = true;
            await Task.Delay(2000);
            j.GetJournalInformation();
            BindingContext = new JournalViewModel();
            publicRefresh.IsRefreshing = false;
        }
        private void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            BindingContext = new JournalViewModel(searchBar.Text);

            if (String.IsNullOrEmpty(sender.ToString()))
            {
                BindingContext = new JournalViewModel();
            }
        }
        async void OnItemClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Filter On?", "Cancel", null, "Normal", "Nightmare", "False Awakening", "Recurring", "NSFW");
            BindingContext = new JournalViewModel(action, DateTime.Now);
        }
    }
}
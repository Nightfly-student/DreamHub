using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Lucid.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//ButtonCheck//

namespace Lucid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditJournal : ContentPage
    {
        private const string BasePath = "https://dreamhub-90140-default-rtdb.firebaseio.com/";
        private const string FirebaseSecret = "3w5A2pqhjMiEl05pow8Kw13e0ku6yTmY7sTxR3nw";
        private static FirebaseClient client;

        public string journalTitle = Preferences.Get("journalTitle", "");
        public string journalDesc = Preferences.Get("journalDesc", "");
        private int isLucid;
        private int isPublic;
        private string session;
        private string time;
        private string cateGory;
        private List<string> taglist;
        private int counter = 0;

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = FirebaseSecret,
            BasePath = BasePath
        };
        public EditJournal()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            try
            {
                client = new FirebaseClient(ifc);
            } catch(Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
            retrieveData();
            if (isPublic == 1)
            {
                DreamIsPublic.IsChecked = true;
            } 
            else if (isLucid == 1)
            {
                DreamIsLucid.IsChecked = true;
            }
        }
        private void retrieveData()
        {
            FirebaseResponse res = client.Get(@"journalTable");
            Dictionary<string, journalTable> data = JsonConvert.DeserializeObject<Dictionary<string, journalTable>>(res.Body.ToString());
            PopulateRTB(data);
        }
        void PopulateRTB(Dictionary<string, journalTable> record)
        {
            foreach (var item in record)
            {
                if(item.Value.dreamTitle == journalTitle && item.Value.dreamDesc == journalDesc)
                {
                    DreamTitle.Text = item.Value.dreamTitle;
                    DreamDesc.Text = item.Value.dreamDesc;
                    DreamNotes.Text = item.Value.dreamNotes;                   
                    session = item.Value.userSession;
                    time = item.Value.dreamDate;
                    isPublic = item.Value.dreamPublic;
                    isLucid = item.Value.dreamLucid;

                    switch(item.Value.dreamAwareness) 
                    {
                        case "None":
                            AwarenessSlider.Value = 0;
                            break;
                        case "Bad":
                            AwarenessSlider.Value = 1;
                            break;
                        case "Poor":
                            AwarenessSlider.Value = 2;
                            break;
                        case "Normal":
                            AwarenessSlider.Value = 3;
                            break;
                        case "Good":
                            AwarenessSlider.Value = 4;
                            break;
                        case "Perfect":
                            AwarenessSlider.Value = 5;
                            break;
                    }

                    switch (item.Value.dreamVivid)
                    {
                        case "None":
                            VividSlider.Value = 0;
                            break;
                        case "Bad":
                            VividSlider.Value = 1;
                            break;
                        case "Poor":
                            VividSlider.Value = 2;
                            break;
                        case "Normal":
                            VividSlider.Value = 3;
                            break;
                        case "Good":
                            VividSlider.Value = 4;
                            break;
                        case "Perfect":
                            VividSlider.Value = 5;
                            break;
                    }
                    cateGory = item.Value.dreamCategory;
                    taglist = item.Value.tagList;
                }

            }
            if (taglist != null)
            {
                foreach (var item in taglist)
                {
                    lblTag.Text += item + "   ";
                    counter++;
                }
            }
            DreamCategory.SelectedItem = cateGory;
        }
        private void setData()
        {
            var journalKey = Preferences.Get("journalKey", "");
            journalTable js = new journalTable()
            {
                dreamDesc = DreamDesc.Text,
                dreamTitle = DreamTitle.Text,
                dreamNotes = DreamNotes.Text,
                userSession = session,
                dreamDate = time,
                dreamLucid = isLucid,
                dreamPublic = isPublic,
                dreamAwareness = AwarenessLevelCaller.Text,
                dreamVivid = VividLevelCaller.Text,
                dreamCategory = cateGory,
                tagList = taglist

            };

            var res = client.Update(@"journalTable/" + journalKey,js );
        }
        private async void saveJournal(object sender, EventArgs e)
        {

            bool answer = await DisplayAlert("Update Dream Entry", "Are you sure?", "Yes", "No");

            if (answer)
            {
                PickedCategory();
                if (String.IsNullOrEmpty(DreamDesc.Text))
                {
                    await DisplayAlert("Alert", "Please fill in a description", "OK");
                }
                else if (String.IsNullOrEmpty(DreamTitle.Text))
                {
                    await DisplayAlert("Alert", "Please fill in a title", "OK");
                }
                else
                {
                    setData();
                    var vUpdatedPage = new Page2();
                    Navigation.InsertPageBefore(vUpdatedPage, this);
                    await Navigation.PopAsync();
                    navigation();
                }

            }
        }
        void OnCheckBoxLucid(object sender, CheckedChangedEventArgs e)
        {
            if (DreamIsLucid.IsChecked)
            {
                isLucid = 1;
            } else
            {
                isLucid = 0;
            }

        }
        void OnCheckBoxPublic(object sender, CheckedChangedEventArgs e)
        {
            if (DreamIsPublic.IsChecked)
            {
                isPublic = 1;
            } else
            {
                isPublic = 0;
            }

        }
        private async void navigation()
        {
            cateGory = null;
            await Navigation.PushAsync(new Page2());
        }
        private void VividSliderOnValueChanged(object sender, ValueChangedEventArgs e)
        {
            double StepValue = 1.0;

            var newStep = Math.Round(e.NewValue / StepValue);
            VividSlider.Value = newStep * StepValue;

            switch (VividSlider.Value)
            {
                case 0:
                    VividLevelCaller.Text = "None";
                    break;
                case 1:
                    VividLevelCaller.Text = "Bad";
                    break;
                case 2:
                    VividLevelCaller.Text = "Poor";
                    break;
                case 3:
                    VividLevelCaller.Text = "Normal";
                    break;
                case 4:
                    VividLevelCaller.Text = "Good";
                    break;
                case 5:
                    VividLevelCaller.Text = "Perfect";
                    break;
            }
        }
        private void AwarenessSliderOnValueChanged(object sender, ValueChangedEventArgs e)
        {
            double StepValue = 1.0;

            var newStep = Math.Round(e.NewValue / StepValue);
            AwarenessSlider.Value = newStep * StepValue;

            switch (AwarenessSlider.Value)
            {
                case 0:
                    AwarenessLevelCaller.Text = "None";
                    break;
                case 1:
                    AwarenessLevelCaller.Text = "Bad";
                    break;
                case 2:
                    AwarenessLevelCaller.Text = "Poor";
                    break;
                case 3:
                    AwarenessLevelCaller.Text = "Normal";
                    break;
                case 4:
                    AwarenessLevelCaller.Text = "Good";
                    break;
                case 5:
                    AwarenessLevelCaller.Text = "Perfect";
                    break;
            }
        }
        private void PickedCategory()
        {
            if (DreamCategory.SelectedIndex == -1)
            {
                cateGory = DreamCategory.Items[0];
            }
            else
            {
                cateGory = DreamCategory.Items[DreamCategory.SelectedIndex];
            }
        }
        private async void AddTag(object sender, EventArgs e)
        {
            string answer = await DisplayPromptAsync("Dream Tag", "Type your dream tag");

            if (answer != null)
            {
                taglist.Add(answer);
                lblTag.Text += taglist[counter] + ", ";
            }
            counter++;
        }
        private async void DeleteTag(object sender, EventArgs e)
        {
            if (counter == 0)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "No Tag Deleted", "Ok");
            }
            else
            {
                taglist.RemoveAt(counter - 1);
                lblTag.Text = "";
                foreach (var r in taglist)
                {
                    lblTag.Text += r + ", ";
                }
                counter--;
            }
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                if (result) await this.Navigation.PopAsync();
            });
            return true;
        }
    }
}
/*using Firebase.Database;
using Firebase.Database.Query;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Lucid.Model;
using Lucid.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Lucid.Model
{
    public class journalAddTag
    {
        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "3w5A2pqhjMiEl05pow8Kw13e0ku6yTmY7sTxR3nw",
            BasePath = "https://dreamhub-90140-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        JournalEntry journal = new JournalEntry();
        public journalAddTag(List<string> result)
        {
            try
            {
                client = new FireSharp.FirebaseClient(ifc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
            SetData(result);
        }
        private async void SetData(List<string> record)
        {
            string child = "journalTable/" + Preferences.Get("TagDream", "");
            retrieveData();
            Firebase.Database.FirebaseClient fc = new Firebase.Database.FirebaseClient("https://dreamhub-90140-default-rtdb.firebaseio.com/");
            var result = await fc
                .Child(child)
                .PostAsync(record);
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
                if(journal.desc == item.Value.dreamDesc && journal.title == item.Value.dreamTitle && journal.notes == item.Value.dreamDesc)
                {
                    Preferences.Get("TagDream", item.Key);
                }
            }
        }
    }
}
*/
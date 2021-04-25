using CodeHollow.FeedReader;
using Lucid.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Xamarin.Forms;

namespace Lucid.ViewModel
{
    class RssViewModel : BindableObject
    {
        ObservableCollection<RssFeed> Rss;
        public ObservableCollection<RssFeed> rss { get { return Rss; } }

        public string action { get; set; }

        public RssViewModel(string action)
        {
            Rss = new ObservableCollection<RssFeed>();
            this.action = action;
            feedTest();
        }
        private async void feedTest()
        {
            Rss.Clear();
            Rss = new ObservableCollection<RssFeed>();

            switch(action)
            {
                case "Lucid Dreaming":
                    var feed = await FeedReader.ReadAsync("https://luciddreaming.blog/category/lucid-dreaming/feed");
                    feed.Items.ToList().ForEach(i => Rss.Add(new RssFeed { title = i.Title, link = i.Link }));
                    break;
                case "Dreaming":
                    var feed1 = await FeedReader.ReadAsync("https://luciddreaming.blog/category/dreaming/feed");
                    feed1.Items.ToList().ForEach(i => Rss.Add(new RssFeed { title = i.Title, link = i.Link }));
                    break;
                case "Sleep":
                    var feed2 = await FeedReader.ReadAsync("https://luciddreaming.blog/category/sleep/feed");
                    feed2.Items.ToList().ForEach(i => Rss.Add(new RssFeed { title = i.Title, link = i.Link }));
                    break;
                case "Video's":
                    var feed3 = await FeedReader.ReadAsync("https://luciddreaming.blog/category/Video/feed");
                    feed3.Items.ToList().ForEach(i => Rss.Add(new RssFeed { title = i.Title, link = i.Link }));
                    break;
                default:
                    var feed4 = await FeedReader.ReadAsync("https://luciddreaming.blog/category/lucid-dreaming/feed");
                    feed4.Items.ToList().ForEach(i => Rss.Add(new RssFeed { title = i.Title, link = i.Link }));
                    break;
            }

        }
    }
}

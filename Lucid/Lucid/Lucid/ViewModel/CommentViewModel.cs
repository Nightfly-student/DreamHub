using Firebase.Database;
using Lucid.Model;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Lucid.ViewModel
{
    class CommentViewModel : BindableObject
    {
        private ObservableCollection<GroupedClientCommentModel> commentItem;
        public string tag;
        public bool isEmpty = false;
        public ObservableCollection<GroupedClientCommentModel> CommentItem
        {
            get { return commentItem; }
            set
            {
                commentItem = value;
                OnPropertyChanged();
            }
        }
        public ICommand RefreshList { get; set; }

        public CommentViewModel()
        {
            commentItem = new ObservableCollection<GroupedClientCommentModel>();
            RefreshList = new Command(async () => await PerformRefresh());
            GetCommentInformation();
        }
        private async Task PerformRefresh()
        {
            GetCommentInformation();
        }
        public async void GetCommentInformation()
        {
            CommentItem = new ObservableCollection<GroupedClientCommentModel>();
            FirebaseClient fc = new FirebaseClient("https://dreamhub-90140-default-rtdb.firebaseio.com/");
            var GetComments = (await fc
                .Child("Comment")
                .OnceAsync<Comment>()).Select(item => new Comment
                {
                    CommentUser = item.Object.CommentUser,
                    CommentText = item.Object.CommentText,
                    CommentDate = item.Object.CommentDate,
                    dreamId = item.Object.dreamId

                }).ToList().OrderByDescending(x => DateTime.ParseExact(x.CommentDate, "dd/MM/yyyy", null));

            var headergroup = GetComments.Where(i => i.dreamId == Preferences.Get("dreamId", "")).Select(x => x.CommentDate).Distinct().ToList();


            foreach (var item in headergroup)
            {
                if (headergroup.Count != 0)
                {
                    var CommentGroup = new GroupedClientCommentModel() { CommentDate = item };
                    var contents = GetComments.Where(i => i.CommentDate == item).ToList();
                    if (contents.Count != 0)
                    {
                        foreach (var groupitems in contents)
                        {
                            if (groupitems.dreamId == Preferences.Get("dreamId", ""))
                            {
                                CommentGroup.Add(new Comment()
                                {
                                    CommentDate = groupitems.CommentDate,
                                    CommentText = groupitems.CommentText,
                                    CommentUser = groupitems.CommentUser,
                                    dreamId = groupitems.dreamId
                                });
                            }
                        }
                        CommentItem.Add(CommentGroup);
                    }
                } else
                {
                    isEmpty = true;
                }
            }
        }
    }
}

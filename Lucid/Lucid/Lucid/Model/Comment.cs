using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Lucid.Model
{
    public class Comment
    {
        public string CommentText { get; set; }
        public string CommentUser { get; set; }
        public string CommentDate { get; set; }
        public string dreamId { get; set; }

    }
    public class GroupedClientCommentModel : ObservableCollection<Comment>
    {
        public string CommentDate { get; set; }
    }
}

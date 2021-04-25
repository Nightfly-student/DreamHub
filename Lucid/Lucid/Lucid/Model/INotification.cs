using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Model
{
    public interface INotification
    {
        void createNotification(string title, string message);
    }
}

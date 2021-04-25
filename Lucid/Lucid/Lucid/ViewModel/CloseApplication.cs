using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Lucid.ViewModel
{
    public class CloseApplication
    {
        public void closeApplication()
        {
            Application.Current.Quit();
        }
    }
}

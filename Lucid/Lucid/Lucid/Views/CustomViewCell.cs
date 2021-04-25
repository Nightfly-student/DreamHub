using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Lucid.Views
{
    public class CustomViewCell : ViewCell
    {
        public static readonly BindableProperty SelectItemBackgroundColorProperty = BindableProperty.Create("SelectedItemBackgroundColor", typeof(Color), typeof(CustomViewCell), Color.Default);

        public Color SelectedItemBackgroundColor
        {
            get { return (Color)GetValue(SelectItemBackgroundColorProperty); }
            set { SetValue(SelectItemBackgroundColorProperty, value); }
        }
    }
}

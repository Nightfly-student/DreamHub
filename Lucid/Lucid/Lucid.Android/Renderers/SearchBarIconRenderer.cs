using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.SearchBar), typeof(Lucid.Droid.Renderers.SearchBarIconRenderer))]

namespace Lucid.Droid.Renderers
{
    class SearchBarIconRenderer : SearchBarRenderer
    {
        public SearchBarIconRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            var icon = Control?.FindViewById(Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null));
            (icon as ImageView)?.SetColorFilter(Android.Graphics.Color.White, PorterDuff.Mode.Multiply);
        }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("Lucid.Views.SimpleLoginPage.xaml", "Views/SimpleLoginPage.xaml", typeof(global::Lucid.Views.Forms.SimpleLoginPage))]

namespace Lucid.Views.Forms {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\SimpleLoginPage.xaml")]
    public partial class SimpleLoginPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Lucid.Controls.BorderlessEntry PasswordEntry;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label ForgotPasswordLabel;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(SimpleLoginPage));
            PasswordEntry = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Lucid.Controls.BorderlessEntry>(this, "PasswordEntry");
            ForgotPasswordLabel = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "ForgotPasswordLabel");
        }
    }
}

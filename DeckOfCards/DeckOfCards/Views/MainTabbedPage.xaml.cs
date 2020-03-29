using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeckOfCards.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DeckOfCards.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DependencyService.Get<IViewLifecycleService>()?.ViewLoaded();
        }
    }
}

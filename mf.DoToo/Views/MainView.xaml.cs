using mf.DoToo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mf.DoToo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            viewModel.Navigation = Navigation;
            this.BindingContext = viewModel;
            
        }

        private void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ItemsListView.SelectedItem = null;
        }
    }
}
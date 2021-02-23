using System.ComponentModel;
using Xamarin.Forms;
using AnyTask.ViewModels;

namespace AnyTask.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
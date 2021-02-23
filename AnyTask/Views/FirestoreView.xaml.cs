using AnyTask.ViewModels;
using Xamarin.Forms;

namespace AnyTask.Views
{
    public partial class FirestoreView : ContentPage
    {
        public FirestoreView()
        {
            InitializeComponent();

            BindingContext = new FirestoreViewModel();
        }
    }
}

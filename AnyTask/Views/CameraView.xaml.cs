using System;
using System.Collections.Generic;
using AnyTask.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace AnyTask.Views
{
    public partial class CameraView : ContentPage
    {
        public CameraView()
        {
            InitializeComponent();

            BindingContext = new CameraViewModel();
            //zoomLabel.Text = string.Format("Zoom: {0}", zoomSlider.Value);
        }

        void CameraView_OnAvailable(object sender, bool e)
        {
            (BindingContext as CameraViewModel).CanTakePhoto = e;
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        async void SaveToGallery_Clicked(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("Fire ! ", "Save To Gallery", "ok");
        }

        async void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            await (BindingContext as CameraViewModel).PhotoCaptured(e);
        }
    }
}

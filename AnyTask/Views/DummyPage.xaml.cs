using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AnyTask.Views
{
    public partial class DummyPage : ContentPage
    {
        public DummyPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.Navigation.PushModalAsync(new CameraView());
        }

        async void Button_ClickedXCT(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new CameraXCTView());
        }
    }
}

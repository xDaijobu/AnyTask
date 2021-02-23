using System;
using System.Collections.Generic;
using AnyTask.ViewModels;
using AnyTask.Views;
using Xamarin.Forms;

namespace AnyTask
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}

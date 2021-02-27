using System;
using System.Threading.Tasks;
using AnyTask.Models;
using AnyTask.Services;
using Plugin.CloudFirestore;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AnyTask.ViewModels
{
    public class FirestoreViewModel : BaseViewModel
    {
        readonly string collectionPath = "yourCollection";
        ObservableRangeCollection<Item> items;
        public ObservableRangeCollection<Item> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public AsyncCommand AddItemCommand => new AsyncCommand(AddItemAsync, allowsMultipleExecutions: false);
        public AsyncCommand SignInCommand => new AsyncCommand(SignInAnonymously);
        public AsyncCommand SignInWithEmailAndPasswordCommand => new AsyncCommand(SignInWithEmailAndPassword);
        public AsyncCommand SignUpCommand => new AsyncCommand(SignUp);

        public Command DeleteItemsCommand => new Command(() => Items?.Clear()
);
        public Command DeleteItemCommand => new Command<string>(async (id) =>
        {
            await DeleteItemAsync(id);
        });

        //public AsyncCommand GetItemsCommand => new AsyncCommand(GetItemsAsync);

        public FirestoreViewModel()
        {
            Items = new ObservableRangeCollection<Item>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetItemsAsync();
            });
        }

        //string? cris;
        private async Task AddItemAsync()
        {
            //string x = cris!;
            //System.Diagnostics.Debug.WriteLine(x);
            System.Diagnostics.Debug.WriteLine("Fire !");

            string base64Guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            var item = new Item()
            {
                Id = base64Guid,
                Description = DateTime.Now.ToLongDateString(),
                Text = "hahaha"
            };

            var result  = await CrossCloudFirestore.Current
                         .Instance
                         .Collection(collectionPath)
                         .AddAsync(item);
            //.AddAsync(new YourModel());

            item.CollectionPath = result.Id;
            Items.Add(item);
            Console.WriteLine(result);
        }

        private async Task DeleteItemAsync(string id)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Document($"{collectionPath}/{id}")
                         .DeleteAsync();
        }

        private async Task GetItemsAsync()
        {
            var document = await CrossCloudFirestore.Current
                .Instance
                .Collection(collectionPath)
                //.Document
                .GetAsync();

            var items = document.ToObjects<Item>();

            Items.AddRange(items);
        }

        private AuthService authService = new AuthService();

        private async Task SignInAnonymously()
        {
            var result = await authService.SignInAnonymously();
            System.Diagnostics.Debug.WriteLine("Result : " + result);
        }

        private async Task SignUp()
        {
            var result = await authService.SignUp("christwurangian@gmail.com", "123456");

            authService.CurrentUser();
            System.Diagnostics.Debug.WriteLine("SignUp : " + result);
        }

        private async Task SignInWithEmailAndPassword()
        {
            var result = await authService.SignInWithEmailPassword("christwurangian@gmail.com", "123456");
            await DisplayToastAsync("Uid : " + result.User.Uid);
        }
    }
}

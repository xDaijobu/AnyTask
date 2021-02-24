using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using AnyTask.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;
using Image = AnyTask.Models.Image;

namespace AnyTask.ViewModels
{
    /*
     * Fix camera media rotation android issue
     * https://github.com/xamarin/XamarinCommunityToolkit/pull/886
     * 
     */
    public class CameraViewModel : BaseViewModel
    {
        bool canTakePhoto;
        public bool CanTakePhoto
        {
            get => canTakePhoto;
            set => SetProperty(ref canTakePhoto, value);
        }

        string lastSavedPhotoPath;
        public string LastSavedPhotoPath
        {
            get => lastSavedPhotoPath;
            set => SetProperty(ref lastSavedPhotoPath, value);
        }

        ObservableCollection<Image> images;
        public ObservableCollection<Image> Images
        {
            get => images;
            set => SetProperty(ref images, value);
        }

        Image selectedImage;
        public Image SelectedImage
        {
            get => selectedImage;
            set => SetProperty(ref selectedImage, value);
        }

        List<string> Paths = new List<string>();

        public ICommand PhotoCapturedCommand => new AsyncCommand<MediaCapturedEventArgs>(PhotoCaptured);
        public CameraViewModel()
        {
            Images = new ObservableCollection<Image>();
        }

        private async Task PhotoCaptured(MediaCapturedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine(args);
            System.Diagnostics.Debug.WriteLine(args.ImageData.ToString());

            await SavePhotoAsync(args.ImageData);

            GetImages();
        }

        private async Task SavePhotoAsync(byte[] photoData)
        {
            var readStatus = await CheckAndRequestPermissionAsync(new StorageRead());
            if (readStatus != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return;
            }

            var writeStatus = await CheckAndRequestPermissionAsync(new StorageWrite());
            if (writeStatus != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return;
            }

            var savedPhotoPath = await SavePhotoFileAsync(photoData, Guid.NewGuid().ToString() + ".jpg");

            LastSavedPhotoPath = savedPhotoPath;
        }

        private async Task<string> SavePhotoFileAsync(byte[] photoData, string fileName)
        {
            var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Directory.CreateDirectory(dirPath);

            var savedPhotoPath = Path.Combine(dirPath, fileName);

            using (var fs = new FileStream(savedPhotoPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                await fs.WriteAsync(photoData, 0, photoData.Length);
                Paths.Add(savedPhotoPath);
                return savedPhotoPath;
            }
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        private void GetImages()
        {
            Images.Clear();

            foreach(var path in Paths)
            {
                Images.Add(new Image()
                {
                    Name = "hehe",
                    Path = path,
                    Source = ImageSource.FromFile(path)
                });
            }
        }
    }
}

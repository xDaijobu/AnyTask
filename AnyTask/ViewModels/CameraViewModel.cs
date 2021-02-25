using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;
using Image = AnyTask.Models.Image;

namespace AnyTask.ViewModels
{
    /*
     * Camera View ( https://github.com/xamarin/XamarinCommunityToolkit/tree/main/src/CommunityToolkit/Xamarin.CommunityToolkit/Views/CameraView )
     * Issues :
     * 1. CapturePhoto yg ada di iOS belum jalan 
     * 2. Performa nya lumayan berat ( saat pertama kali CameraView nya dibuka , harus menunggu bbrp detik utk bisa mengambil Foto )
     * 
     * 
     * Fix camera media rotation android issue ( ref : https://github.com/xamarin/XamarinCommunityToolkit/pull/886 )
     * TODO : 
     * 1. Rotation (90deg)
     * 2. ReSize 
     * 
     * Solusi untuk Rotation & ReSize :
     * - Bitmap adanya di SkiaSharp & Native masing2 platform
     *      #Bitmap ( SkiaSharp ) | paling cocok diimplementasikan ke GeneralModul ( karena Camera adanya di GeneralModul jg )
     *      Pros
     *      - SkiaSharp bisa langsung dr Xamarin.Forms ( tnpa harus implement ke native ny lg )
     *      - dengan 1 codingan / 1 source code / 1 file bisa diterapkan langsung ke semua project 
     *      Cons
     *      - kalau pakai SkiaSharp, maka harus add packageny dia yg lmyan gede yg otomatis hasil apk ny jg nambah
     *      
     *      #Bitmap ( Native )
     *      Pros
     *      - tdk perlu add package sprti SkiaSharp & hasil apk akan tetap sama 
     *      - hasil yg diberikan mungkin lebih bagus ( dikarenakan pakai Engine nya langsung dari Native )
     *      Cons
     *      - harus diimplementasikan ke masing2 project Android nya ( DependencyInjection ) | Sa.To, HR.To, & app lain nya harus melakukan hal ini
     *      - 1 codingan ( file nya jadi banyak ) tapi harus diimplementasikan ke banyak project
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

        public async Task PhotoCaptured(MediaCapturedEventArgs args)
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
                status = await permission.RequestAsync();

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
                    Source = ImageSource.FromFile(path),
                    Rotation = 90,
                });
            }
        }
    }
}

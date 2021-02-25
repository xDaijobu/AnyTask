using System;
using Xamarin.Forms;

namespace AnyTask.Models
{
    public class Image
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ImageSource Source { get; set; }
        public double Rotation { get; set; }
    }
}

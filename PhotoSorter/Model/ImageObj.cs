using System;
using System.Security.Policy;
using System.Windows.Media.Imaging;

namespace PhotoSorter.Model
{
    public class ImageObj
    {
        public BitmapImage Image { get; private set; }
        public int Index { get; set; }
        public string UriSource { get; private set; } // Store the file path


        public ImageObj(string path)
        {
            UriSource = path;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(UriSource);
            image.DecodePixelWidth = 1920; // Maybe one size for image and different size for thumbnail preview?
            image.EndInit();
            Image = image;
        }

    }
}

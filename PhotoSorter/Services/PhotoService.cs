using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoSorter.Model;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PhotoSorter.Services
{
    class PhotoService
    {
        private int bufferSize = 20; // Total number of images to keep in memory
        private int batchSize { get; set; } = 10;

        public List<string> photoPaths = new List<string>();

        public Dictionary<int, ImageObj> photos = new Dictionary<int, ImageObj>(); // Int is the index of the photo, Photo is the photo object




        public async Task InitializeFolderPair(FolderPair folderPair)
        {
            // Load photo paths asynchronously
            var jpegFiles = await Task.Run(() => Directory.GetFiles(folderPair.SourceFolderPath, "*.jpg"));
            var pngFiles = await Task.Run(() => Directory.GetFiles(folderPair.SourceFolderPath, "*.png"));
            var jpegFiles2 = await Task.Run(() => Directory.GetFiles(folderPair.SourceFolderPath, "*.jpeg"));

            // Combine all file paths
            photoPaths = jpegFiles
                .Concat(jpegFiles2)
                .Concat(pngFiles)
                .ToList();

            // Load the initial batch of images into the buffer
            LoadBuffer(folderPair.CurrentIndex);
        }

        public void LoadBuffer(int currentIndex)
        {
            // Calculate the center of the buffer
            int startIndex = Math.Max(0, currentIndex - bufferSize / 2);
            int endIndex = Math.Min(photoPaths.Count - 1, startIndex + bufferSize - 1);

            for (int i = startIndex; i <= endIndex; i++)
            {;
                foreach (var photo in photos)
                {
                }
                if (!photos.ContainsKey(i)) // Only add if it doesn't already exist
                {
                    photos[i] = new ImageObj(photoPaths[i]);
                }
                else
                {
                    // Optionally update the existing photo if needed
                    photos[i] = photos[i]; // This line could be simplified
                }
            }

            foreach(var photo in photos)
            {
                Debug.WriteLine("Photo: " + photo.Key + " is copied: " + photo.Value.IsCopied);
            }   
        }

    }
}

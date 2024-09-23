﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoSorter.Commands;
using System.Windows.Input;
using PhotoSorter.Model;
using System.Windows;
using System.Windows.Controls;
using PhotoSorter.Services;
using System.Windows.Media.Imaging;

namespace PhotoSorter.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public FolderPair FolderPair { get; }

        private PhotoService photoService = new PhotoService();
        // Commands for navigating through the files
        public ICommand Prev5Command { get; }
        public ICommand PrevCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand Next5Command { get; }


        // Copies the file from source to destination
        public ICommand CopyCommand { get; }


        private ImageObj _currentPhoto;


        public BitmapImage CurrentPhoto
        {
            get => _currentPhoto?.Image;
            set
            {
                if (value != null)
                {
                    _currentPhoto = new ImageObj(value.UriSource.ToString());
                }
                else
                {
                    _currentPhoto = null;
                }
                OnPropertyChanged(nameof(CurrentPhoto)); // Notify view of property change
            }
        }

        private int _currentIndex;

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                OnPropertyChanged(nameof(CurrentIndex));
                photoService.LoadBuffer(CurrentIndex); // Load the buffer whenever index changes
                UpdateCurrentPhoto();
            }
        }

        // Binds file paths to the view
        private string _sourceFolder;
        public string SourceFolder
        {
            get => _sourceFolder;
            set
            {
                _sourceFolder = value;
                OnPropertyChanged(nameof(SourceFolder)); // Notify view of property change
            }
        }

        private string _destinationFolder;
        public string DestinationFolder
        {
            get => _destinationFolder;
            set
            {
                _destinationFolder = value;
                OnPropertyChanged(nameof(DestinationFolder)); // Notify view of property change
            }
        }

        public MainWindowViewModel(FolderPair folderPair)
        {
            FolderPair = folderPair;

            // Assign the values from FolderPair to the properties to bind to the view
            SourceFolder = folderPair.SourceFolderPath;
            DestinationFolder = folderPair.DestinationFolderPath;


            // Initialize the commands with appropriate action methods
            Prev5Command = new RelayCommand(ExecutePrev5);
            PrevCommand = new RelayCommand(ExecutePrev);
            NextCommand = new RelayCommand(ExecuteNext);
            Next5Command = new RelayCommand(ExecuteNext5);
            CopyCommand = new RelayCommand(ExecuteCopy);

            InitializeAsync(folderPair);
        }

        private async void InitializeAsync(FolderPair folderPair)
        {
            await photoService.InitializeFolderPair(folderPair);

            // Load the first photo
            if (photoService.photos.Count > 0)
            {
                CurrentPhoto = photoService.photos[0].Image; // Set the path to the first photo
                CurrentIndex = folderPair.CurrentIndex; // 0 if no index is set
            }
        }

        // Action methods for each command
        private void ExecutePrev5(object parameter)
        {
            // Logic for skipping back 5 items
        }

        private void ExecutePrev(object parameter)
        {
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
            }
        }

        private void ExecuteCopy(object parameter)
        {
            // Logic for copying something
            string fileToCopy = photoService.photos[CurrentIndex].UriSource;
            try
            {
                System.IO.File.Copy(fileToCopy, DestinationFolder + "\\" + System.IO.Path.GetFileName(fileToCopy));
            }
            catch (Exception e)
            {
                MessageBox.Show("Error copying file: " + e.Message);
            }
        }

        // C:\temp\testimages\SAMPLING\16BIT\RGB\2400x2400\C00C00

        private void ExecuteNext(object parameter)
        {
            if (CurrentIndex < photoService.photoPaths.Count - 1)
            {
                CurrentIndex++;
            }
        }

        private void ExecuteNext5(object parameter)
        {
            // Logic for skipping forward 5 items
        }

        private void UpdateCurrentPhoto()
        {
            if (photoService.photos.ContainsKey(CurrentIndex))
            {
                CurrentPhoto = photoService.photos[CurrentIndex].Image;
            }
            else
            {
                CurrentPhoto = null; // Handle case where photo isn't available
            }
        }
    }
}
using System;
using PhotoSorter.Commands;
using System.Windows.Input;
using PhotoSorter.Model;
using System.Windows;
using PhotoSorter.Services;
using System.Windows.Media.Imaging;


namespace PhotoSorter.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        // FolderPair object to hold the source, destination folders and progress index
        public FolderPair FolderPair { get; }

        private PhotoService photoService = new PhotoService();

        // Object to hold the current photo, index, and other properties
        private ImageObj _currentPhoto;

        // Index of the current photo, used to navigate through the photos
        private int _currentIndex;

        // Properties
        public BitmapImage CurrentPhoto
        {
            get => _currentPhoto?.Image;
            set
            {
                if (value != null)
                {
                    // Find the index of the photo in the photoPaths list
                    int index = photoService.photoPaths.IndexOf(value.UriSource.LocalPath);
                    if (index >= 0 && photoService.photos.TryGetValue(index, out var existingPhoto))
                    {
                        _currentPhoto = existingPhoto; // Use the existing ImageObj
                        FileName = existingPhoto.UriSource; // Set the file name
                        IsPhotoCopied = existingPhoto.IsCopied; // Preserve the IsCopied state
                    }
                }
                else
                {
                    _currentPhoto = null;
                }
                OnPropertyChanged(nameof(CurrentPhoto)); // Notify view of property change
            }
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                OnPropertyChanged(nameof(CurrentIndex));
                OnPropertyChanged(nameof(DisplayIndex));
                photoService.LoadBuffer(CurrentIndex); // Load the buffer whenever index changes
                UpdateCurrentPhoto();
            }
        }

        public string DisplayIndex => $"{CurrentIndex + 1} / {photoService.photoPaths.Count}"; // Display the current index in the UI
        public string SourceAndDestination => $"Source: {SourceFolder} \nDestination: {DestinationFolder}"; // Display the source and destination folders in the UI

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

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                string filePath = new Uri(value).LocalPath;
                _fileName = System.IO.Path.GetFileName(filePath);
                OnPropertyChanged(nameof(FileName)); // Notify view of property change
            }
        }

        private bool _isPhotoCopied;
        public bool IsPhotoCopied
        {
            get => _isPhotoCopied;
            set
            {
                if (_isPhotoCopied != value) // Only raise change if the value is different
                {
                    _isPhotoCopied = value;
                    OnPropertyChanged(nameof(IsPhotoCopied)); // Notify the UI of the change
                }
            }
        }

        private double _rotationAngle;
        public double RotationAngle
        {
            get => _rotationAngle;
            set
            {
                _rotationAngle = value;
                OnPropertyChanged(nameof(RotationAngle));
            }
        }

        // Commands for navigating through the files
        public ICommand Prev5Command { get; }
        public ICommand PrevCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand Next5Command { get; }
        public ICommand RotateLeftCommand { get; }
        public ICommand RotateRightCommand { get; }
        public ICommand CopyCommand { get; }

        // Constructor
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
            RotateLeftCommand = new RelayCommand(param => RotateImage(-90));
            RotateRightCommand = new RelayCommand(param => RotateImage(90));

            InitializeAsync(folderPair);
        }

        // Methods
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

        public void RotateImage(double angle)
        {
            RotationAngle += angle;
        }

        private void ExecutePrev5(object parameter)
        {
            if (CurrentIndex >= 5)
            {
                CurrentIndex -= 5;
            }
            else if (CurrentIndex > 0)
            {
                CurrentIndex = 0;
            }
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
            string fileToCopy = photoService.photos[CurrentIndex].UriSource;
            try
            {
                // First parameter is the source file, second is the destination file.
                System.IO.File.Copy(fileToCopy, DestinationFolder + "\\" + System.IO.Path.GetFileName(fileToCopy));

                photoService.photos[CurrentIndex].IsCopied = true; // Mark the photo as copied

                CurrentIndex++; // Move to the next photo after copying              

                // Update the current index in the FolderPair
                if (CurrentIndex > FolderPair.CurrentIndex)
                {
                    FolderPair.CurrentIndex = CurrentIndex;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error copying file: " + e.Message);
            }
        }

        private void ExecuteNext(object parameter)
        {
            if (CurrentIndex < photoService.photoPaths.Count - 1)
            {
                CurrentIndex++;
            }
        }

        private void ExecuteNext5(object parameter)
        {
            if (CurrentIndex < photoService.photoPaths.Count - 5)
            {
                CurrentIndex += 5;
            }
            else if (CurrentIndex < photoService.photoPaths.Count - 1)
            {
                CurrentIndex = photoService.photoPaths.Count - 1;
            }
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

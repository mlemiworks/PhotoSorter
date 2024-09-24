using PhotoSorter.Commands;
using PhotoSorter.Model;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System;

namespace PhotoSorter.ViewModel
{
    public class StartWindowViewModel : ViewModelBase
    {
        public ICommand OpenMainWindowCommand { get; }
        public ICommand ToggleVisibilityCommand { get; }

        public ICommand SetSourceFolderCommand { get; }

        public ICommand SetDestinationFolderCommand { get; }

        private string _sourcePath;
        public string SourcePath
        {
            get => _sourcePath;
            set
            {
                _sourcePath = value;
                OnPropertyChanged(nameof(SourcePath));
            }
        }

        private string _destinationPath;
        public string DestinationPath
        {
            get => _destinationPath;
            set
            {
                _destinationPath = value;
                OnPropertyChanged(nameof(DestinationPath));
            }
        }




        // Properties for the file name input, to toggle visibility
        private bool _isFileNameInputVisible;
        public bool IsFileNameInputVisible
        {
            get => _isFileNameInputVisible;
            set
            {
                _isFileNameInputVisible = value;
                OnPropertyChanged(nameof(IsFileNameInputVisible));
            }
        }


        // Properties for the start menu buttons, to toggle visibility
        private bool _isStartMenuButtonsVisible;
        public bool IsStartMenuButtonsVisible
        {
            get => _isStartMenuButtonsVisible;
            set
            {
                _isStartMenuButtonsVisible = value;
                OnPropertyChanged(nameof(IsStartMenuButtonsVisible));
            }
        }


        public StartWindowViewModel()
        {
            IsStartMenuButtonsVisible = true; // Start with the buttons visible
            // Initialize commands
            ToggleVisibilityCommand = new RelayCommand(ToggleVisibility);
            OpenMainWindowCommand = new RelayCommand(OpenMainWindow);
            SetSourceFolderCommand = new RelayCommand(SetSourceFolder);
            SetDestinationFolderCommand = new RelayCommand(SetDestinationFolder);


        }

        private void ToggleVisibility(object parameter)
        {
            IsFileNameInputVisible = !IsFileNameInputVisible; // Toggle visibility
            IsStartMenuButtonsVisible = !IsStartMenuButtonsVisible; // Toggle visibility
        }

        private void SetSourceFolder(object parameter)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select a Source folder";
                folderBrowserDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                // Show the dialog and check if the user clicked "OK"
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    SourcePath = selectedPath;
                }
            }
        }

        private void SetDestinationFolder(object parameter)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select a destination folder";
                folderBrowserDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                // Show the dialog and check if the user clicked "OK"
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    DestinationPath = selectedPath;
                }
            }
        }




        // Accept the tuple containing source and destination paths
        private void OpenMainWindow(object parameter)
        {
            // The parameter is expected to be a tuple of two strings
            if (parameter is (string source, string destination))
            {
                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination))
                {
                    System.Windows.MessageBox.Show("Please select both a source and destination folder.");
                    return;
                }
                // Initialize the object (model) with the source and destination paths
                FolderPair folderPair = new FolderPair(source, destination);

                // Open the MainWindow with the initialized object
                MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(folderPair);
                MainWindow mainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel
                };
                mainWindow.Show();

                // Close the StartWindow

                System.Windows.Application.Current.Windows[0].Close();

            }
        }
    }
}

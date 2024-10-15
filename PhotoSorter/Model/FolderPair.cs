using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSorter.Model
{
    public class FolderPair
    {
        public int? Id { get; set; }
        public string SourceFolderPath { get; set; }
        public string DestinationFolderPath { get; set; }
        public int CurrentIndex { get; set; } = 0;

        public FolderPair(int? id, string sourceFolderPath, string destinationFolderPath, int currentIndex = 0)
        {
            Id = id;
            SourceFolderPath = sourceFolderPath;
            DestinationFolderPath = destinationFolderPath;
            CurrentIndex = currentIndex;
        }
    }

    


}
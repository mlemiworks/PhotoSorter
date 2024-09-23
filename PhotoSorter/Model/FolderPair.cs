using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSorter.Model
{
    public class FolderPair
    {
        public int Id { get; set; }
        public string SourceFolderPath { get; set; }
        public string DestinationFolderPath { get; set; }
        public int CurrentIndex { get; set; } = 0;

        public FolderPair(string sourceFolderPath, string destinationFolderPath)
        {
            SourceFolderPath = sourceFolderPath;
            DestinationFolderPath = destinationFolderPath;
        }
    }

    


}
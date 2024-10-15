using PhotoSorter.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows.Documents;

namespace PhotoSorter.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            string dbFilePath = Environment.CurrentDirectory + "\\DB\\PhotoSorter.db";

            _connectionString = $"Data Source={dbFilePath};Version=3;";


            InitializeDatabase();
        }

        private void InitializeDatabase()
        {

            if (!Directory.Exists(Environment.CurrentDirectory + "\\DB"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\DB");
            }
            if (!File.Exists(Environment.CurrentDirectory + "\\DB\\PhotoSorter.db"))
            {
                SQLiteConnection.CreateFile(Directory.GetCurrentDirectory() + "\\DB\\PhotoSorter.db");
            }

            

            Debug.WriteLine(_connectionString);
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string createTable = @"CREATE TABLE IF NOT EXISTS folders (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        SourcePath TEXT NOT NULL,
                                        DestinationPath TEXT NOT NULL,
                                        ProgressIndex INTEGER DEFAULT 0
                                        );";

                using (var command = new SQLiteCommand(createTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public ObservableCollection<FolderPair> getAllFolderPairs()
        {
            ObservableCollection<FolderPair> folderPairs = new ObservableCollection<FolderPair>();

            // get the data in to an array variable
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM folders";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FolderPair folderPair = new FolderPair(
                                reader.IsDBNull(reader.GetOrdinal("Id")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Id")), // Handle nullable Id
                                reader["SourcePath"].ToString(), 
                                reader["DestinationPath"].ToString(),
                                reader.GetInt32(reader.GetOrdinal("ProgressIndex"))
                                );
                          
                            folderPairs.Add(folderPair);


                            Debug.WriteLine(reader["Id"]);
                            Debug.WriteLine(reader["SourcePath"]);
                            Debug.WriteLine(reader["DestinationPath"]);
                        }
                    }
                }
            }

            return folderPairs;
        }
    }
}

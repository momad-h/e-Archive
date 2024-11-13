using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive
{
    public class FileBrowser
    {
        public int Counter = 0;
        public int Counter1 = 0;
        private string _path;
        IPublicServices _publicServices;

        public FileBrowser()
        {
            _publicServices = new PublicServices();    
        }
        public void ProcessMainFoldersParallel(string rootPath, int maxDegreeOfParallelism)
        {
            var mainFolders = Directory.GetDirectories(rootPath, "p??????");

            Parallel.ForEach(mainFolders.OrderBy(f => f),
                new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism },
                mainFolder =>
                {
                    string personnelCode = Path.GetFileName(mainFolder).Substring(1, 6);

                    if (_publicServices.IsFolderProcessed(mainFolder)) return;

                    try
                    {
                        ProcessFolderWithLogging(mainFolder, personnelCode);
                        _publicServices.LogProcessing(mainFolder, personnelCode, null, "Completed", null);
                        //Counter += 1;
                    }
                    catch (Exception ex)
                    {
                        LogError(mainFolder, personnelCode, null, ex.Message);
                    }
                });
        }

        private void ProcessFolderWithLogging(string folderPath, string personnelCode)
        {
            ProcessFilesInFolder(folderPath, personnelCode, "root");

            for (int i = 1; i <= 6; i++)
            {
                string category = $"c{i}";
                string subFolder = Path.Combine(folderPath, $"{Path.GetFileName(folderPath)}-{category}");
                if (Directory.Exists(subFolder))
                {
                    ProcessFilesInFolder(subFolder, personnelCode, category);
                }
            }
        }

        private void ProcessFilesInFolder(string folderPath, string personnelCode, string category)
        {
            var files = Directory.GetFiles(folderPath, $"{Path.GetFileName(folderPath)}-*");

            foreach (var file in files.OrderBy(f => f))
            {
                if (_publicServices.IsFileProcessed(file)) continue;

                try
                {
                    byte[] fileBytes = File.ReadAllBytes(file);
                    string fileName = Path.GetFileName(file);
                    string fileExtension = Path.GetExtension(file);

                    //Run(fileBytes, fileName, fileExtension, personnelCode, category);
                    _publicServices.LogProcessing(file, personnelCode, category, "Processed", null);
                    Counter1 += 1;
                }
                catch (Exception ex)
                {
                    LogError(file, personnelCode, category, ex.Message);
                }
            }
        }

        private void LogError(string fileName, string personnelCode, string category, string errorMessage)
        {
            _publicServices.LogProcessing(fileName, personnelCode, category, "Error", errorMessage);
        }

    }
}
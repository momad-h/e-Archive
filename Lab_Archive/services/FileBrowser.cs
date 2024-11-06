using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive
{
    internal class FileBrowser
    {
        private string _path;
        public void ProcessMainFolders(string rootPath)
        {
            _path = rootPath;
            var mainFolders = Directory.GetDirectories(rootPath, "p??????");

            foreach (var mainFolder in mainFolders.OrderBy(f => f))
            {
                string personnelCode = Path.GetFileName(mainFolder).Substring(1, 6);

                ProcessFilesInFolder(mainFolder, personnelCode, null);

                for (int i = 1; i <= 6; i++)
                {
                    string category = $"c{i}";
                    string subFolder = Path.Combine(mainFolder, $"{Path.GetFileName(mainFolder)}-{category}");
                    if (Directory.Exists(subFolder))
                    {
                        ProcessFilesInFolder(subFolder, personnelCode, category);
                    }
                }
            }
        }

        private void ProcessFilesInFolder(string folderPath, string personnelCode, string category)
        {
            var files = Directory.GetFiles(folderPath, $"{Path.GetFileName(folderPath)}-*");

            foreach (var file in files.OrderBy(f => f))
            {
                byte[] fileBytes = File.ReadAllBytes(file);
                string fileName = Path.GetFileName(file);
                string fileExtension = Path.GetExtension(file);

                Run(fileBytes, fileName, fileExtension, personnelCode, category);
            }
        }

        private void Run(byte[] fileBytes, string fileName, string type, string personnelCode, string category)
        {
            File.AppendAllText(_path + @"\Log.txt", $"Processing file: {fileName},    Type: {type},   Personnel Code: {personnelCode},   Category: {category}" + Environment.NewLine);
        }
    }
}

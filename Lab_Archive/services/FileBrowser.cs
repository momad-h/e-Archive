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

        private void ProcessFilesInFolder(string mainFolder, string personnelCode, object value)
        {
            throw new NotImplementedException();
        }
    }
}

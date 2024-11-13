using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive
{
    public class ProccessByPersonelID
    {
        public int Counter = 0;
        public int TotalCounter = 0;
        private int parentEc = 0;
        DocumentManagement documentManagement;
        SubDocumentManagement subDocumentManagement;
        FormManagement formManagement;
        PublicServices services;
        public ProccessByPersonelID()
        {
            documentManagement = new DocumentManagement();
            subDocumentManagement = new SubDocumentManagement();
            formManagement = new FormManagement();
            services = new PublicServices();
        }
        public void ProcessFolder(string baseFolder, string personalCode)
        {
            try
            {
                string mainFolderPath = Path.Combine(baseFolder, "p" + personalCode);
                if (!Directory.Exists(mainFolderPath))
                {
                    throw new Exception($"[{personalCode}]: پوشه مورد نظر پیدا نشد.");
                }

                ProcessSubFoldersAndFiles(mainFolderPath, personalCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProcessSubFoldersAndFiles(string folderPath, string personalCode)
        {
            try
            {
                foreach (var file in Directory.GetFiles(folderPath))
                {
                    ProcessFile(file, personalCode);
                }

                foreach (var subFolder in Directory.GetDirectories(folderPath))
                {
                    ProcessSubFoldersAndFiles(subFolder, personalCode);
                }

                if (Directory.GetFileSystemEntries(folderPath).Length == 0)
                {
                    Directory.Delete(folderPath);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ProcessFile(string filePath, string personalCode)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);
                string extension = Path.GetExtension(filePath);
                string category = GetCategoryFromFileName(fileName);
                byte[] fileData = File.ReadAllBytes(filePath);

                bool result = Run(fileData, fileName, extension, category, filePath, personalCode);

                if (result)
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string GetCategoryFromFileName(string fileName)
        {
            try
            {
                var parts = fileName.Split('-');
                return parts.Length > 1 ? parts[1] : "";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool Run(byte[] fileData, string fileName, string type, string category, string fullPath, string personalCode)
        {
            try
            {
                string multiFormField;
                parentEc = documentManagement.MainForm_InsertDocument(ConfigInfo.MasterETC, ConfigInfo.WhereConditionFieldName, personalCode);
                int result = subDocumentManagement.SubForm_InsertDocument(fileData, fileName, type, personalCode, category);

                switch (category)
                {
                    case "c1": multiFormField = "PersonnelInfo_C1"; break;
                    case "c2": multiFormField = "PersonnelInfo_C2"; break;
                    case "c3": multiFormField = "PersonnelInfo_C3"; break;
                    case "c4": multiFormField = "PersonnelInfo_C4"; break;
                    case "c5": multiFormField = "PersonnelInfo_C5"; break;
                    case "c6": multiFormField = "PersonnelInfo_C6"; break;
                    default: multiFormField = "PersonnelInfo_Other"; break;
                }


                if (result != 0 && parentEc != 0)
                {
                    formManagement.SetSlaveFormInMaster(ConfigInfo.MasterETC, parentEc, ConfigInfo.SlaveETC, result, multiFormField);
                    Counter += 1;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

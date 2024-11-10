using Lab_Archive.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive
{
    public interface IPublicServices
    {
        bool Login();
        bool Logout();
        void Loging(LogInfo logInfo);
        InsertResult InsertDocument(string xmlStr);
        bool AttachFileInForm(byte[] bfile, string fileName, string fileExtension, int MainETC, int MainEC, string FieldName, bool sendFileISFarzinEncryption);
        int CountFiles(string path);
        InsertResult ParseXml(string xmlString);
        void LogProcessing(string fileName, string personnelCode, string category, string status, string errorMessage);
        bool IsFileProcessed(string filePath);
        bool IsLogExists(string fileName, string status);
        bool IsFolderProcessed(string folderPath);
        DataTable GetDataForAddSubForm();
        void UpdateSubFormLog(DataTable data);
        void AddFileToFildLog(int etc, int ec,int status=1);
        bool SetSlaveFormInMaster(int ETC_Master, int EC_Master, int ETC_Slave, int EC_Slave, string fieldName);
        int GetUserIDByPersonalID(string personalID);
    }
}
using Lab_Archive.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab_Archive.services
{
    public class SubDocumentManagement
    {
        public string SubForm_InsertDocument(byte[] bFile, string fileName,string fileExtention, string peersonnelID, string category)
        {
            LogInfo logInfo;
            try
            {
                InsertResult result = new InsertResult();
                string inputStr = "";
                PublicServices services = new PublicServices();
                if (services.Login())
                {
                    result = services.InsertDocument(inputStr);
                    logInfo = new LogInfo() { ETC = result.ETC.ToString(), EC = result.EC.ToString(), Category = category, Message = "Success", FileName = fileName, Level = "SubForm_InsertDocument"};
                    services.Loging(logInfo);
                    bool addFileRes=services.AttachFileInForm(bFile,fileName, fileExtention,result.ETC,result.EC,ConfigInfo.SubFormFileFieldName,false);
                    if (addFileRes)
                    {
                        services.AddFileToFildLog(result.ETC,result.EC);
                    }
                    services.Logout();
                }
                return fileName;
            }
            catch (Exception ex)
            {
                logInfo = new LogInfo() { Message = ex.Message, Level = "SubForm_InsertDocument", StackTrace = ex.StackTrace, FileName = fileName };
                return fileName+ "_Failed";
            }
        }
        public void SubForm_InsertDocument(DataTable inputs)
        {
            try
            {
                foreach (DataRow row in inputs.Rows)
                {
                    byte[] bFile = File.ReadAllBytes(row["FullFilePath"].ToString());
                    string fileName = row["FileName"].ToString();
                    string peersonnelID= row["PersonnelCode"].ToString();
                    string category= row["Category"].ToString();
                    string extentionType = row["FileExtension"].ToString();
                    SubForm_InsertDocument(bFile,fileName,extentionType, peersonnelID,category);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

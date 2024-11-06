using Lab_Archive.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive.services
{
    public class SubDocumentManagement
    {
        public string SubForm_InsertDocument(byte[] file, string fileName, string peersonnelID, string category)
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
    }
}

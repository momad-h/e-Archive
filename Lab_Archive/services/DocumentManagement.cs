using Lab_Archive.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive
{
    class DocumentManagement
    {

        public string MainForm_InsertDocument(string personnelId,string xmlStr)
        {
            LogInfo logInfo;
            try
            {
                InsertResult result = new InsertResult();
                PublicServices services = new PublicServices();
                if (services.Login())
                {
                    result = services.InsertDocument(personnelId, xmlStr);
                    logInfo = new LogInfo() { ETC = result.ETC.ToString(), EC = result.EC.ToString(),Message = "Success", Level = "MainForm_InsertDocument" };
                    services.Loging(logInfo);
                    services.Logout();
                }
                return result.EC.ToString();
            }
            catch (Exception ex)
            {
                logInfo = new LogInfo() { Message = ex.Message, Level = "MainForm_InsertDocument", StackTrace = ex.StackTrace};
                return  "-1";
            }
        }
    }

    
}


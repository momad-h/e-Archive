using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAN.FarzinSDK.WebServices.Proxy;
using Lab_Archive.DataModels;

namespace Lab_Archive
{
    public class ServiceManager
    {
        LogInfo logInfo;
        public bool Start_Service(int ETC,int EC,int WFID,int Starter)
        {
            string infoMsgService;
            string errMsgService;
            bool resService;

            StringBuilder contentXml = new StringBuilder();
            contentXml.Insert(0, "<Entity><ETC>");
            contentXml.Append(ETC.ToString());
            contentXml.Append("</ETC><EC>");
            contentXml.Append(EC.ToString());
            contentXml.Append("</EC><Starter>");
            contentXml.Append(Starter.ToString());
            contentXml.Append("</Starter></Entity>");
            PublicServices services = new PublicServices();

            try
            {
                
                if (services.Login())
                {
                    WorkflowManagment wm = new WorkflowManagment(ConfigInfo.FarzinUrl);
                    resService = wm.StartService(WFID, contentXml.ToString(), out infoMsgService, out errMsgService);
                    if (resService)
                    {
                        logInfo = new LogInfo() { ETC = ETC.ToString(), EC = EC.ToString(), Message = "Success", StackTrace = infoMsgService, Level = "Start_Service" };
                        
                    }
                    else
                    {
                        logInfo = new LogInfo() { ETC = ETC.ToString(), EC = EC.ToString(), Message = "Exception", StackTrace = errMsgService, Level = "Start_Service" };
                    }
                    services.Loging(logInfo);
                    services.Logout();
                    return resService;
                }
                return false;

            }
            catch (Exception ex)
            {

                logInfo = new LogInfo() { ETC = ETC.ToString(), EC = EC.ToString(), Message = "Exception", StackTrace = ex.ToString(), Level = "Start_Service" };
                services.Loging(logInfo);
                services.Logout();
                return false;
            }
            
        }
    }
}

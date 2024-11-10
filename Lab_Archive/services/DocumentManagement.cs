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

        public string MainForm_InsertDocument(string personnelID)
        {
            LogInfo logInfo;
            try
            {
                InsertResult result = new InsertResult();
                PublicServices services = new PublicServices();
                string xmlStr = "<Document>";
                xmlStr += "<SourceSoftware name='ElectronicFilePersonnel' version='1.0' repository='Entity_ElectronicFilePersonnel' />";
                xmlStr += "<Structure>";
                xmlStr += "<BaseFields>";
                xmlStr += "<Field name='CreatorID' type='int''><![CDATA[349]]></Field>";
                xmlStr += "<Field name='CreatorRoleID' type='int'><![CDATA[421]]></Field>";
                xmlStr += "</BaseFields>";
                xmlStr += "<Field name='PersonnelSpecifications' type='int'><![CDATA[" + services.GetUserIDByPersonalID(personnelID) + "]]></Field>";
                xmlStr += "<Signatures>";
                xmlStr += "<Sign userName='Ican' fieldName='PersonnelSigne' type='nvarchar'>";
                xmlStr += "</Sign>";
                xmlStr += "</Signatures>";
                xmlStr += "</Structure>";
                xmlStr += "</Document>";
                if (services.Login())
                {
                    result = services.InsertDocument(xmlStr);
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


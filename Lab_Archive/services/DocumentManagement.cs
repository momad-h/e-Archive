using Lab_Archive.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive
{
    public class DocumentManagement
    {
        IPublicServices services;
        LogInfo logInfo;
        public DocumentManagement()
        {
            services = new PublicServices();
        }
        public string MainForm_InsertDocument(string personnelID)
        {
            try
            {
                InsertResult result = new InsertResult();
                PublicServices services = new PublicServices();
                string xmlStr = "<Document>";
                xmlStr += "<SourceSoftware name='ElectronicFilePersonnel' version='1.0' repository='Entity_ElectronicFilePersonnel' />";
                xmlStr += "<Structure>";
                xmlStr += "<BaseFields>";
                xmlStr += "<Field name='CreatorID' type='int'><![CDATA[349]]></Field>";
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
                    logInfo = new LogInfo() { ETC = result.ETC.ToString(), EC = result.EC.ToString(), Message = "Success", Level = "MainForm_InsertDocument",PersonnelID=personnelID };
                    services.Loging(logInfo);
                    services.Logout();
                }
                return result.EC.ToString();
            }
            catch (Exception ex)
            {
                logInfo = new LogInfo() { Message = ex.Message, Level = "MainForm_InsertDocument", StackTrace = ex.StackTrace,PersonnelID=personnelID };
                return "-1";
            }
        }
        public void MainForm_InsertDocument()
        {
            string personnelID = "";
            DataTable inputs = services.GetDataForAddMainForm();
            try
            {
                foreach (DataRow row in inputs.Rows)
                {
                    personnelID = row["PersonnelCode"].ToString();
                    string res = MainForm_InsertDocument(personnelID);
                    if (res != "Failed")
                    {
                        row["MainFormInsert"] = 1;
                    }
                    else
                    {
                        row["MainFormInsert"] = -1;
                    }
                }
                services.UpdateMainFormLog(inputs);
            }
            catch (Exception ex)
            {
                logInfo = new LogInfo() { Message = ex.Message, Level = "MainForm_InsertDocument", StackTrace = ex.StackTrace, PersonnelID = personnelID };
                services.Loging(logInfo);
            }
        }
    }


}


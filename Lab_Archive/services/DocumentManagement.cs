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
        public int TotalCounter = 0;
        public int Counter = 0;
        public DocumentManagement()
        {
            services = new PublicServices();
        }
        public int MainForm_InsertDocument(int parentEtc, string whereConditionFieldName, string personnelID)
        {
            try
            {
                InsertResult result = new InsertResult();
                PublicServices services = new PublicServices();
                int parentEc = 0;
                int userid = services.GetUserIDByPersonalID(personnelID);
                string whereCondition = $"{whereConditionFieldName}={userid}";
                string xmlStr = "<Document>";
                xmlStr += "<SourceSoftware name='ElectronicFilePersonnel' version='1.0' repository='Entity_ElectronicFilePersonnel' />";
                xmlStr += "<Structure>";
                xmlStr += "<BaseFields>";
                xmlStr += "<Field name='CreatorID' type='int'><![CDATA[349]]></Field>";
                xmlStr += "<Field name='CreatorRoleID' type='int'><![CDATA[421]]></Field>";
                xmlStr += "</BaseFields>";
                xmlStr += "<Field name='PersonnelSpecifications' type='int'><![CDATA[" + userid + "]]></Field>";
                xmlStr += "<Signatures>";
                xmlStr += "<Sign userName='Ican' fieldName='PersonnelSigne' type='nvarchar'>";
                xmlStr += "</Sign>";
                xmlStr += "</Signatures>";
                xmlStr += "</Structure>";
                xmlStr += "</Document>";
                if (services.Login())
                {
                    parentEc = services.CheckExistsDocument(parentEtc, whereCondition);
                    if (parentEc == -1)
                    {
                        result = services.InsertDocument(xmlStr);
                    }
                    else
                    {
                        result.EC = parentEc;
                    }
                    services.Logout();
                }
                return result.EC;
            }
            catch (Exception ex)
            {
                logInfo = new LogInfo() { Message = ex.Message, Level = "MainForm_InsertDocument", StackTrace = ex.StackTrace, PersonnelID = personnelID };
                return 0;
            }
        }
        public void MainForm_InsertDocument()
        {
            string personnelID = "";
            DataTable inputs = services.GetDataForAddMainForm();
            try
            {
                TotalCounter = inputs.Rows.Count;
                foreach (DataRow row in inputs.Rows)
                {
                    personnelID = row["PersonnelCode"].ToString();
                    int res = MainForm_InsertDocument(3466,"",personnelID);
                    if (res != 0)
                    {
                        row["MainFormInsert"] = 1;
                        Counter += 1;
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


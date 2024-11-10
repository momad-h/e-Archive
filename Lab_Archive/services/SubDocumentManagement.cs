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
        IPublicServices services;
        LogInfo logInfo;
        public SubDocumentManagement()
        {
            services = new PublicServices();
        }
        public string SubForm_InsertDocument(byte[] bFile, string fileName, string fileExtention, string personnelID, string category)
        {
            try
            {
                int ctg = 0;
                switch (category)
                {
                    case "c1": ctg = 1; break;
                    case "c2": ctg = 2; break;
                    case "c3": ctg = 3; break;
                    case "c4": ctg = 4; break;
                    case "c5": ctg = 5; break;
                    case "c6": ctg = 6; break;
                    default: ctg = 0; break;
                }

                InsertResult result = new InsertResult();
                string xmlStr = "<Document>";
                xmlStr += "<SourceSoftware name='PersonnelDetail' version='1.0' repository='Entity_PersonnelDetail' />";
                xmlStr += "<Structure>";
                xmlStr += "<BaseFields>";
                xmlStr += "<Field name='CreatorID' type='int'><![CDATA[349]]></Field>";
                xmlStr += "<Field name='CreatorRoleID' type='int'><![CDATA[421]]></Field>";
                xmlStr += "</BaseFields>";
                xmlStr += "<Field name='SpecificationPersonnel' type='int'><![CDATA[" + services.GetUserIDByPersonalID(personnelID) + "]]></Field>";
                xmlStr += "<Field name='GroupAndTag' type='int'><![CDATA[" + ctg + "]]></Field>";
                xmlStr += "<Signatures>";
                xmlStr += "<Sign userName='Ican' fieldName='Sign' type='nvarchar'>";
                xmlStr += "</Sign>";
                xmlStr += "</Signatures>";
                xmlStr += "</Structure>";
                xmlStr += "</Document>";
                if (services.Login())
                {
                    result = services.InsertDocument(xmlStr);
                    logInfo = new LogInfo() { ETC = result.ETC.ToString(), EC = result.EC.ToString(), Category = category, Message = "Success", FileName = fileName, Level = "SubForm_InsertDocument" };
                    services.Loging(logInfo);
                    bool addFileRes = services.AttachFileInForm(bFile, fileName, fileExtention, result.ETC, result.EC, ConfigInfo.SubFormFileFieldName, false);
                    if (addFileRes)
                    {
                        services.AddFileToFildLog(result.ETC, result.EC);
                    }
                    services.Logout();
                }
                return fileName;
            }
            catch (Exception ex)
            {
                logInfo = new LogInfo() { Message = ex.Message, Level = "SubForm_InsertDocument", StackTrace = ex.StackTrace, FileName = fileName };
                services.Loging(logInfo);
                return "Failed";
            }
        }
        public void SubForm_InsertDocument()
        {
            string fileName = "";
            string personnelID = "";
            string category = "";
            string extentionType = "";
            DataTable inputs = services.GetDataForAddSubForm();
            try
            {
                foreach (DataRow row in inputs.Rows)
                {
                    byte[] bFile = File.ReadAllBytes(row["FullFilePath"].ToString());
                    fileName = row["FileName"].ToString();
                    personnelID = row["PersonnelCode"].ToString();
                    category = row["Category"].ToString();
                    extentionType = row["FileExtension"].ToString();
                    string res = SubForm_InsertDocument(bFile, fileName, extentionType, personnelID, category);
                    if (res != "Failed")
                    {
                        row["InsertStatus"] = 1;
                    }
                }
                services.UpdateSubFormLog(inputs);
            }
            catch (Exception ex)
            {
                logInfo = new LogInfo() { Message = ex.Message, Level = "SubForm_InsertDocument", StackTrace = ex.StackTrace, FileName = fileName };
                services.Loging(logInfo);
            }
        }

    }
}

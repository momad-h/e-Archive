using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using ICAN.FarzinSDK.WebServices.Proxy;
using Lab_Archive.DataModels;
using System.IO;

namespace Lab_Archive
{
    public class PublicServices : IPublicServices
    {
        private string _errMessage = "";
        private string _key = "";
        private string _connectionStr;
        private string _farzinUrl;
        private string _farzinUsername;
        private string _farzinPassword;
        public PublicServices()
        {
            _connectionStr = ConfigInfo.ConnectionStr;
            _farzinUrl = ConfigInfo.FarzinUrl;
            _farzinUsername = ConfigInfo.FarzinUsername;
            _farzinPassword = ConfigInfo.FarzinPassword;
        }
        public bool Login()
        {
            try
            {
                CSHA1 cSHA1 = new CSHA1();
                string hashPassword = cSHA1.hex_sha1(_farzinPassword);
                Authentication authentication = new Authentication(_farzinUrl, _key, 300000);
                if (!authentication.Login(_farzinUsername, hashPassword, out _errMessage))
                {
                    throw new Exception(_errMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Logout()
        {
            try
            {
                Authentication authentication = new Authentication(_farzinUrl, _key, 300000);
                if (!authentication.Logout(out _errMessage))
                {
                    throw new Exception(_errMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Loging(LogInfo logInfo)
        {
            try
            {
                string _query = "ICAN_SP_AddToArchiveLog";
                SqlConnection connection = new SqlConnection(_connectionStr);
                SqlCommand command = new SqlCommand()
                {
                    CommandText = _query,
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Message", logInfo.Message);
                command.Parameters.AddWithValue("@Level", logInfo.Level);
                command.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                command.Parameters.AddWithValue("@StackTrace", logInfo.StackTrace ?? "");
                command.Parameters.AddWithValue("@ETC", logInfo.ETC ?? "0");
                command.Parameters.AddWithValue("@EC", logInfo.EC ?? "0");
                command.Parameters.AddWithValue("@PersonnelID", logInfo.PersonnelID ?? "0");
                command.Parameters.AddWithValue("@Category", logInfo.Category ?? "");
                command.Parameters.AddWithValue("@FileName", logInfo.FileName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public InsertResult InsertDocument(string xmlStr)
        {
            try
            {
                InsertResult insertResult = new InsertResult();
                eFormManagment eForm = new eFormManagment(_farzinUrl, _key, 300000);
                insertResult=ParseXml(eForm.InsertDocument(xmlStr));
                return insertResult;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public InsertResult ParseXml(string xmlString)
        {
            InsertResult insertResult = new InsertResult();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            XmlNode documentNode = xmlDoc.SelectSingleNode("//Document");

            if (documentNode != null)
            {
                insertResult.ETC = Convert.ToInt32(documentNode.Attributes["entityTypeCode"].Value);
                insertResult.EC = Convert.ToInt32(documentNode.Attributes["entityCode"].Value);
                insertResult.EntityNumber = documentNode.Attributes["EntityNumber"].Value;
            }
            else
            {
                XmlNode resultNode = xmlDoc.SelectSingleNode("//Result");

                if (resultNode != null)
                {
                    insertResult.ErrorMessage = resultNode.Attributes["errorMessage"].Value;
                }
            }
            return insertResult;
        }

        public int CountFiles(string path)
        {
            int count = 0;

            try
            {
                count += Directory.GetFiles(path).Length;

                foreach (string directory in Directory.GetDirectories(path))
                {
                    count += CountFiles(directory);
                }
            }
            catch (Exception ex)
            {
                Loging(new LogInfo() { Level= "CountFiles",Message=ex.Message,StackTrace=ex.StackTrace,FileName="N/A" });
            }

            return count;
        }
    }
}
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
using static System.Net.WebRequestMethods;
using Microsoft.SqlServer.Server;

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
        private int _numberOfRecordsFetched;
        public PublicServices()
        {
            _connectionStr = ConfigInfo.ConnectionStr;
            _farzinUrl = ConfigInfo.FarzinUrl;
            _farzinUsername = ConfigInfo.FarzinUsername;
            _farzinPassword = ConfigInfo.FarzinPassword;
            _numberOfRecordsFetched = ConfigInfo.NumberOfRecordsFetched;
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
            string _query = "ICAN_SP_AddToArchiveLog";
            SqlConnection connection = new SqlConnection(_connectionStr);
            try
            {
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
                command.Parameters.AddWithValue("@FileName", logInfo.FileName ?? "");
                command.Parameters.AddWithValue("@AddFileStat", logInfo.AddFileStat);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }
        public InsertResult InsertDocument(string xmlStr)
        {
            try
            {
                InsertResult insertResult = new InsertResult();
                eFormManagment eForm = new eFormManagment(_farzinUrl, _key, 300000);
                insertResult = ParseXml(eForm.InsertDocument(xmlStr));
                return insertResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int CheckExistsDocument(int etc,string whereCondition)
        {
            int ec = 0;
            string errMessage = ""; 
            try
            {
                eFormManagment eForm = new eFormManagment(_farzinUrl, _key, 300000);
                bool isExistst=eForm.CheckExistDocument(etc,whereCondition,out ec,out errMessage);
                return ec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AttachFileInForm(byte[] bFile, string fileName, string fileExtension, int mainETC, int mainEC, string fieldName, bool sendFileISFarzinEncryption)
        {
            try
            {
                eFormManagment eForm = new eFormManagment(_farzinUrl, _key, 300000);
                bool res = eForm.AttachFileInForm(bFile, fileName, fileExtension, mainETC, mainEC, fieldName, sendFileISFarzinEncryption, out _errMessage);
                if (!res)
                {
                    throw new Exception(_errMessage);
                }
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
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
                Loging(new LogInfo() { Level = "CountFiles", Message = ex.Message, StackTrace = ex.StackTrace, FileName = "N/A" });
            }

            return count;
        }

        public void LogProcessing(string fileName, string personnelCode, string category, string status, string errorMessage)
        {
            // اگر لاگ قبلاً ثبت شده، دوباره لاگ ثبت نمی‌کنیم
            if (IsLogExists(fileName, status)) return;

            string fullFilePath = (status == "Processed") ? fileName : null;
            string fileExtension = (status == "Processed") ? Path.GetExtension(fileName) : null;

            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO FileProcessingLog (FileName, FolderPath, FullFilePath, FileExtension, PersonnelCode, Category, Status, ProcessedOn, ErrorMessage) " +
                    "VALUES (@FileName, @FolderPath, @FullFilePath, @FileExtension, @PersonnelCode, @Category, @Status, @ProcessedOn, @ErrorMessage)", conn))
                {
                    cmd.Parameters.AddWithValue("@FileName", Path.GetFileName(fileName));
                    cmd.Parameters.AddWithValue("@FolderPath", Path.GetDirectoryName(fileName));
                    cmd.Parameters.AddWithValue("@FullFilePath", (object)fullFilePath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FileExtension", (object)fileExtension ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PersonnelCode", personnelCode);
                    cmd.Parameters.AddWithValue("@Category", category ?? "");
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@ProcessedOn", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ErrorMessage", (object)errorMessage ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool IsFileProcessed(string filePath)
        {
            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM FileProcessingLog WHERE FileName = @FileName AND Status = 'Processed'", conn))
                {
                    cmd.Parameters.AddWithValue("@FileName", Path.GetFileName(filePath));
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public bool IsLogExists(string fileName, string status)
        {
            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(1) FROM FileProcessingLog WHERE FileName = @FileName AND Status = @Status", conn))
                {
                    cmd.Parameters.AddWithValue("@FileName", Path.GetFileName(fileName));
                    cmd.Parameters.AddWithValue("@Status", status);

                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public bool IsFolderProcessed(string folderPath)
        {
            using (SqlConnection conn = new SqlConnection(_connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM FileProcessingLog WHERE FolderPath = @FolderPath AND Status = 'Completed'", conn))
                {
                    cmd.Parameters.AddWithValue("@FolderPath", folderPath);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public DataTable GetDataForAddSubForm()
        {
            string _query = "ICAN_SP_GetDataForAddSubForm";
            try
            {
                DataTable dt = new DataTable();
                SqlConnection conn = new SqlConnection(_connectionStr);
                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = _query,
                    CommandType = CommandType.StoredProcedure,
                    Connection = conn
                };
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSubFormLog(DataTable data)
        {
            string _queryUpdate = "ICAN_SP_UpdateDataForAddSubForm";
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.UpdateCommand = new SqlCommand()
                {
                    CommandText = _queryUpdate,
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };

                adapter.UpdateCommand.Parameters.Add("@InsertStatus", SqlDbType.VarChar, 50, "InsertStatus");
                adapter.UpdateCommand.Parameters.Add("@LogId", SqlDbType.Int, 0, "LogId").SourceVersion = DataRowVersion.Original;

                adapter.Update(data);
            }
        }

        public void AddFileToFildLog(int etc, int ec, int status = 1)
        {
            string _query = "ICAN_SP_UpdateArchiveLog_FileAdd";
            SqlConnection con = new SqlConnection(_connectionStr);
            try
            {
                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = _query,
                    CommandType = CommandType.StoredProcedure,
                    Connection = con
                };
                cmd.Parameters.AddWithValue("@ETC", etc);
                cmd.Parameters.AddWithValue("@EC", ec);
                cmd.Parameters.AddWithValue("@Stat", status);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
        }

        public bool SetSlaveFormInMaster(int etcMaster, int ecMaster, int etcSlave, int ecSlave, string fieldName)
        {
            SqlConnection connection = new SqlConnection(_connectionStr);
            try
            {
                string query = "ICAN_SP_SetSlaveForm";
                SqlCommand command = new SqlCommand()
                {
                    CommandText = query,
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@ETC_Master", etcMaster);
                command.Parameters.AddWithValue("@EC_Master", ecMaster);
                command.Parameters.AddWithValue("@ETC_Slave", etcSlave);
                command.Parameters.AddWithValue("@EC_Slave", ecSlave);
                command.Parameters.AddWithValue("@fieldName", fieldName);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        public int GetUserIDByPersonalID(string personalID)
        {
            SqlConnection connection = new SqlConnection(_connectionStr);
            try
            {
                string query = "ICAN_FN_GetUserIDByPersonalID";
                SqlCommand command = new SqlCommand()
                {
                    CommandText = query,
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@PID", Convert.ToInt32(personalID));
                SqlParameter returnVal = command.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnVal.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return Convert.ToInt32(returnVal.Value);
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        public DataTable GetDataForAddMainForm()
        {
            string _query = "ICAN_SP_GetDataForAddMainForm";
            try
            {
                DataTable dt = new DataTable();
                SqlConnection conn = new SqlConnection(_connectionStr);
                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = _query,
                    CommandType = CommandType.StoredProcedure,
                    Connection = conn
                };
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateMainFormLog(DataTable data)
        {
            string _queryUpdate = "ICAN_SP_UpdateDataForAddMainForm";
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.UpdateCommand = new SqlCommand()
                {
                    CommandText = _queryUpdate,
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };

                adapter.UpdateCommand.Parameters.Add("@MainFormInsert", SqlDbType.VarChar, 50, "MainFormInsert");
                adapter.UpdateCommand.Parameters.Add("@LogId", SqlDbType.Int, 0, "LogId").SourceVersion = DataRowVersion.Original;

                adapter.Update(data);
            }
        }

        public DataTable GetPersonalCodStrFromUsers()
        {
            string _query = "ICAN_SP_GetPersonalCodStrFromUser";
            try
            {
                DataTable dt = new DataTable();
                SqlConnection conn = new SqlConnection(_connectionStr);
                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = _query,
                    CommandType = CommandType.StoredProcedure,
                    Connection = conn
                };
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAN.FarzinSDK.WebServices.Proxy;
using Lab_Archive.DataModels;

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
                    CommandText= _query,
                    CommandType=CommandType.StoredProcedure,
                    Connection=connection
                };
                command.Parameters.AddWithValue("@Message", logInfo.Message);
                command.Parameters.AddWithValue("@Level", logInfo.Level);
                command.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                command.Parameters.AddWithValue("@StackTrace", logInfo.StackTrace);
                command.Parameters.AddWithValue("@ETC", logInfo.ETC);
                command.Parameters.AddWithValue("@EC", logInfo.EC);
                command.Parameters.AddWithValue("@PersonnelID", logInfo.PersonnelID);
                command.Parameters.AddWithValue("@Category", logInfo.Category);
                command.Parameters.AddWithValue("@FileName", logInfo.FileName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAN.FarzinSDK.WebServices.Proxy;

namespace Lab_Archive
{
    public class PublicServices : IPublicServices
    {
        private string _errMessage = "";
        private string _key = "";
        private string _connectionStr;
        private string _dbUserName;
        private string _dbPassword;
        private string _farzinUrl;
        private string _farzinUsername;
        private string _farzinPassword;
        public PublicServices(ConfigInfo config)
        {
            _connectionStr = config.ConnectionStr;
            _dbUserName = config.DbUserName;
            _dbPassword = config.DbPassword;
            _farzinUrl = config.FarzinUrl;
            _farzinUsername = config.FarzinUsername;
            _farzinPassword = config.DbPassword;
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
        public void LoadConfig()
        {
            throw new NotImplementedException();
        }
        public void Loging()
        {
            throw new NotImplementedException();
        }

    }
}

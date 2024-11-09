using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive
{
    public class FormManagement
    {
        private string connectionStr;
        public FormManagement()
        {
            connectionStr = ConfigInfo.ConnectionStr;
        }
        public bool SetSlaveFormInMaster(int ETC_Master, int EC_Master, int ETC_Slave, int EC_Slave,string fieldName)
        {
            try
            {
                string query = "ICAN_SP_SetSlaveForm";
                SqlConnection connection = new SqlConnection(connectionStr);
                SqlCommand command = new SqlCommand()
                {
                    CommandText = query,
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@ETC_Master", ETC_Master);
                command.Parameters.AddWithValue("@EC_Master", EC_Master);
                command.Parameters.AddWithValue("@ETC_Slave", ETC_Slave);
                command.Parameters.AddWithValue("@EC_Slave", EC_Slave);
                command.Parameters.AddWithValue("@fieldName", fieldName);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
            
            
        }
    }
}

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
        public bool SetSlaveFormInMaster(int etcMaster, int ecMaster, int etcSlave, int ecSlave, string fieldName)
        {
            SqlConnection connection = new SqlConnection(connectionStr);
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
                command.Parameters.AddWithValue("@FieldName", fieldName);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception)
            {
                connection.Close();
                return false;
            }
        }
    }
}

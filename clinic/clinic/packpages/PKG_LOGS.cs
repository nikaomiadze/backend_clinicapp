using Oracle.ManagedDataAccess.Client;

namespace clinic.packpages
{
   
        public interface IPKG_LOG
        {
            public void Add_log(string message, string? username = null);
        }
        public class PKG_LOGS : PKG_BASE, IPKG_LOG
        {

            public PKG_LOGS(IConfiguration config) : base(config) { }

            public void Add_log(
                string message, string? email = null)
            {
                OracleConnection conn = new OracleConnection
                {
                    ConnectionString = ConnStr
                };
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "olerning.PKG_NO_CLINIC_LOGS.add_logs";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("p_message", OracleDbType.Varchar2).Value = message;
                cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;

                cmd.ExecuteNonQuery();


                conn.Close();
            }

        }
    
}

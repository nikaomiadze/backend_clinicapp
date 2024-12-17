using clinic.models;
using Oracle.ManagedDataAccess.Client;

namespace clinic.packpages
{
    public interface IPKG_ADMIN
    {
        void Add_Doctor(Doctor doctor, byte[] pic, byte[] cv);
    }
    public class PKG_ADMIN : PKG_BASE, IPKG_ADMIN
    {
        public PKG_ADMIN(IConfiguration configuration) : base(configuration) { }
        public void Add_Doctor(Doctor doctor, byte[] pic, byte[] cv)
        {

            OracleConnection conn = new OracleConnection
            {
                ConnectionString = ConnStr
            };
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = "olerning.PKG_NO_CLINIC_DOCTOR.Add_Doctor";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("v_first_name", OracleDbType.Varchar2).Value = doctor.FirstName;
                cmd.Parameters.Add("v_last_name", OracleDbType.Varchar2).Value = doctor.LastName;
                cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = doctor.Email;
                cmd.Parameters.Add("v_person_id", OracleDbType.Varchar2).Value = doctor.Person_id;
                if (pic != null)
                {
                    cmd.Parameters.Add("v_profile_img", OracleDbType.Blob).Value = pic;
                }
                else
                {
                    cmd.Parameters.Add("v_profile_img", OracleDbType.Blob).Value = DBNull.Value;
                }

                if (cv != null)
                {
                    cmd.Parameters.Add("v_cv", OracleDbType.Blob).Value = cv;
                }
                else
                {
                    cmd.Parameters.Add("v_cv", OracleDbType.Blob).Value = DBNull.Value;
                }

                cmd.Parameters.Add("v_category_id", OracleDbType.Int32).Value = doctor.Category_id;
                cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = doctor.Password;


                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                // Check for the specific error code from the stored procedure
                if (ex.Number == 20002) // Match the error code defined in the stored procedure
                {
                    throw new InvalidOperationException("A doctor with this email is already registered.", ex);
                }
                
                else
                {
                    // Rethrow for any other errors
                    throw;
                }
            }
            conn.Close();
        }
    }
}

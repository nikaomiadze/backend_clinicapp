using clinic.models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using static clinic.packpages.PKG_USER;

namespace clinic.packpages
{
    public interface IPKG_USER
    {
        void Add_User(User user, byte[] pic);
        List<User> Get_Users();
        void Add_v_code(string toEmail, string verificationCode);
        bool ValidateVerificationCode(string email, string verificationCode);
        User? authentification(Login loginData);
        User? GetUserById(string id);
    }
    public class PKG_USER : PKG_BASE, IPKG_USER
    {
        public PKG_USER(IConfiguration configuration) : base(configuration) { }
        public void Add_User(User user, byte[] pic)
        {

            OracleConnection conn = new OracleConnection
            {
                ConnectionString = ConnStr
            };
            try
            {
                conn.Open();

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "olerning.PKG_NO_CLINIC.Add_User";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("v_first_name", OracleDbType.Varchar2).Value = user.FirstName;
                cmd.Parameters.Add("v_last_name", OracleDbType.Varchar2).Value = user.LastName;
                cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = user.Email;
                cmd.Parameters.Add("v_person_id", OracleDbType.Varchar2).Value = user.Person_id;
                cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = user.Password;
                cmd.Parameters.Add("p_verification_code", OracleDbType.Int32).Value = user.VerificationCode;
                if (pic != null)
                {
                    cmd.Parameters.Add("v_profile_img", OracleDbType.Blob).Value = pic;
                }
                else
                {
                    cmd.Parameters.Add("v_profile_img", OracleDbType.Blob).Value = DBNull.Value;
                }

                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                // Check for the specific error code from the stored procedure
                if (ex.Number == 20002) // Match the error code defined in the stored procedure
                {
                    throw new InvalidOperationException("A user with this email is already registered.", ex);
                }
                else if (ex.Number == 20001) // Error for invalid verification code
                {
                    throw new InvalidOperationException("Invalid or expired verification code.", ex);
                }
                else
                {
                    // Rethrow for any other errors
                    throw;
                }
            }

            conn.Close();
        }
        public List<User> Get_Users()
        {

            List<User> list = new List<User>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC.Get_User";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                User user = new User();
                user.Id = int.Parse(reader["id"].ToString());
                user.FirstName = reader["first_name"].ToString();
                user.LastName = reader["last_name"].ToString();
                user.Email = reader["email"].ToString();
                user.Person_id = reader["person_id"].ToString();
                user.Password = reader["password"].ToString();
                var pictureData = reader["profile_img"] as byte[];

                if (pictureData != null)
                {
                    user.Picture = Convert.ToBase64String(pictureData);
                }
                else
                {
                    user.Picture = null;
                }


                list.Add(user);
            }
            conn.Close();
            return list;


        }
        public void Add_v_code(string toEmail, string verificationCode)
        {

            OracleConnection conn = new OracleConnection
            {
                ConnectionString = ConnStr
            };
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC.Add_v_code";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("v_to_email", OracleDbType.Varchar2).Value = toEmail;
            cmd.Parameters.Add("v_verification_code", OracleDbType.Int32).Value = verificationCode;

            cmd.ExecuteNonQuery();


            conn.Close();
        }
        public bool ValidateVerificationCode(string email, string verificationCode)
        {
            bool isValid = false;

            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "olerning.PKG_NO_CLINIC.Validate_Verification_Code";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = email;
                    cmd.Parameters.Add("v_verification_code", OracleDbType.Varchar2).Value = verificationCode;

                    var isValidParam = new OracleParameter("p_is_valid", OracleDbType.Int32)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(isValidParam);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine($"Oracle Exception: {ex.Message}");
                        throw;
                    }
                    if (isValidParam.Value != DBNull.Value)
                    {
                        OracleDecimal oracleDecimalValue = (OracleDecimal)isValidParam.Value;
                        int result = (int)oracleDecimalValue.ToInt32();

                        if (result == 1)
                        {
                            isValid = true;
                        }
                    }
                    else
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
        public User? authentification(Login loginData)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC.auth_user";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = loginData.Email;
            cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = loginData.Password;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            User? user = null;

            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader["id"] != DBNull.Value ? int.Parse(reader["id"].ToString()) : 0,
                            Role_id = reader["role_id"] != DBNull.Value ? int.Parse(reader["role_id"].ToString()) : 0,
                        };
                    }
                }

                conn.Close();
                return user;

            }
        }
        public User? GetUserById(string id)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC.Get_user_byid";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("v_id",OracleDbType.Int32).Value=id;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


            OracleDataReader reader = cmd.ExecuteReader();
            User? user = null;

            if (reader.Read())
            {
                var pictureData = reader["profile_img"] as byte[];


                user = new User();
                user.Id = int.Parse(reader["id"].ToString());
                user.FirstName = reader["first_name"].ToString();
                user.LastName = reader["last_name"].ToString();
                user.Email = reader["email"].ToString();
                user.Person_id = reader["person_id"].ToString();
                if (pictureData != null)
                {
                    user.Picture = Convert.ToBase64String(pictureData);
                }
                else
                {
                    user.Picture = null; // Explicitly set to null or a default value
                }
            }
            conn.Close();
            return user;


        }

    }
}

using clinic.models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace clinic.packpages
{ 
    public interface IPKG_DOCTOR
    {
        List<Doctor> Get_doctor();
        List<Doctor> Get_doctro_bycat(string id);
        Doctor? GetDoctorById(string id);
        List<Doctor> Get_doctro_byusername(string username);
        List<Doctor> Get_doctro_by_category_name(string category_name);
        void Delete_doctor_by_id(int id);


    }
    public class PKG_DOCTOR : PKG_BASE, IPKG_DOCTOR
    {
        public PKG_DOCTOR(IConfiguration configuration) : base(configuration) { }
        public List<Doctor> Get_doctor()
        {
            List<Doctor> list = new List<Doctor>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC_DOCTOR.Get_doctor";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    var pictureData = reader["profile_img"] as byte[];

                    Doctor doctor = new Doctor();
                    doctor.Id = int.Parse(reader["id"].ToString());
                    doctor.FirstName = reader["first_name"].ToString();
                    doctor.LastName = reader["last_name"].ToString();
                    doctor.Email = reader["email"].ToString();
                    doctor.Category_name = reader["category_name"].ToString();
                    if (pictureData != null)
                    {
                        doctor.Picture = Convert.ToBase64String(pictureData);
                    }
                    else
                    {
                        doctor.Picture = null; // Explicitly set to null or a default value
                    }
                    doctor.Doctor_review = int.TryParse(reader["doctor_review"]?.ToString(), out int review) ? review : 0;



                    list.Add(doctor);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;

        }

        public List<Doctor> Get_doctro_bycat(string id)
        {
            List<Doctor> list = new List<Doctor>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC_DOCTOR.Get_doctors_bycat";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("v_category_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {

                var pictureData = reader["profile_img"] as byte[];

                Doctor doctor = new Doctor();
                doctor.Id = int.Parse(reader["id"].ToString());
                doctor.FirstName = reader["first_name"].ToString();
                doctor.LastName = reader["last_name"].ToString();
                doctor.Email = reader["email"].ToString();
                doctor.Category_name = reader["category_name"].ToString();
                if (pictureData != null)
                {
                    doctor.Picture = Convert.ToBase64String(pictureData);
                }
                else
                {
                    doctor.Picture = null; // Explicitly set to null or a default value
                }
                doctor.Doctor_review = int.TryParse(reader["doctor_review"]?.ToString(), out int review) ? review : 0;



                list.Add(doctor);


            }
            return list;
        }
        public Doctor? GetDoctorById(string id)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC_DOCTOR.Get_Doctor_byid";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


            OracleDataReader reader = cmd.ExecuteReader();
            Doctor? doctor = null;

            if (reader.Read())
            {
                var pictureData = reader["profile_img"] as byte[];


                doctor = new Doctor();
                doctor.Id = int.Parse(reader["id"].ToString());
                doctor.FirstName = reader["first_name"].ToString();
                doctor.LastName = reader["last_name"].ToString();
                doctor.Email = reader["email"].ToString();
                doctor.Person_id = reader["person_id"].ToString();
                if (pictureData != null)
                {
                    doctor.Picture = Convert.ToBase64String(pictureData);
                }
                else
                {
                    doctor.Picture = null; // Explicitly set to null or a default value
                }
            }
            conn.Close();
            return doctor;


        }
        public List<Doctor> Get_doctro_byusername(string username)
        {
            List<Doctor> list = new List<Doctor>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC_DOCTOR.Get_Doctor_By_Username";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {

                var pictureData = reader["profile_img"] as byte[];

                Doctor doctor = new Doctor();
                doctor.Id = int.Parse(reader["id"].ToString());
                doctor.FirstName = reader["first_name"].ToString();
                doctor.LastName = reader["last_name"].ToString();
                doctor.Email = reader["email"].ToString();
                doctor.Category_name = reader["category_name"].ToString();
                if (pictureData != null)
                {
                    doctor.Picture = Convert.ToBase64String(pictureData);
                }
                else
                {
                    doctor.Picture = null; // Explicitly set to null or a default value
                }
                doctor.Doctor_review = int.TryParse(reader["doctor_review"]?.ToString(), out int review) ? review : 0;



                list.Add(doctor);


            }
            return list;
        }
        public List<Doctor> Get_doctro_by_category_name(string category_name)
        {
            List<Doctor> list = new List<Doctor>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC_DOCTOR.Get_Doctors_By_Category";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("v_category_name)", OracleDbType.Varchar2).Value = category_name;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {

                var pictureData = reader["profile_img"] as byte[];

                Doctor doctor = new Doctor();
                doctor.Id = int.Parse(reader["id"].ToString());
                doctor.FirstName = reader["first_name"].ToString();
                doctor.LastName = reader["last_name"].ToString();
                doctor.Email = reader["email"].ToString();
                doctor.Category_name = reader["category_name"].ToString();
                if (pictureData != null)
                {
                    doctor.Picture = Convert.ToBase64String(pictureData);
                }
                else
                {
                    doctor.Picture = null; // Explicitly set to null or a default value
                }
                doctor.Doctor_review = int.TryParse(reader["doctor_review"]?.ToString(), out int review) ? review : 0;



                list.Add(doctor);


            }
            return list;
        }
        public void Delete_doctor_by_id(int id)
        {
            OracleConnection conn = new OracleConnection
            {
                ConnectionString = ConnStr
            };
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC_DOCTOR.Delete_Doctor_by_id";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;


            cmd.ExecuteNonQuery();


            conn.Close();
        }
    }


}
           

 
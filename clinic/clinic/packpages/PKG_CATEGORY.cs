using clinic.models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace clinic.packpages
{
    public interface IPKG_CATEGORY
    {
        List<Category> Get_Category();
    }
    public class PKG_CATEGORY : PKG_BASE, IPKG_CATEGORY
    {
        public PKG_CATEGORY(IConfiguration configuration) : base(configuration) { }
        public List<Category> Get_Category()
        {
            List<Category> list = new List<Category>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_NO_CLINIC_CATEGORY.Get_cat";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Category category = new Category();
                category.Id = int.Parse(reader["id"].ToString());
                category.Category_name = reader["category_name"].ToString();
                category.Category_count = int.Parse(reader["category_count"].ToString());
                list.Add(category);
            }
            conn.Close();
            return list;
        }
    }
    
}

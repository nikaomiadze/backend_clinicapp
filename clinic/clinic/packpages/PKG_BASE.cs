namespace clinic.packpages
{
    public class PKG_BASE
    {
        string connStr;
        IConfiguration config;
        public PKG_BASE(IConfiguration config)
        {
            this.config = config;
            connStr = this.config.GetConnectionString("OracleConnStr");
        }
        protected string ConnStr
        {
            get { return connStr; }
        }

    }
}

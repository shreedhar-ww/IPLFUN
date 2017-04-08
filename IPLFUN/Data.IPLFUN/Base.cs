using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IPLFUN
{
    public class DALBase
    {
        public string _connectionString = string.Empty;

        public string connectionString { get; private set; }
        public DALBase()
        {
            // connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["IPLFun"].ConnectionString;
            connectionString = @"Data Source=DESKTOP-FOFRT0J\SQLEXPRESS;Initial Catalog=IPL;Integrated Security=SSPI;";
        }
      
    }
}

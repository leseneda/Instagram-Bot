using System.Data;
using System.Data.SqlClient;

namespace AES.UpGram.Repository
{
    public class SqlRepository
    {
        public IDbConnection Connect()
        {
            return new SqlConnection("Data Source=localHost;Initial Catalog=UpSocial;Persist Security Info=True;User ID=sa;Password=xd2r37da;");
        }
    }
}

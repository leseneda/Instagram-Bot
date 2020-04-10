using System.Data;
using System.Data.SqlClient;

namespace MeConecta.Gram.Infra.Data.Repository
{
    public class SqlRepository
    {
        public IDbConnection Connect()
        {
            return new SqlConnection("Data Source=localHost;Initial Catalog=UpSocial;Persist Security Info=True;User ID=sa;Password=xd2r37da;");
        }
    }
}

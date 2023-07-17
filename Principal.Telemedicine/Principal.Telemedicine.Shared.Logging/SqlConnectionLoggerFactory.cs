using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Logging;
public  class SqlConnectionLoggerFactory
{
    private readonly string connectionString;
    private SqlConnection sqlConnection;

    public SqlConnectionLoggerFactory(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        if (sqlConnection == null)
        {
            sqlConnection = new SqlConnection(connectionString);
        }

        return sqlConnection;
    }
}

using Microsoft.Data.SqlClient;
namespace Candidates.Service
{
	public class SqlconnectionFactory
	{
		private readonly string _connectionString;
		public SqlconnectionFactory(string connectionString)
		{
			_connectionString = connectionString;
		}
		public SqlConnection Create() 
		{
			return new SqlConnection(_connectionString);
			
		}
	}
}

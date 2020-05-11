using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ImportProcessor.Data.Common {
	public interface IDataRepository {

		IEnumerable<T> Query<T>( string sql, object parameters );

		T ExecuteScalar<T>( string sql, object parameters );

		void Execute( string sql, object parameters );

		void Run( Action<MySqlConnection> action );

	}
}

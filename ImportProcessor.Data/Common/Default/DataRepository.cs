using System;
using System.Collections.Generic;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Serilog;

namespace ImportProcessor.Data.Common.Default {

	public class DataRepository : IDataRepository {

		private readonly string m_connectionString;

		public DataRepository( string connectionString ) {
			m_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

			if( !connectionString.ToLower().Replace( " ", string.Empty ).Contains( "allowuservariables=true" ) ) {
				throw new ArgumentException( "'allow user variables=true;' is  option in connection string." );
			}

#if DEBUG
			// override connection settings for dev machine
			switch( $"{Environment.UserDomainName}_{Environment.UserName}" ) {
				case "EDX0-OPETS_alex":
				m_connectionString = "server=localhost;database=tasly;user=alex;password=1234;Allow User Variables=True;";
				break;
			}
#endif
		}

		IEnumerable<T> IDataRepository.Query<T>( string sql, object parameters ) {
			IEnumerable<T> items = null;
			Log.Logger.Debug(
				$@"SQL: IDataRepository.Query
				sql:{sql}
				parameters: {JsonConvert.SerializeObject( parameters )}"
			);

			Run( connection => { items = connection.Query<T>( sql, parameters ); } );
			return items;
		}

		T IDataRepository.ExecuteScalar<T>( string sql, object parameters ) {
			Log.Logger.Debug(
				$@"SQL: IDataRepository.ExecuteScalar 
				sql:{sql}
				parameters: {JsonConvert.SerializeObject( parameters )}"
			);

			T id = default;
			Run( connection => { id = connection.ExecuteScalar<T>( sql, parameters ); } );
			return id;
		}

		void IDataRepository.Execute( string sql, object parameters ) {
			Log.Logger.Debug(
				$@"SQL: IDataRepository.Execute 
				sql:{sql}
				parameters: {JsonConvert.SerializeObject( parameters )}"
			);

			Run( connection => { connection.Execute( sql, parameters ); } );
		}

		void IDataRepository.Run( Action<MySqlConnection> action ) {
			Log.Logger.Debug( @"SQL: IDataRepository.Run" );

			Run( action );
		}

		private void Run( Action<MySqlConnection> action ) {
			using( var connection = new MySqlConnection( m_connectionString ) ) {
				try {

					connection.Open();
					action( connection );

				} catch( Exception ex ) {

					Log.Logger.Error( ex, "SQL: Error occured while running sql command." );
					throw;
				} finally {

					connection.Close();
				}
			}
		}

	}
}

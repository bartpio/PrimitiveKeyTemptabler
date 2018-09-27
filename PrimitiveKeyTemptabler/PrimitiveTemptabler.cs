using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

using Dapper;

namespace PrimitiveKeyTemptabler
{
    /// <summary>
    /// Primitive Temptabler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PrimitiveTemptabler<T> where T : struct
    {
        /// <summary>
        /// access to tbl passed
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// qualified temptable name
        /// </summary>
        private string QualifiedTable { get; }

        /// <summary>
        /// access to colname
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// quoted col name
        /// </summary>
        private string QuotedCol { get; }

        /// <summary>
        /// keep conn
        /// </summary>
        private readonly DbConnection _conn;

        /// <summary>
        /// make reader
        /// </summary>
        /// <param name="enu"></param>
        /// <returns></returns>
        private IDataReader MakeReader(IEnumerable<T> enu)
        {
            if (typeof(T) == typeof(int))
            {
                return new Int32DataReader((IEnumerable<int>)enu, ColumnName);
            }
            else if (typeof(T) == typeof(long))
            {
                return new Int64DataReader((IEnumerable<long>)enu, ColumnName);
            }
            else if (typeof(T) == typeof(Guid))
            {
                return new GuidDataReader((IEnumerable<Guid>)enu, ColumnName);
            }
            else
            {
                throw new InvalidOperationException($"Not expecting to see type <T> {typeof(T).Name}");
            }
        }

        /// <summary>
        /// cons
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tempTableName"></param>
        /// <param name="columnName"></param>
        /// <param name="trans"></param>
        /// <param name="clusterit">if true, let's create a clustered primary key</param>
        public PrimitiveTemptabler(DbConnection conn, string tempTableName, string columnName = "id", IDbTransaction trans = null, bool clusterit = true)
        {
            if (conn == null)
            {
                throw new ArgumentNullException("conn");
            }
            if (String.IsNullOrEmpty(tempTableName))
            {
                throw new ArgumentNullException("tempTableName");
            }
            if (String.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException("columnName");
            }

            TableName = tempTableName;
            ColumnName = columnName;

            _conn = conn;
            QualifiedTable = conn.ExecuteScalar<string>("select 'tempdb.' + quotename(@tempTableName);", new { tempTableName }, trans);
            QuotedCol = conn.ExecuteScalar<string>("select quotename(@columnName);", new { columnName }, trans);

            using (var tempReader = MakeReader(new T[0] { }))
            {
                var sqltype = tempReader.GetDataTypeName(0);
                var pkspec = clusterit ? "primary key clustered" : String.Empty;
                conn.Execute($"create table {QualifiedTable} ({QuotedCol} {sqltype} not null {pkspec});");
            }
        }

        /// <summary>
        /// Insert some keys
        /// </summary>
        /// <param name="keys"></param>
        public void InsertKeys(IEnumerable<T> keys)
        {
            using (var bcp = new SqlBulkCopy((SqlConnection)_conn, SqlBulkCopyOptions.TableLock, null))
            {
                bcp.DestinationTableName = TableName;
                bcp.EnableStreaming = true;
                var sbcm = new SqlBulkCopyColumnMapping(0, 0)
                {
                    DestinationColumn = ColumnName,
                    SourceColumn = ColumnName,
                    DestinationOrdinal = 0,
                    SourceOrdinal = 0
                };

                bcp.ColumnMappings.Add(sbcm);
                using (var sdr = MakeReader(keys))
                {
                    bcp.WriteToServer(sdr);
                }
            }
        }

        /// <summary>
        /// truncate the temptable
        /// </summary>
        public void Truncate()
        {
            _conn.Execute($"truncate table {QualifiedTable};");
        }

        /// <summary>
        /// drop the temptable; of course no futher operations possible afterwards
        /// </summary>
        public void Drop()
        {
            _conn.Execute($"drop table {QualifiedTable};");
        }
    }
}

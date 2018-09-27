using System;
using System.Collections.Generic;
using System.Data;

namespace PrimitiveKeyTemptabler.Internals
{
    /// <summary>
    /// Abstract base; use the impl provided in root namespace
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PrimitiveDataReader<T> : IDataReader where T : struct
    {
        /// <summary>
        /// enumerator plucked from enumerable
        /// </summary>
        private IEnumerator<T> Enumerator { get; }

        /// <summary>
        /// The Current Value from the plucked enumerator
        /// </summary>
        private T Current => Enumerator.Current;

        /// <summary>
        /// my type for easier diags
        /// </summary>
        protected string MyType => typeof(T).Name;

        /// <summary>
        /// Actually just one column
        /// </summary>
        private string ColumnName { get; }

        /// <summary>
        /// cons, given enumerable
        /// </summary>
        /// <param name="enumerable"></param>
        internal PrimitiveDataReader(IEnumerable<T> enumerable, string columnName)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }
            if (String.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException("columnName");
            }

            Enumerator = enumerable.GetEnumerator();
            ColumnName = columnName;
        }

        /// <summary>
        /// assert that ordinal is right (zero)
        /// </summary>
        /// <param name="ordinal"></param>
        private void AssertOrdinal(int ordinal)
        {
            if (ordinal != 0)
            {
                throw new ArgumentOutOfRangeException("ordinal");
            }
        }

        /// <summary>
        /// assert that ordinal is zero, and read current
        /// </summary>
        /// <param name="ordinal">ordinal, which must be zero</param>
        /// <returns>val @ current enumerator position</returns>
        protected T AssertAndReadCurrent(int ordinal)
        {
            AssertOrdinal(ordinal);             
            return Current;
        }

        /// <summary>
        /// assert that name is correct, and read current
        /// </summary>
        /// <param name="name"></param>
        /// <returns>val @ current enumerator position</returns>
        private T AssertAndReadCurrent(string name)
        {
            if (!ColumnName.Equals(name))
            {
                throw new ArgumentOutOfRangeException("name");
            }
            else
            {
                return Current;
            }
        }

        /// <summary>
        /// read by ordinal
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public object this[int i] => AssertAndReadCurrent(i);

        /// <summary>
        /// read by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name] => AssertAndReadCurrent(name);

        /// <summary>
        /// depth is always zero (no nesting here)
        /// </summary>
        public int Depth => 0;

        /// <summary>
        /// Are we closed AKA disposed?
        /// </summary>
        public bool IsClosed => _disposed;

        /// <summary>
        /// this is a read statement so we're returning -1 as per
        /// https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.recordsaffected?view=netframework-4.7.2
        /// </summary>
        public int RecordsAffected => -1;

        /// <summary>
        /// We are always dealing with one field exactly
        /// </summary>
        public int FieldCount => 1;

        /// <summary>
        /// disposed?
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Close it
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Disposal disposes the enumerator
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Enumerator?.Dispose();
            }
        }

        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldOffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldoffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get data type name
        /// </summary>
        /// <param name="i"></param>
        /// <returns>int or bigint</returns>
        public string GetDataTypeName(int i)
        {
            AssertOrdinal(i);

            switch(typeof(T).Name)
            {
                case nameof(Int32):
                    return "int";
                case nameof(Int64):
                    return "bigint";
                case nameof(Guid):
                    return "varbinary(8)";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pull field type
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Type GetFieldType(int i)
        {
            AssertOrdinal(i);
            return typeof(T);
        }

        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pull guid
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public abstract Guid GetGuid(int i);


        /// <summary>
        /// Not avail
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pull int32
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public abstract int GetInt32(int i);

        /// <summary>
        /// Pull long
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public abstract long GetInt64(int i);
        
        /// <summary>
        /// Pull name
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetName(int i)
        {
            AssertOrdinal(i);
            return ColumnName;
        }

        /// <summary>
        /// Pull ordinal
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetOrdinal(string name)
        {
            if (ColumnName.Equals(name))
            {
                return 0;
            }
            else
            {
                throw new ArgumentOutOfRangeException("name");
            }
        }

        /// <summary>
        /// Schematable not currently available
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException("GetSchemaTable is not implemented in this edition of the library");
        }

        /// <summary>
        /// Get stringform
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetString(int i)
        {
            return AssertAndReadCurrent(i).ToString();
        }

        /// <summary>
        /// read objform
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public object GetValue(int i)
        {
            return AssertAndReadCurrent(i);
        }

        /// <summary>
        /// pull values
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public int GetValues(object[] values)
        {
            values[0] = Current;
            return 1; //One value read.
        }

        /// <summary>
        /// Can't possibly be dbnull.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool IsDBNull(int i)
        {
            return false;
        }

        /// <summary>
        /// for tracking end; rather an edge case
        /// </summary>
        private bool _eof;

        /// <summary>
        /// advance to next result; not applicable
        /// </summary>
        /// <returns>always false</returns>
        public bool NextResult()
        {
            _eof = true; //way past end.
            return false; //no, nothing here.
        }

        /// <summary>
        /// Try reading from the enumerator.
        /// </summary>
        /// <returns>True if there was data; otherwise false</returns>
        public bool Read()
        {
            if (!_eof)
            {
                return Enumerator.MoveNext();
            }
            else
            {
                return false;  //Rare case.
            }
        }
    }
}

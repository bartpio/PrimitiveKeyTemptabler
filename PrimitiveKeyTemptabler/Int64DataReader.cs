using System;
using System.Collections.Generic;
using System.Text;

using PrimitiveKeyTemptabler.Internals;

namespace PrimitiveKeyTemptabler
{
    /// <summary>
    /// reader impl specifically for Int64
    /// </summary>
    public sealed class Int64DataReader : PrimitiveDataReader<Int64>
    {
        /// <summary>
        /// Cons
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="columnName"></param>
        public Int64DataReader(IEnumerable<Int64> enumerable, string columnName = "id")
            : base(enumerable, columnName)
        {
        }

        /// <summary>
        /// No support
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override Guid GetGuid(int i)
        {
            throw new NotSupportedException($"We can only read {MyType}");
        }

        /// <summary>
        /// Pull typed!
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override int GetInt32(int i)
        {
            throw new NotSupportedException($"We can only read {MyType}");
        }

        /// <summary>
        /// No support
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override long GetInt64(int i)
        {
            return AssertAndReadCurrent(i);
        }
    }
}

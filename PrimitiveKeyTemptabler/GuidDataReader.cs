using System;
using System.Collections.Generic;
using System.Text;

using PrimitiveKeyTemptabler.Internals;

namespace PrimitiveKeyTemptabler
{
    /// <summary>
    /// reader impl specifically for Guid
    /// </summary>
    public sealed class GuidDataReader : PrimitiveDataReader<Guid>
    {
        /// <summary>
        /// Cons
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="columnName"></param>
        public GuidDataReader(IEnumerable<Guid> enumerable, string columnName = "id")
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
            return AssertAndReadCurrent(i);
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
            throw new NotSupportedException($"We can only read {MyType}");
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimitiveKeyTemptabler;
using System;
using System.Linq;

namespace PrimitiveKeyTemptabler.Tests
{
    [TestClass]
    public class PrimitiveDataReaderTests
    {
        /// <summary>
        /// testout int
        /// </summary>
        [TestMethod]
        public void EnuTestInt()
        {
            var ins = Enumerable.Range(0, 5000).ToArray();
            using (var reader = new Int32DataReader(ins))
            {
                bool flag;
                int val;

                for (var idx = 0; idx < ins.Length; idx++)
                {
                    (flag, val) = (reader.Read(), reader.GetInt32(0));
                    Assert.IsTrue(flag);
                    Assert.AreEqual(idx, val);
                }

                Assert.IsFalse(reader.Read(), "When done, read should be false");
            }
        }

        /// <summary>
        /// testout long
        /// </summary>
        [TestMethod]
        public void EnuTestLong()
        {
            var ins = Enumerable.Range(0, 5000).Select(x => (long)x).ToArray();
            using (var reader = new Int64DataReader(ins))
            {
                bool flag;
                long val;

                for (var idx = 0; idx < ins.Length; idx++)
                {
                    (flag, val) = (reader.Read(), reader.GetInt64(0));
                    Assert.IsTrue(flag);
                    Assert.AreEqual(idx, val);
                }

                Assert.IsFalse(reader.Read(), "When done, read should be false");
            }
        }

        /// <summary>
        /// Testout guid
        /// </summary>
        [TestMethod]
        public void EnuTestGuid()
        {
            var ins = Enumerable.Range(0, 5000).Select(x => Guid.NewGuid()).ToArray();
            using (var reader = new GuidDataReader(ins))
            {
                bool flag;
                Guid val;

                for (var idx = 0; idx < ins.Length; idx++)
                {
                    (flag, val) = (reader.Read(), reader.GetGuid(0));
                    Assert.IsTrue(flag);
                    Assert.AreEqual(ins[idx], val);
                }

                Assert.IsFalse(reader.Read(), "When done, read should be false");
            }
        }

        /// <summary>
        /// test oddballs
        /// </summary>
        [TestMethod]
        public void EnuTestLongOddballs()
        {
            var ins = Enumerable.Range(0, 5000).Select(x => (long)x).ToArray();
            var reader = new Int64DataReader(ins);
            Assert.IsFalse(reader.NextResult());
            Assert.IsFalse(reader.IsClosed);
            Assert.AreEqual(-1, reader.RecordsAffected);
            reader.Dispose();
            Assert.IsTrue(reader.IsClosed);
        }

        /// <summary>
        /// Test empty enu edgecase
        /// </summary>
        [TestMethod]
        public void EnuTestLongEmpty()
        {
            var ins = new long[0] { };
            using (var reader = new Int64DataReader(ins))
            {
                Assert.IsFalse(reader.IsClosed);
                Assert.IsFalse(reader.Read());
                Assert.IsFalse(reader.NextResult());
            }
        }
    }
}

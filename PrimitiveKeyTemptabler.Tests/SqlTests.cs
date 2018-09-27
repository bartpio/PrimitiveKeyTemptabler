using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Diagnostics;

namespace PrimitiveKeyTemptabler.Tests
{
    /// <summary>
    /// very crude sql based tests requiring a SQL Server db to exist
    /// </summary>
    [TestClass]
    public class SqlTests
    {
        /// <summary>
        /// connstring
        /// requires sometests db to exist on localhost
        /// </summary>
        public string Connstring
        {
            get
            {
                var scsb = new SqlConnectionStringBuilder
                {
                    DataSource = "localhost",
                    IntegratedSecurity = true,
                    InitialCatalog = "sometests"
                };

                return scsb.ToString();
            }
        }

        /// <summary>
        /// bulk ins test
        /// </summary>
        [TestMethod]
        public void Bulktest()
        {
            var somethings = Enumerable.Range(0, 5000);
            var somethings2 = Enumerable.Range(0, 50);
            var somethings3 = Enumerable.Range(0, 50000);

            using (var conn = new SqlConnection(Connstring))
            {
                conn.Open();
                var tt = new PrimitiveTemptabler<int>(conn, "#tt");
                {
                    tt.InsertKeys(somethings);
                }

                var sw = Stopwatch.StartNew();
                var tt2 = new PrimitiveTemptabler<int>(conn, "#tt2", "id", null, false);
                {
                    //sw.Restart();
                    tt2.InsertKeys(somethings2);
                }
                sw.Stop();
                Debug.Print(sw.Elapsed.TotalMilliseconds.ToString());

                //now try a glob
                var ttg = new PrimitiveTemptabler<int>(conn, "##ttg", "id", null, false);
                {
                    //sw.Restart();
                    ttg.InsertKeys(somethings3);
                }

                Debug.Print("Fin.");
            }
        }
    }
}


using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using RDFSharp.Model;
using RDFSharp.Query;

namespace RDFSharp.Test.Query.Mirella.Algebra.Filters
{
    [TestClass]
    public class RDFExistsFilterTest
    {

        public RDFVariable x;
        public RDFVariable y;
        public RDFResource knows;
        public RDFExistsFilter filter;

        [TestInitialize]
        public void Init()
        {
            x = new RDFVariable("x");
            y = new RDFVariable("y");
            knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            filter = new(new RDFPattern(x, knows, y));
        }

        #region RDFExistsFilter

        [TestMethod]
        public void ShouldThrowExceptionOnRDFExistsFilterBecauseFilterNull()
             => Assert.ThrowsException<RDFQueryException>(() => new RDFExistsFilter(null));

        [TestMethod]
        public void ShouldThrowExceptionOnRDFExistsFilterBecauseFilterIsWrong()
        {
            RDFPlainLiteral literal_x = new("x");
            RDFPlainLiteral literal_y = new("y");
            //Act and Assert
            //legalább az egyiknek változónak kell lennie!
            Assert.ThrowsException<RDFQueryException>(() => new RDFExistsFilter(new RDFPattern(literal_x, knows, literal_y)));
        }

        [TestMethod]
        public void RDFExistsFilterOK()
        {           
            var pattern = new RDFPattern(x, knows, y);
            RDFExistsFilter filter_pattern = new RDFExistsFilter(pattern);
            Assert.AreEqual<RDFPattern>(pattern, filter_pattern.Pattern);
            Assert.IsTrue(filter.IsEvaluable);
        }

        [TestMethod]
        public void RDFExistsFilterToString()
        {
            //Arrange
            var pattern = new RDFPattern(x, knows, y);

            //Act
            RDFExistsFilter n = new RDFExistsFilter(pattern);
            string msg = n.ToString();

            //Assert
            Assert.AreEqual<string>("FILTER ( EXISTS { ?X <http://purl.org/dc/elements/1.1/knows> ?Y } )", msg);
        }

        [TestMethod]
        public void RDFExistsFilterApplyFilterContains()
        {
            //Arrange      
            var pattern = new RDFPattern(x, knows, y);
            RDFExistsFilter n = new RDFExistsFilter(pattern);

            DataTable table = new DataTable();
            //táblázat oszlopai
            table.Columns.Add("?A", typeof(string));
            table.Columns.Add("?B", typeof(string));
            //táblázat sorai
            DataRow row = table.NewRow();
            row["?A"] = null;
            row["?B"] = new RDFPlainLiteral("hello", "en-US").ToString();
            table.Rows.Add(row);
            table.AcceptChanges();

            n.PatternResults = table;

            //Act and Assert
            //A megadott oszlopainkat nem tartalmazza a pattern, tehát diszjunkt halmazt alkotnak, ezért az új sort hozzáadjuk, az eredmény true
            Assert.IsTrue(n.ApplyFilter(row, false));
            //A második paraméter negálja az előzőt, tehát ennek false-nak kell lennie
            Assert.IsFalse(n.ApplyFilter(row, true));
        }


        [TestMethod]
        public void RDFExistsFilterApplyFilterNotDisjointAndComparedISFalse()
        {
            //Arrange         
            var pattern = new RDFPattern(x, knows, y);
            RDFExistsFilter n = new RDFExistsFilter(pattern);

            DataTable table = new DataTable();
            //táblázat oszlopai
            table.Columns.Add("?A", typeof(string));
            table.Columns.Add("?B", typeof(string));
            //táblázat oszlopai
            DataRow row = table.NewRow();
            row["?A"] = null;
            row["?B"] = new RDFPlainLiteral("hello", "en-US").ToString();
            table.Rows.Add(row);
            table.AcceptChanges();
            
            n.PatternResults = table;

            DataTable table2 = new DataTable();
            table2.Columns.Add("?X", typeof(string));
            DataRow row2 = table2.NewRow();
            row2["?X"] = new RDFPlainLiteral("bonjour", "fr-FR").ToString();
            table2.Rows.Add(row2);
            table2.AcceptChanges();

            //Assert
            //A megadott oszlop és a pattern nem diszjunkt halmaz, és az Alany az Állítmány és a Tárgy is különbözik, ezért nem tarthatjuk meg a sort.
            Assert.IsFalse(n.ApplyFilter(row2, false));
            //A második paraméter negálja az előzőt, tehát ennek false-nak kell lennie
            Assert.IsTrue(n.ApplyFilter(row2, true));
        }

        [TestMethod]
        public void RDFExistsFilterApplyFilterNotDisjointButSubjectComparedIsTrue()
        {
            //Arrange         
            var pattern = new RDFPattern(x, knows, y);
            RDFExistsFilter n = new RDFExistsFilter(pattern);

            DataTable table = new DataTable();
            //táblázat oszlopai
            table.Columns.Add("?X", typeof(string));
            table.Columns.Add("?B", typeof(string));
            //táblázat oszlopai
            DataRow row = table.NewRow();
            row["?X"] = new RDFPlainLiteral("hello", "en-US").ToString(); 
            row["?B"] = null;
            table.Rows.Add(row);
            table.AcceptChanges();

            n.PatternResults = table;

            DataTable table2 = new DataTable();
            table2.Columns.Add("?X", typeof(string));
            table2.Columns.Add("?B", typeof(string));
            DataRow row2 = table2.NewRow();
            row2["?X"] = new RDFPlainLiteral("hello", "en-US").ToString();
            row2["?B"] = new RDFPlainLiteral("bonjour", "fr-FR").ToString();
            table2.Rows.Add(row2);
            table2.AcceptChanges();

            //Assert
            //A megadott oszlop és a pattern nem diszjunkt halmaz, de az Alany a sorban és a pattern-ben megegyezik, ezért itt megtartjuk.
            Assert.IsTrue(n.ApplyFilter(row2, false));
            //A második paraméter negálja az előzőt, tehát ennek false-nak kell lennie
            Assert.IsFalse(n.ApplyFilter(row2, true));
        }

        [TestMethod]
        public void RDFExistsFilterApplyFilterNotDisjointButPredicateComparedIsTrue()
        {
            //Arrange         
            var pattern = new RDFPattern(x, knows, y);
            RDFExistsFilter n = new RDFExistsFilter(pattern);
          
            DataTable table = new DataTable();
            //táblázat oszlopai
            table.Columns.Add(knows.ToString(), typeof(string));
            table.Columns.Add("?B", typeof(string));
            //táblázat oszlopai
            DataRow row = table.NewRow();
            row[knows.ToString()] = new RDFPlainLiteral("hello", "en-US").ToString();
            row["?B"] = null;
            table.Rows.Add(row);
            table.AcceptChanges();

            n.PatternResults = table;

            DataTable table2 = new DataTable();
            table2.Columns.Add("http://purl.org/dc/elements/1.1/knows", typeof(string));
            table2.Columns.Add("?B", typeof(string));
            DataRow row2 = table2.NewRow();
            row2["http://purl.org/dc/elements/1.1/knows"] = new RDFPlainLiteral("hello", "en-US").ToString();
            row2["?B"] = new RDFPlainLiteral("bonjour", "fr-FR").ToString();
            table2.Rows.Add(row2);
            table2.AcceptChanges();

            //Assert
            //A megadott oszlop és a pattern nem diszjunkt halmaz, de az Állítmány a sorban és a pattern-ben megegyezik, ezért itt megtartjuk.
            Assert.IsTrue(n.ApplyFilter(row2, false));
            //A második paraméter negálja az előzőt, tehát ennek false-nak kell lennie
            Assert.IsFalse(n.ApplyFilter(row2, true));
        }

        [TestMethod]
        public void RDFExistsFilterApplyFilterNotDisjointButObjectComparedIsTrue()
        {
            //Arrange         
            var pattern = new RDFPattern(x, knows, y);
            RDFExistsFilter n = new RDFExistsFilter(pattern);

            DataTable table = new DataTable();
            //táblázat oszlopai
            table.Columns.Add("?Y", typeof(string));
            table.Columns.Add("?B", typeof(string));
            //táblázat oszlopai
            DataRow row = table.NewRow();
            row["?Y"] = new RDFPlainLiteral("hello", "en-US").ToString();
            row["?B"] = null;
            table.Rows.Add(row);
            table.AcceptChanges();

            n.PatternResults = table;

            DataTable table2 = new DataTable();
            table2.Columns.Add("?Y", typeof(string));
            table2.Columns.Add("?B", typeof(string));
            DataRow row2 = table2.NewRow();
            row2["?Y"] = new RDFPlainLiteral("hello", "en-US").ToString();
            row2["?B"] = new RDFPlainLiteral("bonjour", "fr-FR").ToString();
            table2.Rows.Add(row2);
            table2.AcceptChanges();

            //Assert
            //A megadott oszlop és a pattern nem diszjunkt halmaz, de a Tárgy a sorban és a pattern-ben megegyezik, ezért itt megtartjuk.
            Assert.IsTrue(n.ApplyFilter(row2, false));
            //A második paraméter negálja az előzőt, tehát ennek false-nak kell lennie
            Assert.IsFalse(n.ApplyFilter(row2, true));
        }

        #endregion

        #region RDFNotExistsFilter

        [TestMethod]
        public void RDFNotExistsFilterOK()
        {       
            //Arrange
            var pattern = new RDFPattern(x, knows, y);
            //Act
            RDFNotExistsFilter f = new RDFNotExistsFilter(pattern);
            //Assert
            Assert.AreEqual<RDFPattern>(pattern, f.Pattern);
            Assert.IsTrue(filter.IsEvaluable);
        }

        [TestMethod]
        public void RDFNotExistsFilterToString()
        {
            //Arrange         
            var pattern = new RDFPattern(x, knows, y);

            //Act
            RDFNotExistsFilter n = new RDFNotExistsFilter(pattern);
            string msg = n.ToString();

            //Assert
            Assert.AreEqual<string>("FILTER ( NOT EXISTS { ?X <http://purl.org/dc/elements/1.1/knows> ?Y } )", msg);
        }

        [TestMethod]
        public void RDFNotExistsFilterApplyFilterContains()
        {
            //Arrange
            var pattern = new RDFPattern(x, knows, y);
            RDFNotExistsFilter n = new RDFNotExistsFilter(pattern);

            DataTable table = new DataTable();
            table.Columns.Add("?A", typeof(string));
            table.Columns.Add("?B", typeof(string));
            DataRow row = table.NewRow();
            row["?A"] = null;
            row["?B"] = new RDFPlainLiteral("hello", "en-US").ToString();
            table.Rows.Add(row);
            table.AcceptChanges();

            n.PatternResults = table;

            //Assert

            Assert.IsTrue(n.ApplyFilter(row, true));
            Assert.IsFalse(n.ApplyFilter(row, false));
        }
        #endregion
    }
}

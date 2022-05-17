
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
        #region RDFExistsFilter
        [TestMethod]
        public void ShouldThrowExceptionOnRDFExistsFilterBecauseFilterNull()
             => Assert.ThrowsException<RDFQueryException>(() => new RDFExistsFilter(null));

        [TestMethod]
        public void ShouldThrowExceptionOnRDFExistsFilterBecauseFilterIsWrong()
        {
            RDFPlainLiteral x = new RDFPlainLiteral("x");
            RDFPlainLiteral y = new RDFPlainLiteral("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            
            Assert.ThrowsException<RDFQueryException>(() => new RDFExistsFilter(new RDFPattern(x, knows, y)));
        }

        [TestMethod]
        public void RDFExistsFilterOK()
        {
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            var pattern = new RDFPattern(x, knows, y);
            RDFExistsFilter filter = new RDFExistsFilter(pattern);
            Assert.AreEqual<RDFPattern>(pattern, filter.Pattern);
            Assert.IsTrue(filter.IsEvaluable);
        }

        [TestMethod]
        public void RDFExistsFilterToString()
        {
            //Arrange
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
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
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            var pattern = new RDFPattern(x, knows, y);
            RDFExistsFilter n = new RDFExistsFilter(pattern);

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
            Assert.IsTrue(n.ApplyFilter(row, false));
        }

        [TestMethod]
        public void RDFExistsFilterApplyFilterNotContains()
        {
            //Arrange
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            var pattern = new RDFPattern(x, knows, y);
            RDFExistsFilter n = new RDFExistsFilter(pattern);

            DataTable table = new DataTable();
            table.Columns.Add("?A", typeof(string));
            table.Columns.Add("?B", typeof(string));
            DataRow row = table.NewRow();
            row["?A"] = null;
            row["?B"] = new RDFPlainLiteral("hello", "en-US").ToString();
            table.Rows.Add(row);
            table.AcceptChanges();

            n.PatternResults = table; 
            
            DataTable table2 = new DataTable();
            table.Columns.Add("?C", typeof(string));
            table.Columns.Add("?D", typeof(string));
            DataRow row2 = table.NewRow();
            row2["?C"] = null;
            row2["?D"] = new RDFPlainLiteral("bonjour", "fr-FR").ToString();
            table.Rows.Add(row2);
            table.AcceptChanges();

            //Assert
            Assert.IsTrue(n.ApplyFilter(row2, false));
            Assert.IsFalse(n.ApplyFilter(row2, true));
        }
        #endregion

        #region RDFNotExistsFilter
       
        [TestMethod]
        public void RDFNotExistsFilterOK()
        {
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            var pattern = new RDFPattern(x, knows, y);
            RDFNotExistsFilter filter = new RDFNotExistsFilter(pattern);
            Assert.AreEqual<RDFPattern>(pattern, filter.Pattern);
            Assert.IsTrue(filter.IsEvaluable);
        }

        [TestMethod]
        public void RDFNotExistsFilterToString()
        {
            //Arrange
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
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
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            var pattern = new RDFPattern(x, knows, y);
            RDFExistsFilter n = new RDFExistsFilter(pattern);

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
            Assert.IsTrue(n.ApplyFilter(row, false));
        }

        [TestMethod]
        public void RDFNotExistsFilterApplyFilterNotContains()
        {
            //Arrange
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
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

            DataTable table2 = new DataTable();
            table.Columns.Add("?C", typeof(string));
            table.Columns.Add("?D", typeof(string));
            DataRow row2 = table.NewRow();
            row2["?C"] = null;
            row2["?D"] = new RDFPlainLiteral("bonjour", "fr-FR").ToString();
            table.Rows.Add(row2);
            table.AcceptChanges();

            //Assert
            Assert.IsTrue(n.ApplyFilter(row2, true));
            Assert.IsFalse(n.ApplyFilter(row2, false));
        }
        #endregion
    }
}

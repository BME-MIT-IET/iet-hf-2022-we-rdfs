
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using RDFSharp.Model;
using RDFSharp.Query;

namespace RDFSharp.Test.Query.Mirella.Algebra.Filters
{
    [TestClass]
    public class RDFBooleanFilterTest
    {
        public RDFVariable x;
        public RDFVariable y;
        public RDFResource knows;
        public RDFExistsFilter filter_xy;


        public void InitVariables()
        {
            x = new RDFVariable("x");
            y = new RDFVariable("y");
            knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            filter_xy = new(new RDFPattern(x, knows, y));
        }

        #region RDFBooleanAndFilterTest
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanAndFilterBecauseLeftFilterNull()
              => Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanAndFilter(null, new RDFBoundFilter(new RDFVariable("?VAR"))));
        
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanAndFilterBecauseLeftFilterIsWrong()
        {
            //Arrange  
            InitVariables();

            //Act and Assert
            Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanAndFilter(filter_xy, new RDFBoundFilter(new RDFVariable("?VAR"))));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanAndFilterBecauseRightFilterNull()
              => Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanAndFilter(new RDFBoundFilter(new RDFVariable("?VAR")),null));
        [TestMethod]

        public void ShouldThrowExceptionOnRDFBooleanAndFilterBecauseRightFilterIsWrong()
        {
            //Arrange
            InitVariables();

            //Act and Assert
            Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanAndFilter(new RDFBoundFilter(new RDFVariable("?VAR")), filter_xy));
        }

        [TestMethod]
        public void RDFBooleanAndFiltersAreOK()
        {   
            //Arrange
            var filter = new RDFBoundFilter(new RDFVariable("?VAR"));

            //Act
            RDFBooleanAndFilter n=  new RDFBooleanAndFilter(filter, filter);

            //Assert
            Assert.AreEqual<RDFFilter>(filter, n.LeftFilter);
            Assert.AreEqual<RDFFilter>(filter, n.RightFilter);
        }

        [TestMethod]
        public void RDFBooleanAndFilterToStringOK()
        {
            //Arrange
            var filter = new RDFBoundFilter(new RDFVariable("?VAR"));
       
            //Act
            RDFBooleanAndFilter n = new RDFBooleanAndFilter(filter, filter);
            string msg = n.ToString();

            //Assert
            Assert.AreEqual<string>("FILTER ( ( BOUND(?VAR) ) && ( BOUND(?VAR) ) )",msg);
        }

        [TestMethod]
        public void ApplyToBoolenAndTest()
        {
            //Arrange
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
            var filter = new RDFBoundFilter(new RDFVariable("?VAR"));
            RDFBooleanAndFilter n = new RDFBooleanAndFilter(filter, filter);
            //Act and assert
            Assert.IsTrue(n.ApplyFilter(row, false));
            Assert.IsFalse(n.ApplyFilter(row, true));
        }

        #endregion

        #region RDFBooleanNotFilterTest
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanNotFilterBecauseFilterNull()
              => Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanNotFilter(null));
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanNotFilterBecauseFilterIsWrong()
        {
            //Arrange
            InitVariables();

            //Act and assert
            Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanNotFilter(filter_xy));
        }

        [TestMethod]
        public void RDFBooleanNotFilterIsOK()
        {
            //Arrange
            var filter = new RDFBoundFilter(new RDFVariable("?VAR"));

            //Act
            var n = new RDFBooleanNotFilter(filter);

            //Assert
            Assert.AreEqual<RDFFilter>(filter, n.Filter);
        }

        [TestMethod]
        public void RDFBooleanNotFilterToStringOK()
        {
            //Arrange
            var filter = new RDFBoundFilter(new RDFVariable("?VAR"));

            //Act
            RDFBooleanNotFilter n = new RDFBooleanNotFilter(filter);
            string msg = n.ToString();

            //Assert
            Assert.AreEqual<string>("FILTER ( !( BOUND(?VAR) ) )", msg);
        }

        [TestMethod]
        public void ApplyToBooleanNotTest()
        {
            //Arrange
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
            RDFBooleanNotFilter n = new RDFBooleanNotFilter(new RDFBoundFilter(new RDFVariable("?VAR")));
            //Act and assert
            Assert.IsTrue(n.ApplyFilter(row, false));
            Assert.IsFalse(n.ApplyFilter(row, true));
        }
        #endregion

        #region RDFBooleanOrFilterTest
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanOrFilterBecauseLeftFilterNull()
              => Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanOrFilter(null, new RDFBoundFilter(new RDFVariable("?VAR"))));
        
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanOrFilterBecauseLeftFilterIsWrong()
        {
            //Arrange
            InitVariables();

            //Act and Assert
            Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanOrFilter(filter_xy, new RDFBoundFilter(new RDFVariable("?VAR"))));
        }
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanOrFilterBecauseRightFilterNull()
              => Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanOrFilter(new RDFBoundFilter(new RDFVariable("?VAR")), null));
      
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanOrFilterBecauseRightFilterIsWrong()
        {
            //Arrange
            InitVariables();

            //Act and Assert
            Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanOrFilter(new RDFBoundFilter(new RDFVariable("?VAR")), filter_xy));
        }

        [TestMethod]
        public void RDFBooleanOrFilterIsOK()
        {
            //Arrange
            var filter = new RDFBoundFilter(new RDFVariable("?VAR"));

            //Act
            RDFBooleanOrFilter n = new RDFBooleanOrFilter(filter, filter);

            //Assert
            Assert.AreEqual<RDFFilter>(filter, n.LeftFilter);
            Assert.AreEqual<RDFFilter>(filter, n.RightFilter);
        }

        [TestMethod]
        public void RDFBooleanOrFilterToStringOK()
        {
            //Arrange
            var filter = new RDFBoundFilter(new RDFVariable("?VAR"));

            //Act
            RDFBooleanOrFilter n = new RDFBooleanOrFilter(filter, filter);
            string msg = n.ToString();

            //Assert
            Assert.AreEqual<string>("FILTER ( ( BOUND(?VAR) ) || ( BOUND(?VAR) ) )", msg);
        }

        [TestMethod]
        public void ApplyToBooleanOrTest()
        {
            //Arrange
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
            var filter = new RDFBoundFilter(new RDFVariable("?VAR"));
            RDFBooleanOrFilter n = new RDFBooleanOrFilter(filter, filter);
            //Act and assert
            Assert.IsTrue(n.ApplyFilter(row, false));
            Assert.IsFalse(n.ApplyFilter(row, true));
        }
        #endregion
    }
}
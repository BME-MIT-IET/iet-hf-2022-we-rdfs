
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
        #region RDFBooleanAndFilterTest
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanAndFilterBecauseLeftFilterNull()
              => Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanAndFilter(null, new RDFBoundFilter(new RDFVariable("?VAR"))));
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanAndFilterBecauseLeftFilterIsWrong()
        {
            RDFVariable x =new RDFVariable("x");
            RDFVariable y =new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI+"knows");            
            RDFExistsFilter filter = new RDFExistsFilter(new RDFPattern(x,knows,y));
            Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanAndFilter(filter, new RDFBoundFilter(new RDFVariable("?VAR"))));
        }
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanAndFilterBecauseRightFilterNull()
              => Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanAndFilter(new RDFBoundFilter(new RDFVariable("?VAR")),null));
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanAndFilterBecauseRightFilterIsWrong()
        {
            //Arrange
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            RDFExistsFilter filter = new RDFExistsFilter(new RDFPattern(x, knows, y));
            //Act and Assert
            Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanAndFilter(new RDFBoundFilter(new RDFVariable("?VAR")), filter));
        }

        [TestMethod]
        public void RDFBooleanAndFilterIsOK()
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
        #endregion

        #region RDFBooleanNotFilterTest
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanNotFilterBecauseFilterNull()
              => Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanNotFilter(null));
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanNotFilterBecauseFilterIsWrong()
        {
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            RDFExistsFilter filter = new RDFExistsFilter(new RDFPattern(x, knows, y));
            Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanNotFilter(filter));
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
        #endregion

        #region RDFBooleanOrFilterTest
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanOrFilterBecauseLeftFilterNull()
              => Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanOrFilter(null, new RDFBoundFilter(new RDFVariable("?VAR"))));
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanOrFilterBecauseLeftFilterIsWrong()
        {
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            RDFExistsFilter filter = new RDFExistsFilter(new RDFPattern(x, knows, y));
            Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanOrFilter(filter, new RDFBoundFilter(new RDFVariable("?VAR"))));
        }
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanOrFilterBecauseRightFilterNull()
              => Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanOrFilter(new RDFBoundFilter(new RDFVariable("?VAR")), null));
        [TestMethod]
        public void ShouldThrowExceptionOnRDFBooleanOrFilterBecauseRightFilterIsWrong()
        {
            //Arrange
            RDFVariable x = new RDFVariable("x");
            RDFVariable y = new RDFVariable("y");
            RDFResource knows = new RDFResource(RDFVocabulary.DC.BASE_URI + "knows");
            RDFExistsFilter filter = new RDFExistsFilter(new RDFPattern(x, knows, y));
            //Act and Assert
            Assert.ThrowsException<RDFQueryException>(() => new RDFBooleanOrFilter(new RDFBoundFilter(new RDFVariable("?VAR")), filter));
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
        #endregion
    }
}
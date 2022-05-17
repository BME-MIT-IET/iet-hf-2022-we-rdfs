
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using RDFSharp.Model;
using RDFSharp.Query;

namespace RDFSharp.Test.Query.Mirella.Algebra.Queries
{
    [TestClass]
    public class RDFAskQueryTest
    {
        RDFAskQuery q;

        [TestInitialize]
        public void init()
        {
            q = new RDFAskQuery();
        }

        [TestMethod]
        public void AddPatternGroupTest() 
        {
            init();
            var pattern =new RDFPatternGroup("PG1");
            q.AddPatternGroup(pattern);

            Assert.IsTrue(q.QueryMembers.Contains(pattern));
        }

        [TestMethod]
        public void AddPrefixTest()
        {
            init();
            var namesp = new RDFNamespace("ex","http://ex.org/%22");
            q.AddPrefix(namesp);

            Assert.IsTrue(q.Prefixes.Contains(namesp));
        }

        [TestMethod]
        public void AddSubQueryTest()
        {
            init();
            var sq = new RDFSelectQuery();
            q.AddSubQuery(sq);

            Assert.IsTrue(q.QueryMembers.Contains(sq));
        }

        [TestMethod]
        public void ApplyToGraphEmptyTest()
        {
            init();
            RDFGraph g = null;
            var r =  q.ApplyToGraph(g);

            Assert.IsFalse(r.AskResult);
        }

        
    }
}

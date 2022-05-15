using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using RDFSharp.Model;
using RDFSharp.Query;
using System;

namespace RDFSharp.Test.Query.Mirella
{
    [TestClass]
    public class RDFOperationPrinterTest
    {
        [TestMethod]
        public void PrintInsertTest()
        {
            RDFResource marco = new RDFResource("http://ex.org/marco%22");
            RDFResource loves = new RDFResource("http://ex.org/loves%22");
            RDFResource valentina = new RDFResource("http://ex.org/valentina%22");

            RDFInsertDataOperation insertOperation = new RDFInsertDataOperation()
                            .AddPrefix(new RDFNamespace("ex", "http://ex.org/%22"))
                            .AddInsertTemplate(new RDFPattern(marco, loves, valentina));

            var msg = RDFOperationPrinter.PrintInsertDataOperation(insertOperation);
            
            Assert.IsTrue(msg.Contains("http://ex.org/marco"));
            Assert.IsTrue(msg.Contains("http://ex.org/loves"));
            Assert.IsTrue(msg.Contains("http://ex.org/valentina"));
            Assert.IsTrue(msg.Contains("INSERT DATA"));
            Assert.IsTrue(msg.Contains("PREFIX ex:"));
            Assert.IsTrue(msg.Contains("http://ex.org/"));
        }

        [TestMethod]
        public void PrintInsertWhereTest()
        {
            RDFResource marco = new RDFResource("http://ex.org/marco%22");
            RDFResource loves = new RDFResource("http://ex.org/loves%22");
            RDFResource isLovedBy = new RDFResource("http://ex.org/isLovedBy%22");
            RDFVariable loved = new RDFVariable("?loved");

            RDFInsertWhereOperation insertOperation = new
                                RDFInsertWhereOperation()
                                 .AddPrefix(new RDFNamespace("ex", "http://ex.org/%22"))
                                 .AddInsertTemplate(new RDFPattern(loved, isLovedBy, marco))
                                 .AddPatternGroup(new RDFPatternGroup("PG1")
                                 .AddPattern(new RDFPattern(marco, loves, loved)));

            var msg = RDFOperationPrinter.PrintInsertWhereOperation(insertOperation);

            Assert.IsTrue(msg.Contains("PREFIX ex:"));
            Assert.IsTrue(msg.Contains("http://ex.org/"));
            Assert.IsTrue(msg.Contains("INSERT"));
            Assert.IsTrue(msg.Contains("?LOVED"));
            Assert.IsTrue(msg.Contains("WHERE"));
            Assert.IsTrue(msg.Contains("http://ex.org/marco"));
            Assert.IsTrue(msg.Contains("http://ex.org/isLovedBy"));
            Assert.IsTrue(msg.Contains("http://ex.org/loves"));
        }

        [TestMethod]
        public void PrintDeleteDataOperationTest()
        {
            // Create resources
            RDFResource marco = new RDFResource("http://ex.org/marco%22");
            RDFResource loves = new RDFResource("http://ex.org/loves%22");
            RDFResource valentina = new RDFResource("http://ex.org/valentina%22");
            // Compose operation
            RDFDeleteDataOperation deleteOperation = new
            RDFDeleteDataOperation()
             .AddPrefix(new RDFNamespace("ex", "http://ex.org/%22"))
             .AddDeleteTemplate(new RDFPattern(marco, loves, valentina));

            var msg = RDFOperationPrinter.PrintDeleteDataOperation(deleteOperation);

            Assert.IsTrue(msg.Contains("PREFIX ex:"));
            Assert.IsTrue(msg.Contains("http://ex.org/"));
            Assert.IsTrue(msg.Contains("DELETE DATA"));
            Assert.IsTrue(msg.Contains("http://ex.org/marco"));
            Assert.IsTrue(msg.Contains("http://ex.org/loves"));
            Assert.IsTrue(msg.Contains("http://ex.org/valentina"));
        }

        [TestMethod]
        public void PrintDeleteWhereOperationTest()
        {
            // Create resources
            RDFResource marco = new RDFResource("http://ex.org/marco%22");
            RDFResource loves = new RDFResource("http://ex.org/loves%22");
            RDFResource isLovedBy = new RDFResource("http://ex.org/isLovedBy%22");
            RDFVariable loved = new RDFVariable("?loved");
            // Compose operation
            RDFDeleteWhereOperation deleteOperation = new
            RDFDeleteWhereOperation()
             .AddPrefix(new RDFNamespace("ex", "http://ex.org/%22"))
             .AddDeleteTemplate(new RDFPattern(loved, isLovedBy, marco))
             .AddPatternGroup(new RDFPatternGroup("PG1")
             .AddPattern(new RDFPattern(marco, loves, loved)));

            var msg = RDFOperationPrinter.PrintDeleteWhereOperation(deleteOperation);

            Assert.IsTrue(msg.Contains("PREFIX ex:"));
            Assert.IsTrue(msg.Contains("http://ex.org/"));
            Assert.IsTrue(msg.Contains("DELETE"));
            Assert.IsTrue(msg.Contains("?LOVED"));
            Assert.IsTrue(msg.Contains("WHERE"));
            Assert.IsTrue(msg.Contains("http://ex.org/marco"));
            Assert.IsTrue(msg.Contains("http://ex.org/isLovedBy"));
            Assert.IsTrue(msg.Contains("http://ex.org/loves"));
        }

        [TestMethod]
        public void PrintDeleteInsertWhereOperationTest()
        {
            // Create resources
            RDFResource marco = new RDFResource("http://ex.org/marco%22");
            RDFResource loves = new RDFResource("http://ex.org/loves%22");
            RDFResource isLovedBy = new RDFResource("http://ex.org/isLovedBy%22");
            RDFResource isBelovedBy = new RDFResource("http://ex.org/isBelovedBy%22");
            RDFVariable loved = new RDFVariable("?loved");
            // Compose operation
            RDFDeleteInsertWhereOperation delInsOperation = new
            RDFDeleteInsertWhereOperation()
             .AddPrefix(new RDFNamespace("ex", "http://ex.org/%22"))
             .AddDeleteTemplate(new RDFPattern(loved, isLovedBy, marco))
             .AddInsertTemplate(new RDFPattern(loved, isBelovedBy, marco))
             .AddPatternGroup(new RDFPatternGroup("PG1")
             .AddPattern(new RDFPattern(marco, loves, loved)));

            var msg = RDFOperationPrinter.PrintDeleteInsertWhereOperation(delInsOperation);
            
            Assert.IsTrue(msg.Contains("PREFIX ex:"));
            Assert.IsTrue(msg.Contains("http://ex.org/"));
            Assert.IsTrue(msg.Contains("DELETE"));
            Assert.IsTrue(msg.Contains("INSERT"));
            Assert.IsTrue(msg.Contains("?LOVED"));
            Assert.IsTrue(msg.Contains("WHERE"));
            Assert.IsTrue(msg.Contains("http://ex.org/marco"));
            Assert.IsTrue(msg.Contains("http://ex.org/isLovedBy"));
            Assert.IsTrue(msg.Contains("http://ex.org/isBelovedBy"));
            Assert.IsTrue(msg.Contains("http://ex.org/loves"));
        }

        [TestMethod]
        public void PrintLoadOperationTest()
        {
            // Compose operation
            RDFLoadOperation loadOperation = new RDFLoadOperation(new
            Uri("http://ex.org/graph%22"))
             .Silent()
             .SetContext(new Uri("http://test.org/test/%22"));
            var msg = RDFOperationPrinter.PrintLoadOperation(loadOperation);
           
            Assert.IsTrue(msg.Contains("LOAD SILENT"));
            Assert.IsTrue(msg.Contains("INTO GRAPH"));
            Assert.IsTrue(msg.Contains("http://ex.org/graph"));
            Assert.IsTrue(msg.Contains("http://test.org/test/"));
        }

        [TestMethod]
        public void PrintClearOperationTest()
        {
            RDFClearOperation clearOperation = new RDFClearOperation(new
                Uri("http://test.org/test/%22")).Silent();

            var msg = RDFOperationPrinter.PrintClearOperation(clearOperation);

            Assert.IsTrue(msg.Contains("CLEAR SILENT GRAPH"));
            Assert.IsTrue(msg.Contains("http://test.org/test/"));
        }
    }
}

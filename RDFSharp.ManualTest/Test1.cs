using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;
using RDFSharp.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDFSharp.ManualTest
{
    public class Test1
    {
        public static void Run(RDFGraph graph)
        {
            Console.WriteLine("======================[Running Test1]======================");

            /* SparQL:
             * -----------------------------------------------------
             * 
             * PREFIX ecrm: <http://erlangen-crm.org/current/>
               SELECT ?s ?v {
                 ?s rdf:type ecrm:E54_Dimension ;
                 ecrm:P90_has_value ?v
               }
             * 
             */

            // Create Variables
            var actor = new RDFVariable("actor");
            var actorType = new RDFResource("http://erlangen-crm.org/current/E39_Actor");

            // Compose Query
            var query1 = new RDFSelectQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("rdf"))
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("ecrm"))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(actor, RDFVocabulary.RDF.TYPE, actorType)))
                .AddProjectionVariable(actor);

            var query1Result = query1.ApplyToGraph(graph).SelectResultsCount;
            Console.WriteLine($"Count of result:{query1Result}");


            // Assert result with expected value
            Assert.IsTrue(query1Result == 1743);
            Console.WriteLine("\t--> Test result: " + (query1Result == 1743 ? "Success" : "Fail"));
            Console.WriteLine("======================[Test1 Done.]========================\n");
        }
    }
}

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
    public class Test2
    {
        public static void Run(RDFGraph graph)
        {
            Console.WriteLine("======================[Running Test2]======================");

            /* SparQL:
             * -----------------------------------------------------
             * 
             *  PREFIX ecrm: <http://erlangen-crm.org/current/>
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
                SELECT ?a ?n{
                ?a rdf:type ecrm:E39_Actor .
                ?a rdfs:label ?n .
                FILTER regex(?n,"giovanni","i")
                }
             * 
             */

            // Create Variables
            var actor = new RDFVariable("actor");
            var actorType = new RDFResource("http://erlangen-crm.org/current/E39_Actor");
            var name=new RDFVariable("name");
            var nameType = new RDFResource("http://www.w3.org/2000/01/rdf-schema#label");

            // Compose Query
            var query2 = new RDFSelectQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("rdf"))
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("ecrm"))
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("rdfs"))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(actor, RDFVocabulary.RDF.TYPE, actorType))
                    .AddPattern(new RDFPattern(actor, RDFVocabulary.RDFS.LABEL,name))
                    .AddFilter(new RDFRegexFilter(name, new System.Text.RegularExpressions.Regex(@"giovanni", System.Text.RegularExpressions.RegexOptions.IgnoreCase))))
                .AddProjectionVariable(actor)
                .AddProjectionVariable(name);
     

            var query2Result = query2.ApplyToGraph(graph).SelectResultsCount;
            Console.WriteLine($"Count of result:{query2Result}");
            

            // Assert result with expected value
            Assert.IsTrue(query2Result == 54);
            Console.WriteLine("\t--> Test result: " + (query2Result == 54 ? "Success" : "Fail"));
            Console.WriteLine("======================[Test2 Done.]========================\n");
        }
    }
}

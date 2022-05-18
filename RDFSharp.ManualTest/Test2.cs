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
        public static void Run()
        {
            // Read RDF Graph from file
            var format = RDFModelEnums.RDFFormats.RdfXml;
            var graph = RDFGraph.FromFile(format, "szepmuveszeti.rdf");

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
     

            var query2Result = query2.ApplyToGraph(graph);
            //query2Result.ToSparqlXmlResult("result2.srq");
            var result2 = query2Result.SelectResultsCount;
            Console.WriteLine(result2);
 
            // Assert result with expected value
            // TODO
        }
    }
}

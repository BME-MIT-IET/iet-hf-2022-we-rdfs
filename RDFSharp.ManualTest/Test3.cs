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
    internal class Test3
    {

        public static void Run(RDFGraph graph)
        {
            Console.WriteLine("======================[Running Test3]======================");

            /* SparQL:
             * -----------------------------------------------------
             * 
             *  PREFIX ecrm: <http://erlangen-crm.org/current/>
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                SELECT ?a ?c ?t {
                ?a rdf:type ecrm:E39_Actor
                ?c ecrm:P11_had_participant ?a ;
                a ecrm:E65_Creation 
                ?t ecrm:P12i_was_present_at ?c ;
                a ecrm:E18_Physical_Thing
                }
             * 
             */

            // Create Variables
            var actor = new RDFVariable("actor");
            var actorType = new RDFResource("http://erlangen-crm.org/current/E39_Actor");
            var creation = new RDFVariable("creation");
            var creationType = new RDFResource("http://erlangen-crm.org/current/E65_Creation");
            var thing = new RDFVariable("thing");
            var thingType = new RDFResource("http://erlangen-crm.org/current/E18_Physical_Thing");

            // Create Patterns
            var hadParticipant = new RDFResource("http://erlangen-crm.org/current/P11_had_participant");
            var wasPresentAt = new RDFResource("http://erlangen-crm.org/current/P12i_was_present_at"); 

            // Compose Query
            var query3 = new RDFSelectQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("rdf"))
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("ecrm"))            
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(creation,hadParticipant,actor))
                    .AddPattern(new RDFPattern(thing,wasPresentAt,creation))) 
                .AddProjectionVariable(actor)
                .AddProjectionVariable(creation)
                .AddProjectionVariable(thing);


            var query3Result = query3.ApplyToGraph(graph).SelectResultsCount;
            Console.WriteLine($"Count of result:{query3Result}");


            // Assert result with expected value
            Assert.IsTrue(query3Result == 4409);
            Console.WriteLine("\t--> Test result: " + (query3Result == 4409 ? "Success" : "Fail"));
            Console.WriteLine("======================[Test3 Done.]========================\n");
        }
    }
}

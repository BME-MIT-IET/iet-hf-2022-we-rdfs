using RDFSharp.Model;
using RDFSharp.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDFSharp.ManualTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Read RDF Graph from file
            var format = RDFModelEnums.RDFFormats.RdfXml;
            var graph = RDFGraph.FromFile(format, "szepmuveszeti.rdf");
            
            //Tests
            Test1.Run(graph);
            Test2.Run(graph);
            Test3.Run(graph);
            Test4.Run();

            Console.ReadKey();
        }
    }
}

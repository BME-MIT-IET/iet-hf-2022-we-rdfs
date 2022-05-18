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
            /*
             * 1. Teszt:
             *       Bemenet: szepmuveszeti.rdf
             *       Elvárt kimenet: TODO
             *
             *       Lekérdezzük a gyakorlati queryket (3 db kb)
             *       Összehasonlítjuk a kimenetet az elvárt kimenettel.
             *
             */
            // Read RDF Graph from file
            var format = RDFModelEnums.RDFFormats.RdfXml;
            var graph = RDFGraph.FromFile(format, "szepmuveszeti.rdf");
            //Test1.Run();
            //Test2.Run();
            Test3.Run(graph);
            //Test1.Run();

            //Test4.Run();

            Console.ReadKey();
        }
    }
}

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
             * 1,2,3. Teszt:
             *       Bemenet: szepmuveszeti.rdf
             *       Elvárt kimenet: TODO
             *
             *       Lekérdezzük a gyakorlati queryket (3 db kb)
             *       Összehasonlítjuk a kimenetet az elvárt kimenettel.
             *
             */

            /*
             *  4. Teszt:
             *       Felépíteni memóriában egy egyszerű RDF gráfot országok és azok fővárosairól kettő-kettő példa adattal.
             *       Kiírni egy RDF XML fileba a gráfot.
             *       Beolvasni az RDF XML fileból a gráfot memóriába.
             *       Letesztelni, hogy tényleg 2 ország, 2 város létrejött-e query segítségével.
             *       Hozzáadni egy új várost a gráfhoz:
             *       Letesztelni, hogy a hozzáadás után tényleg 2 ország, 3 város létezik-e query segítségével.
             *       Kitörölni egy országot a gráfból.
             *       Letesztelni, hogy a törlés után tényleg 1 ország, 3 város létezik-e query segítségével.
             * 
             */

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

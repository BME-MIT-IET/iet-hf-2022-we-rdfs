using RDFSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDFSharp.NonFunctionalTests
{
    public class LoadModels
    {
        public static void ReadFromXml(int iteration=10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_loading_time = TimeSpan.Zero;

            var format = RDFModelEnums.RDFFormats.RdfXml;

            Console.WriteLine("Loading an xml file");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                var graph = RDFGraph.FromFile(format, "szepmuveszeti.rdf");
                end = DateTime.Now;

                ts = (end - start);
                total_loading_time += ts;
                Console.WriteLine("\t{0}. iteration: loading time is {1} ms",i+1, ts.TotalMilliseconds);
            }

            Console.WriteLine("Total loading time:{0} ms, Average loading time:{1} ms", total_loading_time.TotalMilliseconds, total_loading_time.TotalMilliseconds / iteration);

        }
    }
}

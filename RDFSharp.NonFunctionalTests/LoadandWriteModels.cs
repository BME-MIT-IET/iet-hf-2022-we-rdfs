using RDFSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDFSharp.NonFunctionalTests
{
    public class LoadandWriteModels
    {
        private static string ResultsFileName = "results.txt";
        public static void CreateFiles()
        {
            var graph = RDFGraph.FromFile(RDFModelEnums.RDFFormats.RdfXml, "szepmuveszeti.rdf");

            using FileStream ntriples_file = File.OpenWrite("szepmuveszeti.n3");
            graph.ToStream(RDFModelEnums.RDFFormats.NTriples, ntriples_file);

            using FileStream trix_file = File.OpenWrite("szepmuveszeti.trix");
            graph.ToStream(RDFModelEnums.RDFFormats.TriX, trix_file);

            /*using FileStream turtle_file = File.OpenWrite("szepmuveszeti.ttl");
            graph.ToStream(RDFModelEnums.RDFFormats.Turtle, turtle_file);*/
        }


        public static void TestXmlRead(int iteration=10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_loading_time = TimeSpan.Zero;

            var format = RDFModelEnums.RDFFormats.RdfXml;

            using StreamWriter results = new(ResultsFileName);

            results.Write("Loading an xml file\n");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                var graph = RDFGraph.FromFile(format, "szepmuveszeti.rdf");
                end = DateTime.Now;

                ts = (end - start);
                total_loading_time += ts;
                results.WriteLine("\t{0}. iteration: loading time is {1} ms",i+1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total loading time:{0} ms, Average loading time:{1} ms\n", total_loading_time.TotalMilliseconds, total_loading_time.TotalMilliseconds / iteration);

        }

        public static void TestN3Read(int iteration = 10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_loading_time = TimeSpan.Zero;

            var format = RDFModelEnums.RDFFormats.NTriples;

            using StreamWriter results = File.AppendText(ResultsFileName);

            results.Write("Loading an n-triples file\n");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                var graph = RDFGraph.FromFile(format, "szepmuveszeti.n3");
                end = DateTime.Now;

                ts = (end - start);
                total_loading_time += ts;
                results.WriteLine("\t{0}. iteration: loading time is {1} ms", i + 1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total loading time:{0} ms, Average loading time:{1} ms\n", total_loading_time.TotalMilliseconds, total_loading_time.TotalMilliseconds / iteration);
        }

        public static void TestTrixRead(int iteration = 10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_loading_time = TimeSpan.Zero;

            var format = RDFModelEnums.RDFFormats.TriX; 

            using StreamWriter results = File.AppendText(ResultsFileName);

            results.Write("Loading a triX file\n");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                var graph = RDFGraph.FromFile(format, "szepmuveszeti.trix");
                end = DateTime.Now;

                ts = (end - start);
                total_loading_time += ts;
                results.WriteLine("\t{0}. iteration: loading time is {1} ms", i + 1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total loading time:{0} ms, Average loading time:{1} ms\n", total_loading_time.TotalMilliseconds, total_loading_time.TotalMilliseconds / iteration);
        }

        public static void TestTurtle(int iteration = 10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_loading_time = TimeSpan.Zero;

            var format = RDFModelEnums.RDFFormats.Turtle;

            using StreamWriter results = File.AppendText(ResultsFileName);

            results.Write("Loading a turtle file\n");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                var graph = RDFGraph.FromFile(format, "szepmuveszeti.ttl");
                end = DateTime.Now;

                ts = (end - start);
                total_loading_time += ts;
                results.WriteLine("\t{0}. iteration: loading time is {1} ms", i + 1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total loading time:{0} ms, Average loading time:{1} ms\n", total_loading_time.TotalMilliseconds, total_loading_time.TotalMilliseconds / iteration);
        }
    }
}

using RDFSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDFSharp.NonFunctionalTests
{
    /// <summary>
    /// Tests the library's performance loading and writing graph models.
    /// </summary>
    public class LoadandWriteModels
    {
        private static string ResultsFileName = "results.txt";

        private static RDFGraph Graph;

        /// <summary>
        /// Creates files in different formats
        /// </summary>
        public static void CreateFiles()
        {
            Graph = RDFGraph.FromFile(RDFModelEnums.RDFFormats.RdfXml, "szepmuveszeti.rdf");

            using FileStream ntriples_file = File.OpenWrite("szepmuveszeti.n3");
            Graph.ToStream(RDFModelEnums.RDFFormats.NTriples, ntriples_file);

            using FileStream trix_file = File.OpenWrite("szepmuveszeti.trix");
            Graph.ToStream(RDFModelEnums.RDFFormats.TriX, trix_file);

            /* Deserialising turtle files leads to an error so perfomance on them is not tested.
             * using FileStream turtle_file = File.OpenWrite("szepmuveszeti.ttl");
             * Graph.ToStream(RDFModelEnums.RDFFormats.Turtle, turtle_file);
             */
        }

        /// <summary>
        /// Reads an rdf/xml file the given number of times.
        /// Calculates average and total loading time.
        /// </summary>
        /// <param name="iteration">Number of times to read the file</param>
        public static void TestXmlRead(int iteration=10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_loading_time = TimeSpan.Zero;

            var format = RDFModelEnums.RDFFormats.RdfXml;

            using StreamWriter results = new(ResultsFileName);

            results.Write("Loading an rdf/xml file\n");

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

        /// <summary>
        /// Reads an n-triple file the given number of times.
        /// Calculates average and total loading time.
        /// </summary>
        /// <param name="iteration">Number of times to read the file</param>
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

        /// <summary>
        /// Reads a triX file the given number of times.
        /// Calculates average and total loading time.
        /// </summary>
        /// <param name="iteration">Number of times to read the file</param>
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

        /// <summary>
        /// Reads a turtle file the given number of times.
        /// Calculates average and total loading time.
        /// Currently not working.
        /// </summary>
        /// <param name="iteration">Number of times to read the file</param>
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

        /// <summary>
        /// Writes an rdf/xml file the given number of times.
        /// Calculates average and total writing time.
        /// </summary>
        /// <param name="iteration">Number of times to read the file</param>
        public static void TestXmlWrite(int iteration = 10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_writing_time = TimeSpan.Zero;

            var format = RDFModelEnums.RDFFormats.RdfXml;

            using StreamWriter results = File.AppendText(ResultsFileName);

            results.Write("Writing an rdf/xml file\n");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                Graph.ToFile(format, "szepmuveszeti_test.rdf");
                end = DateTime.Now;

                ts = (end - start);
                total_writing_time += ts;
                results.WriteLine("\t{0}. iteration: writing time is {1} ms", i + 1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total writing time:{0} ms, Average writing time:{1} ms\n",
                total_writing_time.TotalMilliseconds, total_writing_time.TotalMilliseconds / iteration);

        }

        /// <summary>
        /// Writes an n-triple file the given number of times.
        /// Calculates average and total writing time.
        /// </summary>
        /// <param name="iteration">Number of times to read the file</param>
        public static void TestN3Write(int iteration = 10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_writing_time = TimeSpan.Zero;

            var format = RDFModelEnums.RDFFormats.NTriples;

            using StreamWriter results = File.AppendText(ResultsFileName);

            results.Write("Writing an n-triple file\n");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                Graph.ToFile(format, "szepmuveszeti_test.n3");
                end = DateTime.Now;

                ts = (end - start);
                total_writing_time += ts;
                results.WriteLine("\t{0}. iteration: writing time is {1} ms", i + 1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total writing time:{0} ms, Average writing time:{1} ms\n",
                total_writing_time.TotalMilliseconds, total_writing_time.TotalMilliseconds / iteration);

        }

        /// <summary>
        /// Writes a triX file the given number of times.
        /// Calculates average and total writing time.
        /// </summary>
        /// <param name="iteration">Number of times to read the file</param>
        public static void TestTrixWrite(int iteration = 10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_writing_time = TimeSpan.Zero;

            var format = RDFModelEnums.RDFFormats.TriX;

            using StreamWriter results = File.AppendText(ResultsFileName);

            results.Write("Writing a TriX file\n");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                Graph.ToFile(format, "szepmuveszeti_test.trix");
                end = DateTime.Now;

                ts = (end - start);
                total_writing_time += ts;
                results.WriteLine("\t{0}. iteration: writing time is {1} ms", i + 1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total writing time:{0} ms, Average writing time:{1} ms\n",
                total_writing_time.TotalMilliseconds, total_writing_time.TotalMilliseconds / iteration);

        }
    }


}

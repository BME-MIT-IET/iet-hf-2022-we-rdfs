﻿using RDFSharp.Model;
using RDFSharp.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RDFSharp.NonFunctionalTests
{
    /// <summary>
    /// Tests the library's performance when executing querries
    /// </summary>
    public class Queries
    {
        private static string ResultsFileName = "query_results.txt";

        private static RDFGraph Graph;

        public static void Run()
        {
            Graph = RDFGraph.FromFile(RDFModelEnums.RDFFormats.RdfXml, "szepmuveszeti.rdf");
            QueryAllActors(0);
            QueryActorsByName(0);
            QueryCreatorsandCreations(0);
            QueryRembrandtEtchings();
        }

        public static void QueryAllActors(int iteration=10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_time = TimeSpan.Zero;

            // Create Variables
            var actor = new RDFVariable("actor");
            var actorType = new RDFResource("http://erlangen-crm.org/current/E39_Actor");

            // Compose Query
            var query = new RDFSelectQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("rdf"))
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("ecrm"))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(actor, RDFVocabulary.RDF.TYPE, actorType)))
                .AddProjectionVariable(actor);

            // Writing to results file
            using StreamWriter results = new(ResultsFileName);

            results.Write("Querying all actors\n");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                var queryResult = query.ApplyToGraph(Graph);
                end = DateTime.Now;

                ts = (end - start);
                total_time += ts;
                results.WriteLine("\t{0}. iteration: query time is {1} ms", i + 1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total query time:{0} ms, Average query time:{1} ms\n", 
                total_time.TotalMilliseconds, total_time.TotalMilliseconds / iteration);
        }

        public static void QueryActorsByName(int iteration = 10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_time = TimeSpan.Zero;

            // Create Variables
            var actor = new RDFVariable("actor");
            var name = new RDFVariable("name");
            var actorType = new RDFResource("http://erlangen-crm.org/current/E39_Actor");

            // Compose Query
            var query = new RDFSelectQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("rdf"))
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("ecrm"))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(actor, RDFVocabulary.RDF.TYPE, actorType))
                    .AddPattern(new RDFPattern(actor,RDFVocabulary.RDFS.LABEL, name))
                    .AddFilter(new RDFRegexFilter(name,new Regex(@"Giovanni", RegexOptions.IgnoreCase) )))
                .AddProjectionVariable(actor);

            // Writing to results file
            using StreamWriter results = File.AppendText(ResultsFileName);

            results.Write("Querying actors named Giovanni\n");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                var queryResult = query.ApplyToGraph(Graph);
                end = DateTime.Now;

                ts = (end - start);
                total_time += ts;
                results.WriteLine("\t{0}. iteration: query time is {1} ms", i + 1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total query time:{0} ms, Average query time:{1} ms\n",
                total_time.TotalMilliseconds, total_time.TotalMilliseconds / iteration);
        }

        public static void QueryCreatorsandCreations(int iteration = 10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_time = TimeSpan.Zero;

            // Create Variables
            var actor = new RDFVariable("actor");
            var creation = new RDFVariable("creation");
            var thing = new RDFVariable("thing");

            var actorType = new RDFResource("http://erlangen-crm.org/current/E39_Actor");

            var hadParticipant = new RDFResource("http://erlangen-crm.org/current/P11_had_participant");
            var wasPresentAt = new RDFResource("http://erlangen-crm.org/current/P12i_was_present_at");

            // Compose Query
            var query = new RDFSelectQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("rdf"))
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("ecrm"))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(actor, RDFVocabulary.RDF.TYPE, actorType))
                    .AddPattern(new RDFPattern(creation, hadParticipant, actor))
                    .AddPattern(new RDFPattern(thing, wasPresentAt, creation)))
                .AddProjectionVariable(actor)
                .AddProjectionVariable(creation)
                .AddProjectionVariable(thing);

            // Writing to results file
            using StreamWriter results = File.AppendText(ResultsFileName);

            results.Write("Querying actors and their creations\n");
           
            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                var queryResult = query.ApplyToGraph(Graph);
                end = DateTime.Now;

                ts = (end - start);
                total_time += ts;
                results.WriteLine("\t{0}. iteration: query time is {1} ms", i + 1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total query time:{0} ms, Average query time:{1} ms\n",
                total_time.TotalMilliseconds, total_time.TotalMilliseconds / iteration);
        }

        public static void QueryRembrandtEtchings(int iteration = 10)
        {
            DateTime start;
            DateTime end;
            TimeSpan ts;
            TimeSpan total_time = TimeSpan.Zero;

            // Create Variables
            var actor = new RDFVariable("actor");
            var name = new RDFVariable("name");
            var creation = new RDFVariable("creation");
            var material = new RDFVariable("material");
            var thing = new RDFVariable("thing");

            var actorType = new RDFResource("http://erlangen-crm.org/current/E39_Actor");

            var hadParticipant = new RDFResource("http://erlangen-crm.org/current/P11_had_participant");
            var wasPresentAt = new RDFResource("http://erlangen-crm.org/current/P12i_was_present_at");
            var hasNote = new RDFResource("http://erlangen-crm.org/current/P3_has_note");

            // Compose Query
            var query = new RDFSelectQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("rdf"))
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("ecrm"))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(actor, RDFVocabulary.RDF.TYPE, actorType))
                    .AddPattern(new RDFPattern(actor, RDFVocabulary.RDFS.LABEL, name )) 
                    .AddPattern(new RDFPattern(creation, hadParticipant, actor))
                    .AddPattern(new RDFPattern(creation,hasNote,material))
                    .AddPattern(new RDFPattern(thing, wasPresentAt, creation))
                    .AddFilter(new RDFRegexFilter(name, new Regex(@"Rembrandt",RegexOptions.IgnoreCase)))
                    .AddFilter(new RDFRegexFilter(material, new Regex(@"rézkarc", RegexOptions.IgnoreCase))))
                .AddProjectionVariable(actor)
                .AddProjectionVariable(creation)
                .AddProjectionVariable(thing)
                .AddProjectionVariable(material)
                .AddProjectionVariable(name);

            // Writing to results file
            using StreamWriter results = File.AppendText(ResultsFileName);

            results.Write("Querying actors and their creations\n");

            for (int i = 0; i < iteration; i++)
            {
                start = DateTime.Now;
                var queryResult = query.ApplyToGraph(Graph);
                queryResult.ToSparqlXmlResult("results.srq");
                end = DateTime.Now;

                ts = (end - start);
                total_time += ts;
                results.WriteLine("\t{0}. iteration: query time is {1} ms", i + 1, ts.TotalMilliseconds);
            }

            results.WriteLine("Total query time:{0} ms, Average query time:{1} ms\n",
                total_time.TotalMilliseconds, total_time.TotalMilliseconds / iteration);
        }
    }
}

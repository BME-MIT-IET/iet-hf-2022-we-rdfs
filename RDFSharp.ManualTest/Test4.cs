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
    public class Test4
    {
        public static void Run()
        {
            Console.WriteLine("======================[Running Test4]======================");
            Console.WriteLine("Initializing graph...");

            // Init Predicates
            RDFResource city_name = new RDFResource("http://example.org/name");
            RDFResource country_capital = new RDFResource("http://example.org/capital");
            RDFResource city_population = new RDFResource("http://example.org/population");

            // Init subject resources
            RDFResource type_country = new RDFResource("http://example.org/country");
            RDFResource type_city = new RDFResource("http://example.org/city");

            RDFResource hungary = new RDFResource("https://en.wikipedia.org/wiki/Hungary");
            RDFResource netherlands = new RDFResource("https://en.wikipedia.org/wiki/Netherlands");

            RDFResource budapest = new RDFResource("https://en.wikipedia.org/wiki/Budapest");
            RDFResource amsterdam = new RDFResource("https://en.wikipedia.org/wiki/Amsterdam");

            // Init object resources
            RDFPlainLiteral budapest_name_en = new RDFPlainLiteral("Budapest", "en-US");
            RDFTypedLiteral budapest_population = new RDFTypedLiteral("1723836", RDFModelEnums.RDFDatatypes.XSD_INTEGER);

            RDFPlainLiteral amsterdam_name_en = new RDFPlainLiteral("Amsterdam", "en-US");
            RDFTypedLiteral amsterdam_population = new RDFTypedLiteral("860124", RDFModelEnums.RDFDatatypes.XSD_INTEGER);

            // Init assertions
            List<RDFTriple> tripleList = new List<RDFTriple>();
            tripleList.Add(new RDFTriple(budapest, city_name, budapest_name_en));
            tripleList.Add(new RDFTriple(budapest, city_population, budapest_population));

            tripleList.Add(new RDFTriple(amsterdam, city_name, amsterdam_name_en));
            tripleList.Add(new RDFTriple(amsterdam, city_population, amsterdam_population));

            tripleList.Add(new RDFTriple(hungary, country_capital, budapest));
            tripleList.Add(new RDFTriple(netherlands, country_capital, amsterdam));

            tripleList.Add(new RDFTriple(hungary, RDFVocabulary.RDF.TYPE, type_country));
            tripleList.Add(new RDFTriple(netherlands, RDFVocabulary.RDF.TYPE, type_country));

            tripleList.Add(new RDFTriple(budapest, RDFVocabulary.RDF.TYPE, type_city));
            tripleList.Add(new RDFTriple(amsterdam, RDFVocabulary.RDF.TYPE, type_city));

            // Init graph with 2 country and 2 city.
            RDFGraph rdfGraph_countries_cities = new RDFGraph(tripleList);

            Console.WriteLine("Graph initialized.\n");

            // Write it out to a file
            var format = RDFModelEnums.RDFFormats.RdfXml;
            var fileName = "countries_cities.rdf";

            Console.WriteLine($"Writing it out to \'{fileName}\' ...");
            rdfGraph_countries_cities.ToFile(format, fileName);
            Console.WriteLine("Writing done.\n");

            // Reads it back from the file to a different object
            Console.WriteLine($"Reading it back from \'{fileName}\' ...");
            var graph = RDFGraph.FromFile(format, fileName);
            Console.WriteLine("Reading done.\n");

            // Init the queries to get cities count.
            Console.WriteLine("Initializing query...");

            // --> Create Variables
            var var_city = new RDFVariable("?city");
            var var_country = new RDFVariable("?city");

            // --> Compose Queries
            var query_cities = new RDFSelectQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("rdf"))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(var_city, RDFVocabulary.RDF.TYPE, type_city)))
                .AddProjectionVariable(var_city);

            var query_countries = new RDFSelectQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix("rdf"))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(var_country, RDFVocabulary.RDF.TYPE, type_country)))
                .AddProjectionVariable(var_country);

            Console.WriteLine("Queries initialized.\n");

            // Executing query.
            Console.WriteLine("Executing queries...");
            var count_of_cities_original = query_cities.ApplyToGraph(graph).SelectResultsCount;
            var count_of_countries_original = query_countries.ApplyToGraph(graph).SelectResultsCount;
            Console.WriteLine("Queries executed.\n");

            Console.WriteLine("Testing the graph...");

            Console.WriteLine($"Count of cities: {count_of_cities_original}");
            Console.WriteLine($"Count of countries: {count_of_countries_original}");
            
            Console.WriteLine("\t--> Test result: " + (count_of_cities_original == 2 ? "Success" : "Fail"));
            Console.WriteLine("\t--> Test result: " + (count_of_countries_original == 2 ? "Success" : "Fail"));

            // Adding one more city to the graph without assigning it to a country.
            Console.WriteLine("Initializing and adding a new city to the graph...");

            // --> Initializing a new city.
            Console.WriteLine("Initializing the new city...");

            RDFResource berlin = new RDFResource("https://en.wikipedia.org/wiki/Berlin");
            RDFPlainLiteral berlin_name_en = new RDFPlainLiteral("Berlin", "en-US");
            RDFTypedLiteral berlin_population = new RDFTypedLiteral("3664088", RDFModelEnums.RDFDatatypes.XSD_INTEGER);

            // --> Adding the city to the graph.
            Console.WriteLine("Adding new city to the graph...");

            graph.AddTriple(new RDFTriple(berlin, city_name, berlin_name_en));
            graph.AddTriple(new RDFTriple(berlin, city_population, berlin_population));
            graph.AddTriple(new RDFTriple(berlin, RDFVocabulary.RDF.TYPE, type_city));

            Console.WriteLine("New city initialized and added to the graph.\n");

            // Testing if city count increased by one.
            Console.WriteLine("Testing the graph with the added city...");
            var count_of_cities_add = query_cities.ApplyToGraph(graph).SelectResultsCount;
            var count_of_countries_add = query_countries.ApplyToGraph(graph).SelectResultsCount;

            Console.WriteLine($"\t--> Count of cities: {count_of_cities_add}");
            Console.WriteLine($"\t--> Count of countries: {count_of_countries_add}");

            Console.WriteLine("Test result: " + (count_of_cities_add == count_of_cities_original + 1 ? "Success" : "Fail"));
            Console.WriteLine("Test result: " + (count_of_countries_original == count_of_countries_add ? "Success" : "Fail"));

            Console.WriteLine("Testing done.\n");

            // Removing a country of the graph. (Netherlands)
            Console.WriteLine("Removing a country of the graph (Netherlands)...");
            graph.RemoveTriplesBySubject(netherlands);

            Console.WriteLine("City removed of the graph.\n");

            // Testing if country count decreased by one.
            Console.WriteLine("Testing the graph with the removed country...");
            var count_of_cities_removed = query_cities.ApplyToGraph(graph).SelectResultsCount;
            var count_of_countries_removed = query_countries.ApplyToGraph(graph).SelectResultsCount;

            Console.WriteLine($"\t--> Count of cities: {count_of_cities_removed}");
            Console.WriteLine($"\t--> Count of countries: {count_of_countries_removed}");

            Console.WriteLine("Test result: " + (count_of_cities_removed == count_of_cities_add ? "Success" : "Fail"));
            Console.WriteLine("Test result: " + (count_of_countries_removed == count_of_countries_add - 1 ? "Success" : "Fail"));

            Console.WriteLine("Testing done.");

            Console.WriteLine("======================[Test4 Done.]======================\n");
        }
    }
}

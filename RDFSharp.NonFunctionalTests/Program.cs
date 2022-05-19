using RDFSharp.Model;
using RDFSharp.NonFunctionalTests;
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

            LoadandWriteModels.Run(5);

            Queries.Run();

            Console.WriteLine("All tests are finished.");
            Console.ReadKey();
        }
    }
}
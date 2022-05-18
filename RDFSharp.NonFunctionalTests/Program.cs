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

            /*LoadandWriteModels.CreateFiles();

            LoadandWriteModels.TestXmlRead(3);
            LoadandWriteModels.TestN3Read(3);
            LoadandWriteModels.TestTrixRead(3);
            //LoadandWriteModels.TestTurtle(3);

            Console.WriteLine("Reading done");

            LoadandWriteModels.TestXmlWrite(3);
            LoadandWriteModels.TestN3Write(3);
            LoadandWriteModels.TestTrixWrite(3);

            Console.WriteLine("Writing done");*/

            Queries.Run();

            Console.WriteLine("All tests are finished.");
            Console.ReadKey();
        }
    }
}
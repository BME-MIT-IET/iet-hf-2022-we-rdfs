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

            LoadandWriteModels.CreateFiles();

            LoadandWriteModels.TestXmlRead(3);
            LoadandWriteModels.TestN3Read(3);
            LoadandWriteModels.TestTrixRead(3);


            //LoadandWriteModels.TestTurtle(3);
            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}
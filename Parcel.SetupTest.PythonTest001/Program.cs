using System;
using Parcel.Python;

namespace Parcel.SetupTest.PythonTest001
{
    class Program
    {
        static void Main(string[] args)
        {
            // QuickRunScript("Scripts.BaseTypeTest.py");
            REPLMode();
        }

        private static void REPLMode()
        {
            var engine = new REPLEngine();
            while (true)
            {
                Console.Write(">>> ");
                string input = Console.ReadLine();
                if (input == "exit()") break;
                try
                {
                    engine.Execute(input);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        private static void QuickRunScript(string embeddedResourcePath)
        {
            string content = Helper.ReadResource(embeddedResourcePath);
            GenericEngine.Execute(content);
        }
    }
}
using System;
using Microsoft.Scripting.Hosting;

namespace Parcel.Python
{
    public static class GenericEngine
    {
        public static void Execute(string script)
        {
            var eng = IronPython.Hosting.Python.CreateEngine();
            var scope = eng.CreateScope();
            eng.Execute(script, scope);
            dynamic entry = scope.GetVariable("main");
            System.Console.WriteLine(entry());
        }
    }

    public class REPLEngine
    {
        private ScriptEngine Engine { get; set; }
        private ScriptScope Scope { get; set; }
        public REPLEngine()
        {
            Engine = IronPython.Hosting.Python.CreateEngine();
            Scope = Engine.CreateScope();
        }
        public void Execute(string snippet)
        {
            Engine.Execute(snippet, Scope);
        }
    }
}
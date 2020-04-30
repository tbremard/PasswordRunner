// Password Runner
// Thierry Brémard
// 2020-04-26

using Modules;
using Runner.Interfaces;
using System;
using System.Reflection;

namespace Runner
{
    public static class MyFactory
    {
        public static IPasswordProducer CreatePasswordProducer()
        {
            return new AlphabeticalLowerProducer();
        }

        public static T CreateInstance<T>(string binaryFile, string className, object[]? args)
        {
            var assembly = Assembly.LoadFrom(binaryFile);
            bool ignoreCase = true;
            BindingFlags bindingAttr = BindingFlags.CreateInstance;
            Binder? binder = null;
            var ret = (T)assembly.CreateInstance(className, ignoreCase, bindingAttr, binder, args, null, null);
            Console.WriteLine("Class loaded: " + ret.GetType().FullName);
            return ret;
        }
    }
}

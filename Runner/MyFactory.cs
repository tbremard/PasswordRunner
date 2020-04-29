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

        public static IPasswordProducer CreatePasswordProducerByReflection(string binaryFile, string className)
        {
            IPasswordProducer ret;
            var assembly = Assembly.LoadFrom(binaryFile);
            Console.WriteLine("assembly loaded: "+assembly.FullName);
            ret = (IPasswordProducer)assembly.CreateInstance(className);
            Console.WriteLine("class found:" + ret.GetType().FullName);
            return ret;
        }
    }
}

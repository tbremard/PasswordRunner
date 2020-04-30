﻿using Modules;
using NUnit.Framework;

namespace Runner.UnitTest
{
    public class MyFactoryTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreatePasswordProducer_WhenCalled_ThenValid()
        {

            var ret = MyFactory.CreatePasswordProducer();

            Assert.IsNotNull(ret);
        }

        [Test]
        public void CreatePasswordProducerByReflection_WhenCalled_ThenValid()
        {
            string binaryFile = "Modules.dll";
            string className = "Modules.AlphabeticalLowerProducer";

            var ret = MyFactory.CreatePasswordProducerByReflection(binaryFile, className);

            Assert.IsNotNull(ret);
            Assert.AreEqual("a", ret.CurrentValue);
        }
    }

}